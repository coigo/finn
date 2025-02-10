using Movimentacoes.Models;

namespace Movimentacoes.Models;

public class MovimentacaoParcela {

    
    public int Id { get; set; }
    public required Movimentacao MovimentacaoId { get; set; }
    public required int Numero { get; set; }
    public required float Valor { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
}