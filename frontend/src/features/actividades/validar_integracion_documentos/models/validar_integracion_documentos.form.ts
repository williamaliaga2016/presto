import type {
  ValidarIntegracionDocumentosData,
  ValidarIntegracionDocumentosForm,
} from './validar_integracion_documentos';

export const VALIDAR_INTEGRACION_ACTIVITY_ID = 'ACT_VALIDAR_INTEGRACION';

export const buildInitialValidarIntegracion = (
  id_expediente: number,
): ValidarIntegracionDocumentosForm => ({
  actividad: {
    id: 0,
    idExpediente: id_expediente,
    idActividad: VALIDAR_INTEGRACION_ACTIVITY_ID,
    documentosCorrectos: null,
    creditoCondicionado: false,
    motivoDevolucion: '',
    observaciones: '',
  },
  datos_credito: {
    tipo_credito: '',
    tiene_garantia: false,
  },
});

export const normalizeValidarIntegracion = (
  source: ValidarIntegracionDocumentosData | null | undefined,
  id_expediente_fallback: number,
): ValidarIntegracionDocumentosForm => ({
  actividad: {
    id: Number(source?.id ?? 0),
    idExpediente: Number(source?.idExpediente ?? id_expediente_fallback),
    idActividad:
      source?.idActividad ?? VALIDAR_INTEGRACION_ACTIVITY_ID,
    documentosCorrectos: source?.documentosCorrectos ?? null,
    creditoCondicionado: Boolean(source?.creditoCondicionado ?? false),
    motivoDevolucion: source?.motivoDevolucion ?? '',
    observaciones: source?.observaciones ?? '',
  },
  datos_credito: {
    tipo_credito: '',
    tiene_garantia: false,
  },
});

export const toValidarIntegracionSavePayload = (
  form: ValidarIntegracionDocumentosForm,
  id_expediente: number,
): ValidarIntegracionDocumentosData => ({
  ...form.actividad,
  idExpediente: id_expediente,
});
