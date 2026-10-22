package salarios

import (
	"context"
	"errors"
	"testing"
	"time"

	dominio "github.com/coigo/finn/internal/domain/salarios"
	"github.com/coigo/finn/internal/platform/money"
)

// fakeRecorder grava ordem de chamadas entre deps.
type recorder struct{ eventos string }

func (r *recorder) add(ev string) { r.eventos += ev + ";" }

type fakeRepo struct {
	lastRet   dominio.Salario
	lastErr   error
	createErr error
	rec       *recorder
}

func (f *fakeRepo) Last(_ context.Context) (dominio.Salario, error) {
	f.rec.add("repo.Last")
	if f.lastErr != nil {
		return dominio.Salario{}, f.lastErr
	}
	return f.lastRet, nil
}

func (f *fakeRepo) Create(_ context.Context, salario dominio.Salario) (dominio.Salario, error) {
	f.rec.add("repo.Create")
	if f.createErr != nil {
		return dominio.Salario{}, f.createErr
	}
	salario.ID = 7 // como o banco faria
	return salario, nil
}

type fakeSaldos struct {
	err error
	rec *recorder
}

func (f *fakeSaldos) AddDelta(_ context.Context, nome string, delta money.Money) error {
	f.rec.add("saldos.AddDelta(" + nome + ", " + itoa(int(delta)) + ")")
	if f.err != nil {
		return f.err
	}
	return nil
}

type fakeMovs struct {
	err error
	rec *recorder
}

func (f *fakeMovs) CreateEntradaSalario(_ context.Context, valor money.Money) error {
	f.rec.add("movs.CreateEntradaSalario(" + itoa(int(valor)) + ")")
	if f.err != nil {
		return f.err
	}
	return nil
}

func itoa(i int) string {
	if i == 0 {
		return "0"
	}
	neg := i < 0
	if neg {
		i = -i
	}
	var b []byte
	for i > 0 {
		b = append([]byte{byte('0' + i%10)}, b...)
		i /= 10
	}
	if neg {
		return "-" + string(b)
	}
	return string(b)
}

func TestCreateHappy(t *testing.T) {
	repo := fakeRepo{rec: &recorder{}}
	svc := NewService(&repo, nil, nil)

	salario, err := svc.Create(context.Background(), money.FromCents(500000))
	if err != nil {
		t.Fatalf("Create: %v", err)
	}
	if salario.ID != 7 || salario.Valor.Cents() != 500000 {
		t.Errorf("salario = %+v", salario)
	}
	if repo.rec.eventos != "repo.Create;" {
		t.Errorf("eventos = %q", repo.rec.eventos)
	}
}

func TestCreateValorNegativo(t *testing.T) {
	repo := fakeRepo{rec: &recorder{}}
	svc := NewService(&repo, nil, nil)

	_, err := svc.Create(context.Background(), money.FromCents(-1))
	if !errors.Is(err, dominio.ErrValorNegativo) {
		t.Fatalf("esperava ErrValorNegativo; obtive %v", err)
	}
	if err.Error() != "Use um valor positivo" {
		t.Errorf("mensagem = %q", err.Error())
	}
	if repo.rec.eventos != "" {
		t.Errorf("repo.Create não deveria ser chamado; eventos = %q", repo.rec.eventos)
	}
}

func TestGetCurrent(t *testing.T) {
	repo := fakeRepo{lastRet: dominio.Salario{ID: 9, Valor: money.FromCents(350025), CriadoEm: time.Now()}, rec: &recorder{}}
	svc := NewService(&repo, nil, nil)

	salario, err := svc.GetCurrent(context.Background())
	if err != nil {
		t.Fatalf("GetCurrent: %v", err)
	}
	if salario.ID != 9 {
		t.Errorf("id = %d", salario.ID)
	}
}

func TestGetCurrentSemSalario(t *testing.T) {
	repo := fakeRepo{lastErr: dominio.ErrSalarioNaoCadastrado, rec: &recorder{}}
	svc := NewService(&repo, nil, nil)

	_, err := svc.GetCurrent(context.Background())
	if !errors.Is(err, dominio.ErrSalarioNaoCadastrado) || err.Error() != "Salário não cadastrado" {
		t.Errorf("err = %v", err)
	}
}

func TestPayCurrentFluxo(t *testing.T) {
	rec := &recorder{}
	repo := fakeRepo{lastRet: dominio.Salario{ID: 3, Valor: money.FromCents(420000)}, rec: rec}
	saldos := fakeSaldos{rec: rec}
	movs := fakeMovs{rec: rec}
	svc := NewService(&repo, &saldos, &movs)

	salario, err := svc.PayCurrent(context.Background())
	if err != nil {
		t.Fatalf("PayCurrent: %v", err)
	}

	// ordem do C#: saldo primeiro, movimentação depois
	queremos := "repo.Last;saldos.AddDelta(Corrente, 420000);movs.CreateEntradaSalario(420000);"
	if rec.eventos != queremos {
		t.Errorf("eventos = %q; esperado %q", rec.eventos, queremos)
	}
	if salario.ID != 3 {
		t.Errorf("salario retornado: %+v", salario)
	}
}

func TestPayCurrentSemSalario(t *testing.T) {
	// C#.adapters: sem salário → NullReferenceException → 500 (não é 400).
	rec := &recorder{}
	repo := fakeRepo{lastErr: dominio.ErrSalarioNaoCadastrado, rec: rec}
	svc := NewService(&repo, nil, nil)

	_, err := svc.PayCurrent(context.Background())
	if err == nil {
		t.Fatal("esperava erro")
	}
	if errors.Is(err, dominio.ErrSalarioNaoCadastrado) {
		// divergência consciente: NRE do C# (500) não deve receber 400 aqui;
		// na porta o handler responderá 500 com mensagem genérica.
		t.Errorf("PayCurrent não deve repassar o sentinel de 400 (C# devolve 500): %v", err)
	}
}
