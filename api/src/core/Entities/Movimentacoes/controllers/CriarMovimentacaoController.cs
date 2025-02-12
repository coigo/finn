using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.UseCases;

namespace Pessoa.Routes ;

[ApiController]
[Route("movimentacoes")]
public class CriarMovimentacaoController : ControllerBase {

    private readonly CriarMovimentacaoUseCase UseCase;

    public CriarMovimentacaoController(CriarMovimentacaoUseCase useCase) {
        UseCase = useCase;
    }

    [HttpGet]
    public  int handle () {
        Console.WriteLine("asdasdad");
        

        return 200;
        // return await UseCase.Execute(data);
    }

}