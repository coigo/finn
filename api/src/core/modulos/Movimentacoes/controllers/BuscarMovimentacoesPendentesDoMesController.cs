using Infra.Shared;
using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.UseCases;

namespace Infra.Http.Controllers.Movimentacoes;

[ApiController]
[Route("movimentacoes/pendentes")]
public class BuscarMovimentacoesPendentesDoMesController : ControllerBase
{

    private readonly BuscarPendentesDoMesUseCase UseCase;

    public BuscarMovimentacoesPendentesDoMesController(BuscarPendentesDoMesUseCase useCase)
    {
        UseCase = useCase;
    }

    [HttpGet]
    public async Task<List<ListaPendentesDTO>> handle()
    {
        var mov = await UseCase.Execute(Unity.Value);
        return mov;
    }

}