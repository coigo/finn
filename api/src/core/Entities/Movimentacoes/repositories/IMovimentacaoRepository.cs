
using Model.Movimentacao;

namespace Repositories.Movimentacao;

interface IMovimentacaoRepository {

    public Task<MovimentacaoModel> CriarMovimentacao(MovimentacaoModel data);
    public Task<List<MovimentacaoModel>> BuscarPorPeriodo(DateTime inicio, DateTime fim);
    public Task<MovimentacaoModel> AtualizarMovimentacao(int id, MovimentacaoModel data);

}