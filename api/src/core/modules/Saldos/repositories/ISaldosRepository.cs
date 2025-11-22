using Saldos.Models;

namespace Infra.Repositories;

public interface ISaldoRepository
{
    public Task<Saldo> CriarSaldo(Saldo data);

    public Task<Saldo> AtualizarSaldo(int Id, Saldo data);

    public Task<Saldo> AtualizarSaldo(string nome, decimal valor);

    public Task<List<Saldo>> BuscarTodosSaldos();

    public Task<Saldo> BuscarSaldoPorNome(string Nome);

}