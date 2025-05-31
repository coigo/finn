using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movimentacoes.Models;

[Table("movimentacoes_persistentes")]
public class MovimentacaoPersistente {

    [Key()]
    [Column("id")]
    public int Id { get; private set; }

    [Column("valor")]
    public decimal Valor { get; private set; }

    [Column("descricao")]
    public string Descricao { get; private set; }

    [Column("tipo")]
    public MovimentacaoTipo Tipo { get ; set; }

    [Column("categoriaId")]
    public int  CategoriaId { get ; set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get; private set; } = DateTime.Now;

    protected MovimentacaoPersistente() { }

    public MovimentacaoPersistente(string descricao,decimal valor, MovimentacaoTipo tipo, int categoriaId ) {
        Descricao = descricao;
        Valor = valor;
        CategoriaId = categoriaId;
        Tipo = tipo;
    }
}
