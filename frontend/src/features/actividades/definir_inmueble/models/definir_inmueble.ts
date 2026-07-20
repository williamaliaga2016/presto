import type { ValidarInformacionBBVA } from
  '../../validar_informacion/models/validar_informacion';

export const DEFINIR_INMUEBLE_ACTIVITY_ID =
  'BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803';

export interface DefinirInmuebleBBVA {
  id?: number;
  id_expediente: number;
  id_actividad: string;
  cliente_cuenta_inmueble_definido?: boolean | null;
  constructora?: string | null;
  fecha_estimada_entrega?: string | null;
  estatus_general?: string | null;
  motivo_devolucion?: string | null;
  observaciones?: string | null;
  is_active?: boolean;
  row_status?: boolean;
  created_by?: number;
  created_date?: string;
  modified_by?: number | null;
  modified_date?: string | null;
}


export interface DefinirInmuebleAvanzarRequest {
  confirmar: boolean;
}

export interface DefinirInmuebleAvanzarResponse {
  requiere_confirmacion: boolean;
  mensaje?: string | null;
  workflow?: unknown;
}

export const EMPTY_DEFINIR_INMUEBLE = (
  id_expediente: number,
): DefinirInmuebleBBVA => ({
  id_expediente,
  id_actividad: DEFINIR_INMUEBLE_ACTIVITY_ID,
  cliente_cuenta_inmueble_definido: false,
  estatus_general: 'SIN_INM',
  constructora: '',
  fecha_estimada_entrega: null,
  motivo_devolucion: '',
  observaciones: '',
});
