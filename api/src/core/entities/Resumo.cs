using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movimentacoes.Models;

[Table("resumo")]
public class Resumo {

    [Key]
    [Column("id")]
    public int Id { get ; set; }

    [Column("valor")]
    public float Valor { get ; set; }

    [Column("nome")]
    public string Nome { get ; set; }

    public List<ResumoHistorico> ResumoHistorico { get ; set; } =  new List<ResumoHistorico>();


    protected Resumo () {}

    public Resumo ( 
        float valor, 
        string nome
        ) {
        
        Valor = valor;
        Nome = nome;

    }

} 

