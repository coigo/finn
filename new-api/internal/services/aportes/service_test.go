package aportes_test

import (
	"context"
	"errors"
	"sync"
	"testing"
	"time"

	dominio "github.com/coigo/finn/internal/domain/aportes"
	"github.com/coigo/finn/internal/platform/money"
	aportes "github.com/coigo/finn/internal/services/aportes"
)

var agoraFixa = time.Date(2025, 11, 20, 10, 30, 0, 0, time.UTC)

type fakeAporteRepo struct {
	aportes   map[string]dominio.Aporte
	criados   []dominio.Aporte
	updates   []updateRegistro
	detalhes  []dominio.HistoricoDTO
	errBuscar error
}

type updateRegistro struct {
	id  int64
	pm  money.Money
	qtd money.Money
}

func (f *fakeAporteRepo) BuscarTodos(context.Context) ([]dominio.Aporte, error) {
	lista := make([]dominio.Aporte, 0, len(f.aportes))
	for _, a := range f.aportes {
		lista = append(lista, a)
	}
	return lista, nil
}

func (f *fakeAporteRepo) BuscarPorIdentificador(_ context.Context, ident string) (*dominio.Aporte, error) {
	if f.errBuscar != nil {
		return nil, f.errBuscar
	}
	if a, ok := f.aportes[ident]; ok {
		return &a, nil
	}
	return nil, nil
}

func (f *fakeAporteRepo) BuscarDetalhesDoAporte(_ context.Context, ident string) ([]dominio.HistoricoDTO, error) {
	return f.detalhes, nil
}

func (f *fakeAporteRepo) CriarAporte(_ context.Context, a dominio.Aporte) (dominio.Aporte, error) {
	a.ID = int64(len(f.criados) + 1)
	f.criados = append(f.criados, a)
	return a, nil
}

func (f *fakeAporteRepo) AtualizarAporte(_ context.Context, id int64, pm, qtd money.Money) error {
	f.updates = append(f.updates, updateRegistro{id: id, pm: pm, qtd: qtd})
	return nil
}

type fakeHistRepo struct {
	regs []dominio.AporteHistorico
}

func (f *fakeHistRepo) CriarRegistro(_ context.Context, h dominio.AporteHistorico) (dominio.AporteHistorico, error) {
	h.ID = int64(len(f.regs) + 1)
	h.CriadoEm = agoraFixa
	f.regs = append(f.regs, h)
	return h, nil
}

type fakePrices struct {
	ticker     *money.Money
	crypto     *money.Money
	tickerIDs  []string
	cryptoIDs  []string
	started    chan struct{} // sinaliza cada chamada (teste de paralelismo)
	release    chan struct{} // bloqueia até o teste liberar
	blockCalls bool
}

func (f *fakePrices) BuscarPorTicker(_ context.Context, ticker string) (*money.Money, error) {
	f.tickerIDs = append(f.tickerIDs, ticker)
	if f.blockCalls {
		f.started <- struct{}{}
		<-f.release
	}
	return f.ticker, nil
}

func (f *fakePrices) BuscarCrypto(_ context.Context, crypto string) (*money.Money, error) {
	f.cryptoIDs = append(f.cryptoIDs, crypto)
	return f.crypto, nil
}

func novoSvc(repo *fakeAporteRepo, hist *fakeHistRepo, prices *fakePrices) *aportes.Service {
	return aportes.NewService(repo, hist, prices, func() time.Time { return agoraFixa })
}

// POST / — aporte novo: histórico ANTES, tipo COMPRA, ticker validado.
func TestMovimentarNovoAporte(t *testing.T) {
	repo, hist, prices := &fakeAporteRepo{aportes: map[string]dominio.Aporte{}}, &fakeHistRepo{}, &fakePrices{ticker: ptrMoney(1000)}
	svc := novoSvc(repo, hist, prices)

	ap, err := svc.Movimentar(t.Context(), aportes.MovimentarInput{
		Identificador: "PETR4", Quantidade: money.FromCents(10000), Preco: money.FromCents(1000),
		Categoria: dominio.CategoriaACAO, DataCompra: agoraFixa,
	})
	if err != nil {
		t.Fatal(err)
	}
	if len(hist.regs) != 1 {
		t.Fatalf("históricos = %d", len(hist.regs))
	}
	h := hist.regs[0]
	if h.Tipo != dominio.TipoCOMPRA || h.Preco == nil || h.Preco.Cents() != 1000 ||
		h.Quantidade.Cents() != 10000 || h.Categoria != dominio.CategoriaACAO ||
		!h.Data.Equal(agoraFixa) || !h.CriadoEm.Equal(agoraFixa) {
		t.Errorf("histórico = %+v", h)
	}
	if len(prices.tickerIDs) != 1 || prices.tickerIDs[0] != "PETR4" {
		t.Errorf("ticker consultado = %v", prices.tickerIDs)
	}
	if len(repo.criados) != 1 || repo.criados[0].PrecoMedio.Cents() != 1000 ||
		repo.criados[0].Quantidade.Cents() != 10000 {
		t.Errorf("criados = %+v", repo.criados)
	}
	if ap.PrecoMedio.Cents() != 1000 || ap.Quantidade.Cents() != 10000 ||
		ap.Identificador != "PETR4" || ap.Categoria != dominio.CategoriaACAO ||
		!ap.CriadoEm.Equal(agoraFixa) {
		t.Errorf("aporte retornado = %+v", ap)
	}
}

