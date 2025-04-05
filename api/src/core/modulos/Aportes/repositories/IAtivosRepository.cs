using Ativos.Models;

namespace Infra.Repositories;

public interface IAtivosRepository {
    Task<Ativo>BuscarPorTicker(string ticker);  
}