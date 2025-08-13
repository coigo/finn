using Aportes.DTOS;
using Aportes.Models;
using Infra.Repositories;
using Infra.Shared;

namespace Aportes.UseCases;

public class BuscarAportesHistoricoUseCase : IUseCase<string, List<AporteHistoricoDTO>>
{
    private readonly IAporteRepository _aportes;

    public BuscarAportesHistoricoUseCase(IAporteRepository aportes)
    {
        _aportes = aportes;
    }

    public async Task<List<AporteHistoricoDTO>> Execute(string Identificador)
    {
        Console.WriteLine(Identificador);
        return await this._aportes.BuscarDetalhesDoAporte(Identificador);
    }

}