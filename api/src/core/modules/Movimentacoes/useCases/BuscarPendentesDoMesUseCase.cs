using Infra.Shared;
using Infra.Repositories;
using Movimentacoes.DTOS;

namespace Movimentacoes.UseCases;

public class BuscarPendentesDoMesUseCase : IUseCase<Unity, List<ListaPendentesDTO>> {

    private readonly IMovimentacaoRepository _movimentacoes;
    public BuscarPendentesDoMesUseCase  ( IMovimentacaoRepository movimentacoes ) {
        _movimentacoes = movimentacoes;
    }

    public async Task<List<ListaPendentesDTO>> Execute(Unity _)
    {
        var hoje = DateTime.Now;
        var parcelas = await this._movimentacoes.BuscarPendentesDoMes(hoje);
        return parcelas;
    }

}