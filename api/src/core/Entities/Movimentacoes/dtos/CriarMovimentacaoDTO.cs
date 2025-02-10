using Movimentacoes.Models;

namespace Movimentacoes.DTOS;

public record CriarMovimentacao {
    public float valor;
    public MovimentacaoTipo tipo;
    public MovimentacaoCategoria categoria;
    
}