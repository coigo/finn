
using Movimentacoes.DTOS;
using Movimentacoes.Models;

namespace Movimentacoes.Repositories;

public interface IMovimentacaoRepository {

    public Task<Movimentacao> CriarMovimentacao(Movimentacao data);
    public Task<List<ListaMovimentacoesDTO>> BuscarPorPeriodo(DateTime inicio, DateTime fim);
    public Task<Movimentacao> AtualizarMovimentacao(int id, Movimentacao data);

    public Task<MovimentacaoParcela> CriarParcelas(MovimentacaoParcela data);

}