using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record BuscarPorPeriodoDTO (string inicio, string fim);

public record ListaMovimentacoesDTO (
    decimal valor,
    MovimentacaoTipo tipo,
    int categoria_id,
    DateTime data,
    DateTime? vencimento
);