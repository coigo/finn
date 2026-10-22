-- +goose Up
-- Schema do banco do finn (convenções do docs/analysis.md §4):
--   moeda = INTEGER centavos (B5); timestamps = TEXT RFC3339 (B3);
--   bool = INTEGER 0/1; enums seguem o storage do C# (assimetria preservada:
--   movimentacoes.tipo/categorias em TEXT, parcelas/persistentes em INTEGER).

CREATE TABLE movimentacoes_categorias (
    id         INTEGER PRIMARY KEY,
    nome       TEXT NOT NULL,
    tipo       TEXT NOT NULL CHECK (tipo IN ('ENTRADA', 'SAIDA', 'INVESTIMENTOS')),
    criadoEm   TEXT NOT NULL,
    deletadoEm TEXT
) STRICT;

-- seeds do OnModelCreating (ids fixos do enum MovimentacaoCategoriaDTO)
INSERT INTO movimentacoes_categorias (id, nome, tipo, criadoEm) VALUES
    (1,  'Comida e Mercado',          'SAIDA',   '0001-01-01T00:00:00Z'),
    (2,  'Educação e Desenvolvimento','SAIDA',   '0001-01-01T00:00:00Z'),
    (3,  'Investimentos',             'SAIDA',   '0001-01-01T00:00:00Z'),
    (4,  'Lazer e Bem-estar',         'SAIDA',   '0001-01-01T00:00:00Z'),
    (5,  'Serviços',                  'SAIDA',   '0001-01-01T00:00:00Z'),
    (6,  'Moradia',                   'SAIDA',   '0001-01-01T00:00:00Z'),
    (7,  'Transporte',                'SAIDA',   '0001-01-01T00:00:00Z'),
    (8,  'Saúde',                     'SAIDA',   '0001-01-01T00:00:00Z'),
    (9,  'Salário',                   'ENTRADA', '0001-01-01T00:00:00Z'),
    (10, 'Dividendo',                 'ENTRADA', '0001-01-01T00:00:00Z'),
    (11, 'Venda',                    'ENTRADA', '0001-01-01T00:00:00Z'),
    (12, 'Transferências',           'ENTRADA', '0001-01-01T00:00:00Z'),
    (13, 'Outros',                   'ENTRADA', '0001-01-01T00:00:00Z');

CREATE TABLE movimentacoes (
    id            INTEGER PRIMARY KEY,
    valor         INTEGER NOT NULL,
    tipo          TEXT    NOT NULL CHECK (tipo IN ('ENTRADA', 'SAIDA', 'INVESTIMENTOS')),
    categoriaId   INTEGER NOT NULL REFERENCES movimentacoes_categorias(id) ON DELETE CASCADE,
    descricao     TEXT    NOT NULL,
    data          TEXT    NOT NULL,
    parcelaId     INTEGER REFERENCES movimentacoes_parcelas(id) ON DELETE SET NULL,
    persistenteId INTEGER REFERENCES movimentacoes_persistentes(id) ON DELETE SET NULL,
    criadoEm      TEXT    NOT NULL
) STRICT;

CREATE INDEX idx_movimentacoes_categoriaId ON movimentacoes (categoriaId);
CREATE INDEX idx_movimentacoes_parcelaId ON movimentacoes (parcelaId);
CREATE INDEX idx_movimentacoes_persistenteId ON movimentacoes (persistenteId);
CREATE INDEX idx_movimentacoes_data ON movimentacoes (data);

CREATE TABLE movimentacoes_parcelas (
    id          INTEGER PRIMARY KEY,
    valor       INTEGER NOT NULL,
    numero      INTEGER NOT NULL,
    tipo        INTEGER NOT NULL CHECK (tipo IN (0, 1, 2)),
    categoriaId INTEGER NOT NULL REFERENCES movimentacoes_categorias(id) ON DELETE RESTRICT,
    descricao   TEXT    NOT NULL,
    vencimento  TEXT    NOT NULL,
    criadoEm    TEXT    NOT NULL,
    paga        INTEGER NOT NULL DEFAULT 0 CHECK (paga IN (0, 1))
) STRICT;

CREATE INDEX idx_movimentacoes_parcelas_vencimento ON movimentacoes_parcelas (vencimento);

CREATE TABLE movimentacoes_persistentes (
    id          INTEGER PRIMARY KEY,
    valor       INTEGER NOT NULL,
    descricao   TEXT    NOT NULL,
    tipo        INTEGER NOT NULL CHECK (tipo IN (0, 1, 2)),
    categoriaId INTEGER NOT NULL REFERENCES movimentacoes_categorias(id) ON DELETE RESTRICT,
    criadoEm    TEXT    NOT NULL,
    deletadoEm  TEXT
) STRICT;

CREATE TABLE saldos (
    id    INTEGER PRIMARY KEY,
    valor INTEGER NOT NULL,
    nome  TEXT NOT NULL
) STRICT;

INSERT INTO saldos (id, valor, nome) VALUES (1, 0, 'Corrente'), (2, 0, 'Investimentos');

CREATE TABLE saldos_historico (
    id       INTEGER PRIMARY KEY,
    valor    INTEGER NOT NULL,
    criadoEm TEXT    NOT NULL,
    resumoId INTEGER NOT NULL REFERENCES saldos(id) ON DELETE CASCADE
) STRICT;

CREATE INDEX idx_saldos_historico_resumoId ON saldos_historico (resumoId);

CREATE TABLE aportes (
    id            INTEGER PRIMARY KEY,
    identificador TEXT    NOT NULL UNIQUE,
    precoMedio    INTEGER NOT NULL,
    quantidade    INTEGER NOT NULL,
    categoria     TEXT    NOT NULL,
    criadoEm      TEXT    NOT NULL
) STRICT;

CREATE TABLE aportes_historico (
    id            INTEGER PRIMARY KEY,
    preco         INTEGER,
    identificador TEXT    NOT NULL,
    quantidade    INTEGER NOT NULL,
    categoria     TEXT    NOT NULL,
    tipo          TEXT    NOT NULL,
    data          TEXT    NOT NULL,
    criadoEm      TEXT    NOT NULL
) STRICT;

CREATE INDEX idx_aportes_historico_identificador ON aportes_historico (identificador);

CREATE TABLE salarios (
    id       INTEGER PRIMARY KEY,
    valor    INTEGER NOT NULL,
    criadoEm TEXT    NOT NULL
) STRICT;

-- +goose Down
DROP TABLE IF EXISTS salarios;
DROP INDEX IF EXISTS idx_aportes_historico_identificador;
DROP TABLE IF EXISTS aportes_historico;
DROP TABLE IF EXISTS aportes;
DROP INDEX IF EXISTS idx_saldos_historico_resumoId;
DROP TABLE IF EXISTS saldos_historico;
DROP TABLE IF EXISTS saldos;
DROP TABLE IF EXISTS movimentacoes;
DROP INDEX IF EXISTS idx_movimentacoes_categoriaId;
DROP INDEX IF EXISTS idx_movimentacoes_parcelaId;
DROP INDEX IF EXISTS idx_movimentacoes_persistenteId;
DROP INDEX IF EXISTS idx_movimentacoes_data;
DROP INDEX IF EXISTS idx_movimentacoes_parcelas_vencimento;
DROP TABLE IF EXISTS movimentacoes_parcelas;
DROP TABLE IF EXISTS movimentacoes_persistentes;
DROP TABLE IF EXISTS movimentacoes_categorias;
