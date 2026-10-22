# Análise da API C# ("finn") — Fase 1

Fonte: `api/` (.NET 8, EF Core + SQLite). Objetivo: paridade de comportamento externo (rotas, contratos JSON, status codes, regras) na porta para Go em `/new-api`.

**Decisões já acordadas nesta fase:** domínios conforme módulos existentes · router **chi** · migrations **goose** · **copiar dados** do SQLite atual · **corrigir bugs óbvios** da porta (sem silenciar divergências).

---

## 1. Mapa de domínios

A divisão do C# já é por módulo vertical (`api/src/core/modules/*`); ela é adotada quase tal qual em Go:

| Domínio Go | Módulo C# | Tabelas (dono) | Service (métodos ← usecases C#) |
|---|---|---|---|
| `movimentacoes` | `Movimentacoes` | `movimentacoes`, `movimentacoes_categorias`, `movimentacoes_parcelas`, `movimentacoes_persistentes` | `Create` (←CriarMovimentacao+Factory), `CreateParcelas` (←CriarParcela), `CreatePersistente` (←CriarPersistente), `ListByPeriodo` (←BuscarMovimentacoes), `ListPendentes` (←BuscarPendentesDoMes), `ProcessPendentes`, `Undo` (←Desfazer), `SubtrairParcelas`, `ListCategorias`, `GetPersistente`, `EditPersistente`, `SoftDeletePersistente` |
| `aportes` | `Aportes` | `aportes`, `aportes_historico` | `Movimentar` (←MovimentarAportes), `MovimentarPorIdentificador` (←...PorIdentificador), `List` (←BuscarAportes), `ListHistoricoPorIdentificador`, `Categorias` |
| `saldos` | `Saldos` | `saldos`, `saldos_historico¹` | `Get` (←BuscarSaldo), `Set` (←EditarSaldo), `AddDelta` (sub-domínio usado por outros) |
| `salarios` | `Salarios` | `salarios` | `Create` (←CriarSalario), `GetCurrent` (←BuscarSalarioAtual), `PayCurrent` (←AdicionarSalarioAtual) |

¹ `saldos_historico` existe no schema e no EF (FK cascade), mas **nenhum use case grava/lê** — manter tabela no schema por paridade de dados, sem handler/query.

**Tabela `rotina`** (`core/entities/Rotina`): existe no schema, mas **nenhum use case/controller a usa**. **B2 — RESOLVIDO: descartada** — não vai para o schema novo nem para a migração de dados.

Arquivos mortos no C# a ignorar: `useCases/BuscarAporteHistorico.cs` e `BuscarAportePorIdentificador.cs` (vazios), `AdicionarSalarioAtualUseCase copy.cs` (não registrado), `IAporteHistoricoRepository.Buscar(HistoricoFiltrosDTO)` sem consumidor, `HistoricoFiltrosDTO`, `ResultadoMovimentacaoFactory`, `IUseCase<>`/`Unity` (sumiu em Go), middleware `BasePathEnforcer` (não registrado), Swagger pacote registrado sem `UseSwagger` (desabilitado), `context.Rotinas`.

### Grafo de dependências e interfaces

```
salarios ──(soma saldo; cria movimentação)──▶ saldos, movimentacoes
movimentacoes ──(incrementa/decrementa saldo)──▶ saldos
aportes ──(cotação)──▶ API externa (brapi.dev / coingecko) — adapter no próprio domínio
saldos ──▶ (nenhum)
```

Sem joins SQL entre tabelas de domínios distintos (o JOIN que existe em `movimentacoes` é intra-domínio). Interfaces definidas **no consumidor**, injetadas em `main.go`:

