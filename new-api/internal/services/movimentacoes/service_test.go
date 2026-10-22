package movimentacoes_test

import (
	"context"
	"errors"
	"strconv"
	"testing"
	"time"

	dominio "github.com/coigo/finn/internal/domain/movimentacoes"
	"github.com/coigo/finn/internal/platform/money"
	movimentacoes "github.com/coigo/finn/internal/services/movimentacoes"
)

// ---- fakes ----

type mRecorder struct{ eventos string }

func (r *mRecorder) add(ev string) { r.eventos += ev + ";" }

type mRepo struct {
	rec *mRecorder

	// resultados
	movRet         dominio.Movimentacao
	persistenteRet dominio.Persistente
	categorias     []dominio.Categoria
	resumos        []dominio.Resumo
	parcelas       []dominio.Parcela
	pendentesSeq   [][]dominio.Pendente // uma lista por chamada, em ordem

	// erros (sentinels do domínio)
	getErr         error
	persistenteErr error

	// gravadas
	capturedMov         dominio.Movimentacao
	capturedMovs        []dominio.Movimentacao
	capturedParcelas    []dominio.Parcela
	capturedPersistente dominio.Persistente
	capturedIDsParcelas []int64
	capturedInicio      time.Time
	capturedFim         time.Time
	pendentesArgs       []time.Time
	deletedIDs          []int64
	marcadasPagas       []int64
	capturedPersistEdit dominio.Persistente
}

func (f *mRepo) Create(_ context.Context, m dominio.Movimentacao) (dominio.Movimentacao, error) {
	f.rec.add("Create")
	f.capturedMovs = append(f.capturedMovs, m)
	f.capturedMov = m
	return m, nil
}

func (f *mRepo) Get(_ context.Context, _ int64) (dominio.Movimentacao, error) {
	f.rec.add("Get")
	if f.getErr != nil {
		return dominio.Movimentacao{}, f.getErr
	}
	return f.movRet, nil
}

func (f *mRepo) Delete(_ context.Context, id int64) error {
	f.rec.add("Delete")
	f.deletedIDs = append(f.deletedIDs, id)
	return nil
}

func (f *mRepo) CreateParcelas(_ context.Context, ps []dominio.Parcela) ([]dominio.Parcela, error) {
	f.rec.add("CreateParcelas")
	f.capturedParcelas = ps
	return ps, nil
}

func (f *mRepo) ListParcelasPorIDs(_ context.Context, ids []int64) ([]dominio.Parcela, error) {
	f.rec.add("ListParcelasPorIDs")
	f.capturedIDsParcelas = ids
	return f.parcelas, nil
}

func (f *mRepo) ListByPeriodo(_ context.Context, inicio, fim time.Time) ([]dominio.Resumo, error) {
	f.rec.add("ListByPeriodo")
	f.capturedInicio, f.capturedFim = inicio, fim
	return f.resumos, nil
}

func (f *mRepo) ListPendentesDoMes(_ context.Context, hoje time.Time) ([]dominio.Pendente, error) {
	f.rec.add("ListPendentesDoMes")
	f.pendentesArgs = append(f.pendentesArgs, hoje)
	if len(f.pendentesSeq) == 0 {
		return nil, nil
	}
	res := f.pendentesSeq[0]
	f.pendentesSeq = f.pendentesSeq[1:]
	return res, nil
}

func (f *mRepo) ListCategorias(_ context.Context) ([]dominio.Categoria, error) {
	f.rec.add("ListCategorias")
	return f.categorias, nil
}

func (f *mRepo) CreatePersistente(_ context.Context, p dominio.Persistente) (dominio.Persistente, error) {
	f.rec.add("CreatePersistente")
	f.capturedPersistente = p
	return p, nil
}

func (f *mRepo) Persistente(_ context.Context, _ int64) (dominio.Persistente, error) {
	f.rec.add("Persistente")
	if f.persistenteErr != nil {
		return dominio.Persistente{}, f.persistenteErr
	}
	return f.persistenteRet, nil
}

func (f *mRepo) EditPersistente(_ context.Context, p dominio.Persistente) error {
	f.rec.add("EditPersistente")
	f.capturedPersistEdit = p
	return nil
}

