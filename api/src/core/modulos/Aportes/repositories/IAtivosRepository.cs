
namespace Infra.Repositories;

public interface IAtivosRepository {
    Task<decimal?>BuscarPorTicker(string ticker);  
    Task<decimal?>BuscarCrypto(string crypto);  
}