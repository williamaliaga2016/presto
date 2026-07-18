export interface CorregirReparoEstudioTitulos {
  id_corregir_reparo_estudio_titulos: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  is_subsanar: boolean;
  observaciones?: string | null;

  /**
   * Campos enriquecidos/visualización.
   * Vienen desde el reparo generado en Generar Estudio de Títulos
   * y/o desde la composición realizada en backend.
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
