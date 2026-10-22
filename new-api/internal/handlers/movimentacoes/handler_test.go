package movimentacoes_test

import (
	"bytes"
	"context"
	"encoding/json"
	"io"
	"net/http"
	"net/http/httptest"
	"strconv"
	"testing"
	"time"

	"github.com/go-chi/chi/v5"

	dominio "github.com/coigo/finn/internal/domain/movimentacoes"
	movimentacoes "github.com/coigo/finn/internal/handlers/movimentacoes"
	"github.com/coigo/finn/internal/platform/money"
	servico "github.com/coigo/finn/internal/services/movimentacoes"
)

// fakeService satisfaz movimentacoes.ServiceAPI.
type fakeMovSvc struct {
	// Create
	in servico.CriarInput
	// SubtractParcelas
	saidaIDs []int64
	subErr   error
	// ListByPeriodo
	resumos []dominio.Resumo
	perErr  error
	// pendentes
	pend    []dominio.Pendente
	periodo string
	penErr  error
	procErr error
	// undo
	undoID  int64
	undoErr error
	// categorias
	cats   []dominio.Categoria
	catErr error
	// persistente
	per        dominio.Persistente
	perErrGet  error
	perErrEdit error
	perErrDel  error
	softID     int64
	// capturadas
	editIn     dominio.Persistente
	deleteArgs map[string]time.Time
}

func (f *fakeMovSvc) Create(_ context.Context, in servico.CriarInput) error {
	f.in = in
	return nil
}

func (f *fakeMovSvc) SubtractParcelas(_ context.Context, ids []int64) ([]int64, error) {
	if f.subErr != nil {
		return nil, f.subErr
	}
	f.saidaIDs = ids
	return ids, nil
}

func (f *fakeMovSvc) ListByPeriodo(context.Context, time.Time, time.Time) ([]dominio.Resumo, error) {
	if f.perErr != nil {
		return nil, f.perErr
	}
	return f.resumos, nil
}

func (f *fakeMovSvc) ListPendentes(context.Context, time.Time) ([]dominio.Pendente, string, error) {
	if f.penErr != nil {
		return nil, "", f.penErr
	}
	return f.pend, f.periodo, nil
}

func (f *fakeMovSvc) ProcessPendentes(context.Context, time.Time) error { return f.procErr }

func (f *fakeMovSvc) Undo(_ context.Context, id int64) (int64, error) {
	if f.undoErr != nil {
		return 0, f.undoErr
	}
	f.undoID = id
	return id, nil
}

func (f *fakeMovSvc) ListCategorias(context.Context) ([]dominio.Categoria, error) {
	if f.catErr != nil {
		return nil, f.catErr
	}
	return f.cats, nil
}

func (f *fakeMovSvc) GetPersistente(context.Context, int64) (dominio.Persistente, error) {
	if f.perErrGet != nil {
		return dominio.Persistente{}, f.perErrGet
	}
	return f.per, nil
}

func (f *fakeMovSvc) EditPersistente(_ context.Context, p dominio.Persistente) error {
	f.editIn = p
	return f.perErrEdit
}

func (f *fakeMovSvc) SoftDeletePersistente(_ context.Context, id int64, deletadoEm time.Time) (int64, error) {
	if f.perErrDel != nil {
		return 0, f.perErrDel
	}
	f.softID = id
	if f.deleteArgs == nil {
		f.deleteArgs = map[string]time.Time{}
	}
	f.deleteArgs[strconv.FormatInt(id, 10)] = deletadoEm
	return id, nil
}

func agoraFixa() time.Time { return time.Date(2026, 1, 15, 8, 30, 0, 0, time.UTC) }

func mvCall(t *testing.T, method, path, body string, svc *fakeMovSvc) (int, string) {
	t.Helper()
	ro := chi.NewRouter()
	movimentacoes.NewHandler(svc, agoraFixa).Mount(ro)

	req := httptest.NewRequest(method, path, io.NopCloser(bytes.NewReader([]byte(body))))
	req.Header.Set("Content-Type", "application/json")
	rec := httptest.NewRecorder()
	ro.ServeHTTP(rec, req)
	raw, _ := io.ReadAll(rec.Body)
	return rec.Code, string(raw)
}

