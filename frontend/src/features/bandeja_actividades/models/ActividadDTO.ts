export interface ActividadDTO {
  nro_row: number;
  id_expediente: number;
  id_actividad?: string | null;
  actividad?: string | null;
  fecha_asignacion?: string | null;
  estado?: string | null;
  rol?: string | null;
  nombre_responsable?: string | null;
  url_act?: string | null;

  id_etapa?: number;
  title?: string[] | null;
}
