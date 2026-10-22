package saldos

import (
	"context"
	"encoding/json"
	"io"
	"net/http"
	"net/http/httptest"
	"testing"
	"time"

	dominio "github.com/coigo/finn/internal/domain/saldos"
	"github.com/go-chi/chi/v5"

	"github.com/coigo/finn/internal/platform/money"
)

// fakeService satisfaz ServiceAPI para os testes de handler.
type fakeService struct {
	getRet  dominio.Saldo
	getErr  error
	setRet  dominio.Saldo
	setErr  error
	setName string
	setVal  money.Money
}

func (f *fakeService) Get(_ context.Context, _ string) (dominio.Saldo, error) {
	return f.getRet, f.getErr
}

func (f *fakeService) Set(_ context.Context, nome string, valor money.Money) (dominio.Saldo, error) {
	f.setName, f.setVal = nome, valor
	if f.setErr != nil {
		return dominio.Saldo{}, f.setErr
	}
	return f.setRet, nil
}

func chamada(t *testing.T, h http.Handler, method, path, body string) (int, string) {
	t.Helper()
	var bodyReader io.Reader
	if body != "" {
		bodyReader = io.NopCloser(io.Reader(newStringReader(body)))
	}
	req := httptest.NewRequest(method, path, bodyReader)
	req.Header.Set("Content-Type", "application/json")
	rec := httptest.NewRecorder()
	h.ServeHTTP(rec, req)
	raw, _ := io.ReadAll(rec.Result().Body)
	return rec.Result().StatusCode, string(raw)
}

type stringReader struct {
	s string
	i int
}

func (r *stringReader) Read(p []byte) (int, error) {
	if r.i >= len(r.s) {
		return 0, io.EOF
	}
	n := copy(p, r.s[r.i:])
	r.i += n
	return n, nil
}

func newStringReader(s string) io.Reader { return &stringReader{s: s} }

func TestHandlerGetSaldo(t *testing.T) {
	svc := &fakeService{getRet: dominio.Saldo{ID: 1, Valor: money.FromCents(123456) /* 1234.56 */, Nome: "Corrente"}}
	r := chi.NewRouter()
	NewHandler(svc).Mount(r)

	status, body := chamada(t, r, http.MethodGet, "/api/saldos?saldoNome=Corrente", "")

	corpoEsperado := map[string]any{"id": float64(1), "valor": 1234.56, "nome": "Corrente"}
	var got map[string]any
	if status != 200 {
		t.Fatalf("status = %d, body = %s", status, body)
	}
	if err := json.Unmarshal([]byte(body), &got); err != nil {
		t.Fatalf("json inválido: %s", body)
	}

	// caminho feliz: 200 com {id, valor (decimal), nome}
	if got["id"].(float64) != corpoEsperado["id"] || got["nome"] != corpoEsperado["nome"] {
		t.Errorf("body = %q", body)
	}
	if v, ok := got["valor"].(float64); !ok || v != 1234.56 {
		t.Errorf("valor = %v (%T); esperado número decimal 1234.56", got["valor"], got["valor"])
	}
}

func TestHandlerGetSaldoNaoEncontrado(t *testing.T) {
	svc := &fakeService{getErr: dominio.ErrSaldoNaoEncontrado}
	r := chi.NewRouter()
	NewHandler(svc).Mount(r)

	status, body := chamada(t, r, http.MethodGet, "/api/saldos?saldoNome=x", "")

	// 400 com {status: 400, message: "Saldo não encontrado!"} — igual ao ErrorHandler do C#
	if status != 400 {
		t.Fatalf("status = %d, body = %s", status, body)
	}
	if body != "{\"status\":400,\"message\":\"Saldo não encontrado!\"}\n" {
		t.Errorf("body = %s", body)
	}
}

func TestHandlerSetSaldo(t *testing.T) {
	svc := &fakeService{setRet: dominio.Saldo{ID: 1, Valor: money.FromCents(10000), Nome: "Corrente"}}
	r := chi.NewRouter()
	NewHandler(svc).Mount(r)

	status, body := chamada(t, r, http.MethodPost, "/api/saldos", `{"nome":"Corrente","valor":100.5}`)

	if status != 200 {
		t.Fatalf("status = %d, body = %s", status, body)
	}
	if svc.setName != "Corrente" || svc.setVal.Cents() != 10050 {
		t.Errorf("service.Set args: %q %d", svc.setName, svc.setVal)
	}
	var got map[string]any
	_ = json.Unmarshal([]byte(body), &got)
	_ = time.Now
	if got["valor"] != 100.0 {
		t.Errorf("valor retornado = %v", got["valor"])
	}
}
