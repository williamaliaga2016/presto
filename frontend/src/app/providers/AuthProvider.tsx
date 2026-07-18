/* eslint-disable react-refresh/only-export-components */
import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useMemo,
  useState,
} from "react";
import { authStorage } from "@/core/auth/authStorage";
import { getTokenClaims } from "@/core/auth/tokenClaims";
import type { AuthUser } from "@/features/auth/models/AuthUser";

export interface AuthContextType {
  token: string | null;
  user: AuthUser | null;
  isAuthenticated: boolean;
  isTemporalAccess: boolean;
  login: (token: string, user?: AuthUser) => void;
  /** Autentica sesiones temporales sin datos de usuario persistidos del login tradicional. */
  loginWithToken: (token: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

/**
 * Provee el estado de autenticacion para sesiones normales y accesos temporales.
 *
 * @param props - Propiedades del proveedor.
 * @param props.children - Arbol de componentes que consumira el contexto de autenticacion.
 * @returns Provider de React con token, usuario y acciones de autenticacion.
 */
export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(authStorage.getToken());
  const [user, setUser] = useState<AuthUser | null>(authStorage.getUser());
  const tokenClaims = useMemo(() => getTokenClaims(token), [token]);
  const isTemporalAccess = tokenClaims?.tipo_acceso === 'temporal';

  /**
   * Autentica una sesion normal y persiste opcionalmente los datos del usuario.
   *
   * @param newToken - JWT emitido por el login tradicional.
   * @param newUser - Datos del usuario autenticado, cuando estan disponibles.
   * @returns No retorna valor.
   */
  const login = useCallback((newToken: string, newUser?: AuthUser): void => {
    authStorage.setToken(newToken);
    if (newUser) {
      authStorage.setUser(newUser);
      setUser(newUser);
    }
    setToken(newToken);
  }, []);

  /**
   * Autentica una sesion temporal sin conservar usuario del login tradicional.
   *
   * @param newToken - JWT temporal emitido al validar el link publico.
   * @returns No retorna valor.
   */
  const loginWithToken = useCallback((newToken: string): void => {
    authStorage.clear();
    authStorage.setToken(newToken);
    setUser(null);
    setToken(newToken);
  }, []);

  /**
   * Cierra la sesion actual y limpia el almacenamiento local de autenticacion.
   *
   * @returns No retorna valor.
   */
  const logout = useCallback((): void => {
    authStorage.clear();
    setToken(null);
    setUser(null);
  }, []);

  const value = useMemo(
    () => ({
      token,
      user,
      isAuthenticated: !!token,
      isTemporalAccess,
      login,
      loginWithToken,
      logout,
    }),
    [isTemporalAccess, login, loginWithToken, logout, token, user]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

/**
 * Obtiene el contexto de autenticacion activo dentro del arbol de React.
 *
 * @returns Estado y acciones de autenticacion disponibles para la aplicacion.
 */
export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth debe usarse dentro de AuthProvider");
  }

  return context;
}
