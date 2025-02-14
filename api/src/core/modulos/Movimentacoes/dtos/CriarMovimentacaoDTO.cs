using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record CriarMovimentacao (
    float valor,
    MovimentacaoTipo tipo,
    int categoriaId,

    int? quantidadeParcelas,
    DateTime? primeiroVencimento

) {}

//	"quantidadeParcelas":undefined,
//"primeiroVencimento":undefined