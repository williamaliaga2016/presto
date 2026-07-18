import { Checkbox } from 'primereact/checkbox';
import { Dropdown } from 'primereact/dropdown';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';
import type { EncabezadoValidarInformacion } from '../models/encabezado_validar_informacion';

type Props = {
  data: ValidarInformacionBBVA;
  encabezado: EncabezadoValidarInformacion;
  controles: ControlesValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
  invalidFields?: Set<string>;
};

export default function DatosCreditoSection({
  data, encabezado, controles, isEditing, onChange, invalidFields,
}: Props) {
  const inv = (field: string) => invalidFields?.has(field) ?? false;

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Scoring</label>
        <p className="text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {encabezado.scoring ?? '—'}
        </p>
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Subproducto</label>
        <p className="text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {encabezado.id_tipo_sub_producto ?? '—'}
        </p>
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Tipo de Crédito *</label>
        <Dropdown
          value={data.tipo_credito}
          options={controles.tipo_credito}
          optionLabel="description"
          optionValue="code"
          disabled={!isEditing}
          onChange={(e) => onChange('tipo_credito', e.value)}
          placeholder="Seleccionar tipo..."
          className={`w-full${inv('tipo_credito') ? ' p-invalid' : ''}`}
        />
        {inv('tipo_credito') && (
          <small className="p-error">El tipo de crédito es requerido</small>
        )}
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">¿Tiene Garantía? *</label>
        <div
          className={`flex items-center gap-3 py-2 px-3 rounded border ${
            inv('tiene_garantia') ? 'border-red-500 bg-red-50' : 'border-gray-200 bg-gray-50'
          }`}
        >
          <Checkbox
            inputId="tiene_garantia"
            checked={data.tiene_garantia ?? false}
            disabled={!isEditing}
            onChange={(e) => onChange('tiene_garantia', e.checked ?? false)}
          />
          <label htmlFor="tiene_garantia" className="text-sm cursor-pointer">
            {data.tiene_garantia ? 'Sí tiene garantía' : 'No tiene garantía'}
          </label>
        </div>
        {inv('tiene_garantia') && (
          <small className="p-error">Debe indicar si tiene garantía</small>
        )}
      </div>
    </div>
  );
}
