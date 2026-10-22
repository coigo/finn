# Rastreabilidade C# → Go

Mapa endpoint → use case C# → implementação Go → testes. Referência de regras: `docs/analysis.md`.

## Platform

| Peça | C# | Go | Testes |
|---|---|---|---|
| Dinheiro | `decimal` | `money.Money` (centavos int64; JSON decimal) | `money_test.go`, `arith_test.go` |
| Erro HTTP | `ErrorHandler` (BusinessError→4xx, resto→500) | `httpx.Error` + `StatusMapper` por domínio | por domínio |
| Bind JSON | model binding | `httpx.DecodeJSON` (400 "corpo inválido") | por domínio |
| Datas no bind | `DateTime` (aceita "2025-11-20") | `httpx.FlexDate` (data simples ou RFC3339) | handler tests |
| Banco teste | — | `testdb.New/Migrate` (skip até Fase 4) | store tests |

## saldos — base `/api/saldos`

| Endpoint | Use case C# | Go | Testes |
|---|---|---|---|
| `GET /?saldoNome=` | BuscarSaldoAtual | `Service.Get` / `Handler.get` | `TestHandlerGetSaldo*`, `TestGetSaldo*` |
| `POST /` {nome,valor} | AdicionarSaldo | `Service.Set` / `Handler.set` | `TestHandlerPostSaldo*`, `TestSetSaldo*` |

Erros: "Saldo não encontrado!" (400). Store grava em `saldos`.

## salarios — base `/api/salario`

| Endpoint | Use case C# | Go | Testes |
|---|---|---|---|
| `GET /` | BuscarSalarioAtual | `Service.GetCurrent` | `TestGetCurrent*` |
| `POST /` {valor} | AdicionarSalario | `Service.Create` | `TestCreate*` (positivo/negativo) |
| `POST /adicionar` | AdicionarSalarioAtual | `Service.PayCurrent` | `TestPayCurrent*` (ordem saldo→movimentação; sem salário→500) |

Erros: "Salário não cadastrado" (400), "Use um valor positivo" (400). `PayCurrent` usa `SaldosUpdater.AddDelta` + `MovimentacoesCreator.CreateEntradaSalario` (categoria 9, ENTRADA, "Salário", sem tocar saldo dentro do use case de movimentações).

## movimentacoes — base `/api/movimentacoes`

| Endpoint | Use case C# | Go | Testes |
|---|---|---|---|
| `POST /` | CriarMovimentacao (+parcelas/persistente) | `Service.Create` | `TestCreate*` (delta por tipo/categoria 10/11; parcelas valor/n, resto perdido, venc +i meses, tipo ordinal; persistente) |
| `POST /parcelas` {ids} | SubtrairParcelas | `Service.SubtractParcelas` | `TestSubtrairParcelas` (débito −soma em Corrente, sem movimentação, echo `{"parcelas":[...]}`) |
| `GET /?inicio&fim` | BuscarMovimentacoes | `Service.ListByPeriodo` | `TestListByPeriodo*` (fim = +1d−1µs; JOIN categoria; ORDER BY data DESC) |
| `GET /pendentes` | BuscarMovimentacoesPendentes | `Service.ListPendentes` | `TestListPendentes*` (UNION; mês do vencimento; vazio→mês seguinte→PROXIMO_MES) |
| `POST /pendentes` | ProcessarMovimentacoesPendentes | `Service.ProcessPendentes` | `TestProcesandoPendentes` (parcela→ParcelaID+MarkPaga; persistente→PersistenteID; ±delta por tipo) |
| `DELETE /{id}` | DesfazerMovimentacao | `Service.Undo` | `TestUndo*` (Δ invertido, saldo antes do delete; "Movimentação não encontrada") |
| `GET /categorias` | BuscarMovimentacoesCategorias | `Service.ListCategorias` | `TestListCategoriasPassthrough`, `TestStoreCategoriasSeeds` (13 seeds) |
| `GET /persistentes/{id}` | BuscarMovimentacaoPersistentePorId | `Service.GetPersistente` | `TestGetPersistente*` ("...encontrada!" com !) |
| `PUT /persistentes/` | AtualizarMovimentacaoPersistente | `Service.EditPersistente` | `TestEditPersistenteRepasse` ("Movimentacao não encontrada" sem acento) |
| `DELETE /persistentes/{id}` | DeletarMovimentacaoPersistente | `Service.SoftDeletePersistente` | `TestSoftDeletePersistente` (soft delete; B6: GET continua retornando) |

## aportes — base `/api/aportes`

