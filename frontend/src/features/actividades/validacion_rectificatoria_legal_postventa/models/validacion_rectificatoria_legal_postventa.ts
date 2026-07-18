import { CargaOperacionBancoAntecedenteComprador, CargaOperacionBancoAntecedenteVendedor } from "../../carga_operacion_banco/models/carga_operacion_banco";

export interface ValidacionRectificatoriaLegalPostventa {
  id_validacion_rectificatoria_legal_postventa: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  is_subsanar: boolean;
  observaciones?: string | null;
  rol_comparecencia?: string | null;
  /**
   * Campos heredados/visualización.
   * Vienen desde el reparo generado en Verificar Reparo en CBR
   * y/o desde la consulta con Users.
   */
  solicitante?: string | null;
  observaciones_reparo?: string | null;
  fecha_ingreso?: string | null;
  require_documentacion: string | null;
  realiza_pago: string | null;
  encargado_firma: boolean;
  requiere_inscripcion_cbr: boolean;

  antecedentes_comprador?: CargaOperacionBancoAntecedenteComprador[] | null;
  antecedentes_vendedor?: CargaOperacionBancoAntecedenteVendedor[] | null;
  validacion_rectificatoria_legal_postventa_datos_personales?: ValidacionRectificatoriaLegalPostventaDatosPersonales[] | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface ValidacionRectificatoriaLegalPostventaDatosPersonales {
  id_validacion_rectificatoria_legal_postventa_datos_personales: number;
  id_validacion_rectificatoria_legal_postventa: number;
  id_expediente: number;

  rut?: string | null;
  fecha_nacimiento?: string | null;
  genero?: string | null;
  nombres?: string | null;
  apellido_paterno?: string | null;
  apellido_materno?: string | null;
  nacionalidad?: string | null;
  relacion_titular?: string | null;
  profesion?: string | null;
  direccion?: string | null;
  estado_civil?: string | null;
  telefono?: string | null;
  email?: string | null;
  region?: string | null;
  comuna?: string | null;
  rol_comparecencia?: string | null;
  
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}