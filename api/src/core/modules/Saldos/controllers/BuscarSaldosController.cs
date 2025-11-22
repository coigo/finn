using Microsoft.AspNetCore.Mvc;
using Saldos.Models;
using Saldos.UseCases;

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
    public async Task<Saldo> handle([FromQuery] string saldoNome)
    {
        return await UseCase.Execute(saldoNome);
    }

}