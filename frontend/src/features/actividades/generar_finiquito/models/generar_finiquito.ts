export interface GenerarFiniquito {
  id_generar_finiquito: number;
  id_expediente: number;
  fojas_propiedad: string;
  numero_propiedad: string;
  año_propiedad: string;
  fojas_hipoteca: string;
  numero_hipoteca: string;
  año_hipoteca: string;
  fojas_prohibicion: string;
  numero_prohibicion: string;
  año_prohibicion: string;
  fojas_hipoteca_2grado: string;
  numero_hipoteca_2grado: string;
  año_hipoteca_2grado: string;
  observaciones?: string | null;
  
  tipo_propiedad?: string | null;
  direccion?: string | null;
  comuna?: string | null;
  rol_avaluo?: string | null;
  tipo_tasacion: string | null;
  fecha_informe_tasacion?: string | null;
  fecha_solicitud_tasacion?: string | null;
  fecha_recepcion_tasacion?: string | null;
  
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}