-- BBV-141 — Realizar Vobo Gerencia COH (Escrituración y Garantías)
-- Script idempotente

CREATE TABLE IF NOT EXISTS public.vobo_gerencia_coh_bbva (
    id                  BIGSERIAL PRIMARY KEY,
    id_expediente       BIGINT NOT NULL,
    id_actividad        VARCHAR(100) NOT NULL,
    concepto_vobo       VARCHAR(20) CHECK (concepto_vobo IS NULL OR concepto_vobo IN ('Favorable', 'No Favorable')),
    observaciones       VARCHAR(1000),
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    row_status          BOOLEAN NOT NULL DEFAULT TRUE,
    created_by          INTEGER NOT NULL,
    created_date        TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by         INTEGER,
    modified_date       TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_vobo_gerencia_coh_bbva_expediente
    ON public.vobo_gerencia_coh_bbva (id_expediente)
    WHERE is_active = true AND row_status = true;

-- Permisos
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.vobo_gerencia_coh_bbva TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.vobo_gerencia_coh_bbva_id_seq TO multibanca;
