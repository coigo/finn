using Microsoft.AspNetCore.Mvc;
using Salarios.UseCases;
using Salarios.Models;
using Infra.Shared;

namespace Infra.Http.Controllers.Salarios;

[ApiController]
[Route("api/salario")]
public class AdicionarSalarioController : ControllerBase
{

    private readonly AdicionarSalarioAtualUseCase UseCase;

    public AdicionarSalarioController(AdicionarSalarioAtualUseCase useCase)
    {
        UseCase = useCase;
    }

    [HttpPost("adicionar")]
    public async Task<Salario> handle()
    {
        Console.WriteLine("asdasdasd");
        return await UseCase.Execute(Unity.Value);
    }

}