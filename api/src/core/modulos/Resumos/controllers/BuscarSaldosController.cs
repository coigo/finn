using Microsoft.AspNetCore.Mvc;
using Resumos.Models;
using Resumos.UseCases;

namespace Infra.Http.Controllers.Movimentacoes;

 [ApiController]
[Route("api/saldos")]
public class BuscarSaldoController : ControllerBase
{

    private readonly BuscarSaldoUseCase UseCase;

    public BuscarSaldoController(BuscarSaldoUseCase useCase)
    {
        UseCase = useCase;
    }

    [HttpGet]
    public async Task<Resumo> handle([FromQuery] string saldoNome)
    {
        return await UseCase.Execute(saldoNome);
    }

}