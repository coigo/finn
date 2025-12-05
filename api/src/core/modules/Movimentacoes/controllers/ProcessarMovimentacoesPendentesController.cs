using Microsoft.AspNetCore.Mvc;
using Movimentacoes.UseCases;

namespace Movimentacoes.Routes ;

[ApiController]
[Route("api/movimentacoes")]
public class ProcessarMovimentacoesPendentesController : ControllerBase {

    private readonly ProcessarMovimentacoesPendentesUseCase UseCase;

    public ProcessarMovimentacoesPendentesController(ProcessarMovimentacoesPendentesUseCase useCase) {
        UseCase = useCase;
    }

    [HttpPost("pendentes")]
    public async Task<bool> handle()
    {
        await UseCase.Execute();
        return true;
    }

}