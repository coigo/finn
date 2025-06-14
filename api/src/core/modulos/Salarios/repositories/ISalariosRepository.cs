using Salarios.Models;

namespace Infra.Repositories;

public interface ISalarioRepository {
    public Task<Salario> CriarSalario(Salario data);

    public Task<Salario?> BuscarUltimo();
}