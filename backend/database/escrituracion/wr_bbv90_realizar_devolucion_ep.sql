-- BBV-90 — Realizar Devolución EP (Escrituración y Garantías)
-- Script idempotente
--
-- ORÍGENES (4 actividades que pueden enviar casos a Devolución EP):
--   1. Firmar Rep. Legal (BBV-91)         → concepto_firma rechazado
--   2. Realizar VB Final Abogado (BBV-96) → requiere_devolucion = "SI"
--   3. Revisar EP Abogado (BBV-130)       → ep_conforme = "NO"
--   4. Realizar Gestión Comercial         → retorno a Devolución EP
--
-- La grilla "Conceptos / Dictámenes Previos" se construye dinámicamente
-- consultando las tablas de cada origen por id_expediente.
--

DROP TABLE IF EXISTS public.realizar_devolucion_ep CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_devolucion_ep (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100),
    requiere_escalamiento_comercial VARCHAR(2),
    tipologia                       VARCHAR(200),
    casuistica                      VARCHAR(200),
    accion_a_seguir                 VARCHAR(100),
    observaciones                   VARCHAR(500),
    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_devolucion_ep_expediente
    ON public.realizar_devolucion_ep (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_realizar_devolucion_ep_bbva(p_id_expediente BIGINT)
RETURNS SETOF public.realizar_devolucion_ep LANGUAGE sql STABLE AS $$
    SELECT * FROM public.realizar_devolucion_ep
    WHERE id_expediente = p_id_expediente AND is_active = TRUE AND row_status = TRUE
    ORDER BY id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_devolucion_ep TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_devolucion_ep_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_devolucion_ep_bbva(BIGINT) TO multibanca;

-- cat_actividades_ws
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Devolución EP', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_devolucion_ep', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP');

-- xpdl_transitions — 4 destinos de salida de Devolución EP
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_GESTION_COMERCIAL', 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_GESTION_COMERCIAL',
       'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_GESTION_COMERCIAL');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_ESCRITURA', 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_ESCRITURA',
       'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_ESCRITURA');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_REP_LEGAL',
       'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_REP_LEGAL');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_EP_REGISTRADAS', 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_EP_REGISTRADAS',
       'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_EP_REGISTRADAS');
