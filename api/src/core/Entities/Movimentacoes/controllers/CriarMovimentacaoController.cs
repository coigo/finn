using System.ComponentModel;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.Models;
using Movimentacoes.UseCases;

namespace Movimentacoes.Controller;

[ApiController]
[Route("movimentacoes")]
public class CriarMovimentacaoController : ControllerBase{

    private readonly CriarMovimentacaoUseCase UseCase;

    public CriarMovimentacaoController ( CriarMovimentacaoUseCase usecase) {
        UseCase = usecase;
    }
    
    [HttpPost("/")]
    public async Task<Movimentacao> CriarMovimentacao ([FromBody] CriarMovimentacao data) {
        return await this.UseCase.Execute(data);
    } 

}