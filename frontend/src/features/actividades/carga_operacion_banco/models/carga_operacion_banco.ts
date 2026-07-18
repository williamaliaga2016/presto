export interface CargaOperacionBancoDatosOperacion {
  id_carga_operacion_banco_datos_operacion: number;
  id_carga_operacion_banco: number;
  id_expediente: number;

  id_scoring?: string | null;
  codigo_asesor?: string | null;
  codigo_oficina?: string | null;
  descripcion_oficina?: string | null;
  canal_originacion?: string | null;
  tipo_inmueble?: string | null;
  estado_inmueble?: string | null;
  // NUEVO (BBV-76): aún no confirmado en backend, se deja comentado al armar el payload de envío.
  descripcion_estado_inmueble?: string | null;
  codigo_proyecto?: string | null;
  descripcion_proyecto?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface CargaOperacionBancoAntecedenteComprador {
  id_carga_operacion_banco_antecedente_comprador: number;
  id_carga_operacion_banco: number;
  id_expediente: number;

  /**
   * Campos compartidos con ValidacionRectificatoriaLegal(Postventa) —
   * ambas features importan este tipo y leen estos campos. No eliminar.
   */
  rut?: string | null;
  nombres?: string | null;
  apellido_paterno?: string | null;
  apellido_materno?: string | null;
  relacion_titular?: string | null;
  estado_civil?: string | null;
  nacionalidad?: string | null;

  direccion?: string | null;
  telefono?: string | null;
  email?: string | null;

  // BBVA Colombia
  numero_identificacion?: string | null;
  tipo_documento_id?: string | null;
  nombre_completo?: string | null;
  celular?: string | null;
  situacion_laboral?: string | null;
  cliente_nomina?: boolean | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

/**
 * NOTA: Este tipo ya no se usa en ningún formulario de carga_operacion_banco
 * (BBV-76 no contempla un rol de "vendedor" en la radicación de crédito).
 * Se mantiene únicamente porque ValidacionRectificatoriaLegal(Postventa) lo
 * importa directamente para su propia grilla de comparecientes. No eliminar
 * sin antes desacoplar esas dos features.
 */
export interface CargaOperacionBancoAntecedenteVendedor {
  id_carga_operacion_banco_antecedente_vendedor: number;
  id_carga_operacion_banco: number;
  id_expediente: number;

  rut?: string | null;
  nombres?: string | null;
  apellido_paterno?: string | null;
  apellido_materno?: string | null;
  relacion_titular?: string | null;
  estado_civil?: string | null;
  nacionalidad?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface CargaOperacionBancoAntecedenteCredito {
  id_carga_operacion_banco_antecedente_credito: number;
  id_carga_operacion_banco: number;
  id_expediente: number;

  /**
   * Usado por Registrar Tasación (useFactorConversionUF). No eliminar.
   */
  factor_conversion_uf?: number | null;

  tasa?: number | null;
  plazo?: number | null;

  // BBVA Colombia
  id_tipo_sub_producto?: string | null;
  monto_otorgado?: number | null;
  fecha_aprobacion?: string | null;
  condiciones_organismo_decisor?: string | null;
  aplica_fast_track?: boolean | null;
  aplica_leasing?: boolean | null;
  aplica_cobertura?: boolean | null;
  aplica_compra_cartera?: boolean | null;
  aplica_remodelacion?: boolean | null;
  gente_bbva?: boolean | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface CargaOperacionBancoDatosComercial {
  id_carga_operacion_banco_datos_comercial: number;
  id_carga_operacion_banco: number;
  id_expediente: number;

  // BBVA Colombia
  correo_declarativo_cliente?: string | null;
  numero_telefono_declarativo?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface CargaOperacionBanco {
  id_carga_operacion_banco: number;
  id_expediente: number;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;

  datos_operacion?: CargaOperacionBancoDatosOperacion | null;
  antecedentes_comprador?: CargaOperacionBancoAntecedenteComprador[] | null;
  antecedente_credito?: CargaOperacionBancoAntecedenteCredito | null;
  datos_comercial?: CargaOperacionBancoDatosComercial | null;
}
