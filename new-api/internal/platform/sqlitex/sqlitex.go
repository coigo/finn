// Package sqlitex abre conexões SQLite (modernc.org/sqlite) com os PRAGMAs
// do feat.md: WAL, busy_timeout e foreign_keys; uma conexão só.
package sqlitex

import (
	"database/sql"
	"fmt"
	"net/url"
	"os"
	"path/filepath"
)

// Open abre o banco no caminho dado, criando o diretório-pai se necessário.
func Open(path string) (*sql.DB, error) {
	if dir := filepath.Dir(path); dir != "." && dir != "" {
		if err := os.MkdirAll(dir, 0o755); err != nil {
			return nil, fmt.Errorf("criar diretório do banco %s: %w", dir, err)
		}
	}
	dsn := "file:" + url.PathEscape(path) +
		"?_pragma=busy_timeout(5000)&_pragma=journal_mode(WAL)&_pragma=foreign_keys(1)"
	db, err := sql.Open("sqlite", dsn)
	if err != nil {
		return nil, fmt.Errorf("abrir banco %s: %w", path, err)
	}
	// SQLite: uma conexão só — evita SQLITE_BUSY sob concorrência (feat.md §5).
	db.SetMaxOpenConns(1)
	if err := db.Ping(); err != nil {
		return nil, fmt.Errorf("ping no banco %s: %w", path, err)
	}
	return db, nil
}
