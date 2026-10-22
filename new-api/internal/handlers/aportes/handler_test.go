package aportes_test

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

	"github.com/go-chi/chi/v5"

	dominio "github.com/coigo/finn/internal/domain/aportes"
	aportes "github.com/coigo/finn/internal/handlers/aportes"
	plataformaMw "github.com/coigo/finn/internal/platform/middleware"
	"github.com/coigo/finn/internal/platform/money"
	servico "github.com/coigo/finn/internal/services/aportes"
)

func errNaoSentinel() error { return errors.New("falha inesperada") }

type fakeAporteSvc struct {
	movInput      servico.MovimentarInput
	movPorIDIdent string
	movPorIDInput servico.MovimentarPorIdentificadorInput
	aporte        dominio.Aporte
	lista         []dominio.ListDTO
	historico     []dominio.HistoricoDTO
	errMov        error
	errList       error
	panica        bool
}

func (f *fakeAporteSvc) Movimentar(_ context.Context, in servico.MovimentarInput) (dominio.Aporte, error) {
	if f.panica {
		panic("runtime error: integer divide by zero")
	}
	f.movInput = in
	if f.errMov != nil {
		return dominio.Aporte{}, f.errMov
	}
	return f.aporte, nil
}

func (f *fakeAporteSvc) MovimentarPorIdentificador(_ context.Context, ident string, in servico.MovimentarPorIdentificadorInput) (dominio.Aporte, error) {
	f.movPorIDIdent = ident
	f.movPorIDInput = in
	if f.errMov != nil {
		return dominio.Aporte{}, f.errMov
	}
	return f.aporte, nil
}

func (f *fakeAporteSvc) List(context.Context) ([]dominio.ListDTO, error) {
	if f.errList != nil {
		return nil, f.errList
	}
	return f.lista, nil
}

func (f *fakeAporteSvc) ListHistorico(_ context.Context, ident string) ([]dominio.HistoricoDTO, error) {
	f.movPorIDIdent = ident
	return f.historico, nil
}

func (f *fakeAporteSvc) Categorias() []dominio.SelectValue {
	return []dominio.SelectValue{
		{ID: 1, Name: "ACAO"}, {ID: 2, Name: "FII"}, {ID: 3, Name: "RENDA_FIXA"},
		{ID: 4, Name: "CRIPTOMOEDA"}, {ID: 5, Name: "ETF"},
	}
}

func apCall(t *testing.T, method, path, body string, svc *fakeAporteSvc) (int, string) {
	t.Helper()
	r := chi.NewRouter()
	r.Use(plataformaMw.Recuperar()) // igual ao main: recover global (ErrorHandler C#)
	aportes.NewHandler(svc).Mount(r)
	var req *http.Request
	if body != "" {
		req = httptest.NewRequest(method, path, bytes.NewReader([]byte(body)))
	} else {
		req = httptest.NewRequest(method, path, nil)
	}
	w := httptest.NewRecorder()
	r.ServeHTTP(w, req)
	out, _ := io.ReadAll(w.Result().Body)
	return w.Result().StatusCode, string(out)
}

// POST / — normaliza identificador; ecoa a entidade Aporte (categoria número).
func TestHandlerMovimentar(t *testing.T) {
	svc := &fakeAporteSvc{aporte: dominio.Aporte{
		ID: 1, Identificador: "PETR4", PrecoMedio: money.FromCents(1050),
		Quantidade: money.FromCents(10000), Categoria: 1,
		CriadoEm: time.Date(2025, 11, 20, 10, 30, 0, 0, time.UTC),
	}}
	status, body := apCall(t, http.MethodPost, "/api/aportes/",
		`{"identificador":"  petr4 ","quantidade":100,"preco":10.5,"categoria":1,"dataCompra":"2025-11-20"}`, svc)
	if status != 200 {
		t.Fatalf("status=%d body=%s", status, body)
	}
	if svc.movInput.Identificador != "PETR4" {
		t.Errorf("identificador = %q", svc.movInput.Identificador)
	}
	if svc.movInput.Quantidade.Cents() != 10000 || svc.movInput.Preco.Cents() != 1050 {
		t.Errorf("input = %+v", svc.movInput)
	}
	if !svc.movInput.DataCompra.Equal(time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC)) {
		t.Errorf("dataCompra = %v", svc.movInput.DataCompra)
	}
	var got dominio.Aporte
	if err := json.Unmarshal([]byte(body), &got); err != nil {
		t.Fatalf("body = %s", body)
	}
	if got.ID != 1 || got.PrecoMedio.Cents() != 1050 || got.Categoria != 1 {
		t.Errorf("aporte = %+v", got)
	}
}

