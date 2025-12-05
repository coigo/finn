using Movimentacoes.DTOS;
using Movimentacoes.Models;

public record MovimentacaoResultado 
(
    Movimentacao? Movimentacao,
    List<MovimentacaoParcela>? Parcela,
    MovimentacaoPersistente? Persistente,

    MovimentacoesDerivadosDTO derivado
) {}