func (f *mRepo) SoftDeletePersistente(_ context.Context, id int64, _ time.Time) error {
	f.rec.add("SoftDeletePersistente")
	return nil
}

func (f *mRepo) MarkParcelaPaga(_ context.Context, id int64) error {
	f.rec.add("MarkParcelaPaga")
	f.marcadasPagas = append(f.marcadasPagas, id)
	return nil
}

type mSaldos struct {
	rec    *mRecorder
	deltas []int64
}

func (f *mSaldos) AddDelta(_ context.Context, nome string, delta money.Money) error {
	f.rec.add("AddDelta(" + nome + "," + strconv.FormatInt(delta.Cents(), 10) + ")")
	f.deltas = append(f.deltas, delta.Cents())
	return nil
}

// ---- harness ----

type svcHarness struct {
	svc  *movimentacoes.Service
	repo *mRepo
	sal  *mSaldos
	rec  *mRecorder
}

func novoHarness() *svcHarness {
	rec := &mRecorder{}
	repo := &mRepo{rec: rec}
	sal := &mSaldos{rec: rec}
	return &svcHarness{
		svc:  movimentacoes.NewService(repo, sal),
		repo: repo,
		sal:  sal,
		rec:  rec,
	}
}

func datas(t time.Time) *time.Time { return &t }
func ptrB(v bool) *bool            { return &v }
func ptrN(i int64) *int64          { return &i }

func dinheiro(v int64) money.Money { return money.FromCents(v) }

// ---- regras de Create ----

func TestCreateEntradaComumSomaCorrente(t *testing.T) {
	h := novoHarness()
	data := time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC)

	err := h.svc.Create(context.Background(), movimentacoes.CriarInput{
		Valor: dinheiro(125000), Tipo: "ENTRADA", CategoriaID: 9, Descricao: "Salário", Data: data,
	})
	if err != nil {
		t.Fatal(err)
	}
	// ordem do C#: saldo antes de inserir
	if h.rec.eventos != "AddDelta(Corrente,125000);Create;" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
	mov := h.repo.capturedMov
	if mov.Tipo != "ENTRADA" || mov.Valor.Cents() != 125000 || mov.CategoriaID != 9 || !mov.Data.Equal(data) {
		t.Errorf("mov capturada: %+v", mov)
	}
}

func TestCreateEntradaSemDeltaParaDividendoEVenda(t *testing.T) {
	for _, categoria := range []int64{10, 11} {
		h := novoHarness()
		if err := h.svc.Create(context.Background(), movimentacoes.CriarInput{
			Valor: dinheiro(1000), Tipo: "ENTRADA", CategoriaID: categoria, Descricao: "x", Data: time.Now(),
		}); err != nil {
			t.Fatal(err)
		}
		if h.rec.eventos != "Create;" {
			t.Errorf("categoria %d: eventos = %q (AddDelta não deveria ocorrer)", categoria, h.rec.eventos)
		}
	}
}

func TestCreateInvestimentosDebitaCorrente(t *testing.T) {
	h := novoHarness()
	if err := h.svc.Create(context.Background(), movimentacoes.CriarInput{
		Valor: dinheiro(700), Tipo: "INVESTIMENTOS", CategoriaID: 3, Descricao: "x", Data: time.Now(),
	}); err != nil {
		t.Fatal(err)
	}
	if h.rec.eventos != "AddDelta(Corrente,-700);Create;" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
}

func TestCreateSaidaDebitaCorrente(t *testing.T) {
	h := novoHarness()
	if err := h.svc.Create(context.Background(), movimentacoes.CriarInput{
		Valor: dinheiro(300), Tipo: "SAIDA", CategoriaID: 1, Descricao: "x", Data: time.Now(),
	}); err != nil {
		t.Fatal(err)
	}
	if h.rec.eventos != "AddDelta(Corrente,-300);Create;" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
}

