-- BBV-97 — Realizar Gestión Comercial (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_gestion_comercial CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_gestion_comercial (
    id                      BIGSERIAL PRIMARY KEY,
    id_expediente           BIGINT NOT NULL,
    id_actividad            VARCHAR(100),
    origen_escalamiento     VARCHAR(100),
    cliente_desiste         VARCHAR(2),
    observaciones           VARCHAR(500),
    is_active               BOOLEAN NOT NULL DEFAULT TRUE,
    row_status              BOOLEAN NOT NULL DEFAULT TRUE,
    created_by              INTEGER NOT NULL,
    created_date            TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by             INTEGER,
    modified_date           TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_gestion_comercial_expediente
    ON public.realizar_gestion_comercial (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_realizar_gestion_comercial_bbva(p_id_expediente BIGINT)
RETURNS SETOF public.realizar_gestion_comercial LANGUAGE sql STABLE AS $$
    SELECT * FROM public.realizar_gestion_comercial
    WHERE id_expediente = p_id_expediente AND is_active = TRUE AND row_status = TRUE
    ORDER BY id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_gestion_comercial TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_gestion_comercial_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_gestion_comercial_bbva(BIGINT) TO multibanca;

-- cat_actividades_ws
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Gestión Comercial', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_gestion_comercial', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL');
