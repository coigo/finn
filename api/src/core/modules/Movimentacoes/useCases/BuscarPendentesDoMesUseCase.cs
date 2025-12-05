using Infra.Shared;
using Infra.Repositories;
using Movimentacoes.DTOS;

namespace Movimentacoes.UseCases;

public class BuscarPendentesDoMesUseCase : IUseCase<Unity, PendentesDTO> {

    private readonly IMovimentacaoRepository _movimentacoes;
    public BuscarPendentesDoMesUseCase  ( IMovimentacaoRepository movimentacoes ) {
        _movimentacoes = movimentacoes;
    }

    public async Task<PendentesDTO> Execute(Unity _)
    {
        var hoje = DateTime.Now;
        var parcelas = await this._movimentacoes.BuscarPendentesDoMes(hoje);
        
        Periodo periodo = Periodo.MES_ATUAL;
        
        if (parcelas.Count == 0)
        {   
            var nextMonth = DateTime.Now.AddMonths(1);
            parcelas = await this._movimentacoes.BuscarPendentesDoMes(nextMonth);
            periodo = Periodo.PROXIMO_MES;
        }

        return new PendentesDTO(parcelas, periodo);
    }

}