using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record CriarMovimentacao (
    decimal valor,
    MovimentacaoTipo tipo,
    int categoriaId,
    string descricao,
    DateTime data,

    int? quantidadeParcelas,
    DateTime? primeiroVencimento

) {}
