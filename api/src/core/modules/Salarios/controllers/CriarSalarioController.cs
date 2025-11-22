using Microsoft.AspNetCore.Mvc;
using Salarios.UseCases;
using Salarios.Models;

namespace Infra.Http.Controllers.Salarios;

[ApiController]
[Route("api/salario")]
public class CriarSalarioController : ControllerBase
{

    private readonly CriarSalarioUseCase UseCase;

    public CriarSalarioController(CriarSalarioUseCase useCase)
    {
        UseCase = useCase;
    }

    [HttpPost]
    public async Task<Salario> handle([FromBody] CriarSalarioDto salario)
    {
        return await UseCase.Execute(salario);
    }

}