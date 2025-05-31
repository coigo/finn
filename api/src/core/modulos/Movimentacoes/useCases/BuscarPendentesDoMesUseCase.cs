using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;

namespace Movimentacoes.UseCases;

public class BuscarPendentesDoMesUseCase : IUseCase<Unity, List<MovimentacaoParcela>> {

    private readonly IMovimentacaoRepository _movimentacoes;
    public BuscarPendentesDoMesUseCase  ( IMovimentacaoRepository movimentacoes ) {
        _movimentacoes = movimentacoes;
    }

    public async Task<List<MovimentacaoParcela>> Execute(Unity _)
    {
        var hoje = DateTime.Now;
        var parcelas = await this._movimentacoes.BuscarParcelasDoMes(hoje);
        return parcelas;
    }

}