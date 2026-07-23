-- BBV-92 — Realizar Entrega EP Firmada (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_entrega_ep_firmada CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_entrega_ep_firmada (
    id                  BIGSERIAL PRIMARY KEY,
    id_expediente       BIGINT NOT NULL,
    id_actividad        VARCHAR(100),
    entregado_a         VARCHAR(200),
    aplica_excepcion    VARCHAR(2),
    observaciones       VARCHAR(500),
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    row_status          BOOLEAN NOT NULL DEFAULT TRUE,
    created_by          INTEGER NOT NULL,
    created_date        TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by         INTEGER,
    modified_date       TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_entrega_ep_firmada_expediente
    ON public.realizar_entrega_ep_firmada (id_expediente)
    WHERE is_active = true AND row_status = true;

-- Stored Procedure — Consulta por expediente
CREATE OR REPLACE FUNCTION public.usp_select_realizar_entrega_ep_firmada_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.realizar_entrega_ep_firmada
LANGUAGE sql
STABLE
AS $$
    SELECT actividad.*
    FROM public.realizar_entrega_ep_firmada actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE
      AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC
    LIMIT 1;
$$;

-- Permisos
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_entrega_ep_firmada TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_entrega_ep_firmada_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_entrega_ep_firmada_bbva(BIGINT) TO multibanca;


-- ============================================================
-- Registro en cat_actividades_ws (BBVA_LEGALIZACION)
-- Necesario para que la actividad aparezca en la bandeja
-- ============================================================

-- Realizar Entrega EP Firmada (esta HU)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Entrega EP Firmada', 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_entrega_ep_firmada', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA');

-- Destino: Realizar Recepción Boleta (siempre)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Recepción Boleta', 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_recepcion_boleta', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA');

-- Destino: Realizar Excepción Desembolso (si aplica excepción)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Excepción Desembolso', 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_excepcion_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO');
