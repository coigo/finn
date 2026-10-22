// Pacote db embarca as migrations (goose.Up no main e nos testes).
package db

import "embed"

//go:embed migrations/*.sql
var FS embed.FS
