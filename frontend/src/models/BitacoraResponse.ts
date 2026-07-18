export interface BitacoraResponse {
  idBitacora: number;

  idExpediente: number;
  idUsuario: number;
  idActividad: string;
  fechaAlta: string;
  observaciones: string;

  isActive: boolean;
  rowStatus: boolean;
  createdBy: number;
  createdDate: string;
  modifiedBy: number | null;
  modifiedDate: string | null;

  actividad: string;
  usuario: string;
  rol: string;
}