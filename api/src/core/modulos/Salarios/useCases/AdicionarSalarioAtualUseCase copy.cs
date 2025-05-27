using Infra.Shared;
using Infra.Repositories;
using Salarios.Models;
using Movimentacoes.Models;

namespace Salarios.UseCases;

public class AdicionarSalarioAtualUseCase : IUseCase<Unity, Salario>
{

    private readonly ISalarioRepository _salario;
    private readonly IMovimentacaoRepository _movimentacao;
    private readonly IResumoRepository _resumo;

    public AdicionarSalarioAtualUseCase(ISalarioRepository salario, IMovimentacaoRepository movimentacao, IResumoRepository resumo)
    {
        _salario = salario;
        _movimentacao = movimentacao;
        _resumo = resumo;
    }

    public async Task<Salario> Execute(Unity _)
    {
        var salario = await this._salario.BuscarUltimo();

        Movimentacao mov = new(
            salario.Valor,
            MovimentacaoTipo.ENTRADA,
            9,
            "Salário",
            DateTime.Now);

        await this._resumo.AtualizarSaldo("Corrente", salario.Valor);
        await this._movimentacao.CriarMovimentacao(mov);

        return salario;
    }

}