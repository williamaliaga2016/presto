export interface CorregirReparoDatosOperacion {
  id_corregir_reparo_datos_operacion: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  is_subsanar: boolean;
  observaciones?: string | null;

  /**
   * Campos heredados/visualización.
   * Vienen desde el reparo generado en Ingresar Datos Operación
   * y/o desde la consulta con Users.
   */
  solicitante?: string | null;
  observaciones_reparo?: string | null;
  fecha_ingreso?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
