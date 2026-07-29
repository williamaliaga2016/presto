import type { Auditoria } from '@/models/Auditoria';

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REVISAR_MARCACION_COBERTURA';

// ─── Modelo principal (formulario editable) ─────────────────────────────────

export interface RevisionMarcacionCobertura extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;

  // CA05 — Notificación a Colocaciones (bloqueante, CA08)
  email_area_colocaciones: string | null;

  // Identificación
  consecutivo: string | null;
  tipo_documento: string | null;
  numero_documento: string | null;
  tipo_tramite: string | null;
  nombre: string | null;

  // Proyecto / Constructora (Diligencia SITCAR)
  constructora: string | null;
  proyecto: string | null;
  fecha_aceptacion_plataforma: string | null;
  tipo_vivienda: string | null;

  // Valores y obligación
  valor_subsidio: number | null;
  numero_obligacion: string | null;
  fecha_desembolso: string | null;
  fecha_proximo_canon: string | null;
  valor_desembolso: number | null;
  intereses_corrientes: number | null;
  capital: number | null;
  seguros: number | null;
  cuota_mensual: number | null;
  plazo: number | null;
  observacion: string | null;

  // Solicitud / Respuesta de marcación (SITCAR)
  fecha_solicitud_marcacion: string | null;
  hora_solicitud_marcacion: string | null;
  fecha_respuesta_marcacion: string | null;
  hora_respuesta_marcacion: string | null;
  responsable_m5: string | null;

  // Resolución
  no_resolucion: string | null;
  fecha_resolucion: string | null;
  fecha_envio_resolucion: string | null;
  estado_proceso: string | null;

  // CA07 — Observaciones generales
  observaciones: string | null;
}

// ─── Datos heredados del encabezado (solo lectura, CA02) ────────────────────

export interface RevisionMarcacionCoberturaHerencia {
  nombre_cliente: string | null;
  numero_identificacion: string | null;
  tipo_identificacion: string | null;
}

// ─── Respuesta del GET ────────────────────────────────────────────────────────

export interface RevisionMarcacionCoberturaResponse {
  formulario: RevisionMarcacionCobertura;
  herencia: RevisionMarcacionCoberturaHerencia | null;
}

// ─── Valor vacío por defecto ─────────────────────────────────────────────────

export const EMPTY_REVISION_MARCACION_COBERTURA = (
  id_expediente: number,
): RevisionMarcacionCobertura => ({
  id: 0,
  id_expediente,
  id_actividad: ACTIVITY_ID,
  email_area_colocaciones: null,
  consecutivo: null,
  tipo_documento: null,
  numero_documento: null,
  tipo_tramite: null,
  nombre: null,
  constructora: null,
  proyecto: null,
  fecha_aceptacion_plataforma: null,
  tipo_vivienda: null,
  valor_subsidio: null,
  numero_obligacion: null,
  fecha_desembolso: null,
  fecha_proximo_canon: null,
  valor_desembolso: null,
  intereses_corrientes: null,
  capital: null,
  seguros: null,
  cuota_mensual: null,
  plazo: null,
  observacion: null,
  fecha_solicitud_marcacion: null,
  hora_solicitud_marcacion: null,
  fecha_respuesta_marcacion: null,
  hora_respuesta_marcacion: null,
  responsable_m5: null,
  no_resolucion: null,
  fecha_resolucion: null,
  fecha_envio_resolucion: null,
  estado_proceso: null,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});

// ─── Reglas de negocio (fuente única de verdad) ─────────────────────────────

/**
 * CA04: campos obligatorios del modelado de datos. Los condicionados
 * (Fecha/Hora Respuesta Marcación) deben ir ambos o ninguno.
 */
export function camposObligatoriosFaltantes(form: RevisionMarcacionCobertura): string[] {
  const missing: string[] = [];

  if (!form.tipo_documento) missing.push('Tipo de Documento');
  if (!form.numero_documento?.trim()) missing.push('C.C (Número)');
  if (!form.tipo_tramite?.trim()) missing.push('TT (Tipo Trámite)');
  if (!form.nombre?.trim()) missing.push('Nombre');
  if (!form.constructora?.trim()) missing.push('Constructora');
  if (!form.proyecto?.trim()) missing.push('Proyecto');
  if (!form.fecha_aceptacion_plataforma) missing.push('Fecha de Aceptación Plataforma');
  if (!form.tipo_vivienda) missing.push('Tipo de Vivienda');
  if (form.valor_subsidio == null) missing.push('Valor Subsidio');
  if (!form.numero_obligacion?.trim()) missing.push('N° Obligación');
  if (!form.fecha_desembolso) missing.push('Fecha de Desembolso');
  if (!form.fecha_proximo_canon) missing.push('Fecha Próximo Canon');
  if (form.valor_desembolso == null) missing.push('Valor Desembolso');
  if (!form.fecha_solicitud_marcacion) missing.push('Fecha Solicitud Marcación');
  if (!form.hora_solicitud_marcacion?.trim()) missing.push('Hora Solicitud Marcación');
  if (!form.responsable_m5?.trim()) missing.push('Responsable M5');
  if (!form.no_resolucion?.trim()) missing.push('No Resolución');
  if (!form.fecha_resolucion) missing.push('Fecha de la Resolución');
  if (!form.fecha_envio_resolucion) missing.push('Fecha Envío Resolución');
  if (!form.estado_proceso) missing.push('Estado Proceso');
  if (!form.observaciones?.trim()) missing.push('Observaciones');

  const tieneFecha = !!form.fecha_respuesta_marcacion;
  const tieneHora = !!form.hora_respuesta_marcacion?.trim();
  if (tieneFecha !== tieneHora) {
    missing.push('Fecha y Hora de Respuesta Marcación (ambas o ninguna)');
  }

  return missing;
}

/** CA08: el correo del Área de Colocaciones es obligatorio y debe tener formato válido. */
export function emailColocacionesInvalido(
  email: string | null,
  isValidEmail: (value?: string | null) => boolean,
): boolean {
  return !email?.trim() || !isValidEmail(email);
}
