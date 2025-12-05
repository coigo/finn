using Microsoft.AspNetCore.Mvc;
using Salarios.UseCases;
using Salarios.Models;
using Infra.Shared;

namespace Infra.Http.Controllers.Salarios;

[ApiController]
[Route("api/salario")]
public class BuscarSalarioAtualController : ControllerBase
{

    private readonly BuscarSalarioAtualUseCase UseCase;

    public BuscarSalarioAtualController(BuscarSalarioAtualUseCase useCase)
    {
        UseCase = useCase;
    }

    [HttpGet]
    public async Task<Salario> handle()
    {
        return await UseCase.Execute(Unity.Value);
    }

}