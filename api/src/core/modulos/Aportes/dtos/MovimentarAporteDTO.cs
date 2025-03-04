using Aportes.Models;

namespace Aportes.DTOS;

public record MovimentarAporteDTO (
    string Identificador,
    float Quantidade,
    float Preco,
    AporteCategoria Categoria,
    DateTime DataCompra
);