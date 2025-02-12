using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record CriarMovimentacao (
    float valor,
    string tipo,
    int categoria,

    int? quantidadeParcelas,
    DateTime? primeiroVencimento

) {}

//	"quantidadeParcelas":undefined,
//"primeiroVencimento":undefined