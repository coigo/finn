using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aportes.Models;

[Table("aportes_historico")]
public class AporteHistorico {

    [Key()]
    [Column("id")]
    public int Id { get ; set; }
    
    [Column("precoMedio")]
    public float Preco { get ; set; }
    
    [Column("identificador")]
    public string Identificador { get ; set; }

    [Column("categoria")]
    public AporteCategoria Categoria { get ; set; }

    [Column("tipo")]
    public AporteTipo Tipo { get ; set; }

    [Column("data")]
    public DateTime Data { get ; set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get ; set; } = DateTime.Now;

    protected AporteHistorico () {}

    public AporteHistorico (
        float preco,
        string identificador,
        AporteTipo tipo,
        AporteCategoria categoria,
        DateTime data

    ) {
        Preco = preco;
        Identificador = identificador;
        Tipo = tipo;
        Categoria = categoria;
        Data = data;
    }

}
