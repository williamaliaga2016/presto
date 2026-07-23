-- BBV-130 - Revisar EP Abogado (Escrituración y Garantías)
-- Script idempotente para BBVA_LEGALIZACION:
--   1. Crear la tabla revisar_ep_abogado

-- =============================================================================
-- PARTE 1: Crear tabla revisar_ep_abogado_bbva
-- =============================================================================

CREATE TABLE IF NOT EXISTS public.revisar_ep_abogado_bbva (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100),
    representante_legal             VARCHAR(200),
    ep_conforme                     VARCHAR(2) CHECK (ep_conforme IN ('SI', 'NO')),
    tipologia                       VARCHAR(150),
    casuistica                      VARCHAR(150),
    observaciones_legales           TEXT,
    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_rea_expediente
    ON public.revisar_ep_abogado (id_expediente)
    WHERE is_active = true AND row_status = true;


GRANT ALL PRIVILEGES ON TABLE public.revisar_ep_abogado_bbva TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.revisar_ep_abogado_bbva_id_seq TO multibanca;
