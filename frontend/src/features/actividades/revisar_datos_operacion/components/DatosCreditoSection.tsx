import { useEffect, useMemo, useRef } from 'react';
import { Checkbox } from 'primereact/checkbox';
import { Dropdown } from 'primereact/dropdown';
import { InputNumber } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';
import { RadioButton } from 'primereact/radiobutton';

import type { RevisarDatosOperacionCredito } from '../models/revisar_datos_operacion';
import {
  EMPTY_CONTROLES_CREDITO,
  type ControlesCredito,
} from '../models/catalogo';

type Props = {
  value: RevisarDatosOperacionCredito | null | undefined;
  disabled: boolean;
  controles?: ControlesCredito;
  loadingControles?: boolean;
  onChange: <K extends keyof RevisarDatosOperacionCredito>(
    field: K,
    value: RevisarDatosOperacionCredito[K],
  ) => void;
};

const calcPorcentaje = (
  vivienda_social?: boolean | null,
  dfl2?: boolean | null,
  propietario_0_1_dfl2?: boolean | null,
  recepcion_final_mayor_2?: boolean | null,
): number => {
  if (vivienda_social) return 0;
  if (recepcion_final_mayor_2 && (dfl2 || propietario_0_1_dfl2)) return 0.8;
  if (dfl2 || propietario_0_1_dfl2) return 0.2;
  return 0;
};

const fieldClass = 'flex flex-col gap-1';
const labelClass = 'font-semibold text-sm text-gray-700';
const inputClass = 'form-input-presto w-full';
const dropdownClass = 'form-dropdown-presto w-full';

