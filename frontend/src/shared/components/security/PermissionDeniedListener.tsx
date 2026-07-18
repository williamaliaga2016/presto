import { useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';

import { Toast } from 'primereact/toast';

import {
  PERMISSION_DENIED_EVENT,
  type permission_denied_payload,
} from '@/core/security/permission_denied_event';

function get_summary(source?: permission_denied_payload['source']): string {
  switch (source) {
    case 'get_by_expediente':
      return 'Permiso de consulta denegado';
    case 'save':
      return 'Permiso de edición denegado';
    case 'avanzar':
      return 'Permiso de avance denegado';
    default:
      return 'Permiso denegado';
  }
}

function get_default_message(source?: permission_denied_payload['source']): string {
  switch (source) {
    case 'get_by_expediente':
      return 'No tiene permisos para consultar esta actividad.';
    case 'save':
      return 'No tiene permisos para editar o guardar esta actividad.';
    case 'avanzar':
      return 'No tiene permisos para avanzar esta actividad.';
    default:
      return 'No tiene permisos para ejecutar esta acción.';
  }
}

function should_go_back(source?: permission_denied_payload['source']): boolean {
  return source === 'get_by_expediente';
}

export function PermissionDeniedListener() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const handler = (event: Event) => {
      const custom_event = event as CustomEvent<permission_denied_payload>;
      const detail = custom_event.detail;

      toast.current?.show({
        severity: 'warn',
        summary: get_summary(detail?.source),
        detail: detail?.message || get_default_message(detail?.source),
        life: 5000,
      });

      /*
        Solo en consulta volvemos atrás para evitar que el usuario quede
        en un formulario vacío cuando no tiene permiso para GetByExpediente.

        En Save y Avanzar NO navegamos:
        - Save: se mantiene en la pantalla y se informa que no puede guardar.
        - Avanzar: se mantiene en la pantalla y se informa que no puede avanzar.
      */
      if (should_go_back(detail?.source)) {
        setTimeout(() => {
          navigate(-1);
        }, 150);
      }
    };

    window.addEventListener(PERMISSION_DENIED_EVENT, handler);

    return () => {
      window.removeEventListener(PERMISSION_DENIED_EVENT, handler);
    };
  }, [navigate]);

  return <Toast ref={toast} position="top-right" />;
}
