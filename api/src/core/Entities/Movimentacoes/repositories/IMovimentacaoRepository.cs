
using Model.Movimentacao;

namespace Repositories.Movimentacao;

interface IMovimentacaoRepository {

    public void CriarMovimentacao(MovimentacaoModel data);
    public Task<MovimentacaoModel[]> BuscarMovimentacoes();
    public void AtualizarMovimentacao(int id, MovimentacaoModel data);

}