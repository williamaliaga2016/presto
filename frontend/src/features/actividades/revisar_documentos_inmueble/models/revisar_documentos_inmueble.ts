export const ACTIVIDAD_ID_REVISAR_DOCUMENTOS_INMUEBLE = 'ACT_REVISAR_DOCS';

export interface InformacionGeneralRDI {
  editable: boolean;
  responsable?: string | null;
  actividad?: string | null;
  estado?: string | null;
  usuario_asignado?: string | null;
  fecha_alta?: string | null;
  fecha_asignacion?: string | null;
}

export interface DatosTitularHeredado {
  tipo_identificacion?: string | null;
  numero_identificacion?: string | null;
  nombre_completo_t1?: string | null;
  situacion_laboral?: string | null;
}

export interface DatosInmuebleHeredado {
  tipo_inmueble?: string | null;
  estado_inmueble?: string | null;
  constructora?: string | null;
  descripcion_proyecto?: string | null;
  departamento_inmueble?: string | null;
  municipio_inmueble?: string | null;
}

export interface DatosCreditoHeredado {
  tipo_credito?: string | null;
  tiene_garantia?: boolean | null;
  monto_otorgado_vi?: number | null;
}

export interface CondicionesFinancierasHeredado {
  scoring?: string | null;
  subproducto?: string | null;
  monto_otorgado?: number | null;
  plazo_meses?: number | null;
  tasa?: number | null;
  condiciones_organismo_decisor?: string | null;
}

export interface DatosHeredadosRDI {
  editable: boolean;
  datos_titular: DatosTitularHeredado;
  datos_inmueble: DatosInmuebleHeredado;
  datos_credito: DatosCreditoHeredado;
  condiciones_financieras: CondicionesFinancierasHeredado;
}

export interface DocumentosObligatoriosRDI {
  completos: boolean;
  faltantes: string[];
}

export interface RevisarDocumentosInmuebleFormulario {
  id: number;
  id_expediente: number;
  id_actividad?: string | null;
  documentos_correctos: boolean | null;
  motivo_devolucion: string | null;
  observaciones: string | null;
  requiere_actualizacion_avaluos: string | null;
  homologacion: string | null;
  is_active?: boolean;
  row_status?: boolean;
  created_by?: number;
  created_date?: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RevisarDocumentosInmuebleDetail {
  informacion_general: InformacionGeneralRDI;
  datos_heredados: DatosHeredadosRDI;
  expediente_digital: unknown[];
  documentos_obligatorios: DocumentosObligatoriosRDI;
  formulario: RevisarDocumentosInmuebleFormulario;
}

export const EMPTY_DATOS_HEREDADOS_RDI: DatosHeredadosRDI = {
  editable: false,
  datos_titular: {},
  datos_inmueble: {},
  datos_credito: {},
  condiciones_financieras: {},
};

export const EMPTY_DOCUMENTOS_OBLIGATORIOS_RDI: DocumentosObligatoriosRDI = {
  completos: true,
  faltantes: [],
};

export const buildInitialState = (
  id_expediente: number,
): RevisarDocumentosInmuebleFormulario => ({
  id: 0,
  id_expediente,
  id_actividad: ACTIVIDAD_ID_REVISAR_DOCUMENTOS_INMUEBLE,
  documentos_correctos: null,
  motivo_devolucion: null,
  observaciones: null,
  requiere_actualizacion_avaluos: null,
  homologacion: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

export const normalizeRevisarDocumentosInmueble = (
  source: Partial<RevisarDocumentosInmuebleFormulario> | null | undefined,
  id_expediente_fallback: number,
): RevisarDocumentosInmuebleFormulario => ({
  id: Number(source?.id ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  id_actividad: source?.id_actividad ?? ACTIVIDAD_ID_REVISAR_DOCUMENTOS_INMUEBLE,
  documentos_correctos: source?.documentos_correctos ?? null,
  motivo_devolucion: source?.motivo_devolucion ?? null,
  observaciones: source?.observaciones ?? null,
  requiere_actualizacion_avaluos: source?.requiere_actualizacion_avaluos ?? null,
  homologacion: source?.homologacion ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? new Date().toISOString(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

/**
 * Replica el orden de validación del backend en /avanzar (sección 2.4 de la
 * especificación): documentos_correctos es obligatorio, y motivo_devolucion
 * solo se exige cuando documentos_correctos = false. Observaciones NO es
 * obligatorio para avanzar según la especificación.
 */
export function validateAvanzarFields(
  form: Pick<RevisarDocumentosInmuebleFormulario, 'documentos_correctos' | 'motivo_devolucion'>,
): Set<string> {
  const missing = new Set<string>();

  if (form.documentos_correctos === null || form.documentos_correctos === undefined) {
    missing.add('documentos_correctos');
    return missing;
  }

  if (form.documentos_correctos === false && !form.motivo_devolucion?.trim()) {
    missing.add('motivo_devolucion');
  }

  return missing;
}