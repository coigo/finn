package saldos

import (
	"errors"

	"github.com/coigo/finn/internal/platform/money"
)

type Saldo struct {
	ID    int64       `json:"id"`
	Valor money.Money `json:"valor"` // centavos; serializa como número decimal (1250 → 12.5)
	Nome  string      `json:"nome"`
	// Paridade: o Saldo C# tem nav collection SaldoHistorico que o EF serializa
	// vazia (nunca populada nas consultas) → campo sempre [] no JSON.
	SaldoHistorico []any `json:"saldoHistorico"`
}

// Sentinels do domínio (mensagens idênticas às do C#).
var (
	// ErrSaldoNaoEncontrado replica KeyNotFoundException("Saldo não encontrado!")
	// de BuscarSaldoPorNome → 400.
	ErrSaldoNaoEncontrado = errors.New("Saldo não encontrado!")
)
