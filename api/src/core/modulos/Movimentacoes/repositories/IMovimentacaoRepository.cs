
using Movimentacoes.DTOS;
using Movimentacoes.Models;

namespace Infra.Repositories;

public interface IMovimentacaoRepository {

    public Task<Movimentacao> CriarMovimentacao(Movimentacao data);
    public Task CriarMovimentacaoParcela(List<MovimentacaoParcela> data);
    public Task<MovimentacaoPersistente> CriarMovimentacaoPersistente(MovimentacaoPersistente data);
    
    public Task<List<ListaMovimentacoesDTO>> BuscarPorPeriodo(DateTime inicio, DateTime fim);
    public Task<Movimentacao> AtualizarMovimentacao(int id, Movimentacao data);

    public Task<List<MovimentacaoParcela>> CriarParcelas(IEnumerable<MovimentacaoParcela> data);
    public Task<List<MovimentacaoParcela>> BuscarParcelasPorId( int[] parcelas );
    public Task<List<MovimentacaoParcela>> BuscarParcelasDoMes( DateTime hoje );

    public Task<MovimentacaoCategoria> BuscarCategoriaPorNome(string nome);
    public Task<List<BuscarMovimentacaoCategoriaDTO>> BuscarCategorias();

}