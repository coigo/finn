using Aportes.Models;

namespace Aportes.DTOS;

public record AtualizarAporteDTO(
    float PrecoMedio,
    string Identificador,
    float Quantidade,
    AporteCategoria Categoria
);