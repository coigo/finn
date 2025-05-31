using Movimentacoes.DTOS;

namespace Movimentacoes.Factories;

public interface IMovimentacaoFactory
{
    public MovimentacaoResultado Execute(CriarMovimentacao data);
}