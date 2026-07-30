-- BBV-91 — Configuración del nodo Parallel para "Firmada Conforme"
-- Ejecutar en la BD del workflow (DBWFBBVA)
--
-- Flujo resultante:
-- Firmar Rep. Legal
--   ├─ (NO_FIRMA)       → Devolución EP
--   └─ (FIRMA_CONFORME) → [Parallel Node] → Entrega EP Firmada
--                                          → Preformalizar
--
-- Excepción Desembolso se crea desde código (condicional, no pasa por workflow)

-- ============================================================
-- 1. Crear actividad de tipo "Parallel" (nodo de bifurcación)
-- ============================================================
INSERT INTO xpdl_activities (activity_id, workflow_process_id, display_name, name, task_type, task_form_type, task_form_uri, performer, sub_flow_id)
SELECT 'BBVA_ESCRITURACION_PARALLEL_FIRMADA_CONFORME', 'WP_BBVA_CONTACTO_CLIENTE', 'Bifurcación Firmada Conforme', 'Bifurcación Firmada Conforme', 'Parallel', '', '', '', ''
WHERE NOT EXISTS (SELECT 1 FROM xpdl_activities WHERE activity_id = 'BBVA_ESCRITURACION_PARALLEL_FIRMADA_CONFORME');

-- ============================================================
-- 2. Modificar transición ENTREGA_EP para que apunte al Parallel
-- ============================================================
UPDATE xpdl_transitions
SET to_activity = 'BBVA_ESCRITURACION_PARALLEL_FIRMADA_CONFORME'
WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP';

-- ============================================================
-- 3. Crear transiciones desde el Parallel hacia los 2 destinos
-- ============================================================
INSERT INTO xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'TR_PARALLEL_FIRMADA_ENTREGA_EP', 'TR_PARALLEL_FIRMADA_ENTREGA_EP',
       'BBVA_ESCRITURACION_PARALLEL_FIRMADA_CONFORME', 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM xpdl_transitions WHERE transition_id = 'TR_PARALLEL_FIRMADA_ENTREGA_EP');

INSERT INTO xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'TR_PARALLEL_FIRMADA_PREFORMALIZAR', 'TR_PARALLEL_FIRMADA_PREFORMALIZAR',
       'BBVA_ESCRITURACION_PARALLEL_FIRMADA_CONFORME', 'BBVA_ESCRITURACION_PREFORMALIZAR', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM xpdl_transitions WHERE transition_id = 'TR_PARALLEL_FIRMADA_PREFORMALIZAR');

-- ============================================================
-- 4. Eliminar transiciones directas que ya no se usan
-- ============================================================
DELETE FROM xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_PREFORMALIZAR';
DELETE FROM xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_EXCEPCION_DESEMBOLSO';
