
namespace Aportes.DTOS;

public record AporteHistoricoDTO{
    public required string Identificador { get; init; }
    public decimal Quantidade { get; init; }
    public decimal Preco { get; init; }
    public DateTime Data { get; init; }
    public required string Tipo { get; init; }
}