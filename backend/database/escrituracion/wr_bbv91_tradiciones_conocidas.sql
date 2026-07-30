-- BBV-91 — Tabla tradiciones_conocidas + transiciones de Firmar Rep. Legal
-- Script idempotente

-- ============================================================
-- Tabla: tradiciones_conocidas
-- Usada para evaluar si aplica Excepción de Desembolso
-- ============================================================
CREATE TABLE IF NOT EXISTS public.tradiciones_conocidas (
    id                          BIGSERIAL PRIMARY KEY,
    ciudad_del_inmueble         VARCHAR(200),
    codigo_proyecto             VARCHAR(50) NOT NULL,
    proyecto                    VARCHAR(200),
    constructora                VARCHAR(200),
    firma                       VARCHAR(200),
    numero_identificacion_firma VARCHAR(50),
    nit_abogado                 VARCHAR(50),
    nombre_abogado              VARCHAR(200),
    vip                         VARCHAR(10),
    fecha_creacion_modificacion TIMESTAMP WITHOUT TIME ZONE,
    proyecto_av_tipo            VARCHAR(100),
    tipo_desembolso             VARCHAR(50),
    correo_constructora         VARCHAR(200),
    is_active                   BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                  BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                  INTEGER NOT NULL DEFAULT 1,
    created_date                TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_tradiciones_conocidas_codigo_proyecto
    ON public.tradiciones_conocidas (codigo_proyecto)
    WHERE is_active = true AND row_status = true;

-- SP: Consulta por código de proyecto (trae todos los datos)
CREATE OR REPLACE FUNCTION public.usp_select_tradiciones_conocidas_by_codigo_proyecto(
    p_codigo_proyecto VARCHAR)
RETURNS SETOF public.tradiciones_conocidas
LANGUAGE sql
STABLE
AS $$
    SELECT *
    FROM public.tradiciones_conocidas
    WHERE codigo_proyecto = p_codigo_proyecto
      AND is_active = TRUE
      AND row_status = TRUE
    ORDER BY id DESC
    LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.tradiciones_conocidas TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.tradiciones_conocidas_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_tradiciones_conocidas_by_codigo_proyecto(VARCHAR) TO multibanca;

-- ============================================================
-- cat_actividades_ws — destinos de Firmar Rep. Legal
-- ============================================================

-- Preformalizar (nuevo)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Preformalizar', 'BBVA_ESCRITURACION_PREFORMALIZAR', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'preformalizar', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_PREFORMALIZAR');

-- Realizar Excepción Desembolso (si no existe)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Excepción Desembolso', 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_excepcion_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO');

-- ============================================================
-- xpdl_transitions — Firmar Rep. Legal (4 transiciones)
-- ============================================================

-- 1. NO firmada → Devolución EP
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_DEVOLUCION', 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_DEVOLUCION',
       'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_DEVOLUCION');

-- 2. Firmada → Entrega EP Firmada (paralelo)
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP', 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP',
       'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP');

-- 3. Firmada → Preformalizar (paralelo)
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_PREFORMALIZAR', 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_PREFORMALIZAR',
       'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_PREFORMALIZAR', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_PREFORMALIZAR');

-- 4. Firmada + condición → Excepción Desembolso (paralelo condicional)
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_EXCEPCION_DESEMBOLSO', 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_EXCEPCION_DESEMBOLSO',
       'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_EXCEPCION_DESEMBOLSO');
