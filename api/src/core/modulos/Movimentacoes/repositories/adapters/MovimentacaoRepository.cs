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
    
    public async Task<MovimentacaoPersistente> CriarMovimentacaoPersistente(MovimentacaoPersistente data)
    {

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        await _context.MovimentacoesPersistentes.AddAsync(data);
        await _context.SaveChangesAsync();
        return data;
    }
    
    public async Task CriarMovimentacaoParcela(List<MovimentacaoParcela> data)
    {

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        await _context.MovimentacoesParcelas.AddRangeAsync(data);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<ListaMovimentacoesDTO>> BuscarPorPeriodo(DateTime inicio, DateTime fim)
    {
        return await _context.Database.SqlQuery<ListaMovimentacoesDTO>($@"
            SELECT 
                m.valor AS valor,
                m.tipo,
                mc.nome as categoria,
                m.data
            FROM movimentacoes m
            LEFT JOIN movimentacoes_categorias mc 
                ON mc.id = m.categoriaId 
            WHERE 
                m.data >= {inicio} AND m.data <=  {fim}
            GROUP BY m.id
            ORDER by m.data desc
            ")
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
        
        await _context.MovimentacoesParcelas.AddRangeAsync(data);
        await _context.SaveChangesAsync();
        return data.ToList();
    }

    public async Task<List<MovimentacaoParcela>> BuscarParcelasPorId(int[] parcelas) {
        return await this._context.MovimentacoesParcelas
            .Where(p => parcelas.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<List<ListaPendentesDTO>> BuscarPendentesDoMes(DateTime hoje)
    {
        var result = await _context.Database.SqlQuery<ListaPendentesDTO>($@"
        SELECT 
            mp.valor,
            mc.nome AS categoria,
            mp.vencimento,
            'PARCELA' as tipo
        FROM movimentacoes_parcelas mp
        LEFT JOIN movimentacoes_categorias mc 
            ON mc.id = mp.categoriaId
        WHERE 
            strftime('%m', mp.vencimento) = {hoje.Month.ToString("D2")} AND
            strftime('%Y', mp.vencimento) = {hoje.Year.ToString()}
        union
        select 
            mp.valor,
            mc.nome as categoria,
            null as vencimento,
            'PERSISTENTE' as tipo
        from movimentacoes_persistentes mp 
        left join movimentacoes_categorias mc on mc.id = mp.categoriaId 
        where mp.id not in (
            select persistenteId 
            from movimentacoes 
            where persistenteId is not null 
            and strftime('%m', data) = {hoje.Month.ToString("D2")}
            AND strftime('%Y', data) = {hoje.Year.ToString()}
        )").ToListAsync();
        return result;
        
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