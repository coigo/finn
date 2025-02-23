using System.Runtime.CompilerServices;
using Infra.Repositories;
using Infra.Shared;
using Resumos.Models;

namespace Resumos.UseCases;

public class SubtrairParcelasDoMes : IUseCase<object, Resumo> {

    private readonly IResumoRepository _resumos;
    private readonly IMovimentacaoRepository _movimentecoes;

    public SubtrairParcelasDoMes ( IMovimentacaoRepository movimentacao, IResumoRepository resumos ) {
        _movimentecoes = movimentacao;
        _resumos = resumos;
    }

    public async Task<Resumo> Execute( object _) {

        DateTime now = DateTime.Now;
        DateTime inicio = new (now.Year, now.Month, 1);
        DateTime fim = new (now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));


        var parcelas = await this._movimentecoes.BuscarParcelasDoMes(inicio, fim);

        if ( parcelas.Count == 0  ){
            Console.WriteLine("nadinha");
        }
        float removerValor = parcelas.Sum(p => p.Valor);

        Resumo resumo = await this._resumos.AtualizarSaldo("Corrente", -removerValor);
        return resumo;


    }

}