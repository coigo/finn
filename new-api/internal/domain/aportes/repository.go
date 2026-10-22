package aportes

import (
	"context"

	"github.com/coigo/finn/internal/platform/money"
)

// Repository é o IAporteRepository C# (menos BuscarPorCategoria, sem
// consumidor no C#).
type Repository interface {
	BuscarTodos(ctx context.Context) ([]Aporte, error)
	BuscarPorIdentificador(ctx context.Context, identificador string) (*Aporte, error)
	// BuscarDetalhesDoAporte alimenta GET /{identificador}/historico.
	BuscarDetalhesDoAporte(ctx context.Context, identificador string) ([]HistoricoDTO, error)
	CriarAporte(ctx context.Context, aporte Aporte) (Aporte, error)
	// AtualizarAporte replica o adapter C#: atualiza apenas precoMedio e
	// quantidade; aporte inexistente é erro NÃO-sentinel (KeyNotFoundException
	// → 500 no C#).
	AtualizarAporte(ctx context.Context, id int64, precoMedio, quantidade money.Money) error
}

// HistoricoRepository é o IAporteHistoricoRepository C# (só CriarRegistro;
// Buscar(HistoricoFiltrosDTO) não tem consumidor).
type HistoricoRepository interface {
	CriarRegistro(ctx context.Context, h AporteHistorico) (AporteHistorico, error)
}

// PriceFinder é o IAtivosRepository C# (brapi.dev / coingecko atrás de
// interface — injetado no main). nil = ativo não encontrado.
type PriceFinder interface {
	BuscarPorTicker(ctx context.Context, ticker string) (*money.Money, error)
	BuscarCrypto(ctx context.Context, crypto string) (*money.Money, error)
}
