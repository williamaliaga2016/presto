import { Dropdown } from 'primereact/dropdown';
import { InputNumber } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';

import {
  type CatalogoOption,
  EMPTY_CONTROLES_PROPIEDAD,
  type ControlesPropiedad,
} from '../models/catalogo';
import type { RevisarDatosOperacionPropiedad } from '../models/revisar_datos_operacion';

type Props = {
  value: RevisarDatosOperacionPropiedad | null | undefined;
  disabled: boolean;
  controles?: ControlesPropiedad;
  loadingControles?: boolean;
  onChange: <K extends keyof RevisarDatosOperacionPropiedad>(
    field: K,
    value: RevisarDatosOperacionPropiedad[K],
  ) => void;
};

const fieldClass = 'flex flex-col gap-1';
const labelClass = 'font-semibold text-sm text-gray-700';
const inputClass = 'form-input-presto w-full';
const dropdownClass = 'form-dropdown-presto w-full';

const resolveCatalogCode = (
  options: CatalogoOption[],
  value?: string | null,
): string | null => {
  if (!value) return null;

  const normalizedValue = String(value).trim();
  const option = options.find(
    (item) =>
      String(item.code ?? '').trim() === normalizedValue ||
      String(item.description ?? '').trim() === normalizedValue,
  );

  return option?.code ?? normalizedValue;
};

export default function PropiedadSection({
  value,
  disabled,
  controles = EMPTY_CONTROLES_PROPIEDAD,
  loadingControles = false,
  onChange,
}: Props) {
  const tipoPropiedadOptions = controles.tipo_propiedad;
  const estadoPropiedadOptions = controles.estado_propiedad;
  const tipoVentaOptions = controles.tipo_venta;
  const tipoConstruccionOptions = controles.tipo_construccion;
  const tipoDireccionOptions = controles.tipo_direccion;
  const existeRolAvaluoOptions = controles.existe_rol_avaluo;
  const regionOptions = controles.region;
  const comunaOptions = controles.comuna;
  const controlDisabled = disabled || loadingControles;

  return (
    <div className="space-y-5">
      <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
        <div className="mb-4 border-b border-gray-100 pb-3">
          <h3 className="text-base font-semibold text-gray-800">Datos de la Propiedad</h3>
          <p className="text-sm text-gray-500">Clasificación, estado y ubicación de la propiedad.</p>
        </div>

        <div className="grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
          <div className={fieldClass}>
            <label className={labelClass}>Tipo Propiedad *</label>
            <Dropdown
              value={value?.tipo_propiedad ?? null}
              options={tipoPropiedadOptions}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('tipo_propiedad', e.value)}
              showClear
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Estado *</label>
            <Dropdown
              value={value?.estado ?? null}
              options={estadoPropiedadOptions}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('estado', e.value)}
              showClear
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Tipo Venta *</label>
            <Dropdown
              value={value?.tipo_venta ?? null}
              options={tipoVentaOptions}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('tipo_venta', e.value)}
              showClear
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Tipo Construcción *</label>
            <Dropdown
              value={value?.tipo_construccion ?? null}
              options={tipoConstruccionOptions}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('tipo_construccion', e.value)}
              showClear
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Tipo Dirección *</label>
            <Dropdown
              value={value?.tipo_direccion ?? null}
              options={tipoDireccionOptions}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('tipo_direccion', e.value)}
              showClear
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Existe Rol Avalúo *</label>
            <Dropdown
              value={value?.existe_rol_avaluo ?? null}
              options={existeRolAvaluoOptions}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('existe_rol_avaluo', e.value)}
              showClear
            />
          </div>
        </div>
      </div>

      <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
        <div className="mb-4 border-b border-gray-100 pb-3">
          <h3 className="text-base font-semibold text-gray-800">Dirección y Rol de Avalúo</h3>
          <p className="text-sm text-gray-500">Detalle de dirección, identificación y valor de avalúo.</p>
        </div>

        <div className="grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
          <div className="flex flex-col gap-1 xl:col-span-3">
            <label className={labelClass}>Dirección *</label>
            <InputText
              value={value?.direccion ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('direccion', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Villa / Condominio *</label>
            <InputText
              value={value?.villa_condominio ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('villa_condominio', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Número *</label>
            <InputText
              value={value?.numero ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('numero', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>N° Casa Habitantes *</label>
            <InputText
              value={value?.numero_casa_habitantes ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('numero_casa_habitantes', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Conjunto *</label>
            <InputText
              value={value?.conjunto ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('conjunto', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Manzana *</label>
            <InputText
              value={value?.manzana ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('manzana', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Lote *</label>
            <InputText
              value={value?.lote ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('lote', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Región *</label>
            <Dropdown
              value={resolveCatalogCode(regionOptions, value?.region)}
              options={regionOptions}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('region', e.value)}
              showClear
              filter
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Comuna *</label>
            <Dropdown
              value={resolveCatalogCode(comunaOptions, value?.comuna)}
              options={comunaOptions}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('comuna', e.value)}
              showClear
              filter
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Valor Avalúo Pesos *</label>
            <InputNumber
              value={value?.valor_avaluo_pesos ?? null}
              disabled={controlDisabled}
              className={inputClass}
              inputClassName="w-full"
              mode="decimal"
              minFractionDigits={0}
              maxFractionDigits={2}
              onValueChange={(e) => onChange('valor_avaluo_pesos', e.value ?? null)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Rol Avalúo 1 *</label>
            <InputText
              value={value?.rol_avaluo_1 ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('rol_avaluo_1', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Rol Avalúo 2 *</label>
            <InputText
              value={value?.rol_avaluo_2 ?? ''}
              disabled={controlDisabled}
              className={inputClass}
              onChange={(e) => onChange('rol_avaluo_2', e.target.value)}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
