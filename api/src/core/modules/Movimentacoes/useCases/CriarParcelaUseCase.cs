using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;
using Movimentacoes.Factories;

namespace Movimentacoes.UseCases;

public class CriarParcelaUseCase
{

    private readonly IMovimentacaoRepository _movimentacoes;

    public CriarParcelaUseCase(IMovimentacaoRepository movimentacoes)
    {
        _movimentacoes = movimentacoes;
    }

    public async void Execute(CriarMovimentacao data)
    {
        int quantidade = data.quantidadeParcelas.Value;
        DateTime vencimento = data.primeiroVencimento.Value;

        List<MovimentacaoParcela> mov = Enumerable.Range(0, quantidade).Select(i =>
            new MovimentacaoParcela(
                data.descricao,
                data.valor / quantidade,
                i + 1,
                data.categoriaId,
                data.tipo,
                vencimento.AddMonths(i))
        ).ToList();

        await this._movimentacoes.CriarMovimentacaoParcela(mov);
    }

}