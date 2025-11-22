using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Salarios.Models;

[Table("salarios")]
public class Salario {

    [Key()]
    [Column("id")]
    public int Id { get ; set; }
    
    [Column("valor")]
    public decimal Valor { get ; set; }
    
    [Column("criadoEm")]
    public DateTime CriadoEm { get ; set; } = DateTime.Now;

    public Salario (decimal valor) {
        Valor = valor;
    }

}