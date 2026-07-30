-- BBV-86 - Firmar Escritura Cliente (Escrituración y Garantías)
-- Script idempotente para crear la tabla firmar_escritura_cliente

CREATE TABLE IF NOT EXISTS public.firmar_escritura_cliente (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100),
    notaria                         VARCHAR(150),
    fecha_notaria                   DATE,
    numero_notaria                  INTEGER,
    ciudad_notaria                  VARCHAR(100),
    numero_escritura                VARCHAR(20),
    fecha_escritura                 DATE,
    representante_legal             VARCHAR(200),
    requiere_escalamiento_comercial VARCHAR(2),
    tipologia                       VARCHAR(100),
    requiere_causar                 VARCHAR(2),
    tipo_credito                    VARCHAR(100),
    observaciones                   TEXT,
    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_fec_expediente
    ON public.firmar_escritura_cliente (id_expediente)
    WHERE is_active = true AND row_status = true;

-- cat_actividades_ws
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Firmar Escritura Cliente', 'BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'firmar_escritura_cliente', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F');
