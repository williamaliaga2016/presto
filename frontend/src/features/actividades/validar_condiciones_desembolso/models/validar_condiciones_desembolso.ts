import type { Auditoria } from '@/models/Auditoria';

export interface ValidarCondicionesDesembolso extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  origen_tramite: string | null;
  confirmar_plan_pagos: boolean;
  requiere_escalamiento_comercial: string | null;
  suspendida: boolean;
  conteo_caidas: number;
  observaciones: string | null;
}

export interface GetByExpedienteResponse {
  formulario: ValidarCondicionesDesembolso;
  puede_suspender: boolean;
  origen_tramite: string;
  conteo_caidas: number;
}

export const EMPTY_VALIDAR_CONDICIONES_DESEMBOLSO = (id_expediente: number): ValidarCondicionesDesembolso => ({
  id: 0, id_expediente, id_actividad: 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO',
  origen_tramite: null, confirmar_plan_pagos: false, requiere_escalamiento_comercial: null,
  suspendida: false, conteo_caidas: 0, observaciones: null,
  is_active: true, row_status: true, created_by: 0, created_date: '', modified_by: null, modified_date: null,
});
