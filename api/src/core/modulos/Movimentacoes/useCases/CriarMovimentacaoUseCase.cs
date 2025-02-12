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
        
        var (valor, tipo, categoria, quantidadeParcelas, primeiroVencimento) = data;

        Movimentacao movimentacao = new (
            valor,
            tipo, 
            categoria
        );

        Movimentacao mov = await this._movimentacoes.CriarMovimentacao(movimentacao);

        if (quantidadeParcelas != null ) {

            int quantidade = quantidadeParcelas.Value;
            
            // for ( int i = 0; i < quantidadeParcelas; i ++ ) {
            //     MovimentacaoParcela parcela = new (
            //         mov.Id, 
            //         mov.Valor / quantidade,
            //         i,
            //         primeiroVencimento
            //     );
            // }

        }
        return mov;
    }

}