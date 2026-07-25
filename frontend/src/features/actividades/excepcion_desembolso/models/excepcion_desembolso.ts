import type { Auditoria } from '@/models/Auditoria';

// ─── Modelo principal de la actividad ────────────────────────────────────────

export interface ExcepcionDesembolso extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;

  /** Registro informativo: 'SI' | 'NO' | null (no afecta enrutamiento) */
  excepcion_autorizada: 'SI' | 'NO' | null;

  /** Compuerta de escalamiento: 'SI' → Gerente COH | 'NO' → Analista Vivienda */
  requiere_vobo_gerencia: 'SI' | 'NO' | null;

  /** Checkbox obligatorio para avanzar */
  confirmacion_excepcion: boolean;

  /** Observaciones opcionales, máx 1000 caracteres */
  observaciones_excepcion: string | null;

  /** Determinado automáticamente al cargar la pantalla */
  origen_caso: 'FIRMAR_REP_LEGAL' | 'RECEPCION_BOLETA' | null;
}

// ─── Respuesta del endpoint GET ───────────────────────────────────────────────

export interface ExcepcionDesembolsoHerencia {
  /** Concepto de firma exitosa */
  conceptoFirma: string | null;
  conceptoFirmaDesc: string | null;
  /** Fecha de ingreso a la oficina de registro. Solo presente si origen = RECEPCION_BOLETA */
  fechaBoleta: string | null;
  /** Número de radicado / boleta. Solo presente si origen = RECEPCION_BOLETA */
  numeroBoleta: string | null;
  /** Tipo de boleta. Solo presente si origen = RECEPCION_BOLETA */
  tipoBoleta: string | null;
  /** Oficina de registro. Solo presente si origen = RECEPCION_BOLETA */
  oficinaRegistro: string | null;

  esRecepcionBoleta: boolean;
}

export interface ExcepcionDesembolsoResponse {
  /** Datos editables del formulario */
  formulario: ExcepcionDesembolso;
  /** Campos heredados de la actividad previa. Null si el origen no pudo determinarse */
  herencia: ExcepcionDesembolsoHerencia | null;
  /** Origen determinado automáticamente: "FIRMAR_REP_LEGAL" | "RECEPCION_BOLETA" | null */
  origenCaso: string ;
}

// ─── Valor vacío por defecto ───────────────────────────────────────────────────

export const EMPTY_EXCEPCION_DESEMBOLSO = (
  id_expediente: number,
): ExcepcionDesembolso => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO',
  excepcion_autorizada: null,
  requiere_vobo_gerencia: null,
  confirmacion_excepcion: false,
  observaciones_excepcion: null,
  origen_caso: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
