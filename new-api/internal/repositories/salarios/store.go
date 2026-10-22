package salarios

import (
	"context"
	"database/sql"
	"errors"
	"time"

	dominio "github.com/coigo/finn/internal/domain/salarios"
	"github.com/coigo/finn/internal/platform/money"
	salarios "github.com/coigo/finn/internal/sqlcgen/salarios"
)

// Store replica o SalarioRepository do C# (EF) sobre o código gerado do sqlc.
type Store struct {
	q *salarios.Queries
}

func NewStore(db *sql.DB) *Store {
	return &Store{q: salarios.New(db)}
}

// Create replica CriarSalario.
func (s *Store) Create(ctx context.Context, sal dominio.Salario) (dominio.Salario, error) {
	if sal.CriadoEm.IsZero() {
		sal.CriadoEm = time.Now()
	}
	row, err := s.q.InsertSalario(ctx, salarios.InsertSalarioParams{
		Valor:    int64(sal.Valor),
		Criadoem: ts(sal.CriadoEm),
	})
	if err != nil {
		return dominio.Salario{}, err
	}
	return deRow(row), nil
}

// Last replica BuscarUltimo: sem salário -> dominio.ErrSalarioNaoCadastrado (400).
func (s *Store) Last(ctx context.Context) (dominio.Salario, error) {
	row, err := s.q.FindSalarioMaisRecente(ctx)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return dominio.Salario{}, dominio.ErrSalarioNaoCadastrado
		}
		return dominio.Salario{}, err
	}
	return deRow(row), nil
}

func deRow(row salarios.Salario) dominio.Salario {
	return dominio.Salario{
		ID:       row.ID,
		Valor:    money.FromCents(row.Valor),
		CriadoEm: parseTS(row.Criadoem),
	}
}

func ts(t time.Time) string {
	return t.Format(time.RFC3339Nano)
}

// parseTS tolera os formatos antigos do SQLite ('yyyy-MM-dd HH:mm:ss.fff')
// e o novo RFC3339 (B3) — dados antigos são lidos normalmente.
func parseTS(s string) time.Time {
	if s == "" {
		return time.Time{}
	}
	for _, layout := range []string{time.RFC3339Nano, "2006-01-02 15:04:05.999", "2006-01-02"} {
		if v, err := time.Parse(layout, s); err == nil {
			return v
		}
	}
	return time.Time{}
}
