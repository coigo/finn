 using Model.Movimentacao;

namespace Model.Movimentacao;

public class MovimentacaoCategoriaModel {

    public int Id { get ; set; }
    public string Nome { get ; set; }
    public MovimentacaoTipo Tipo { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    public DateTime? DeletadoEm { get; set; }
    public List<MovimentacaoModel> Movimentacoes { get ; set; } = new List<MovimentacaoModel>();

    public MovimentacaoCategoriaModel (
        string nome,
        MovimentacaoTipo tipo
    ) {
        Nome = nome;
        Tipo = tipo;
    }

}

