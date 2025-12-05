using Infra.Repositories;
using Movimentacoes.Models;

namespace Movimentacoes.UseCases;

public class ProcessarMovimentacoesPendentesUseCase
{

    private readonly IMovimentacaoRepository _movimentacoes;
    private readonly ISaldoRepository _saldo;
    public ProcessarMovimentacoesPendentesUseCase(IMovimentacaoRepository movimentacoes, ISaldoRepository saldo)
    {
        _movimentacoes = movimentacoes;
        _saldo = saldo;
    }

    public async Task Execute()
    {
        var pendentes = await this._movimentacoes.BuscarPendentesDoMes(DateTime.Now);
        if (pendentes.Count == 0 )
        {
            var nextMonth = DateTime.Now.AddMonths(1);
            pendentes = await this._movimentacoes.BuscarPendentesDoMes(nextMonth);
            Console.WriteLine("O proximo mes foi buscado");
        }
        foreach (var mov in pendentes)
        {
            Movimentacao novaMovimentacao = new(
                mov.Valor,
                mov.Tipo,
                mov.CategoriaId,
                mov.Descricao,
                DateTime.Now
            );

            var atualizarSaldo = this._saldo.AtualizarSaldo("Corrente",

            mov.Tipo == MovimentacaoTipo.ENTRADA
                ? novaMovimentacao.Valor
                : -novaMovimentacao.Valor
            );

            if (mov.TipoDerivado == "PARCELA")
            {
                novaMovimentacao.ParcelaId = mov.Id;
                await this._movimentacoes.CriarMovimentacao(novaMovimentacao);
                await this._movimentacoes.MarcarParcelaComoPaga(mov.Id);
            }
            else if (mov.TipoDerivado == "PERSISTENTE")
            {
                novaMovimentacao.PersistenteId = mov.Id;
                await this._movimentacoes.CriarMovimentacao(novaMovimentacao);

            }
            await atualizarSaldo;
        }

    }

}