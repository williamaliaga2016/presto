import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';
import { InputSwitch } from 'primereact/inputswitch';
import { Tooltip } from 'primereact/tooltip';
import type { ControlBaseDTO } from '@/core/api/models/ControlBaseDTO';
import type { RealizarRecepcionBoleta } from '../models/realizar_recepcion_boleta';

interface Props {
  form: RealizarRecepcionBoleta;
  isDisabled: boolean;
  updateField: <K extends keyof RealizarRecepcionBoleta>(field: K, value: RealizarRecepcionBoleta[K]) => void;
  tipoBoletaOptions: ControlBaseDTO[];
  oficinaOptions: ControlBaseDTO[];
}

export default function RecepcionBoletaSection({ form, isDisabled, updateField, tipoBoletaOptions, oficinaOptions }: Props) {
  const esFisica = form.tipo_boleta === 'TBOL-2' || form.tipo_boleta === 'TBOL-3';

  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-4">
      {/* Tipo de Boleta */}
      <div className="flex flex-col gap-1">
        <label className="text-xs font-medium text-gray-700">Tipo de Boleta *</label>
        <Dropdown
          value={form.tipo_boleta}
          options={tipoBoletaOptions}
          optionLabel="description"
          optionValue="code"
          onChange={(e) => updateField('tipo_boleta', e.value)}
          disabled={isDisabled}
          placeholder="Seleccione..."
        />
      </div>

      {/* Boleta En Poder De (condicional CA11) */}
      {esFisica && (
        <div className="flex flex-col gap-1">
          <label className="text-xs font-medium text-gray-700" id="tooltip-boleta-poder">
            Boleta En Poder De *
            <i className="pi pi-info-circle ml-1 text-blue-500 cursor-pointer" data-pr-tooltip="Indicar la Oficina BBVA quien tiene el resguardo de la boleta." />
          </label>
          <Tooltip target="[data-pr-tooltip]" />
          <InputText
            value={form.boleta_en_poder_de ?? ''}
            onChange={(e) => updateField('boleta_en_poder_de', e.target.value || null)}
            disabled={isDisabled}
            placeholder="Oficina BBVA con resguardo"
          />
        </div>
      )}

      {/* Código Zona */}
      <div className="flex flex-col gap-1">
        <label className="text-xs font-medium text-gray-700">Código Zona *</label>
        <InputText
          value={form.codigo_zona ?? ''}
          onChange={(e) => updateField('codigo_zona', e.target.value || null)}
          disabled={isDisabled}
          placeholder="Código de zona"
        />
      </div>

      {/* Oficina de Registro */}
      <div className="flex flex-col gap-1">
        <label className="text-xs font-medium text-gray-700">Oficina de Registro *</label>
        <Dropdown
          value={form.oficina_registro}
          options={oficinaOptions}
          optionLabel="description"
          optionValue="code"
          onChange={(e) => updateField('oficina_registro', e.value)}
          disabled={isDisabled}
          placeholder="Buscar oficina..."
          filter
          filterPlaceholder="Escriba para buscar"
        />
      </div>

      {/* Boleta Recibida */}
      <div className="flex flex-col gap-1 justify-center">
        <label className="text-xs font-medium text-gray-700">Boleta Recibida</label>
        <InputSwitch
          checked={form.boleta_recibida}
          onChange={(e) => updateField('boleta_recibida', e.value ?? false)}
          disabled={isDisabled}
        />
      </div>

      {/* ¿Aplica Excepción Desembolso? (readonly) */}
      <div className="flex flex-col gap-1 justify-center">
        <label className="text-xs font-medium text-gray-700">¿Aplica Excepción Desembolso?</label>
        <InputSwitch
          checked={form.aplica_excepcion === 'SI'}
          disabled
        />
      </div>

      {/* Observaciones */}
      <div className="flex flex-col gap-1 md:col-span-3">
        <label className="text-xs font-medium text-gray-700">Observaciones</label>
        <InputTextarea
          value={form.observaciones ?? ''}
          onChange={(e) => updateField('observaciones', e.target.value || null)}
          disabled={isDisabled}
          rows={3}
          placeholder="Observaciones opcionales..."
        />
      </div>
    </div>
  );
}
