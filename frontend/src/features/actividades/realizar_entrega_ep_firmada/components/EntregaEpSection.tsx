import InputTextForm from '@/shared/components/InputTextForm';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import type { RealizarEntregaEpFirmada } from '../models/realizar_entrega_ep_firmada';

interface Props {
  form: RealizarEntregaEpFirmada;
  isDisabled: boolean;
  updateField: <K extends keyof RealizarEntregaEpFirmada>(
    field: K,
    value: RealizarEntregaEpFirmada[K],
  ) => void;
}

export default function EntregaEpSection({
  form,
  isDisabled,
  updateField,
}: Props) {
  return (
    <>
      {/* Entregado a */}
      <InputTextForm
        label="Entregado a"
        value={form.entregado_a ?? ''}
        onChange={(val) => updateField('entregado_a', val || null)}
        maxLength={200}
        placeholder="Nombre de la persona o tramitador que recibe la EP"
        disabled={isDisabled}
        required
      />

      {/* ¿Aplica Excepción? — solo lectura, calculado en backend */}
      <div className="flex flex-col gap-1.5">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
          ¿Aplica Excepción de Desembolso?
        </label>
        <div className={`px-3 py-2 rounded border text-sm font-medium ${
          form.aplica_excepcion === 'SI'
            ? 'bg-yellow-50 border-yellow-300 text-yellow-800'
            : 'bg-gray-50 border-gray-300 text-gray-700'
        }`}>
          {form.aplica_excepcion === 'SI' ? 'SÍ — Aplica excepción' :
           form.aplica_excepcion === 'NO' ? 'NO — No aplica' :
           'Pendiente de cálculo...'}
        </div>
        <span className="text-xs text-gray-500">
          Campo calculado automáticamente según el tipo de crédito del expediente.
        </span>
      </div>

      {/* Observaciones */}
      <div className="md:col-span-3 flex flex-col gap-1.5">
        <InputTextAreaForm
          label="Observaciones"
          value={form.observaciones ?? ''}
          onChange={(val) => updateField('observaciones', val || null)}
          maxLength={500}
          rows={4}
          placeholder="Notas sobre la entrega o la excepción (opcional)"
          disabled={isDisabled}
        />
      </div>
    </>
  );
}
