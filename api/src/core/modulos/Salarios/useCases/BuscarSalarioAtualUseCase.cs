using Infra.Shared;
using Infra.Repositories;
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
        var salario = await this._salario.BuscarUltimo();
        if (salario == null)
        {
            Console.WriteLine("asdasda");
            throw new KeyNotFoundException("Salãrio não cadastrado");
        }
        return salario;
    }

}