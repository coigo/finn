// Package testdb fornece um SQLite real para os store_test: banco temporário
// por teste em t.TempDir(), com uma única conexão (modelo single-user do
// SQLite; :memory: com pool cria bancos distintos por conexão) e com as
// migrations reais de db/migrations aplicadas a cada teste.
package testdb

import (
	"database/sql"
	"fmt"
	"os"
	"path/filepath"
	"sort"
	"strings"
	"testing"

	_ "modernc.org/sqlite"
)

// New abre um banco temporário com uma conexão única.
func New(t *testing.T) *sql.DB {
	t.Helper()
	dir := t.TempDir()
	dsn := "file:" + filepath.ToSlash(filepath.Join(dir, "test.sqlite")) + "?_pragma=foreign_keys(1)&_pragma=busy_timeout(1000)&_txlock=immediate"
	db, err := sql.Open("sqlite", dsn)
	if err != nil {
		t.Fatalf("abrir banco de teste: %v", err)
	}
	t.Cleanup(func() { _ = db.Close() })
	db.SetMaxOpenConns(1)
	return db
}

// Migrate aplica as migrations de db/migrations (Fase 4). Enquanto a pasta
// estiver vazia os store tests pulam com Skipf.
func Migrate(t *testing.T, db *sql.DB) {
	t.Helper()
	root, err := filepath.Abs(filepath.Join("..", "..", "db", "migrations"))
	if err != nil {
		t.Fatalf("resolver db/migrations: %v", err)
	}
	if _, statErr := os.Stat(root); statErr != nil {
		t.Skipf("migrations ainda não criadas (Fase 4): %s", root)
	}
	files, err := filepath.Glob(filepath.Join(root, "*.sql"))
	if err != nil || len(files) == 0 {
		t.Skipf("nenhuma migration em %s (Fase 4)", root)
	}
	sort.Strings(files)
	for _, f := range files {
		content, err := os.ReadFile(f)
		if err != nil {
			t.Fatalf("ler %s: %v", f, err)
		}
		// Executa só o bloco Up (o script cru não suporta marcadores goose).
		if i := strings.Index(string(content), "-- +goose Down"); i >= 0 {
			content = content[:i]
		}
		if _, err := db.Exec(string(content)); err != nil {
			t.Fatalf("migration %s: %v", filepath.Base(f), err)
		}
	}
}

// Seed insere linhas de apoio (helper simples para os store tests).
func Seed(t *testing.T, db *sql.DB, q string, args ...any) {
	t.Helper()
	if _, err := db.Exec(q, args...); err != nil {
		t.Fatalf("seed %q: %v", strings.TrimSpace(q), err)
	}
	_ = fmt.Sprintf // manter fmt importado para formatações nos seeds futuros
}
