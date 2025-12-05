using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Infra.Shared;
using Movimentacoes.DTOS;
using Movimentacoes.Models;
using Movimentacoes.UseCases;

namespace Movimentacoes.Factories;

public class MovimentacaoFactory
{

    private CriarMovimentacaoUseCase movimentacao; 
    private CriarPersistenteUseCase persistente;
    private CriarParcelaUseCase parcela;


    public MovimentacaoFactory ( 
        CriarMovimentacaoUseCase movimentacao, 
        CriarPersistenteUseCase persistente, 
        CriarParcelaUseCase parcela
        )
    {
        this.movimentacao = movimentacao;
        this.persistente = persistente;
        this.parcela = parcela;
    }

    private Movimentacao? Movimentacao;
    private List<MovimentacaoParcela>? Parcela;
    private MovimentacaoPersistente? Persistente;


    public async void Execute (CriarMovimentacao data)
    {
        var tipoMovimentacao = this.DefineMovimentacao(data);
        var methods = new Dictionary<MovimentacoesDerivadosDTO, Action<CriarMovimentacao>>
        {
            { MovimentacoesDerivadosDTO.MOVIMENTACAO, this.movimentacao.Execute },
            { MovimentacoesDerivadosDTO.PERSISTENTE, this.persistente.Execute },
            { MovimentacoesDerivadosDTO.PARCELAS, this.parcela.Execute }
        };
        methods[tipoMovimentacao](data);

    }

    private MovimentacoesDerivadosDTO DefineMovimentacao (CriarMovimentacao data)
    {
        if (data.quantidadeParcelas != null)
        {
            return MovimentacoesDerivadosDTO.PARCELAS;
        }
        else if (data.persistente == true)
        {
            return MovimentacoesDerivadosDTO.PERSISTENTE;
        }
        return MovimentacoesDerivadosDTO.MOVIMENTACAO;
    }
}