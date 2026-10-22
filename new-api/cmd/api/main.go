// Comando da API: abre o banco, aplica migrations, monta o DI dos 4 domínios
// e serve na 0.0.0.0:PORT — réplica do Program.cs do C#.
package main

import (
	"context"
	"database/sql"
	"errors"
	"log/slog"
	"net/http"
	"os"
	"os/signal"
	"path/filepath"
	"syscall"
	"time"

	"github.com/go-chi/chi/v5"
	goose "github.com/pressly/goose/v3"
	_ "modernc.org/sqlite"

	finndb "github.com/coigo/finn/db"
	aportesAd "github.com/coigo/finn/internal/adapters/aportes"
	aportesHdl "github.com/coigo/finn/internal/handlers/aportes"
	movimentacoesHdl "github.com/coigo/finn/internal/handlers/movimentacoes"
	salariosHdl "github.com/coigo/finn/internal/handlers/salarios"
	saldosHdl "github.com/coigo/finn/internal/handlers/saldos"
	"github.com/coigo/finn/internal/platform/config"
	plataformaMw "github.com/coigo/finn/internal/platform/middleware"
	"github.com/coigo/finn/internal/platform/sqlitex"
	aportesRep "github.com/coigo/finn/internal/repositories/aportes"
	movimentacoesRep "github.com/coigo/finn/internal/repositories/movimentacoes"
	salariosRep "github.com/coigo/finn/internal/repositories/salarios"
	saldosRep "github.com/coigo/finn/internal/repositories/saldos"
	aportesSvc "github.com/coigo/finn/internal/services/aportes"
	movimentacoesSvc "github.com/coigo/finn/internal/services/movimentacoes"
	salariosSvc "github.com/coigo/finn/internal/services/salarios"
	saldosSvc "github.com/coigo/finn/internal/services/saldos"
)

func main() {
	if err := run(); err != nil {
		slog.Error("falha no startup", "erro", err)
		os.Exit(1)
	}
}

func run() error {
	cfg, err := config.Load()
	if err != nil {
		return err
	}

	db, err := sqlitex.Open(cfg.DBPath)
	if err != nil {
		return err
	}
	defer db.Close()

	// Réplica do modo --migrate do C# (init container no K8s): aplica e sai.
	if len(os.Args) > 1 && os.Args[1] == "--migrate" {
		return migrar(db)
	}
	if err := migrar(db); err != nil {
		return err
	}

	// DI: stores (sqlc) -> services -> handlers -> rotas (feat.md §4).
	saldoStore := saldosRep.NewStore(db)
	movStore := movimentacoesRep.NewStore(db)
	salarioStore := salariosRep.NewStore(db)
	aporteStore := aportesRep.NewStore(db)
	prices := aportesAd.NewAtivos(cfg.B3.URL, cfg.B3.Key, cfg.Crypto.URL, cfg.Crypto.Key)

	saldoSvc := saldosSvc.NewService(saldoStore)
	movSvc := movimentacoesSvc.NewService(movStore, saldoSvc)
	salarioSvc := salariosSvc.NewService(salarioStore, saldoSvc, movSvc)
	aporteSvc := aportesSvc.NewService(aporteStore, aporteStore, prices, time.Now)

	// Réplica da pipeline do Program.cs C#:
	// ErrorHandlerMiddleware → UseDefaultFiles/UseStaticFiles → UseCors →
	// UseRouting/MapControllers → MapFallbackToFile("index.html").
	r := chi.NewRouter()
	r.Use(plataformaMw.Recuperar())
	r.Use(plataformaMw.Cors(cfg.WebURL))

	movimentacoesHdl.NewHandler(movSvc, time.Now).Mount(r)
	aportesHdl.NewHandler(aporteSvc).Mount(r)
	saldosHdl.NewHandler(saldoSvc).Mount(r)
	salariosHdl.NewHandler(salarioSvc).Mount(r)

	estatico(r, cfg.WebRoot)

	// Rotas não casadas: como o C# (MapFallbackToFile), tentam index.html;
	// sem SPA, 404 vazio (Kestrel não imprime "404 page not found").
	r.NotFound(func(w http.ResponseWriter, req *http.Request) {
		if arquivoSPAFallback(w, req, cfg.WebRoot) {
			return
		}
		w.WriteHeader(http.StatusNotFound)
	})

	srv := &http.Server{
		Addr:    "0.0.0.0:" + cfg.Port,
		Handler: r,
	}

	ctx, stop := signal.NotifyContext(context.Background(), os.Interrupt, syscall.SIGTERM)
	defer stop()

	erroSrv := make(chan error, 1)
	go func() {
		slog.Info("API escutando", "addr", srv.Addr)
		erroSrv <- srv.ListenAndServe()
	}()

	select {
	case err := <-erroSrv:
		if err != nil && !errors.Is(err, http.ErrServerClosed) {
			return err
		}
		return nil
	case <-ctx.Done():
		slog.Info("encerrando…")
		desliga, cancel := context.WithTimeout(context.Background(), 10*time.Second)
		defer cancel()
		return srv.Shutdown(desliga)
	}
}

func migrar(db *sql.DB) error {
	goose.SetBaseFS(finndb.FS)
	if err := goose.SetDialect("sqlite3"); err != nil {
		return err
	}
	if err := goose.Up(db, "migrations"); err != nil {
		return err
	}
	return nil
}

// estatico replica UseDefaultFiles + UseStaticFiles: serve o wwwroot
// (UseDefaultFiles mapeia / para index.html). Sem diretório, tudo 404 vazio
// (idêntico ao Kestrel com wwwroot ausente).
func estatico(r chi.Router, raiz string) {
	r.Get("/", func(w http.ResponseWriter, req *http.Request) {
		if !arquivoSPAFallback(w, req, raiz) {
			w.WriteHeader(http.StatusNotFound)
		}
	})
	r.Handle("/*", http.HandlerFunc(func(w http.ResponseWriter, req *http.Request) {
		// Arquivo inexistente → fallback do SPA (rotas do front como /saldos).
		if !arquivoSPAFallback(w, req, raiz) {
			w.WriteHeader(http.StatusNotFound)
			return
		}
	}))
}

// arquivoSPAFallback serve wwwroot/index.html (requested before static files:
// o Kestrel primeiro procura o arquivo, depois cai no fallback do SPA);
// false quando nem o index existe — aí quem responde é o 404 vazio.
func arquivoSPAFallback(w http.ResponseWriter, req *http.Request, raiz string) bool {
	indexPath := filepath.Join(raiz, "index.html")
	info, err := os.Stat(indexPath)
	if err != nil || info.IsDir() {
		return false
	}
	f, err := os.Open(indexPath)
	if err != nil {
		return false
	}
	defer f.Close()
	http.ServeContent(w, req, info.Name(), info.ModTime(), f)
	return true
}
