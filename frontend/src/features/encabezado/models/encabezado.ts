export interface EncabezadoDTO {
  id_expediente: number;
  actividad: string;
  estado: string;
  usuario_asignado: string;
  fecha_alta?: string | null;
  fecha_asignacion?: string | null;

  // ============================================================
  // CAMPOS BBVA COLOMBIA
  // ============================================================

  // Identificación
  id_scoring?: string | null;

  // Titular T1
  tipo_documento_id_t1?: string | null;
  numero_identificacion_t1?: string | null;
  nombre_completo_t1?: string | null;
  celular_t1?: string | null;

  // Titular T2
  numero_identificacion_t2?: string | null;
  nombre_completo_t2?: string | null;

  // Datos laborales
  cliente_nomina?: boolean | null;
  situacion_laboral?: string | null;
  correo_declarativo?: string | null;
  telefono_declarativo?: string | null;

  // Oficina y asesor BBVA
  codigo_oficina_bbva?: string | null;
  descripcion_oficina_bbva?: string | null;
  codigo_asesor_bbva?: string | null;

  // Crédito
  id_tipo_sub_producto?: string | null;
  monto_otorgado?: number | null;
  canal_originacion?: string | null;
  fecha_aprobacion?: string | null;

  // Inmueble
  codigo_proyecto?: string | null;
  descripcion_proyecto?: string | null;
  estado_inmueble?: string | null;
  tipo_inmueble?: string | null;
  condiciones_organismo_decisor?: string | null;
}
