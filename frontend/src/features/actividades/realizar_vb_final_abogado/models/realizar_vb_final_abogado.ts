import type { Auditoria } from '@/models/Auditoria';

export interface RealizarVBFinalAbogado extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  requiere_devolucion: string | null;
  tipologia: string | null;
  casuistica: string | null;
  origen_tramite: string | null;
  bandera_excepcion: boolean;
  observaciones: string | null;
}

export interface GetByExpedienteResponse {
  formulario: RealizarVBFinalAbogado;
  origen_tramite: string;
  bandera_excepcion: boolean;
}

export const EMPTY_REALIZAR_VB_FINAL_ABOGADO = (id_expediente: number): RealizarVBFinalAbogado => ({
  id: 0, id_expediente, id_actividad: 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO',
  requiere_devolucion: 'NO', tipologia: null, casuistica: null,
  origen_tramite: null, bandera_excepcion: false, observaciones: null,
  is_active: true, row_status: true, created_by: 0, created_date: '', modified_by: null, modified_date: null,
});
