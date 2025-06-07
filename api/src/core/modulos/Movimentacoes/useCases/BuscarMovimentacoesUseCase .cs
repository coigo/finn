using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;

namespace Movimentacoes.UseCases;

public class BuscarMovimentacoesUseCase : IUseCase<BuscarPorPeriodoDTO, List<ListaMovimentacoesDTO>> {

    private readonly IMovimentacaoRepository _movimentacoes;
    public BuscarMovimentacoesUseCase  ( IMovimentacaoRepository movimentacoes ) {
        _movimentacoes = movimentacoes;
    }

    public async Task<List<ListaMovimentacoesDTO>> Execute (BuscarPorPeriodoDTO data)
    {
        var (inicio, fim) = data;

        var inicioDate = DateTime.Parse(inicio).Date;
        var fimDate = DateTime.Parse(fim).AddDays(1).AddMicroseconds(-1);
        
        var mov = await this._movimentacoes.BuscarPorPeriodo(inicioDate, fimDate);
        return mov;
    }

}