using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.Factories;
using Movimentacoes.UseCases;

namespace Movimentacoes.Routes ;

[ApiController]
[Route("api/movimentacoes")]
public class CriarMovimentacaoController : ControllerBase {

    private readonly MovimentacaoFactory UseCase;

    public CriarMovimentacaoController(MovimentacaoFactory useCase) {
        UseCase = useCase;
    }

    [HttpPost]
    public async Task<CriarMovimentacao> handle ([FromBody] CriarMovimentacao data) {
        UseCase.Execute(data);
        return data;
    }

}