export interface CorregirReparoInstPago {
  id_corregir_reparo_inst_pago: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  is_subsanar: boolean;
  observaciones?: string | null;
  enviar_a_reparo: boolean;

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
