using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record ListaPendentesDTO (
    int Id,
    decimal Valor,
    string Descricao,
    string Categoria,
    int CategoriaId,
    DateTime? Vencimento,
    MovimentacaoTipo Tipo,
    string TipoDerivado
);

public enum Periodo
{
    MES_ATUAL,
    PROXIMO_MES
}
public record PendentesDTO(
    List<ListaPendentesDTO> pendentes,
    Periodo periodo
);