// POST / — echo do request com valores parseados (data RFC3339; tipo número).
func TestHandlerCreateEchoParcelasNulas(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodPost, "/api/movimentacoes",
		`{"valor":12.5,"tipo":0,"categoriaId":1,"descricao":"mercado","data":"2025-11-20"}`, svc)

	if status != 200 {
		t.Fatalf("status = %d; body = %s", status, body)
	}
	var echo struct {
		Valor              float64 `json:"valor"`
		Tipo               int64   `json:"tipo"`
		CategoriaID        int64   `json:"categoriaId"`
		Descricao          string  `json:"descricao"`
		Data               string  `json:"data"`
		QuantidadeParcelas *int64  `json:"quantidadeParcelas"`
		PrimeiroVencimento *string `json:"primeiroVencimento"`
		Persistente        *bool   `json:"persistente"`
	}
	if err := json.Unmarshal([]byte(body), &echo); err != nil {
		t.Fatalf("body = %s", body)
	}
	if echo.Valor != 12.5 || echo.Tipo != 0 || echo.CategoriaID != 1 || echo.Data != "2025-11-20T00:00:00Z" {
		t.Errorf("echo = %s", body)
	}
	if echo.QuantidadeParcelas != nil || echo.PrimeiroVencimento != nil || echo.Persistente != nil {
		t.Errorf("campos null esperados: %s", body)
	}
	// serviço recebeu tipo string
	if svc.in.Tipo != "ENTRADA" {
		t.Errorf("svc.in.Tipo = %q", svc.in.Tipo)
	}
}

// POST / com parcelas — derivado PARCELAS repassa quantidade e vencimento.
func TestHandlerCreateComParcelas(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodPost, "/api/movimentacoes",
		`{"valor":90.0,"tipo":1,"categoriaId":6,"descricao":"juju","data":"2025-11-15","quantidadeParcelas":3,"primeiroVencimento":"2025-12-01"}`, svc)

	if status != 200 {
		t.Fatalf("status = %d; body = %s", status, body)
	}
	if svc.in.QuantidadeParcelas == nil || *svc.in.QuantidadeParcelas != 3 {
		t.Errorf("quantidadeParcelas = %+v", svc.in.QuantidadeParcelas)
	}
	if svc.in.PrimeiroVencimento == nil || !svc.in.PrimeiroVencimento.Equal(time.Date(2025, 12, 1, 0, 0, 0, 0, time.UTC)) {
		t.Errorf("primeiroVencimento = %+v", svc.in.PrimeiroVencimento)
	}
	var echo map[string]any
	_ = json.Unmarshal([]byte(body), &echo)
	// tipo ecoa como número (enum C#)
	if echo["tipo"] != float64(1) || echo["valor"] != 90.0 {
		t.Errorf("echo = %s", body)
	}
}

// POST /parcelas — echo e repasse; lista em ordem.
func TestHandlerSubtrairParcelas(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodPost, "/api/movimentacoes/parcelas", `{"parcelas":[1,2,5]}`, svc)

	if status != 200 {
		t.Fatalf("status = %d", status)
	}
	var got map[string][]float64
	_ = json.Unmarshal([]byte(body), &got)
	if len(got["parcelas"]) != 3 || got["parcelas"][0] != 1 || got["parcelas"][2] != 5 {
		t.Errorf("body = %s", body)
	}
	if svc.saidaIDs[0] != 1 || svc.saidaIDs[2] != 5 {
		t.Errorf("ids repassados = %v", svc.saidaIDs)
	}
}

// GET /?inicio&fim — lista de resumos (tipo string e categoria nome).
func TestHandlerListByPeriodo(t *testing.T) {
	svc := &fakeMovSvc{}
	svc.resumos = []dominio.Resumo{
		{ID: 2, Valor: money.FromCents(1200), Tipo: "ENTRADA", Descricao: "salário parcial", Categoria: "Dividendo", Data: time.Date(2025, 11, 5, 0, 0, 0, 0, time.UTC)},
	}
	status, body := mvCall(t, http.MethodGet, "/api/movimentacoes?inicio=2025-11-01&fim=2025-11-30", "", svc)
	if status != 200 {
		t.Fatalf("status = %d", status)
	}
	var got []struct {
		ID        float64 `json:"id"`
		Valor     float64 `json:"valor"`
		Tipo      string  `json:"tipo"`
		Descricao string  `json:"descricao"`
		Categoria string  `json:"categoria"`
		Data      string  `json:"data"`
	}
	_ = json.Unmarshal([]byte(body), &got)
	if len(got) != 1 || got[0].Valor != 12.0 || got[0].Tipo != "ENTRADA" || got[0].Categoria != "Dividendo" {
		t.Errorf("body = %s", body)
	}
	if got[0].Data != "2025-11-05T00:00:00Z" {
		t.Errorf("data = %s", got[0].Data)
	}
}

