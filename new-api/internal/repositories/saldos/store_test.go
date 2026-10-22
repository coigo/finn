package saldos

import (
	"context"
	"testing"

	dominio "github.com/coigo/finn/internal/domain/saldos"
	"github.com/coigo/finn/internal/platform/money"
	"github.com/coigo/finn/internal/platform/testdb"
)

// Novo store/teste: banco real com migrations (Fase 4). Enquanto migrations
// não existem, est es conjunto de testes pulam (t.Skip).
func TestStoreByNameSeeds(t *testing.T) {
	db := testdb.New(t)
	testdb.Migrate(t, db)

	store := NewStore(db)
	saldo, err := store.ByName(context.Background(), "Corrente")
	if err != nil {
		t.Fatalf("ByName Corrente: %v", err)
	}
	if saldo.ID != 1 || saldo.Valor.Cents() != 0 || saldo.Nome != "Corrente" {
		t.Errorf("seed Corrente: %+v", saldo)
	}

	if _, err := store.ByName(context.Background(), "Investimentos"); err != nil {
		t.Fatalf("ByName Investimentos: %v", err)
	}
	if _, err := store.ByName(context.Background(), "Inexistente"); err != dominio.ErrSaldoNaoEncontrado {
		t.Errorf("esperava dominio.ErrSaldoNaoEncontrado; obtive %v", err)
	}
}

func TestStoreAddDelta(t *testing.T) {
	db := testdb.New(t)
	testdb.Migrate(t, db)
	store := NewStore(db)

	saldo, err := store.AddDelta(t.Context(), "Corrente", money.FromCents(2500))
	if err != nil {
		t.Fatalf("AddDelta: %v", err)
	}
	if saldo.Valor.Cents() != 2500 {
		t.Errorf("valor = %d; esperado 2500", saldo.Valor)
	}

	saldo, err = store.AddDelta(t.Context(), "Corrente", money.FromCents(-500))
	if err != nil {
		t.Fatalf("AddDelta decremento: %v", err)
	}
	if saldo.Valor.Cents() != 2000 {
		t.Errorf("valor = %d; esperado 2000", saldo.Valor)
	}
}

func TestStoreSet(t *testing.T) {
	db := testdb.New(t)
	testdb.Migrate(t, db)
	store := NewStore(db)

	saldo, err := store.Set(t.Context(), dominio.Saldo{ID: 2, Valor: money.FromCents(777), Nome: "Investimentos"})
	if err != nil {
		t.Fatalf("Set: %v", err)
	}
	if saldo.Valor.Cents() != 777 {
		t.Errorf("valor = %d; esperado 777", saldo.Valor)
	}

	porNome, err := store.ByName(t.Context(), "Investimentos")
	if err != nil {
		t.Fatalf("ByName: %v", err)
	}
	if porNome.Valor.Cents() != 777 {
		t.Errorf("valor persistido = %d", porNome.Valor)
	}
}
