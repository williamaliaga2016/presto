/**
 * Link temporal generado para que un usuario externo continue la carga documental.
 */
export interface AccesoTemporalGenerado {
  url: string;
  token: string;
  fecha_expiracion: string;
}

/**
 * Respuesta del avance de Validar Informacion con datos de workflow y acceso temporal opcional.
 */
export interface AvanzarValidarInformacionResponse {
  workflow: unknown;
  acceso_temporal?: AccesoTemporalGenerado | null;
}
