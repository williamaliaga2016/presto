export interface ExpedienteDigital {
  id_archivo?: number;
  id_expediente: number;
  /** Actividad workflow que origina la carga para trazabilidad por etapa. */
  activity_id?: string | null;
  id_documento: number;
  id_usuario?: number | null;
  nombre_archivo: string;
  nombre_archivo_original: string;
  extension: string;
  version_archivo: number;
  fecha_alta?: string | null;
  comentarios: string;
  is_active: boolean;

  // Campos solo de UI.
  is_checked?: boolean;
}
