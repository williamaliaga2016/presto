export interface RealizarRevisionPrevioFirmaBanco {
  id_realizar_revision_previo_firma_banco: number;
  id_expediente: number;
  enviar_a_reparo: boolean;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
