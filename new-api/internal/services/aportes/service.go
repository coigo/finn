package aportes

import (
	"context"
	"fmt"
	"strings"
	"sync"
	"time"

	dominio "github.com/coigo/finn/internal/domain/aportes"
	"github.com/coigo/finn/internal/platform/money"
)

// MovimentarInput é o MovimentarAporteDTO C# já pós-bind (identificador
// normalizado pelo handler).
type MovimentarInput struct {
	Identificador string
	Quantidade    money.Money
	Preco         money.Money
	Categoria     int64
	DataCompra    time.Time
}

// MovimentarPorIdentificadorInput é o MovimentarAportePorIdentificadorDTO C#.
// TipoStr chega cru ("venda") — normalizado dentro do service (getter C#).
// Preco é opcional (decimal? no C#).
type MovimentarPorIdentificadorInput struct {
	Quantidade money.Money
	Preco      *money.Money
	TipoStr    string
	Data       time.Time
}

type Service struct {
	aportes   dominio.Repository
	historico dominio.HistoricoRepository
	prices    dominio.PriceFinder
	now       func() time.Time
}

func NewService(aportes dominio.Repository, historico dominio.HistoricoRepository, prices dominio.PriceFinder, now func() time.Time) *Service {
	if now == nil {
		now = time.Now
	}
	return &Service{aportes: aportes, historico: historico, prices: prices, now: now}
}

// Movimentar é o MovimentarAportesUseCase: registra histórico ANTES de
// criar/atualizar; tipo = quantidade < 0 ? VENDA : COMPRA; na criação valida
// o ticker na API externa (mesmo para CRIPTOMOEDA — paridade).
// O retorno replica o rastreamento do EF: aporte atualizado com os valores
// novos de precoMedio/quantidade.
func (s *Service) Movimentar(ctx context.Context, input MovimentarInput) (dominio.Aporte, error) {
	aporte, err := s.aportes.BuscarPorIdentificador(ctx, input.Identificador)
	if err != nil {
		return dominio.Aporte{}, err
	}

	tipo := dominio.TipoCOMPRA
	if input.Quantidade < 0 {
		tipo = dominio.TipoVENDA
	}
	if _, err := s.historico.CriarRegistro(ctx, dominio.AporteHistorico{
		Preco:         &input.Preco,
		Identificador: input.Identificador,
		Quantidade:    input.Quantidade,
		Categoria:     input.Categoria,
		Tipo:          tipo,
		Data:          input.DataCompra,
	}); err != nil {
		return dominio.Aporte{}, err
	}

	if aporte == nil {
		return s.criarAporte(ctx, input)
	}

	novaQtd := aporte.Quantidade.Add(input.Quantidade)
	precoMedio := precoMedioPonderado(aporte.PrecoMedio, aporte.Quantidade, input.Preco, input.Quantidade, novaQtd)
	if err := s.aportes.AtualizarAporte(ctx, aporte.ID, precoMedio, novaQtd); err != nil {
		return dominio.Aporte{}, err
	}
	return comValores(*aporte, precoMedio, novaQtd), nil
}

func (s *Service) criarAporte(ctx context.Context, input MovimentarInput) (dominio.Aporte, error) {
	// O C# valida ticker com BuscarPorTicker mesmo para CRIPTOMOEDA — manter.
	preco, err := s.prices.BuscarPorTicker(ctx, input.Identificador)
	if err != nil {
		return dominio.Aporte{}, err
	}
	if preco == nil {
		return dominio.Aporte{}, fmt.Errorf("O Ticker %s não existe! %w", input.Identificador, dominio.ErrTickerNaoExiste)
	}
	return s.aportes.CriarAporte(ctx, dominio.Aporte{
		Identificador: input.Identificador,
		PrecoMedio:    input.Preco,
		Quantidade:    input.Quantidade,
		Categoria:     input.Categoria,
		CriadoEm:      s.now(),
	})
}

// MovimentarPorIdentificador é o MovimentarAportesPorIdentificadorUseCase:
// aporte deve existir; tipoStr inválido é erro NÃO-sentinel (ArgumentException
// → 500); atualiza ANTES de gravar o histórico (categoria do aporte original).
func (s *Service) MovimentarPorIdentificador(ctx context.Context, identificador string, input MovimentarPorIdentificadorInput) (dominio.Aporte, error) {
	aporte, err := s.aportes.BuscarPorIdentificador(ctx, identificador)
	if err != nil {
		return dominio.Aporte{}, err
	}
	if aporte == nil {
		return dominio.Aporte{}, dominio.ErrAporteNaoEncontrado
	}

	if input.Quantidade <= 0 { // primeiro do CalcularQuantidade do C#
		return dominio.Aporte{}, dominio.ErrQuantidadeInvalida
	}
	var novaQtd money.Money
	switch strings.ToUpper(input.TipoStr) { // getter do DTO normaliza
	case dominio.TipoCOMPRA:
		novaQtd = aporte.Quantidade.Add(input.Quantidade)
	case dominio.TipoVENDA:
		novaQtd = aporte.Quantidade.Sub(input.Quantidade)
	case dominio.TipoDESDOBRAMENTO:
		novaQtd = aporte.Quantidade.Mul(input.Quantidade)
	default:
		return dominio.Aporte{}, dominio.ErrTipoInvalido
	}

	precoMedio, err := calcularPrecoMedioPorIdentificador(input, *aporte, novaQtd)
	if err != nil {
		return dominio.Aporte{}, err
	}

	if err := s.aportes.AtualizarAporte(ctx, aporte.ID, precoMedio, novaQtd); err != nil {
		return dominio.Aporte{}, err
	}

	if _, err := s.historico.CriarRegistro(ctx, dominio.AporteHistorico{
		Preco:         input.Preco,
		Identificador: identificador,
		Quantidade:    input.Quantidade,
		Categoria:     aporte.Categoria,
		Tipo:          strings.ToUpper(input.TipoStr),
		Data:          input.Data,
	}); err != nil {
		return dominio.Aporte{}, err
	}
	return comValores(*aporte, precoMedio, novaQtd), nil
}