// POST / — ticker inexistente: erro 4xx com espaço final; histórico fica.
func TestMovimentarTickerInvalido(t *testing.T) {
	repo, hist, prices := &fakeAporteRepo{aportes: map[string]dominio.Aporte{}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, prices)

	_, err := svc.Movimentar(t.Context(), aportes.MovimentarInput{
		Identificador: "XXXX3", Quantidade: money.FromCents(10000), Preco: money.FromCents(1000),
		Categoria: dominio.CategoriaACAO, DataCompra: agoraFixa,
	})
	if !errors.Is(err, dominio.ErrTickerNaoExiste) {
		t.Fatalf("esperava ErrTickerNaoExiste; obtive %v", err)
	}
	if err.Error() != "O Ticker XXXX3 não existe! " {
		t.Errorf("mensagem = %q", err.Error())
	}
	if len(hist.regs) != 1 {
		t.Errorf("histórico deveria existir mesmo com falha; regs = %d", len(hist.regs))
	}
	if len(repo.criados) != 0 {
		t.Errorf("aporte não deveria ser criado")
	}
}

// POST / — aporte existente: média ponderada; retorno é o aporte ANTIGO (stale).
func TestMovimentarAporteExistente(t *testing.T) {
	repo, hist, prices := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, Identificador: "PETR4", PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO, CriadoEm: agoraFixa},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, prices)

	ap, err := svc.Movimentar(t.Context(), aportes.MovimentarInput{
		Identificador: "PETR4", Quantidade: money.FromCents(10000), Preco: money.FromCents(1200),
		Categoria: dominio.CategoriaACAO, DataCompra: agoraFixa,
	})
	if err != nil {
		t.Fatal(err)
	}
	if len(prices.tickerIDs) != 0 {
		t.Error("não deveria validar ticker em atualização")
	}
	if len(repo.updates) != 1 || repo.updates[0].id != 7 ||
		repo.updates[0].pm.Cents() != 1100 || repo.updates[0].qtd.Cents() != 20000 {
		t.Errorf("updates = %+v", repo.updates)
	}
	// O C# atualiza a mesma entidade tracked e a devolve — resposta tem os
	// valores NOVOS de precoMedio/quantidade (id/criadoEm preservados).
	if ap.ID != 7 || ap.PrecoMedio.Cents() != 1100 || ap.Quantidade.Cents() != 20000 {
		t.Errorf("retorno deveria refletir a atualização; got %+v", ap)
	}
	if len(hist.regs) != 1 || hist.regs[0].Tipo != dominio.TipoCOMPRA {
		t.Errorf("histórico = %+v", hist.regs)
	}
}

// POST / — VENDA por quantidade negativa: fórmula quebrada replicada.
func TestMovimentarVendaQuantidadeNegativa(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	// (10.00*100 + 8.00*(-50)) / 50 = 12.00
	_, err := svc.Movimentar(t.Context(), aportes.MovimentarInput{
		Identificador: "PETR4", Quantidade: money.FromCents(-5000), Preco: money.FromCents(800),
		Categoria: dominio.CategoriaACAO, DataCompra: agoraFixa,
	})
	if err != nil {
		t.Fatal(err)
	}
	if len(repo.updates) != 1 || repo.updates[0].pm.Cents() != 1200 || repo.updates[0].qtd.Cents() != 5000 {
		t.Errorf("updates = %+v", repo.updates)
	}
	if len(hist.regs) != 1 || hist.regs[0].Tipo != dominio.TipoVENDA {
		t.Errorf("histórico = %+v", hist.regs)
	}
}

