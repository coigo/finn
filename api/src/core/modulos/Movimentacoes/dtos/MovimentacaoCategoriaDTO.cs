using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record BuscarMovimentacaoCategoriaDTO (
    int Id,
    string Name,
    string Tipo
) {}
