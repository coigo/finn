using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumos.Models;

[Table("resumo_historico")]
public class ResumoHistorico {

    [Key]
    [Column("id")]
    public int Id { get ; set; }

    [Column("valor")]
    public float Valor { get ; set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get ; set; } = DateTime.Now;

    [Column("resumoId")]
    public int ResumoId { get ; set; }

    [ForeignKey("ResumoId")]
    public Resumo Resumo { get ; set; }



    protected ResumoHistorico () {}

    public ResumoHistorico ( 
        float valor 
        ) {
        
        Valor = valor;

    }

} 

