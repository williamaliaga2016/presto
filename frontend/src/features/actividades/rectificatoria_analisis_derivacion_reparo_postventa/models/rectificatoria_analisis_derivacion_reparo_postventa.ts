export interface RectificatoriaAnalisisDerivacionReparoPostventa {
  id_rectificatoria_analisis_derivacion_reparo_postventa: number;
  id_expediente: number;
  enviar_reparo_a?: boolean | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
