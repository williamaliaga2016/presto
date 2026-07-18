import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';

import type { DatosOperacionBancoAcreedor } from '../models/datos_operacion';
import {
  EMPTY_CONTROLES_BANCO_ACREEDOR,
  type ControlesBancoAcreedor,
} from '../models/catalogo';

type Props = {
  value: DatosOperacionBancoAcreedor | null | undefined;
  disabled: boolean;
  controles?: ControlesBancoAcreedor;
  loadingControles?: boolean;
  onChange: <K extends keyof DatosOperacionBancoAcreedor>(
    field: K,
    value: DatosOperacionBancoAcreedor[K],
  ) => void;
};

const boolToCatalogCode = (value?: boolean | null) => {
  if (value === null || value === undefined) return null;
  return value ? '001' : '002';
};

const catalogCodeToBool = (value?: string | null) => {
  if (!value) return null;
  return ['001', '01', '1', 'S'].includes(value);
};

const fieldClass = 'flex flex-col gap-1';
const labelClass = 'font-semibold text-sm text-gray-700';
const inputClass = 'form-input-presto w-full';
const dropdownClass = 'form-dropdown-presto w-full';

export default function BancoAcreedorSection({
  value,
  disabled,
  controles = EMPTY_CONTROLES_BANCO_ACREEDOR,
  loadingControles = false,
  onChange,
}: Props) {
  const controlDisabled = disabled || loadingControles;
  return (
    <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
      <div className="mb-4 border-b border-gray-100 pb-3">
        <h3 className="text-base font-semibold text-gray-800">Datos del Banco Acreedor</h3>
        <p className="text-sm text-gray-500">Información asociada a carta de resguardo, institución y reparos.</p>
      </div>

      <div className="grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
        <div className={fieldClass}>
          <label className={labelClass}>Cuenta Carta de Resguardo *</label>
          <Dropdown
            value={boolToCatalogCode(value?.cuenta_carta_resguardo)}
            options={controles.si_no}
            optionLabel="description"
            optionValue="code"
            disabled={disabled}
            className={dropdownClass}
            placeholder="Seleccione"
            onChange={(e) => onChange('cuenta_carta_resguardo', catalogCodeToBool(e.value))}
            showClear
          />
        </div>

        <div className={fieldClass}>
          <label className={labelClass}>Institución *</label>
          <Dropdown
            value={value?.institucion ?? null}
            options={controles.banco_acreedor_institucion}
            optionLabel="description"
            optionValue="code"
            disabled={disabled}
            className={dropdownClass}
            placeholder="Seleccione"
            onChange={(e) => onChange('institucion', e.value)}
            showClear
            filter
          />
        </div>

        <div className={fieldClass}>
          <label className={labelClass}>RUT Banco Acreedor *</label>
          <InputText
            value={value?.rut_banco_acreedor ?? ''}
            disabled={disabled}
            className={inputClass}
            placeholder="Ingrese RUT"
            onChange={(e) => onChange('rut_banco_acreedor', e.target.value)}
          />
        </div>
      </div>
    </div>
  );
}
