using Infra.Shared;
using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.UseCases;

namespace Infra.Http.Controllers.Movimentacoes;

[ApiController]
[Route("api/movimentacoes/categorias")]
public class BuscarMovimentacoesCategoriasController : ControllerBase
{

    private readonly BuscarMovimentacoesCategoriasUseCase UseCase;

    public BuscarMovimentacoesCategoriasController(BuscarMovimentacoesCategoriasUseCase useCase)
    {
        UseCase = useCase;
    }

    [HttpGet]
    public async Task<List<BuscarMovimentacaoCategoriaDTO>> handle()
    {

        var mov = await UseCase.Execute(Unity.Value);
        return mov;
    }

}