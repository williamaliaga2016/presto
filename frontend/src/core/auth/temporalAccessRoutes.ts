import { matchPath } from "react-router-dom";
import type { AuthTokenClaims } from "./tokenClaims";

const temporalActivityRoutes: Record<string, string> = {
  ACT_DOCS_CLIENTE:
    "/home/cargar_documentos_cliente/:id_expediente",

  BBVA_CONTACTO_CARGAR_DOCUMENTOS_CLIENTE_CBF7A738:
    "/home/cargar_documentos_cliente/:id_expediente",

  ACT_SOPORTES_PAGO:
    "/home/cargar_soportes_pago/:id_expediente",

  BBVA_CONTACTO_CARGAR_SOPORTES_DE_PAGO_899F408B:
    "/home/cargar_soportes_pago/:id_expediente",
};

export function getTemporalActivityRoute(
  id_actividad: string,
  id_expediente: number,
): string {
  const route = temporalActivityRoutes[id_actividad];

  if (!route) {
    return "/home/bandeja";
  }

  return route.replace(
    ":id_expediente",
    String(id_expediente),
  );
}

export function isAllowedTemporalRoute(
  claims: AuthTokenClaims | null,
  pathname: string,
): boolean {
  const id_actividad = claims?.id_actividad;
  const id_expediente = claims?.id_expediente;

  if (!id_actividad || !id_expediente) {
    return false;
  }

  const routePattern =
    temporalActivityRoutes[id_actividad];

  if (!routePattern) {
    return false;
  }

  const match = matchPath(
    {
      path: routePattern,
      end: true,
    },
    pathname,
  );

  return (
    match?.params.id_expediente ===
    String(id_expediente)
  );
}
