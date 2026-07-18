import type { ReparoCdrDetalle } from "./reparo_cdr_detalle";

export interface CorregirReparoCdr {
  id_corregir_reparo_cdr: number;
  id_expediente: number;
  observaciones: string | null;
  reparo: ReparoCdrDetalle | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
