import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { Checkbox } from 'primereact/checkbox';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';

type Props = {
  data: ValidarInformacionBBVA;
  controles: ControlesValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
};

export default function DatosInmuebleSection({
  data, controles, isEditing, onChange,
}: Props) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div className="flex items-center gap-2">
        <Checkbox
          checked={data.inmueble_definido ?? false}
          disabled={!isEditing}
          onChange={(e) => onChange('inmueble_definido', e.checked)}
          inputId="inmueble_definido"
        />
        <label htmlFor="inmueble_definido" className="text-sm font-medium">
          ¿Cliente tiene Inmueble Definido?
        </label>
      </div>

      {data.inmueble_definido && (
        <>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Tipo Inmueble</label>
            <Dropdown
              value={data.tipo_inmueble}
              options={controles.tipo_inmueble}
              optionLabel="description"
              optionValue="code"
              disabled={!isEditing}
              onChange={(e) => onChange('tipo_inmueble', e.value)}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Constructora</label>
            <InputText
              value={data.constructora ?? ''}
              disabled={!isEditing}
              onChange={(e) => onChange('constructora', e.target.value)}
              className="w-full"
            />
          </div>
          <div className="flex items-center gap-2">
            <Checkbox
              checked={data.es_constructora_vip ?? false}
              disabled={!isEditing}
              onChange={(e) => onChange('es_constructora_vip', e.checked)}
              inputId="es_constructora_vip"
            />
            <label htmlFor="es_constructora_vip" className="text-sm">
              Constructora VIP
            </label>
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Código Proyecto</label>
            <InputText
              value={data.codigo_proyecto ?? ''}
              disabled={!isEditing}
              onChange={(e) => onChange('codigo_proyecto', e.target.value)}
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Descripción Proyecto</label>
            <InputText
              value={data.descripcion_proyecto ?? ''}
              disabled={!isEditing}
              onChange={(e) => onChange('descripcion_proyecto', e.target.value)}
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Departamento Inmueble</label>
            <Dropdown
              value={data.departamento_inmueble}
              options={controles.departamento}
              optionLabel="description"
              optionValue="code"
              disabled={!isEditing}
              onChange={(e) => onChange('departamento_inmueble', e.value)}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Municipio Inmueble</label>
            <Dropdown
              value={data.municipio_inmueble}
              options={controles.municipio}
              optionLabel="description"
              optionValue="code"
              disabled={!isEditing}
              onChange={(e) => onChange('municipio_inmueble', e.value)}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
        </>
      )}
    </div>
  );
}
