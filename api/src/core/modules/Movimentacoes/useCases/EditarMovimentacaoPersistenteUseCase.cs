using Infra.Shared;
using Movimentacoes.Models;
using Infra.Repositories;
using Movimentacoes.DTOS;

namespace Movimentacoes.UseCases;

public class EditarPersistenteUseCase : IUseCase<EditarPersistenteDTO, EditarPersistenteDTO>
{

    private readonly IMovimentacaoRepository _movimentacoes;

    public EditarPersistenteUseCase(IMovimentacaoRepository movimentacoes, ISaldoRepository saldos)
    {
        _movimentacoes = movimentacoes;
    }

    public async Task<EditarPersistenteDTO> Execute(EditarPersistenteDTO data)
    {   
        this._movimentacoes.EditarMovimentacaoPersistente(data);
        return data;
    }

}