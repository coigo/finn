-- Queries do dominio salarios (replica do SalarioRepository C#).

-- name: InsertSalario :one
INSERT INTO salarios (valor, criadoEm) VALUES (?1, ?2) RETURNING *;

-- name: FindSalarioMaisRecente :one
-- BuscarUltimo: ORDER BY id DESC LIMIT 1.
SELECT * FROM salarios ORDER BY id DESC LIMIT 1;
