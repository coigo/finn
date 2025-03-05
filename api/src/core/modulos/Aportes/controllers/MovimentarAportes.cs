using System.Reflection.Metadata;
using Aportes.DTOS;
using Aportes.Models;
using Aportes.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Infra.Http.Controllers.Aportes;

[ApiController]
[Route("aportes")]
public class MovimentarAportes {
    
    private readonly MovimentarAportesUseCase _useCase;

    public MovimentarAportes (MovimentarAportesUseCase useCase) {
        _useCase = useCase;
    }

    [HttpPost]
    public async Task<Aporte> Handle ([FromBody] MovimentarAporteDTO data) {
        return await this._useCase.Execute(data);
    }

}