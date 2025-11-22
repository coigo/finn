using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Saldos.Models;

namespace Infra.Repositories.Adapters;

public class SaldoRepository : ISaldoRepository
{

    private readonly Context _context;

    public SaldoRepository(Context context)
    {
        _context = context;
    }

    public async Task<Saldo> CriarSaldo(Saldo data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        await _context.Saldos.AddAsync(data);
        await _context.SaveChangesAsync();
        return data;
    }

    public async Task<Saldo> AtualizarSaldo(int Id, Saldo data)
    {
        var saldoExistente = await _context.Saldos.FindAsync(Id);
        if (saldoExistente == null)
        {
            throw new KeyNotFoundException("Reusmo não encontrado!");
        }

        _context.Entry(saldoExistente).CurrentValues.SetValues(data);
        await _context.SaveChangesAsync();
        return data;

    }

    public async Task<Saldo> AtualizarSaldo(string nome, decimal valor)
    {

        var saldoExistente = await _context.Saldos
            .Where(r => r.Nome == nome)
            .FirstAsync();

        if (saldoExistente == null)
        {
            throw new KeyNotFoundException("Reusmo não encontrado!");
        }

        saldoExistente.Valor += valor;

        await _context.SaveChangesAsync();
        return saldoExistente;

    }

    public async Task<List<Saldo>> BuscarTodosSaldos()
    {
        var saldos = await _context.Saldos.ToListAsync();
        return saldos;
    }

    public async Task<Saldo> BuscarSaldoPorNome(string Nome)
    {
        var saldo = await _context.Saldos.Where(m => m.Nome == Nome).FirstAsync();
        if (saldo == null)
        {
            throw new KeyNotFoundException("Reusmo não encontrado!");
        }
        return saldo;
    }
}