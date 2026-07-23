import type { Auditoria } from '@/models/Auditoria';

export interface RealizarEntregaEpFirmada extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  entregado_a: string | null;
  aplica_excepcion: string | null;  // "SI" | "NO" — readonly, calculado en backend
  observaciones: string | null;
}

export interface DatosHeredadosEntregaEp {
  concepto_firma?: string | null;
  concepto_firma_descripcion?: string | null;
  notaria?: string | null;
  numero_notaria?: number | null;
  ciudad_notaria?: string | null;
  fecha_notaria?: string | null;
  numero_escritura?: string | null;
  fecha_escritura?: string | null;
  representante_legal?: string | null;
}

export interface GetByExpedienteResponse {
  formulario: RealizarEntregaEpFirmada;
  datos_heredados: DatosHeredadosEntregaEp;
}

export const EMPTY_REALIZAR_ENTREGA_EP_FIRMADA = (
  id_expediente: number,
): RealizarEntregaEpFirmada => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA',
  entregado_a: null,
  aplica_excepcion: null,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
