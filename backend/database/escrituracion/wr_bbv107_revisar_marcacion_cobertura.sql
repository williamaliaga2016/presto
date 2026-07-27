-- BBV-107 — Revisar Marcación de Cobertura (Escrituración y Garantías)
-- Script idempotente

CREATE TABLE IF NOT EXISTS public.revision_marcacion_cobertura_bbva (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100) NOT NULL,

    -- CA05 — Notificación a Colocaciones (bloqueante, CA08)
    email_area_colocaciones         VARCHAR(150),

    -- Identificación (Precargado Editable salvo consecutivo)
    consecutivo                     VARCHAR(50),
    tipo_documento                  VARCHAR(10),
    numero_documento                VARCHAR(30),
    tipo_tramite                    VARCHAR(30),
    nombre                          VARCHAR(200),

    -- Proyecto / Constructora (Diligencia SITCAR)
    constructora                    VARCHAR(200),
    proyecto                        VARCHAR(200),
    fecha_aceptacion_plataforma     DATE,
    tipo_vivienda                   VARCHAR(50),

    -- Valores y obligación
    valor_subsidio                  NUMERIC(18,2),
    numero_obligacion               VARCHAR(50),
    fecha_desembolso                DATE,
    fecha_proximo_canon             DATE,
    valor_desembolso                NUMERIC(18,2),
    intereses_corrientes            NUMERIC(18,2),
    capital                         NUMERIC(18,2),
    seguros                         NUMERIC(18,2),
    cuota_mensual                   NUMERIC(18,2),
    plazo                           INTEGER,
    observacion                     VARCHAR(1000),

    -- Solicitud / Respuesta de marcación (SITCAR)
    fecha_solicitud_marcacion       DATE,
    hora_solicitud_marcacion        VARCHAR(5),   -- formato HH:MM
    fecha_respuesta_marcacion       DATE,          -- condicionado
    hora_respuesta_marcacion        VARCHAR(5),    -- condicionado, formato HH:MM
    responsable_m5                  VARCHAR(150),

    -- Resolución
    no_resolucion                   VARCHAR(50),
    fecha_resolucion                DATE,
    fecha_envio_resolucion          DATE,
    estado_proceso                  VARCHAR(50),

    -- CA07 — Observaciones generales
    observaciones                   VARCHAR(1000),

    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_revision_marcacion_cobertura_bbva_expediente
    ON public.revision_marcacion_cobertura_bbva (id_expediente)
    WHERE is_active = true AND row_status = true;

-- Permisos
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.revision_marcacion_cobertura_bbva TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.revision_marcacion_cobertura_bbva_id_seq TO multibanca;


-- ============================================================
-- Registro en cat_actividades_ws (bandeja)
-- ============================================================

-- Revisar Marcación de Cobertura (esta HU) — actividad independiente (ver sección 0.1
-- de contexto/BBV-107_RECETA_IMPLEMENTACION.md): no fue pre-registrada por ningún script anterior.
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Revisar Marcación de Cobertura', 'BBVA_ESCRITURACION_REVISAR_MARCACION_COBERTURA', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'revisar_marcacion_cobertura', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REVISAR_MARCACION_COBERTURA');

-- Destino: Validar Condiciones Desembolso (ya pre-registrada por el script de BBV-94; INSERT idempotente por si acaso)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Validar Condiciones Desembolso', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'validar_condiciones_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO');
