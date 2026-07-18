export interface CalculoGeneracionDocumento {
  id_calculo_generacion_documento: number;
  id_expediente: number;
  tipo_propiedad: string | null;
  tipo_direccion: string | null;
  direccion: string | null;
  region: string | null;
  comuna: string | null;
  existe_rol_avaluo: boolean | null;
  rol_avaluo: string | null;
  valor_avaluo_pesos: number | null;
  revision_rol_propiedad: 'ROL_INCORRECTO' | 'ACEPTAR' | null;
  valor_uf_fecha_hoy: number | null;
  fecha_calculo: string | null;
  valor_uf_fecha_calculo: number | null;
  is_enviar_reparo: boolean;
  observaciones: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by: number | null;
  modified_date: string | null;
}
