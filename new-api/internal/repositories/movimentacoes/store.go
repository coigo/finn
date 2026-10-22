package movimentacoes

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"time"

	dominio "github.com/coigo/finn/internal/domain/movimentacoes"
	"github.com/coigo/finn/internal/platform/money"
	movimentacoes "github.com/coigo/finn/internal/sqlcgen/movimentacoes"
)

// Store replica o MovimentacaoRepository do C# (EF) sobre o código gerado do sqlc.
type Store struct {
	db *sql.DB
	q  *movimentacoes.Queries
}

func NewStore(db *sql.DB) *Store {
	return &Store{db: db, q: movimentacoes.New(db)}
}

// Create replica Adicionar (Add + SaveChanges): devolve a entidade com o id.
func (s *Store) Create(ctx context.Context, m dominio.Movimentacao) (dominio.Movimentacao, error) {
	if m.CriadoEm.IsZero() {
		m.CriadoEm = time.Now()
	}
	row, err := s.q.InsertMovimentacao(ctx, movimentacoes.InsertMovimentacaoParams{
		Valor:         int64(m.Valor),
		Tipo:          m.Tipo,
		Categoriaid:   m.CategoriaID,
		Descricao:     m.Descricao,
		Data:          ts(m.Data),
		Parcelaid:     nullInt(m.ParcelaID),
		Persistenteid: nullInt(m.PersistenteID),
		Criadoem:      ts(m.CriadoEm),
	})
	if err != nil {
		return dominio.Movimentacao{}, err
	}
	return movDeRow(row), nil
}

// Get replica BuscarMovimentacao; ausente -> dominio.ErrMovimentacaoNaoEncontrada.
func (s *Store) Get(ctx context.Context, id int64) (dominio.Movimentacao, error) {
	row, err := s.q.FindMovimentacaoByID(ctx, id)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return dominio.Movimentacao{}, dominio.ErrMovimentacaoNaoEncontrada
		}
		return dominio.Movimentacao{}, err
	}
	return movDeRow(row), nil
}

// Delete replica ApagarMovimentacao (ExecuteDelete, sem checagem — paridade).
func (s *Store) Delete(ctx context.Context, id int64) error {
	return s.q.DeleteMovimentacao(ctx, id)
}

// CreateParcelas replica CriarMovimentacaoParcela (AddRange): uma transação,
// um INSERT com RETURNING por linha, devolvendo as entidades com ids.
func (s *Store) CreateParcelas(ctx context.Context, parcelas []dominio.Parcela) ([]dominio.Parcela, error) {
	if len(parcelas) == 0 {
		return nil, nil
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, err
	}
	defer tx.Rollback()
	qtx := s.q.WithTx(tx)

	criadas := make([]dominio.Parcela, 0, len(parcelas))
	for _, p := range parcelas {
		if p.CriadoEm.IsZero() {
			p.CriadoEm = time.Now()
		}
		row, err := qtx.InsertParcela(ctx, movimentacoes.InsertParcelaParams{
			Valor:       int64(p.Valor),
			Numero:      p.Numero,
			Tipo:        p.Tipo,
			Categoriaid: p.CategoriaID,
			Descricao:   p.Descricao,
			Vencimento:  ts(p.Vencimento),
			Criadoem:    ts(p.CriadoEm),
			Paga:        boolInt(p.Paga),
		})
		if err != nil {
			return nil, err
		}
		criadas = append(criadas, parcelaDeRow(row))
	}
	if err := tx.Commit(); err != nil {
		return nil, err
	}
	return criadas, nil
}

// ListParcelasPorIDs replica BuscarParcelasPorId (Contains).
func (s *Store) ListParcelasPorIDs(ctx context.Context, ids []int64) ([]dominio.Parcela, error) {
	rows, err := s.q.FindParcelasPorIDs(ctx, ids)
	if err != nil {
		return nil, err
	}
	lista := make([]dominio.Parcela, 0, len(rows))
	for _, r := range rows {
		lista = append(lista, parcelaDeRow(r))
	}
	return lista, nil
}

// MarkParcelaPaga replica MarcarParcelaComoPaga (sem checagem de existência).
func (s *Store) MarkParcelaPaga(ctx context.Context, id int64) error {
	return s.q.MarkParcelaPaga(ctx, id)
}

// ListByPeriodo replica BuscarPorPeriodo (LEFT JOIN categorias, data DESC).
func (s *Store) ListByPeriodo(ctx context.Context, ini, fim time.Time) ([]dominio.Resumo, error) {
	rows, err := s.q.ListMovimentacoesPorPeriodo(ctx, movimentacoes.ListMovimentacoesPorPeriodoParams{
		Data:   ts(ini),
		Data_2: ts(fim),
	})
	if err != nil {
		return nil, err
	}
	lista := make([]dominio.Resumo, 0, len(rows))
	for _, r := range rows {
		lista = append(lista, dominio.Resumo{
			ID:        r.ID,
			Valor:     money.FromCents(r.Valor),
			Tipo:      r.Tipo,
			Descricao: r.Descricao,
			Categoria: r.Categoria.String,
			Data:      parseTS(r.Data),
		})
	}
	return lista, nil
}

