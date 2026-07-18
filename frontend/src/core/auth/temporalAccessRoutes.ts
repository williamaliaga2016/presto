import { matchPath } from 'react-router-dom';
import type { AuthTokenClaims } from './tokenClaims';

const temporalActivityRoutes: Record<string, string> = {
  ACT_DOCS_CLIENTE: '/home/cargar_documentos_cliente/:id_expediente',
  ACT_SOPORTES_PAGO: '/home/cargar_soportes_pago/:id_expediente',
};

/**
 * Valida que la sesion temporal navegue solo hacia la ruta autorizada por el token.
 *
 * @param claims Claims publicos extraidos del JWT temporal.
 * @param pathname Ruta actual del navegador.
 * @returns `true` cuando la ruta coincide con la actividad y expediente del token.
 */
export function isAllowedTemporalRoute(
  claims: AuthTokenClaims | null,
  pathname: string,
): boolean {
  const activityId = claims?.id_actividad;
  const expedienteId = claims?.id_expediente;

  if (!activityId || !expedienteId) return false;

  const routePattern = temporalActivityRoutes[activityId];
  if (!routePattern) return false;

  const match = matchPath(
    {
      path: routePattern,
      end: true,
    },
    pathname,
  );

  return match?.params.id_expediente === expedienteId;
}
