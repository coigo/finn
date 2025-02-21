using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Movimentacoes.Repositories;

namespace Movimentacoes.UseCases;

public class CriarMovimentacaoUseCase : IUseCase<CriarMovimentacao, Movimentacao> {

    private readonly IMovimentacaoRepository _movimentacoes;
    public CriarMovimentacaoUseCase ( IMovimentacaoRepository movimentacoes ) {
        _movimentacoes = movimentacoes;
    }

    public async Task<Movimentacao> Execute (CriarMovimentacao data) {
        
        var (valor, tipo, categoriaId, quantidadeParcelas, primeiroVencimento) = data;

        Movimentacao movimentacao = new (
            valor,
            tipo, 
            categoriaId
        );

        Movimentacao mov = await this._movimentacoes.CriarMovimentacao(movimentacao);

        var SalvarPorTipo = new Dictionary<MovimentacaoTipo, Action<Movimentacao, CriarMovimentacao>>{

            {MovimentacaoTipo.INVESTIMENTOS, CriarTipoInvestimento},
            {MovimentacaoTipo.ENTRADA, CriarTipoEntrada},
            {MovimentacaoTipo.SAIDA, CriarTipoSaida}
        };

        SalvarPorTipo[tipo](mov, data);

        return mov;
    }

    private async void CriarTipoSaida(Movimentacao movimentacao, CriarMovimentacao data) {

        var (valor, tipo, categoriaId, quantidadeParcelas, primeiroVencimento) = data;
        if (quantidadeParcelas != null && primeiroVencimento != null ) {

        int quantidade = quantidadeParcelas.Value;
        DateTime vencimento = primeiroVencimento.Value;

        for ( int i = 0; i < quantidadeParcelas; i ++ ) {
                MovimentacaoParcela parcela = new (
                    movimentacao, 
                    movimentacao.Valor / quantidade,
                    i + 1,
                    vencimento.AddMonths(i)
                );

                await this._movimentacoes.CriarParcelas(parcela);
            }
        }

    }

    private async void CriarTipoInvestimento(Movimentacao movimentacao, CriarMovimentacao data) {
        Console.WriteLine("testinho");
    }

    private async void CriarTipoEntrada(Movimentacao movimentacao, CriarMovimentacao data) {
        Console.WriteLine("testinho");
        
    }

}