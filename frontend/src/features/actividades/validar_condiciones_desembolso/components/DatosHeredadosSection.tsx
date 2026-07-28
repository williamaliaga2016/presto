interface Props {
  datosHeredados: any | null;
}

const fmt = (v?: string | null) => v ? new Date(v).toLocaleDateString('es-CO') : '-';

export default function DatosHeredadosSection({ datosHeredados }: Props) {
  if (!datosHeredados) return <div className="text-sm text-gray-500 italic">No hay datos heredados disponibles.</div>;

  return (
    <div className="space-y-4">
      {/* Procedencia */}
      <div className="p-3 bg-blue-50 border border-blue-200 rounded">
        <span className="text-xs text-gray-500 uppercase">Procedencia del trámite</span>
        <p className="text-sm font-medium text-blue-700">{datosHeredados.origen_label ?? '-'}</p>
        {datosHeredados.concepto_juridico && (
          <p className="text-sm font-medium text-green-700 mt-1">{datosHeredados.concepto_juridico}</p>
        )}
        {datosHeredados.observaciones_abogado && (
          <p className="text-xs text-gray-600 mt-1">Observaciones: {datosHeredados.observaciones_abogado}</p>
        )}
      </div>

      {/* Datos Cliente */}
      {datosHeredados.tipo_documento && (
        <div>
          <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">Datos Cliente</h4>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-x-4 gap-y-2">
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Tipo Documento</span><span className="text-sm font-medium text-gray-800">{datosHeredados.tipo_documento ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nro. Documento</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_documento ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nombre Completo</span><span className="text-sm font-medium text-gray-800">{datosHeredados.nombre_completo ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Tipo Crédito</span><span className="text-sm font-medium text-gray-800">{datosHeredados.tipo_credito ?? '-'}</span></div>
          </div>
        </div>
      )}

      {/* Datos Notaría */}
      {datosHeredados.notaria && (
        <div>
          <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">Datos Notaría</h4>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-x-4 gap-y-2">
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Notaría</span><span className="text-sm font-medium text-gray-800">{datosHeredados.notaria ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nro. Notaría</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_notaria ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Ciudad</span><span className="text-sm font-medium text-gray-800">{datosHeredados.ciudad_notaria ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nro. Escritura</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_escritura ?? '-'}</span></div>
          </div>
        </div>
      )}

      {/* Datos Boleta */}
      {datosHeredados.numero_boleta && (
        <div>
          <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">Datos Recepción Boleta</h4>
          <div className="grid grid-cols-1 md:grid-cols-5 gap-x-4 gap-y-2">
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Radicado</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_boleta ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Fecha Boleta</span><span className="text-sm font-medium text-gray-800">{fmt(datosHeredados.fecha_boleta)}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Tipo Boleta</span><span className="text-sm font-medium text-gray-800">{datosHeredados.tipo_boleta ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Oficina Registro</span><span className="text-sm font-medium text-gray-800">{datosHeredados.oficina_registro ?? '-'}</span></div>
            <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Matrícula</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_matricula ?? '-'}</span></div>
          </div>
        </div>
      )}
    </div>
  );
}