| Endpoint | Use case C# | Go | Testes |
|---|---|---|---|
| `POST /` | MovimentarAportes | `Service.Movimentar` | `TestMovimentar*` (histórico ANTES; tipo=qtd<0?VENDA:COMPRA; cria valida ticker → "O Ticker X não existe! " c/ espaço; média ponderada quebrada replicada; venda total→pânico→500; retorno stale) |
| `POST /{identificador}` | MovimentarAportesPorIdentificador | `Service.MovimentarPorIdentificador` | `TestMovPorIdent*` (existir senão "Aporte não encontrado"; qtd<=0→"Quantidade inválida"; COMPRA/VENDA(PM=0 se zera)/DESDOBRAMENTO; negativo→"Preço médio inválido"; preco null+COMPRA→idem; tipoStr inválido→500 DEPOIS de buscar aporte; histórico DEPOIS com categoria do aporte) |
| `GET /` | BuscarAportes | `Service.List` | `TestList*` (REND_FIXA usa PM; ACAO/FII/ETF→ticker; CRIPTO→crypto; falha→"Ativo não encontrado!"; **B4 paralelo** — `TestListParalelo`) |
| `GET /categorias` | BuscarCategorias | `Service.Categorias` | `TestCategorias`, `TestHandlerCategorias` |
| `GET /{identificador}/historico` | BuscarAportesHistorico | `Service.ListHistorico` | `TestListHistoricoPassthrough` (identificador CRU; tipo STRING; preco nulo) |

Identificador normalizado (trim+upper) no bind dos dois POSTs (feito no handler, como o DTO C#); `GET /historico` não normaliza.

## Status de verificação (Fase 3)

- `gofmt`/`go vet ./...`: limpos.
- **`go test ./...`: TODOS os 5 pacotes verde** (saldos, salarios, movimentacoes, aportes, money).
- Store tests continuam **skip** até as migrations existirem (Fase 4) — esperado.
- Detalhe descoberto na implementação: o C# devolve no POST de aportes a **entidade EF tracked atualizada** (PM/quantidade novos na resposta) — replicado e fixado em `TestMovimentarAporteExistente`.

## Status de verificação (Fase 4)

- `db/migrations/0001_init.sql` (goose, STRICT, seeds de 13 categorias + 2 saldos) aplicada via `goose.Up` no main e nos store tests (`testdb.Migrate`); `--migrate` aplica e sai (paridade do init container).
- `sqlc generate`/`vet` OK (v1.31.1) — código em `internal/<dominio>/internal/sqlcgen`. **Armadilha do parser sqlc-sqlite: comentários com acentos UTF-8 quebram o generate** (comentários dos queries.sql em ASCII; strings com acento passam).
- Stores implementados sobre o sqlc: conversão cents↔Money, timestamps RFC3339↔time.Time (parser tolera o formato antigo `yyyy-MM-dd HH:mm:ss.fff` do C#), `sql.ErrNoRows`→sentinels, categoria TEXT↔enum (aportes).
- Divergências conscientes documentadas: (a) ramo PERSISTENTE de pendentes devolve `''` no SQL (scan) e vira `null` no JSON; (b) `AtualizarAporte` usa `:execrows` para detectar id inexistente (KeyNotFoundException→500); (c) `CreateParcelas` abre transação explícita (AddRange atômico do C#); (d) aritmética de moeda trunca em centavos (B5) — o C# decimal podia carregar sub-centavos em preço médio.
- `cmd/api/main.go`: config de env (PORT/WEB_URL/B3/CRYPTO obrigatórios, falha no startup como o C#), `sqlitex.Open` (WAL, busy_timeout, foreign_keys, MaxOpenConns(1)), DI stores→services→handlers→`Mount` no chi, shutdown gracioso.
- Adapters de preço (`aportes/prices.go`): brapi/coingecko; não-2xx→(nil,nil)→"Ativo não encontrado!" 400; erro de rede/JSON→500.
- Smoke test real (binário em :18099 + SQLite em disco): migrations OK; `GET /saldos?saldoNome=Corrente`→200 seed; `GET /salario`→400 `{"status":400,"message":"Salário não cadastrado"}`; saldos inexistente→400 `{"status":400,"message":"Saldo não encontrado!"}`; categorias/aportes/movimentacoes/pendentes→200.
- `gofmt`/`go vet` limpos; **`go test ./...` verde (9 pacotes)**. golangci-lint não instalado na máquina — lint efetivo = gofmt+vet.

## Status de verificação (Fase 5)

- Ferramentas de paridade: `docs/parity-requests.md` (coleção de 33 requests) + `scripts/parity.ps1` (sobe C# e Go em portas separadas com DBs SQLite frescas em cópia temporária — **nunca** toca `api/data/finndb.sqlite`; corpos trafegam por arquivo por causa das mutilações do PS 5.1).
- **Resultado: PARIDADE TOTAL — 33/33 IGUAL** (ver `docs/parity.md` para método e divergências aceitas).
- Correções de paridade aplicadas nesta fase (todas com evidência no C#):
  - `periodo` de pendentes serializa como NÚMERO (0/1 — enum `Periodo` do C#), não string.
  - `saldoHistorico: []` no JSON de saldos (nav collection do EF).
  - Not-found → **500 com mensagem vazada** onde o C# usa `BusinessError` (undo, GET persistentes/{id}, aportes: `Aporte não encontrado`, `Ativo não encontrado!`, `Tipo inválido` e validações) — `KeyNotFoundException` continua 400 (saldos/salário).
  - `PUT/DELETE persistentes`: resposta é sempre 200 (echo / id pedido) porque o repo C# é async void sem await; erro mata o processo C# — Go registra em log e NÃO replica a queda do processo.
  - "Use um valor positivo" (salário) → 500 (BusinessError), não 400.
- Ambiente: `go test ./...` verde (9 pacotes), `gofmt`/`go vet` limpos.
