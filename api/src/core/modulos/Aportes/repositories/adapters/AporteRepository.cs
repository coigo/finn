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

    public async Task<List<Aporte>> BuscarPorIdentificador (string Identificador) {
        return await this._context.Aportes
        .Where(a => a.Identificador == Identificador)
        .ToListAsync();

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
    
    public async Task<Aporte> AtualizarAporte (int id, Aporte data) {
        var aporte = await this._context.Aportes.FindAsync(id);

        if ( aporte == null ) {
            throw new KeyNotFoundException();
        }

        _context.Entry(aporte).CurrentValues.SetValues(data);
        await this._context.SaveChangesAsync();
        return data;

    }
}