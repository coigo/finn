using Infra.Shared;
using Movimentacoes.Models;
using Infra.Repositories;
using Movimentacoes.DTOS;

namespace Movimentacoes.UseCases;

public class DeletarPersistenteUseCase : IUseCase<int, int>
{

    private readonly IMovimentacaoRepository _movimentacoes;

    public DeletarPersistenteUseCase(IMovimentacaoRepository movimentacoes, ISaldoRepository saldos)
    {
        _movimentacoes = movimentacoes;
    }

    public async Task<int> Execute(int id)
    {   
        
        this._movimentacoes.DeletarMovimentacaoPersistente(id);
        Console.WriteLine("wewee");
        return id;
    }

}