export interface Bitacora {
  id_bitacora?: number;
  id_expediente: number;
  id_actividad: string;
  observaciones: string;
  fecha_alta?: string | null;
  usuario?: string | null;
}
