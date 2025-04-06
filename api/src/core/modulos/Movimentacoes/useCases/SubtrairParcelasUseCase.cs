using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;
using System.Numerics;
using api.Migrations;

namespace Movimentacoes.UseCases;

public class SubtrairParcelasUseCase  : IUseCase<SubtrairParcelas, SubtrairParcelas> {

    private readonly IMovimentacaoRepository _movimentacoes;
    private readonly IResumoRepository _resumo;

    public SubtrairParcelasUseCase  ( IMovimentacaoRepository movimentacoes, IResumoRepository resumo ) {
        _movimentacoes = movimentacoes;
        _resumo = resumo;
    }

    public async Task<SubtrairParcelas> Execute (SubtrairParcelas data) {

        var parcelas = await this._movimentacoes.BuscarParcelasPorId(data.parcelas);

        decimal valorTotal = parcelas.Sum(p => p.Valor);
        await this._resumo.AtualizarSaldo("Corrente", -valorTotal);
        return data;
    }

}