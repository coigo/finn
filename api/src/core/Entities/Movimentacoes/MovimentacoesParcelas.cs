using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movimentacoes.Models;

[Table("movimentacoes_parcelas")]
public class MovimentacaoParcela {

    [Key()]
    [Column("id")]
    public int Id { get; private set; }

    [Column("valor")]
    public decimal Valor { get; private set; }

    [Column("numero")]
    public int Numero { get; private set; }

    [Column("descricao")]
    public string Descricao { get; private set; }
    
    [DataType(DataType.Date)]
    [Column("vencimento")]
    public DateTime Vencimento { get; private set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get; private set; } = DateTime.Now;

    [Column("paga")]
    public bool Paga { get; private set; } = false;

    protected MovimentacaoParcela() { }

    public MovimentacaoParcela(string descricao, decimal valor, int numero, DateTime vencimento) {
        Descricao = descricao;
        Valor = valor;
        Numero = numero;
        Vencimento = vencimento;
    }
}
