/**
 * SeccionNotificacionColocaciones
 *
 * Sección independiente "Emisión de Notificación a Colocaciones" (CA05).
 * El correo es obligatorio y debe tener formato válido para poder avanzar
 * (CA08) — el disparo del envío ocurre en el backend al accionar "Avanzar".
 */
import InputTextForm from '@/shared/components/InputTextForm';

interface SeccionNotificacionColocacionesProps {
  email: string | null;
  onChange: (value: string | null) => void;
  disabled?: boolean;
  invalid?: boolean;
}

export default function SeccionNotificacionColocaciones({
  email,
  onChange,
  disabled = false,
  invalid = false,
}: SeccionNotificacionColocacionesProps) {
  return (
    <div>
      <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
        Emisión de Notificación a Colocaciones
      </h3>
      <div className="md:w-1/2 flex flex-col gap-1.5">
        <InputTextForm
          label="Correo Área de Colocaciones"
          value={email ?? ''}
          onChange={(v) => onChange(v || null)}
          placeholder="area.colocaciones@bbva.com"
          required
          disabled={disabled}
        />
        {invalid && (
          <span role="alert" className="text-xs text-red-600">
            El correo es obligatorio y debe tener un formato de email válido.
          </span>
        )}
      </div>
    </div>
  );
}
