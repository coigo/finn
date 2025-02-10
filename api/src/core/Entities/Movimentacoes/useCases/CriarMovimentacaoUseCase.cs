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

        Movimentacao movimentacao = new (
            data.valor,
            data.tipo, 
            data.categoria
        );
        
        return await this._movimentacoes.CriarMovimentacao(movimentacao);
    }

}