using Aportes.Models;

namespace Infra.Repositories;

public interface IAporteRepository {
    public Task<List<Aporte>> BuscarTodos ();

    public Task<List<Aporte>> BuscarPorIdentificador ();

    public Task<List<Aporte>> BuscarPorCategoria ();
    
    public Task<Aporte> CriarAporte (Aporte data);
    
    public Task<Aporte> AtualizarAporte (int id, Aporte data);

}