interface DatosHeredados {
  // Cliente
  tipo_documento?: string | null;
  numero_documento?: string | null;
  nombre_completo?: string | null;
  tipo_credito?: string | null;
  // Notaría
  notaria?: string | null;
  numero_notaria?: number | null;
  ciudad_notaria?: string | null;
  numero_escritura?: string | null;
  fecha_escritura?: string | null;
  // Boleta
  numero_boleta?: string | null;
  fecha_boleta?: string | null;
  tipo_boleta?: string | null;
  oficina_registro?: string | null;
  numero_matricula?: string | null;
  // EP Registradas
  confirmacion_ep_registrada?: boolean | null;
  finalizacion?: string | null;
  causal_ep?: string | null;
}

interface Props {
  datosHeredados: DatosHeredados | null;
  origenTramite: string | null;
}

const fmt = (v?: string | null) => v ? new Date(v).toLocaleDateString('es-CO') : '-';

export default function DatosHeredadosSection({ datosHeredados, origenTramite }: Props) {
  if (!datosHeredados) return <div className="text-sm text-gray-500 italic">No hay datos heredados disponibles.</div>;

  return (
    <div className="space-y-3">
      <div className="text-xs text-blue-600 font-medium">
        Origen: {origenTramite === 'CONTROL_GARANTIAS' ? 'Gestionar Control de Garantías' : 'Realizar EP Registradas'}
      </div>

      {/* Datos Cliente */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">Datos Cliente</h4>
        <div className="grid grid-cols-1 md:grid-cols-4 gap-x-4 gap-y-2">
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Tipo Documento</span><span className="text-sm font-medium text-gray-800">{datosHeredados.tipo_documento ?? '-'}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nro. Documento</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_documento ?? '-'}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nombre Completo</span><span className="text-sm font-medium text-gray-800">{datosHeredados.nombre_completo ?? '-'}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Tipo Crédito</span><span className="text-sm font-medium text-gray-800">{datosHeredados.tipo_credito ?? '-'}</span></div>
        </div>
      </div>

      {/* Datos Notaría */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">Datos Notaría</h4>
        <div className="grid grid-cols-1 md:grid-cols-5 gap-x-4 gap-y-2">
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Notaría</span><span className="text-sm font-medium text-gray-800">{datosHeredados.notaria ?? '-'}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nro. Notaría</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_notaria ?? '-'}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Ciudad</span><span className="text-sm font-medium text-gray-800">{datosHeredados.ciudad_notaria ?? '-'}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nro. Escritura</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_escritura ?? '-'}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Fecha Escritura</span><span className="text-sm font-medium text-gray-800">{fmt(datosHeredados.fecha_escritura)}</span></div>
        </div>
      </div>

      {/* Datos Boleta */}
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

      {/* EP Registradas */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">Confirmación EP Registrada</h4>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-2">
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">EP Registrada</span><span className="text-sm font-medium text-gray-800">{datosHeredados.confirmacion_ep_registrada ? 'Sí' : 'No'}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Finalización</span><span className="text-sm font-medium text-gray-800">{fmt(datosHeredados.finalizacion)}</span></div>
          <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Causal</span><span className="text-sm font-medium text-gray-800">{datosHeredados.causal_ep ?? '-'}</span></div>
        </div>
      </div>
    </div>
  );
}
