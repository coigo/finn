using Aportes.DTOS;
using Aportes.Models;
using Infra.Repositories;
using Infra.Shared;

namespace Aportes.UseCases;

public class BuscarAportesUseCase: IUseCase<Unit, List<BuscarAportesDTO>> {

    private readonly IAtivosRepository _ativos;
    private readonly IAporteRepository _aportes;

    public BuscarAportesUseCase ( IAporteRepository aportes, IAtivosRepository ativos ) {
        _ativos = ativos;
        _aportes = aportes;
    }

    public async Task<List<BuscarAportesDTO>> Execute (Unit _) {
        var aportes = await this._aportes.BuscarTodos();
        
        var listaAportes = new List<BuscarAportesDTO>();

        foreach (var aporte in aportes) {
            var ativoInfo = await this._ativos.BuscarPorTicker(aporte.Identificador);
            listaAportes.Add(new BuscarAportesDTO(
                aporte.Identificador,
                ativoInfo.ShortName,
                aporte.PrecoMedio,
                ativoInfo.RegularMarketPrice,
                aporte.Quantidade,
                aporte.Categoria.ToString()
            ));
        }
        return listaAportes;
    }

}