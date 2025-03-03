using Aportes.Models;

namespace Aportes.DTOS;

public record HistoricoFiltrosDTO (
    bool FiltrarPeriodo,
    DateTime? Inicio,
    DateTime? Fim,

    bool FiltrarCategoria,
    AporteCategoria? Categoria
);