func TestCreateParcelas(t *testing.T) {
	h := novoHarness()
	base := time.Date(2025, 11, 15, 0, 0, 0, 0, time.UTC)

	err := h.svc.Create(context.Background(), movimentacoes.CriarInput{
		Valor: dinheiro(10000), Tipo: "SAIDA", CategoriaID: 6, Descricao: "juju",
		Data: base, QuantidadeParcelas: ptrN(3), PrimeiroVencimento: datas(base),
	})
	if err != nil {
		t.Fatal(err)
	}
	if h.rec.eventos != "CreateParcelas;" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
	ps := h.repo.capturedParcelas
	if len(ps) != 3 {
		t.Fatalf("len parcelas = %d", len(ps))
	}
	// 10000/3 → 3333 centavos por parcela (resto perdido — paridade do decimal C#)
	if ps[0].Valor.Cents() != 3333 {
		t.Errorf("valor parcela = %d", ps[0].Valor)
	}
	if ps[0].Numero != 1 || ps[1].Numero != 2 || ps[2].Numero != 3 {
		t.Errorf("números: %d %d %d", ps[0].Numero, ps[1].Numero, ps[2].Numero)
	}
	if ps[0].Tipo != 1 { // SAIDA ordinal
		t.Errorf("tipo ordinal = %d", ps[0].Tipo)
	}
	if !ps[2].Vencimento.Equal(time.Date(2026, 1, 15, 0, 0, 0, 0, time.UTC)) {
		t.Errorf("vencimento 3 = %v (esperado 2026-01-15)", ps[2].Vencimento)
	}
	if ps[0].Paga {
		t.Error("parcelas começam não pagas")
	}
}

func TestCreatePersistente(t *testing.T) {
	h := novoHarness()
	if err := h.svc.Create(context.Background(), movimentacoes.CriarInput{
		Valor: dinheiro(8000), Tipo: "ENTRADA", CategoriaID: 9, Descricao: "pa", Data: time.Now(),
		Persistente: ptrB(true),
	}); err != nil {
		t.Fatal(err)
	}
	if h.rec.eventos != "CreatePersistente;" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
	p := h.repo.capturedPersistente
	if p.Valor.Cents() != 8000 || p.Tipo != 0 || p.CategoriaID != 9 || p.Descricao != "pa" {
		t.Errorf("persistente capturada: %+v", p)
	}
}

// ---- listas ----

func TestListByPeriodoFimInclusivo(t *testing.T) {
	h := novoHarness()
	inicio := time.Date(2025, 11, 1, 0, 0, 0, 0, time.UTC)
	fim := time.Date(2025, 11, 30, 0, 0, 0, 0, time.UTC)
	h.repo.resumos = []dominio.Resumo{{ID: 1, Tipo: "SAIDA", Categoria: "Comida e Mercado"}}

	_, err := h.svc.ListByPeriodo(context.Background(), inicio, fim)
	if err != nil {
		t.Fatal(err)
	}
	queremosFim := fim.AddDate(0, 0, 1).Add(-time.Microsecond)
	if !h.repo.capturedFim.Equal(queremosFim) {
		t.Errorf("fim repassado = %v; esperado %v", h.repo.capturedFim, queremosFim)
	}
	if !h.repo.capturedInicio.Equal(inicio) {
		t.Errorf("inicio repassado = %v", h.repo.capturedInicio)
	}
}

func TestListPendentesMesAtual(t *testing.T) {
	h := novoHarness()
	hoje := time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC)
	venc := time.Date(2025, 11, 5, 0, 0, 0, 0, time.UTC)
	h.repo.pendentesSeq = [][]dominio.Pendente{
		{{ID: 4, Valor: dinheiro(1200), Tipo: 1, TipoDerivado: "PARCELA", Vencimento: &venc}},
	}

	lista, periodo, err := h.svc.ListPendentes(context.Background(), hoje)
	if err != nil {
		t.Fatal(err)
	}
	if periodo != "MES_ATUAL" {
		t.Errorf("periodo = %q", periodo)
	}
	if len(lista) != 1 || lista[0].ID != 4 {
		t.Errorf("lista = %+v", lista)
	}
	if len(h.repo.pendentesArgs) != 1 { // não cai no mês seguinte
		t.Errorf("chamadas pendentes = %d", len(h.repo.pendentesArgs))
	}
}

