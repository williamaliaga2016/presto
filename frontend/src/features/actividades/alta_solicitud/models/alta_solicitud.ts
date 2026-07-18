export interface AltaSolicitud {
  id_alta_solicitud: number;
  id_expediente: number;
  id_tipo_moneda?: number | null;
  id_tipo_documento?: number | null;
  numero_documento?: string | null;
  nombre_razon_social?: string | null;
  fecha_emision?: string | null;
  nro_comprobante?: string | null;
  comprobante_detalle?: string | null;
  importe: number;
  fecha_recepcion_factura?: string | null;
  fecha_vencimiento?: string | null;
  observaciones?: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
