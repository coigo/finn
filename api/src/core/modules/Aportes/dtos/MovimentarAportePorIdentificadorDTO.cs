using Aportes.Models;

public record MovimentarAportePorIdentificadorDTO(
    string? identificador,
    decimal Quantidade,
    decimal? Preco,
    string TipoStr,
    DateTime Data
)
{
    public string Identificador = identificador?.Trim().ToUpper() ?? string.Empty;

    public AporteTipo Tipo => TipoStr.ToUpper() switch
    {
        "COMPRA" => AporteTipo.COMPRA,
        "VENDA" => AporteTipo.VENDA,
        "DESDOBRAMENTO" => AporteTipo.DESDOBRAMENTO,
        _ => throw new ArgumentException("Tipo inválido")
    };
}
