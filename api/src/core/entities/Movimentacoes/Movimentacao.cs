using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movimentacoes.Models;

[Table("movimentacoes")]
public class Movimentacao {

    [Key]
    [Column("id")]
    public int Id { get ; set; }

    [Column("valor")]
    public decimal Valor { get ; set; }

    [Column("tipo")]
    public MovimentacaoTipo Tipo { get ; set; }

    [Column("categoriaId")]
    public int  CategoriaId { get ; set; }

    [Column("descricao")]
    public string Descricao { get ; set; }

    [Column("data")]
    public DateTime Data { get ; set; }

    [Column("parcelaId")]
    public int? ParcelaId { get; set; }

    [Column("persistenteId")]
    public int? PersistenteId { get; set; }

    [Column("criadoEm")]
    public DateTime CriadoEm { get ; set; } = DateTime.Now;   
    
    [ForeignKey("CategoriaId")]
    public MovimentacaoCategoria Categoria { get ; set; }
    
    [ForeignKey("ParcelaId")]
    public MovimentacaoParcela? Parcela { get; set; }

    [ForeignKey("PersistenteId")]
    public MovimentacaoPersistente? Persistente { get; set; }

    protected Movimentacao() { }

    public Movimentacao ( 
        decimal valor, 
        MovimentacaoTipo tipo, 
        int categoriaId,
        string descricao,
        DateTime data
        ) {
        
        Valor = valor;
        Tipo = tipo;
        CategoriaId = categoriaId;
        Descricao = descricao;
        Data = data;

    }

} 

