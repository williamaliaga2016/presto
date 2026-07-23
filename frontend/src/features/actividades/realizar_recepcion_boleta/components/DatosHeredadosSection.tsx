import type { DatosHeredadosRecepcionBoleta } from '../models/realizar_recepcion_boleta';

interface Props {
  datosHeredados: DatosHeredadosRecepcionBoleta | null;
}

export default function DatosHeredadosSection({ datosHeredados }: Props) {
  if (!datosHeredados) {
    return (
      <div className="text-sm text-gray-500 italic">
        No hay datos heredados disponibles.
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {/* Datos Cliente */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">Datos Cliente</h4>
        <div className="grid grid-cols-1 md:grid-cols-4 gap-x-4 gap-y-2">
          <div className="flex flex-col gap-0.5">
            <span className="text-xs text-gray-500">Tipo Documento</span>
            <span className="text-sm font-medium text-gray-800">{datosHeredados.tipo_documento ?? '-'}</span>
          </div>
          <div className="flex flex-col gap-0.5">
            <span className="text-xs text-gray-500">Nro. Documento</span>
            <span className="text-sm font-medium text-gray-800">{datosHeredados.numero_documento ?? '-'}</span>
          </div>
          <div className="flex flex-col gap-0.5">
            <span className="text-xs text-gray-500">Nombre Completo</span>
            <span className="text-sm font-medium text-gray-800">{datosHeredados.nombre_completo ?? '-'}</span>
          </div>
          <div className="flex flex-col gap-0.5">
            <span className="text-xs text-gray-500">Tipo Crédito</span>
            <span className="text-sm font-medium text-gray-800">{datosHeredados.tipo_credito ?? '-'}</span>
          </div>
        </div>
      </div>

      {/* Datos Notaría */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">Datos Notaría</h4>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-2">
          <div className="flex flex-col gap-0.5">
            <span className="text-xs text-gray-500">Ciudad Notaría</span>
            <span className="text-sm font-medium text-gray-800">{datosHeredados.ciudad_notaria ?? '-'}</span>
          </div>
          <div className="flex flex-col gap-0.5">
            <span className="text-xs text-gray-500">Nro. Notaría</span>
            <span className="text-sm font-medium text-gray-800">{datosHeredados.numero_notaria ?? '-'}</span>
          </div>
          <div className="flex flex-col gap-0.5">
            <span className="text-xs text-gray-500">Número Escritura</span>
            <span className="text-sm font-medium text-gray-800">{datosHeredados.numero_escritura ?? '-'}</span>
          </div>
        </div>
      </div>
    </div>
  );
}
