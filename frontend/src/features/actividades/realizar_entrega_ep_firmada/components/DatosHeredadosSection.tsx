import type { DatosHeredadosEntregaEp } from '../models/realizar_entrega_ep_firmada';

interface Props {
  datosHeredados: DatosHeredadosEntregaEp | null;
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
      <div className="md:col-span-3 flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Concepto Firma Rep. Legal</span>
        <span className="text-sm font-semibold text-green-700">
          {datosHeredados.concepto_firma_descripcion
            ?? datosHeredados.concepto_firma
            ?? '-'}
        </span>
      </div>

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

      <div className="flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Representante Legal</span>
        <span className="text-sm font-medium text-gray-800">
          {datosHeredados.representante_legal ?? '-'}
        </span>
      </div>
    </div>
  );
}
