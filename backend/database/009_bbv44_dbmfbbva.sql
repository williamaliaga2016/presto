-- BBV-44 - Workflow Definir Inmueble.
--
-- Base destino: DBMFBBVA.
--
-- Reutiliza la actividad existente BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803
-- observada en el snapshot de DBMFBBVA y agrega solo la transicion tecnica
-- que falta para el backend actual:
--   ACT_VALIDAR_INFO -> BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803 por ValidarInformacion_SinInmueble
--
-- La salida desde Definir Inmueble ya existe en snapshot como:
--   BBVA_CONTACTO_TR_029 / name TR_029
--   BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803 -> BBVA_CONTACTO_GATEWAY_E6C26513
--
-- No ejecutar en BBVA_LEGALIZACION: esa base usa el script de catalogo aplicativo.

BEGIN;

DO $$
DECLARE
    v_workflow_process_id CONSTANT varchar := '_yYE04AsMEeaJXe4_X2YDKw';
BEGIN
    IF to_regclass('public.xpdl_activities') IS NULL THEN
        RAISE EXCEPTION 'La tabla public.xpdl_activities no existe. Verifique que este script se ejecute en DBMFBBVA.';
    END IF;

    IF to_regclass('public.xpdl_transitions') IS NULL THEN
        RAISE EXCEPTION 'La tabla public.xpdl_transitions no existe. Verifique que este script se ejecute en DBMFBBVA.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM public.xpdl_activities
        WHERE activity_id = 'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803'
    ) THEN
        RAISE EXCEPTION 'No existe la actividad BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803 en DBMFBBVA. Verifique el snapshot/base antes de aplicar BBV-44.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM public.xpdl_transitions
        WHERE transition_id = 'BBVA_CONTACTO_TR_029'
          AND from_activity = 'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803'
    ) THEN
        RAISE EXCEPTION 'No existe la transicion existente BBVA_CONTACTO_TR_029 desde Definir Inmueble. Verifique el workflow antes de implementar Avanzar.';
    END IF;

    INSERT INTO public.xpdl_transitions (
        transition_id,
        name,
        from_activity,
        to_activity,
        condition,
        workflow_process_id
    )
    SELECT seed.transition_id,
           seed.name,
           seed.from_activity,
           seed.to_activity,
           seed.condition,
           seed.workflow_process_id
    FROM (
        VALUES
            (
                'ValidarInformacion_SinInmueble',
                'ValidarInformacion_SinInmueble',
                'ACT_VALIDAR_INFO',
                'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803',
                'SIN_INM',
                v_workflow_process_id
            )
    ) AS seed (
        transition_id,
        name,
        from_activity,
        to_activity,
        condition,
        workflow_process_id
    )
    WHERE NOT EXISTS (
        SELECT 1
        FROM public.xpdl_transitions current_transition
        WHERE current_transition.transition_id = seed.transition_id
    );

    UPDATE public.xpdl_transitions current_transition
    SET name = seed.name,
        from_activity = seed.from_activity,
        to_activity = seed.to_activity,
        condition = seed.condition,
        workflow_process_id = seed.workflow_process_id
    FROM (
        VALUES
            ('ValidarInformacion_SinInmueble', 'ValidarInformacion_SinInmueble', 'ACT_VALIDAR_INFO', 'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803', 'SIN_INM', v_workflow_process_id)
    ) AS seed (
        transition_id,
        name,
        from_activity,
        to_activity,
        condition,
        workflow_process_id
    )
    WHERE current_transition.transition_id = seed.transition_id;
END $$;

COMMIT;

-- Validaciones sugeridas despues de ejecutar:
--
-- SELECT activity_id, workflow_process_id, display_name, task_form_uri, performer
-- FROM public.xpdl_activities
-- WHERE activity_id IN (
--     'ACT_VALIDAR_INFO',
--     'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803'
-- )
-- ORDER BY activity_id;
--
-- SELECT transition_id, name, from_activity, to_activity, condition
-- FROM public.xpdl_transitions
-- WHERE transition_id IN (
--     'ValidarInformacion_SinInmueble',
--     'BBVA_CONTACTO_TR_029'
-- )
-- ORDER BY transition_id;
