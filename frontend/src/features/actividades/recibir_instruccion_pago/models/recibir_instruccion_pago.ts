export interface RecibirInstruccionPago {
  id_recibir_instruccion_pago: number;
  id_expediente: number;
  enviar_a_reparo: boolean;
  condicion_especial_desembolso?: string | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
