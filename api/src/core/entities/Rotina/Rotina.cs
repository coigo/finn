using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rotinas.Models;

[Table("rotina")]
public class Rotina {

    [Key]
    [Column("id")]
    public int Id { get ; set; }

    [Column("data")]
    public DateTime Data { get; set; } = DateTime.Now;

    public Rotina () {}

} 
