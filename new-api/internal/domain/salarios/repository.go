package salarios

import (
	"context"

	"github.com/coigo/finn/internal/platform/money"
)

// Repository do domínio (implementado por Store; sqlc na Fase 4).
type Repository interface {
	// Last replica BuscarUltimo (ORDER BY id DESC LIMIT 1);
	// nenhum salário → ErrSalarioNaoCadastrado (mapeado no store).
	Last(ctx context.Context) (Salario, error)
	// Create replica CriarSalario.
	Create(ctx context.Context, salario Salario) (Salario, error)
}

// Interfaces de outros domínios — definidas no consumidor (feat.md §4):
type SaldosUpdater interface {
	// AddDelta replica ISaldoRepository.AtualizarSaldo(nome, valor)
	// (saldo do "Corrente" += valor; usado por POST /salario/adicionar).
	AddDelta(ctx context.Context, nome string, delta money.Money) error
}

type MovimentacoesCreator interface {
	// CreateEntradaSalario replica IMovimentacaoRepository.CriarMovimentacao
	// com Movimentacao{ENTRADA, categoriaId 9, "Salário", data agora}.
	CreateEntradaSalario(ctx context.Context, valor money.Money) error
}
