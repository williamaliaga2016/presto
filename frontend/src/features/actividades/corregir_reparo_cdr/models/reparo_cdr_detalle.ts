export interface ReparoCdrDetalle {
  id_reparo_cdr: number;
  id_expediente: number;
  solicitante: string;
  observaciones: string;
  fecha_ingreso: string | null;
  subsanar: boolean;
}
