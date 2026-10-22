# Paridade C# → Go — coleção de requests

Coleção executada por `scripts/parity.ps1` contra a API C# (`dotnet run`, cwd
temporário → SQLite fresco) e a API Go (binário, SQLite fresco), na MESMA
ordem, comparando status + corpo. Datas ISO com timestamp (data/hora com
subsegundos) são normalizadas antes do diff (o `criadoEm` do C# tem
precisão ~ms/7 dígitos e offset local; Go nanos — ruído de relógio, não de
regra de negócio). Chaves de cotação (B3/CRYPTO) deliberadamente inválidas:
os caminhos de aportes exercem o fluxo de erro "Ativo não encontrado"/ticker.

| #  | Método/rota | Corpo | O que verifica |
|----|-------------|-------|----------------|
| 01 | GET /api/saldos?saldoNome=Corrente | – | seeds, shape {id,valor,nome} |
| 02 | GET /api/saldos?saldoNome=Investimentos | – | segunda seed |
| 03 | GET /api/saldos?saldoNome=Zzz | – | 400 "Saldo não encontrado!" |
| 04 | POST /api/saldos | {"nome":"Corrente","valor":"12.34"} | set de saldo, string→decimal |
| 05 | GET /api/saldos?saldoNome=Corrente | – | persistência do set |
| 06 | GET /api/salario | – | 400 "Salário não cadastrado" |
| 07 | POST /api/salario | {"valor":"100000.00"} | cria salário |
| 08 | GET /api/salario | – | último salário |
| 09 | POST /api/salario {"valor":"-1} | {"valor":"-1"} | "Use um valor positivo" (status real do B1) |
| 10 | POST /api/salario/adicionar | – | pagamento: movimentação ENTRADA + saldo Corrente += salário |
| 11 | GET /api/movimentacoes/categorias | – | 13 seeds, ordem, shape id/name/tipo |
| 12 | POST /api/movimentacoes | simples 25.50 SAIDA cat 1 data 2026-03-15 | criação + saldo Corrente -=25.50 |
| 13 | GET /api/movimentacoes?inicio=2026-03-01&fim=2026-03-31 | – | lista: JOIN categoria, ORDER data DESC, formato de data |
| 14 | POST /api/movimentacoes | parcelado 300.00/3 cat 6 primeiroVenc 2026-05-01 | AddRange de parcelas + vencimentos +n meses |
| 15 | GET /api/movimentacoes/pendentes | – | UNION parcela/persistente, período MES_ATUAL |
| 16 | POST /api/movimentacoes/pendentes | {} | processar pendentes: cria movimentaçãos + marca parcelas pagas + delta saldo |
| 17 | GET /api/movimentacoes/pendentes | – | vazia após processar |
| 18 | POST /api/movimentacoes | persistente 19.90 cat 5 "streaming" persistente=true | fábrica cria persistente + movimentação |
| 19 | POST /api/movimentacoes/persistentes | {"valor":"21.90","descricao":"net+","tipo":0,"categoriaId":5} | cria persistente pura |
| 20 | GET /api/movimentacoes/persistentes/2 | – | entidade crua (deletadoEm null); shape/JSON |
| 21 | PUT /api/movimentacoes/persistentes | {"id":2,"valor":"29.90","descricao":"net premium","tipo":0,"categoriaId":4} | edição |
| 22 | GET /api/movimentacoes/persistentes/2 | – | confere edição |
| 23 | DELETE /api/movimentacoes/persistentes/2 | – | soft delete |
| 24 | GET /api/movimentacoes/persistentes/2 | – | B6: GET retorna deletada (deletadoEm setado) |
| 25 | GET /api/movimentacoes/persistentes/999 | – | mensagem de não encontrado (variante) |
| 26 | DELETE /api/movimentacoes/999 | – | desenfaz inexistente |
| 27 | DELETE /api/movimentacoes/2 | – | undo: despesa volta pro saldo |
| 28 | POST /api/aportes | PETR4 100@10.5 cat 1 dataCompra | keys mortas → caminho de erro de ticker |
| 29 | POST /api/aportes/PETR4 | {"quantidade":50,"tipoStr":"FOO"} | tipo inválido → 500 (mensagem da exceção) |
| 30 | GET /api/aportes | – | lista vazia, shape ListDTO |
| 31 | GET /api/aportes/categorias | – | 5 SelectValues |
| 32 | GET /api/inexistente | – | fallback de rotas (sem wwwroot: 404 vazio) |
| 33 | GET / | – | DefaultFiles sem wwwroot → 404 vazio |

## O que não dá para comparar automaticamente

- `%TEMP%` timestamps reais (criadoEm) — normalizados no diff (documentado).
- Concorrência/timing (request-id, logs do servidor).
- Aportes com preço REAL (precisaria moq de brapi/coingecko nas duas pontas —
  fora do escopo do script; testes unitários cobrem o mapeamento).
