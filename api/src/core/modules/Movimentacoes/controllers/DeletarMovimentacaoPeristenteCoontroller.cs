using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.UseCases;

namespace Movimentacoes.Routes ;

[ApiController]
[Route("api/movimentacoes/persistentes")]
public class DeletarMovimentacaoPeristenteCoontroller : ControllerBase {

    private readonly DeletarPersistenteUseCase UseCase;

    public DeletarMovimentacaoPeristenteCoontroller(DeletarPersistenteUseCase useCase) {
        UseCase = useCase;
    }

    [HttpDelete("{movimentacaoId}")]
    public async Task<int> handle ([FromRoute] int movimentacaoId) {
        Console.WriteLine("asdasd");
        Console.WriteLine(movimentacaoId);
        var mov = await UseCase.Execute(movimentacaoId);
        return mov;
    }

}