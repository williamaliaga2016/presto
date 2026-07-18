import { useEffect, useMemo, useState } from 'react';
import { utilidadesServices } from '../api/utilidadesServices';
import type { UtilidadesDTO } from '../models/UtilidadesDTO';
import type { ControlBaseDTO } from '../models/ControlBaseDTO';

type MessageType = 'success' | 'error' | 'info';

interface MessageState {
  type: MessageType;
  text: string;
}

const initialForm: UtilidadesDTO = {
  id_expediente: '',
  accion_id: 0,
  actividad_id: 0,
  actividad_usuario_id: 0,
  observaciones: '',
  excepcion_id: '',
  accion_excepcion_id: '',
  usuario_excepcion_id: '',
  user_id: 0,
};

const ACCION_DESESTIMAR = 1;
const ACCION_REASIGNAR = 2;

export function useReasignacionDesestimacion() {
  const [form, setForm] = useState<UtilidadesDTO>(initialForm);
  const [acciones, setAcciones] = useState<ControlBaseDTO[]>([]);
  const [actividades, setActividades] = useState<ControlBaseDTO[]>([]);
  const [usuariosActividad, setUsuariosActividad] = useState<ControlBaseDTO[]>(
    [],
  );
  const [isEditing, setIsEditing] = useState(true);
  const [message, setMessage] = useState<MessageState | null>(null);

  const isReassign = useMemo(
    () => form.accion_id === ACCION_REASIGNAR,
    [form.accion_id],
  );

  useEffect(() => {
    void loadAcciones();
  }, []);

  const loadAcciones = async () => {
    try {
      const response = await utilidadesServices.catalogTypeUtility();

      if (!response.status) {
        setMessage({
          type: 'error',
          text:
            response.message ?? 'No se pudo cargar el catálogo de acciones.',
        });
        return;
      }

      setAcciones(response.detail ?? []);
    } catch (error) {
      console.error('Error cargando catálogo de acciones:', error);

      setMessage({
        type: 'error',
        text:
          error instanceof Error
            ? error.message
            : 'No se pudo cargar el catálogo de acciones.',
      });
    }
  };

  const updateField = <K extends keyof UtilidadesDTO>(
    field: K,
    value: UtilidadesDTO[K],
  ) => {
    setForm((current) => ({
      ...current,
      [field]: value,
    }));
  };

  const validateRequestNumber = async () => {
    const idExpediente = form.id_expediente.trim();

    if (!idExpediente) {
      return;
    }

    try {
      const response =
        await utilidadesServices.validateRequestNumber(idExpediente);

      setMessage({
        type: response.status ? 'success' : 'error',
        text:
          response.message ??
          (response.status ? 'Solicitud válida.' : 'Solicitud no válida.'),
      });
    } catch (error) {
      console.error('Error validando número de solicitud:', error);

      setMessage({
        type: 'error',
        text:
          error instanceof Error
            ? error.message
            : 'No se pudo validar la solicitud.',
      });
    }
  };

  const changeAccion = async (accionId: number) => {
    const nextForm: UtilidadesDTO = {
      ...form,
      accion_id: accionId,
      actividad_id: 0,
      actividad_usuario_id: 0,
    };

    setForm(nextForm);
    setActividades([]);
    setUsuariosActividad([]);

    if (accionId === ACCION_DESESTIMAR) {
      return;
    }

    if (accionId !== ACCION_REASIGNAR) {
      return;
    }

    if (!nextForm.id_expediente.trim()) {
      setMessage({
        type: 'error',
        text: 'Debe ingresar el número de solicitud.',
      });
      return;
    }

    try {
      const response = await utilidadesServices.getActivities(nextForm);

      if (!response.status) {
        setMessage({
          type: 'error',
          text: response.message ?? 'No se pudo cargar las actividades.',
        });
        setActividades([]);
        return;
      }

      setActividades(response.detail ?? []);
    } catch (error) {
      console.error('Error cargando actividades:', error);

      setMessage({
        type: 'error',
        text:
          error instanceof Error
            ? error.message
            : 'No se pudo cargar las actividades.',
      });
    }
  };

  const changeActividad = async (actividadId: number) => {
    const nextForm: UtilidadesDTO = {
      ...form,
      actividad_id: actividadId,
      actividad_usuario_id: 0,
    };

    setForm(nextForm);
    setUsuariosActividad([]);

    if (!actividadId) {
      return;
    }

    try {
      const response = await utilidadesServices.getUserActivity(nextForm);

      if (!response.status) {
        setMessage({
          type: 'error',
          text: response.message ?? 'No se pudo cargar los usuarios.',
        });
        setUsuariosActividad([]);
        return;
      }

      setUsuariosActividad(response.detail ?? []);
    } catch (error) {
      console.error('Error cargando usuarios de actividad:', error);

      setMessage({
        type: 'error',
        text:
          error instanceof Error
            ? error.message
            : 'No se pudo cargar los usuarios de la actividad.',
      });
    }
  };

  const validateSave = (): boolean => {
    if (!form.id_expediente.trim()) {
      setMessage({
        type: 'error',
        text: 'Debe ingresar el número de solicitud.',
      });
      return false;
    }

    if (!form.accion_id) {
      setMessage({ type: 'error', text: 'Debe seleccionar la acción.' });
      return false;
    }

    if (form.accion_id === ACCION_REASIGNAR && !form.actividad_id) {
      setMessage({
        type: 'error',
        text: 'Debe seleccionar la actividad actual.',
      });
      return false;
    }

    if (form.accion_id === ACCION_REASIGNAR && !form.actividad_usuario_id) {
      setMessage({
        type: 'error',
        text: 'Debe seleccionar el usuario destino.',
      });
      return false;
    }

    if (!form.observaciones.trim()) {
      setMessage({ type: 'error', text: 'Debe ingresar comentarios.' });
      return false;
    }

    return true;
  };

  const save = async () => {
    if (!validateSave()) {
      return;
    }

    try {
      const response = await utilidadesServices.save({
        ...form,
        id_expediente: form.id_expediente.trim(),
        observaciones: form.observaciones.trim(),
      });

      setMessage({
        type: response.status ? 'success' : 'error',
        text:
          response.message ??
          (response.status
            ? 'Operación realizada correctamente.'
            : 'No se pudo realizar la operación.'),
      });

      if (response.status) {
        setIsEditing(false);
      }
    } catch (error) {
      console.error('Error guardando utilidad:', error);

      setMessage({
        type: 'error',
        text:
          error instanceof Error
            ? error.message
            : 'No se pudo guardar la operación.',
      });
    }
  };

  const reset = () => {
    setForm(initialForm);
    setActividades([]);
    setUsuariosActividad([]);
    setIsEditing(true);
    setMessage(null);
  };

  return {
    form,
    acciones,
    actividades,
    usuariosActividad,
    isEditing,
    isReassign,
    message,
    setIsEditing,
    updateField,
    validateRequestNumber,
    changeAccion,
    changeActividad,
    save,
    reset,
  };
}
