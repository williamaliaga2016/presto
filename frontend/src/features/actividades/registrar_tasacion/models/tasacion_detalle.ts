import type { TipoTasacion } from "./registrar_tasacion";

export interface TasacionDetalle {
  id_tasacion_detalle: number;
  id_tasacion: number;
  id_expediente: number;
  tipo_tasacion: boolean;
  nro_tasacion_p1: string | null;
  nro_tasacion_p2: string | null;
  nro_tasacion_p3: string | null;
  superficie_edificada: string | null;
  superficie_terreno: string | null;
  fecha_informe_tasacion: string | null;
  fecha_solicitud_tasacion: string | null;
  fecha_recepcion_tasacion: string | null;
  valor_tasacion_uf: number | null;
  valor_tasacion_pesos: number | null;
  valor_liquidacion_uf: number | null;
  valor_liquidacion_pesos: number | null;
  monto_asegurable_uf: number | null;
  monto_asegurable_pesos: number | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by: number | null;
  modified_date: string | null;
}
