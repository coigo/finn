package salarios

import (
	"context"
	"fmt"
	"time"

	dominio "github.com/coigo/finn/internal/domain/salarios"
	"github.com/coigo/finn/internal/platform/money"
)

const saldoCorrente = "Corrente"

// Service agrupa os usecases Salarios:
//   - Create     ← CriarSalarioUseCase
//   - GetCurrent ← BuscarSalarioAtualUseCase
//   - PayCurrent ← AdicionarSalarioAtualUseCase
type Service struct {
	repo dominio.Repository
	// dependências de outros domínios (interfaces acima, injetadas no main).
	saldos dominio.SaldosUpdater
	movs   dominio.MovimentacoesCreator
}

func NewService(repo dominio.Repository, saldos dominio.SaldosUpdater, movs dominio.MovimentacoesCreator) *Service {
	return &Service{repo: repo, saldos: saldos, movs: movs}
}

// Create replica CriarSalarioUseCase: valor < 0 → ErrValorNegativo; senão cria.
// O C# não valida valor == 0 (só < 0) — paridade mantida.
func (s *Service) Create(ctx context.Context, valor money.Money) (dominio.Salario, error) {
	if valor < 0 {
		return dominio.Salario{}, dominio.ErrValorNegativo
	}
	return s.repo.Create(ctx, dominio.Salario{Valor: valor, CriadoEm: time.Now()})
}

// GetCurrent replica BuscarSalarioAtualUseCase (último por id;
// banco vazio → ErrSalarioNaoCadastrado, mapeado no store).
func (s *Service) GetCurrent(ctx context.Context) (dominio.Salario, error) {
	return s.repo.Last(ctx)
}

// PayCurrent replica AdicionarSalarioAtualUseCase: pega o último salário,
// soma no saldo "Corrente" e cria a movimentação ENTRADA (saldo primeiro).
// Sem salário o C# estoura NullReferenceException → 500; aqui erro
// NÃO-sentinel para o handler não mapear 400.
func (s *Service) PayCurrent(ctx context.Context) (dominio.Salario, error) {
	salario, err := s.repo.Last(ctx)
	if err != nil {
		return dominio.Salario{}, fmt.Errorf("pagamento sem salário cadastrado (%v)", err)
	}
	if err := s.saldos.AddDelta(ctx, saldoCorrente, salario.Valor); err != nil {
		return dominio.Salario{}, err
	}
	if err := s.movs.CreateEntradaSalario(ctx, salario.Valor); err != nil {
		return dominio.Salario{}, err
	}
	return salario, nil
}
