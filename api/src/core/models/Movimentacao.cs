using System.ComponentModel.DataAnnotations.Schema;
using Model.Movimentacao;

namespace Model.Movimentacao;

[Table("movimentacao")]
public class MovimentacaoModel {

    public int id { get ; set; }
    public float Valor { get ; set; }
    public MovimentacaoTipo Tipo { get ; set; }
    public required MovimentacaoCategoriaModel Categoria { get ; set; }
    public DateTime criadoEm { get ; set; } = DateTime.Now;   

    public MovimentacaoModel ( 
        float valor, 
        MovimentacaoTipo tipo, 
        MovimentacaoCategoriaModel categoria
        ) {
        
        Valor = valor;
        Tipo = tipo;
        Categoria = categoria;

    }

} 

