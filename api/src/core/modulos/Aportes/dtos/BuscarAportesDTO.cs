using Aportes.Models;

namespace Aportes.DTOS;

public record BuscarAportesDTO (
    string Identificador,
    string NomeCurto,
    decimal PrecoMedio,
    decimal PrecoAtual,
    decimal Quantidade,
    AporteCategoria categoria

);