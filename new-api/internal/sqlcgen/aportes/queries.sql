-- Queries do dominio aportes (replica do AporteRepository/AporteHistoricoRepository C#).
-- Sem ORDER BY na busca de historico: o C# nao ordena (ordem natural do EF ~ rowid/insercao) - paridade.

-- name: InsertAporte :one
INSERT INTO aportes (identificador, precoMedio, quantidade, categoria, criadoEm)
VALUES (?1, ?2, ?3, ?4, ?5)
RETURNING *;

-- name: InsertAporteHistorico :one
INSERT INTO aportes_historico (preco, identificador, quantidade, categoria, tipo, data, criadoEm)
VALUES (?1, ?2, ?3, ?4, ?5, ?6, ?7)
RETURNING *;

-- name: FindAporteByIdentificador :one
SELECT * FROM aportes WHERE identificador = ?1;

-- name: ListAportesComQuantidade :many
-- BuscarTodos: Where(a.Quantidade > 0).
SELECT * FROM aportes WHERE quantidade > 0;

-- name: UpdateAporte :execrows
-- AtualizarAporte: so precoMedio/quantidade (SetValues do C#).
-- execrows p/ detectar id inexistente (KeyNotFoundException -> 500).
UPDATE aportes SET precoMedio = ?1, quantidade = ?2 WHERE id = ?3;

-- name: ListAportesHistoricoByIdentificador :many
-- BuscarDetalhesDoAporte: sem ORDER BY (ordem de insercao), deletados nao existem aqui.
SELECT * FROM aportes_historico WHERE identificador = ?1;
