using Aportes.Models;

namespace Aportes.DTOS;

public record AtualizarAporteDTO(
    decimal PrecoMedio,
    string Identificador,
    decimal Quantidade,
    AporteCategoria Categoria
);