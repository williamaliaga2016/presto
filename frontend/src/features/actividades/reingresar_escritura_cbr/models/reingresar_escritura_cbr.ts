export interface ReingresarEscrituraCbr {
  id_reingresar_escritura_cbr: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  is_subsanar: boolean;
  observaciones?: string | null;

  /**
   * Campos heredados/visualización.
   * Vienen desde el reparo generado en Recepción Carga Fábrica
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
