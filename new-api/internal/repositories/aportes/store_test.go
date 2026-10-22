package aportes_test

import (
	"testing"
	"time"

	dominio "github.com/coigo/finn/internal/domain/aportes"
	"github.com/coigo/finn/internal/platform/money"
	"github.com/coigo/finn/internal/platform/testdb"
	aportes "github.com/coigo/finn/internal/repositories/aportes"
)

func novoStore(t *testing.T) *aportes.Store {
	t.Helper()
	db := testdb.New(t)
	testdb.Migrate(t, db)
	return aportes.NewStore(db)
}

// Criar/Buscar: ciclo completo sobre a tabela aportes.
func TestStoreCriarEBuscar(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()

	criado, err := store.CriarAporte(ctx, dominio.Aporte{
		Identificador: "PETR4", PrecoMedio: money.FromCents(1000),
		Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO,
		CriadoEm: time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC),
	})
	if err != nil {
		t.Fatal(err)
	}

	porID, err := store.BuscarPorIdentificador(ctx, "PETR4")
	if err != nil {
		t.Fatal(err)
	}
	if porID == nil || porID.ID != criado.ID || porID.PrecoMedio.Cents() != 1000 {
		t.Errorf("porID = %+v", porID)
	}

	if inexistente, _ := store.BuscarPorIdentificador(ctx, "XXXX3"); inexistente != nil {
		t.Errorf("inexistente = %+v", inexistente)
	}
}

// BuscarTodos: apenas quantidade > 0 (paridade do WHERE do C#).
func TestStoreBuscarTodosFiltroQuantidade(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()

	for _, qtd := range []int64{10000, 0, 5000} {
		if _, err := store.CriarAporte(ctx, dominio.Aporte{
			Identificador: "A" + string(rune('0'+qtd/1000)), PrecoMedio: money.FromCents(1000),
			Quantidade: money.FromCents(qtd), Categoria: dominio.CategoriaACAO, CriadoEm: time.Now(),
		}); err != nil {
			t.Fatal(err)
		}
	}

	lista, err := store.BuscarTodos(ctx)
	if err != nil {
		t.Fatal(err)
	}
	if len(lista) != 2 {
		t.Fatalf("lista = %+v", lista)
	}
}

// AtualizarAporte: só precoMedio/quantidade; id inexistente é erro NÃO-sentinel.
func TestStoreAtualizar(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()

	criado, err := store.CriarAporte(ctx, dominio.Aporte{
		Identificador: "PETR4", PrecoMedio: money.FromCents(1000),
		Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO, CriadoEm: time.Now(),
	})
	if err != nil {
		t.Fatal(err)
	}

	if err := store.AtualizarAporte(ctx, criado.ID, money.FromCents(1100), money.FromCents(20000)); err != nil {
		t.Fatal(err)
	}
	porID, _ := store.BuscarPorIdentificador(ctx, "PETR4")
	if porID.PrecoMedio.Cents() != 1100 || porID.Quantidade.Cents() != 20000 ||
		porID.Categoria != dominio.CategoriaACAO {
		t.Errorf("atualizado = %+v", porID)
	}

	if err := store.AtualizarAporte(ctx, 999, money.FromCents(1), money.FromCents(1)); err == nil {
		t.Error("id inexistente deveria falhar (KeyNotFoundException → 500)")
	}
}

// Histórico: insert + busca por identificador (sem ORDER BY — ordem de rowid).
func TestStoreHistorico(t *testing.T) {
	store := novoStore(t)
	ctx := t.Context()

	for i, tipo := range []string{dominio.TipoCOMPRA, dominio.TipoVENDA} {
		preco := money.FromCents(1000 + int64(i))
		if _, err := store.CriarRegistro(ctx, dominio.AporteHistorico{
			Preco: &preco, Identificador: "PETR4", Quantidade: money.FromCents(10000),
			Categoria: dominio.CategoriaACAO, Tipo: tipo,
			Data: time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC), CriadoEm: time.Now(),
		}); err != nil {
			t.Fatal(err)
		}
	}

	detalhes, err := store.BuscarDetalhesDoAporte(ctx, "PETR4")
	if err != nil {
		t.Fatal(err)
	}
	if len(detalhes) != 2 {
		t.Fatalf("detalhes = %+v", detalhes)
	}
	if detalhes[0].Tipo != "COMPRA" || detalhes[1].Tipo != "VENDA" {
		t.Errorf("ordem de inserção: %+v", detalhes)
	}
	if detalhes[0].Preco == nil || detalhes[0].Preco.Cents() != 1000 {
		t.Errorf("preco = %+v", detalhes[0].Preco)
	}

	vazio, err := store.BuscarDetalhesDoAporte(ctx, "XXXX3")
	if err != nil || len(vazio) != 0 {
		t.Errorf("vazio = %+v err = %v", vazio, err)
	}
}
