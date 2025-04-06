using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aportes.Models;

[Table("aportes")]
public class Aporte {

    [Key()]
    [Column("id")]
    public int Id { get ; set; }
    
    [Column("identificador")]
    public string Identificador { get ; set; }

    [Column("precoMedio")]
    public decimal PrecoMedio { get ; set; }
    
    [Column("quantidade")]
    public decimal Quantidade { get ; set; }
    
    [Column("categoria")]
    public AporteCategoria Categoria { get ; set; }



    [Column("criadoEm")]
    public DateTime CriadoEm { get ; set; } = DateTime.Now;

    protected Aporte () {}

    public Aporte (
        decimal precoMedio,
        string identificador,
        decimal quantidade,
        AporteCategoria categoria
    ) {
        PrecoMedio = precoMedio;
        Identificador = identificador;
        Quantidade = quantidade;
        Categoria = categoria;
    }

}