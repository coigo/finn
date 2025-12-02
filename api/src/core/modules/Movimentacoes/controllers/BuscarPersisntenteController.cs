using Microsoft.AspNetCore.Mvc;
using Movimentacoes.Models;
using Movimentacoes.UseCases;

namespace Infra.Http.Controllers.Movimentacoes;

[ApiController]
[Route("api/movimentacoes/persistentes")]
public class BuscarPersisntentesController : ControllerBase
{

    private readonly BuscarPersistenteUseCase UseCase;

    public BuscarPersisntentesController(BuscarPersistenteUseCase useCase)
    {
        UseCase = useCase;
    }

    [HttpGet("{persistenteId}")]
    public async Task<MovimentacaoPersistente> handle([FromRoute] int persistenteId)
    {
        Console.WriteLine(persistenteId);
        return await UseCase.Execute(persistenteId);
    }

}