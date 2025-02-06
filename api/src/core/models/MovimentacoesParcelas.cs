using Model.Movimentacao;

namespace Model.Movimentacao;

public class MovimentacaoParcelaModel {

    public int Id { get; set; }
    public required MovimentacaoModel MovimentacaoId { get; set; }
    public required int Vumero { get; set; }
    public required float Valor { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
}