// GET / com datas inválidas → 400 {status, message:"corpo inválido"} (divergência: C# 500).
func TestHandlerListByPeriodoDatasInvalidas(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodGet, "/api/movimentacoes?inicio=abc&fim=2025-11-30", "", svc)
	if status != 400 || body != "{\"status\":400,\"message\":\"corpo inválido\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// GET /pendentes — shape {pendentes:[…], periodo}.
func TestHandlerPendentes(t *testing.T) {
	svc := &fakeMovSvc{}
	venc := time.Date(2025, 11, 12, 0, 0, 0, 0, time.UTC)
	svc.pend = []dominio.Pendente{
		{ID: 4, Valor: money.FromCents(1500), Descricao: "aluguel", Categoria: "Moradia", CategoriaID: 6, Vencimento: &venc, Tipo: 1, TipoDerivado: "PARCELA"},
	}
	svc.periodo = "MES_ATUAL"

	status, body := mvCall(t, http.MethodGet, "/api/movimentacoes/pendentes", "", svc)
	if status != 200 {
		t.Fatalf("status = %d; body = %s", status, body)
	}
	var got struct {
		Pendentes []struct {
			ID           float64 `json:"id"`
			Valor        float64 `json:"valor"`
			Descricao    string  `json:"descricao"`
			Categoria    string  `json:"categoria"`
			CategoriaID  float64 `json:"categoriaId"`
			Vencimento   *string `json:"vencimento"`
			Tipo         float64 `json:"tipo"`
			TipoDerivado string  `json:"tipoDerivado"`
		} `json:"pendentes"`
		Periodo int64 `json:"periodo"`
	}
	if err := json.Unmarshal([]byte(body), &got); err != nil {
		t.Fatalf("body = %s", body)
	}
	if got.Periodo != 0 || len(got.Pendentes) != 1 {
		t.Fatalf("body = %s", body)
	}
	p := got.Pendentes[0]
	if p.Vencimento == nil || *p.Vencimento != "2025-11-12T00:00:00Z" || p.Tipo != 1 || p.TipoDerivado != "PARCELA" {
		t.Errorf("pendentes[0] = %+v", p)
	}
}

// GET /pendentes — ramo PERSISTENTE: vencimento null.
func TestHandlerPendentesPersistenteVencimentoNulo(t *testing.T) {
	svc := &fakeMovSvc{}
	svc.pend = []dominio.Pendente{{ID: 8, Valor: money.FromCents(900), Tipo: 1, TipoDerivado: "PERSISTENTE"}}
	svc.periodo = "MES_ATUAL"

	_, body := mvCall(t, http.MethodGet, "/api/movimentacoes/pendentes", "", svc)
	if !bytes.Contains([]byte(body), []byte(`"vencimento":null`)) {
		t.Errorf("vencimento nulo esperado; body = %s", body)
	}
}

// POST /pendentes — body bool true.
func TestHandlerProcessarPendentes(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodPost, "/api/movimentacoes/pendentes", "", svc)
	if status != 200 || body != "true\n" {
		t.Errorf("status=%d body=%q", status, body)
	}
}

// DELETE /{id} (Desfazer) — body numérico com o id.
func TestHandlerUndo(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodDelete, "/api/movimentacoes/9", "", svc)
	if status != 200 || body != "9\n" {
		t.Errorf("status=%d body=%q", status, body)
	}
}

