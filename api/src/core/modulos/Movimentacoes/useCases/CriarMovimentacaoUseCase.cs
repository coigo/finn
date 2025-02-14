using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Movimentacoes.Repositories;

namespace Movimentacoes.UseCases;

public class CriarMovimentacaoUseCase : IUseCase<CriarMovimentacao, Movimentacao> {

    private readonly IMovimentacaoRepository _movimentacoes;
    public CriarMovimentacaoUseCase ( IMovimentacaoRepository movimentacoes ) {
        _movimentacoes = movimentacoes;
    }

    public async Task<Movimentacao> Execute (CriarMovimentacao data) {
        
        var (valor, tipo, categoriaId, quantidadeParcelas, primeiroVencimento) = data;

        Movimentacao movimentacao = new (
            valor,
            tipo, 
            categoriaId
        );

        Movimentacao mov = await this._movimentacoes.CriarMovimentacao(movimentacao);

        if (quantidadeParcelas != null && primeiroVencimento != null ) {

            int quantidade = quantidadeParcelas.Value;
            DateTime vencimento = primeiroVencimento.Value;

            for ( int i = 0; i < quantidadeParcelas; i ++ ) {
                MovimentacaoParcela parcela = new (
                    mov, 
                    mov.Valor / quantidade,
                    i + 1,
                    vencimento.AddMonths(i)
                );

                await this._movimentacoes.CriarParcelas(parcela);
            }

        }
        return mov;
    }

}