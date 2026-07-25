/**
 * SeccionDecision
 *
 * Encapsula la sección "Decisión de Excepción" del formulario de Excepción Desembolso.
 * Componente controlado: todos los valores y sus manejadores de cambio se reciben por props.
 *
 * Campos:
 * - "Excepción Autorizada" — dropdown SÍ/NO, obligatorio, solo informativo (Req 3.1, 3.2)
 * - "¿Requiere VoBo Gerencia?" — dropdown SÍ/NO, obligatorio, siempre habilitado (Req 5.1, 5.2)
 * - "Observaciones de Excepción" — textarea opcional, máx 1000 caracteres con contador (Req 10.1, 10.2)
 * - "Confirmar continuación con excepción de desembolso" — checkbox obligatorio (Req 4.1)
 */
import DropdownForm from '@/shared/components/DropdownForm';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import type { CatalogoOption } from '@/models/CatalogoOption';
import ConfirmacionExcepcionCheckbox from './ConfirmacionExcepcionCheckbox';

// ─── Constantes ───────────────────────────────────────────────────────────────

const SI_NO_OPTIONS: CatalogoOption[] = [
  { code: 'SI', description: 'SÍ' },
  { code: 'NO', description: 'NO' },
];

// ─── Props ────────────────────────────────────────────────────────────────────

interface SeccionDecisionProps {
  /** Controlled value for "Excepción Autorizada" dropdown */
  excepcionAutorizada: 'SI' | 'NO' | null;
  /** Controlled value for "¿Requiere VoBo Gerencia?" dropdown */
  requiereVoboGerencia: 'SI' | 'NO' | null;
  /** Controlled value for "Confirmar continuación con excepción de desembolso" checkbox */
  confirmacionExcepcion: boolean;
  /** Controlled value for "Observaciones de Excepción" textarea */
  observacionesExcepcion: string | null;
  /** Change handler for excepcionAutorizada */
  onExcepcionAutorizadaChange: (value: 'SI' | 'NO' | null) => void;
  /** Change handler for requiereVoboGerencia */
  onRequiereVoboGerenciaChange: (value: 'SI' | 'NO' | null) => void;
  /** Change handler for confirmacionExcepcion */
  onConfirmacionExcepcionChange: (checked: boolean) => void;
  /** Change handler for observacionesExcepcion */
  onObservacionesExcepcionChange: (value: string | null) => void;
  /** When true, all inputs are disabled (busy state or load error) */
  disabled?: boolean;
  /**
   * When true, highlights the confirmation checkbox as an error field (Req 4.3).
   * Cleared externally once the user checks the box.
   */
  checkboxError?: boolean;
}

// ─── Componente ──────────────────────────────────────────────────────────────

export default function SeccionDecision({
  excepcionAutorizada,
  requiereVoboGerencia,
  confirmacionExcepcion,
  observacionesExcepcion,
  onExcepcionAutorizadaChange,
  onRequiereVoboGerenciaChange,
  onConfirmacionExcepcionChange,
  onObservacionesExcepcionChange,
  disabled = false,
  checkboxError = false,
}: SeccionDecisionProps) {
  return (
    <div>
      {/* Título de sección */}
      <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
        Decisión de Excepción
      </h3>

      {/* Grid de dos columnas: dropdowns (Req 3.1, 5.1) */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-x-4 gap-y-4 mb-6">
        {/* Excepción Autorizada — solo informativo, no controla otros campos (Req 3.1, 3.2) */}
        <DropdownForm
          label="Excepción Autorizada"
          value={excepcionAutorizada}
          options={SI_NO_OPTIONS}
          onChange={(v) => onExcepcionAutorizadaChange(v as 'SI' | 'NO' | null)}
          required
          disabled={disabled}
        />

        {/* ¿Requiere VoBo Gerencia? — siempre habilitado, nunca depende de excepcionAutorizada (Req 5.1, 5.2) */}
        <DropdownForm
          label="¿Requiere VoBo Gerencia?"
          value={requiereVoboGerencia}
          options={SI_NO_OPTIONS}
          onChange={(v) => onRequiereVoboGerenciaChange(v as 'SI' | 'NO' | null)}
          required
          disabled={disabled}
        />
      </div>

      {/* Observaciones de Excepción — opcional, máx 1000 caracteres, contador X/1000 (Req 10.1, 10.2) */}
      <div className="mb-6">
        <InputTextAreaForm
          label="Observaciones de Excepción"
          value={observacionesExcepcion ?? ''}
          onChange={(v) => onObservacionesExcepcionChange(v || null)}
          maxLength={1000}
          rows={4}
          disabled={disabled}
          showCounter
        />
      </div>

      {/* Checkbox de confirmación — obligatorio para avanzar (Req 4.1, 4.3) */}
      <ConfirmacionExcepcionCheckbox
        checked={confirmacionExcepcion}
        onChange={onConfirmacionExcepcionChange}
        disabled={disabled}
        hasError={checkboxError}
      />
    </div>
  );
}