func TestListPendentesProximoMes(t *testing.T) {
	h := novoHarness()
	hoje := time.Date(2025, 11, 20, 0, 0, 0, 0, time.UTC)
	h.repo.pendentesSeq = [][]dominio.Pendente{
		nil, // mês atual vazio
		{{ID: 7, Valor: dinheiro(500), Tipo: 1, TipoDerivado: "PERSISTENTE"}},
	}

	lista, periodo, err := h.svc.ListPendentes(context.Background(), hoje)
	if err != nil {
		t.Fatal(err)
	}
	if periodo != "PROXIMO_MES" {
		t.Errorf("periodo = %q", periodo)
	}
	if len(lista) != 1 || lista[0].ID != 7 {
		t.Errorf("lista = %+v", lista)
	}
	if len(h.repo.pendentesArgs) != 2 {
		t.Fatalf("chamadas = %d", len(h.repo.pendentesArgs))
	}
	esperado := hoje.AddDate(0, 1, 0)
	if !h.repo.pendentesArgs[1].Equal(esperado) {
		t.Errorf("segunda chamada usou %v; esperado %v", h.repo.pendentesArgs[1], esperado)
	}
}

func TestProcesandoPendentes(t *testing.T) {
	h := novoHarness()
	hoje := time.Date(2026, 1, 10, 0, 0, 0, 0, time.UTC)
	h.repo.pendentesSeq = [][]dominio.Pendente{
		{
			{ID: 10, Valor: dinheiro(1000), Tipo: 0, TipoDerivado: "PARCELA", Descricao: "parcela", Categoria: "x", CategoriaID: 2},
			{ID: 11, Valor: dinheiro(2000), Tipo: 1, TipoDerivado: "PERSISTENTE", Descricao: "conta", Categoria: "y", CategoriaID: 7},
		},
	}

	if err := h.svc.ProcessPendentes(context.Background(), hoje); err != nil {
		t.Fatal(err)
	}
	movs := h.repo.capturedMovs
	if len(movs) != 2 {
		t.Fatalf("movimentações criadas = %d", len(movs))
	}
	// parcela: movimentação com parcelaId, saldo +valor e marca paga
	if movs[0].ParcelaID == nil || *movs[0].ParcelaID != 10 {
		t.Errorf("parcelaId = %+v", movs[0].ParcelaID)
	}
	if len(h.sal.deltas) != 2 || h.sal.deltas[0] != 1000 {
		t.Errorf("deltas = %v", h.sal.deltas)
	}
	if len(h.repo.marcadasPagas) != 1 || h.repo.marcadasPagas[0] != 10 {
		t.Errorf("parcelas pagas: %v", h.repo.marcadasPagas)
	}
	// persistente: movimentação com persistenteId e saldo −valor (tipo 1/SAIDA)
	if movs[1].PersistenteID == nil || *movs[1].PersistenteID != 11 {
		t.Errorf("persistenteId = %+v", movs[1].PersistenteID)
	}
	if h.sal.deltas[1] != -2000 {
		t.Errorf("delta persistente = %d (esperado -2000)", h.sal.deltas[1])
	}
}

// ---- undo ----

func TestUndoEntrada(t *testing.T) {
	h := novoHarness()
	id := int64(5)
	h.repo.movRet = dominio.Movimentacao{ID: id, Valor: dinheiro(900), Tipo: "ENTRADA"}

	ret, err := h.svc.Undo(context.Background(), id)
	if err != nil {
		t.Fatal(err)
	}
	if ret != id {
		t.Errorf("ret = %d", ret)
	}
	// ordem do C#: saldo primeiro, delete depois; ENTRADA reverte com −valor
	if h.rec.eventos != "Get;AddDelta(Corrente,-900);Delete;" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
}

func TestUndoSaidaEInvestimentos(t *testing.T) {
	for _, tc := range []struct{ tipo, esperado string }{
		{"SAIDA", "Get;AddDelta(Corrente,900);Delete;"},
		{"INVESTIMENTOS", "Get;AddDelta(Corrente,900);Delete;"},
	} {
		h := novoHarness()
		h.repo.movRet = dominio.Movimentacao{ID: 6, Valor: dinheiro(900), Tipo: tc.tipo}
		if _, err := h.svc.Undo(context.Background(), 6); err != nil {
			t.Fatal(err)
		}
		if h.rec.eventos != tc.esperado {
			t.Errorf("tipo %s: eventos = %q", tc.tipo, h.rec.eventos)
		}
	}
}

