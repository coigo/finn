using System.ComponentModel;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;

namespace Movimentacoes.Controller;

[ApiController]
[Route("movimentacoes")]
public class MovimentacaoController : ControllerBase{
    [HttpGet]
    public string Teste () {
        return "ok";
    } 

}