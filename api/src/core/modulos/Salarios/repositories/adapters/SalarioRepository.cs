using Infra.Database;
using Infra.Shared;
using Microsoft.EntityFrameworkCore;
using Salarios.Models;

namespace Infra.Repositories.Adapters;

public class SalarioRepository : ISalarioRepository {

    private readonly Context _context;

    public SalarioRepository (Context context) {
        _context = context;
    }

    public async Task<Salario> CriarSalario (Salario data) {
        await _context.Salarios.AddAsync(data);
        await _context.SaveChangesAsync();

        return data;
    }

    public async Task<Salario?> BuscarUltimo () {
        var result = await _context.Salarios.OrderByDescending(s => s.Id).FirstOrDefaultAsync();
        return result;
    }

}