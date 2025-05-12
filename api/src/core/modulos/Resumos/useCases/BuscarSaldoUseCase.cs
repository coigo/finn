using Infra.Repositories;
using Infra.Shared;
using Resumos.Models;

namespace Resumos.UseCases;

public class BuscarSaldoUseCase : IUseCase<string, Resumo> {

    private IResumoRepository _resumo;
    
    public BuscarSaldoUseCase (IResumoRepository resumo) {
        _resumo = resumo;
    }

    public async Task<Resumo> Execute (string saldoNome) {
        return await this._resumo.BuscarResumoPorNome(saldoNome);
    }

}