
namespace Movimentacoes.DTOS;

public record BuscarPorPeriodoDTO (string inicio, string fim);

public record ListaMovimentacoesDTO (
    int id,
    decimal valor,
    string tipo,
    string descricao,
    string categoria,
    DateTime data
);