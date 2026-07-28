/**
 * SeccionDictamen
 *
 * Sección editable del dictamen del Gerente COH dentro del acordeón "Vobo Gerencia COH".
 * Componente controlado: todos los valores y sus manejadores se reciben por props.
 *
 * Campos:
 * - "Concepto" — dropdown "Favorable" / "No Favorable", sin valor preseleccionado (Req 3.1)
 * - "Observaciones" — textarea, máx 1 000 caracteres con contador N/1 000 (Req 3.2)
 *   · Se marca como obligatorio (*) cuando concepto_vobo === 'No Favorable' (Req 3.3)
 *   · El asterisco desaparece al cambiar a "Favorable" (Req 3.5)
 *   · Muestra mensaje de error inline cuando observacionesError === true (Req 3.6)
 *
 * Requisitos: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7
 */
import DropdownForm from '@/shared/components/DropdownForm';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import type { CatalogoOption } from '@/models/CatalogoOption';
import {
  CONCEPTO_VOBO,
  esObservacionesObligatoria,
  type ConceptoVoBo,
} from '../models/vobo_gerencia_coh';

// ─── Constantes ───────────────────────────────────────────────────────────────

const CONCEPTO_OPTIONS: CatalogoOption[] = [
  { code: CONCEPTO_VOBO.FAVORABLE, description: CONCEPTO_VOBO.FAVORABLE },
  { code: CONCEPTO_VOBO.NO_FAVORABLE, description: CONCEPTO_VOBO.NO_FAVORABLE },
];

const MAX_OBSERVACIONES = 1000;

// ─── Props ────────────────────────────────────────────────────────────────────

interface SeccionDictamenProps {
  /** Valor controlado del dropdown Concepto VoBo */
  concepto_vobo: ConceptoVoBo | null;
  /** Valor controlado del textarea Observaciones */
  observaciones: string | null;
  /** Handler de cambio para Concepto VoBo */
  onConceptoChange: (value: ConceptoVoBo | null) => void;
  /** Handler de cambio para Observaciones */
  onObservacionesChange: (value: string | null) => void;
  /** Cuando true, deshabilita todos los inputs (Req 1.5, 5.6) */
  disabled?: boolean;
  /**
   * Cuando true, muestra el mensaje de error inline bajo el campo Observaciones.
   * Aplica solo cuando concepto_vobo === 'No Favorable' y observaciones está vacío (Req 3.6).
   */
  observacionesError?: boolean;
}

// ─── Componente ──────────────────────────────────────────────────────────────

export default function SeccionDictamen({
  concepto_vobo,
  observaciones,
  onConceptoChange,
  onObservacionesChange,
  disabled = false,
  observacionesError = false,
}: SeccionDictamenProps) {
  // Observaciones es obligatorio (para avanzar) cuando concepto es "No Favorable" (Req 3.3)
  const observacionesRequeridas = esObservacionesObligatoria(concepto_vobo);

  // Texto actual del textarea (nunca undefined para el controlled input)
  const textoObservaciones = observaciones ?? '';

  return (
    <div>
      {/* Título de sección */}
      <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
        Dictamen del Gerente COH
      </h3>

      {/* ── Concepto VoBo — dropdown sin valor preseleccionado (Req 3.1) ────── */}
      <div className="mb-6 md:w-1/2">
        <DropdownForm
          label="Concepto"
          value={concepto_vobo}
          options={CONCEPTO_OPTIONS}
          onChange={(v) => onConceptoChange(v as ConceptoVoBo | null)}
          required
          disabled={disabled}
          placeholder="Seleccionar..."
        />
      </div>

      {/* ── Observaciones — textarea con contador y error inline (Req 3.2, 3.6) */}
      <InputTextAreaForm
        id="seccion-dictamen-observaciones"
        label="Observaciones"
        value={textoObservaciones}
        onChange={(v) => onObservacionesChange(v || null)}
        rows={4}
        maxLength={MAX_OBSERVACIONES}
        placeholder="Ingrese las observaciones..."
        disabled={disabled}
        required={observacionesRequeridas}
        invalid={observacionesError && observacionesRequeridas}
        errorMessage='Las observaciones son obligatorias cuando el concepto es "No Favorable".'
      />
    </div>
  );
}
