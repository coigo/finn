using Aportes.Models;

namespace Aportes.DTOS;

public record HistoricoFiltrosDTO (
    DateTime Inicio,
    DateTime Fim,
    AporteCategoria Categoria
);