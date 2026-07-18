-- BBV-46 / BBV-49 - Workflow.
--
-- Base destino: DBMFBBVA.
--
-- Este script consolida las actividades y transiciones workflow requeridas por:
--   BBV-46 - Cargar Documentos Cliente
--   BBV-49 - Cargar Soportes de Pago
--
-- No ejecutar en BBVA_LEGALIZACION: esa base usa el script de objetos de
-- aplicacion y catalogo.
-- Es idempotente: se puede ejecutar mas de una vez sin duplicar registros.

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

    INSERT INTO public.xpdl_activities (
        activity_id,
        workflow_process_id,
        display_name,
        name,
        task_type,
        task_form_type,
        task_form_uri,
        performer,
        sub_flow_id
    )
    SELECT seed.activity_id,
           seed.workflow_process_id,
           seed.display_name,
           seed.name,
           seed.task_type,
           seed.task_form_type,
           seed.task_form_uri,
           seed.performer,
           seed.sub_flow_id
    FROM (
        VALUES
            (
                'ACT_VALIDAR_INFO',
                v_workflow_process_id,
                'Validar Informacion',
                'Validar Informacion',
                NULL,
                NULL,
                'validar_informacion',
                'ANALISTA_VIVIENDA',
                NULL
            ),
            (
                'ACT_DOCS_CLIENTE',
                v_workflow_process_id,
                'Cargar Documentos Cliente',
                'Cargar Documentos Cliente',
                NULL,
                NULL,
                'cargar_documentos_cliente',
                'CLIENTE',
                NULL
            ),
            (
                'ACT_REVISAR_DOCS',
                v_workflow_process_id,
                'Revisar Documentos Cliente',
                'Revisar Documentos Cliente',
                NULL,
                NULL,
                NULL,
                'ANALISTA_VIVIENDA',
                NULL
            ),
            (
                'ACT_SOPORTES_PAGO',
                v_workflow_process_id,
                'Soportes de Pago',
                'Soportes de Pago',
                'TaskUser',
                'UserDefined',
                'cargar_soportes_pago',
                '_KUDpMMpaEeyR_OjaR3r9lQ',
                NULL
            ),
            (
                'ACT_GESTIONAR_FIRMA',
                v_workflow_process_id,
                'Gestionar Firma',
                'Gestionar Firma',
                'TaskUser',
                'UserDefined',
                'gestionar_firma',
                '_KUDpMMpaEeyR_OjaR3r9lQ',
                NULL
            )
    ) AS seed (
        activity_id,
        workflow_process_id,
        display_name,
        name,
        task_type,
        task_form_type,
        task_form_uri,
        performer,
        sub_flow_id
    )
    WHERE NOT EXISTS (
        SELECT 1
        FROM public.xpdl_activities current_activity
        WHERE current_activity.activity_id = seed.activity_id
    );

    UPDATE public.xpdl_activities current_activity
    SET workflow_process_id = seed.workflow_process_id,
        display_name = seed.display_name,
        name = seed.name,
        task_type = seed.task_type,
        task_form_type = seed.task_form_type,
        task_form_uri = seed.task_form_uri,
        performer = seed.performer,
        sub_flow_id = seed.sub_flow_id
    FROM (
        VALUES
            ('ACT_VALIDAR_INFO', v_workflow_process_id, 'Validar Informacion', 'Validar Informacion', NULL, NULL, 'validar_informacion', 'ANALISTA_VIVIENDA', NULL),
            ('ACT_DOCS_CLIENTE', v_workflow_process_id, 'Cargar Documentos Cliente', 'Cargar Documentos Cliente', NULL, NULL, 'cargar_documentos_cliente', 'CLIENTE', NULL),
            ('ACT_REVISAR_DOCS', v_workflow_process_id, 'Revisar Documentos Cliente', 'Revisar Documentos Cliente', NULL, NULL, NULL, 'ANALISTA_VIVIENDA', NULL),
            ('ACT_SOPORTES_PAGO', v_workflow_process_id, 'Soportes de Pago', 'Soportes de Pago', 'TaskUser', 'UserDefined', 'cargar_soportes_pago', '_KUDpMMpaEeyR_OjaR3r9lQ', NULL),
            ('ACT_GESTIONAR_FIRMA', v_workflow_process_id, 'Gestionar Firma', 'Gestionar Firma', 'TaskUser', 'UserDefined', 'gestionar_firma', '_KUDpMMpaEeyR_OjaR3r9lQ', NULL)
    ) AS seed (
        activity_id,
        workflow_process_id,
        display_name,
        name,
        task_type,
        task_form_type,
        task_form_uri,
        performer,
        sub_flow_id
    )
    WHERE current_activity.activity_id = seed.activity_id;

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
                'ValidarInformacion_Avanzar',
                'ValidarInformacion_Avanzar',
                'ACT_VALIDAR_INFO',
                'ACT_DOCS_CLIENTE',
                'AVANZAR',
                v_workflow_process_id
            ),
            (
                'DocsCliente_Avanzar',
                'DocsCliente_Avanzar',
                'ACT_DOCS_CLIENTE',
                'ACT_REVISAR_DOCS',
                NULL,
                v_workflow_process_id
            ),
            (
                'SoportesPago_Avanzar',
                'SoportesPago_Avanzar',
                'ACT_SOPORTES_PAGO',
                'ACT_GESTIONAR_FIRMA',
                NULL,
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
            ('ValidarInformacion_Avanzar', 'ValidarInformacion_Avanzar', 'ACT_VALIDAR_INFO', 'ACT_DOCS_CLIENTE', 'AVANZAR', v_workflow_process_id),
            ('DocsCliente_Avanzar', 'DocsCliente_Avanzar', 'ACT_DOCS_CLIENTE', 'ACT_REVISAR_DOCS', NULL, v_workflow_process_id),
            ('SoportesPago_Avanzar', 'SoportesPago_Avanzar', 'ACT_SOPORTES_PAGO', 'ACT_GESTIONAR_FIRMA', NULL, v_workflow_process_id)
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
-- SELECT activity_id, display_name, task_form_uri, performer
-- FROM public.xpdl_activities
-- WHERE activity_id IN (
--     'ACT_VALIDAR_INFO',
--     'ACT_DOCS_CLIENTE',
--     'ACT_REVISAR_DOCS',
--     'ACT_SOPORTES_PAGO',
--     'ACT_GESTIONAR_FIRMA'
-- )
-- ORDER BY activity_id;
--
-- SELECT transition_id, name, from_activity, to_activity, condition
-- FROM public.xpdl_transitions
-- WHERE transition_id IN (
--     'ValidarInformacion_Avanzar',
--     'DocsCliente_Avanzar',
--     'SoportesPago_Avanzar'
-- )
-- ORDER BY transition_id;
