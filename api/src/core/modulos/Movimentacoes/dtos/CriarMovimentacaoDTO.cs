using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record CriarMovimentacao (
    decimal valor,
    MovimentacaoTipo tipo,
    int categoriaId,

    int? quantidadeParcelas,
    DateTime? primeiroVencimento

) {}
