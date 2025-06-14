using Infra.Shared;
using Infra.Repositories;
using Salarios.Models;

namespace Salarios.UseCases;

public class CriarSalarioUseCase : IUseCase<CriarSalarioDto, Salario>
{

    private readonly ISalarioRepository _salario;

    public CriarSalarioUseCase(ISalarioRepository salario)
    {
        _salario = salario;
    }

    public async Task<Salario> Execute(CriarSalarioDto salario)
    {
        Console.WriteLine(salario);
        if (salario.valor < 0)
        {
            throw new BusinessError("Use um valor positivo");
        }
        return await this._salario.CriarSalario(new Salario(salario.valor));
    }

}