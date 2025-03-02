using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aportes.Models;

[Table("aportes")]
public class Aporte {

    [Key()]
    [Column("id")]
    public int Id { get ; set; }
    
    [Column("precoMedio")]
    public float PrecoMedio { get ; set; }
    
    [Column("identificador")]
    public string Identificador { get ; set; }
    
    [Column("categoriaId")]
    public int CategoriaId { get ; set; }

    [ForeignKey("CategoriaId")]
    public AporteCategoria Categoria { get ; set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get ; set; } = DateTime.Now;

    protected Aporte () {}

    public Aporte (
        float precoMedio,
        string identificador,
        int categoriaId
    ) {
        PrecoMedio = precoMedio;
        Identificador = identificador;
        CategoriaId = categoriaId;


    }

}