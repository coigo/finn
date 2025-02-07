using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Model.Movimentacao;
using Repositories.Movimentacao;

namespace Repositories.Adapters.Movimentacao;

public class MovimentacaoRepository: IMovimentacaoRepository {

        private readonly Context _context;

    public MovimentacaoRepository (Context context) {
        _context = context;
        
    }
    public async Task<MovimentacaoModel> CriarMovimentacao(MovimentacaoModel data) {

        if (data == null) {
            throw new ArgumentNullException(nameof(data));
        }

        await _context.Movimentacoes.AddAsync(data);
        await _context.SaveChangesAsync();
        return data;
    }
    public async Task<List<MovimentacaoModel>> BuscarPorPeriodo(DateTime inicio, DateTime fim) {
        return await _context.Movimentacoes
            .Where(m => m.criadoEm >= inicio && m.criadoEm <= fim)
            .OrderBy(m => m.criadoEm)
            .ToListAsync();
    }
    public async Task<MovimentacaoModel> AtualizarMovimentacao(int id, MovimentacaoModel data) {

        var movimentacaoExistente = await _context.Movimentacoes.FindAsync(id);
        if ( movimentacaoExistente == null ) {
            throw new KeyNotFoundException("Movimentação não encontrada!");
        }

        _context.Entry(movimentacaoExistente).CurrentValues.SetValues(data);
        return data;
    }

}