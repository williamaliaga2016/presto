/**
 * DatosHeredados
 *
 * Muestra en modo estrictamente de solo lectura los datos generales de la
 * solicitud (CA02), derivados del encabezado del expediente.
 *
 * No existe una actividad predecesora real construida todavía ("Validar
 * Cumplimiento de Políticas" / BBV-104), así que la herencia se limita a la
 * información del encabezado en vez de datos de un formulario previo
 * (ver sección 0.1 de contexto/BBV-107_RECETA_IMPLEMENTACION.md).
 */
import type { RevisionMarcacionCoberturaHerencia } from '../models/revision_marcacion_cobertura';

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

interface DatosHeredadosProps {
  herencia: RevisionMarcacionCoberturaHerencia | null;
}

export default function DatosHeredados({ herencia }: DatosHeredadosProps) {
  if (!herencia) return null;

  return (
    <div>
      <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
        Datos del Cliente
      </h4>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-3">
        <ReadonlyField label="Nombre del Cliente" value={herencia.nombre_cliente} />
        <ReadonlyField label="Número de Identificación" value={herencia.numero_identificacion} />
        <ReadonlyField label="Tipo de Identificación" value={herencia.tipo_identificacion} />
      </div>
    </div>
  );
}
