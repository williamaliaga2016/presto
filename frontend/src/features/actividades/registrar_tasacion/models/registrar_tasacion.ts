import type { TasacionDetalle } from "./tasacion_detalle";

export type TipoTasacion = 1 | 0;

export interface Tasacion {
  id_tasacion: number;
  id_expediente: number;
  is_enviar_reparo: boolean;
  observaciones: string | null;

  detalles?: TasacionDetalle[] | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by: number | null;
  modified_date: string | null;
}

export interface EvaluarReparoAutomatico {
  aplica_reparo_automatico: boolean;
  mensaje: string | null;
  precio_venta_moneda_original: number | null;
  valor_tasacion_uf: number | null;
  prestamo_maximo: number | null;
  porcentaje_financiamiento: number | null;
  monto_calculado: number | null;
  glosa_producto: string | null;
  tipo_tasacion: boolean | null;
}
