export interface RegistroContactoBBVA {
  id?: number;
  id_expediente: number;
  id_actividad: string;
  id_usuario?: number;
  nro_contacto?: number | null;
  canal_contacto: string;
  resultado_contacto: string;
  detalle_contacto?: string | null;
  inmueble_definido?: boolean | null;
  observaciones?: string | null;
  fecha_contacto?: string;
}

export const EMPTY_REGISTRO_CONTACTO = (
  id_expediente: number,
  id_actividad: string,
): RegistroContactoBBVA => ({
  id_expediente,
  id_actividad,
  nro_contacto: null,
  canal_contacto: '',
  resultado_contacto: '',
  detalle_contacto: null,
  inmueble_definido: null,
  observaciones: '',
});