func TestUndoNaoEncontrada(t *testing.T) {
	h := novoHarness()
	h.repo.getErr = dominio.ErrMovimentacaoNaoEncontrada

	_, err := h.svc.Undo(context.Background(), 42)
	if !errors.Is(err, dominio.ErrMovimentacaoNaoEncontrada) || err.Error() != "Movimentação não encontrada" {
		t.Errorf("err = %v", err)
	}
	if h.rec.eventos != "Get;" {
		t.Errorf("nada deve ser executado após Get; eventos = %q", h.rec.eventos)
	}
}

// ---- demais ----

func TestSubtrairParcelas(t *testing.T) {
	h := novoHarness()
	h.repo.parcelas = []dominio.Parcela{
		{ID: 1, Valor: dinheiro(1000)},
		{ID: 2, Valor: dinheiro(250)},
	}

	ids, err := h.svc.SubtractParcelas(context.Background(), []int64{1, 2})
	if err != nil {
		t.Fatal(err)
	}
	if h.rec.eventos != "ListParcelasPorIDs;AddDelta(Corrente,-1250);" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
	if !equalIDs(ids, []int64{1, 2}) {
		t.Errorf("ids = %v", ids)
	}
}

func TestGetPersistente(t *testing.T) {
	h := novoHarness()
	deleted := time.Date(2026, 1, 1, 0, 0, 0, 0, time.UTC)
	h.repo.persistenteRet = dominio.Persistente{ID: 3, Valor: dinheiro(444), Tipo: 1, DeletadoEm: &deleted}

	p, err := h.svc.GetPersistente(context.Background(), 3)
	if err != nil {
		t.Fatal(err)
	}
	// B6 confirmado: retorna inclusive deletada
	if p.ID != 3 || p.DeletadoEm == nil {
		t.Errorf("p = %+v", p)
	}
}

func TestGetPersistenteNaoEncontrada(t *testing.T) {
	h := novoHarness()
	h.repo.persistenteErr = dominio.ErrMovimentacaoNaoEncontradaExcl

	_, err := h.svc.GetPersistente(context.Background(), 3)
	if !errors.Is(err, dominio.ErrMovimentacaoNaoEncontradaExcl) || err.Error() != "Movimentação não encontrada!" {
		t.Errorf("err = %v", err)
	}
}

func TestEditPersistenteRepasse(t *testing.T) {
	h := novoHarness()
	p := dominio.Persistente{ID: 9, Valor: dinheiro(4321), Descricao: "nova", CategoriaID: 4}

	if err := h.svc.EditPersistente(context.Background(), p); err != nil {
		t.Fatal(err)
	}
	if h.rec.eventos != "EditPersistente;" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
	cap := h.repo.capturedPersistEdit
	if cap.ID != 9 || cap.Valor.Cents() != 4321 || cap.Descricao != "nova" || cap.CategoriaID != 4 {
		t.Errorf("persistente editada: %+v", cap)
	}
}

func TestSoftDeletePersistente(t *testing.T) {
	h := novoHarness()

	id, err := h.svc.SoftDeletePersistente(context.Background(), 77, time.Now())
	if err != nil {
		t.Fatal(err)
	}
	if id != 77 {
		t.Errorf("id = %d", id)
	}
	if h.rec.eventos != "SoftDeletePersistente;" {
		t.Errorf("eventos = %q", h.rec.eventos)
	}
}

func TestListCategoriasPassthrough(t *testing.T) {
	h := novoHarness()
	h.repo.categorias = []dominio.Categoria{
		{ID: 1, Nome: "Comida e Mercado", Tipo: "SAIDA"},
		{ID: 9, Nome: "Salário", Tipo: "ENTRADA"},
	}

	cats, err := h.svc.ListCategorias(context.Background())
	if err != nil {
		t.Fatal(err)
	}
	if len(cats) != 2 || cats[1].Nome != "Salário" {
		t.Errorf("cats = %+v", cats)
	}
}

func equalIDs(a, b []int64) bool {
	if len(a) != len(b) {
		return false
	}
	for i := range a {
		if a[i] != b[i] {
			return false
		}
	}
	return true
}
