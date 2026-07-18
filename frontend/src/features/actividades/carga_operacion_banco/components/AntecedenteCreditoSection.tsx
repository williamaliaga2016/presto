import { Calendar } from 'primereact/calendar';
import { Dropdown } from 'primereact/dropdown';
import { InputNumber } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';

import type { ControlesAntecedenteCredito } from '../models/catalogo';
import type { CargaOperacionBancoAntecedenteCredito } from '../models/carga_operacion_banco';

interface AntecedenteCreditoSectionProps {
  value: CargaOperacionBancoAntecedenteCredito;
  disabled: boolean;
  controles: ControlesAntecedenteCredito;
  loadingControles?: boolean;
  onChange: <K extends keyof CargaOperacionBancoAntecedenteCredito>(
    field: K,
    value: CargaOperacionBancoAntecedenteCredito[K],
  ) => void;
}

const emptyMessage = 'Sin resultados';

const toDate = (value?: string | null): Date | null => {
  if (!value) return null;

  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? null : date;
};

const toIsoDate = (value: Date | null | undefined): string | null => {
  if (!value) return null;
  return value.toISOString();
};

export default function AntecedenteCreditoSection({
  value,
  disabled,
  controles,
  loadingControles = false,
  onChange,
}: AntecedenteCreditoSectionProps) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Id SubProducto</label>
        <Dropdown
          value={value.id_tipo_sub_producto ?? null}
          options={controles.tipo_subproducto}
          optionLabel="description"
          optionValue="code"
          onChange={(e) => onChange('id_tipo_sub_producto', e.value ?? null)}
          className="form-dropdown-presto w-full"
          disabled={disabled}
          loading={loadingControles}
          placeholder="Seleccione"
          emptyMessage={emptyMessage}
          showClear
          filter
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Monto Otorgado</label>
        <InputNumber
          value={value.monto_otorgado ?? null}
          onValueChange={(e) => onChange('monto_otorgado', e.value ?? null)}
          mode="currency"
          currency="COP"
          locale="es-CO"
          className="form-input-presto w-full"
          inputClassName="w-full"
          disabled={disabled}
          maxFractionDigits={0}
          placeholder="$0"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Plazo Otorgado (meses)</label>
        <InputNumber
          value={value.plazo ?? null}
          onValueChange={(e) => onChange('plazo', e.value ?? null)}
          className="form-input-presto w-full"
          inputClassName="w-full"
          useGrouping={false}
          disabled={disabled}
          placeholder="Ingrese plazo"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Tasa</label>
        <InputNumber
          value={value.tasa ?? null}
          onValueChange={(e) => onChange('tasa', e.value ?? null)}
          className="form-input-presto w-full"
          inputClassName="w-full"
          disabled={disabled}
          minFractionDigits={0}
          maxFractionDigits={6}
          placeholder="Ingrese tasa"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Fecha de Aprobación</label>
        <Calendar
          value={toDate(value.fecha_aprobacion)}
          onChange={(e) => onChange('fecha_aprobacion', toIsoDate(e.value as Date | null))}
          className="form-input-presto w-full"
          disabled={disabled}
          dateFormat="dd/mm/yy"
          showIcon
          placeholder="Seleccione fecha"
        />
      </div>

      <div className="flex flex-col gap-1 md:col-span-2">
        <label className="font-semibold text-sm">Condiciones Organismo Decisor</label>
        <InputText
          value={value.condiciones_organismo_decisor ?? ''}
          onChange={(e) => onChange('condiciones_organismo_decisor', e.target.value)}
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="Ingrese condiciones del organismo decisor"
        />
      </div>
    </div>
  );
}
