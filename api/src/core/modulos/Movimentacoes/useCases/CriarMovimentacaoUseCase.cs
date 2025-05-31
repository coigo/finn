using Infra.Shared;
using Movimentacoes.Models;
using Movimentacoes.DTOS;
using Infra.Repositories;
using Movimentacoes.Factories;

namespace Movimentacoes.UseCases;

public class CriarMovimentacaoUseCase : IUseCase<CriarMovimentacao, CriarMovimentacao>
{

    private readonly IMovimentacaoFactory _movFactory;
    private readonly IMovimentacaoRepository _movimentacoes;
    private readonly IResumoRepository _resumos;

    public CriarMovimentacaoUseCase(IMovimentacaoRepository movimentacoes, IResumoRepository resumos, IMovimentacaoFactory factory)
    {
        _movimentacoes = movimentacoes;
        _movFactory = factory;
        _resumos = resumos;
    }

    public async Task<CriarMovimentacao> Execute(CriarMovimentacao data)
    {

        var derivadosMovimentacao = this._movFactory.Execute(data);

        var coisa = new Dictionary<MovimentacoesDerivadosDTO, Func<Task>>
        {
            {MovimentacoesDerivadosDTO.MOVIMENTACAO, () => CriarMovimentacao(derivadosMovimentacao.Movimentacao) },
            {MovimentacoesDerivadosDTO.PERSISTENTE, () => CriarPersistente(derivadosMovimentacao.Persistente) },
            {MovimentacoesDerivadosDTO.PARCELAS, () => CriarParcela(derivadosMovimentacao.Parcela) }
        };
        Console.WriteLine();
        await coisa[derivadosMovimentacao.derivado]();
        return data;
    }

    private async Task CriarPersistente(MovimentacaoPersistente? data)
    {
        if (data == null) return;

        await this._movimentacoes.CriarMovimentacaoPersistente(data);
        
    }
    
    private async Task CriarParcela(List<MovimentacaoParcela>? data)
    {
        if (data == null) return;
        await this._movimentacoes.CriarMovimentacaoParcela(data);
    }
    
    private async Task CriarMovimentacao(Movimentacao? data) {

        if (data == null) return; 
        
        var SalvarPorTipo = new Dictionary<MovimentacaoTipo, Func<Movimentacao, Task>>{
            { MovimentacaoTipo.INVESTIMENTOS, CriarTipoInvestimento },
            { MovimentacaoTipo.ENTRADA, CriarTipoEntrada },
            { MovimentacaoTipo.SAIDA, CriarTipoSaida }
        };
        await SalvarPorTipo[data.Tipo](data);
        await this._movimentacoes.CriarMovimentacao(data);
    }

    private async Task CriarTipoEntrada(Movimentacao data)
    {
        int[] categorias = { (int)MovimentacaoCategoriaDTO.DIVIDENDO, (int)MovimentacaoCategoriaDTO.VENDA };

        if (!categorias.Contains(data.CategoriaId))
        {
            await this._resumos.AtualizarSaldo("Corrente", data.Valor);
        }
    }
    
    private async Task CriarTipoInvestimento(Movimentacao data)
    {
        await this._resumos.AtualizarSaldo("Corrente", -data.Valor);
    }
    private async Task CriarTipoSaida(Movimentacao data) {
        await this._resumos.AtualizarSaldo("Corrente", -data.Valor);
    }

}