using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;
using Movimentacoes.Factories;

namespace Movimentacoes.UseCases;

public class CriarPersistenteUseCase
{

    private readonly IMovimentacaoRepository _movimentacoes;

    public CriarPersistenteUseCase(IMovimentacaoRepository movimentacoes)
    {
        _movimentacoes = movimentacoes;
    }

    public async void Execute(CriarMovimentacao data)
    {
        var (valor, tipo, categoriaId, descricao, date, quantidadeParcelas, primeiroVencimento, persistente) = data;

        if (persistente != true)
        {
            throw new Exception("Para criar uma movimentacao persistente é necessário marca-la como persistente.");
        }

        MovimentacaoPersistente mov = new (
            descricao,
            valor,
            tipo,
            categoriaId
        );

        await this._movimentacoes.CriarMovimentacaoPersistente(mov);

    }

}