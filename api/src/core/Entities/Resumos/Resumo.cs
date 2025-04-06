using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumos.Models;

[Table("resumo")]
public class Resumo {

    [Key]
    [Column("id")]
    public int Id { get ; set; }

    [Column("valor")]
    public decimal Valor { get ; set; }

    [Column("nome")]
    public string Nome { get ; set; }

    public List<ResumoHistorico> ResumoHistorico { get ; set; } =  new List<ResumoHistorico>();


    protected Resumo () {}

    public Resumo ( 
        decimal valor, 
        string nome
        ) {
        
        Valor = valor;
        Nome = nome;

    }

} 

