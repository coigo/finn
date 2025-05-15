using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;
using Resumos.Models;
using Salarios.Models;

namespace Salarios.UseCases;

public class BuscarSalarioAtualUseCase : IUseCase<Unity, Salario>
{

    private readonly ISalarioRepository _salario;

    public BuscarSalarioAtualUseCase(ISalarioRepository salario)
    {
        _salario = salario;
    }

    public async Task<Salario> Execute(Unity _)
    {
        return await this._salario.BuscarUltimo();
    }

}