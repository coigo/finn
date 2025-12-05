using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saldos.Models;

[Table("saldos")]
public class Saldo
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("valor")]
    public decimal Valor { get; set; }

    [Column("nome")]
    public string Nome { get; set; }

    public List<SaldoHistorico> SaldoHistorico { get; set; } = new List<SaldoHistorico>();


    protected Saldo() { }

    public Saldo(
        decimal valor,
        string nome
        )
    {

        Valor = valor;
        Nome = nome;

    }

}

