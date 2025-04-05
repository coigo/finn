using Aportes.Models;
using Aportes.UseCases;
using Infra.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Infra.Http.Controllers.Aportes;

[ApiController]
[Route("aportes")]
public class BuscarAportes {
    
    private readonly BuscarAportesUseCase _useCase;

    public BuscarAportes (BuscarAportesUseCase useCase) {
        _useCase = useCase;
    }

    [HttpGet]
    public async Task<List<Aporte>> Handle () {
        return await  this._useCase.Execute(Unit.Value);
    }

}