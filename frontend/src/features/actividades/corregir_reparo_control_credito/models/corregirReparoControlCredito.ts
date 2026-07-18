export interface CorregirReparoControlCredito {
  id_corregir_reparo_control_credito?: number | null;
  id_realizar_control_credito?: number | null;
  id_expediente?: number | null;
  id_usuario_solicitante?: number | null;
  subsanar?: boolean | null;
  observaciones?: string | null;
  
  id_solicitud?: number | null;
  id_solicitante?: number | null;
  solicitante?: string | null;
  observaciones_reparo?: string | null;
  fecha_ingreso?: string | null;

  is_active?: boolean | null;
  row_status?: boolean | null;
  created_by?: number | null;
  created_date?: string | null;
  modified_by?: number | null;
  modified_date?: string | null;
}