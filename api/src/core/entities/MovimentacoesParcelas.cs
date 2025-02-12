using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movimentacoes.Models;

[Table("movimentacoes_parcelas")]
public class MovimentacaoParcela {

    [Key()]
    [Column("id")]
    public int Id { get; private set; }

    [Column("movimentacao_id")]
    public int MovimentacaoId { get; private set; }

    [ForeignKey("MovimentacaoId")]
    public Movimentacao Movimentacao { get; private set; }  

    [Column("valor")]
    public float Valor { get; private set; }

    [Column("numero")]
    public int Numero { get; private set; }
    
    [DataType(DataType.Date)]
    [Column("vencimento")]
    public DateTime Vencimento { get; private set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get; private set; } = DateTime.Now;

    protected MovimentacaoParcela() { }

    public MovimentacaoParcela(Movimentacao movimentacao, float valor, int numero, DateTime vencimento) {
        Movimentacao = movimentacao ?? throw new ArgumentNullException(nameof(movimentacao));
        MovimentacaoId = movimentacao.Id;
        Valor = valor;
        Numero = numero;
        Vencimento = vencimento;
    }
}
