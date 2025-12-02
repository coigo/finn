using Infra.Shared;
using Infra.Repositories;
using Movimentacoes.Models;

namespace Movimentacoes.UseCases;

public class BuscarPersistenteUseCase : IUseCase<int, MovimentacaoPersistente> {

    private readonly IMovimentacaoRepository _movimentacoes;
    public BuscarPersistenteUseCase  ( IMovimentacaoRepository movimentacoes ) {
        _movimentacoes = movimentacoes;
    }

    public async Task<MovimentacaoPersistente> Execute(int id)
    {
        var movimentacao = await this._movimentacoes.BuscarMovimentacaoPersistente(id);
        if (movimentacao == null)
        {
            throw new BusinessError("Movimentação não encontrada!");
        }
        return movimentacao;
    }

}