using Infra.Shared;
using Infra.Repositories;

namespace Movimentacoes.UseCases;

public class SubtrairParcelasUseCase : IUseCase<SubtrairParcelas, SubtrairParcelas>
{

    private readonly IMovimentacaoRepository _movimentacoes;
    private readonly ISaldoRepository _saldo;

    public SubtrairParcelasUseCase(IMovimentacaoRepository movimentacoes, ISaldoRepository saldo)
    {
        _movimentacoes = movimentacoes;
        _saldo = saldo;
    }

    public async Task<SubtrairParcelas> Execute(SubtrairParcelas data)
    {

        var parcelas = await this._movimentacoes.BuscarParcelasPorId(data.parcelas);

        decimal valorTotal = parcelas.Sum(p => p.Valor);
        await this._saldo.AtualizarSaldo("Corrente", -valorTotal);
        return data;
    }

}