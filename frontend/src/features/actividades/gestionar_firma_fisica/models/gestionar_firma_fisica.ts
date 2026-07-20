export interface GestionarFirmaFisica {
  id: number;
  id_expediente: number;
  id_actividad: string;
  motorizado_asignado?: string | null;
  fecha_gestoria?: string | null;
  resultado_gestoria?: string | null;
  observaciones?: string | null;
}
