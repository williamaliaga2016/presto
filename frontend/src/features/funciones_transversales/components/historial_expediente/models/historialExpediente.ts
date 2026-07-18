export interface HistorialExpediente {
  actividad: string;
  status: string;
  usuario: string;
  rol: string;
  fecha_inicio?: string | null;
  fecha_termino?: string | null;
  orden: number;
}
