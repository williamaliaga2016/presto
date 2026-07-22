interface DatosHeredados {
  notaria?: string | null;
  numero_notaria?: number | null;
  ciudad_notaria?: string | null;
  fecha_notaria?: string | null;
  numero_escritura?: string | null;
  fecha_escritura?: string | null;
}

interface Props {
  datosHeredados: DatosHeredados | null;
}

const formatFecha = (value?: string | null) =>
  value ? new Date(value).toLocaleDateString('es-CO') : '-';

export default function DatosHeredadosSection({ datosHeredados }: Props) {
  if (!datosHeredados) {
    return (
      <div className="text-sm text-gray-500 italic">
        No hay datos heredados disponibles.
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-3">
      <div className="flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Notaría</span>
        <span className="text-sm font-medium text-gray-800">
          {datosHeredados.notaria ?? '-'}
        </span>
      </div>

      <div className="flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Número Notaría</span>
        <span className="text-sm font-medium text-gray-800">
          {datosHeredados.numero_notaria ?? '-'}
        </span>
      </div>

      <div className="flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Ciudad Notaría</span>
        <span className="text-sm font-medium text-gray-800">
          {datosHeredados.ciudad_notaria ?? '-'}
        </span>
      </div>

      <div className="flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Fecha Notaría</span>
        <span className="text-sm font-medium text-gray-800">
          {formatFecha(datosHeredados.fecha_notaria)}
        </span>
      </div>

      <div className="flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Número Escritura</span>
        <span className="text-sm font-medium text-gray-800">
          {datosHeredados.numero_escritura ?? '-'}
        </span>
      </div>

      <div className="flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Fecha Escritura</span>
        <span className="text-sm font-medium text-gray-800">
          {formatFecha(datosHeredados.fecha_escritura)}
        </span>
      </div>
    </div>
  );
}