// POST / — categoria fora de 1..5 → bind inválido (400).
func TestHandlerMovimentarCategoriaInvalida(t *testing.T) {
	svc := &fakeAporteSvc{}
	status, body := apCall(t, http.MethodPost, "/api/aportes/",
		`{"identificador":"PETR4","quantidade":100,"preco":10.5,"categoria":9,"dataCompra":"2025-11-20"}`, svc)
	if status != 400 || body != "{\"status\":400,\"message\":\"corpo inválido\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// POST /{identificador} — identificador da ROTA (normalizado); tipoStr cru.
func TestHandlerMovimentarPorIdentificador(t *testing.T) {
	svc := &fakeAporteSvc{aporte: dominio.Aporte{ID: 7, Identificador: "PETR4"}}
	status, body := apCall(t, http.MethodPost, "/api/aportes/petr4",
		`{"quantidade":50,"preco":null,"tipoStr":"venda","data":"2025-11-21"}`, svc)
	if status != 200 {
		t.Fatalf("status=%d body=%s", status, body)
	}
	if svc.movPorIDIdent != "PETR4" {
		t.Errorf("identificador = %q", svc.movPorIDIdent)
	}
	if svc.movPorIDInput.TipoStr != "venda" || svc.movPorIDInput.Preco != nil {
		t.Errorf("input = %+v", svc.movPorIDInput)
	}
	if !svc.movPorIDInput.Data.Equal(time.Date(2025, 11, 21, 0, 0, 0, 0, time.UTC)) {
		t.Errorf("data = %v", svc.movPorIDInput.Data)
	}
}

// POST /{identificador} — tipoStr inválido → 500 "Tipo inválido"
// (ArgumentException do getter Tipo, mensagem vazada pelo ErrorHandler).
func TestHandlerMovPorIdentTipoInvalido500(t *testing.T) {
	svc := &fakeAporteSvc{errMov: dominio.ErrTipoInvalido}
	status, body := apCall(t, http.MethodPost, "/api/aportes/PETR4",
		`{"quantidade":50,"tipoStr":"FOO","data":"2025-11-21"}`, svc)
	if status != 500 || body != "{\"status\":500,\"message\":\"Tipo inválido\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// POST /{identificador} — aporte inexistente → 400 com mensagem exata.
func TestHandlerMovPorIdentNaoEncontrado(t *testing.T) {
	svc := &fakeAporteSvc{errMov: dominio.ErrAporteNaoEncontrado}
	status, body := apCall(t, http.MethodPost, "/api/aportes/PETR4",
		`{"quantidade":50,"tipoStr":"COMPRA","data":"2025-11-21"}`, svc)
	if status != 500 || body != "{\"status\":500,\"message\":\"Aporte não encontrado\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// POST / — pânico (divisão por zero na venda total) → 500.
func TestHandlerPanicVira500(t *testing.T) {
	svc := &fakeAporteSvc{panica: true}
	status, body := apCall(t, http.MethodPost, "/api/aportes/",
		`{"identificador":"PETR4","quantidade":-100,"preco":10.5,"categoria":1,"dataCompra":"2025-11-20"}`, svc)
	// 500 com a mensagem do pânico (C#: mensagem da exceção vazada).
	if status != 500 || body != "{\"status\":500,\"message\":\"runtime error: integer divide by zero\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// GET / — lista com categoria STRING.
func TestHandlerListar(t *testing.T) {
	svc := &fakeAporteSvc{lista: []dominio.ListDTO{{
		Identificador: "PETR4", PrecoMedio: money.FromCents(1000),
		PrecoAtual: money.FromCents(1500), Quantidade: money.FromCents(10000), Categoria: "ACAO",
	}}}
	status, body := apCall(t, http.MethodGet, "/api/aportes/", "", svc)
	if status != 200 {
		t.Fatalf("status=%d body=%s", status, body)
	}
	var got []dominio.ListDTO
	if err := json.Unmarshal([]byte(body), &got); err != nil {
		t.Fatalf("body = %s", body)
	}
	if len(got) != 1 || got[0].Categoria != "ACAO" || got[0].PrecoAtual.Cents() != 1500 {
		t.Errorf("lista = %+v", got)
	}
}

// GET / — falha de cotação → 500 "Ativo não encontrado!" (BusinessError C#).
func TestHandlerListarAtivoNaoEncontrado(t *testing.T) {
	svc := &fakeAporteSvc{errList: dominio.ErrAtivoNaoEncontrado}
	status, body := apCall(t, http.MethodGet, "/api/aportes/", "", svc)
	if status != 500 || body != "{\"status\":500,\"message\":\"Ativo não encontrado!\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// GET /categorias — {id, name} do enum.
func TestHandlerCategorias(t *testing.T) {
	svc := &fakeAporteSvc{}
	status, body := apCall(t, http.MethodGet, "/api/aportes/categorias", "", svc)
	if status != 200 {
		t.Fatalf("status=%d", status)
	}
	var got []dominio.SelectValue
	if err := json.Unmarshal([]byte(body), &got); err != nil {
		t.Fatalf("body = %s", body)
	}
	if len(got) != 5 || got[0].ID != 1 || got[0].Name != "ACAO" || got[4].Name != "ETF" {
		t.Errorf("cats = %+v", got)
	}
}

// GET /{identificador}/historico — tipo STRING e preco nulo.
func TestHandlerHistorico(t *testing.T) {
	svc := &fakeAporteSvc{historico: []dominio.HistoricoDTO{{
		Identificador: "PETR4", Quantidade: money.FromCents(10000),
		Data: time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC), Tipo: "COMPRA",
	}}}
	status, body := apCall(t, http.MethodGet, "/api/aportes/PETR4/historico", "", svc)
	if status != 200 {
		t.Fatalf("status=%d body=%s", status, body)
	}
	var got []dominio.HistoricoDTO
	if err := json.Unmarshal([]byte(body), &got); err != nil {
		t.Fatalf("body = %s", body)
	}
	if len(got) != 1 || got[0].Tipo != "COMPRA" || got[0].Preco != nil {
		t.Errorf("hist = %+v", got)
	}
}
