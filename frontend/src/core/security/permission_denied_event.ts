export const PERMISSION_DENIED_EVENT = 'multibanca:permission-denied';

export type permission_denied_source =
  | 'get_by_expediente'
  | 'save'
  | 'avanzar'
  | 'general';

export interface permission_denied_payload {
  status: number;
  message: string;
  permission_code?: string;
  source: permission_denied_source;
  url?: string;
}

export function dispatch_permission_denied(payload: permission_denied_payload): void {
  window.dispatchEvent(
    new CustomEvent<permission_denied_payload>(PERMISSION_DENIED_EVENT, {
      detail: payload,
    }),
  );
}
