-- BBV-96 — Realizar VB Final Abogado (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_vb_final_abogado CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_vb_final_abogado (
    id                          BIGSERIAL PRIMARY KEY,
    id_expediente               BIGINT NOT NULL,
    id_actividad                VARCHAR(100),
    requiere_devolucion         VARCHAR(2),
    tipologia                   VARCHAR(200),
    casuistica                  VARCHAR(200),
    origen_tramite              VARCHAR(50),
    bandera_excepcion           BOOLEAN NOT NULL DEFAULT FALSE,
    observaciones               VARCHAR(500),
    is_active                   BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                  BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                  INTEGER NOT NULL,
    created_date                TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                 INTEGER,
    modified_date               TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_vb_final_abogado_expediente
    ON public.realizar_vb_final_abogado (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_realizar_vb_final_abogado_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.realizar_vb_final_abogado
LANGUAGE sql STABLE
AS $$
    SELECT actividad.*
    FROM public.realizar_vb_final_abogado actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_vb_final_abogado TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_vb_final_abogado_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_vb_final_abogado_bbva(BIGINT) TO multibanca;

-- cat_actividades_ws
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar VB Final Abogado', 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_vb_final_abogado', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO');

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Validar Condiciones Desembolso', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'validar_condiciones_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO');

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Gestionar Control de Garantías', 'BBVA_ESCRITURACION_GESTIONAR_CONTROL_GARANTIAS', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'gestionar_control_garantias', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_GESTIONAR_CONTROL_GARANTIAS');


INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Devolución EP', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_devolucion_ep', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP');
