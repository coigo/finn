package movimentacoes_test

import (
	"testing"
	"time"

	dominio "github.com/coigo/finn/internal/domain/movimentacoes"
	"github.com/coigo/finn/internal/platform/money"
	"github.com/coigo/finn/internal/platform/testdb"
	movimentacoes "github.com/coigo/finn/internal/repositories/movimentacoes"
)

func novoStore(t *testing.T) *movimentacoes.Store {
	t.Helper()
	db := testdb.New(t)
	testdb.Migrate(t, db)
	return movimentacoes.NewStore(db)
}

// Create/Get/Delete: ciclo completo sobre a tabela movimentacoes.
func TestStoreVariacaoML(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()

	mov, err := store.Create(ctx, dominio.Movimentacao{
		Valor: money.FromCents(125000), Tipo: "ENTRADA", CategoriaID: 9,
		Descricao: "Salário", Data: time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC),
	})
	if err != nil {
		t.Fatal(err)
	}

	deVolta, err := store.Get(ctx, mov.ID)
	if err != nil {
		t.Fatalf("Get: %v", err)
	}
	if deVolta.Valor.Cents() != 125000 || deVolta.Tipo != "ENTRADA" || deVolta.CategoriaID != 9 {
		t.Errorf("mov de volta: %+v", deVolta)
	}

	if err := store.Delete(ctx, mov.ID); err != nil {
		t.Fatal(err)
	}
	if _, err := store.Get(ctx, mov.ID); err != dominio.ErrMovimentacaoNaoEncontrada {
		t.Errorf("esperava ErrMovimentacaoNaoEncontrada; obtive %v", err)
	}
}

// ListByPeriodo: JOIN categorias; ORDER BY data DESC; filtro inclusivo.
func TestStoreListByPeriodo(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()
	base := time.Date(2025, 11, 1, 0, 0, 0, 0, time.UTC)

	for i, d := range []baseDatas{
		{day: 1, valor: 1000}, {day: 15, valor: 2000}, {day: 29, valor: 3000},
	} {
		if _, err := store.Create(ctx, dominio.Movimentacao{
			Valor: money.FromCents(int64(d.valor)), Tipo: "SAIDA", CategoriaID: 1,
			Descricao: "m" + string(rune('0'+i)), Data: base.AddDate(0, 0, d.day-1),
		}); err != nil {
			t.Fatal(err)
		}
	}

	resumos, err := store.ListByPeriodo(ctx, base, base.Add(5*24*time.Hour))
	if err != nil {
		t.Fatal(err)
	}
	if len(resumos) != 1 || resumos[0].Valor.Cents() != 1000 || resumos[0].Categoria != "Comida e Mercado" {
		t.Errorf("resumos = %+v", resumos)
	}

	resumos, err = store.ListByPeriodo(ctx, base, base.AddDate(0, 0, 1).Add(-time.Microsecond))
	if err != nil {
		t.Fatal(err)
	}
	if len(resumos) != 1 || resumos[0].Valor.Cents() != 1000 {
		t.Errorf("fim inclusivo: %+v", resumos)
	}

	resumos, err = store.ListByPeriodo(ctx, base, base.AddDate(0, 0, 40).Add(-time.Microsecond))
	if err != nil {
		t.Fatal(err)
	}
	// mais novo → mais velho
	if len(resumos) != 3 || resumos[0].Valor.Cents() != 3000 || resumos[2].Valor.Cents() != 1000 {
		t.Errorf("ordem: %+v", resumos)
	}
}

type baseDatas struct {
	day   int
	valor int
}

