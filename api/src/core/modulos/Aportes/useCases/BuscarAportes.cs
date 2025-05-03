using System.Text.Json;
using Aportes.DTOS;
using Aportes.Models;
using Ativos.Models;
using Infra.Repositories;
using Infra.Shared;

namespace Aportes.UseCases;

public class BuscarAportesUseCase : IUseCase<Unity, List<BuscarAportesDTO>>
{

    private readonly IAtivosRepository _ativos;
    private readonly IAporteRepository _aportes;

    public BuscarAportesUseCase(IAporteRepository aportes, IAtivosRepository ativos)
    {
        _ativos = ativos;
        _aportes = aportes;
    }

    public async Task<List<BuscarAportesDTO>> Execute(Unity _)
    {
        var aportes = await this._aportes.BuscarTodos();

        var listaAportes = new List<BuscarAportesDTO>();

        foreach (var aporte in aportes)
        {
            var precoAtual = await this.BuscarPrecoAtual(aporte);
            
            if (!precoAtual.HasValue) {
                throw new BusinessError("Ativo não encontrado!");
            }
            listaAportes.Add(new BuscarAportesDTO(
                aporte.Identificador,
                aporte.PrecoMedio,
                precoAtual.Value,
                aporte.Quantidade,
                aporte.Categoria.ToString()
            ));
        }
        return listaAportes;
    }

    private async Task<decimal?> BuscarPrecoAtual (Aporte aporte) {
        if ( aporte.Categoria == AporteCategoria.CRIPTOMOEDA ) {
            var result = await this._ativos.BuscarCrypto(aporte.Identificador);
            return result;
        }
        return await this._ativos.BuscarPorTicker(aporte.Identificador);
    }

}