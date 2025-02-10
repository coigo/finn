 using Movimentacoes.Models;

namespace Movimentacoes.Models;

public class MovimentacaoCategoria {

    public int Id { get ; set; }
    public string Nome { get ; set; }
    public MovimentacaoTipo Tipo { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
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

