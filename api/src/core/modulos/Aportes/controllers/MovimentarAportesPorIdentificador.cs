using System.Reflection.Metadata;
using Aportes.DTOS;
using Aportes.Models;
using Aportes.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Infra.Http.Controllers.Aportes;

[ApiController]
[Route("api/aportes")]
public class MovimentarAportesPorIndicador {
    
    private readonly MovimentarAportesPorIdentificadorUseCase _useCase;

    public MovimentarAportesPorIndicador (MovimentarAportesPorIdentificadorUseCase useCase) {
        _useCase = useCase;
    }

    [HttpPost("{identificador}")]
    public async Task<Aporte> Handle (string identificador, [FromBody] MovimentarAportePorIdentificadorDTO data) {
        return await this._useCase.Execute(new (
            identificador,
            data.Quantidade,
            data.Preco,
            data.TipoStr,
            data.Data
        ));
    }

}