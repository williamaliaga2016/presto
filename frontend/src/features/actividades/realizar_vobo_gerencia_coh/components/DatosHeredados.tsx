/**
 * DatosHeredados
 *
 * Muestra en modo estrictamente de solo lectura todos los campos heredados
 * de la actividad previa "Realizar Excepción Desembolso".
 *
 * - Ningún campo es editable ni incluye controles de edición (Req 2.1).
 * - Los campos con valor `null` se representan como guion "-" (Req 2.3).
 * - El campo booleano `confirmacion_excepcion` se muestra como "Sí" / "No" / "-".
 *
 * Requisitos: 2.1, 2.2, 2.3
 */
import type { VoboGerenciaCohHerencia } from '../models/vobo_gerencia_coh';

// ─── Helper ──────────────────────────────────────────────────────────────────

/**
 * Campo de solo lectura.
 * Siempre muestra la etiqueta; si no hay valor muestra "-" (Req 2.3).
 */
function ReadonlyField({
  label,
  value,
}: {
  label: string;
  value: string | null | undefined;
}) {
  return (
    <div className="flex flex-col gap-0.5">
      <span className="text-xs font-semibold uppercase tracking-wide text-gray-500">
        {label}
      </span>
      <span className="text-sm font-medium text-gray-800">
        {value ?? '-'}
      </span>
    </div>
  );
}

// ─── Props ───────────────────────────────────────────────────────────────────

interface DatosHeredadosProps {
  herencia: VoboGerenciaCohHerencia | null;
}

// ─── Componente ──────────────────────────────────────────────────────────────

export default function DatosHeredados({ herencia }: DatosHeredadosProps) {
  if (!herencia) return null;

  // Convertir booleano a texto legible (Req 2.3)
  const confirmacionTexto =
    herencia.confirmacion_excepcion === true
      ? 'Sí'
      : herencia.confirmacion_excepcion === false
        ? 'No'
        : null;

  return (
    <div className="space-y-6">

      {/* ── Datos del cliente (Req 2.2) ─────────────────────────────────── */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Datos del Cliente
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-3">
          <ReadonlyField
            label="Nombre del Cliente"
            value={herencia.nombre_cliente}
          />
          <ReadonlyField
            label="Número de Identificación"
            value={herencia.numero_identificacion}
          />
          <ReadonlyField
            label="Tipo de Identificación"
            value={herencia.tipo_identificacion}
          />
        </div>
      </div>

      {/* ── Datos de la notaría (Req 2.2) ───────────────────────────────── */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Datos de la Notaría
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-x-4 gap-y-3">
          <ReadonlyField
            label="Nombre de la Notaría"
            value={herencia.nombre_notaria}
          />
          <ReadonlyField
            label="Ciudad"
            value={herencia.ciudad_notaria}
          />
        </div>
      </div>

      {/* ── Datos de la excepción (Req 2.2) ─────────────────────────────── */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Datos de la Excepción
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-x-4 gap-y-3">
          <ReadonlyField
            label="Excepción Autorizada"
            value={herencia.excepcion_autorizada}
          />
          <ReadonlyField
            label="Requiere VoBo Gerencia"
            value={herencia.requiere_vobo_gerencia}
          />
          <ReadonlyField
            label="Confirmación de Excepción"
            value={confirmacionTexto}
          />
          <ReadonlyField
            label="Observaciones de Excepción"
            value={herencia.observaciones_excepcion}
          />
        </div>
      </div>

    </div>
  );
}
