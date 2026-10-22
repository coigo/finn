# Contexto

Você é um engenheiro sênior especialista em C# (.NET) e Go. Sua tarefa é reescrever a API em C# localizada em `[caminho do projeto C#]` para Go, mantendo o comportamento externo idêntico (rotas, contratos de request/response, status codes, regras de negócio e validações).

Stack atual: [ex.: Entity Framework, SQL Server, MediatR]
Stack alvo em Go: Go [1.27], [chi/gin/echo], **sqlc** (engine `sqlite`, gerando código sobre `database/sql`), **SQLite** via `modernc.org/sqlite` (driver puro Go, sem CGO), migrations com [goose], testify.
Destino do novo projeto: `/new-api`

# Regra principal de arquitetura

## 1. Organização por domínio, não por camada

O código deve ser organizado **por domínio (package by feature)**. É proibido criar pastas globais por camada como `internal/handler`, `internal/service`, `internal/repository` ou `internal/domain`. Tudo que pertence a um domínio (entidades, service, interface de repository, implementação, handlers, DTOs, queries SQL e testes) fica junto, no pacote desse domínio.

## 2. UseCases agrupados em Services

O projeto C# organiza a lógica em **UseCases** (um por operação). No Go, eles **não** devem virar um arquivo/struct por caso de uso. Devem ser **agrupados em um Service por domínio**, onde cada UseCase vira um método do service.

Exemplo: `CreateOrderUseCase`, `CancelOrderUseCase`, `GetOrderByIdUseCase` → `orders.Service` com os métodos `Create`, `Cancel`, `GetByID`.

## 3. Estrutura esperada

```
/cmd/api/main.go              → bootstrap, config, injeção manual de dependências, montagem das rotas
/db/migrations/               → migrations SQL (schema único do banco, também lido pelo sqlc)
/sqlc.yaml                    → um bloco `sql` por domínio, todos apontando para o mesmo schema
/internal/aportes/            → domínio "aportes" (pacote `aportes`)
    order.go                  → entidades, value objects e erros do domínio
    service.go                → Service (antigos usecases) + construtor NewService
    repository.go             → interface Repository (definida aqui, no consumidor)
    store.go                  → implementação sqlite do Repository (envolve o código do sqlc)
    handler.go                → handlers HTTP, DTOs de request/response, mapeamento erro → status code
    routes.go                 → func (h *Handler) Routes() / Mount(r) com as rotas do domínio
    queries.sql               → queries sqlc deste domínio
    internal/sqlcgen/         → código gerado pelo sqlc (NUNCA editar à mão; `internal` impede import externo)
    service_test.go, handler_test.go, store_test.go
/internal/platform/           → infraestrutura transversal SEM regra de negócio:
    config/, logger/, sqlitex/ (abertura do banco, PRAGMAs, TxManager), httpx/ (helpers de resposta/erro JSON,
    paginação), middleware/ (auth, logging, recover, CORS, request ID), testdb/ (helper de banco para testes)
```

Se um domínio ficar grande, pode ser dividido em arquivos adicionais (ex.: `service_cancel.go`) ou, em último caso, em subpacotes do próprio domínio. Justifique qualquer desvio.

## 4. Regras de fronteira entre domínios

