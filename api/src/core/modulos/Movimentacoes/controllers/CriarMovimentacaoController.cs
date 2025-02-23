using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.Models;
using Movimentacoes.UseCases;

namespace Movimentacoes.Routes ;

[ApiController]
[Route("movimentacoes")]
public class CriarMovimentacaoController : ControllerBase {

    private readonly CriarMovimentacaoUseCase UseCase;

    public CriarMovimentacaoController(CriarMovimentacaoUseCase useCase) {
        UseCase = useCase;
    }

    [HttpPost]
    public async Task<CriarMovimentacao> handle ([FromBody] CriarMovimentacao data) {
        
        var mov = await UseCase.Execute(data);
        return mov;
    }

}