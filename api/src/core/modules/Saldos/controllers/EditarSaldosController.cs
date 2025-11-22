using Microsoft.AspNetCore.Mvc;
using Saldos.DTOS;
using Saldos.Models;
using Saldos.UseCases;

namespace Infra.Http.Controllers.Movimentacoes;

[ApiController]
[Route("api/saldos")]
public class EditarSaldoController : ControllerBase
{

    private readonly EditarSaldoUseCase UseCase;

    public EditarSaldoController(EditarSaldoUseCase useCase)
    {
        UseCase = useCase;
    }

    [HttpPut]
    public async Task<Saldo> handle([FromBody] EditarSaldoDTO data)
    {
        return await UseCase.Execute(data);
    }

}