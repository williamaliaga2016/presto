/**
 * Payload publico enviado para validar el token que llega en /acceso-temporal.
 */
export interface AccesoTemporalValidarRequest {
  token: string;
}

/**
 * Respuesta de backend con el JWT temporal y el destino de navegacion.
 */
export interface AccesoTemporalValidarResponse {
  jwt: string;
  id_expediente: number;
  id_actividad: string;
  url_redirect: string;
}
