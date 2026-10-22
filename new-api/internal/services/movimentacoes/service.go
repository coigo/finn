package movimentacoes

import (
	"context"
	"fmt"
	"time"

	dominio "github.com/coigo/finn/internal/domain/movimentacoes"
	"github.com/coigo/finn/internal/platform/money"
)

// CriarInput replica o record CriarMovimentacao do C#.
type CriarInput struct {
	Valor              money.Money
	Tipo               string // "ENTRADA"/"SAIDA"/"INVESTIMENTOS"
	CategoriaID        int64
	Descricao          string
	Data               time.Time
	QuantidadeParcelas *int64
	PrimeiroVencimento *time.Time
	Persistente        *bool
}

// Service agrupa os usecases do domínio Movimentacoes:
//   - Create ← MovimentacaoFactory + CriarMovimentacao/CriarParcela/CriarPersistente
//   - SubtractParcelas ← SubtrairParcelasUseCase (POST /parcelas)
//   - ListByPeriodo ← BuscarMovimentacoesUseCase
//   - ListPendentes ← BuscarPendentesDoMesUseCase
//   - ProcessPendentes ← ProcessarMovimentacoesPendentesUseCase
//   - Undo ← DesfazerMovimentacaoUseCase (DELETE /{id})
//   - ListCategorias ← BuscarMovimentacoesCategoriasUseCase
//   - GetPersistente ← BuscarPersistenteUseCase
//   - EditPersistente ← EditarPersistenteUseCase
//   - SoftDeletePersistente ← DeletarPersistenteUseCase
//   - CreateEntradaSalario (consumido pelo domínio salarios)
type Service struct {
	repo   dominio.Repository
	saldos dominio.SaldosUpdater
}

func NewService(repo dominio.Repository, saldos dominio.SaldosUpdater) *Service {
	return &Service{repo: repo, saldos: saldos}
}

// Create replica o roteamento do factory:
//   - quantidadeParcelas != nil      → PARCELAS (n linhas, valor/n — resto perdido, paridade)
//   - dominio.Persistente == true            → dominio.Persistente
//   - senão dominio.Movimentacao             → regras de saldo + insert
//
// No C# o factory era async void (fire-and-forget); aqui é síncrono (bug corrigido).
// Parcelas sem primeiroVencimento → NullReferenceException no C# (500);
// erro NÃO-sentinel aqui para o handler responder 500.
func (s *Service) Create(ctx context.Context, in CriarInput) error {
	if in.QuantidadeParcelas != nil {
		if in.PrimeiroVencimento == nil {
			return fmt.Errorf("criar parcelas sem primeiroVencimento")
		}
		qtd := *in.QuantidadeParcelas
		ps := make([]dominio.Parcela, 0, qtd)
		porParcela := money.FromCents(in.Valor.Cents() / qtd)
		for i := int64(0); i < qtd; i++ {
			ps = append(ps, dominio.Parcela{
				Valor:       porParcela,
				Numero:      i + 1,
				Tipo:        dominio.OrdinalDe(in.Tipo),
				CategoriaID: in.CategoriaID,
				Descricao:   in.Descricao,
				Vencimento:  in.PrimeiroVencimento.AddDate(0, int(i), 0),
			})
		}
		_, err := s.repo.CreateParcelas(ctx, ps)
		return err
	}

	if in.Persistente != nil && *in.Persistente {
		_, err := s.repo.CreatePersistente(ctx, dominio.Persistente{
			Valor:       in.Valor,
			Descricao:   in.Descricao,
			Tipo:        dominio.OrdinalDe(in.Tipo),
			CategoriaID: in.CategoriaID,
		})
		return err
	}

	mov := dominio.Movimentacao{
		Valor:       in.Valor,
		Tipo:        in.Tipo,
		CategoriaID: in.CategoriaID,
		Descricao:   in.Descricao,
		Data:        in.Data,
	}
	switch in.Tipo {
	case dominio.TipoEntrada:
		// Dividendo/Venda não movem saldo (dinheiro entra pelo próprio
		// investimento que as gerou — paridade do array do C#).
		if in.CategoriaID != dominio.CategoriaIDDividendo && in.CategoriaID != dominio.CategoriaIDVenda {
			if err := s.saldos.AddDelta(ctx, dominio.SaldoCorrente, in.Valor); err != nil {
				return err
			}
		}
	case dominio.TipoSaida, dominio.TipoInvestimentos:
		if err := s.saldos.AddDelta(ctx, dominio.SaldoCorrente, -in.Valor); err != nil {
			return err
		}
	}
	_, err := s.repo.Create(ctx, mov)
	return err
}

// SubtractParcelas replica SubtrairParcelasUseCase: soma os valores das
// parcelas e debita "Corrente" em −soma. (POST /parcelas não cria movimentação!)
func (s *Service) SubtractParcelas(ctx context.Context, ids []int64) ([]int64, error) {
	ps, err := s.repo.ListParcelasPorIDs(ctx, ids)
	if err != nil {
		return nil, err
	}
	soma := money.FromCents(0)
	for _, p := range ps {
		soma = soma.Add(p.Valor)
	}
	if err := s.saldos.AddDelta(ctx, dominio.SaldoCorrente, soma.Neg()); err != nil {
		return nil, err
	}
	return ids, nil
}

// ListByPeriodo replica BuscarMovimentacoesUseCase; fim = fim+1dia−1µs.
func (s *Service) ListByPeriodo(ctx context.Context, inicio, fim time.Time) ([]dominio.Resumo, error) {
	return s.repo.ListByPeriodo(ctx, inicio, fim.AddDate(0, 0, 1).Add(-time.Microsecond))
}

