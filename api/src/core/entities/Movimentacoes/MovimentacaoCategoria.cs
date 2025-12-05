using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movimentacoes.Models;

[Table("movimentacoes_categorias")]

public class MovimentacaoCategoria {

    [Key()]
    [Column("id")]
    public int Id { get ; set; }

    [Column("nome")]
    public string Nome { get ; set; }

    [Column("tipo")]
    public MovimentacaoTipo Tipo { get; set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get; set; } = DateTime.Now;

    [Column("deletadoEm")]
    public DateTime? DeletadoEm { get; set; }
    
    public List<Movimentacao> Movimentacoes { get ; set; } = new List<Movimentacao>();

    public MovimentacaoCategoria (
        string nome,
        MovimentacaoTipo tipo
    ) {
        Nome = nome;
        Tipo = tipo;
    }

}

