using Movimentacoes.DTOS;
using Movimentacoes.Models;

namespace Movimentacoes.Factories;

public class MovimentacaoFactory : IMovimentacaoFactory
{

    private Movimentacao? Movimentacao;
    private List<MovimentacaoParcela>? Parcela;
    private MovimentacaoPersistente? Persistente;

    private MovimentacoesDerivadosDTO derivado;

    public MovimentacaoResultado Execute (CriarMovimentacao data)
    {

        if (data.quantidadeParcelas != null)
            this.CriarParcelas(data);
        else if (data.persistente == true)
            this.CriarMovimentacaoPersistente(data);
        else
            this.CriarMovimentacao(data);

        return new MovimentacaoResultado(
            this.Movimentacao,
            this.Parcela,
            this.Persistente,
            this.derivado   
        );
    }

    private void CriarMovimentacao(CriarMovimentacao data)
    {
        var (valor, tipo, categoriaId, descricao, date, quantidadeParcelas, primeiroVencimento, persistente) = data;

        this.Movimentacao = new(
            valor,
            tipo,
            categoriaId,
            descricao,
            primeiroVencimento ?? date
        );
        this.derivado = MovimentacoesDerivadosDTO.MOVIMENTACAO;

    }
    private void CriarParcelas(CriarMovimentacao data)
    {
        var (valor, tipo, categoriaId, descricao, date, quantidadeParcelas, primeiroVencimento, persistente) = data;

        int quantidade = quantidadeParcelas.Value;
        DateTime vencimento = primeiroVencimento.Value;

        this.Parcela = Enumerable.Range(0, quantidade).Select(i =>
            new MovimentacaoParcela(
                descricao,
                valor / quantidade,
                i + 1,
                categoriaId,
                tipo,
                vencimento.AddMonths(i))
        ).ToList();
        this.derivado = MovimentacoesDerivadosDTO.PARCELAS;

    }

    private void CriarMovimentacaoPersistente(CriarMovimentacao data)
    {
        var (valor, tipo, categoriaId, descricao, date, quantidadeParcelas, primeiroVencimento, persistente) = data;

        this.Persistente =  new (
            descricao,
            valor,
            tipo,
            categoriaId
        );
        this.derivado = MovimentacoesDerivadosDTO.PERSISTENTE;
        
        

    }
}