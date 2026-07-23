import type { Auditoria } from '@/models/Auditoria';

export interface RealizarRecepcionBoleta extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  numero_boleta: string | null;
  fecha_boleta: string | null;
  numero_matricula: string | null;
  tipo_boleta: string | null;
  boleta_en_poder_de: string | null;
  codigo_zona: string | null;
  oficina_registro: string | null;
  boleta_recibida: boolean;
  aplica_excepcion: string | null;
  vur_ejecutado: boolean;
  vur_exitoso: boolean;
  vur_intentos: number;
  observaciones: string | null;
}

export interface DatosHeredadosRecepcionBoleta {
  tipo_documento?: string | null;
  numero_documento?: string | null;
  nombre_completo?: string | null;
  tipo_credito?: string | null;
  ciudad_notaria?: string | null;
  numero_notaria?: number | null;
  numero_escritura?: string | null;
}

export interface GetByExpedienteResponse {
  formulario: RealizarRecepcionBoleta;
  datos_heredados: DatosHeredadosRecepcionBoleta;
}

export const EMPTY_REALIZAR_RECEPCION_BOLETA = (
  id_expediente: number,
): RealizarRecepcionBoleta => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA',
  numero_boleta: null,
  fecha_boleta: null,
  numero_matricula: null,
  tipo_boleta: null,
  boleta_en_poder_de: null,
  codigo_zona: null,
  oficina_registro: null,
  boleta_recibida: false,
  aplica_excepcion: null,
  vur_ejecutado: false,
  vur_exitoso: false,
  vur_intentos: 0,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
