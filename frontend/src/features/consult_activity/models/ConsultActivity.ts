export type EstadoTracking = 'Done' | 'InProgress' | 'NotStarted' | string;

export interface SearchCriteriaDTO {
  option: string | null;
  search_criteria: string | null;
}

export interface ConsultActivityDTO {
  id_expediente: number;
  descripcion: string;
  status: string;
  descripcion_rol: string;
  usuario: string;
  fecha_asignacion: string;
}

export interface ActividadDTO {
  id_etapa: number;
  id_expediente: number;
  id_actividad: string;
  actividad: string;
  estado: EstadoTracking;
  url_act: string;

  // Campos de UI
  title?: string[];
}

export interface EtapaDTO {
  id_etapa: number;
  etapa: string;
  current_actividades: number;
  actividades: ActividadDTO[];

  // Campos de UI
  estado: EstadoTracking;
  title?: string[];
}