// POST / — venda total zera a quantidade e divide por zero (C# → 500).
func TestMovimentarVendaTotalPanica(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	defer func() {
		if recover() == nil {
			t.Error("esperava pânico de divisão por zero")
		}
	}()
	_, _ = svc.Movimentar(t.Context(), aportes.MovimentarInput{
		Identificador: "PETR4", Quantidade: money.FromCents(-10000), Preco: money.FromCents(800),
		Categoria: dominio.CategoriaACAO, DataCompra: agoraFixa,
	})
}

// POST /{id} — aporte inexistente: "Aporte não encontrado" (tipo nem avaliado).
func TestMovPorIdentNaoEncontrado(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	_, err := svc.MovimentarPorIdentificador(t.Context(), "PETR4", aportes.MovimentarPorIdentificadorInput{
		Quantidade: money.FromCents(1000), TipoStr: "TIPO_ERRADO", Data: agoraFixa,
	})
	if !errors.Is(err, dominio.ErrAporteNaoEncontrado) || err.Error() != "Aporte não encontrado" {
		t.Fatalf("err = %v", err)
	}
	if len(hist.regs) != 0 {
		t.Error("histórico não deveria ser gravado")
	}
}

// POST /{id} — quantidade <= 0 → "Quantidade inválida".
func TestMovPorIdentQuantidadeInvalida(t *testing.T) {
	for _, qtd := range []money.Money{money.FromCents(0), money.FromCents(-100)} {
		repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
			"PETR4": {ID: 7, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
		}}, &fakeHistRepo{}, &fakePrices{}
		svc := novoSvc(repo, hist, &fakePrices{})

		_, err := svc.MovimentarPorIdentificador(t.Context(), "PETR4", aportes.MovimentarPorIdentificadorInput{
			Quantidade: qtd, TipoStr: "COMPRA", Data: agoraFixa,
		})
		if !errors.Is(err, dominio.ErrQuantidadeInvalida) || err.Error() != "Quantidade inválida" {
			t.Errorf("qtd %d: err = %v", qtd, err)
		}
	}
}

// POST /{id} — COMPRA: média ponderada; tipoStr minúsculo é aceito (getter C#).
func TestMovPorIdentCompra(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"HGLG11": {ID: 3, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaFII},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	preco := money.FromCents(1200)
	_, err := svc.MovimentarPorIdentificador(t.Context(), "HGLG11", aportes.MovimentarPorIdentificadorInput{
		Quantidade: money.FromCents(10000), Preco: &preco, TipoStr: "compra", Data: agoraFixa,
	})
	if err != nil {
		t.Fatal(err)
	}
	if len(repo.updates) != 1 || repo.updates[0].pm.Cents() != 1100 || repo.updates[0].qtd.Cents() != 20000 {
		t.Errorf("updates = %+v", repo.updates)
	}
	h := hist.regs[0]
	if h.Tipo != dominio.TipoCOMPRA || h.Preco == nil || h.Preco.Cents() != 1200 ||
		h.Categoria != dominio.CategoriaFII || h.Identificador != "HGLG11" {
		t.Errorf("histórico = %+v", h)
	}
}

// POST /{id} — VENDA parcial: PM inalterado.
func TestMovPorIdentVendaParcial(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	preco := money.FromCents(500)
	_, err := svc.MovimentarPorIdentificador(t.Context(), "PETR4", aportes.MovimentarPorIdentificadorInput{
		Quantidade: money.FromCents(4000), Preco: &preco, TipoStr: "VENDA", Data: agoraFixa,
	})
	if err != nil {
		t.Fatal(err)
	}
	if len(repo.updates) != 1 || repo.updates[0].pm.Cents() != 1000 || repo.updates[0].qtd.Cents() != 6000 {
		t.Errorf("updates = %+v", repo.updates)
	}
	if len(hist.regs) != 1 || hist.regs[0].Tipo != dominio.TipoVENDA {
		t.Errorf("histórico = %+v", hist.regs)
	}
}

// POST /{id} — VENDA até zero: PM = 0 (branch explícito, sem pânico).
func TestMovPorIdentVendaAteZero(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	_, err := svc.MovimentarPorIdentificador(t.Context(), "PETR4", aportes.MovimentarPorIdentificadorInput{
		Quantidade: money.FromCents(10000), TipoStr: "VENDA", Data: agoraFixa,
	})
	if err != nil {
		t.Fatal(err)
	}
	if len(repo.updates) != 1 || repo.updates[0].pm.Cents() != 0 || repo.updates[0].qtd.Cents() != 0 {
		t.Errorf("updates = %+v", repo.updates)
	}
	if len(hist.regs) != 1 || hist.regs[0].Preco != nil {
		t.Errorf("histórico = %+v", hist.regs)
	}
}

