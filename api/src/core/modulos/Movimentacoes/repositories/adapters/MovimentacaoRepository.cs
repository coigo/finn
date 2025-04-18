using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Movimentacoes.DTOS;
using Movimentacoes.Models;

namespace Infra.Repositories.Adapters;

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
    public async Task<List<ListaMovimentacoesDTO>> BuscarPorPeriodo(DateTime inicio, DateTime fim) {
        return await _context.Database.SqlQuery<ListaMovimentacoesDTO>($@"
            SELECT 
                COALESCE(mp.valor, m.valor) AS valor,
                m.tipo,
                m.categoria_id,
                m.criadoEm,
                mp.vencimento
            FROM movimentacoes m
            LEFT JOIN movimentacoes_parcelas mp 
                ON mp.movimentacao_id = m.id 
                AND mp.vencimento BETWEEN {inicio} AND {fim}
            WHERE 
                m.criadoEm BETWEEN {inicio} AND {fim}
                OR mp.vencimento BETWEEN {inicio} AND {fim}
            GROUP BY m.id")
        .ToListAsync();
    }
    public async Task<Movimentacao> AtualizarMovimentacao(int id, Movimentacao data) {

        var movimentacaoExistente = await _context.Movimentacoes.FindAsync(id);
        if ( movimentacaoExistente == null ) {
            throw new KeyNotFoundException("Movimentação não encontrada!");
        }

        _context.Entry(movimentacaoExistente).CurrentValues.SetValues(data);
        await this._context.SaveChangesAsync();
        return data;
    }

    public async Task<List<MovimentacaoParcela>> CriarParcelas(IEnumerable<MovimentacaoParcela> data ) {
        if (data == null) {
            throw new ArgumentNullException(nameof(data));
        }
        
        await _context.MovimentacaoParcelas.AddRangeAsync(data);
        await _context.SaveChangesAsync();
        return data.ToList();
    }

    public async Task<List<MovimentacaoParcela>> BuscarParcelasPorId(int[] parcelas) {
        return await this._context.MovimentacaoParcelas
            .Where(p => parcelas.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<MovimentacaoCategoria> BuscarCategoriaPorNome(string nome ) {
        MovimentacaoCategoria categoria = await _context.MovimentacoesCategorias.Where(c => c.Nome == nome).FirstAsync();
        
        if ( categoria == null) {
            throw new KeyNotFoundException(nameof(categoria));
        }
        return categoria;
    }

    public async Task<List<BuscarMovimentacaoCategoriaDTO>> BuscarCategorias( ) {
        var categoria = await _context.MovimentacoesCategorias.Select(x => new BuscarMovimentacaoCategoriaDTO(
            x.Id,
            x.Nome,
            x.Tipo.ToString()
        )).ToListAsync();
        
        if ( categoria == null) {
            throw new KeyNotFoundException(nameof(categoria));
        }
        return categoria;
    }

}