-- Queries do dominio movimentacoes (replica do MovimentacaoRepository C#).
-- Convencoes: moeda = INTEGER centavos; timestamps = TEXT RFC3339 (strftime entende
-- 'T' e espaco, entao pendentes funcionam nos dados antigos e novos - B3).

-- name: InsertMovimentacao :one
INSERT INTO movimentacoes (valor, tipo, categoriaId, descricao, data, parcelaId, persistenteId, criadoEm)
VALUES (?1, ?2, ?3, ?4, ?5, ?6, ?7, ?8)
RETURNING *;

-- name: FindMovimentacaoByID :one
-- BuscarMovimentacao; sem linha -> ErrMovimentacaoNaoEncontrada (traduzido no store).
SELECT * FROM movimentacoes WHERE id = ?1;

-- name: DeleteMovimentacao :exec
-- ApagarMovimentacao: ExecuteDelete (duro).
DELETE FROM movimentacoes WHERE id = ?1;

-- name: InsertParcela :one
-- CriarMovimentacaoParcela: AddRange em Go vira insert por linha (RETURNING p/ pegar o id).
INSERT INTO movimentacoes_parcelas (valor, numero, tipo, categoriaId, descricao, vencimento, criadoEm, paga)
VALUES (?1, ?2, ?3, ?4, ?5, ?6, ?7, ?8)
RETURNING *;

-- name: FindParcelasPorIDs :many
-- BuscarParcelasPorId (Contains -> IN).
SELECT * FROM movimentacoes_parcelas WHERE id IN (sqlc.slice('ids'));

-- name: MarkParcelaPaga :exec
-- MarcarParcelaComoPaga: sem checagem de existencia (no-op se ausente - paridade).
UPDATE movimentacoes_parcelas SET paga = 1 WHERE id = ?1;

-- name: ListMovimentacoesPorPeriodo :many
-- BuscarPorPeriodo: LEFT JOIN categorias, ORDER BY data DESC (fixo, igual ao C#).
-- GROUP BY id do SQL cru nao altera o resultado (id e PK) - replica com SELECT simples.
SELECT
    m.id        AS id,
    m.valor     AS valor,
    m.tipo      AS tipo,
    m.descricao AS descricao,
    mc.nome     AS categoria,
    m.data      AS data
FROM movimentacoes m
LEFT JOIN movimentacoes_categorias mc ON mc.id = m.categoriaId
WHERE m.data >= ?1 AND m.data <= ?2
ORDER BY m.data DESC;

-- name: ListPendentesDoMes :many
-- BuscarPendentesDoMes: UNION parcela/persistente, mes corrente do argumento.
-- Parcelas: nao pagas + vencimento no mes. Persistentes: nao deletadas e sem
-- movimentacao no mes. vencimento nulo no ramo PERSISTENTE (shape do C#).
SELECT
    mp.id,
    mp.valor,
    mp.descricao,
    mc.nome  AS categoria,
    mc.id    AS categoriaId,
    mp.vencimento AS vencimento,
    mp.tipo  AS tipo,
    'PARCELA' AS tipoDerivado
FROM movimentacoes_parcelas mp
LEFT JOIN movimentacoes_categorias mc ON mc.id = mp.categoriaId
WHERE mp.paga = 0
  AND strftime('%m', mp.vencimento) = sqlc.arg('mes')
  AND strftime('%Y', mp.vencimento) = sqlc.arg('ano')
UNION
SELECT
    mpd.id,
    mpd.valor,
    mpd.descricao,
    mcd.nome  AS categoria,
    mcd.id    AS categoriaId,
    ''        AS vencimento,
    mpd.tipo  AS tipo,
    'PERSISTENTE' AS tipoDerivado
FROM movimentacoes_persistentes mpd
LEFT JOIN movimentacoes_categorias mcd ON mcd.id = mpd.categoriaId
WHERE mpd.deletadoEm IS NULL
  AND mpd.id NOT IN (
      SELECT persistenteId FROM movimentacoes
      WHERE persistenteId IS NOT NULL
        AND strftime('%m', data) = sqlc.arg('mes')
        AND strftime('%Y', data) = sqlc.arg('ano')
  );

-- name: ListCategorias :many
-- BuscarCategorias: sem filtro deletadoEm (paridade) e sem ORDER BY.
SELECT id, nome, tipo, criadoEm, deletadoEm FROM movimentacoes_categorias;

-- name: InsertPersistente :one
INSERT INTO movimentacoes_persistentes (valor, descricao, tipo, categoriaId, criadoEm)
VALUES (?1, ?2, ?3, ?4, ?5)
RETURNING *;

-- name: FindMovimentacaoPersistenteByID :one
-- BuscarMovimentacaoPersistente: busca cru (deletadas incluidas - B6);
-- sem linha -> ErrMovimentacaoNaoEncontradaExcl (traduzido no store).
SELECT * FROM movimentacoes_persistentes WHERE id = ?1;

-- name: UpdatePersistente :exec
-- EditarMovimentacaoPersistente: so descricao/valor/categoriaId.
UPDATE movimentacoes_persistentes
SET descricao = ?1, valor = ?2, categoriaId = ?3
WHERE id = ?4;

-- name: SoftDeletePersistente :exec
-- DeletarMovimentacaoPersistente: seta deletadoEm (soft).
UPDATE movimentacoes_persistentes SET deletadoEm = ?1 WHERE id = ?2;
