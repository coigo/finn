using Infra.Repositories;
using Infra.Shared;
using Saldos.Models;
using Saldos.DTOS;
using System.ComponentModel.DataAnnotations;

namespace Saldos.UseCases;
public class EditarSaldoUseCase : IUseCase<EditarSaldoDTO, Saldo>
{

    private ISaldoRepository _saldo;

    public EditarSaldoUseCase(ISaldoRepository saldo)
    {
        _saldo = saldo;
    }

    public async Task<Saldo> Execute(EditarSaldoDTO data)
    {
        var saldoExiste = await this._saldo.BuscarSaldoPorNome(data.nome);
        if (saldoExiste == null)
        {   
            throw new ValidationException(String.Format("Não há saldo com esse nome {data.nome}"));
        }
        saldoExiste.Valor = data.valor;
        return await this._saldo.AtualizarSaldo(saldoExiste.Id, saldoExiste);
    }

}