/**
 * ConfirmacionExcepcionCheckbox
 *
 * Checkbox obligatorio "Confirmar continuación con excepción de desembolso".
 * Se presenta como una tarjeta destacada para mayor visibilidad (Req 4.1, 4.3).
 */
import { Checkbox } from 'primereact/checkbox';

// ─── Props ────────────────────────────────────────────────────────────────────

interface ConfirmacionExcepcionCheckboxProps {
  /** Estado marcado/desmarcado del checkbox */
  checked: boolean;
  /** Callback al cambiar el estado */
  onChange: (checked: boolean) => void;
  /** Deshabilitar el control */
  disabled?: boolean;
  /** Mostrar estado de error (campo obligatorio no marcado) */
  hasError?: boolean;
}

// ─── Componente ──────────────────────────────────────────────────────────────

export default function ConfirmacionExcepcionCheckbox({
  checked,
  onChange,
  disabled = false,
  hasError = false,
}: ConfirmacionExcepcionCheckboxProps) {
  return (
    <div
      className={[
        // Base: tarjeta con borde izquierdo grueso
        'flex items-start gap-4 rounded-lg border-l-4 px-5 py-4 transition-colors',
        // Estado normal vs error vs marcado
        hasError
          ? 'border-l-red-500 bg-red-50 border border-red-300'
          : checked
            ? 'border-l-green-500 bg-green-50 border border-green-200'
            : 'border-l-blue-500 bg-blue-50 border border-blue-200',
        // Opacidad cuando está deshabilitado
        disabled ? 'opacity-60 cursor-not-allowed' : 'cursor-pointer',
      ].join(' ')}
      onClick={() => !disabled && onChange(!checked)}
      role="checkbox"
      aria-checked={checked}
      aria-disabled={disabled}
    >
      {/* Checkbox PrimeReact — tamaño aumentado vía scale */}
      <div className="mt-0.5 scale-125 origin-top-left flex-shrink-0">
        <Checkbox
          inputId="confirmacion_excepcion"
          checked={checked}
          onChange={(e) => onChange(e.checked ?? false)}
          disabled={disabled}
          className={hasError ? 'p-invalid' : ''}
          // Detener propagación para no doble-disparar el onClick del contenedor
          onClick={(e) => e.stopPropagation()}
        />
      </div>

      {/* Texto + mensaje de error */}
      <div className="flex flex-col gap-1 select-none">
        <label
          htmlFor="confirmacion_excepcion"
          className={[
            'text-sm font-semibold leading-snug cursor-pointer',
            hasError
              ? 'text-red-700'
              : checked
                ? 'text-green-800'
                : 'text-blue-900',
          ].join(' ')}
        >
          Confirmar continuación con excepción de desembolso
          <span className="ml-1 text-red-500">*</span>
        </label>

        <p className={[
          'text-xs leading-relaxed',
          hasError ? 'text-red-600' : 'text-slate-500',
        ].join(' ')}>
          {hasError
            ? 'La confirmación de excepción es obligatoria para continuar.'
            : 'Al marcar esta casilla confirma que autoriza la continuación del trámite con excepción de desembolso.'}
        </p>
      </div>
    </div>
  );
}
