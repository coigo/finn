using Infra.Shared;
using Infra.Repositories;
using Salarios.Models;
using Movimentacoes.Models;

namespace Salarios.UseCases;

public class AdicionarSalarioAtualUseCase : IUseCase<Unity, Salario>
{

    private readonly ISalarioRepository _salario;
    private readonly IMovimentacaoRepository _movimentacao;
    private readonly ISaldoRepository _saldo;

    public AdicionarSalarioAtualUseCase(ISalarioRepository salario, IMovimentacaoRepository movimentacao, ISaldoRepository saldo)
    {
        _salario = salario;
        _movimentacao = movimentacao;
        _saldo = saldo;
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

        await this._saldo.AtualizarSaldo("Corrente", salario.Valor);
        await this._movimentacao.CriarMovimentacao(mov);

        return salario;
    }

}