- Cada domínio expõe apenas o necessário: `Service`, `Handler`/`Routes`, entidades e erros públicos. Detalhes (implementação do store, queries, DTOs internos) ficam não exportados sempre que possível.
- **Um domínio nunca importa o `store`, o `sqlcgen` ou as tabelas de outro domínio.** Se o domínio A precisa de dados ou comportamento do domínio B, A define uma **interface pequena** com o que precisa (no próprio pacote de A) e o `main.go` injeta o `Service` de B. Nada de joins SQL entre tabelas de domínios diferentes sem eu aprovar (se o C# faz isso, sinalize na Fase 1 e proponha uma alternativa).
- **Sem dependência circular.** Se dois domínios se referenciam mutuamente, isso indica fronteira mal desenhada: reavalie a divisão ou extraia o conceito compartilhado, e me consulte.
- `internal/platform` nunca importa domínios. Domínios podem importar `platform`.
- Não crie um pacote `common`/`shared`/`utils` genérico. Código compartilhado só entra em `platform` se for infraestrutura; regra de negócio compartilhada pertence a um domínio dono.
- Handlers só falam com o Service do próprio domínio. O Service não conhece `net/http`, `database/sql` nem `sqlcgen`.

# Regras específicas de sqlc + SQLite

- O **schema vem das migrations** centralizadas em `migrations` (a ordem das migrations e as FKs entre tabelas são globais ao banco). Não duplique o schema em outro arquivo.
- O `sqlc.yaml` tem **um bloco `sql` por domínio**: todos com `schema: db/migrations` e `engine: sqlite`, cada um com seu `queries: internal/<dominio>/queries.sql` e saída em `internal/<dominio>/internal/sqlcgen`. Exemplo:

```yaml
version: "2"
sql:
  - engine: sqlite
    schema: db/migrations
    queries: internal/orders/queries.sql
    gen:
      go:
        package: sqlcgen
        out: internal/orders/internal/sqlcgen
        # overrides, emit_pointers_for_null_types etc.
```

- O código gerado **não vaza** do pacote do domínio. O `store.go` converte os models do sqlc em entidades do domínio.
- Toda query usa anotações do sqlc (`-- name: CreateOrder :one`, `:many`, `:exec`, `:execrows`). Nunca concatene SQL com strings.
- Cada domínio só escreve queries sobre as **suas próprias tabelas**. Acesso a dados de outro domínio se faz via Service dele (regra de fronteira acima).
- **Filtros opcionais**: use `sqlc.narg('campo')` com o padrão `WHERE (sqlc.narg('status') IS NULL OR status = sqlc.narg('status'))`. Para listas, use `sqlc.slice('ids')`. O sqlc não suporta `ORDER BY` dinâmico: se o C# permite ordenar por campos variáveis, crie queries separadas por ordenação ou proponha uma alternativa e me consulte.
- **Transações**: `platform/sqlitex` expõe `WithTx(ctx, func(ctx) error) error`, que abre a transação e a coloca no `context`. Os stores obtêm o executor (`DBTX`) a partir do contexto (tx se existir, senão o `*sql.DB`) e usam `queries.WithTx`/`New(dbtx)`. Assim o Service controla a fronteira transacional sem conhecer `database/sql`, e uma transação pode atravessar mais de um domínio quando for realmente necessário. Qualquer operação transacional multi-domínio deve ser sinalizada na Fase 1.
- **Configuração do SQLite** (na abertura do banco): `PRAGMA foreign_keys = ON` (vale por conexão, então configure via DSN), `journal_mode = WAL`, `busy_timeout`, e um pool adequado ao modelo de escrita única do SQLite (ex.: `SetMaxOpenConns(1)` para escrita ou pools separados para leitura e escrita). Documente a decisão.
- **Mapeamento de tipos** (SQLite tem type affinity). Defina e documente uma convenção única para o projeto, usando `overrides` no `sqlc.yaml` quando necessário:
  - `decimal` → INTEGER (centavos/menor unidade) ou TEXT, mapeado para uma lib decimal no Go. **Nunca `REAL`/`float64` para dinheiro**.
  - `DateTime/DateTimeOffset` → INTEGER (unix) ou TEXT em RFC3339 UTC, com uma única convenção em todo o projeto.
  - `Guid` → TEXT. `bool` → INTEGER 0/1. Enums → TEXT ou INTEGER, conforme o C#.
  - Colunas nullable → `sql.Null*` ou ponteiros (configure `emit_pointers_for_null_types` se preferir).
- **Diferenças de comportamento do banco** a investigar: `LIKE` é case-insensitive só para ASCII, collation e ordenação de strings diferem do banco original, `ALTER TABLE` é limitado, tipagem é flexível (use `STRICT` tables quando possível), e `AUTOINCREMENT`/`ROWID` diferem de IDENTITY.

# Processo (siga as fases na ordem e pare no fim de cada uma)

## Fase 1: Entender a lógica e definir os domínios
1. Leia todo o projeto C#: controllers, usecases, entidades, repositories, DTOs, validators, middlewares, filtros, configurações, mapeamentos do ORM e migrations existentes.
2. Produza um documento `docs/analysis.md` contendo:
   - **Mapa de domínios**: proponha a divisão em domínios (ex.: orders, users, catalog), com a justificativa de cada fronteira, quais entidades/tabelas pertencem a qual domínio e quais usecases entram em qual Service.
   - **Grafo de dependências entre domínios**: quem depende de quem, qual interface o consumidor precisa, e eventuais ciclos ou joins entre domínios com a proposta de solução.
   - Lista de endpoints por domínio (método, rota, auth, request, response, status codes)
   - Regras de negócio e validações de cada usecase (inclusive as implícitas, como valores default, normalizações, ordenação e paginação)
   - **Modelo de dados**: tabelas (agrupadas por domínio), colunas, tipos, PKs, FKs (marcando as que cruzam domínios), índices, constraints, defaults e a proposta de schema equivalente em SQLite
   - **Mapa de queries** por domínio: para cada usecase, as queries necessárias, filtros, ordenação, paginação, joins e onde há transação (marcando as multi-domínio). Aponte o que o sqlc não resolve diretamente (queries dinâmicas, `ORDER BY` variável) e a solução proposta
   - Dependências externas (outros bancos, filas, APIs, cache)
   - Tratamento de erros e exceções e como são traduzidos para HTTP (incluindo violações de constraint → erro de domínio)
   - Pontos sem equivalente direto em Go e a decisão proposta (ex.: LINQ, EF lazy loading/change tracking, DI, atributos de validação, async/await, DateTime/timezones, decimal)
   - Se há **migração de dados** do banco atual para o SQLite (ou se começa vazio)
   - Dúvidas e ambiguidades
3. **Pare e aguarde minha validação** (principalmente da divisão em domínios) antes de seguir.

## Fase 2: Criar os testes (antes da implementação)
Trabalhe um domínio por vez, começando pelos que não dependem de outros. Os testes ficam no pacote do próprio domínio.
1. Escreva testes que descrevam o comportamento esperado, derivados do código C# (e dos testes C# existentes, se houver).
2. `service_test.go`: testes unitários do Service usando fakes/mocks da interface `Repository` e das interfaces de outros domínios. Use table-driven tests e cubra: caminho feliz, validações, regras de negócio, erros do repository e casos de borda.
3. `handler_test.go`: testes com `httptest`, validando status code, headers e o JSON de resposta (nomes de campos, formato de datas e nulos devem bater com o C#).
4. `store_test.go`: testes de integração contra **SQLite real** (sem mocks), usando o helper `platform/testdb` (banco temporário por teste em `t.TempDir()` ou `file::memory:?cache=shared`; atenção: `:memory:` simples cria um banco diferente por conexão do pool), aplicando as migrations reais antes de cada teste. Cubra constraints, transações com rollback, filtros, ordenação e paginação.
5. Os testes devem compilar, mas podem falhar nesta fase. Crie apenas os stubs mínimos (assinaturas/interfaces) necessários.
6. Gere uma tabela de rastreabilidade: usecase C# → domínio → método do Service → queries sqlc → testes que o cobrem.
7. **Pare e aguarde validação.**

## Fase 3: Implementar os services
1. Implemente o `Service` de cada domínio, agrupando os usecases conforme o plano da Fase 1.
2. A interface `Repository` e as interfaces de dependências de outros domínios são definidas no pacote do domínio consumidor.
3. Receba dependências via construtor (`NewService(...)`) e passe `context.Context` como primeiro parâmetro em todos os métodos.
4. Use erros de domínio tipados/sentinela (`errors.Is/As`) e `fmt.Errorf("...: %w", err)` para contexto. O Service não importa `database/sql`, `net/http` nem `sqlcgen`.
5. Rode os testes de service até passarem. Não altere um teste para fazê-lo passar sem me avisar e explicar por quê.

## Fase 4: Implementar o restante da arquitetura
Siga esta ordem:
1. **Schema**: escreva as migrations em `db/migrations` (preferencialmente `STRICT` tables, FKs, índices e constraints equivalentes ao original).
2. **Queries**: escreva o `queries.sql` de cada domínio, conforme o mapa da Fase 1.
3. **sqlc**: crie o `sqlc.yaml` (um bloco por domínio), rode `sqlc generate` e `sqlc vet`/`sqlc compile`, e confirme que o código gerado compila. Inclua os comandos num `Makefile` (`make sqlc`, `make migrate`, `make test`, `make lint`).
4. **Platform**: abertura do banco com os PRAGMAs e pool configurado, `WithTx`, execução das migrations no startup (ou comando separado), config, logger, helpers HTTP e middlewares (auth, logging, recover, CORS, request ID e o que mais existir no C#).
5. **Stores**: implemente o `store.go` de cada domínio, envolvendo o código gerado, mapeando models → entidades do domínio, traduzindo erros (`sql.ErrNoRows` → erro de domínio "não encontrado", violação de UNIQUE/FK → erro de domínio correspondente).
6. **Handlers e rotas**: `handler.go` e `routes.go` de cada domínio: binding/validação de input, chamada ao Service, mapeamento de erros de domínio → HTTP status, serialização idêntica ao contrato original. DTOs de request/response ficam separados das entidades de domínio.
7. **`main.go`**: carregue config (env vars), abra o banco, construa stores → services → handlers de cada domínio, injete as dependências entre domínios, monte o router e faça graceful shutdown (fechando o banco corretamente).
8. Rode todos os testes, `go vet` e `golangci-lint`. Se possível, configure uma regra de lint (ex.: `depguard` ou `go-arch-lint`) para impedir imports proibidos entre domínios.

## Fase 5: Verificação de paridade
1. Liste qualquer diferença de comportamento entre C# e Go (conhecida ou suspeita), incluindo as causadas pela troca de banco (collation, ordenação, arredondamento, datas).
2. Sugira como validar a paridade (ex.: coleção de requests rodada contra as duas APIs e comparação dos resultados) e, se houver, como validar a migração de dados.

# Diretrizes de código

- Go idiomático: pacotes pequenos e coesos, interfaces pequenas, retornar erros (sem panic para fluxo normal), sem nomes estilo C# (`IUserRepository`, `UserDto`...). Evite repetição de nome do pacote (`orders.Service`, não `orders.OrderService`).
- Sem over-engineering: não replique padrões do C# (MediatR, AutoMapper, camadas extras) se não agregarem valor em Go.
- Preserve o contrato externo: nomes de campos JSON, formatos de data, tratamento de null/omitempty, códigos de erro e mensagens.
- Atenção a diferenças sutis: `decimal`, datas/timezones, strings vazias vs null, ordenação default, case sensitivity e paginação.
- Não edite arquivos gerados pelo sqlc. Se algo estiver errado, corrija a query, o schema ou o `sqlc.yaml` e gere de novo.
- Comentários apenas onde a regra de negócio não for óbvia.

# Regras de trabalho

- Se algo estiver ambíguo, pergunte antes de assumir.
- Os dominios propostos nesse arquivo são apenas exemplos. Utilize o que de fato existe no código original
- Nunca invente comportamento que não exista no código C#. Se achar um bug no original, sinalize e pergunte se deve ser replicado ou corrigido.
- Trabalhe **um domínio por vez**, terminando o ciclo completo (testes → service → store → handler) de um antes de iniciar o próximo, começando pelos domínios sem dependências.
- Ao final de cada fase, resuma o que foi feito, as decisões tomadas e o que falta.

Comece pela Fase 1.