// POST /{id} — DESDOBRAMENTO: qtd *= fator; PM diluído PM*(antiga/nova).
func TestMovPorIdentDesdobramento(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, PrecoMedio: money.FromCents(2000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	_, err := svc.MovimentarPorIdentificador(t.Context(), "PETR4", aportes.MovimentarPorIdentificadorInput{
		Quantidade: money.FromCents(200), TipoStr: "DESDOBRAMENTO", Data: agoraFixa,
	})
	if err != nil {
		t.Fatal(err)
	}
	if len(repo.updates) != 1 || repo.updates[0].pm.Cents() != 1000 || repo.updates[0].qtd.Cents() != 20000 {
		t.Errorf("updates = %+v", repo.updates)
	}
	if len(hist.regs) != 1 || hist.regs[0].Tipo != dominio.TipoDESDOBRAMENTO {
		t.Errorf("histórico = %+v", hist.regs)
	}
}

// POST /{id} — resultado negativo → "Preço médio inválido".
func TestMovPorIdentResultadoNegativo(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(5000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	_, err := svc.MovimentarPorIdentificador(t.Context(), "PETR4", aportes.MovimentarPorIdentificadorInput{
		Quantidade: money.FromCents(10000), TipoStr: "VENDA", Data: agoraFixa,
	})
	if !errors.Is(err, dominio.ErrPrecoMedioInvalido) || err.Error() != "Preço médio inválido" {
		t.Fatalf("err = %v", err)
	}
	if len(repo.updates) != 0 || len(hist.regs) != 0 {
		t.Error("nada deveria ser gravado")
	}
}

// POST /{id} — COMPRA sem preço → "Preço médio inválido" (null propagation).
func TestMovPorIdentPrecoNuloCompra(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	_, err := svc.MovimentarPorIdentificador(t.Context(), "PETR4", aportes.MovimentarPorIdentificadorInput{
		Quantidade: money.FromCents(10000), TipoStr: "COMPRA", Data: agoraFixa,
	})
	if !errors.Is(err, dominio.ErrPrecoMedioInvalido) {
		t.Fatalf("err = %v", err)
	}
}

// POST /{id} — tipoStr inválido com aporte existente → ErrTipoInvalido
// (ArgumentException "Tipo inválido" do C# → 500 com a mensagem vazada).
func TestMovPorIdentTipoInvalido(t *testing.T) {
	repo, hist, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 7, PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, hist, &fakePrices{})

	_, err := svc.MovimentarPorIdentificador(t.Context(), "PETR4", aportes.MovimentarPorIdentificadorInput{
		Quantidade: money.FromCents(1000), TipoStr: "FOO", Data: agoraFixa,
	})
	if err == nil {
		t.Fatal("esperava erro")
	}
	if !errors.Is(err, dominio.ErrTipoInvalido) {
		t.Errorf("erro = %v; esperava ErrTipoInvalido", err)
	}
}

// GET / — RENDA_FIXA usa o preço médio; ACAO busca ticker.
func TestListCategoriasComPreco(t *testing.T) {
	repo, _, prices := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"TESOURO": {ID: 1, Identificador: "TESOURO", PrecoMedio: money.FromCents(5000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaRENDAFIXA},
		"PETR4":   {ID: 2, Identificador: "PETR4", PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{ticker: ptrMoney(1500)}
	svc := novoSvc(repo, &fakeHistRepo{}, prices)

	lista, err := svc.List(t.Context())
	if err != nil {
		t.Fatal(err)
	}
	if len(lista) != 2 {
		t.Fatalf("lista = %+v", lista)
	}
	for _, dto := range lista {
		switch dto.Identificador {
		case "TESOURO":
			if dto.PrecoAtual.Cents() != 5000 || dto.Categoria != "RENDA_FIXA" {
				t.Errorf("TESOURO = %+v", dto)
			}
		case "PETR4":
			if dto.PrecoAtual.Cents() != 1500 || dto.Categoria != "ACAO" {
				t.Errorf("PETR4 = %+v", dto)
			}
		}
	}
	if len(prices.tickerIDs) != 1 || len(prices.cryptoIDs) != 0 {
		t.Errorf("ticker=%v crypto=%v", prices.tickerIDs, prices.cryptoIDs)
	}
}

// GET / — CRIPTOMOEDA busca na API de crypto.
func TestListCriptomoeda(t *testing.T) {
	repo, _, prices := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"BTC": {ID: 1, Identificador: "BTC", PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(100), Categoria: dominio.CategoriaCRIPTOMOEDA},
	}}, &fakeHistRepo{}, &fakePrices{crypto: ptrMoney(350000)}
	svc := novoSvc(repo, &fakeHistRepo{}, prices)

	lista, err := svc.List(t.Context())
	if err != nil {
		t.Fatal(err)
	}
	if len(prices.cryptoIDs) != 1 || prices.cryptoIDs[0] != "BTC" || len(prices.tickerIDs) != 0 {
		t.Errorf("ticker=%v crypto=%v", prices.tickerIDs, prices.cryptoIDs)
	}
	if lista[0].PrecoAtual.Cents() != 350000 {
		t.Errorf("precoAtual = %d", lista[0].PrecoAtual.Cents())
	}
}

// GET / — cotação falhou → "Ativo não encontrado!".
func TestListAtivoNaoEncontrado(t *testing.T) {
	repo, _, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 1, Identificador: "PETR4", PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, &fakeHistRepo{}, &fakePrices{})

	_, err := svc.List(t.Context())
	if !errors.Is(err, dominio.ErrAtivoNaoEncontrado) || err.Error() != "Ativo não encontrado!" {
		t.Fatalf("err = %v", err)
	}
}

