using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.Models;
using Movimentacoes.UseCases;
using Resumos.Models;
using Resumos.UseCases;

namespace Resumos.Routes ;

[ApiController]
[Route("resumo")]
public class SubtrairParcelasdoMesController : ControllerBase {

    private readonly SubtrairParcelasDoMes UseCase;

    public SubtrairParcelasdoMesController(SubtrairParcelasDoMes useCase) {
        UseCase = useCase;
    }

    [HttpPost]
    public async Task<Resumo> handle ([FromBody] CriarMovimentacao data) {
        
        var mov = await UseCase.Execute(data);
        return mov;
    }

}