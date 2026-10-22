package salarios

import (
	"errors"
	"time"

	"github.com/coigo/finn/internal/platform/money"
)

// Salario replica a entidade do C# (salarios): {id, valor, criadoEm}.
type Salario struct {
	ID       int64       `json:"id"`
	Valor    money.Money `json:"valor"`
	CriadoEm time.Time   `json:"criadoEm"` // RFC3339 (B3 — divergência do ISO local do C#)
}

// Sentinels com mensagens idênticas ao C#.
var (
	// ErrSalarioNaoCadastrado replica KeyNotFoundException("Salário não cadastrado") → 400.
	ErrSalarioNaoCadastrado = errors.New("Salário não cadastrado")
	// ErrValorNegativo replica BusinessError("Use um valor positivo") → 4xx (B1).
	ErrValorNegativo = errors.New("Use um valor positivo")
)
