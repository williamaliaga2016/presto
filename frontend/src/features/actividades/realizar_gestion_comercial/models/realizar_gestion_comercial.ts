import type { Auditoria } from '@/models/Auditoria';

export interface RealizarGestionComercial extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  origen_escalamiento: string | null;
  cliente_desiste: string | null;
  observaciones: string | null;
}

export interface GetByExpedienteResponse {
  formulario: RealizarGestionComercial;
  origen_escalamiento: string;
  datos_heredados: any;
}

export const EMPTY_REALIZAR_GESTION_COMERCIAL = (id_expediente: number): RealizarGestionComercial => ({
  id: 0, id_expediente, id_actividad: 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL',
  origen_escalamiento: null, cliente_desiste: null, observaciones: null,
  is_active: true, row_status: true, created_by: 0, created_date: '', modified_by: null, modified_date: null,
});
