package saldos

import (
	"context"

	"github.com/coigo/finn/internal/platform/money"
)

// Repository é a interface do domínio (implementada por Store em store.go).
type Repository interface {
	// ByName devolve o saldo por nome exato; não achado → ErrSaldoNaoEncontrado.
	ByName(ctx context.Context, nome string) (Saldo, error)
	// AddDelta replica AtualizarSaldo(string, decimal): saldo.Valor += delta.
	// Não achado → ErrSaldoNaoEncontrado (no C# era InvalidOperationException→500; correção acordada).
	AddDelta(ctx context.Context, nome string, delta money.Money) (Saldo, error)
	// Set replica AtualizarSaldo(int, Saldo): sobrescreve Valor (SetValues).
	Set(ctx context.Context, saldo Saldo) (Saldo, error)
}
