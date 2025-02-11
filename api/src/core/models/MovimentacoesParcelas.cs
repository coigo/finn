using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movimentacoes.Models;

[Table("movimentacoes_parcelas")]
public class MovimentacaoParcela {

    [Key()]
    [Column("id")]
    public int Id { get; set; }

    [Column("movimentacao_id")]
    public int MovimentacaoId { get; set; }

    [ForeignKey("MovimentacaoId")]
    public required Movimentacao Movimentacao { get; set; }

    [Column("numero")]
    public required int Numero { get; set; }

    [Column("valor")]
    public required float Valor { get; set; }
    
    [Column("criadoEm")]
    public DateTime CriadoEm { get; set; } = DateTime.Now;
}