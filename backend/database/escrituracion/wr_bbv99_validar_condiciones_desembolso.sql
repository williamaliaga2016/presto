-- BBV-99 — Validar Condiciones Desembolso (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.validar_condiciones_desembolso CASCADE;

CREATE TABLE IF NOT EXISTS public.validar_condiciones_desembolso (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100),
    origen_tramite                  VARCHAR(50),
    confirmar_plan_pagos            BOOLEAN NOT NULL DEFAULT FALSE,
    requiere_escalamiento_comercial VARCHAR(2),
    suspendida                      BOOLEAN NOT NULL DEFAULT FALSE,
    conteo_caidas                   INTEGER NOT NULL DEFAULT 0,
    observaciones                   VARCHAR(500),
    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_validar_condiciones_desembolso_expediente
    ON public.validar_condiciones_desembolso (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_validar_condiciones_desembolso_bbva(p_id_expediente BIGINT)
RETURNS SETOF public.validar_condiciones_desembolso LANGUAGE sql STABLE AS $$
    SELECT * FROM public.validar_condiciones_desembolso
    WHERE id_expediente = p_id_expediente AND is_active = TRUE AND row_status = TRUE
    ORDER BY id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.validar_condiciones_desembolso TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.validar_condiciones_desembolso_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_validar_condiciones_desembolso_bbva(BIGINT) TO multibanca;

-- cat_actividades_ws
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Validar Condiciones Desembolso', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'validar_condiciones_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO');

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Gestión Comercial', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_gestion_comercial', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL');

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Gestionar Escalamientos', 'BBVA_ESCRITURACION_GESTIONAR_ESCALAMIENTOS', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'gestionar_escalamientos', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_GESTIONAR_ESCALAMIENTOS');