func calcularPrecoMedioPorIdentificador(input MovimentarPorIdentificadorInput, aporte dominio.Aporte, novaQtd money.Money) (money.Money, error) {
	if novaQtd < 0 { // primeiro do CalcularPrecoMedio do C#
		return 0, dominio.ErrPrecoMedioInvalido
	}
	switch strings.ToUpper(input.TipoStr) {
	case dominio.TipoCOMPRA:
		if input.Preco == nil { // decimal? * decimal → null → "Preço médio inválido"
			return 0, dominio.ErrPrecoMedioInvalido
		}
		return precoMedioPonderado(aporte.PrecoMedio, aporte.Quantidade, *input.Preco, input.Quantidade, novaQtd), nil
	case dominio.TipoVENDA:
		if novaQtd == 0 {
			return money.FromCents(0), nil
		}
		return aporte.PrecoMedio, nil
	case dominio.TipoDESDOBRAMENTO:
		return aporte.PrecoMedio.Mul(aporte.Quantidade.Div(novaQtd)), nil
	default:
		return aporte.PrecoMedio, nil
	}
}

// List é o BuscarAportesUseCase com B4: cotações em paralelo; RENDA_FIXA usa
// o próprio preço médio; falha de cotação aborta a listagem (ErrAtivoNaoEncontrado).
func (s *Service) List(ctx context.Context) ([]dominio.ListDTO, error) {
	lista, err := s.aportes.BuscarTodos(ctx)
	if err != nil {
		return nil, err
	}

	precos := make([]money.Money, len(lista))
	errs := make([]error, len(lista))
	var wg sync.WaitGroup
	for i, a := range lista {
		if !beanBuscaExterna(a.Categoria) {
			precos[i] = a.PrecoMedio // RENDA_FIXA: preço médio, sem cotação
			continue
		}
		wg.Add(1)
		go func(i int, a dominio.Aporte) {
			defer wg.Done()
			var preco *money.Money
			var err error
			if a.Categoria == dominio.CategoriaCRIPTOMOEDA {
				preco, err = s.prices.BuscarCrypto(ctx, a.Identificador)
			} else {
				preco, err = s.prices.BuscarPorTicker(ctx, a.Identificador)
			}
			if err != nil {
				errs[i] = err
				return
			}
			if preco == nil {
				errs[i] = dominio.ErrAtivoNaoEncontrado
				return
			}
			precos[i] = *preco
		}(i, a)
	}
	wg.Wait()
	for _, err := range errs {
		if err != nil {
			return nil, err
		}
	}

	dtos := make([]dominio.ListDTO, len(lista))
	for i, a := range lista {
		dtos[i] = dominio.ListDTO{
			Identificador: a.Identificador,
			PrecoMedio:    a.PrecoMedio,
			PrecoAtual:    precos[i],
			Quantidade:    a.Quantidade,
			Categoria:     dominio.CategoriaNome(a.Categoria),
		}
	}
	return dtos, nil
}

func beanBuscaExterna(categoria int64) bool {
	switch categoria {
	case dominio.CategoriaACAO, dominio.CategoriaETF, dominio.CategoriaFII, dominio.CategoriaCRIPTOMOEDA:
		return true
	}
	return false
}

// ListHistorico é o BuscarAportesHistoricoUseCase (identificador cru).
func (s *Service) ListHistorico(ctx context.Context, identificador string) ([]dominio.HistoricoDTO, error) {
	return s.aportes.BuscarDetalhesDoAporte(ctx, identificador)
}

// Categorias é o BuscarCategoriasUseCase (sync, sem erro).
func (s *Service) Categorias() []dominio.SelectValue {
	return []dominio.SelectValue{
		{ID: dominio.CategoriaACAO, Name: "ACAO"},
		{ID: dominio.CategoriaFII, Name: "FII"},
		{ID: dominio.CategoriaRENDAFIXA, Name: "RENDA_FIXA"},
		{ID: dominio.CategoriaCRIPTOMOEDA, Name: "CRIPTOMOEDA"},
		{ID: dominio.CategoriaETF, Name: "ETF"},
	}
}

// comValores replica o efeito do rastreamento do EF: mesma entidade com
// precoMedio/quantidade novos. (No C# era método da entidade; aqui função
// livre — o tipo do domínio não é deste pacote.)
func comValores(a dominio.Aporte, precoMedio, quantidade money.Money) dominio.Aporte {
	a.PrecoMedio = precoMedio
	a.Quantidade = quantidade
	return a
}

// precoMedioPonderado replica ((PM*antiga) + (preco*entrada)) / nova.
// Divisão por zero panica (paridade com C#).
func precoMedioPonderado(pmAntigo, qtdAntiga, preco, entrada, novaQtd money.Money) money.Money {
	return pmAntigo.Mul(qtdAntiga).Add(preco.Mul(entrada)).Div(novaQtd)
}
