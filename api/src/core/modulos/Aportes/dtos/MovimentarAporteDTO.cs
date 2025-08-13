using Aportes.Models;

namespace Aportes.DTOS;

public record MovimentarAporteDTO(
    string Identificador,
    decimal Quantidade,
    decimal Preco,
    AporteCategoria Categoria,
    DateTime DataCompra
)
{
    public string Identificador { get; init; } = Identificador.Trim().ToUpper();
};