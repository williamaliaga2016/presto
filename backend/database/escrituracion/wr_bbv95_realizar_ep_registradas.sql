-- BBV-95 — Realizar EP Registradas (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_ep_registradas CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_ep_registradas (
    id                          BIGSERIAL PRIMARY KEY,
    id_expediente               BIGINT NOT NULL,
    id_actividad                VARCHAR(100),
    finalizacion                DATE,
    causal                      VARCHAR(200),
    fecha_finalizacion          DATE,
    confirmacion_ep_registrada  BOOLEAN NOT NULL DEFAULT FALSE,
    observaciones               VARCHAR(500),
    is_active                   BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                  BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                  INTEGER NOT NULL,
    created_date                TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                 INTEGER,
    modified_date               TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_ep_registradas_expediente
    ON public.realizar_ep_registradas (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_realizar_ep_registradas_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.realizar_ep_registradas
LANGUAGE sql STABLE
AS $$
    SELECT actividad.*
    FROM public.realizar_ep_registradas actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_ep_registradas TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_ep_registradas_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_ep_registradas_bbva(BIGINT) TO multibanca;

-- cat_actividades_ws
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar EP Registradas', 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_ep_registradas', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS');

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar VB Final Abogado', 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_vb_final_abogado', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO');
