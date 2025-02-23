using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;
using Resumos.Models;

namespace Movimentacoes.UseCases;

public class CriarMovimentacaoUseCase : IUseCase<CriarMovimentacao, CriarMovimentacao>
{

    private readonly IMovimentacaoRepository _movimentacoes;
    private readonly IResumoRepository _resumos;

    public CriarMovimentacaoUseCase(IMovimentacaoRepository movimentacoes, IResumoRepository resumos)
    {
        _movimentacoes = movimentacoes;
        _resumos = resumos;
    }

    public async Task<CriarMovimentacao> Execute(CriarMovimentacao data)
    {
        var (valor, tipo, categoriaId, quantidadeParcelas, primeiroVencimento) = data;

        var SalvarPorTipo = new Dictionary<MovimentacaoTipo, Func<CriarMovimentacao, Task>>{
            {MovimentacaoTipo.INVESTIMENTOS, CriarTipoInvestimento},
            {MovimentacaoTipo.ENTRADA, CriarTipoEntrada},
            {MovimentacaoTipo.SAIDA, CriarTipoSaida}
        };
        await SalvarPorTipo[tipo](data);

        return data;
    }

    private async Task CriarTipoSaida(CriarMovimentacao data)
    {
        var (valor, tipo, categoriaId, quantidadeParcelas, primeiroVencimento) = data;
        
        Movimentacao movimentacao = await this.CriarMovimentacao(data);

            Console.WriteLine("testinho");
        if (quantidadeParcelas != null && primeiroVencimento != null)
        {
            int quantidade = quantidadeParcelas.Value;
            DateTime vencimento = primeiroVencimento.Value;

            var parcelas = Enumerable.Range(0, quantidade).Select(i => 
                new MovimentacaoParcela(
                    movimentacao,
                    movimentacao.Valor / quantidade,
                    i + 1,
                    vencimento.AddMonths(i))
            );
            await this._movimentacoes.CriarParcelas(parcelas);
        }
        else {
            await this._resumos.AtualizarSaldo("Corrente", -valor);
        }
    }

    private async Task CriarTipoInvestimento(CriarMovimentacao data)
    {
        Movimentacao mov = await this.CriarMovimentacao(data);

        await this._resumos.AtualizarSaldo("Corrente", -data.valor);
        await this._resumos.AtualizarSaldo("Investimentos", data.valor);
    }

    private async Task CriarTipoEntrada(CriarMovimentacao data)
    {
        var (valor, tipo, categoriaId, quantidadeParcelas, primeiroVencimento) = data;

        int[] categorias = { (int)MovimentacaoCategoriaDTO.DIVIDENDO, (int)MovimentacaoCategoriaDTO.VENDA };

        if (categorias.Contains(categoriaId)) {
            await this._resumos.AtualizarSaldo("Investimentos", valor);
        }
        else {
            await this._resumos.AtualizarSaldo("Corrente", valor);
        }
        await this.CriarMovimentacao(data);

    }

    private async Task<Movimentacao> CriarMovimentacao(CriarMovimentacao data)
    {
        var (valor, tipo, categoriaId, quantidadeParcelas, primeiroVencimento) = data;

        Movimentacao movimentacao = new(
            valor,
            tipo,
            categoriaId
        );

        Movimentacao mov = await this._movimentacoes.CriarMovimentacao(movimentacao);
        return mov;
    }

}