using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;
using Movimentacoes.Factories;

namespace Movimentacoes.UseCases;

public class CriarMovimentacaoUseCase
{

    private readonly IMovimentacaoRepository _movimentacoes;
    private readonly ISaldoRepository _saldos;

    public CriarMovimentacaoUseCase (
        IMovimentacaoRepository movimentacoes, 
        ISaldoRepository saldos
    )
    {
        _movimentacoes = movimentacoes;
        _saldos = saldos;
    }

    public async void  Execute(CriarMovimentacao data)
    {

        var SalvarPorTipo = new Dictionary<MovimentacaoTipo, Func<Movimentacao, Task>>{
            { MovimentacaoTipo.INVESTIMENTOS, CriarTipoInvestimento },
            { MovimentacaoTipo.ENTRADA, CriarTipoEntrada },
            { MovimentacaoTipo.SAIDA, CriarTipoSaida }
        };

        Movimentacao mov = new (
            data.valor, 
            data.tipo, 
            data.categoriaId, 
            data.descricao, 
            data.data
        ); 
        await SalvarPorTipo[data.tipo](mov);
        await this._movimentacoes.CriarMovimentacao(mov);
    }

    private async Task CriarTipoEntrada(Movimentacao data)
    {
        int[] categorias = { (int)MovimentacaoCategoriaDTO.DIVIDENDO, (int)MovimentacaoCategoriaDTO.VENDA };

        if (!categorias.Contains(data.CategoriaId))
        {
            await this._saldos.AtualizarSaldo("Corrente", data.Valor);
        }
    }

    private async Task CriarTipoInvestimento(Movimentacao data)
    {
        await this._saldos.AtualizarSaldo("Corrente", -data.Valor);
    }
    private async Task CriarTipoSaida(Movimentacao data)
    {
        await this._saldos.AtualizarSaldo("Corrente", -data.Valor);
    }

}