// Pendentes: UNION parcela/persistente; mês do vencimento; não-pagas.
func TestStoreListPendentes(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()
	hoje := time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC)

	ps, err := store.CreateParcelas(ctx, []dominio.Parcela{
		{Valor: money.FromCents(1000), Numero: 1, Tipo: 1, CategoriaID: 6, Descricao: "aluguel", Vencimento: hoje},
		{Valor: money.FromCents(1000), Numero: 2, Tipo: 1, CategoriaID: 6, Descricao: "aluguel", Vencimento: hoje.AddDate(0, 1, 0)},
		{Valor: money.FromCents(500), Numero: 3, Tipo: 0, CategoriaID: 9, Descricao: "paga", Vencimento: hoje, Paga: true},
	})
	if err != nil {
		t.Fatal(err)
	}

	if _, err := store.CreatePersistente(ctx, dominio.Persistente{
		Valor: money.FromCents(2000), Descricao: "streaming", Tipo: 1, CategoriaID: 6,
	}); err != nil {
		t.Fatal(err)
	}

	pendentes, err := store.ListPendentesDoMes(ctx, hoje)
	if err != nil {
		t.Fatal(err)
	}
	// parcela do mês atual (não paga) + persistente (sin movimentação no mês)
	if len(pendentes) != 2 {
		t.Fatalf("pendentes = %+v", pendentes)
	}
	if pendentes[0].TipoDerivado != "PARCELA" || pendentes[0].ID != ps[0].ID {
		t.Errorf("primeira pendente esperada a parcela: %+v", pendentes[0])
	}
	if pendentes[1].TipoDerivado != "PERSISTENTE" || pendentes[1].Vencimento != nil {
		t.Errorf("segunda pendente: %+v", pendentes[1])
	}

	// cria movimentação à persistente → some das pendentes do mês
	pid := pendentes[1].ID
	if _, err := store.Create(ctx, dominio.Movimentacao{
		Valor: money.FromCents(2000), Tipo: "SAIDA", CategoriaID: 6, Descricao: "streaming",
		Data: hoje, PersistenteID: &pid,
	}); err != nil {
		t.Fatal(err)
	}
	pendentes, err = store.ListPendentesDoMes(ctx, hoje)
	if err != nil {
		t.Fatal(err)
	}
	if len(pendentes) != 1 || pendentes[0].TipoDerivado != "PARCELA" {
		t.Errorf("esperava só a parcela; got %+v", pendentes)
	}
}

func TestStoreMerchantParcelasPorIDs(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()
	ps, err := store.CreateParcelas(ctx, []dominio.Parcela{
		{Valor: money.FromCents(100), Numero: 1, Tipo: 0, CategoriaID: 1, Descricao: "a", Vencimento: time.Now()},
		{Valor: money.FromCents(200), Numero: 2, Tipo: 0, CategoriaID: 1, Descricao: "b", Vencimento: time.Now()},
	})
	if err != nil {
		t.Fatal(err)
	}

	deVolta, err := store.ListParcelasPorIDs(ctx, []int64{ps[0].ID, ps[1].ID})
	if err != nil {
		t.Fatal(err)
	}
	if len(deVolta) != 2 {
		t.Fatalf("got %d", len(deVolta))
	}
	if err := store.MarkParcelaPaga(ctx, ps[0].ID); err != nil {
		t.Fatal(err)
	}
	deVolta, _ = store.ListParcelasPorIDs(ctx, []int64{ps[0].ID})
	if !deVolta[0].Paga {
		t.Error("parcela deveria estar paga")
	}
}

// Persistentes: crua/busca (inclusive deletada)/edita/apaga.
func TestStorePersistente(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()

	p, err := store.CreatePersistente(ctx, dominio.Persistente{
		Valor: money.FromCents(4400), Descricao: "streaming", Tipo: 1, CategoriaID: 6,
	})
	if err != nil {
		t.Fatal(err)
	}

	porID, err := store.Persistente(ctx, p.ID)
	if err != nil {
		t.Fatal(err)
	}
	if porID.Descricao != "streaming" || porID.Tipo != 1 {
		t.Errorf("persistente: %+v", porID)
	}

	if err := store.EditPersistente(ctx, dominio.Persistente{
		ID: p.ID, Valor: money.FromCents(5000), Descricao: "stream+", CategoriaID: 5, Tipo: 1,
	}); err != nil {
		t.Fatal(err)
	}
	porID, _ = store.Persistente(ctx, p.ID)
	if porID.Valor.Cents() != 5000 || porID.Descricao != "stream+" {
		t.Errorf("editada: %+v", porID)
	}

	if err := store.SoftDeletePersistente(ctx, p.ID, time.Now()); err != nil {
		t.Fatal(err)
	}
	// B6: busca continua retornando
	porID, err = store.Persistente(ctx, p.ID)
	if err != nil {
		t.Fatalf("persistente deletada deveria retornar; err = %v", err)
	}
	if porID.DeletadoEm == nil {
		t.Error("deletadoEm deveria estar setado")
	}

	if _, err := store.Persistente(ctx, 999); err != dominio.ErrMovimentacaoNaoEncontradaExcl {
		t.Errorf("esperava ErrMovimentacaoNaoEncontradaExcl; obtive %v", err)
	}
}

// Categorias: seeds de 13 com strings de tipo.
func TestStoreCategoriasSeeds(t *testing.T) {
	store := novoStore(t)
	cats, err := store.ListCategorias(t.Context())
	if err != nil {
		t.Fatal(err)
	}
	if len(cats) != 13 {
		t.Fatalf("len categorias = %d", len(cats))
	}
	if cats[0].Nome != "Comida e Mercado" || cats[0].Tipo != "SAIDA" {
		t.Errorf("primeira: %+v", cats[0])
	}
	if cats[8].Nome != "Salário" || cats[8].Tipo != "ENTRADA" || cats[8].ID != 9 {
		t.Errorf("salário: %+v", cats[8])
	}
}
