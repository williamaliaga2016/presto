import type { Auditoria } from '@/models/Auditoria';

// ─── Concepto del dictamen (fuente única de verdad para el tipo y sus valores) ─

export type ConceptoVoBo = 'Favorable' | 'No Favorable';

export const CONCEPTO_VOBO = {
  FAVORABLE: 'Favorable',
  NO_FAVORABLE: 'No Favorable',
} as const satisfies Record<string, ConceptoVoBo>;

/**
 * Regla de negocio única (CA03): Observaciones es obligatorio para avanzar
 * cuando el concepto es "No Favorable". Reutilizada tanto para la validación
 * de "Avanzar" como para el indicador visual (*) del campo.
 */
export function esObservacionesObligatoria(concepto: ConceptoVoBo | null): boolean {
  return concepto === CONCEPTO_VOBO.NO_FAVORABLE;
}

// ─── Modelo principal (formulario editable) ─────────────────────────────────

export interface VoboGerenciaCoh extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  concepto_vobo: ConceptoVoBo | null;
  observaciones: string | null;
}

// ─── Datos heredados de Excepcion Desembolso (solo lectura) ─────────────────

export interface VoboGerenciaCohHerencia {
  nombre_cliente: string | null;
  numero_identificacion: string | null;
  tipo_identificacion: string | null;
  nombre_notaria: string | null;
  ciudad_notaria: string | null;
  excepcion_autorizada: 'SI' | 'NO' | null;
  requiere_vobo_gerencia: 'SI' | 'NO' | null;
  confirmacion_excepcion: boolean | null;
  observaciones_excepcion: string | null;
}

// ─── Respuesta del GET ────────────────────────────────────────────────────────

export interface VoboGerenciaCohResponse {
  formulario: VoboGerenciaCoh;
  herencia: VoboGerenciaCohHerencia | null;
}

// ─── Valor vacío por defecto ─────────────────────────────────────────────────

export const EMPTY_VOBO_GERENCIA_COH = (id_expediente: number): VoboGerenciaCoh => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH',
  concepto_vobo: null,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
