using Aportes.Models;
using Infra.Repositories;
using Infra.Shared;

namespace Aportes.UseCases;

public class BuscarAportesUseCase: IUseCase<Unit, List<Aporte>> {

    private readonly IAtivosRepository _ativos;
    private readonly IAporteRepository _aportes;

    public BuscarAportesUseCase ( IAporteRepository aportes, IAtivosRepository ativos ) {
        _ativos = ativos;
        _aportes = aportes;
    }

    public async Task<List<Aporte>> Execute (Unit _) {
        var aportes = await this._aportes.BuscarTodos();
        
        foreach (var aporte in aportes) {
            await this._ativos.BuscarPorTicker(aporte.Identificador);
        }
        return aportes;
    }

}