- em `movimentacoes`: `type SaldosUpdater interface { AddDelta(ctx, nome string, delta decimal) error }` — implementado por `saldos.Service`. (C# usa `ISaldoRepository.AtualizarSaldo(nome, valor)` — **semântica de incremento** `saldo.Valor += valor`.)
- em `salarios`: mesma interface acima + `MovimentacoesCreator interface { CreateEntradaSalario(ctx, valor) error }` — implementado por `movimentacoes.Service` (C#: `ISaldoRepository.AtualizarSaldo` + `IMovimentacaoRepository.CriarMovimentacao`).

Sem ciclos. Sem transação multi-domínio hoje: todas as sequências saldo+movimentação são duas transações separadas no C# (ver §7/bugs) — a porta **corrigirá** agrupando em uma `WithTx` por operação (.persistence-unit única, `WithTx` já previsto no design do feat.md).

---

## 2. Endpoints por domínio

Autenticação: **nenhuma** (app single-user, sem middleware de auth). CORS: `app.UseCors("allowLocal")` — origin única de `WEB_URL`, `AllowCredentials`. Prefixo HTTP: nenhuma string de status custom; todas as respostas 200 no caminho feliz com corpo do tipo do use case.

### 2.1 `movimentacoes` — base `/api/movimentacoes`

| Método/Rota | Request | Response (JSON, camelCase) |
|---|---|---|
| `POST /` | `CriarMovimentacao`: `{valor: number, tipo: 0\|1\|2, categoriaId: number, descricao: string, data: "date", quantidadeParcelas?: number, primeiroVencimento?: "date", persistente?: bool}`¹ | Echo do request com valores parseados (`data` em ISO do C#, `tipo` numérico) |
| `GET /?inicio&fim` | duas strings date (parseadas com `DateTime.Parse` invl.) | `[ {id, valor, tipo, descricao, categoria, data} ]` novo→antigo por `data DESC`² |
| `POST /parcelas` | `{parcelas: number[]}` (ids) | echo do request |
| `GET /pendentes` | — | `{pendentes: [{id, valor, descricao, categoria, categoriaId, vencimento?, tipo: 0\|1\|2, tipoDerivado: "PARCELA"\|"PERSISTENTE"}], periodo: "MES_ATUAL"\|"PROXIMO_MES"}` |
| `POST /pendentes` | — | `true` |
| `DELETE /{movimentacaoId}` | — | número do id |
| `GET /categorias` | — | `[{id, name, tipo: "ENTRADA"\|"SAIDA"\|"INVESTIMENTOS"}]` (sem filtro deletadoEm³) |
| `GET /persistentes/{id}` | — | entidade brute: `{id, valor, descricao, tipo: 0\|1\|2, categoriaId, criadoEm, deletadoEm?}` (retorna também deletado⁴) |
| `PUT /persistentes` | `{persistenteId, valor, categoriaId, descricao}` | echo do request |
| `DELETE /persistentes/{id}` | — | número do id |

¹ Caso derivado: `quantidadeParcelas != null` → PARCELAS; senão `persistente === true` → PERSISTENTE; senão MOVIMENTACAO simples. Navariante PARCELAS cria N parcelas `valor/quantidade` (resto decimal perdido — replica), vencimentos `primeiroVencimento + i meses`. PERSISTENTE exige `persistente===true` senão `Exception` genérica → 500.
² `fim` = parse(fim)+1dia−1µs (inclusivo). SQL cru com `GROUP BY m.id`.
³ `BuscarCategorias()` não filtra `deletadoEm` (não há delete de categoria exposto).
⁴ `GetPersistente` **não** filtra `deletadoEm` (achado, não filtrado) — paridade mantida.

### 2.2 `aportes` — base `/api/aportes`

| Método/Rota | Request | Response |
|---|---|---|
| `POST /` | `{identificador, quantidade, preco, categoria: 1..5, dataCompra}` (identificador `Trim().ToUpper()` no bind) | `Aporte` entidade (`{id, identificador, precoMedio, quantidade, categoria: 1..5, criadoEm}`) |
| `POST /{identificador}` | `{identificador?(ignora, usa da rota), quantidade, preco?, tipoStr: "COMPRA"\|"VENDA"\|"DESDOBRAMENTO", data}` | `Aporte` entidade |
| `GET /` | — | `[{identificador, precoMedio, precoAtual, quantidade, categoria: "ACAO"…(string)}]` |
| `GET /{identificador}/historico` | — | `[{identificador, quantidade, preco?, data, tipo: "COMPRA"…}]` (sem ORDER BY explícito²) |
| `GET /categorias` | — | `[{id: 1..5, name: "ACAO"…}]` (enum AporteCategoria) |

Regras de `POST /` (identificador trim/upper no DTO): registra **sempre** um histórico com `tipo = quantidade < 0 ? VENDA : COMPRA`; se aporte não existe: valida ticker na API externa (`BusinessError` "O Ticker X não existe!" → 4xx¹⁵), cria; se existe: `novaQtd = antiga + entrada`, `precoMedio = (antigoPM*antigaQtd + preco*entrada)/novaQtd` — **fórmula quebrada para VENDA com quantidade negativa** (o termo `preco*entrada` entra NEGATIVO no numerador, só "funciona" por sinal; e `preco` para venda ≠ 0 estraga a média)⁶. `quantidade` final pode ficar ≤ 0 — sem proteção.

Regras de `POST /{identificador}`: aporte deve existir (`BusinessError` "Aporte não encontrado" → 4xx¹⁵). `quantidade <= 0` → `BusinessError` "Quantidade inválida". COMPRA: soma e média ponderada; VENDA: subtrai, PM inalterado (0 → `precoMedio = 0`) e `preco` pode ser null; DESDOBRAMENTO: `qtd *= fator` e PM diluído `PM * (antiga/nova)`; resultado `qtd < 0` → "Preço médio inválido". Histórico salvo com o `tipo` informado e `categoria` do aporte existente.

Regras de `GET /`: só `quantidade > 0`; `precoAtual` via API externa para ACAO/ETF/FII/CRIPTO (brapi `regularMarketPrice` para AÇÕES/FII/ETF, coingecko brl para cripto), `RENDA_FIXA` usa `precoMedio` como atual. Falha/ativo inexistente → `BusinessError` "Ativo não encontrado!" → 4xx¹⁵ — **um ativo ruim quebra a listagem inteira**.

¹⁵ No C# `BusinessError` → 500; na porta → **4xx** (decisão B1).
⁶ Bugs possíveis/óbvios — ver §7.

### 2.3 `saldos` — base `/api/saldos`

| Método/Rota | Request | Response |
|---|---|---|
| `GET /?saldoNome=` | nome | `Saldo` `{id, valor, nome}` (não achado → `KeyNotFoundException` → **400** com `{status:400, message:"Saldo não encontrado!"}`) |
| `POST /` | `{nome, valor}` | `Saldo` atualizado — **sobe o valor absoluto** (`saldoExiste.Valor = data.valor`), queima histórico? não, só update |

Mensagem de erro com bug de interpolação: `"Não há saldo com esse nome {data.nome}"` → literal. Corrigir na porta para interpolar.

### 2.4 `salarios` — base `/api/salario`

| Método/Rota | Request | Response |
|---|---|---|
| `GET /` | — | último por `id DESC`; sem nenhum → `KeyNotFoundException` "Salário não cadastrado" → 400 |
| `POST /` | `{valor: number}` | `Salario` `{id, valor, criadoEm}`; `valor < 0` → `BusinessError` "Use um valor positivo" → **4xx** (era 500 no C#¹⁵) |
| `POST /adicionar` | — | pega último salário, **soma no saldo "Corrente"**, cria `Movimentacao` ENTRADA categoriaId **9** ("Salário") com `DateTime.Now`, retorna o salário. Sem salário cadastrado → NRE(`salario.Valor` de null) → 500 |

---

## 3. Regras de negócio/validações (implícitas incluídas)

- **Aportes.** Identificador sempre normalizado (trim + upper). Cotações externas imperativas na listagem — **B4 RESOLVIDO: paralelizar** as N chamadas HTTP (uma por ativo) em Go; erro único de ativo continua abortando a listagem (paridade). Histórico gravado antes da validação do ticker (registra histórico mesmo quando cria/atualiza falha — na variante `POST /`). Ponto crítico: dois endpoints de movimento com **lógicas diferentes** (payload livre vs rota+tipos explícitos; VENDA no payload é `quantidade negativa`, na rota é `tipoStr=VENDA` com `quantidade positiva`).
- **Movimentações.** Valor de saldo sempre em "Corrente" (hardcoded). `tipo=INVESTIMENTOS` também debita Corrente (não movimenta saldo Investimentos, apesar de existir). Categorias DIVIDENDO(10)/VENDA(11) ignoradas no incremento de ENTRADA (dividendos/vendas só registram, não somam ao saldo). Pendentes: parcelas não pagas com vencimento no mês-hoje; se vazio, tenta próximo mês e muda `periodo`. Processar: recreia movimentação com `DateTime.Now`, marca parcela paga; persistentes são filteradas por "não tem movimentação neste mês". `Desfazer` reverte ENTRADA subtraindo, outros adicionando; **não** reverte dado persistente/parcela.
- **Saldos/Salários.** 2 saldos seedados (Corrente=1, Investimentos=2) e 13 categorias seedadas com ids fixos.

## 4. Modelo de dados (atual do EF/SQLite → proposta Go/sqlc)

Convenção de tipos (única no projeto):
- **decimal/money** → **INTEGER (centavos)** — **B5 — RESOLVIDO: mudado para INTEGER**. No Go, `type Money int64` (centavos) em `platform/money`, com `MarshalJSON`/`UnmarshalJSON` convertendo para/de número decimal JSON (ex.: `1250` ↔ `12.50`), para manter o contrato de número decimal que o C# emitia. Aritmética 100% int64 (cuidado com overflow de `quantidade*preco` — validar faixa; `shopspring/decimal` não é mais necessário). Migração de dados: parse do TEXT invariante do banco atual e multiplicação por 100 arredondamento bancário/exato definido (comparar com representação original — documentar possíveis diferenças de último dígito).
- **timestamps** → `TEXT` **RFC3339** (padrão do Go: `time.Now()` no fuso do runtime — container em UTC). **B3 — RESOLVIDO**: a migração de dados **copia as datas do banco antigo como estão** (formato `yyyy-MM-dd HH:mm:ss.fff` sem TZ) — o usuário irá normalizá-las depois. Escrito novo usa RFC3339. Nota: as funções de data do SQLite (`strftime('%m'/**%Y')`) aceitam os dois separadores (`'T'` e espaço), então filtros de pendentes funcionam nos dois formatos; comparações lexicográficas de intervalo só ficam estáveis depois da normalização. `data`/`vencimento` serão gravados como `date` (`YYYY-MM-DD`) em escritas novas para os campos de semântica data.
- **bool** → INTEGER 0/1. **enums** → mesmo storage do C# (TEXT para `movimentacoes.tipo`, `aportes.tipo/categoria`... **mas INTEGER** para `movimentacoes_parcelas.tipo` e `movimentacoes_persistentes.tipo`) — assimetria mantida por paridade de dados/contrato; no Go, um `type` por domínio com métodos `Scan/Marshal`.
- **id** → INTEGER AUTOINCREMENT (SQLite `INTEGER PRIMARY KEY`).
- `FOREIGN KEY = ON`, `journal_mode = WAL`, `busy_timeout` no DSN; `SetMaxOpenConns(1)` (modelo single-user; decisão documentada).

Tabelas (hoje → proposta equivaleente, `STRICT` quando possível; `valor*` agora em INTEGER centavos):

- `movimentacoes(id INTEGER PK, valor INTEGER(cents), tipo TEXT, categoriaId INTEGER FK→movimentacoes_categorias(id) ON DELETE CASCADE, descricao TEXT NOT NULL, data, parcelaId INTEGER? FK mov, persistenteId INTEGER? FK mov, criadoEm)` + idx categoriaId, parcelaId, persistenteId.
- `movimentacoes_categorias(id PK, nome TEXT NOT NULL, tipo TEXT, criadoEm, deletadoEm TEXT?)` seed ids 1–13 (nomes pt, tipo ENT/SAI por prop; ver §5).
- `movimentacoes_parcelas(id PK, valor INTEGER(cents), numero INTEGER, tipo INTEGER!(ordinal), categoriaId INTEGER FK, descricao, vencimento, criadoEm, paga INTEGER 0/1)` — sem FK no C# para categoria? (definido mas sem onDelete — manter FK Restrict).
- `movimentacoes_persistentes(id PK, valor INTEGER(cents), descricao, tipo INTEGER!(ordinal), categoriaId FK, criadoEm, deletadoEm TEXT?)`.
- `saldos(id PK, valor INTEGER(cents), nome TEXT)`; seeds Corrente(id=1), Investimentos(id=2).
- `saldos_historico(id PK, valor INTEGER(cents), criadoEm, resumoId INTEGER FK→saldos(id) ON DELETE CASCADE)` — coluna chama-se `resumoId` (mantida).
- `aportes(id PK, identificador TEXT UNIQUE, precoMedio INTEGER(cents), quantidade INTEGER, categoria TEXT, criadoEm)`.
- `aportes_historico(id PK, preco INTEGER(cents)?, identificador TEXT, quantidade INTEGER, categoria TEXT, tipo TEXT, data, criadoEm)` + idx identificador.
- `salarios(id PK, valor INTEGER(cents), criadoEm)`.
- ~~`rotina`~~ — **B2: descartada**, não vai para o schema novo.

FKs que cruzam domínio: **nenhuma** — todos os FKs internos por domínio. (O único acoplamento é de service, §1.)

## 5. Mapa de queries por domínio (proposta para sqlc)

- `movimentacoes/queries.sql`: `InsertMovimentacao :one`, `FindMovimentacaoByID :one`, `DeleteMovimentacao :exec`, `InsertParcelas :exec`, `ListMovimentacoesPorPeriodo :many` (JOIN categorias, `ORDER BY data DESC` — ordem só pode ser data desc fixa: igual ao C#), `MarkParcelaPaga :exec`, `ListParcelasPorIds :many` (`sqlc.slice`), `ListPendentesDoMes :many` (UNION PARCELA/PERSISTENTE com `strftime` `?month` `?year` — replica dedup do UNION: mesmo shape), `InsertPersistente :one`, `FindPersistenteByID :one`, `UpdatePersistente :exec`, `SoftDeletePersistente :exec`, `ListCategorias :many`.
- `aportes/queries.sql`: `ListAportesComQuantidade :many` (`quantidade > 0`), `FindAporteByIdentificador :one`, `ListAportesHistoricoById :many` (sem `ORDER BY` no C# — ordem por `rowid` ≈ inserção; replicar sem ORDER BY e comentar), `InsertAporte :one`, `UpdateAporte :exec`.
- `saldos/queries.sql`: `FindSaldoByNome :one`, `InsertSaldo :one`, `SetSaldoValor :one`, `AddSaldoDelta :one` (UPDATE … `SET valor = valor + ?1` e devolver linha).
- `salarios/queries.sql`: `InsertSalario :one`, `FindSalarioMaisRecente :one` (`ORDER BY id DESC LIMIT 1`).

sqlc não resolve nada de forma dinâmica aqui (filtros fixos). Paginação: **não existe**. Ordenação variável: **não existe**.

Sem mocks de rede na listagem aportes: `service_test` usa fake de `PriceFinder` (interface nova `aportes`) e `handler_test` também; cotações externas ficam atrás de interface (injetada por `main.go`).

## 6. Dependências externas

- **brapi.dev** (`B3_API_URL/{ticker}?token=…`) → `regularMarketPrice` de `results[0]`. Falha HTTP → retorno nil e o service vira `BusinessError`.
- **coingecko demo** (`CRYPTO_API_URL`, headers query `x_cg_demo_api_key`, `vs_currencies=brl`, `precision=2`, `symbols=crypto`).
- **SQLite** local (`data/finndb.sqlite`) — único banco. Sem filas/cache.

## 7. Erros → HTTP (paridade)

Middleware errorHandler (`ErrorHandler.cs`): `KeyNotFoundException` → **400** com `{status: 400, message}`; **todo o resto** → **500** com `ex.Message` no corpo.

Atualizado (decisão do usuário): no Go, erros de domínio são **sentinels de pacote** (`var ErrNotFound = errors.New(...)`, `var ErrBusiness = errors.New(...)`, `var ErrValidation = errors.New(...)`) verificáveis com `errors.Is` — **sem tipos/wrappers customizados**. Mapeamento em `platform/httpx`: `errors.Is(err, ErrNotFound)` e `errors.Is(err, ErrBusiness)`/`ErrValidation` → **4xx** (400, corpo `{status, message}`, mensagem do erro via contexto `fmt.Errorf("%w: ...")`... nota: sentinels puros não carregam mensagem dinâmica por erro; para mensagens específicas usar `fmt.Errorf("...: %w", ErrBusiness)` e o handler extrai só a parte `errors.Unwrap`/mensagem completa), erro inesperado → 500 com mensagem genérica. Body de request inválido (JSON malformado, campo decimal com tipo errado) retorna **400 automático do `[ApiController]`** com `ValidationProblemDetails` — replicar o shape desse corpo nas validações de bind do chi ou aceitar divergência documentada. Ver §11-B1.

**B1 — RESOLVIDO:** `BusinessError` → **4xx** (não mais 500 como no C#). Divergência de status code consciente; documentar na Fase 5.

## 8. Pontos sem equivalente direto (pacto)

| C# | Ir |
|---|---|
| `async void` / dictionary de Fn no Factory | goroutine implícita sem paralelismo; Go: método síncrono + serial, **corrigido** (`Create` aguarda de fato) |
| `decimal` / `CultureInfo.InvariantCulture` | `Money` int64 (centavos) com parse estrito no bind; sem lib externa |
| LINQ/EF change-tracking | queries sqlc explícitas |
| `DateTime.Now` (local, BRT) | `time.Now().Local()` (container roda em UTC — **B3 decide o comportamento observável**) |
|Inserção em bulk `AddRangeAsync` | `exec` de um stmt por linha dentro de `WithTx` |
| DI auto-wiring | wiring manual em `main.go` |

## 9. Migração de dados (decidido: **copiar**)

1. Script `cmd/import-old/main.go`: lê `data/finndb.sqlite` antigo, lê novo (via goose), copia tabela a tabela **convertendo**: datetimes **copiados como estão** (formato antigo mantido; default time do Go/RFC3339 a partir de escritas novas) — usuário normalizará depois (B3); decimal TEXT → INTEGER centavos; enums mantidos (TEXT/INTEGER iguais).
2. Seeds (categorias/saldos) já existentes no banco não devem ser duplicados — `INSERT OR IGNORE`/skip por id.
3. Validação: contagem de linhas por tabela + checksum sobre somas (`SUM(valor)`, agora em centavos) antes/depois. Tabela `rotina` **não** é migrada (B2).

## 10. Bugs/tristezas a corrigir na porta (e documentar na Fase 5)

1. `async void` (fire-and-forget) em CriarMovimentacao/Factory/Delete/Desfazer → **Go síncrono**; respostas echo continuam iguais.
2. `DesfazerMovimentacao` pode precedentemente apagar antes de saldo decorrer — agora transação única.
3. `SaldosRepository.AtualizarSaldo(nome, valor)` usa `FirstAsync` → nome inexistente lança `InvalidOperationException` (que **não** é `KeyNotFoundException`) → 500 com texto técnico. Corrigido na porta: `ErrNotFound` → 400.
4. Mensagem de validação de Saldo sem interpolação (`"{data.nome}"` literal) → corrigida (interpolar).
5. Fórmula de preço médio quebrada para VENDA em `POST /aportes` e ausência de proteção quando `quantidade` fecha ≤ 0 → **replicado** (comportamento externo idêntico), apenas sinalizado.
6. `ProcessarPendentes` cria a movimentação com `DateTime.Now` (ignora o vencimento real) → replicado (não "corrigir" sem avaliação).
7. `AtivosRepository` com `static HttpClient` compartilhado → em Go o client é um campo do adapter (comportamento equivalente).

## 11. Dúvidas (blocos para validar)

- **B1** — RESOLVIDO: erros de negócio → **4xx**, sentinels com `errors.Is`.
- **B2** — RESOLVIDO: tabela `rotina` descartada (sem schema e sem migração).
- ~~**B3**~~ — RESOLVIDO: timestamps em **RFC3339 (padrão do Go, fuso do runtime)**; dados antigos copiados **como estão**, normalização feita depois pelo usuário.
- **B4** — RESOLVIDO: cotações de `GET /aportes` paralelizadas.
- **B5** — RESOLVIDO: dinheiro → **INTEGER centavos** (JSON continua número decimal via `Money.MarshalJSON`).
- **B6** — RESOLVIDO: `GET /movimentacoes/persistentes/{id}` **replica** o C# e devolve persistente mesmo deletada.

---

**Fase 1 concluída — aguardando validação.** Resumo: 4 domínios (movimentacoes→saldos e salarios→movimentacoes+saldos via 2 interfaces pequenas), 24 endpoints no total, sem joins/queries trans-domínio, sem autenticação/paginação; port calcula paridade de contrato com correção acordada de bugs de concorrência e interpolação; schema 1:1 com o atual (com TEXT para decimal e RFC3339 UTC para datas, salvo B3).
