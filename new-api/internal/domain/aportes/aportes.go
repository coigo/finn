// Domain aportes: investimentos. Paridade com api/src/core/modules/Aportes.
package aportes

import (
	"errors"
	"time"

	"github.com/coigo/finn/internal/platform/money"
)

// Categorias do enum AporteCategoria (mesmos valores do C#).
const (
	CategoriaACAO        int64 = 1
	CategoriaFII         int64 = 2
	CategoriaRENDAFIXA   int64 = 3
	CategoriaCRIPTOMOEDA int64 = 4
	CategoriaETF         int64 = 5
)

// Tipos de movimentação (AporteTipo; COMPRA=0 no enum C#).
const (
	TipoCOMPRA        = "COMPRA"
	TipoVENDA         = "VENDA"
	TipoDESDOBRAMENTO = "DESDOBRAMENTO"
)

// Sentinels com as mensagens exatas do C# (B1: erros → 4xx via errors.Is).
var (
	ErrAporteNaoEncontrado = errors.New("Aporte não encontrado")
	ErrQuantidadeInvalida  = errors.New("Quantidade inválida")
	ErrPrecoMedioInvalido  = errors.New("Preço médio inválido")
	ErrAtivoNaoEncontrado  = errors.New("Ativo não encontrado!")
	// ErrTipoInvalido replica ArgumentException("Tipo inválido") do getter Tipo
	// do MovimentarAportePorIdentificadorDTO (tipoStr fora de COMPRA/VENDA/
	// DESDOBRAMENTO) — 500, mensagem exata.
	ErrTipoInvalido = errors.New("Tipo inválido")
	// Mensagem vazia de propósito: o texto real é dinâmico
	// ("O Ticker %s não existe! " — com espaço final). Use
	// fmt.Errorf("O Ticker %s não existe! %w", id, ErrTickerNaoExiste).
	ErrTickerNaoExiste = errors.New("")
)

// Aporte espelha a entidade C# (serialização camelCase: id, identificador,
// precoMedio, quantidade, categoria como NÚMERO, criadoEm).
type Aporte struct {
	ID            int64       `json:"id"`
	Identificador string      `json:"identificador"`
	PrecoMedio    money.Money `json:"precoMedio"`
	Quantidade    money.Money `json:"quantidade"`
	Categoria     int64       `json:"categoria"`
	CriadoEm      time.Time   `json:"criadoEm"`
}

// AporteHistorico espelha a entidade (uso interno; o contrato HTTP é HistoricoDTO).
type AporteHistorico struct {
	ID            int64
	Preco         *money.Money
	Identificador string
	Quantidade    money.Money
	Categoria     int64
	Tipo          string
	Data          time.Time
	CriadoEm      time.Time
}

// HistoricoDTO é o AporteHistoricoDTO C# (tipo como STRING).
type HistoricoDTO struct {
	Identificador string       `json:"identificador"`
	Quantidade    money.Money  `json:"quantidade"`
	Preco         *money.Money `json:"preco"`
	Data          time.Time    `json:"data"`
	Tipo          string       `json:"tipo"`
}

// ListDTO é o BuscarAportesDTO C# (categoria como STRING).
type ListDTO struct {
	Identificador string      `json:"identificador"`
	PrecoMedio    money.Money `json:"precoMedio"`
	PrecoAtual    money.Money `json:"precoAtual"`
	Quantidade    money.Money `json:"quantidade"`
	Categoria     string      `json:"categoria"`
}

// SelectValue é o SelectValues C# para GET /categorias ({id, name}).
type SelectValue struct {
	ID   int64  `json:"id"`
	Name string `json:"name"`
}

// CategoriaNome mapeia enum → string (AporteCategoria.ToString()).
func CategoriaNome(c int64) string {
	switch c {
	case CategoriaACAO:
		return "ACAO"
	case CategoriaFII:
		return "FII"
	case CategoriaRENDAFIXA:
		return "RENDA_FIXA"
	case CategoriaCRIPTOMOEDA:
		return "CRIPTOMOEDA"
	case CategoriaETF:
		return "ETF"
	}
	return ""
}
