-- BBV-91 — Firmar Rep. Legal (Escrituración y Garantías)
-- Script idempotente para crear la tabla firmar_rep_legal

DROP TABLE IF EXISTS public.firmar_rep_legal CASCADE;

CREATE TABLE IF NOT EXISTS public.firmar_rep_legal (
    id                  BIGSERIAL PRIMARY KEY,
    id_expediente       BIGINT NOT NULL,
    id_actividad        VARCHAR(100),
    concepto_firma      VARCHAR(100),
    tipologia           VARCHAR(200),
    casuistica          VARCHAR(200),
    observaciones       VARCHAR(500),
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    row_status          BOOLEAN NOT NULL DEFAULT TRUE,
    created_by          INTEGER NOT NULL,
    created_date        TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by         INTEGER,
    modified_date       TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_firmar_rep_legal_expediente
    ON public.firmar_rep_legal (id_expediente)
    WHERE is_active = true AND row_status = true;

-- Stored Procedure — Consulta por expediente
CREATE OR REPLACE FUNCTION public.usp_select_firmar_rep_legal_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.firmar_rep_legal
LANGUAGE sql
STABLE
AS $$
    SELECT actividad.*
    FROM public.firmar_rep_legal actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE
      AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC
    LIMIT 1;
$$;

-- Permisos para el usuario multibanca
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.firmar_rep_legal TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.firmar_rep_legal_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_firmar_rep_legal_bbva(BIGINT) TO multibanca;

-- ============================================================
-- Seed de datos — INSERT en tabla catálogo
-- ============================================================

-- L41 — Concepto Firma Rep. Legal
WITH l41(codigo, descripcion, orden) AS (
    VALUES
        ('CRL-1', 'Escritura firmada Conforme', 1),
        ('CRL-2', 'Escritura NO firmada', 2)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'L41_CONCEPTO_FIRMA_REP_LEGAL', l41.descripcion, l41.codigo, NULL, true, l41.orden
FROM l41
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'L41_CONCEPTO_FIRMA_REP_LEGAL' AND c.valor = l41.codigo
);

-- L42 — Tipología Devolución Rep. Legal
WITH l42(codigo, descripcion, orden) AS (
    VALUES
        ('TRL-1', 'Escritura', 1),
        ('TRL-2', 'Documentación', 2)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'L42_TIPOLOGIA_REP_LEGAL', l42.descripcion, l42.codigo, NULL, true, l42.orden
FROM l42
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'L42_TIPOLOGIA_REP_LEGAL' AND c.valor = l42.codigo
);

-- L43 — Casuística Devolución Rep. Legal (dependiente de L42)
WITH l43(codigo, descripcion, padre_codigo, orden) AS (
    VALUES
        ('CasRL-1', 'Corrección abogado', 'TRL-1', 1),
        ('CasRL-2', 'Para corregir por la notaría', 'TRL-1', 2),
        ('CasRL-3', 'Crédito vencido', 'TRL-2', 3),
        ('CasRL-4', 'Estudio de títulos con observaciones sin subsanar', 'TRL-2', 4),
        ('CasRL-5', 'Avalúo con observaciones sin subsanar', 'TRL-2', 5)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'L43_CASUISTICA_REP_LEGAL', l43.descripcion, l43.codigo, padre.id, true, l43.orden
FROM l43
JOIN public.catalogo padre
    ON padre.tipo = 'L42_TIPOLOGIA_REP_LEGAL'
   AND padre.valor = l43.padre_codigo
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'L43_CASUISTICA_REP_LEGAL' AND c.valor = l43.codigo
);


-- ============================================================
-- Registro en cat_actividades_ws (BBVA_LEGALIZACION)
-- Necesario para que la actividad aparezca en la bandeja
-- ============================================================

-- Firmar Rep. Legal (esta HU)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Firmar Rep. Legal', 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'firmar_rep_legal', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL');

-- Destino: Realizar Entrega EP Firmada (cuando firma es conforme)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Entrega EP Firmada', 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_entrega_ep_firmada', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA');

-- Destino: Realizar Devolución EP (cuando escritura NO firmada)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Devolución EP', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_devolucion_ep', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP');