// DELETE /{id} inexistente → 500 (BusinessError → ErrorHandler default C#).
func TestHandlerUndoNaoEncontrada(t *testing.T) {
	svc := &fakeMovSvc{undoErr: dominio.ErrMovimentacaoNaoEncontrada}
	status, body := mvCall(t, http.MethodDelete, "/api/movimentacoes/9", "", svc)
	if status != 500 || body != "{\"status\":500,\"message\":\"Movimentação não encontrada\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// GET /categorias — {id, name, tipo} com tipo string.
func TestHandlerCategorias(t *testing.T) {
	svc := &fakeMovSvc{}
	svc.cats = []dominio.Categoria{
		{ID: 1, Nome: "Comida e Mercado", Tipo: "SAIDA"},
		{ID: 9, Nome: "Salário", Tipo: "ENTRADA"},
	}
	status, body := mvCall(t, http.MethodGet, "/api/movimentacoes/categorias", "", svc)
	if status != 200 {
		t.Fatalf("status = %d", status)
	}
	var got []map[string]any
	_ = json.Unmarshal([]byte(body), &got)
	if len(got) != 2 || got[0]["name"] != "Comida e Mercado" || got[1]["tipo"] != "ENTRADA" {
		t.Errorf("body = %s", body)
	}
}

// GET /persistentes/{id} — entidade crua: tipo número; deletada incluída (B6).
func TestHandlerGetPersistente(t *testing.T) {
	svc := &fakeMovSvc{}
	deleted := time.Date(2026, 1, 1, 0, 0, 0, 0, time.UTC)
	svc.per = dominio.Persistente{
		ID: 3, Valor: money.FromCents(4400), Descricao: "streaming", Tipo: 1, CategoriaID: 6,
		CriadoEm: time.Date(2025, 5, 1, 0, 0, 0, 0, time.UTC), DeletadoEm: &deleted,
	}
	status, body := mvCall(t, http.MethodGet, "/api/movimentacoes/persistentes/3", "", svc)

	var got struct {
		ID          float64 `json:"id"`
		Valor       float64 `json:"valor"`
		Descricao   string  `json:"descricao"`
		Tipo        float64 `json:"tipo"`
		CategoriaID float64 `json:"categoriaId"`
		CriadoEm    string  `json:"criadoEm"`
		DeletadoEm  string  `json:"deletadoEm"`
	}
	_ = json.Unmarshal([]byte(body), &got)
	if status != 200 || got.ID != 3 || got.Valor != 44.0 || got.Tipo != 1 || got.DeletadoEm != "2026-01-01T00:00:00Z" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

func TestHandlerGetPersistenteSemDeletado(t *testing.T) {
	svc := &fakeMovSvc{}
	svc.per = dominio.Persistente{ID: 1, Valor: money.FromCents(1000), Tipo: 2, CriadoEm: time.Date(2025, 1, 1, 0, 0, 0, 0, time.UTC)}
	_, body := mvCall(t, http.MethodGet, "/api/movimentacoes/persistentes/1", "", svc)
	if !bytes.Contains([]byte(body), []byte(`"deletadoEm":null`)) {
		t.Errorf("deletadoEm null esperado; body = %s", body)
	}
}

// GET /persistentes/{id} inexistente → 500 (BusinessError → ErrorHandler C#).
func TestHandlerGetPersistenteNaoEncontrada(t *testing.T) {
	svc := &fakeMovSvc{perErrGet: dominio.ErrMovimentacaoNaoEncontradaExcl}
	status, body := mvCall(t, http.MethodGet, "/api/movimentacoes/persistentes/3", "", svc)
	if status != 500 || body != "{\"status\":500,\"message\":\"Movimentação não encontrada!\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// PUT /persistentes — echo do request; repasse ao serviço.
func TestHandlerEditPersistente(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodPut, "/api/movimentacoes/persistentes",
		`{"persistenteId":4,"valor":50.2,"categoriaId":6,"descricao":"aluguel"}`, svc)
	if status != 200 {
		t.Fatalf("status=%d body=%s", status, body)
	}
	if svc.editIn.ID != 4 || svc.editIn.Valor.Cents() != 5020 || svc.editIn.Descricao != "aluguel" {
		t.Errorf("editIn = %+v", svc.editIn)
	}
	var echo map[string]any
	_ = json.Unmarshal([]byte(body), &echo)
	if echo["persistenteId"] != float64(4) || echo["valor"] != 50.2 {
		t.Errorf("echo = %s", body)
	}
}

// PUT persistente inexistente → C# responde 200 + echo (repo async void não
// observado); o erro mata o processo, nunca a resposta.
func TestHandlerEditPersistenteNaoEncontrada(t *testing.T) {
	svc := &fakeMovSvc{perErrEdit: dominio.ErrMovimentacaoNaoEncontradaSemAcento}
	status, body := mvCall(t, http.MethodPut, "/api/movimentacoes/persistentes", `{"persistenteId":4,"valor":1,"categoriaId":2,"descricao":"x"}`, svc)
	if status != 200 || body != "{\"persistenteId\":4,\"valor\":1,\"categoriaId\":2,\"descricao\":\"x\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}

// DELETE /persistentes/{id} — body com id; deletadoEm recebido agora fixo.
func TestHandlerSoftDeletePersistente(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodDelete, "/api/movimentacoes/persistentes/45", "", svc)
	if status != 200 || body != "45\n" {
		t.Errorf("status=%d body=%q", status, body)
	}
	if dt, ok := svc.deleteArgs["45"]; !ok || !dt.Equal(agoraFixa()) {
		t.Errorf("deletadoEm = %v", dt)
	}
}

// corpo JSON malformado → 400 (divergência: C# ValidationProblemDetails).
func TestHandlerBodyMalformado(t *testing.T) {
	svc := &fakeMovSvc{}
	status, body := mvCall(t, http.MethodPost, "/api/movimentacoes", `{valor`, svc)
	if status != 400 || body != "{\"status\":400,\"message\":\"corpo inválido\"}\n" {
		t.Errorf("status=%d body=%s", status, body)
	}
}
