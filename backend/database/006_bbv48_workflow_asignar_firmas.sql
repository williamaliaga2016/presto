-- BBV-48 - Workflow "Asignar Firmas, Peritos y Abogados"
-- Base de datos destino: dbWFMultibanca
--
-- Este script registra las actividades y transiciones XPDL requeridas por:
--   ACT_ASIGNAR_FIRMAS
--   ACT_SOPORTES_PAGO
--   ACT_GESTIONAR_FIRMA
--
-- Es idempotente: se puede ejecutar mas de una vez sin duplicar registros.

BEGIN;

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
SELECT
    'ACT_ASIGNAR_FIRMAS',
    '_yYE04AsMEeaJXe4_X2YDKw',
    'Asignar Firmas, Peritos y Abogados',
    'Asignar Firmas, Peritos y Abogados',
    'TaskUser',
    'UserDefined',
    'asignar_firmas',
    '_KUDpMMpaEeyR_OjaR3r9lQ',
    NULL
WHERE NOT EXISTS (
    SELECT 1
    FROM public.xpdl_activities
    WHERE activity_id = 'ACT_ASIGNAR_FIRMAS'
);

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
SELECT
    'ACT_GESTIONAR_FIRMA',
    '_yYE04AsMEeaJXe4_X2YDKw',
    'Gestionar Firma',
    'Gestionar Firma',
    'TaskUser',
    'UserDefined',
    'gestionar_firma',
    '_KUDpMMpaEeyR_OjaR3r9lQ',
    NULL
WHERE NOT EXISTS (
    SELECT 1
    FROM public.xpdl_activities
    WHERE activity_id = 'ACT_GESTIONAR_FIRMA'
);

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
SELECT
    'ACT_SOPORTES_PAGO',
    '_yYE04AsMEeaJXe4_X2YDKw',
    'Soportes de Pago',
    'Soportes de Pago',
    'TaskUser',
    'UserDefined',
    'cargar_soportes_pago',
    '_KUDpMMpaEeyR_OjaR3r9lQ',
    NULL
WHERE NOT EXISTS (
    SELECT 1
    FROM public.xpdl_activities
    WHERE activity_id = 'ACT_SOPORTES_PAGO'
);

-- Nodo interno para que el motor cree en paralelo ACT_SOPORTES_PAGO y ACT_GESTIONAR_FIRMA.
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
SELECT
    'PAR_ASIGNAR_FIRMAS_NOTIFICACION',
    '_yYE04AsMEeaJXe4_X2YDKw',
    'Paralelo Asignar Firmas con Notificacion',
    'Paralelo Asignar Firmas con Notificacion',
    'Parallel',
    NULL,
    NULL,
    NULL,
    NULL
WHERE NOT EXISTS (
    SELECT 1
    FROM public.xpdl_activities
    WHERE activity_id = 'PAR_ASIGNAR_FIRMAS_NOTIFICACION'
);

-- Camino B: requiere_envio_notificacion = false.
INSERT INTO public.xpdl_transitions (
    transition_id,
    name,
    from_activity,
    to_activity,
    condition,
    workflow_process_id
)
SELECT
    'TR_ASIGNAR_FIRMAS_SIN_NOTIFICACION',
    'AsignarFirmas_SinNotificacion',
    'ACT_ASIGNAR_FIRMAS',
    'ACT_GESTIONAR_FIRMA',
    'Otherwise',
    NULL
WHERE NOT EXISTS (
    SELECT 1
    FROM public.xpdl_transitions
    WHERE transition_id = 'TR_ASIGNAR_FIRMAS_SIN_NOTIFICACION'
);

-- Camino A: requiere_envio_notificacion = true.
-- La transicion que consume el backend llega a un nodo Parallel.
INSERT INTO public.xpdl_transitions (
    transition_id,
    name,
    from_activity,
    to_activity,
    condition,
    workflow_process_id
)
SELECT
    'TR_ASIGNAR_FIRMAS_CON_NOTIFICACION',
    'AsignarFirmas_ConNotificacion',
    'ACT_ASIGNAR_FIRMAS',
    'PAR_ASIGNAR_FIRMAS_NOTIFICACION',
    'Otherwise',
    NULL
WHERE NOT EXISTS (
    SELECT 1
    FROM public.xpdl_transitions
    WHERE transition_id = 'TR_ASIGNAR_FIRMAS_CON_NOTIFICACION'
);

-- Salidas paralelas del nodo interno.
INSERT INTO public.xpdl_transitions (
    transition_id,
    name,
    from_activity,
    to_activity,
    condition,
    workflow_process_id
)
SELECT
    'TR_PAR_ASIGNAR_FIRMAS_SOPORTES_PAGO',
    'ParaleloAsignarFirmas_SoportesPago',
    'PAR_ASIGNAR_FIRMAS_NOTIFICACION',
    'ACT_SOPORTES_PAGO',
    'Otherwise',
    NULL
WHERE NOT EXISTS (
    SELECT 1
    FROM public.xpdl_transitions
    WHERE transition_id = 'TR_PAR_ASIGNAR_FIRMAS_SOPORTES_PAGO'
);

INSERT INTO public.xpdl_transitions (
    transition_id,
    name,
    from_activity,
    to_activity,
    condition,
    workflow_process_id
)
SELECT
    'TR_PAR_ASIGNAR_FIRMAS_GESTIONAR_FIRMA',
    'ParaleloAsignarFirmas_GestionarFirma',
    'PAR_ASIGNAR_FIRMAS_NOTIFICACION',
    'ACT_GESTIONAR_FIRMA',
    'Otherwise',
    NULL
WHERE NOT EXISTS (
    SELECT 1
    FROM public.xpdl_transitions
    WHERE transition_id = 'TR_PAR_ASIGNAR_FIRMAS_GESTIONAR_FIRMA'
);

COMMIT;

-- Validacion sugerida despues de ejecutar:
--
-- SELECT *
-- FROM public.xpdl_activities
-- WHERE activity_id IN (
--     'ACT_ASIGNAR_FIRMAS',
--     'PAR_ASIGNAR_FIRMAS_NOTIFICACION',
--     'ACT_SOPORTES_PAGO',
--     'ACT_GESTIONAR_FIRMA'
-- )
-- ORDER BY activity_id;
--
-- SELECT *
-- FROM public.xpdl_transitions
-- WHERE transition_id IN (
--     'TR_ASIGNAR_FIRMAS_SIN_NOTIFICACION',
--     'TR_ASIGNAR_FIRMAS_CON_NOTIFICACION',
--     'TR_PAR_ASIGNAR_FIRMAS_SOPORTES_PAGO',
--     'TR_PAR_ASIGNAR_FIRMAS_GESTIONAR_FIRMA'
-- )
-- ORDER BY transition_id;
