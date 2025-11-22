using Infra.Shared;
using Movimentacoes.DTOS;
using Infra.Repositories;

namespace Movimentacoes.UseCases;

public class BuscarMovimentacoesCategoriasUseCase : IUseCase<Unity, List<BuscarMovimentacaoCategoriaDTO>>
{

    private readonly IMovimentacaoRepository _movimentacoes;
    public BuscarMovimentacoesCategoriasUseCase(IMovimentacaoRepository movimentacoes)
    {
        _movimentacoes = movimentacoes;
    }

    public async Task<List<BuscarMovimentacaoCategoriaDTO>> Execute(Unity _)
    {

        var categorias = await this._movimentacoes.BuscarCategorias();
        return categorias;
    }

}