using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Movimentacoes.Repositories;

namespace Movimentacoes.UseCases;

public class BuscarMovimentacoesUseCase  : IUseCase<BuscarPorPeriodoDTO, List<ListaMovimentacoesDTO>> {

    private readonly IMovimentacaoRepository _movimentacoes;
    public BuscarMovimentacoesUseCase  ( IMovimentacaoRepository movimentacoes ) {
        _movimentacoes = movimentacoes;
    }

    public async Task<List<ListaMovimentacoesDTO>> Execute (BuscarPorPeriodoDTO data) {
        var (inicio, fim) = data;

        var mov = await this._movimentacoes.BuscarPorPeriodo(DateTime.Parse(inicio), DateTime.Parse(fim));
        Console.WriteLine(mov);
        return mov;
    }

}