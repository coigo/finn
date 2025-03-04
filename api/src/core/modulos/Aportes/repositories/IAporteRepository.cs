using Aportes.Models;

namespace Infra.Repositories;

public interface IAporteRepository {
    public Task<List<Aporte>> BuscarTodos ();

    public Task<Aporte> BuscarPorIdentificador (string Identificador);

    public Task<List<Aporte>> BuscarPorCategoria (AporteCategoria categoria);
    
    public Task<Aporte> CriarAporte (Aporte data);

    public Task<Aporte> AtualizarAporte (int id, Aporte data);

}