using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movimentacoes.Models;

[Table("movimentacoes")]
public class Movimentacao {

    [Key]
    [Column("id")]
    public int Id { get ; set; }

    [Column("valor")]
    public float Valor { get ; set; }

    [Column("tipo")]
    public MovimentacaoTipo Tipo { get ; set; }

    [Column("categoria_id")]
    public int  CategoriaId { get ; set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get ; set; } = DateTime.Now;   
    
    [Column("criadoEm")]
    public List<MovimentacaoParcela> MovimentacaoParcelas { get ; set; } = new List<MovimentacaoParcela> ();   

    [ForeignKey("CategoriaId")]
    public MovimentacaoCategoria Categoria { get ; set; }

    protected Movimentacao () {}

    public Movimentacao ( 
        float valor, 
        MovimentacaoTipo tipo, 
        MovimentacaoCategoria categoria
        ) {
        
        Valor = valor;
        Tipo = tipo;
        Categoria = categoria;

    }

} 