// ListPendentes replica BuscarPendentesDoMesUseCase: vazio → tenta mês
// seguinte e retorna dominio.PeriodoProximoMes.
func (s *Service) ListPendentes(ctx context.Context, hoje time.Time) ([]dominio.Pendente, string, error) {
	lista, err := s.repo.ListPendentesDoMes(ctx, hoje)
	if err != nil {
		return nil, "", err
	}
	if len(lista) > 0 {
		return lista, dominio.PeriodoMesAtual, nil
	}
	lista, err = s.repo.ListPendentesDoMes(ctx, hoje.AddDate(0, 1, 0))
	if err != nil {
		return nil, "", err
	}
	return lista, dominio.PeriodoProximoMes, nil
}

// ProcessPendentes replica ProcessarMovimentacoesPendentesUseCase (data =
// DateTime.Now no C#; aqui quem define é o handler). Δ do saldo: tipo ENTRADA
// (ordinal 0) → +valor; senão −valor. dominio.Parcela ganha ParcelaId e é marcada
// paga DEPOIS do insert; dominio.Persistente ganha PersistenteId.
func (s *Service) ProcessPendentes(ctx context.Context, hoje time.Time) error {
	pendentes, err := s.repo.ListPendentesDoMes(ctx, hoje)
	if err != nil {
		return err
	}
	if len(pendentes) == 0 {
		pendentes, err = s.repo.ListPendentesDoMes(ctx, hoje.AddDate(0, 1, 0))
		if err != nil {
			return err
		}
	}

	for _, p := range pendentes {
		entradas := p.Tipo == dominio.OrdinalEntrada
		delta := p.Valor
		if !entradas {
			delta = p.Valor.Neg()
		}
		if err := s.saldos.AddDelta(ctx, dominio.SaldoCorrente, delta); err != nil {
			return err
		}

		mov := dominio.Movimentacao{
			Valor:       p.Valor,
			Tipo:        nomeDoOrdinal(p.Tipo),
			CategoriaID: p.CategoriaID,
			Descricao:   p.Descricao,
			Data:        hoje,
		}
		switch p.TipoDerivado {
		case dominio.TipoDerivadoParcela:
			id := p.ID
			mov.ParcelaID = &id
			if _, err := s.repo.Create(ctx, mov); err != nil {
				return err
			}
			if err := s.repo.MarkParcelaPaga(ctx, p.ID); err != nil {
				return err
			}
		case dominio.TipoDerivadoPersistente:
			id := p.ID
			mov.PersistenteID = &id
			if _, err := s.repo.Create(ctx, mov); err != nil {
				return err
			}
		}
	}
	return nil
}

// Undo replica DesfazerMovimentacaoUseCase: ENTRADA→−valor; outros +valor;
// apaga a movimentação. (Ordem do C#: saldo, depois delete.)
func (s *Service) Undo(ctx context.Context, id int64) (int64, error) {
	mov, err := s.repo.Get(ctx, id)
	if err != nil {
		return 0, err
	}
	delta := mov.Valor
	if mov.Tipo == dominio.TipoEntrada {
		delta = mov.Valor.Neg()
	}
	if err := s.saldos.AddDelta(ctx, dominio.SaldoCorrente, delta); err != nil {
		return 0, err
	}
	if err := s.repo.Delete(ctx, id); err != nil {
		return 0, err
	}
	return id, nil
}

// ListCategorias replica BuscarMovimentacoesCategoriasUseCase (sem filtro
// deletadoEm — paridade).
func (s *Service) ListCategorias(ctx context.Context) ([]dominio.Categoria, error) {
	return s.repo.ListCategorias(ctx)
}

// GetPersistente replica BuscarPersistenteUseCase (retorna deletada — B6).
func (s *Service) GetPersistente(ctx context.Context, id int64) (dominio.Persistente, error) {
	return s.repo.Persistente(ctx, id)
}

// EditPersistente replica EditarPersistenteUseCase.
func (s *Service) EditPersistente(ctx context.Context, p dominio.Persistente) error {
	return s.repo.EditPersistente(ctx, p)
}

// SoftDeletePersistente replica DeletarPersistenteUseCase.
func (s *Service) SoftDeletePersistente(ctx context.Context, id int64, agora time.Time) (int64, error) {
	if err := s.repo.SoftDeletePersistente(ctx, id, agora); err != nil {
		return 0, err
	}
	return id, nil
}

// CreateEntradaSalario satisfaz salarios.MovimentacoesCreator: cria
// dominio.Movimentacao{ENTRADA, categoriaId 9, "Salário", data=agora}; NÃO mexe no
// saldo (o chamador salarios.PayCurrent soma no "Corrente" via AddDelta).
func (s *Service) CreateEntradaSalario(ctx context.Context, valor money.Money) error {
	_, err := s.repo.Create(ctx, dominio.Movimentacao{
		Valor:       valor,
		Tipo:        dominio.TipoEntrada,
		CategoriaID: dominio.CategoriaIDSalario,
		Descricao:   "Salário",
		Data:        time.Now(),
	})
	return err
}

// nomeDoOrdinal replica ToString() do enum MovimentacaoTipo.
func nomeDoOrdinal(o int64) string {
	switch o {
	case dominio.OrdinalEntrada:
		return dominio.TipoEntrada
	case dominio.OrdinalSaida:
		return dominio.TipoSaida
	case dominio.OrdinalInvestimentos:
		return dominio.TipoInvestimentos
	default:
		return ""
	}
}
