package saldos

import (
	"context"

	dominio "github.com/coigo/finn/internal/domain/saldos"
	"github.com/coigo/finn/internal/platform/money"
)

// Service agrupa os usecases Saldos (BuscarSaldo + EditarSaldo) e expõe
// AddDelta (replica AtualizarSaldo(nome, valor) do C#) para os domínios
// movimentacoes e salarios — consumidos como interfaces definidas naqueles
// pacotes (AddDelta retorna só error, mais simples de satisfazer).
type Service struct {
	repo dominio.Repository
}

func NewService(repo dominio.Repository) *Service { return &Service{repo: repo} }

// Get replica BuscarSaldoUseCase.Execute (passa direto ao repo; o resto
// do erro — 400 vs 500 — é trabalho do handler).
func (s *Service) Get(ctx context.Context, nome string) (dominio.Saldo, error) {
	return s.repo.ByName(ctx, nome)
}

// Set replica EditarSaldoUseCase.Execute: busca por nome, então sobrescreve
// Valor. (No C# o ramo de nome inexistente é código morto — o repo lança antes.)
func (s *Service) Set(ctx context.Context, nome string, valor money.Money) (dominio.Saldo, error) {
	saldo, err := s.repo.ByName(ctx, nome)
	if err != nil {
		return dominio.Saldo{}, err
	}
	saldo.Valor = valor
	return s.repo.Set(ctx, saldo)
}

// AddDelta replica AtualizarSaldo(nome, delta): saldo.Valor += delta; retorna
// apenas erro (o Saldo retornado não é consumido por nenhum caso do C#).
func (s *Service) AddDelta(ctx context.Context, nome string, delta money.Money) error {
	_, err := s.repo.AddDelta(ctx, nome, delta)
	return err
}
