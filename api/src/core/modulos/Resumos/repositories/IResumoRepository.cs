using Resumos.Models;

namespace Infra.Repositories;

public interface IResumoRepository {
    public Task<Resumo> CriarResumo(Resumo data);

    public Task<Resumo> AtualizarResumo(int Id, Resumo data);

    public Task<Resumo> AtualizarSaldo(string nome, decimal valor);

    public Task<List<Resumo>> BuscarTodosResumos();

    public Task<Resumo> BuscarResumoPorNome(string Nome);

}