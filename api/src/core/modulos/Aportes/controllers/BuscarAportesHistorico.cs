using Aportes.DTOS;
using Aportes.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Infra.Http.Controllers.Aportes;

[ApiController]
[Route("api/aportes")]
public class BuscarAportesHistorico
{

    private readonly BuscarAportesHistoricoUseCase _useCase;

    public BuscarAportesHistorico(BuscarAportesHistoricoUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("{Identificador}/historico")]
    public async Task<List<AporteHistoricoDTO>> Handle([FromRoute] string Identificador)
    {
        return await this._useCase.Execute(Identificador);
    }

}