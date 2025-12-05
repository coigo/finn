using Aportes.Models;

namespace Aportes.DTOS;

public record BuscarAportesDTO (
    string Identificador,
    decimal PrecoMedio,
    decimal PrecoAtual,
    decimal Quantidade,
    string categoria

);