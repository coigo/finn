using Microsoft.AspNetCore.Mvc;
using Movimentacoes.UseCases;

namespace Movimentacoes.Routes ;

[ApiController]
[Route("movimentacoes")]
public class SubtrairParcelasController : ControllerBase {

    private readonly SubtrairParcelasUseCase UseCase;

    public SubtrairParcelasController(SubtrairParcelasUseCase useCase) {
        UseCase = useCase;
    }

    [HttpPost]
    [Route("parcelas")]
    public async Task<SubtrairParcelas> Handle ([FromBody] SubtrairParcelas data) {
        Console.WriteLine("teste");
        var mov = await UseCase.Execute(data);
        return mov;
    }
}
