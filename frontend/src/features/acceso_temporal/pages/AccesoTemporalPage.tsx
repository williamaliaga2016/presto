import axios from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { ProgressSpinner } from 'primereact/progressspinner';
import { useAuth } from '@/app/providers/AuthProvider';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { accesoTemporalService } from '../api/accesoTemporalService';
import type { AccesoTemporalValidarResponse } from '../models/accesoTemporal';

interface ErrorResponse {
  message?: string;
}

const DEFAULT_ERROR =
  'No fue posible validar el enlace de acceso temporal. Verifique el enlace o comuniquese con su asesor BBVA.';
const LINK_UNAVAILABLE_ERROR = 'Este link ya no se encuentra disponible';
const LINK_EXPIRED_ERROR =
  'El enlace de acceso ha expirado. Comuníquese con su asesor para obtener un nuevo enlace.';
const validationRequests = new Map<
  string,
  Promise<ApiResponse<AccesoTemporalValidarResponse | null>>
>();

/**
 * Reutiliza una validacion en curso para evitar consumir dos veces el mismo token en Strict Mode.
 *
 * @param token - Token temporal recibido desde la URL publica.
 * @returns Promesa compartida de validacion del token.
 */
function validateTokenOnce(
  token: string,
): Promise<ApiResponse<AccesoTemporalValidarResponse | null>> {
  if (!validationRequests.has(token)) {
    validationRequests.set(token, accesoTemporalService.validar({ token }));
  }

  return validationRequests.get(token)!;
}

/**
 * Convierte errores de validacion del token a mensajes seguros para el usuario externo.
 *
 * @param message - Mensaje o codigo retornado por el backend.
 * @returns Mensaje funcional que debe mostrarse en pantalla.
 */
function normalizeAccessErrorMessage(message?: string): string {
  if (!message) return DEFAULT_ERROR;

  const normalized = message.toUpperCase();
  if (
    normalized.includes('TOKEN_EXPIRADO') ||
    normalized.includes('HA EXPIRADO')
  ) {
    return LINK_EXPIRED_ERROR;
  }

  if (
    normalized.includes('TOKEN_USADO') ||
    normalized.includes('YA FUE UTILIZADO') ||
    normalized.includes('YA NO SE ENCUENTRA DISPONIBLE') ||
    normalized.includes('NOT A VALID NUMBER') ||
    normalized.includes('VALIDATION ERRORS')
  ) {
    return LINK_UNAVAILABLE_ERROR;
  }

  return message;
}

/**
 * Obtiene el mensaje funcional que debe mostrarse cuando falla la validacion del link temporal.
 *
 * @param error - Error recibido desde Axios, React Query o runtime.
 * @returns Mensaje seguro para mostrar al cliente externo.
 */
function getErrorMessage(error: unknown): string {
  if (axios.isAxiosError<ErrorResponse>(error)) {
    return normalizeAccessErrorMessage(error.response?.data?.message);
  }

  if (error instanceof Error) {
    return normalizeAccessErrorMessage(error.message);
  }

  return DEFAULT_ERROR;
}

/**
 * Pagina publica que intercambia el token UUID del link por un JWT temporal y navega al flujo protegido.
 *
 * @returns Vista de validacion o mensaje de error para el link temporal.
 */
export default function AccesoTemporalPage() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const { isTemporalAccess, loginWithToken, logout } = useAuth();
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [isValidating, setIsValidating] = useState(Boolean(searchParams.get('token')));
  const token = searchParams.get('token');
  const currentErrorMessage = token ? errorMessage : DEFAULT_ERROR;

  /**
   * Limpia una sesion temporal previa cuando el link publico ya no puede abrirse.
   *
   * @returns No retorna valor.
   */
  const clearTemporalSessionIfPresent = useCallback((): void => {
    if (isTemporalAccess) {
      logout();
    }
  }, [isTemporalAccess, logout]);

  useEffect(() => {
    let isActive = true;

    if (!token) {
      clearTemporalSessionIfPresent();

      return () => {
        isActive = false;
      };
    }

    validateTokenOnce(token)
      .then((response) => {

        if (!isActive) return;

        if (!response.status || !response.detail?.jwt) {
          clearTemporalSessionIfPresent();
          setErrorMessage(normalizeAccessErrorMessage(response.message));
          return;
        }

        loginWithToken(response.detail.jwt);
        navigate(response.detail.url_redirect, { replace: true });
      })
      .catch((error) => {
        if (isActive) {
          clearTemporalSessionIfPresent();
          setErrorMessage(getErrorMessage(error));
        }
      })
      .finally(() => {
        if (isActive) {
          setIsValidating(false);
        }
      });

    return () => {
      isActive = false;
    };
  }, [clearTemporalSessionIfPresent, loginWithToken, navigate, token]);

  return (
    <main className="flex min-h-screen items-center justify-center bg-slate-50 px-4">
      <section className="w-full max-w-md rounded-lg bg-white p-6 text-center shadow-sm">
        {currentErrorMessage || !isValidating ? (
          <>
            <h1 className="mb-3 text-xl font-semibold text-slate-900">
              Enlace no disponible
            </h1>
            <p className="text-sm leading-6 text-slate-600">
              {currentErrorMessage ?? LINK_UNAVAILABLE_ERROR}
            </p>
          </>
        ) : (
          <>
            <ProgressSpinner aria-label="Validando acceso temporal" />
            <h1 className="mt-4 text-xl font-semibold text-slate-900">
              Validando acceso
            </h1>
          </>
        )}
      </section>
    </main>
  );
}
