using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Model.Movimentacao;
using Repositories.Movimentacao;

namespace Repositories.Adapters.Movimentacao;

public class MovimentacaoRepository: IMovimentacaoRepository {

    public Context Context;

    public MovimentacaoRepository (Context context) {
        Context = context;
        
    }
    public async void CriarMovimentacao(MovimentacaoModel data) {
        await Context.AddAsync(data);
    }
    public async Task<MovimentacaoModel[]> BuscarMovimentacoes() {
        var movimentacoes = await Movimentacoes
            .Where(m => 
                m.criadoEm.Month == DateTime.Now.Month 
                && m.criadoEm.Year == DateTime.Now.Year )
            .ToListAsync();
        return movimentacoes;
    }
    public void AtualizarMovimentacao(int id, MovimentacaoModel data) {
        
    }

}