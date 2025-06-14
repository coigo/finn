using Aportes.DTOS;
using Aportes.Models;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Adapters;

public class AporteHistoricoRepository: IAporteHistoricoRepository {
    
    private readonly Context _context;

    public AporteHistoricoRepository ( Context context ) {
        _context = context;
    }

    public async Task<List<AporteHistorico>> Buscar (HistoricoFiltrosDTO filtros) {
        var query =   this._context.Set<AporteHistorico>().AsQueryable();

        if (filtros.Categoria != null) {
            query = query.Where(h => h.Categoria == filtros.Categoria );
        }
        if (filtros.FiltrarPeriodo) {
            query = query.Where(h => 
                h.CriadoEm <= filtros.Inicio &&
                h.CriadoEm >= filtros.Fim 
            );
        }

        return await query.ToListAsync();
    }

    public async Task<AporteHistorico> CriarRegistro (AporteHistorico data) {
        await this._context.AportesHistoricos.AddAsync(data);
        await this._context.SaveChangesAsync();
        return data;
    }

}