using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record BuscarPorPeriodoDTO (string inicio, string fim);

public record ListaMovimentacoesDTO (
    decimal valor,
    string tipo,
    string categoria,
    DateTime data,
    DateTime? vencimento
);