package saldos

import (
	"context"
	"database/sql"
	"errors"

	dominio "github.com/coigo/finn/internal/domain/saldos"
	"github.com/coigo/finn/internal/platform/money"
	saldos "github.com/coigo/finn/internal/sqlcgen/saldos"
)

// Store replica o SaldosRepository do C# (EF) sobre o código gerado do sqlc.
type Store struct {
	q *saldos.Queries
}

func NewStore(db *sql.DB) *Store {
	return &Store{q: saldos.New(db)}
}

// ByName replica BuscarSaldoPorNome: ausente -> ErrSaldoNaoEncontrado (400).
func (s *Store) ByName(ctx context.Context, nome string) (dominio.Saldo, error) {
	row, err := s.q.FindSaldoByNome(ctx, nome)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return dominio.Saldo{}, dominio.ErrSaldoNaoEncontrado
		}
		return dominio.Saldo{}, err
	}
	return deRow(row), nil
}

// AddDelta replica AtualizarSaldo(nome, decimal): saldo.Valor += delta,
// devolvendo a linha atualizada. Ausente -> ErrSaldoNaoEncontrado.
func (s *Store) AddDelta(ctx context.Context, nome string, delta money.Money) (dominio.Saldo, error) {
	row, err := s.q.AddSaldoDelta(ctx, saldos.AddSaldoDeltaParams{
		Valor: int64(delta),
		Nome:  nome,
	})
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return dominio.Saldo{}, dominio.ErrSaldoNaoEncontrado
		}
		return dominio.Saldo{}, err
	}
	return deRow(row), nil
}

// Set replica AtualizarSaldo(int, Saldo) do C#: sobrescreve o valor pelo id.
func (s *Store) Set(ctx context.Context, sal dominio.Saldo) (dominio.Saldo, error) {
	row, err := s.q.SetSaldoValor(ctx, saldos.SetSaldoValorParams{
		Valor: int64(sal.Valor),
		ID:    sal.ID,
	})
	if err != nil {
		return dominio.Saldo{}, err
	}
	return deRow(row), nil
}

func deRow(row saldos.Saldo) dominio.Saldo {
	return dominio.Saldo{
		ID:             row.ID,
		Valor:          money.FromCents(row.Valor),
		Nome:           row.Nome,
		SaldoHistorico: []any{},
	}
}
