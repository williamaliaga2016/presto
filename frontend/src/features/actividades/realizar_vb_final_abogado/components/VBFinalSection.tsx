import { Dropdown } from 'primereact/dropdown';
import { InputSwitch } from 'primereact/inputswitch';
import { InputTextarea } from 'primereact/inputtextarea';
import type { ControlBaseDTO } from '@/core/api/models/ControlBaseDTO';
import type { RealizarVBFinalAbogado } from '../models/realizar_vb_final_abogado';

interface Props {
  form: RealizarVBFinalAbogado;
  isDisabled: boolean;
  updateField: <K extends keyof RealizarVBFinalAbogado>(field: K, value: RealizarVBFinalAbogado[K]) => void;
  tipologiaOptions: ControlBaseDTO[];
  casuisticaOptions: ControlBaseDTO[];
  onRequiereDevolucionChange: (value: string) => void;
  onTipologiaChange: (value: string) => void;
}

export default function VBFinalSection({ form, isDisabled, updateField, tipologiaOptions, casuisticaOptions, onRequiereDevolucionChange, onTipologiaChange }: Props) {
  const requiereDevolucion = form.requiere_devolucion === 'SI';

  return (
    <div className="space-y-4">
      {/* Toggle ¿Requiere Devolución? */}
      <div className="flex items-center justify-between">
        <label className="text-sm font-medium text-gray-700">¿Requiere Devolución? *</label>
        <div className="flex items-center gap-2">
          <InputSwitch
            checked={requiereDevolucion}
            onChange={(e) => onRequiereDevolucionChange(e.value ? 'SI' : 'NO')}
            disabled={isDisabled}
          />
          <span className="text-sm font-semibold text-gray-700">
            {requiereDevolucion ? 'SI' : 'NO'}
          </span>
        </div>
      </div>

      {/* Campos condicionales: solo si requiere devolución */}
      {requiereDevolucion && (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-x-4 gap-y-4">
          <div className="flex flex-col gap-1">
            <label className="text-xs font-medium text-gray-700">Tipología *</label>
            <Dropdown
              value={form.tipologia}
              options={tipologiaOptions}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => onTipologiaChange(e.value)}
              disabled={isDisabled}
              placeholder="Seleccione..."
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="text-xs font-medium text-gray-700">Casuística *</label>
            <Dropdown
              value={form.casuistica}
              options={casuisticaOptions}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateField('casuistica', e.value)}
              disabled={isDisabled}
              placeholder="Seleccione..."
            />
          </div>
        </div>
      )}

      {/* Observaciones */}
      <div className="flex flex-col gap-1">
        <label className="text-xs font-medium text-gray-700">
          Observaciones {requiereDevolucion ? '*' : ''}
        </label>
        <InputTextarea
          value={form.observaciones ?? ''}
          onChange={(e) => updateField('observaciones', e.target.value || null)}
          disabled={isDisabled}
          rows={3}
          placeholder={requiereDevolucion ? 'Detalle los motivos legales del rechazo...' : 'Observaciones opcionales...'}
        />
      </div>
    </div>
  );
}
