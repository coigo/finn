using Aportes.DTOS;
using Aportes.Models;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Adapters;

public class AporteRepository: IAporteRepository {

    private readonly Context _context;

    public AporteRepository ( Context context ) {
        _context = context;
    }
    
    public async Task<List<Aporte>> BuscarTodos () {
        return await this._context.Aportes.ToListAsync();
    }

    public async Task<Aporte?> BuscarPorIdentificador (string Identificador) {
        return await this._context.Aportes
        .Where(a => a.Identificador == Identificador)
        .FirstOrDefaultAsync();

    }

    public async Task<List<Aporte>> BuscarPorCategoria (AporteCategoria categoria) {
        return await this._context.Aportes
        .Where(a => a.Categoria == categoria)
        .ToListAsync();

    }
    
    public async Task<Aporte> CriarAporte (Aporte data) {
        await this._context.AddAsync(data);
        await this._context.SaveChangesAsync();
        return data;
    }
    
    public async Task<AtualizarAporteDTO> AtualizarAporte (int id, AtualizarAporteDTO data) {
        
        
        var aporte = await this._context.Aportes.FindAsync(id);
        
        if ( aporte == null ) {
            throw new KeyNotFoundException();
        }

        aporte.PrecoMedio = data.PrecoMedio;
        aporte.Quantidade = data.Quantidade;

        _context.Entry(aporte).CurrentValues.SetValues(aporte);
        await this._context.SaveChangesAsync();
        return data;

    }

}