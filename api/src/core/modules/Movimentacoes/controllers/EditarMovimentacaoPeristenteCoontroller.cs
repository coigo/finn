using Microsoft.AspNetCore.Mvc;
using Movimentacoes.DTOS;
using Movimentacoes.UseCases;

namespace Movimentacoes.Routes ;

[ApiController]
[Route("api/movimentacoes/persistentes")]
public class EditarMovimentacaoPeristenteCoontroller : ControllerBase {

    private readonly EditarPersistenteUseCase UseCase;

    public EditarMovimentacaoPeristenteCoontroller(EditarPersistenteUseCase useCase) {
        UseCase = useCase;
    }

    [HttpPut]
    public async Task<EditarPersistenteDTO> handle ([FromBody] EditarPersistenteDTO data) {
        var mov = await UseCase.Execute(data);
        return mov;
    }

}