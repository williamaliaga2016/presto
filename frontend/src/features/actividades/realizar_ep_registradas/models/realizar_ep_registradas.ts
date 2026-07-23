import type { Auditoria } from '@/models/Auditoria';

export interface RealizarEPRegistradas extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  finalizacion: string | null;
  causal: string | null;
  fecha_finalizacion: string | null;
  confirmacion_ep_registrada: boolean;
  observaciones: string | null;
}

export interface DatosHeredadosEPRegistradas {
  tipo_documento?: string | null;
  numero_documento?: string | null;
  nombre_completo?: string | null;
  tipo_credito?: string | null;
  ciudad_notaria?: string | null;
  numero_notaria?: number | null;
  numero_escritura?: string | null;
  numero_boleta?: string | null;
  fecha_boleta?: string | null;
  tipo_boleta?: string | null;
  oficina_registro?: string | null;
  numero_matricula?: string | null;
}

export interface GetByExpedienteResponse {
  formulario: RealizarEPRegistradas;
  datos_heredados: DatosHeredadosEPRegistradas;
}

export const EMPTY_REALIZAR_EP_REGISTRADAS = (id_expediente: number): RealizarEPRegistradas => ({
  id: 0, id_expediente, id_actividad: 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS',
  finalizacion: null, causal: null, fecha_finalizacion: null,
  confirmacion_ep_registrada: false, observaciones: null,
  is_active: true, row_status: true, created_by: 0, created_date: '', modified_by: null, modified_date: null,
});