export default function DatosCreditoSection({
  value,
  disabled,
  controles = EMPTY_CONTROLES_CREDITO,
  loadingControles = false,
  onChange,
}: Props) {
  const controlDisabled = disabled || loadingControles;
  const userTouchedRef = useRef(false);

  useEffect(() => {
    if (!userTouchedRef.current) return;

    const porcentaje = calcPorcentaje(
      value?.vivienda_social,
      value?.dfl2,
      value?.propietario_0_1_dfl2,
      value?.recepcion_final_mayor_2,
    );
    const monto = value?.monto_credito_afecto ?? 0;
    const impuesto = porcentaje * monto;

    if (value?.porcentaje_impuesto !== porcentaje) {
      onChange('porcentaje_impuesto', porcentaje);
    }
    if (value?.impuesto_a_pagar !== impuesto) {
      onChange('impuesto_a_pagar', impuesto);
    }
  }, [
    value?.vivienda_social,
    value?.dfl2,
    value?.propietario_0_1_dfl2,
    value?.recepcion_final_mayor_2,
    value?.monto_credito_afecto,
  ]);

  const handleCondition = <K extends keyof RevisarDatosOperacionCredito>(
    field: K,
    boolValue: RevisarDatosOperacionCredito[K],
  ) => {
    userTouchedRef.current = true;
    onChange(field, boolValue);
  };

  const handleMonto = (monto: number | null) => {
    userTouchedRef.current = true;
    onChange('monto_credito_afecto', monto);
  };

  const porcentajeDisplay = useMemo(() => {
    const p = value?.porcentaje_impuesto ?? 0;
    return `${p.toFixed(2)} %`;
  }, [value?.porcentaje_impuesto]);

  const impuestoDisplay = useMemo(() => {
    const i = value?.impuesto_a_pagar ?? 0;
    return i.toFixed(2);
  }, [value?.impuesto_a_pagar]);

  return (
    <div className="space-y-5">
      <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
        <div className="mb-4 border-b border-gray-100 pb-3">
          <h3 className="text-base font-semibold text-gray-800">Datos del Crédito</h3>
          <p className="text-sm text-gray-500">
            Información general del crédito asociado al expediente.
          </p>
        </div>

        <div className="grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
          <div className={fieldClass}>
            <label className={labelClass}>Ubicación *</label>
            <div className="flex flex-col gap-3 py-2">
              <div className="flex items-center gap-2">
                <RadioButton
                  className="form-radio-presto"
                  inputId="ubicacion_santiago"
                  checked={value?.santiago === true}
                  disabled={disabled}
                  onChange={() => {
                    onChange('santiago', true);
                    onChange('regiones', false);
                  }}
                />
                <label htmlFor="ubicacion_santiago" className="text-sm text-gray-700">
                  Santiago
                </label>
              </div>
              <div className="flex items-center gap-2">
                <RadioButton
                  className="form-radio-presto"
                  inputId="ubicacion_regiones"
                  checked={value?.regiones === true}
                  disabled={disabled}
                  onChange={() => {
                    onChange('santiago', false);
                    onChange('regiones', true);
                  }}
                />
                <label htmlFor="ubicacion_regiones" className="text-sm text-gray-700">
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
              filter
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
                <label htmlFor="fines_generales_si" className="text-sm text-gray-700">
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
                <label htmlFor="fines_generales_no" className="text-sm text-gray-700">
                  No
                </label>
              </div>
            </div>
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Nombre Proyecto</label>
            <InputText
              value={value?.nombre_proyecto ?? ''}
              disabled
              className={`${inputClass} bg-gray-50`}
              placeholder="Pre-cargado desde 3.1"
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Crédito Segunda Vivienda *</label>
            <div className="flex items-center h-10">
              <div className="flex items-center gap-2">
                <Checkbox
                  inputId="credito_segunda_vivienda"
                  className="form-checkbox-presto"
                  checked={!!value?.credito_segunda_vivienda}
                  disabled={disabled}
                  onChange={(e) => onChange('credito_segunda_vivienda', !!e.checked)}
                />
                <label htmlFor="credito_segunda_vivienda" className="text-sm text-gray-700">
                  Sí
                </label>
              </div>
            </div>
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Inmobiliaria</label>
            <InputText
              value={value?.inmobiliaria ?? ''}
              disabled
              className={`${inputClass} bg-gray-50`}
              placeholder="Pre-cargado desde 3.1"
            />
          </div>
        </div>
      </div>

      <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
        <div className="mb-4 border-b border-gray-100 pb-3">
          <h3 className="text-base font-semibold text-gray-800">Condiciones de la Propiedad</h3>
          <p className="text-sm text-gray-500">
            Indicadores tributarios que determinan el % de impuesto.
          </p>
        </div>

        <div className="overflow-hidden border border-gray-200 bg-white">
          <table className="w-full border-collapse">
            <thead>
              <tr className="bg-blue-600 text-white">
                <th className="border border-gray-300 px-3 py-2 text-left">Descripción</th>
                <th className="border border-gray-300 px-3 py-2 text-center">Sí</th>
                <th className="border border-gray-300 px-3 py-2 text-center">No</th>
              </tr>
            </thead>

            <tbody>
              <tr>
                <td className="border border-gray-300 px-3 py-2">
                  Propiedad adquirida es Vivienda Social
                </td>
                <td className="border border-gray-300 text-center">
                  <RadioButton
                    aria-label="Vivienda Social Sí"
                    className="form-radio-presto"
                    inputId="vivienda_social_si"
                    checked={value?.vivienda_social === true}
                    disabled={disabled}
                    onChange={() => handleCondition('vivienda_social', true)}
                  />
                </td>
                <td className="border border-gray-300 text-center">
                  <RadioButton
                    aria-label="Vivienda Social No"
                    className="form-radio-presto"
                    inputId="vivienda_social_no"
                    checked={value?.vivienda_social === false}
                    disabled={disabled}
                    onChange={() => handleCondition('vivienda_social', false)}
                  />
                </td>
              </tr>

              <tr>
                <td className="border border-gray-300 px-3 py-2">
                  Propiedad adquirida es DFL 2
                </td>
                <td className="border border-gray-300 text-center">
                  <RadioButton
                    aria-label="DFL2 Sí"
                    className="form-radio-presto"
                    inputId="dfl2_si"
                    checked={value?.dfl2 === true}
                    disabled={disabled}
                    onChange={() => handleCondition('dfl2', true)}
                  />
                </td>
                <td className="border border-gray-300 text-center">
                  <RadioButton
                    aria-label="DFL2 No"
                    className="form-radio-presto"
                    inputId="dfl2_no"
                    checked={value?.dfl2 === false}
                    disabled={disabled}
                    onChange={() => handleCondition('dfl2', false)}
                  />
                </td>
              </tr>

              <tr>
                <td className="border border-gray-300 px-3 py-2">
                  Comprador propietario 0 ó 1 vivienda DFL 2
                </td>
                <td className="border border-gray-300 text-center">
                  <RadioButton
                    aria-label="Propietario DFL2 Sí"
                    className="form-radio-presto"
                    inputId="propietario_dfl2_si"
                    checked={value?.propietario_0_1_dfl2 === true}
                    disabled={disabled}
                    onChange={() => handleCondition('propietario_0_1_dfl2', true)}
                  />
                </td>
                <td className="border border-gray-300 text-center">
                  <RadioButton
                    aria-label="Propietario DFL2 No"
                    className="form-radio-presto"
                    inputId="propietario_dfl2_no"
                    checked={value?.propietario_0_1_dfl2 === false}
                    disabled={disabled}
                    onChange={() => handleCondition('propietario_0_1_dfl2', false)}
                  />
                </td>
              </tr>

              <tr>
                <td className="border border-gray-300 px-3 py-2">
                  Recepción Final Mayor a 2 Años
                </td>
                <td className="border border-gray-300 text-center">
                  <RadioButton
                    aria-label="Recepción Final Sí"
                    className="form-radio-presto"
                    inputId="recepcion_si"
                    checked={value?.recepcion_final_mayor_2 === true}
                    disabled={disabled}
                    onChange={() => handleCondition('recepcion_final_mayor_2', true)}
                  />
                </td>
                <td className="border border-gray-300 text-center">
                  <RadioButton
                    aria-label="Recepción Final No"
                    className="form-radio-presto"
                    inputId="recepcion_no"
                    checked={value?.recepcion_final_mayor_2 === false}
                    disabled={disabled}
                    onChange={() => handleCondition('recepcion_final_mayor_2', false)}
                  />
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div className="mt-5 grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
          <div className={fieldClass}>
            <label className={labelClass}>% Impuesto</label>
            <InputText
              value={porcentajeDisplay}
              disabled
              className={`${inputClass} bg-gray-50`}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Monto Crédito Afecto a Impuesto (UF) *</label>
            <InputNumber
              value={value?.monto_credito_afecto ?? null}
              disabled={controlDisabled}
              className={inputClass}
              inputClassName="w-full"
              placeholder="0.00"
              mode="decimal"
              minFractionDigits={2}
              maxFractionDigits={2}
              onValueChange={(e) => handleMonto(e.value ?? null)}
            />
          </div>

          <div className={fieldClass}>
            <label className={labelClass}>Impuesto a Pagar (UF)</label>
            <InputText
              value={impuestoDisplay}
              disabled
              className={`${inputClass} bg-gray-50`}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
