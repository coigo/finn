namespace Saldos.UseCases;

public class BuscarSaldoUseCase : IUseCase<string, Saldo>
{

    private ISaldoRepository _saldo;

    public BuscarSaldoUseCase(ISaldoRepository saldo)
    {
        _saldo = saldo;
    }

    public async Task<Saldo> Execute(string saldoNome)
    {
        return await this._saldo.BuscarSaldoPorNome(saldoNome);
    }

}