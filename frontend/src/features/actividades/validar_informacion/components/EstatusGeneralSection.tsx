import { Dropdown } from 'primereact/dropdown';
import { InputTextarea } from 'primereact/inputtextarea';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';

type Props = {
  data: ValidarInformacionBBVA;
  controles: ControlesValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
  invalidFields?: Set<string>;
};

export default function EstatusGeneralSection({
  data, controles, isEditing, onChange, invalidFields,
}: Props) {
  const inv = (field: string) => invalidFields?.has(field) ?? false;

  const estatusActual = controles.estatus_general.find(
    (e) => e.code === data.estatus_general,
  );

  const esEscalarComercial = data.estatus_general === 'ESCALAR_COMERCIAL';

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Estatus General del Folio *</label>
        <Dropdown
          value={data.estatus_general}
          options={controles.estatus_general}
          optionLabel="description"
          optionValue="code"
          disabled={!isEditing}
          onChange={(e) => onChange('estatus_general', e.value)}
          placeholder="Seleccionar estatus..."
          className={`w-full${inv('estatus_general') ? ' p-invalid' : ''}`}
        />
        {inv('estatus_general') && (
          <small className="p-error">El estatus general es requerido</small>
        )}
      </div>

      {estatusActual && (
        <div className="flex items-center">
          <span
            className={`px-3 py-1 rounded-full text-xs font-semibold ${
              data.estatus_general === 'LISTO'
                ? 'bg-green-100 text-green-800'
                : data.estatus_general === 'BLOQUEADO'
                  ? 'bg-red-100 text-red-800'
                  : data.estatus_general === 'ESCALAR_COMERCIAL'
                    ? 'bg-orange-100 text-orange-800'
                    : 'bg-yellow-100 text-yellow-800'
            }`}
          >
            {estatusActual.description}
          </span>
        </div>
      )}

      {esEscalarComercial && (
        <div className="flex flex-col gap-1 col-span-full">
          <label className="text-xs text-gray-500">Motivo de Devolución *</label>
          <Dropdown
            value={data.motivo_devolucion}
            options={controles.motivo_devolucion}
            optionLabel="description"
            optionValue="code"
            disabled={!isEditing}
            onChange={(e) => onChange('motivo_devolucion', e.value)}
            placeholder="Seleccionar motivo..."
            className={`w-full${inv('motivo_devolucion') ? ' p-invalid' : ''}`}
          />
          {inv('motivo_devolucion') && (
            <small className="p-error">El motivo de devolución es requerido</small>
          )}
        </div>
      )}

      <div className="flex flex-col gap-1 col-span-full">
        <label className="text-xs text-gray-500">Observaciones</label>
        <InputTextarea
          value={data.observaciones ?? ''}
          disabled={!isEditing}
          onChange={(e) => onChange('observaciones', e.target.value)}
          rows={3}
          autoResize
          className="w-full"
          placeholder="Ingrese observaciones..."
        />
      </div>

      <div className="col-span-full">
        <label className="text-xs text-gray-500 block mb-2">
          Decisiones de Avance
        </label>
        <div className="flex flex-col gap-2">
          <label className="flex items-center gap-2 text-sm">
            <input
              type="checkbox"
              checked={data.requiere_definir_inmueble ?? false}
              disabled={!isEditing}
              onChange={(e) => onChange('requiere_definir_inmueble', e.target.checked)}
            />
            Requiere Definir Inmueble
          </label>
          <label className="flex items-center gap-2 text-sm">
            <input
              type="checkbox"
              checked={data.requiere_carga_cliente ?? false}
              disabled={!isEditing}
              onChange={(e) => onChange('requiere_carga_cliente', e.target.checked)}
            />
            Requiere Carga de Documentos al Cliente
          </label>
          <label className="flex items-center gap-2 text-sm">
            <input
              type="checkbox"
              checked={data.requiere_carga_constructora ?? false}
              disabled={!isEditing}
              onChange={(e) =>
                onChange('requiere_carga_constructora', e.target.checked)
              }
            />
            Requiere Carga de Documentos a Constructora VIP
          </label>
        </div>
      </div>
    </div>
  );
}
