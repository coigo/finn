using Infra.Database;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Resumos.Models;

namespace Infra.Repositories.Adapters;

public class ResumoRepository : IResumoRepository {

    private readonly Context _context;

    public ResumoRepository(Context context) {
        _context = context;
    }
    
    public async Task<Resumo> CriarResumo(Resumo data) {
        if (data == null) {
            throw new ArgumentNullException(nameof(data));
        }

        await _context.Resumos.AddAsync(data);
        await _context.SaveChangesAsync();
        return data;
    }

    public async Task<Resumo> AtualizarResumo(int Id, Resumo data) {
        var resumoExistente = await _context.Resumos.FindAsync(Id);
        if (resumoExistente == null) {
            throw new KeyNotFoundException("Reusmo não encontrado!");
        }

        _context.Entry(resumoExistente).CurrentValues.SetValues(data);
        await _context.SaveChangesAsync();
        return data;

    }

    public async Task<List<Resumo>> BuscarTodosResumos() {
        var resumos = await _context.Resumos.ToListAsync(); 
        return resumos;
    }

    public async Task<Resumo> BuscarResumoPorNome(string Nome) {
        var resumo = await _context.Resumos.Where(m => m.Nome == Nome).FirstAsync();
        if (resumo == null) {
            throw new KeyNotFoundException("Reusmo não encontrado!");
        }
        return resumo;
    }
}