import { useEffect, useMemo } from "react";
import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "../providers/AuthProvider";
import { isAllowedTemporalRoute } from "@/core/auth/temporalAccessRoutes";
import { getTokenClaims } from "@/core/auth/tokenClaims";

interface TemporalAccessRedirectProps {
  logout: () => void;
}

/**
 * Limpia la sesion temporal cuando se intenta acceder a una ruta fuera del alcance autorizado.
 *
 * @param props Propiedades del componente.
 * @param props.logout Accion que limpia el token local.
 * @returns `null` mientras el contexto de autenticacion aplica la salida.
 */
function TemporalAccessRedirect({ logout }: TemporalAccessRedirectProps) {
  useEffect(() => {
    logout();
  }, [logout]);

  return null;
}

/**
 * Protege rutas privadas y limita sesiones temporales a la actividad autorizada por el JWT.
 *
 * @returns Rutas hijas cuando la sesion es valida o redireccion al login cuando no corresponde.
 */
export function ProtectedRoute() {
  const { isAuthenticated, isTemporalAccess, logout, token } = useAuth();
  const location = useLocation();
  const tokenClaims = useMemo(() => getTokenClaims(token), [token]);
  const isTemporalRouteAllowed = !isTemporalAccess ||
    isAllowedTemporalRoute(tokenClaims, location.pathname);

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  if (!isTemporalRouteAllowed) {
    return <TemporalAccessRedirect logout={logout} />;
  }

  return <Outlet />;
}
