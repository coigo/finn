package movimentacoes

import (
	"errors"
	"time"

	"github.com/coigo/finn/internal/platform/money"
)

// Tipos (TEXT no banco, igual à conversão HasConversion<string> do C# em
// movimentacoes.tipo e movimentacoes_categorias.tipo).
const (
	TipoEntrada       = "ENTRADA"
	TipoSaida         = "SAIDA"
	TipoInvestimentos = "INVESTIMENTOS"
)

// Ordinais (INTEGER) usados em parcelas/persistentes — o C# NÃO aplica
// HasConversion<string> nessas tabelas (assimetria mantida por paridade).
const (
	OrdinalEntrada       int64 = 0
	OrdinalSaida         int64 = 1
	OrdinalInvestimentos int64 = 2
)

// OrdinalDe replica o binding numérico: ENTRADA=0, SAIDA=1, INVESTIMENTOS=2.
func OrdinalDe(tipo string) int64 {
	switch tipo {
	case TipoEntrada:
		return OrdinalEntrada
	case TipoSaida:
		return OrdinalSaida
	case TipoInvestimentos:
		return OrdinalInvestimentos
	default:
		return -1
	}
}

// Movimentacao (tabela movimentacoes).
type Movimentacao struct {
	ID            int64
	Valor         money.Money
	Tipo          string // TEXT no banco (diferente de parcelas/persistentes)
	CategoriaID   int64
	Descricao     string
	Data          time.Time
	ParcelaID     *int64
	PersistenteID *int64
	CriadoEm      time.Time
}

// Categoria (movimentacoes_categorias).
type Categoria struct {
	ID         int64
	Nome       string
	Tipo       string
	CriadoEm   time.Time
	DeletadoEm *time.Time
}

// Parcela (movimentacoes_parcelas); Tipo ordinal.
type Parcela struct {
	ID          int64
	Valor       money.Money
	Numero      int64
	Tipo        int64
	CategoriaID int64
	Descricao   string
	Vencimento  time.Time
	CriadoEm    time.Time
	Paga        bool
}

// Persistente (movimentacoes_persistentes); Tipo ordinal. Tags de JSON
// porque GET /persistentes/{id} serializa a entidade crua no C#.
type Persistente struct {
	ID          int64       `json:"id"`
	Valor       money.Money `json:"valor"`
	Descricao   string      `json:"descricao"`
	Tipo        int64       `json:"tipo"`
	CategoriaID int64       `json:"categoriaId"`
	CriadoEm    time.Time   `json:"criadoEm"`
	DeletadoEm  *time.Time  `json:"deletadoEm"`
}

// Resumo corresponde ao ListaMovimentacoesDTO (GET /?inicio&fim): tipo string.
type Resumo struct {
	ID        int64       `json:"id"`
	Valor     money.Money `json:"valor"`
	Tipo      string      `json:"tipo"`
	Descricao string      `json:"descricao"`
	Categoria string      `json:"categoria"`
	Data      time.Time   `json:"data"`
}

// Pendente corresponde ao ListaPendentesDTO: vencimento nulo no ramo
// PERSISTENTE (a UNION do C# devolve null); Tipo ordinal (número no JSON).
type Pendente struct {
	ID           int64       `json:"id"`
	Valor        money.Money `json:"valor"`
	Descricao    string      `json:"descricao"`
	Categoria    string      `json:"categoria"`
	CategoriaID  int64       `json:"categoriaId"`
	Vencimento   *time.Time  `json:"vencimento"`
	Tipo         int64       `json:"tipo"`
	TipoDerivado string      `json:"tipoDerivado"`
}

// Sentinels — mensagens exatas do C# (três variantes existem porque o
// original tem três strings diferentes em lugares diferentes):
var (
	// ErrMovimentacaoNaoEncontrada replica DesfazerMovimentacaoUseCase
	// ("Movimentação não encontrada"), mapeado pelo store na busca por id.
	ErrMovimentacaoNaoEncontrada = errors.New("Movimentação não encontrada")
	// ErrMovimentacaoNaoEncontradaExcl replica BuscarPersistenteUseCase
	// ("Movimentação não encontrada!", com exclamação) → GET /persistentes/{id}.
	ErrMovimentacaoNaoEncontradaExcl = errors.New("Movimentação não encontrada!")
	// ErrMovimentacaoNaoEncontradaSemAcento replica os adapters
	// ("Movimentacao não encontrada", sem acento) em editar/deletar persistente.
	ErrMovimentacaoNaoEncontradaSemAcento = errors.New("Movimentacao não encontrada")
)

// ids de categoria fixos em regras (seeds; ver docs/analysis.md §5).
const (
	CategoriaIDSalario   int64 = 9
	CategoriaIDDividendo int64 = 10
	CategoriaIDVenda     int64 = 11
)

// SaldoCorrente: todas as regras de saldo usam "Corrente" (hardcoded no C#).
const SaldoCorrente = "Corrente"

// Periodos e tipoDerivado das pendentes (strings da UNION do C#).
const (
	PeriodoMesAtual         = "MES_ATUAL"
	PeriodoProximoMes       = "PROXIMO_MES"
	TipoDerivadoParcela     = "PARCELA"
	TipoDerivadoPersistente = "PERSISTENTE"
)