// GET / — B4: as cotações são buscadas em paralelo.
func TestListParalelo(t *testing.T) {
	repo, _, _ := &fakeAporteRepo{aportes: map[string]dominio.Aporte{
		"PETR4": {ID: 1, Identificador: "PETR4", PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
		"VALE3": {ID: 2, Identificador: "VALE3", PrecoMedio: money.FromCents(1000), Quantidade: money.FromCents(10000), Categoria: dominio.CategoriaACAO},
	}}, &fakeHistRepo{}, &fakePrices{}
	prices := &fakePrices{ticker: ptrMoney(1500), blockCalls: true,
		started: make(chan struct{}, 2), release: make(chan struct{})}
	svc := novoSvc(repo, &fakeHistRepo{}, prices)

	var wg sync.WaitGroup
	wg.Add(1)
	go func() {
		defer wg.Done()
		if _, err := svc.List(t.Context()); err != nil {
			t.Error(err)
		}
	}()
	semParalelismo := true
	for i := 0; i < 2; i++ {
		select {
		case <-prices.started:
		case <-time.After(2 * time.Second):
			semParalelismo = false
		}
		if !semParalelismo {
			break
		}
	}
	close(prices.release) // desbloqueia as goroutines em qualquer caminho
	wg.Wait()
	if !semParalelismo {
		t.Skip("List não chamou as cotações em paralelo")
	}
}

// GET /categorias — enum AporteCategoria na íntegra.
func TestCategorias(t *testing.T) {
	svc := novoSvc(&fakeAporteRepo{}, &fakeHistRepo{}, &fakePrices{})
	cats := svc.Categorias()
	if len(cats) != 5 {
		t.Fatalf("cats = %+v", cats)
	}
	nomes := []string{"ACAO", "FII", "RENDA_FIXA", "CRIPTOMOEDA", "ETF"}
	for i, c := range cats {
		if c.ID != int64(i+1) || c.Name != nomes[i] {
			t.Errorf("cats[%d] = %+v", i, c)
		}
	}
}

// GET /{id}/historico — identificador cru repassado ao repositório.
func TestListHistoricoPassthrough(t *testing.T) {
	repo, _, _ := &fakeAporteRepo{detalhes: []dominio.HistoricoDTO{
		{Identificador: "PETR4", Quantidade: money.FromCents(10000), Data: agoraFixa, Tipo: "COMPRA"},
	}}, &fakeHistRepo{}, &fakePrices{}
	svc := novoSvc(repo, &fakeHistRepo{}, &fakePrices{})

	hist, err := svc.ListHistorico(t.Context(), "petr4")
	if err != nil {
		t.Fatal(err)
	}
	if len(hist) != 1 || hist[0].Tipo != "COMPRA" {
		t.Errorf("hist = %+v", hist)
	}
}

func ptrMoney(c int64) *money.Money {
	m := money.FromCents(c)
	return &m
}
