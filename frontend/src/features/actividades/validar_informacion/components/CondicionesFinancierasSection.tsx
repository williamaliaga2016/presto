import { InputNumber } from 'primereact/inputnumber';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import type { EncabezadoValidarInformacion } from '../models/encabezado_validar_informacion';

type Props = {
  data: ValidarInformacionBBVA;
  encabezado: EncabezadoValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
  invalidFields?: Set<string>;
};

const formatCOP = (value: number | null | undefined) => {
  if (value === null || value === undefined) return '—';
  return new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 }).format(value);
};

export default function CondicionesFinancierasSection({
  data, encabezado, isEditing, onChange, invalidFields,
}: Props) {
  const montoOriginal = encabezado.monto_otorgado_original;
  const montoVI = data.monto_otorgado_vi;

  const isOverMax =
    montoOriginal !== null &&
    montoOriginal !== undefined &&
    montoVI !== null &&
    montoVI !== undefined &&
    montoVI > montoOriginal;

  const hasMontoError = isOverMax || (invalidFields?.has('monto_otorgado_vi') ?? false);

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Plazo (meses)</label>
        <p className="text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {encabezado.plazo_meses !== null && encabezado.plazo_meses !== undefined
            ? `${encabezado.plazo_meses} meses`
            : '—'}
        </p>
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Tasa</label>
        <p className="text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {encabezado.tasa !== null && encabezado.tasa !== undefined
            ? `${encabezado.tasa}%`
            : '—'}
        </p>
      </div>

      <div className="flex flex-col gap-1 col-span-full">
        <label className="text-xs text-gray-500">Condiciones Organismo Decisor</label>
        <p className="text-sm py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {encabezado.condiciones_organismo_decisor ?? '—'}
        </p>
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">
          Monto Otorgado VI * (máx. {formatCOP(montoOriginal)})
        </label>
        <InputNumber
          value={montoVI ?? null}
          mode="currency"
          currency="COP"
          locale="es-CO"
          maxFractionDigits={0}
          disabled={!isEditing}
          onChange={(e) => onChange('monto_otorgado_vi', e.value)}
          className={`w-full${hasMontoError ? ' p-invalid' : ''}`}
          max={montoOriginal ?? undefined}
          placeholder="$0"
        />
        {isOverMax && (
          <small className="p-error">
            El monto no puede superar {formatCOP(montoOriginal)}
          </small>
        )}
        {!isOverMax && (invalidFields?.has('monto_otorgado_vi') ?? false) && (
          <small className="p-error">El monto otorgado VI es requerido</small>
        )}
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Monto Otorgado Original</label>
        <p className="text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {formatCOP(montoOriginal)}
        </p>
      </div>
    </div>
  );
}
