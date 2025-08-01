using Aportes.DTOS;
using Aportes.Models;
using Infra.Repositories;
using Infra.Shared;

namespace Aportes.UseCases;

public class MovimentarAportesUseCase : IUseCase<MovimentarAporteDTO, Aporte>
{

    private readonly IAporteRepository _aportes;
    private readonly IAporteHistoricoRepository _historico;
    private readonly IResumoRepository _resumo;

    public MovimentarAportesUseCase(
        IAporteRepository aportes,
        IAporteHistoricoRepository historico,
        IResumoRepository resumo
    )
    {
        _aportes = aportes;
        _historico = historico;
        _resumo = resumo;
    }

    public async Task<Aporte> Execute(MovimentarAporteDTO data)
    {
        var ( Identificador, Quantidade, Preco, Categoria, DataCompra ) = data;
        var aporte = await this._aportes.BuscarPorIdentificador(data.Identificador);
        
        var tipo = Quantidade < 0 ? AporteTipo.VENDA : AporteTipo.COMPRA; 
        await this._historico.CriarRegistro(new AporteHistorico(Preco, Identificador, tipo, Categoria, DataCompra));

        return aporte == null 
        ? await this.CriarAporte(data)
        : await this.AtualizarAporte(data, aporte);

    }

    private async Task<Aporte> CriarAporte(MovimentarAporteDTO data) {
        Aporte novoAporte = new ( data.Preco, data.Identificador, data.Quantidade, data.Categoria );
        return await this._aportes.CriarAporte(novoAporte);
    }

    private async Task<Aporte> AtualizarAporte(MovimentarAporteDTO data, Aporte aporte) {
            var novaQuantidade = aporte.Quantidade + data.Quantidade;
            var precoMedio = (( aporte.PrecoMedio * aporte.Quantidade ) + ( data.Preco * data.Quantidade )) / novaQuantidade;

            AtualizarAporteDTO aporteAtualizado = new (precoMedio, data.Identificador, aporte.Quantidade + data.Quantidade, data.Categoria);
            await this._aportes.AtualizarAporte(aporte.Id, aporteAtualizado);
            return aporte;
            
    }

}