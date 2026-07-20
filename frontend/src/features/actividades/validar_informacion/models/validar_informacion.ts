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

  // Titulares adicionales (4-10)
  titulares_adicionales?: TitularBBVA[] | null;

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
  fecha_estimada_entrega?: string | null;

  // Crédito
  tipo_credito?: string | null;
  tiene_garantia?: boolean | null;
  garantia_constituida?: boolean | null;
  monto_otorgado_vi?: number | null;
  monto_otorgado_vivienda_original?: number | null;

  // Datos Comerciales
  correo_declarativo?: string | null;
  telefono_declarativo?: string | null;
  codigo_oficina?: string | null;
  descripcion_oficina?: string | null;
  codigo_asesor?: string | null;

  // Estatus y flags
  estatus_general?: string | null;
  motivo_devolucion?: string | null;
  origen_devolucion?: string | null;
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

export interface TitularBBVA {
  id?: number;
  id_expediente: number;
  id_actividad?: string | null;
  numero_titular?: number;
  tipo_identificacion?: string | null;
  numero_identificacion?: string | null;
  nombre_completo?: string | null;
  celular_cliente?: string | null;
  telefono_residente?: string | null;
  email?: string | null;
  direccion_residencia?: string | null;
  telefono_declarativo?: string | null;
  correo_declarativo?: string | null;
}

export const EMPTY_VALIDAR_INFORMACION = (
  id_expediente: number,
): ValidarInformacionBBVA => ({
  id_expediente,
  id_actividad: 'ACT_VALIDAR_INFO',
  inmueble_definido: false,
  estatus_general: 'SIN_INMUEBLE_DEFINIDO',
  requiere_definir_inmueble: false,
  requiere_carga_cliente: false,
  requiere_carga_constructora: false,
});
