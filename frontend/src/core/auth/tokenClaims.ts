/**
 * Claims publicos usados por el frontend para identificar caracteristicas de la sesion autenticada.
 */
export interface AuthTokenClaims {
  tipo_acceso?: string;
  id_expediente?: string;
  id_actividad?: string;
}

/**
 * Extrae los claims publicos del JWT para identificar el tipo de sesion activa.
 *
 * @param token JWT almacenado para la sesion actual.
 * @returns Claims decodificados o `null` cuando el token no tiene formato esperado.
 */
export function getTokenClaims(token: string | null): AuthTokenClaims | null {
  if (!token) return null;

  const [, payload] = token.split('.');
  if (!payload) return null;

  try {
    const normalizedPayload = payload.replace(/-/g, '+').replace(/_/g, '/');
    const paddedPayload = normalizedPayload.padEnd(
      normalizedPayload.length + ((4 - (normalizedPayload.length % 4)) % 4),
      '=',
    );

    return JSON.parse(atob(paddedPayload)) as AuthTokenClaims;
  } catch {
    return null;
  }
}
