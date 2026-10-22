package aportes

import (
	"context"
	"log/slog"
	"net/http"
	"strings"

	"github.com/go-chi/chi/v5"

	dominio "github.com/coigo/finn/internal/domain/aportes"
	"github.com/coigo/finn/internal/platform/httpx"
	"github.com/coigo/finn/internal/platform/money"
	servico "github.com/coigo/finn/internal/services/aportes"
)

// ServiceAPI é a interface consumida pelo handler (definida pelo consumidor —
// os testes injetam fakes).
type ServiceAPI interface {
	Movimentar(ctx context.Context, input servico.MovimentarInput) (dominio.Aporte, error)
	MovimentarPorIdentificador(ctx context.Context, identificador string, input servico.MovimentarPorIdentificadorInput) (dominio.Aporte, error)
	List(ctx context.Context) ([]dominio.ListDTO, error)
	ListHistorico(ctx context.Context, identificador string) ([]dominio.HistoricoDTO, error)
	Categorias() []dominio.SelectValue
}

type Handler struct {
	svc ServiceAPI
}

func NewHandler(svc ServiceAPI) *Handler { return &Handler{svc: svc} }

// movimentarEcho é o MovimentarAporteDTO C# (identificador normalizado no bind).
type movimentarEcho struct {
	Identificador string         `json:"identificador"`
	Quantidade    money.Money    `json:"quantidade"`
	Preco         money.Money    `json:"preco"`
	Categoria     int64          `json:"categoria"`
	DataCompra    httpx.FlexDate `json:"dataCompra"`
}

func (h *Handler) movimentar(w http.ResponseWriter, r *http.Request) {
	var echo movimentarEcho
	if !httpx.DecodeJSON(w, r, &echo) {
		return
	}
	ident := strings.ToUpper(strings.TrimSpace(echo.Identificador))
	if echo.Categoria < dominio.CategoriaACAO || echo.Categoria > dominio.CategoriaETF {
		slog.Error("categoria de aporte inválida", "valor", echo.Categoria)
		httpx.WriteStatusMessage(w, http.StatusBadRequest, "corpo inválido")
		return
	}
	aporte, err := h.svc.Movimentar(r.Context(), servico.MovimentarInput{
		Identificador: ident,
		Quantidade:    echo.Quantidade,
		Preco:         echo.Preco,
		Categoria:     echo.Categoria,
		DataCompra:    echo.DataCompra.Time,
	})
	if err != nil {
		// Paridade: todos os erros de aportes (ticker, aporte, ativo,
		// quantidade, preço médio) são BusinessError no C# → ErrorHandler
		// default → 500 com a mensagem do erro.
		httpx.Error(w, err)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, aporte)
}

// movPorIdentEcho é o MovimentarAportePorIdentificadorDTO C# (preco opcional;
// tipoStr cru — quem normaliza é o service, como o getter C#).
type movPorIdentEcho struct {
	Quantidade money.Money    `json:"quantidade"`
	Preco      *money.Money   `json:"preco"`
	TipoStr    string         `json:"tipoStr"`
	Data       httpx.FlexDate `json:"data"`
}

func (h *Handler) movimentarPorIdentificador(w http.ResponseWriter, r *http.Request) {
	ident, ok := identificadorDaRota(w, r)
	if !ok {
		return
	}
	var echo movPorIdentEcho
	if !httpx.DecodeJSON(w, r, &echo) {
		return
	}
	aporte, err := h.svc.MovimentarPorIdentificador(r.Context(), ident, servico.MovimentarPorIdentificadorInput{
		Quantidade: echo.Quantidade,
		Preco:      echo.Preco,
		TipoStr:    echo.TipoStr,
		Data:       echo.Data.Time,
	})
	if err != nil {
		// Paridade: BusinessError → 500 com a mensagem.
		httpx.Error(w, err)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, aporte)
}

func (h *Handler) listar(w http.ResponseWriter, r *http.Request) {
	lista, err := h.svc.List(r.Context())
	if err != nil {
		httpx.Error(w, err)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, lista)
}

func (h *Handler) categorias(w http.ResponseWriter, r *http.Request) {
	httpx.WriteJSON(w, http.StatusOK, h.svc.Categorias())
}

func (h *Handler) historico(w http.ResponseWriter, r *http.Request) {
	// identificador CRU da rota (o C# não normaliza aqui).
	ident := chi.URLParam(r, "identificador")
	historico, err := h.svc.ListHistorico(r.Context(), ident)
	if err != nil {
		httpx.Error(w, err)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, historico)
}

func identificadorDaRota(w http.ResponseWriter, r *http.Request) (string, bool) {
	raw := chi.URLParam(r, "identificador")
	if strings.TrimSpace(raw) == "" {
		slog.Error("identificador vazio na rota")
		httpx.WriteStatusMessage(w, http.StatusBadRequest, "corpo inválido")
		return "", false
	}
	return strings.ToUpper(strings.TrimSpace(raw)), true
}
