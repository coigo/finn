using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.UseCases;

namespace Movimentacoes.Routes ;

[ApiController]
[Route("api/movimentacoes")]
public class DesfazerMovimentacaoController : ControllerBase {

    private readonly DesfazerMovimentacaoUseCase UseCase;

    public DesfazerMovimentacaoController(DesfazerMovimentacaoUseCase useCase) {
        UseCase = useCase;
    }

    [HttpDelete("{movimentacaoId}")]
    public async Task<int> handle ([FromRoute] int movimentacaoId) {
        var mov = await UseCase.Execute(movimentacaoId);
        return mov;
    }

}