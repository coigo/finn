namespace Movimentacoes.DTOS;

public record ListaPendentesDTO (
    decimal Valor,
    string Categoria,
    DateTime? Vencimento,
    string Tipo 
);