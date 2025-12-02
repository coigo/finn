namespace Movimentacoes.DTOS;

public record EditarPersistenteDTO(
    int PersistenteId,
    decimal Valor,
    int CategoriaId,
    string Descricao
);