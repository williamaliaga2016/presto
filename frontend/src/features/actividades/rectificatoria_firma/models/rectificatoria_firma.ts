import { RectificatoriaFirmaDetalle } from "./rectificatoria_firma_detalle";

export interface RectificatoriaFirma {
  id_rectificatoria_firma: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  is_subsanar: boolean;
  observaciones?: string | null;

  /**
   * Campos heredados/visualización.
   * Vienen desde el reparo generado en Visar Operación.
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

  detalles?: RectificatoriaFirmaDetalle[];
}
