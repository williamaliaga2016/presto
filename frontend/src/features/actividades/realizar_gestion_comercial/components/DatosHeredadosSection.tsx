interface Props { datosHeredados: any | null; }

export default function DatosHeredadosSection({ datosHeredados }: Props) {
  if (!datosHeredados) return <div className="text-sm text-gray-500 italic">No hay datos heredados disponibles.</div>;

  return (
    <div className="p-3 bg-gray-50 border border-gray-200 rounded space-y-3">
      <div className="flex flex-col gap-0.5">
        <span className="text-xs text-gray-500 uppercase">Origen del Escalamiento</span>
        <span className="text-sm font-medium text-blue-700">{datosHeredados.origen_label ?? '-'}</span>
      </div>

      {/* Datos según origen */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-2">
        {datosHeredados.tipo_credito && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Tipo Crédito</span><span className="text-sm font-medium text-gray-800">{datosHeredados.tipo_credito}</span></div>}
        {datosHeredados.nombre_completo && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Cliente</span><span className="text-sm font-medium text-gray-800">{datosHeredados.nombre_completo}</span></div>}
        {datosHeredados.notaria && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Notaría</span><span className="text-sm font-medium text-gray-800">{datosHeredados.notaria}</span></div>}
        {datosHeredados.numero_notaria && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nro. Notaría</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_notaria}</span></div>}
        {datosHeredados.ciudad_notaria && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Ciudad</span><span className="text-sm font-medium text-gray-800">{datosHeredados.ciudad_notaria}</span></div>}
        {datosHeredados.numero_escritura && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Nro. Escritura</span><span className="text-sm font-medium text-gray-800">{datosHeredados.numero_escritura}</span></div>}
        {datosHeredados.representante_legal && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Rep. Legal</span><span className="text-sm font-medium text-gray-800">{datosHeredados.representante_legal}</span></div>}
        {datosHeredados.tipologia_rechazo && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Tipología Rechazo</span><span className="text-sm font-medium text-gray-800">{datosHeredados.tipologia_rechazo}</span></div>}
        {datosHeredados.casuistica_rechazo && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Casuística</span><span className="text-sm font-medium text-gray-800">{datosHeredados.casuistica_rechazo}</span></div>}
        {datosHeredados.observaciones_abogado && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Obs. Abogado</span><span className="text-sm font-medium text-gray-800">{datosHeredados.observaciones_abogado}</span></div>}
        {datosHeredados.plan_pagos_confirmado !== undefined && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Plan Pagos</span><span className="text-sm font-medium text-gray-800">{datosHeredados.plan_pagos_confirmado ? 'Confirmado' : 'No confirmado'}</span></div>}
        {datosHeredados.requiere_escalamiento && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Escalamiento</span><span className="text-sm font-medium text-gray-800">{datosHeredados.requiere_escalamiento}</span></div>}
        {datosHeredados.observaciones_condiciones && <div className="flex flex-col gap-0.5"><span className="text-xs text-gray-500">Observaciones</span><span className="text-sm font-medium text-gray-800">{datosHeredados.observaciones_condiciones}</span></div>}
      </div>
    </div>
  );
}
