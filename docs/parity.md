# Paridade HTTP C# ↔ Go (Fase 5)

Resultado final da coleção `docs/parity-requests.md` (33 requests saldos/salarios/
movimentacoes/aportes/roteamento), rodada por `scripts/parity.ps1`:

```
== PARIDADE TOTAL na colecao (33 requests)
```

O script roda as duas APIs simultaneamente (C# em porta 18101, Go em 18102),
ambas com **banco SQLite fresco em diretório temporário**, e compara status +
corpo (com normalizações documentadas abaixo). Log completo em
`%TEMP%\opencode\parity\<timestamp>\log.txt`.

## Como o script roda sem tocar o banco real (lições da rodada suja)

1. `dotnet run --project <caminho>` usa o **diretório do projeto como cwd** —
   como `Data Source=data/finndb.sqlite` é relativo, uma rodada ingênua lê/escreve
   no `api/data/finndb.sqlite` REAL. O script copia o código de `api\` para o
   `%TEMP%` (robocopy, excluindo `bin`, `obj`, `wwwroot`, **`data`** e `.env`) e
   roda sempre essa cópia, com `data\` recriado vazio (o EF cria o SQLite).
2. PowerShell 5.1 mutila JSON em argv (aspas somem) e não lê UTF-8 de console
   (`não` → `n+ão`). Os corpos de request E as respostas vão via arquivo
   (UTF-8 sem BOM); só o código HTTP passa pelo stdout.
3. Comparações normalizam: timestamps → `<TS>`, escapes `\uXXXX` do C# →
   caractere real, e zeros à direita de decimais (ver B5 abaixo) nos dois lados.

## Divergências ACEITAS (decisões do usuário — não corrigir)

| # | Tema | C# | Go | Decisão |
|---|------|----|----|---------|
| D1 | Escala decimal (B5) | preserva escala do decimal (`25.50`, `0.0`, `100000.00`) | normaliza (`25.5`, `0`, `100000`) | manter B5 (ECO; documentado) |
| D2 | Escapes JSON | `\uXXXX` (System.Text.Json) | UTF-8 cru | manter raw (ACEITO; comparável via normalização) |
| D3 | JSON malformado | 400 + ValidationProblemDetails (RFC7807, corpo longo) | 400 + `{"status":400,"message":"corpo inválido"}` | manter (B2) |
| D4 | Panic de goroutine | mensagem runtime própria (ex.: `Integer divided by zero`) | `{"status":500,"message":"runtime error: integer divide by zero"}` | shape 500 idêntico; texto depende do runtime |

## Divergência de COMPORTAMENTO descoberta e replicada

- Erros de negócio no C# são `BusinessError`/`KeyNotFoundException` e o
  `ErrorHandler` mapeia **`KeyNotFoundException → 400`; TODO o resto → 500 com
  a mensagem da exceção vazada**. O mapa Go exato:
  - `saldos`: "Saldo não encontrado!"/"Reusmo não encontrado!" → 400 (KeyNotFound).
  - `salarios`: "Salário não cadastrado" → 400; **"Use um valor positivo" → 500** (BusinessError).
  - `movimentacoes`: GET/undo não-encontrado → **500** (BusinessError awaited);
    `PUT/DELETE persistentes` → **200 mesmo com erro** (veja abaixo); `saldoHistorico: []` no JSON (nav collection do EF sempre vazia).
  - `aportes`: TODOS → 500 (`Aporte não encontrado`, `Ativo não encontrado!`,
    `O Ticker X não existe! `, `Quantidade inválida`, `Preço médio inválido`,
    **`Tipo inválido`** — ArgumentException do getter).
- **PUT/DELETE `/persistentes` C# é async void sem await**: a resposta é SEMPRE
  200 + echo (e 200 + id pedido no DELETE), e um erro (ex.: id inexistente)
  vira exceção de task não observada que **mata o processo C#** depois. A Go
  repassa ao serviço, registra o erro em log e responde 200 — **não replicamos
  a queda do processo** (divergência documentada de robustez).
- `GET /persistentes/{id}` retorna entidade soft-deletada também (sem filtro
  `deletadoEm`) — B6, replicado.
- Roteamento: sem wwwroot, unmatched e `/` → 404 vazio nas duas (coleção [32], [33]).
- POST `/api/movimentacoes/persistentes` → 405 nas duas ( rota não existe no C#; coleção [19]).

## Pendências de migração de dados (fora da paridade HTTP)

Ao migrar os dados reais do `api/data/finndb.sqlite` para o `data/finn.sqlite`
Go, validar explicitamente (não cobertos pela coleção, que usa bancos frescos):

1. **Timestamps B3**: o C# grava `2025-05-01 12:34:56.789` (SQLite TEXT); o Go
   grava/lê RFC3339 (`2025-05-01T12:34:56ZZ`). O parser Go já tolera o formato
   antigo; migrador deve converter todas as colunas de data (`data, vencimento,
   criadoEm, deletadoEm, dataCompra`).
2. **Dinheiro**: C# SQLite guarda decimal textual (ex.: `25.50`); Go guarda
   INTEGER cents (`2550`). Migrador precisa `ROUND(x*100)` (B5: trunca
   sub-centavos — valores com mais de 2 casas mudam).
3. **Categorias TEXT↔enum nos aportes** (`'ACAO'` ↔ 1) — já no store; conferir dados existentes.
4. **IDs**: FKs (`persistenteId` nas movimentações, `movimentacaoId` no histórico de aportes etc.) devem ser preservadas 1:1.
