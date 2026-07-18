export interface DevolucionVbComercialData {
  id: number;
  idExpediente: number;
  idActividad: string;
  clienteDesiste: boolean | null;
  motivoCierre?: string | null;
  observaciones?: string | null;
}

export interface DevolucionVbComercialForm {
  actividad: DevolucionVbComercialData;
}
