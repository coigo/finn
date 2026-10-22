package aportes

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"time"

	dominio "github.com/coigo/finn/internal/domain/aportes"
	"github.com/coigo/finn/internal/platform/money"
	aportes "github.com/coigo/finn/internal/sqlcgen/aportes"
)

// Store replica o AporteRepository/AporteHistoricoRepository do C# (EF).
type Store struct {
	q *aportes.Queries
}

func NewStore(db *sql.DB) *Store {
	return &Store{q: aportes.New(db)}
}

// CriarAporte replica Adicionar (Add + SaveChanges): devolve a entidade com id.
func (s *Store) CriarAporte(ctx context.Context, a dominio.Aporte) (dominio.Aporte, error) {
	if a.CriadoEm.IsZero() {
		a.CriadoEm = time.Now()
	}
	row, err := s.q.InsertAporte(ctx, aportes.InsertAporteParams{
		Identificador: a.Identificador,
		Precomedio:    int64(a.PrecoMedio),
		Quantidade:    int64(a.Quantidade),
		Categoria:     dominio.CategoriaNome(a.Categoria),
		Criadoem:      ts(a.CriadoEm),
	})
	if err != nil {
		return dominio.Aporte{}, err
	}
	return aporteDeRow(row), nil
}

// CriarRegistro replica o Add do AporteHistoricoRepository.
func (s *Store) CriarRegistro(ctx context.Context, h dominio.AporteHistorico) (dominio.AporteHistorico, error) {
	if h.CriadoEm.IsZero() {
		h.CriadoEm = time.Now()
	}
	preco := sql.NullInt64{}
	if h.Preco != nil {
		preco = sql.NullInt64{Int64: int64(*h.Preco), Valid: true}
	}
	row, err := s.q.InsertAporteHistorico(ctx, aportes.InsertAporteHistoricoParams{
		Preco:         preco,
		Identificador: h.Identificador,
		Quantidade:    int64(h.Quantidade),
		Categoria:     dominio.CategoriaNome(h.Categoria),
		Tipo:          h.Tipo,
		Data:          ts(h.Data),
		Criadoem:      ts(h.CriadoEm),
	})
	if err != nil {
		return dominio.AporteHistorico{}, err
	}
	return histDeRow(row), nil
}

// BuscarPorIdentificador replica BuscarPorIdentificador: inexistente -> (nil, nil).
func (s *Store) BuscarPorIdentificador(ctx context.Context, identificador string) (*dominio.Aporte, error) {
	row, err := s.q.FindAporteByIdentificador(ctx, identificador)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return nil, nil
		}
		return nil, err
	}
	ap := aporteDeRow(row)
	return &ap, nil
}

// BuscarTodos replica BuscarTodos (Where Quantidade > 0).
func (s *Store) BuscarTodos(ctx context.Context) ([]dominio.Aporte, error) {
	rows, err := s.q.ListAportesComQuantidade(ctx)
	if err != nil {
		return nil, err
	}
	lista := make([]dominio.Aporte, 0, len(rows))
	for _, r := range rows {
		lista = append(lista, aporteDeRow(r))
	}
	return lista, nil
}

// AtualizarAporte replica AtualizarAporte (só precoMedio/quantidade).
// Id inexistente -> erro NÃO-sentinel (KeyNotFoundException do C# -> 500).
func (s *Store) AtualizarAporte(ctx context.Context, id int64, precoMedio, quantidade money.Money) error {
	n, err := s.q.UpdateAporte(ctx, aportes.UpdateAporteParams{
		Precomedio: int64(precoMedio),
		Quantidade: int64(quantidade),
		ID:         id,
	})
	if err != nil {
		return err
	}
	if n == 0 {
		return fmt.Errorf("atualizar aporte %d: nenhuma linha afetada", id)
	}
	return nil
}

// BuscarDetalhesDoAporte replica o BuscarDetalhesDoAporte do C#: sem ORDER BY
// (ordem natural de inserção/rowid) e já devolve o HistoricoDTO (paridade da
// interface — o repositório C# também mapeia para o DTO).
func (s *Store) BuscarDetalhesDoAporte(ctx context.Context, identificador string) ([]dominio.HistoricoDTO, error) {
	rows, err := s.q.ListAportesHistoricoByIdentificador(ctx, identificador)
	if err != nil {
		return nil, err
	}
	lista := make([]dominio.HistoricoDTO, 0, len(rows))
	for _, r := range rows {
		h := histDeRow(r)
		lista = append(lista, dominio.HistoricoDTO{
			Identificador: h.Identificador,
			Quantidade:    h.Quantidade,
			Preco:         h.Preco,
			Data:          h.Data,
			Tipo:          h.Tipo,
		})
	}
	return lista, nil
}

func aporteDeRow(r aportes.Aporte) dominio.Aporte {
	return dominio.Aporte{
		ID:            r.ID,
		Identificador: r.Identificador,
		PrecoMedio:    money.FromCents(r.Precomedio),
		Quantidade:    money.FromCents(r.Quantidade),
		Categoria:     parseCategoria(r.Categoria),
		CriadoEm:      parseTS(r.Criadoem),
	}
}

func histDeRow(r aportes.AportesHistorico) dominio.AporteHistorico {
	var preco *money.Money
	if r.Preco.Valid {
		p := money.FromCents(r.Preco.Int64)
		preco = &p
	}
	return dominio.AporteHistorico{
		ID:            r.ID,
		Preco:         preco,
		Identificador: r.Identificador,
		Quantidade:    money.FromCents(r.Quantidade),
		Categoria:     parseCategoria(r.Categoria),
		Tipo:          r.Tipo,
		Data:          parseTS(r.Data),
		CriadoEm:      parseTS(r.Criadoem),
	}
}

// parseCategoria converte o TEXT do banco de volta para o enum (o C# grava
// AporteCategoria.ToString(): "ACAO", "FII", ...).
func parseCategoria(nome string) int64 {
	switch nome {
	case "ACAO":
		return dominio.CategoriaACAO
	case "FII":
		return dominio.CategoriaFII
	case "RENDA_FIXA":
		return dominio.CategoriaRENDAFIXA
	case "CRIPTOMOEDA":
		return dominio.CategoriaCRIPTOMOEDA
	case "ETF":
		return dominio.CategoriaETF
	}
	return 0
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
