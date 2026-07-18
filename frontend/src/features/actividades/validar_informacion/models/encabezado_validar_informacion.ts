import type { ValidarInformacionBBVA } from './validar_informacion';

export interface EncabezadoValidarInformacion {
  scoring?: string | null;
  id_tipo_sub_producto?: string | null;
  monto_otorgado_original?: number | null;
  plazo_meses?: number | null;
  tasa?: number | null;
  condiciones_organismo_decisor?: string | null;
  codigo_oficina?: string | null;
  descripcion_oficina?: string | null;
  codigo_asesor?: string | null;
  correo_declarativo_original?: string | null;
  telefono_declarativo_original?: string | null;
}

export interface ValidarInformacionConEncabezadoDTO {
  formulario: ValidarInformacionBBVA;
  encabezado: EncabezadoValidarInformacion;
}

export const EMPTY_ENCABEZADO_VALIDAR_INFORMACION: EncabezadoValidarInformacion = {
  scoring: null,
  id_tipo_sub_producto: null,
  monto_otorgado_original: null,
  plazo_meses: null,
  tasa: null,
  condiciones_organismo_decisor: null,
  codigo_oficina: null,
  descripcion_oficina: null,
  codigo_asesor: null,
  correo_declarativo_original: null,
  telefono_declarativo_original: null,
};
