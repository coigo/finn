using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movimentacoes.Models  ;

namespace Movimentacoes.Models;

[Table("movimentacao")]
public class Movimentacao {

    [Key]
    public int Id { get ; set; }
    public float Valor { get ; set; }
    public MovimentacaoTipo Tipo { get ; set; }
    public MovimentacaoCategoria Categoria { get ; set; }
    public DateTime criadoEm { get ; set; } = DateTime.Now;   

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

