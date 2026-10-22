package salarios

import (
	"testing"
	"time"

	dominio "github.com/coigo/finn/internal/domain/salarios"
	"github.com/coigo/finn/internal/platform/money"
	"github.com/coigo/finn/internal/platform/testdb"
)

// last com banco vazio → dominio.ErrSalarioNaoCadastrado (store mapeia sql.ErrNoRows).
func TestStoreLastVazio(t *testing.T) {
	db := testdb.New(t)
	testdb.Migrate(t, db)

	store := NewStore(db)
	if _, err := store.Last(t.Context()); err != dominio.ErrSalarioNaoCadastrado {
		t.Errorf("esperava dominio.ErrSalarioNaoCadastrado; obtive %v", err)
	}
}

func TestStoreLastOrdem(t *testing.T) {
	db := testdb.New(t)
	testdb.Migrate(t, db)
	store := NewStore(db)
	ctx := t.Context()

	if _, err := store.Create(ctx, dominio.Salario{Valor: money.FromCents(100000), CriadoEm: time.Now()}); err != nil {
		t.Fatal(err)
	}
	if _, err := store.Create(ctx, dominio.Salario{Valor: money.FromCents(200000), CriadoEm: time.Now()}); err != nil {
		t.Fatal(err)
	}

	salario, err := store.Last(ctx)
	if err != nil {
		t.Fatalf("Last: %v", err)
	}
	if salario.Valor.Cents() != 200000 {
		t.Errorf("último por id: valor = %d", salario.Valor)
	}
}
