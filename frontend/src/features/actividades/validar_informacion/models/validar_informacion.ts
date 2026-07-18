export interface ValidarInformacionBBVA {
  id?: number;
  id_expediente: number;
  id_actividad: string;

  // Titular T1
  tipo_id_t1?: string | null;
  numero_id_t1?: string | null;
  nombre_completo_t1?: string | null;
  celular_t1?: string | null;
  telefono_t1?: string | null;
  email_t1?: string | null;
  direccion_t1?: string | null;
  departamento_t1?: string | null;
  municipio_t1?: string | null;
  situacion_laboral_t1?: string | null;
  cliente_nomina_t1?: boolean | null;

  // Titular T2
  tipo_id_t2?: string | null;
  numero_id_t2?: string | null;
  nombre_completo_t2?: string | null;
  celular_t2?: string | null;
  email_t2?: string | null;

  // Titular T3
  tipo_id_t3?: string | null;
  numero_id_t3?: string | null;
  nombre_completo_t3?: string | null;
  celular_t3?: string | null;
  email_t3?: string | null;

  // Inmueble
  inmueble_definido?: boolean | null;
  tipo_inmueble?: string | null;
  estado_inmueble?: string | null;
  constructora?: string | null;
  es_constructora_vip?: boolean | null;
  codigo_proyecto?: string | null;
  descripcion_proyecto?: string | null;
  departamento_inmueble?: string | null;
  municipio_inmueble?: string | null;

  // Crédito
  tipo_credito?: string | null;
  tiene_garantia?: boolean | null;
  monto_otorgado_vi?: number | null;

  // Datos Comerciales
  correo_declarativo?: string | null;
  telefono_declarativo?: string | null;

  // Estatus y flags
  estatus_general?: string | null;
  motivo_devolucion?: string | null;
  observaciones?: string | null;
  requiere_definir_inmueble?: boolean | null;
  requiere_carga_cliente?: boolean | null;
  requiere_carga_constructora?: boolean | null;

  // Auditoría
  is_active?: boolean;
  row_status?: boolean;
  created_by?: number;
  created_date?: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RegistroContactoBBVA {
  id?: number;
  id_expediente: number;
  id_actividad: string;
  id_usuario?: number;
  canal_contacto: string;
  resultado_contacto: string;
  observaciones?: string | null;
  fecha_contacto?: string;
}

export const EMPTY_VALIDAR_INFORMACION = (
  id_expediente: number,
): ValidarInformacionBBVA => ({
  id_expediente,
  id_actividad: 'ACT_VALIDAR_INFO',
  inmueble_definido: false,
  estatus_general: 'SIN_INM',
  requiere_definir_inmueble: false,
  requiere_carga_cliente: false,
  requiere_carga_constructora: false,
});
