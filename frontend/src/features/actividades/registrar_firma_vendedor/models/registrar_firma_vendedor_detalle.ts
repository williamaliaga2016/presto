export interface FirmaVendedorDetalle {
  id_firma_vendedor_detalle: number;
  id_firma_vendedor: number;
  id_expediente: number;
  relacion_titular: string | null;
  rut?: string | null;
  nombres?: string | null;
  apellido_paterno?: string | null;
  apellido_materno?: string | null;
  estado_civil?: string | null;
  fecha_firma?: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
