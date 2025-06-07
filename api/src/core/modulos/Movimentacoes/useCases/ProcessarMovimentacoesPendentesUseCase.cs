using Infra.Repositories;
using Movimentacoes.Models;

namespace Movimentacoes.UseCases;

public class ProcessarMovimentacoesPendentesUseCase {

    private readonly IMovimentacaoRepository _movimentacoes;
    private readonly IResumoRepository _resumo;
    public ProcessarMovimentacoesPendentesUseCase  ( IMovimentacaoRepository movimentacoes, IResumoRepository resumo ) {
        _movimentacoes = movimentacoes;
        _resumo = resumo;
    }

    public async Task Execute()
    {
        var pendentes = await this._movimentacoes.BuscarPendentesDoMes(DateTime.Now);

        foreach (var mov in pendentes)
        {
            Movimentacao novaMovimentacao = new(
                mov.Valor,
                mov.Tipo,
                mov.CategoriaId,
                mov.Descricao,
                DateTime.Now
            );

            var atualizarSaldo = this._resumo.AtualizarSaldo("Corrente",

            mov.Tipo == MovimentacaoTipo.ENTRADA
                ? novaMovimentacao.Valor 
                : - novaMovimentacao.Valor
            );
            
            if (mov.TipoDerivado == "PARCELA")
            {
                novaMovimentacao.ParcelaId = mov.Id;
                await this._movimentacoes.CriarMovimentacao(novaMovimentacao);
                await this._movimentacoes.MarcarParcelaComoPaga(mov.Id);
            }
            else if (mov.TipoDerivado == "PERSISTENTE") {
                novaMovimentacao.PersistenteId = mov.Id;
                await this._movimentacoes.CriarMovimentacao(novaMovimentacao);

            }
            await atualizarSaldo;
        }

    }

}