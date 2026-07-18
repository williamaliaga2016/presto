import type {
  DevolucionVbComercialData,
  DevolucionVbComercialForm,
} from './devolucion_vb_comercial';

export const DEVOLUCION_VB_COMERCIAL_ACTIVITY_ID =
  'ACT_DEVOLUCION_VB_COMERCIAL';

export const buildInitialDevolucionVbComercial = (
  id_expediente: number,
): DevolucionVbComercialForm => ({
  actividad: {
    id: 0,
    idExpediente: id_expediente,
    idActividad: DEVOLUCION_VB_COMERCIAL_ACTIVITY_ID,
    clienteDesiste: null,
    motivoCierre: '',
    observaciones: '',
  },
});

export const normalizeDevolucionVbComercial = (
  source: DevolucionVbComercialData | null | undefined,
  id_expediente_fallback: number,
): DevolucionVbComercialForm => ({
  actividad: {
    id: Number(source?.id ?? 0),
    idExpediente: Number(source?.idExpediente ?? id_expediente_fallback),
    idActividad:
      source?.idActividad ?? DEVOLUCION_VB_COMERCIAL_ACTIVITY_ID,
    clienteDesiste: source?.clienteDesiste ?? null,
    motivoCierre: source?.motivoCierre ?? '',
    observaciones: source?.observaciones ?? '',
  },
});

export const toDevolucionVbComercialSavePayload = (
  form: DevolucionVbComercialForm,
  id_expediente: number,
): DevolucionVbComercialData => ({
  ...form.actividad,
  idExpediente: id_expediente,
});
