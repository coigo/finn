using Aportes.DTOS;
using Aportes.Models;
using Infra.Repositories;
using Infra.Shared;

namespace Aportes.UseCases;

public class MovimentarAportesPorIdentificadorUseCase : IUseCase<MovimentarAportePorIdentificadorDTO, Aporte>
{
    private readonly IAporteRepository _aportes;
    private readonly IAporteHistoricoRepository _historico;

    public MovimentarAportesPorIdentificadorUseCase(
        IAporteRepository aportes,
        IAporteHistoricoRepository historico
    )
    {
        _aportes = aportes;
        _historico = historico;
    }

    public async Task<Aporte> Execute(MovimentarAportePorIdentificadorDTO dto)
    {
        var aporte = await _aportes.BuscarPorIdentificador(dto.Identificador)
            ?? throw new BusinessError("Aporte não encontrado");

        var aporteAtualizado = await AtualizarAporte(dto, aporte);
        var historico = new AporteHistorico(
            dto.Preco,
            dto.Identificador,
            dto.Quantidade,
            dto.Tipo,
            aporte.Categoria,
            dto.Data
        );
        await _historico.CriarRegistro(historico);

        return aporteAtualizado;
    }

    private async Task<Aporte> AtualizarAporte(MovimentarAportePorIdentificadorDTO dto, Aporte aporte)
    {
        var novaQuantidade = GarantirValor(CalcularQuantidade(dto, aporte), "Quantidade inválida");
        Console.WriteLine(novaQuantidade);
        var precoMedio = GarantirValor(CalcularPrecoMedio(dto, aporte, novaQuantidade), "Preço médio inválido");
        Console.WriteLine(precoMedio);

        var dtoAtualizado = new AtualizarAporteDTO(
            precoMedio,
            dto.Identificador,
            novaQuantidade,
            aporte.Categoria
        );

        await  _aportes.AtualizarAporte(aporte.Id, dtoAtualizado);

        return aporte;
    }

    private decimal? CalcularQuantidade(MovimentarAportePorIdentificadorDTO dto, Aporte aporte)
    {
        if (dto.Quantidade <= 0) return null;

        return dto.Tipo switch
        {
            AporteTipo.COMPRA => aporte.Quantidade + dto.Quantidade,
            AporteTipo.VENDA => aporte.Quantidade - dto.Quantidade,
            AporteTipo.DESDOBRAMENTO => aporte.Quantidade * dto.Quantidade,
            _ => aporte.Quantidade
        };
    }

    private decimal? CalcularPrecoMedio(MovimentarAportePorIdentificadorDTO dto, Aporte aporte, decimal novaQuantidade)
    {
        if (novaQuantidade < 0) return null;

        return dto.Tipo switch
        {
            AporteTipo.COMPRA => ((aporte.PrecoMedio * aporte.Quantidade) + (dto.Preco * dto.Quantidade)) / novaQuantidade,
            AporteTipo.DESDOBRAMENTO => aporte.PrecoMedio * (aporte.Quantidade / novaQuantidade),
            AporteTipo.VENDA => novaQuantidade == 0 ? 0 : aporte.PrecoMedio,
            _ => aporte.PrecoMedio
        };
    }

    private static decimal GarantirValor(decimal? valor, string mensagemErro)
    {
        return valor ?? throw new BusinessError(mensagemErro);
    }
}
