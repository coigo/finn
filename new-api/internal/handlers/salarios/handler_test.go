package salarios

import (
	"bytes"
	"context"
	"encoding/json"
	"errors"
	"io"
	"net/http"
	"net/http/httptest"
	"testing"
	"time"

	dominio "github.com/coigo/finn/internal/domain/salarios"
	"github.com/go-chi/chi/v5"

	"github.com/coigo/finn/internal/platform/money"
)

type fakeService struct {
	getRet    dominio.Salario
	getErr    error
	createRet dominio.Salario
	createErr error
	payRet    dominio.Salario
	payErr    error
	nf        *notificador
}

type notificador struct{ chamou bool }

func (f *fakeService) Create(_ context.Context, _ money.Money) (dominio.Salario, error) {
	if f.nf != nil {
		f.nf.chamou = true
	}
	if f.createErr != nil {
		return dominio.Salario{}, f.createErr
	}
	return f.createRet, nil
}

func (f *fakeService) GetCurrent(_ context.Context) (dominio.Salario, error) {
	return f.getRet, f.getErr
}

func (f *fakeService) PayCurrent(_ context.Context) (dominio.Salario, error) {
	return f.payRet, f.payErr
}

func call(t *testing.T, h http.Handler, method, path, body string) (int, string) {
	t.Helper()
	req := httptest.NewRequest(method, path, io.NopCloser(bytes.NewReader([]byte(body))))
	req.Header.Set("Content-Type", "application/json")
	rec := httptest.NewRecorder()
	h.ServeHTTP(rec, req)
	raw, _ := io.ReadAll(rec.Result().Body)
	return rec.Code, string(raw)
}

// GET /api/salario — caminho feliz: último salário por id.
func TestHandlerGetCurrent(t *testing.T) {
	svc := &fakeService{getRet: dominio.Salario{ID: 4, Valor: money.FromCents(350000), CriadoEm: time.Date(2026, 10, 1, 12, 0, 0, 0, time.UTC)}}
	h := chi.NewRouter()
	NewHandler(svc).Mount(h)

	status, body := call(t, h, http.MethodGet, "/api/salario", "")

	if status != 200 {
		t.Fatalf("status = %d; body = %s", status, body)
	}
	var got struct {
		ID       int64   `json:"id"`
		Valor    float64 `json:"valor"`
		CriadoEm string  `json:"criadoEm"`
	}
	if err := json.Unmarshal([]byte(body), &got); err != nil {
		t.Fatalf("body: %s", body)
	}
	// {id, valor (decimal), criadoEm (RFC3339)}
	if got.ID != 4 || got.Valor != 3500.0 || got.CriadoEm != "2026-10-01T12:00:00Z" {
		t.Errorf("got %+v body %s", got, body)
	}
}

// GET sem salário → 400 {status, message:"Salário não cadastrado"}.
func TestHandlerGetCurrentVazio(t *testing.T) {
	svc := &fakeService{getErr: dominio.ErrSalarioNaoCadastrado}
	h := chi.NewRouter()
	NewHandler(svc).Mount(h)

	status, body := call(t, h, http.MethodGet, "/api/salario", "")

	if status != 400 {
		t.Fatalf("status = %d", status)
	}
	// ordem de campos: status depois message (igual ao ErrorHandler C#)
	if body != "{\"status\":400,\"message\":\"Salário não cadastrado\"}\n" {
		t.Errorf("body = %s", body)
	}
}

// POST /api/salario — cria e ecoa o dominio.Salario; formato decimal nos números.
func TestHandlerCreate(t *testing.T) {
	svc := &fakeService{createRet: dominio.Salario{ID: 5, Valor: money.FromCents(12345), CriadoEm: time.Date(2026, 1, 2, 3, 4, 5, 0, time.UTC)}}
	h := chi.NewRouter()
	NewHandler(svc).Mount(h)

	status, body := call(t, h, http.MethodPost, "/api/salario", `{"valor":123.45}`)

	if status != 200 {
		t.Fatalf("status = %d; body = %s", status, body)
	}
	var got map[string]any
	_ = json.Unmarshal([]byte(body), &got)
	if got["valor"] != 123.45 {
		t.Errorf("valor = %v", got["valor"])
	}
}

// POST com valor negativo → 400 "Use um valor positivo".
func TestHandlerCreateValorNegativo(t *testing.T) {
	svc := &fakeService{createErr: dominio.ErrValorNegativo}
	h := chi.NewRouter()
	NewHandler(svc).Mount(h)

	status, body := call(t, h, http.MethodPost, "/api/salario", `{"valor":-1}`)

	if status != 500 || body != "{\"status\":500,\"message\":\"Use um valor positivo\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// POST /api/salario/adicionar — pega último salário e soma no saldo.
func TestHandlerPayCurrent(t *testing.T) {
	svc := &fakeService{payRet: dominio.Salario{ID: 2, Valor: money.FromCents(100000), CriadoEm: time.Now()}}
	h := chi.NewRouter()
	NewHandler(svc).Mount(h)

	status, body := call(t, h, http.MethodPost, "/api/salario/adicionar", "")

	if status != 200 {
		t.Fatalf("status = %d; body = %s", status, body)
	}
	var got map[string]any
	_ = json.Unmarshal([]byte(body), &got)
	if got["valor"] != 1000.0 || got["id"] != float64(2) {
		t.Errorf("body = %s", body)
	}
}

// Sem salário cadastrado: C# cai em NullReferenceException → 500
// (mensagem genérica na porta — divergência documentada).
func TestHandlerPayCurrentSemSalario(t *testing.T) {
	svc := &fakeService{payErr: errors.New("nenhum salário")} // erro inesperado (não sentinel)
	h := chi.NewRouter()
	NewHandler(svc).Mount(h)

	status, body := call(t, h, http.MethodPost, "/api/salario/adicionar", "")

	if status != 500 {
		t.Fatalf("status = %d", status)
	}
	if body != "{\"status\":500,\"message\":\"nenhum salário\"}\n" {
		t.Errorf("body = %s", body)
	}
}
