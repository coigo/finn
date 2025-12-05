using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saldos.Models;

[Table("saldos_historico")]
public class SaldoHistorico
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("valor")]
    public decimal Valor { get; set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get; set; } = DateTime.Now;

    [Column("resumoId")]
    public int SaldoId { get; set; }

    [ForeignKey("SaldoId")]
    public Saldo Saldo { get; set; }



    protected SaldoHistorico() { }

    public SaldoHistorico(
        decimal valor
        )
    {

        Valor = valor;

    }

}

