export interface GestionRectificatoriaSolucionReparo {
  id_gestion_rectificatoria_solucion_reparo?: number | null;
  id_gestion_rectificatoria?: number | null;
  id_expediente?: number | null;
  id_usuario_solicitante?: number | null;
modificar_datos_memo?: boolean | null;
  descontabilizar_operacion?: boolean | null;
  subsanar?: boolean | null;
  observaciones?: string | null;
  id_solicitante:number | null;
  id_solicitud:number | null;
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