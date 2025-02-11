using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Movimentacoes.Models;

namespace Movimentacoes.Repositories.Adapters;

public class MovimentacaoRepository: IMovimentacaoRepository {

        private readonly Context _context;

    public MovimentacaoRepository (Context context) {
        _context = context;
        
    }
    public async Task<Movimentacao> CriarMovimentacao(Movimentacao data) {

        if (data == null) {
            throw new ArgumentNullException(nameof(data));
        }

        await _context.Movimentacoes.AddAsync(data);
        await _context.SaveChangesAsync();
        return data;
    }
    public async Task<List<Movimentacao>> BuscarPorPeriodo(DateTime inicio, DateTime fim) {
        return await _context.Movimentacoes
            .Where(m => m.CriadoEm >= inicio && m.CriadoEm <= fim)
            .OrderBy(m => m.CriadoEm)
            .ToListAsync();
    }
    public async Task<Movimentacao> AtualizarMovimentacao(int id, Movimentacao data) {

        var movimentacaoExistente = await _context.Movimentacoes.FindAsync(id);
        if ( movimentacaoExistente == null ) {
            throw new KeyNotFoundException("Movimentação não encontrada!");
        }

        _context.Entry(movimentacaoExistente).CurrentValues.SetValues(data);
        return data;
    }

}