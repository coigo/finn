package movimentacoes

import (
	"context"
	"time"

	"github.com/coigo/finn/internal/platform/money"
)

// Repository do domínio (implementado por Store na Fase 4). Not-found
// mapeados nos sentinels do domínio aqui declarados.
type Repository interface {
	// Create replica CriarMovimentacao.
	Create(ctx context.Context, mov Movimentacao) (Movimentacao, error)
	// Get replica BuscarMovimentacao; ausente → ErrMovimentacaoNaoEncontrada.
	Get(ctx context.Context, id int64) (Movimentacao, error)
	// Delete replica ApagarMovimentacao (ExecuteDelete; duro).
	Delete(ctx context.Context, id int64) error
	// CreateParcelas replica CriarMovimentacaoParcela (bulk).
	CreateParcelas(ctx context.Context, parcelas []Parcela) ([]Parcela, error)
	// ListParcelasPorIDs replica BuscarParcelasPorId (IN do array).
	ListParcelasPorIDs(ctx context.Context, ids []int64) ([]Parcela, error)
	// ListByPeriodo replica BuscarPorPeriodo (JOIN categorias, ORDER BY data DESC).
	ListByPeriodo(ctx context.Context, inicio, fim time.Time) ([]Resumo, error)
	// ListPendentesDoMes replica BuscarPendentesDoMes (UNION parcela/persistente).
	ListPendentesDoMes(ctx context.Context, hoje time.Time) ([]Pendente, error)
	// ListCategorias replica BuscarCategorias (sem filtro deletadoEm — paridade).
	ListCategorias(ctx context.Context) ([]Categoria, error)
	// CreatePersistente replica CriarMovimentacaoPersistente.
	CreatePersistente(ctx context.Context, p Persistente) (Persistente, error)
	// Persistente replica BuscarMovimentacaoPersistente; retorno inclui deletadas (B6); ausente → ErrMovimentacaoNaoEncontradaExcl.
	Persistente(ctx context.Context, id int64) (Persistente, error)
	// EditPersistente replica EditarMovimentacaoPersistente; ausente → ErrMovimentacaoNaoEncontradaSemAcento.
	EditPersistente(ctx context.Context, p Persistente) error
	// SoftDeletePersistente replica DeletarMovimentacaoPersistente (set deletadoEm); ausente → ErrMovimentacaoNaoEncontradaSemAcento.
	SoftDeletePersistente(ctx context.Context, id int64, deletadoEm time.Time) error
	// MarkParcelaPaga replica MarcarParcelaComoPaga (ну(valor: sem erro quando não existe — paridade).
	MarkParcelaPaga(ctx context.Context, id int64) error
}

// SaldosUpdater é a interface do domínio saldos usada por este
// consumidor (feat.md §4): implementada por saldos.Service.
type SaldosUpdater interface {
	// AddDelta replica ISaldoRepository.AtualizarSaldo(nome, delta): saldo.Valor += delta.
	AddDelta(ctx context.Context, nome string, delta money.Money) error
}
