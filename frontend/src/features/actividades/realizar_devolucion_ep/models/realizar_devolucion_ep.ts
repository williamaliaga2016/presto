import type { Auditoria } from '@/models/Auditoria';

export interface RealizarDevolucionEP extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  requiere_escalamiento_comercial: string | null;
  tipologia: string | null;
  casuistica: string | null;
  accion_a_seguir: string | null;
  observaciones: string | null;
}

export interface DictamenPrevio {
  area: string;
  tipologia: string | null;
  casuistica: string | null;
  observaciones: string | null;
}

export interface GetByExpedienteResponse {
  formulario: RealizarDevolucionEP;
  dictamenes_previos: DictamenPrevio[];
}

export const EMPTY_REALIZAR_DEVOLUCION_EP = (id_expediente: number): RealizarDevolucionEP => ({
  id: 0, id_expediente, id_actividad: 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP',
  requiere_escalamiento_comercial: 'NO', tipologia: null, casuistica: null, accion_a_seguir: null, observaciones: null,
  is_active: true, row_status: true, created_by: 0, created_date: '', modified_by: null, modified_date: null,
});

export const ACCIONES_A_SEGUIR = [
  { code: 'FIRMAR_ESCRITURA', description: 'Firmar Escritura Cliente' },
  { code: 'FIRMAR_REP_LEGAL', description: 'Firmar Rep. Legal' },
  { code: 'EP_REGISTRADAS', description: 'Realizar EP Registradas' },
];
