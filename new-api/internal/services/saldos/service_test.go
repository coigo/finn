package saldos

import (
	"context"
	"errors"
	"testing"

	dominio "github.com/coigo/finn/internal/domain/saldos"
	"github.com/coigo/finn/internal/platform/money"
)

// fakeRepo para testes de serviço — grava chamadas e devolve resultados fixos.
type fakeRepo struct {
	byNameRet dominio.Saldo
	byNameErr error
	setRet    dominio.Saldo
	setErr    error
	addRet    dominio.Saldo
	addErr    error

	// gravadas
	byNameArg string
	setArg    dominio.Saldo
	addName   string
	addDelta  money.Money
}

func (f *fakeRepo) ByName(_ context.Context, nome string) (dominio.Saldo, error) {
	f.byNameArg = nome
	return f.byNameRet, f.byNameErr
}

func (f *fakeRepo) AddDelta(_ context.Context, nome string, delta money.Money) (dominio.Saldo, error) {
	f.addName, f.addDelta = nome, delta
	return f.addRet, f.addErr
}

func (f *fakeRepo) Set(_ context.Context, saldo dominio.Saldo) (dominio.Saldo, error) {
	f.setArg = saldo
	if f.setErr != nil {
		return dominio.Saldo{}, f.setErr
	}
	return f.setRet, nil
}

func TestGet(t *testing.T) {
	repo := fakeRepo{byNameRet: dominio.Saldo{ID: 1, Valor: money.FromCents(123456), Nome: "Corrente"}}
	svc := NewService(&repo)

	saldo, err := svc.Get(context.Background(), "Corrente")
	if err != nil {
		t.Fatalf("Get: %v", err)
	}
	if repo.byNameArg != "Corrente" {
		t.Errorf("repo.ByName chamado com %q", repo.byNameArg)
	}
	if saldo.Valor.Cents() != 123456 || saldo.Nome != "Corrente" {
		t.Errorf("saldo retornado: %+v", saldo)
	}
}

func TestGetNaoEncontrado(t *testing.T) {
	repo := fakeRepo{byNameErr: dominio.ErrSaldoNaoEncontrado}
	svc := NewService(&repo)

	_, err := svc.Get(context.Background(), "Investimentos")
	if !errors.Is(err, dominio.ErrSaldoNaoEncontrado) {
		t.Fatalf("erros.Is(ErrSaldoNaoEncontrado) = false; err = %v", err)
	}
	if err.Error() != "Saldo não encontrado!" {
		t.Errorf("mensagem = %q", err.Error())
	}
}

func TestSetSobrescreveValor(t *testing.T) {
	repo := fakeRepo{byNameRet: dominio.Saldo{ID: 2, Valor: money.FromCents(1), Nome: "Investimentos"}}
	repo.setRet = dominio.Saldo{ID: 2, Valor: money.FromCents(999) /* alterado pelo fake */, Nome: "Investimentos"}
	svc := NewService(&repo)

	_, err := svc.Set(context.Background(), "Investimentos", money.FromCents(99900))
	if err != nil {
		t.Fatalf("Set: %v", err)
	}
	if repo.setArg.ID != 2 {
		t.Errorf("Set recebido com id %d", repo.setArg.ID)
	}
	if repo.setArg.Valor.Cents() != 99900 {
		t.Errorf("Set recebido com valor %d (esperado 99900)", repo.setArg.Valor)
	}

	// replicate: o uso do C# retornaria o saldo sobrescrito — aceita o que o repo devolve
	ret, _ := svc.Set(context.Background(), "Investimentos", money.FromCents(99900))
	if ret.Valor.Cents() != 999 {
		t.Errorf("retorno não veio do repositório: %+v", ret)
	}
}

func TestSetNomeInexistente(t *testing.T) {
	repo := fakeRepo{byNameErr: dominio.ErrSaldoNaoEncontrado}
	svc := NewService(&repo)

	_, err := svc.Set(context.Background(), "催化", money.FromCents(10))
	if !errors.Is(err, dominio.ErrSaldoNaoEncontrado) {
		t.Fatalf("esperava ErrSaldoNaoEncontrado; obtive %v", err)
	}
}

func TestAddDelta(t *testing.T) {
	repo := fakeRepo{}
	repo.addRet = dominio.Saldo{ID: 1, Valor: money.FromCents(500), Nome: "Corrente"}
	svc := NewService(&repo)

	err := svc.AddDelta(context.Background(), "Corrente", money.FromCents(123))
	if err != nil {
		t.Fatalf("AddDelta: %v", err)
	}
	if repo.addName != "Corrente" || repo.addDelta.Cents() != 123 {
		t.Errorf("AddDelta args: nome=%q delta=%d", repo.addName, repo.addDelta)
	}
}

func TestAddDeltaNaoEncontrado(t *testing.T) {
	repo := fakeRepo{addErr: dominio.ErrSaldoNaoEncontrado}
	svc := NewService(&repo)

	err := svc.AddDelta(context.Background(), "Investimentos", money.FromCents(1))
	if !errors.Is(err, dominio.ErrSaldoNaoEncontrado) {
		t.Fatalf("esperava ErrSaldoNaoEncontrado; obtive %v", err)
	}
}
