using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record CriarMovimentacao (
    float valor,
    MovimentacaoTipo tipo,
    MovimentacaoCategoria categoria,

    int? quantidadeParcelas,
    DateTime? primeiroVencimento

    
) {}