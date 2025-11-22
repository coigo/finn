using Aportes.DTOS;
using Aportes.Models;
using Infra.Repositories;
using Infra.Shared;

namespace Aportes.UseCases;

public class MovimentarAportesUseCase : IUseCase<MovimentarAporteDTO, Aporte>
{

    private readonly IAporteRepository _aportes;
    private readonly IAtivosRepository _ativos;
    private readonly IAporteHistoricoRepository _historico;

    public MovimentarAportesUseCase(
        IAporteRepository aportes,
        IAtivosRepository ativos,
        IAporteHistoricoRepository historico
    )
    {
        _aportes = aportes;
        _ativos = ativos;
        _historico = historico;
    }

    public async Task<Aporte> Execute(MovimentarAporteDTO data)
    {
        var ( Identificador, Quantidade, Preco, Categoria, DataCompra ) = data;
        var aporte = await this._aportes.BuscarPorIdentificador(data.Identificador);
        
        var tipo = Quantidade < 0 ? AporteTipo.VENDA : AporteTipo.COMPRA; 
        await this._historico.CriarRegistro(new AporteHistorico(Preco, Identificador, Quantidade, tipo, Categoria, DataCompra));

        return aporte == null 
        ? await this.CriarAporte(data)
        : await this.AtualizarAporte(data, aporte);

    }

    private async Task<Aporte> CriarAporte(MovimentarAporteDTO data) {
        var aporteExiste = await this._ativos.BuscarPorTicker(data.Identificador);
        if (aporteExiste == null)
        {
            throw new BusinessError($"O Ticker {data.Identificador} não existe! ");
        }
        
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