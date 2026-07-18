-- BBV-48 - Catalogo aplicativo para Asignar Firmas, Peritos y Abogados.
--
-- Base destino: BBVA_LEGALIZACION.
--
-- Registra de forma idempotente ACT_ASIGNAR_FIRMAS en public.cat_actividades_ws
-- para que la bandeja pueda resolver la pantalla /home/asignar_firmas/{id_expediente}.
--
-- No ejecutar en DBMFBBVA: esa base usa los scripts de workflow xpdl_*.

DO $$
DECLARE
    v_workflow_process_id CONSTANT varchar := '_yYE04AsMEeaJXe4_X2YDKw';
    v_role_id integer;
BEGIN
    IF to_regclass('public.cat_actividades_ws') IS NULL THEN
        RAISE EXCEPTION 'La tabla public.cat_actividades_ws no existe. Verifique que este script se ejecute en BBVA_LEGALIZACION.';
    END IF;

    SELECT r.role_id
    INTO v_role_id
    FROM public."role" r
    WHERE r.name IN ('ANALISTA_VIVIENDA_II', 'ANALISTA_VIVIENDA', 'COORDINADOR', 'Coordinador')
       OR r.code IN ('ANALISTA_VIVIENDA_II', 'ANALISTA_VIVIENDA', 'COORDINADOR')
    ORDER BY CASE r.name
        WHEN 'ANALISTA_VIVIENDA_II' THEN 1
        WHEN 'ANALISTA_VIVIENDA' THEN 2
        WHEN 'COORDINADOR' THEN 3
        WHEN 'Coordinador' THEN 4
        ELSE 5
    END
    LIMIT 1;

    IF v_role_id IS NULL THEN
        RAISE EXCEPTION 'No existe rol ANALISTA_VIVIENDA_II, ANALISTA_VIVIENDA ni COORDINADOR. Verifique public."role" antes de registrar ACT_ASIGNAR_FIRMAS.';
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
        'Asignar Firmas, Peritos y Abogados',
        'ACT_ASIGNAR_FIRMAS',
        v_workflow_process_id,
        'Presto Legalizacion',
        v_role_id,
        'USUARIO',
        'asignar_firmas',
        'Firma',
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
        WHERE id_actividad = 'ACT_ASIGNAR_FIRMAS'
    );

    UPDATE public.cat_actividades_ws
    SET actividad = 'Asignar Firmas, Peritos y Abogados',
        id_proceso = v_workflow_process_id,
        proceso = 'Presto Legalizacion',
        id_role = v_role_id,
        tipo = 'USUARIO',
        page = 'asignar_firmas',
        etapa = 'Firma',
        is_active = TRUE,
        row_status = TRUE,
        modified_by = 'system',
        modified_date = NOW()
    WHERE id_actividad = 'ACT_ASIGNAR_FIRMAS';
END $$;

-- Validacion posterior sugerida:
--
-- SELECT id, id_actividad, actividad, id_role, page, etapa, is_active, row_status
-- FROM public.cat_actividades_ws
-- WHERE id_actividad = 'ACT_ASIGNAR_FIRMAS';
--
-- SELECT r.role_id, r.name
-- FROM public."role" r
-- INNER JOIN public.cat_actividades_ws caw ON caw.id_role = r.role_id
-- WHERE caw.id_actividad = 'ACT_ASIGNAR_FIRMAS';
