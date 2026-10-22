-- Queries do dominio saldos (replica do SaldosRepository C#).
-- Convencoes: moeda = INTEGER centavos; timestamps = TEXT RFC3339.

-- name: FindSaldoByNome :one
SELECT * FROM saldos WHERE nome = ?1;

-- name: AddSaldoDelta :one
-- AtualizarSaldo(string, decimal): saldo.Valor += delta; devolve a linha.
-- Sem linha -> ErrSaldoNaoEncontrado (traduzido no store).
UPDATE saldos SET valor = valor + ?1 WHERE nome = ?2 RETURNING *;

-- name: SetSaldoValor :one
-- AtualizarSaldo(int, Saldo): sobrescreve o valor (SetValues).
UPDATE saldos SET valor = ?1 WHERE id = ?2 RETURNING *;
