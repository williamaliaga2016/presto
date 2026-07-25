-- BBV-94 — Realizar Excepción Desembolso (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.excepcion_desembolso CASCADE;

CREATE TABLE IF NOT EXISTS public.excepcion_desembolso (
    id                          BIGSERIAL PRIMARY KEY,
    id_expediente               BIGINT NOT NULL,
    id_actividad                VARCHAR(100) NOT NULL,
    excepcion_autorizada        VARCHAR(2) CHECK (excepcion_autorizada IN ('SI','NO')),
    requiere_vobo_gerencia      VARCHAR(2) CHECK (requiere_vobo_gerencia IN ('SI','NO')),
    confirmacion_excepcion      BOOLEAN NOT NULL DEFAULT FALSE,
    observaciones_excepcion     VARCHAR(1000),
    origen_caso                 VARCHAR(50),
    is_active                   BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                  BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                  INTEGER NOT NULL,
    created_date                TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                 INTEGER,
    modified_date               TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_excepcion_desembolso_expediente
    ON public.excepcion_desembolso (id_expediente)
    WHERE is_active = true AND row_status = true;


-- Permisos
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.excepcion_desembolso TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.excepcion_desembolso_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_excepcion_desembolso_bbva(BIGINT) TO multibanca;


-- ============================================================
-- Registro en cat_actividades_ws (bandeja)
-- ============================================================

-- Realizar Excepción Desembolso (esta HU)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Excepción Desembolso', 'BBVA_ESCRITURACION_EXCEPCION_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'excepcion_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_EXCEPCION_DESEMBOLSO');

-- Destino: Realizar Vobo Gerencia COH (cuando requiere_vobo_gerencia = 'SI')
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Vobo Gerencia COH', 'BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_vobo_gerencia_coh', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH');

-- Destino: Validar Condiciones Desembolso (cuando requiere_vobo_gerencia = 'NO')
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Validar Condiciones Desembolso', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'validar_condiciones_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO');
