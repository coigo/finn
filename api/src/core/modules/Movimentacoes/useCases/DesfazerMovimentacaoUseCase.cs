using Infra.Shared;
using Movimentacoes.Models;
using Infra.Repositories;

namespace Movimentacoes.UseCases;

public class DesfazerMovimentacaoUseCase : IUseCase<int, int>
{

    private readonly IMovimentacaoRepository _movimentacoes;
    private readonly ISaldoRepository _saldos;

    public DesfazerMovimentacaoUseCase(IMovimentacaoRepository movimentacoes, ISaldoRepository saldos)
    {
        _movimentacoes = movimentacoes;
        _saldos = saldos;
    }

    public async Task<int> Execute(int movimentacaoId)
    {   
        var movimentacao = await this._movimentacoes.BuscarMovimentacao(movimentacaoId);
        if (movimentacao == null)
        {
            throw new BusinessError("Movimentação não encontrada");
        }

        decimal valor = movimentacao.Tipo == MovimentacaoTipo.ENTRADA 
        ? -movimentacao.Valor
        : movimentacao.Valor;
        
        await this._saldos.AtualizarSaldo("Corrente", valor);
        this._movimentacoes.ApagarMovimentacao(movimentacao.Id);

        return movimentacao.Id;
    }

}