// ListPendentesDoMes replica BuscarPendentesDoMes: mês/ano extraídos da data.
func (s *Store) ListPendentesDoMes(ctx context.Context, ref time.Time) ([]dominio.Pendente, error) {
	rows, err := s.q.ListPendentesDoMes(ctx, movimentacoes.ListPendentesDoMesParams{
		Mes: fmt.Sprintf("%02d", int(ref.Month())),
		Ano: fmt.Sprintf("%04d", ref.Year()),
	})
	if err != nil {
		return nil, err
	}
	lista := make([]dominio.Pendente, 0, len(rows))
	for _, r := range rows {
		var venc *time.Time
		if r.Vencimento != "" {
			v := parseTS(r.Vencimento)
			venc = &v
		}
		lista = append(lista, dominio.Pendente{
			ID:           r.ID,
			Valor:        money.FromCents(r.Valor),
			Descricao:    r.Descricao,
			Categoria:    r.Categoria.String,
			CategoriaID:  r.Categoriaid.Int64,
			Vencimento:   venc,
			Tipo:         r.Tipo,
			TipoDerivado: r.Tipoderivado,
		})
	}
	return lista, nil
}

// CreatePersistente replica CriarMovimentacaoPersistente.
func (s *Store) CreatePersistente(ctx context.Context, p dominio.Persistente) (dominio.Persistente, error) {
	if p.CriadoEm.IsZero() {
		p.CriadoEm = time.Now()
	}
	row, err := s.q.InsertPersistente(ctx, movimentacoes.InsertPersistenteParams{
		Valor:       int64(p.Valor),
		Descricao:   p.Descricao,
		Tipo:        p.Tipo,
		Categoriaid: p.CategoriaID,
		Criadoem:    ts(p.CriadoEm),
	})
	if err != nil {
		return dominio.Persistente{}, err
	}
	return persistenteDeRow(row), nil
}

// dominio.Persistente replica BuscarMovimentacaoPersistente: busca crua (B6 —
// deletadas voltam); ausente -> dominio.ErrMovimentacaoNaoEncontradaExcl.
func (s *Store) Persistente(ctx context.Context, id int64) (dominio.Persistente, error) {
	row, err := s.q.FindMovimentacaoPersistenteByID(ctx, id)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return dominio.Persistente{}, dominio.ErrMovimentacaoNaoEncontradaExcl
		}
		return dominio.Persistente{}, err
	}
	return persistenteDeRow(row), nil
}

// EditPersistente replica EditarMovimentacaoPersistente.
func (s *Store) EditPersistente(ctx context.Context, p dominio.Persistente) error {
	return s.q.UpdatePersistente(ctx, movimentacoes.UpdatePersistenteParams{
		Descricao:   p.Descricao,
		Valor:       int64(p.Valor),
		Categoriaid: p.CategoriaID,
		ID:          p.ID,
	})
}

// SoftDeletePersistente replica DeletarMovimentacaoPersistente (seta deletadoEm).
func (s *Store) SoftDeletePersistente(ctx context.Context, id int64, quando time.Time) error {
	return s.q.SoftDeletePersistente(ctx, movimentacoes.SoftDeletePersistenteParams{
		Deletadoem: sql.NullString{String: ts(quando), Valid: true},
		ID:         id,
	})
}

// ListCategorias replica BuscarCategorias (sem filtro, sem ORDER BY — paridade).
func (s *Store) ListCategorias(ctx context.Context) ([]dominio.Categoria, error) {
	rows, err := s.q.ListCategorias(ctx)
	if err != nil {
		return nil, err
	}
	lista := make([]dominio.Categoria, 0, len(rows))
	for _, r := range rows {
		lista = append(lista, dominio.Categoria{
			ID:         r.ID,
			Nome:       r.Nome,
			Tipo:       r.Tipo,
			CriadoEm:   parseTS(r.Criadoem),
			DeletadoEm: nullTime(r.Deletadoem),
		})
	}
	return lista, nil
}

func movDeRow(r movimentacoes.Movimentaco) dominio.Movimentacao {
	return dominio.Movimentacao{
		ID:            r.ID,
		Valor:         money.FromCents(r.Valor),
		Tipo:          r.Tipo,
		CategoriaID:   r.Categoriaid,
		Descricao:     r.Descricao,
		Data:          parseTS(r.Data),
		ParcelaID:     nullIntPtr(r.Parcelaid),
		PersistenteID: nullIntPtr(r.Persistenteid),
		CriadoEm:      parseTS(r.Criadoem),
	}
}

func parcelaDeRow(r movimentacoes.MovimentacoesParcela) dominio.Parcela {
	return dominio.Parcela{
		ID:          r.ID,
		Valor:       money.FromCents(r.Valor),
		Numero:      r.Numero,
		Tipo:        r.Tipo,
		CategoriaID: r.Categoriaid,
		Descricao:   r.Descricao,
		Vencimento:  parseTS(r.Vencimento),
		CriadoEm:    parseTS(r.Criadoem),
		Paga:        r.Paga != 0,
	}
}

func persistenteDeRow(r movimentacoes.MovimentacoesPersistente) dominio.Persistente {
	return dominio.Persistente{
		ID:          r.ID,
		Valor:       money.FromCents(r.Valor),
		Descricao:   r.Descricao,
		Tipo:        r.Tipo,
		CategoriaID: r.Categoriaid,
		CriadoEm:    parseTS(r.Criadoem),
		DeletadoEm:  nullTime(r.Deletadoem),
	}
}

func nullInt(v *int64) sql.NullInt64 {
	if v == nil {
		return sql.NullInt64{}
	}
	return sql.NullInt64{Int64: *v, Valid: true}
}

func nullIntPtr(v sql.NullInt64) *int64 {
	if !v.Valid {
		return nil
	}
	copia := v.Int64
	return &copia
}

func nullTime(v sql.NullString) *time.Time {
	if !v.Valid || v.String == "" {
		return nil
	}
	t := parseTS(v.String)
	return &t
}

func boolInt(b bool) int64 {
	if b {
		return 1
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
