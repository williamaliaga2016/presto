import { Checkbox } from 'primereact/checkbox';
import { Dropdown } from 'primereact/dropdown';
import { InputNumber } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';

import type { DatosOperacionDatosCredito } from '../models/datos_operacion';
import {
  EMPTY_CONTROLES_DATOS_CREDITO,
  type ControlesDatosCredito,
} from '../models/catalogo';
import { RadioButton } from 'primereact/radiobutton';

type Props = {
  value: DatosOperacionDatosCredito | null | undefined;
  disabled: boolean;
  controles?: ControlesDatosCredito;
  loadingControles?: boolean;
  onChange: <K extends keyof DatosOperacionDatosCredito>(
    field: K,
    value: DatosOperacionDatosCredito[K],
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

export default function DatosCreditoSection({
  value,
  disabled,
  controles = EMPTY_CONTROLES_DATOS_CREDITO,
  loadingControles = false,
  onChange,
}: Props) {
  const controlDisabled = disabled || loadingControles;
  return (
    <div className="space-y-5">
      <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
        <div className="mb-4 border-b border-gray-100 pb-3">
          <h3 className="text-base font-semibold text-gray-800">Datos del Crédito</h3>
          <p className="text-sm text-gray-500">Información general del crédito asociado al expediente.</p>
        </div>

        <div className="grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
          <div className={fieldClass}>
  <label className={labelClass}>Ubicación *</label>

  <div className="flex flex-col gap-3 py-2">
    <div className="flex items-center gap-2">
      <RadioButton
        className="form-radio-presto"
        inputId="ubicacion_santiago"
        checked={value?.ubicacion === true}
        disabled={disabled}
        onChange={() => onChange('ubicacion', true)}
      />

      <label
        htmlFor="ubicacion_santiago"
        className="text-sm text-gray-700"
      >
        Santiago
      </label>
    </div>

    <div className="flex items-center gap-2">
      <RadioButton
        className="form-radio-presto"
        inputId="ubicacion_regiones"
        checked={value?.ubicacion === false}
        disabled={disabled}
        onChange={() => onChange('ubicacion', false)}
      />

      <label
        htmlFor="ubicacion_regiones"
        className="text-sm text-gray-700"
      >
        Regiones
      </label>
    </div>
  </div>
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Tipo Operación *</label>
            <Dropdown
              value={value?.tipo_operacion ?? null}
              options={controles.tipo_operacion}
              optionLabel="description"
              optionValue="code"
              disabled={controlDisabled}
              className={dropdownClass}
              placeholder="Seleccione"
              onChange={(e) => onChange('tipo_operacion', e.value)}
              showClear
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Fines Generales *</label>

            <div className="flex items-center gap-6 h-10">
              <div className="flex items-center gap-2">
                <RadioButton
                  className="form-radio-presto"
                  inputId="fines_generales_si"
                  checked={value?.fines_generales === true}
                  disabled={disabled}
                  onChange={() => onChange('fines_generales', true)}
                />

                <label
                  htmlFor="fines_generales_si"
                  className="text-sm text-gray-700"
                >
                  Sí
                </label>
              </div>

              <div className="flex items-center gap-2">
                <RadioButton
                  className="form-radio-presto"
                  inputId="fines_generales_no"
                  checked={value?.fines_generales === false}
                  disabled={disabled}
                  onChange={() => onChange('fines_generales', false)}
                />

                <label
                  htmlFor="fines_generales_no"
                  className="text-sm text-gray-700"
                >
                  No
                </label>
              </div>
            </div>
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Nombre Proyecto *</label>
            <InputText
              value={value?.nombre_proyecto ?? ''}
              disabled={disabled}
              className={inputClass}
              onChange={(e) => onChange('nombre_proyecto', e.target.value)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>
              Crédito Segunda Vivienda *
            </label>

            <div className="flex items-center h-10">
              <div className="flex items-center gap-2">
                <Checkbox
                  inputId="credito_segunda_vivienda"
                  className="form-checkbox-presto"
                  checked={!!value?.credito_segunda_vivienda}
                  disabled={disabled}
                  onChange={(e) =>
                    onChange(
                      'credito_segunda_vivienda',
                      !!e.checked
                    )
                  }
                />

                <label
                  htmlFor="credito_segunda_vivienda"
                  className="text-sm text-gray-700"
                >
                  Sí
                </label>
              </div>
            </div>
          </div>
          <div className={fieldClass}>
            <label className={labelClass}>Inmobiliaria *</label>
            <InputText
              value={value?.inmobiliaria ?? ''}
              disabled={disabled}
              className={inputClass}
              onChange={(e) => onChange('inmobiliaria', e.target.value)}
            />
          </div>
        </div>
      </div>

      <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
        <div className="mb-4 border-b border-gray-100 pb-3">
          <h3 className="text-base font-semibold text-gray-800">Condiciones de la Propiedad</h3>
          <p className="text-sm text-gray-500">Indicadores y datos tributarios del crédito.</p>
        </div>
        <div className="overflow-hidden border border-gray-200 bg-white">
          <table className="w-full border-collapse">
            <thead>
              <tr className="bg-blue-600 text-white">
                <th className="border border-gray-300 px-3 py-2 text-left">
                  Descripción
                </th>

                <th className="border border-gray-300 px-3 py-2 text-center">
                  Sí
                </th>

                <th className="border border-gray-300 px-3 py-2 text-center">
                  No
                </th>
              </tr>
            </thead>

            <tbody>
              <tr>
                <td className="border border-gray-300 px-3 py-2">
                  Propiedad adquirida es Vivienda Social
                </td>

                <td className="border border-gray-300 text-center">
                  <RadioButton
                  aria-label="sí"
                  className="form-radio-presto"
                    inputId="vivienda_social_si"
                    checked={value?.vivienda_social === true}
                    disabled={disabled}
                    onChange={() => onChange('vivienda_social', true)}
                  />
                </td>

                <td className="border border-gray-300 text-center">
                  <RadioButton
                  className="form-radio-presto"
                    inputId="vivienda_social_no"
                    checked={value?.vivienda_social === false}
                    disabled={disabled}
                    onChange={() => onChange('vivienda_social', false)}
                  />
                </td>
              </tr>

              <tr>
                <td className="border border-gray-300 px-3 py-2">
                  Propiedad adquirida es DFL 2
                </td>

                <td className="border border-gray-300 text-center">
                  <RadioButton
                  className="form-radio-presto"
                    inputId="dfl2_si"
                    checked={value?.dfl2 === true}
                    disabled={disabled}
                    onChange={() => onChange('dfl2', true)}
                  />
                </td>

                <td className="border border-gray-300 text-center">
                  <RadioButton
                  className="form-radio-presto"
                    inputId="dfl2_no"
                    checked={value?.dfl2 === false}
                    disabled={disabled}
                    onChange={() => onChange('dfl2', false)}
                  />
                </td>
              </tr>

              <tr>
                <td className="border border-gray-300 px-3 py-2">
                  Comprador propietario 0 ó 1 vivienda DFL 2
                </td>

                <td className="border border-gray-300 text-center">
                  <RadioButton
                  className="form-radio-presto"
                    inputId="propietario_dfl2_si"
                    checked={value?.propietario_dfl2 === true}
                    disabled={disabled}
                    onChange={() => onChange('propietario_dfl2', true)}
                  />
                </td>

                <td className="border border-gray-300 text-center">
                  <RadioButton
                  className="form-radio-presto"
                    inputId="propietario_dfl2_no"
                    checked={value?.propietario_dfl2 === false}
                    disabled={disabled}
                    onChange={() => onChange('propietario_dfl2', false)}
                  />
                </td>
              </tr>

              <tr>
                <td className="border border-gray-300 px-3 py-2">
                  Recepción Final Mayor a 2 Años
                </td>

                <td className="border border-gray-300 text-center">
                  <RadioButton
                  className="form-radio-presto"
                    inputId="recepcion_si"
                    checked={value?.recepcion_final_mayor_2_anios === true}
                    disabled={disabled}
                    onChange={() =>
                      onChange('recepcion_final_mayor_2_anios', true)
                    }
                  />
                </td>

                <td className="border border-gray-300 text-center">
                  <RadioButton
                  className="form-radio-presto"
                    inputId="recepcion_no"
                    checked={value?.recepcion_final_mayor_2_anios === false}
                    disabled={disabled}
                    onChange={() =>
                      onChange('recepcion_final_mayor_2_anios', false)
                    }
                  />
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        {/* <div className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-4">
          <div className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 p-3">
            <Checkbox
                  className="form-checkbox-presto"
              checked={!!value?.vivienda_social}
              disabled={disabled}
              onChange={(e) => onChange('vivienda_social', !!e.checked)}
            />
            <label className="text-sm font-medium text-gray-700">Propiedad adquirida es Vivienda Social *</label>
          </div>

          <div className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 p-3">
            <Checkbox
              checked={!!value?.dfl2}
              disabled={disabled}
              onChange={(e) => onChange('dfl2', !!e.checked)}
            />
            <label className="text-sm font-medium text-gray-700">Propiedad adquirida es DFL 2 *</label>
          </div>

          <div className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 p-3">
            <Checkbox
              checked={!!value?.propietario_dfl2}
              disabled={disabled}
              onChange={(e) => onChange('propietario_dfl2', !!e.checked)}
            />
            <label className="text-sm font-medium text-gray-700">Comprador propietario 0 ó 1 vivienda DFL 2 *</label>
          </div>

          <div className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 p-3">
            <Checkbox
              checked={!!value?.recepcion_final_mayor_2_anios}
              disabled={disabled}
              onChange={(e) => onChange('recepcion_final_mayor_2_anios', !!e.checked)}
            />
            <label className="text-sm font-medium text-gray-700">Recepción Final Mayor a 2 Años *</label>
          </div>
        </div> */}

        <div className="mt-5 grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
          <div className={fieldClass}>
            <label className={labelClass}>% Impuesto *</label>
            <InputNumber
              value={value?.porcentaje_impuesto ?? null}
              disabled={disabled}
              className={inputClass}
              inputClassName="w-full"
              mode="decimal"
              minFractionDigits={0}
              maxFractionDigits={6}
              onValueChange={(e) => onChange('porcentaje_impuesto', e.value ?? null)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Monto Crédito Afecto Impuesto *</label>
            <InputNumber
              value={value?.monto_credito_afecto_impuesto ?? null}
              disabled={disabled}
              className={inputClass}
              inputClassName="w-full"
              mode="decimal"
              minFractionDigits={0}
              maxFractionDigits={6}
              onValueChange={(e) => onChange('monto_credito_afecto_impuesto', e.value ?? null)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Impuesto a Pagar *</label>
            <InputNumber
              value={value?.impuesto_a_pagar ?? null}
              disabled={disabled}
              className={inputClass}
              inputClassName="w-full"
              mode="decimal"
              minFractionDigits={0}
              maxFractionDigits={6}
              onValueChange={(e) => onChange('impuesto_a_pagar', e.value ?? null)}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
