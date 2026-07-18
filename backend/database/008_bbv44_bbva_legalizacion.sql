-- BBV-44 - Catalogo aplicativo para Definir Inmueble.
--
-- Base destino: BBVA_LEGALIZACION.
--
-- Registra de forma idempotente BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803 en public.cat_actividades_ws
-- para que la bandeja pueda resolver la pantalla /home/definir_inmueble/{id_expediente}.
--
-- No ejecutar en DBMFBBVA: esa base usa el script de workflow xpdl_*.

CREATE TABLE IF NOT EXISTS public.definir_inmueble_bbva (
    id BIGSERIAL PRIMARY KEY,
    id_expediente BIGINT NOT NULL,
    id_actividad VARCHAR(50) NOT NULL DEFAULT 'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803',
    cliente_cuenta_inmueble_definido BOOLEAN,
    inmueble_definido BOOLEAN,
    tipo_inmueble VARCHAR(50),
    estado_inmueble VARCHAR(50),
    constructora VARCHAR(200),
    es_constructora_vip BOOLEAN,
    codigo_proyecto VARCHAR(50),
    descripcion_proyecto VARCHAR(300),
    departamento_inmueble VARCHAR(50),
    municipio_inmueble VARCHAR(50),
    fecha_estimada_entrega DATE,
    estatus_general VARCHAR(100),
    motivo_devolucion VARCHAR(200),
    observaciones VARCHAR(500),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    row_status BOOLEAN NOT NULL DEFAULT TRUE,
    created_by INTEGER,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by INTEGER,
    modified_date TIMESTAMP WITHOUT TIME ZONE
);

ALTER TABLE public.definir_inmueble_bbva
    ADD COLUMN IF NOT EXISTS inmueble_definido BOOLEAN,
    ADD COLUMN IF NOT EXISTS tipo_inmueble VARCHAR(50),
    ADD COLUMN IF NOT EXISTS estado_inmueble VARCHAR(50),
    ADD COLUMN IF NOT EXISTS es_constructora_vip BOOLEAN,
    ADD COLUMN IF NOT EXISTS codigo_proyecto VARCHAR(50),
    ADD COLUMN IF NOT EXISTS descripcion_proyecto VARCHAR(300),
    ADD COLUMN IF NOT EXISTS departamento_inmueble VARCHAR(50),
    ADD COLUMN IF NOT EXISTS municipio_inmueble VARCHAR(50);

CREATE INDEX IF NOT EXISTS "IX_definir_inmueble_expediente"
    ON public.definir_inmueble_bbva (id_expediente);

DO $$
DECLARE
    v_workflow_process_id CONSTANT varchar := 'WP_BBVA_CONTACTO_CLIENTE';
    v_role_id integer;
BEGIN
    IF to_regclass('public.cat_actividades_ws') IS NULL THEN
        RAISE EXCEPTION 'La tabla public.cat_actividades_ws no existe. Verifique que este script se ejecute en BBVA_LEGALIZACION.';
    END IF;

    SELECT r.role_id
    INTO v_role_id
    FROM public."role" r
    WHERE r.name IN ('ANALISTA_VIVIENDA', 'ANALISTA_VIVIENDA_II', 'COORDINADOR', 'Coordinador')
       OR r.code IN ('ANALISTA_VIVIENDA', 'ANALISTA_VIVIENDA_II', 'COORDINADOR')
    ORDER BY CASE r.name
        WHEN 'ANALISTA_VIVIENDA' THEN 1
        WHEN 'ANALISTA_VIVIENDA_II' THEN 2
        WHEN 'COORDINADOR' THEN 3
        WHEN 'Coordinador' THEN 4
        ELSE 5
    END
    LIMIT 1;

    IF v_role_id IS NULL THEN
        RAISE EXCEPTION 'No existe rol ANALISTA_VIVIENDA, ANALISTA_VIVIENDA_II ni COORDINADOR. Verifique public."role" antes de registrar Definir Inmueble.';
    END IF;

    INSERT INTO public.cat_actividades_ws (
        actividad,
        id_actividad,
        id_proceso,
        proceso,
        id_role,
        tipo,
        page,
        etapa,
        tiempo_promedio,
        is_active,
        row_status,
        created_by,
        created_date,
        modified_by,
        modified_date
    )
    SELECT
        'Definir Inmueble',
        'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803',
        v_workflow_process_id,
        'Presto Legalizacion',
        v_role_id,
        'USUARIO',
        'definir_inmueble',
        'Contacto Cliente',
        NULL,
        TRUE,
        TRUE,
        'system',
        NOW(),
        NULL,
        NULL
    WHERE NOT EXISTS (
        SELECT 1
        FROM public.cat_actividades_ws
        WHERE id_actividad = 'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803'
    );

    UPDATE public.cat_actividades_ws
    SET actividad = 'Definir Inmueble',
        id_proceso = v_workflow_process_id,
        proceso = 'Presto Legalizacion',
        id_role = v_role_id,
        tipo = 'USUARIO',
        page = 'definir_inmueble',
        etapa = 'Contacto Cliente',
        is_active = TRUE,
        row_status = TRUE,
        modified_by = 'system',
        modified_date = NOW()
    WHERE id_actividad = 'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803';
END $$;

-- Validacion posterior sugerida:
--
-- SELECT id, id_actividad, actividad, id_role, page, etapa, is_active, row_status
-- FROM public.cat_actividades_ws
-- WHERE id_actividad = 'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803';
--
-- SELECT r.role_id, r.name
-- FROM public."role" r
-- INNER JOIN public.cat_actividades_ws caw ON caw.id_role = r.role_id
-- WHERE caw.id_actividad = 'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803';
