import type { AxiosError } from 'axios';

import { dispatch_permission_denied, type permission_denied_source } from '@/core/security/permission_denied_event';

function get_source_from_url(url: string): permission_denied_source {
  const normalized_url = url.toLowerCase();

  if (normalized_url.includes('getbyexpediente')) return 'get_by_expediente';
  if (normalized_url.includes('/save')) return 'save';
  if (normalized_url.includes('/avanzar')) return 'avanzar';

  return 'general';
}

function get_message(error: AxiosError): string {
  const data = error.response?.data as { message?: string } | undefined;

  return data?.message || 'No tiene permisos para ejecutar esta acción.';
}

function get_permission_code(error: AxiosError): string | undefined {
  const data = error.response?.data as { permission_code?: string } | undefined;

  return data?.permission_code;
}

export function handle_permission_error(error: AxiosError): Promise<never> {
  const status = error.response?.status ?? 0;
  const url = error.config?.url ?? '';

  if (status === 403) {
    dispatch_permission_denied({
      status,
      message: get_message(error),
      permission_code: get_permission_code(error),
      source: get_source_from_url(url),
      url,
    });
  }

  return Promise.reject(error);
}
