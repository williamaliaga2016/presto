import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';

import type { ControlesDatosOperacion } from '../models/catalogo';
import type { CargaOperacionBancoDatosOperacion } from '../models/carga_operacion_banco';

interface DatosOperacionSectionProps {
  value: CargaOperacionBancoDatosOperacion;
  disabled: boolean;
  catalogos: ControlesDatosOperacion;
  loadingCatalogos?: boolean;
  onChange: <K extends keyof CargaOperacionBancoDatosOperacion>(
    field: K,
    value: CargaOperacionBancoDatosOperacion[K],
  ) => void;
}

const emptyMessage = 'Sin resultados';

export default function DatosOperacionSection({
  value,
  disabled,
  catalogos,
  loadingCatalogos = false,
  onChange,
}: DatosOperacionSectionProps) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Código Asesor</label>
        <InputText
          value={value.codigo_asesor ?? ''}
          onChange={(e) => onChange('codigo_asesor', e.target.value)}
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="Ingrese código asesor"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Código Oficina</label>
        <InputText
          value={value.codigo_oficina ?? ''}
          onChange={(e) => onChange('codigo_oficina', e.target.value)}
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="Ingrese código oficina"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Descripción Oficina</label>
        <InputText
          value={value.descripcion_oficina ?? ''}
          onChange={(e) => onChange('descripcion_oficina', e.target.value)}
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="Ingrese descripción de oficina"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Canal de Originación</label>
        <Dropdown
          value={value.canal_originacion ?? null}
          options={catalogos.canal_originacion}
          optionLabel="description"
          optionValue="code"
          onChange={(e) => onChange('canal_originacion', e.value ?? null)}
          className="form-dropdown-presto w-full"
          disabled={disabled}
          loading={loadingCatalogos}
          placeholder="Seleccione"
          emptyMessage={emptyMessage}
          showClear
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Tipo Inmueble</label>
        <Dropdown
          value={value.tipo_inmueble ?? null}
          options={catalogos.tipo_inmueble}
          optionLabel="description"
          optionValue="code"
          onChange={(e) => onChange('tipo_inmueble', e.value ?? null)}
          className="form-dropdown-presto w-full"
          disabled={disabled}
          loading={loadingCatalogos}
          placeholder="Seleccione"
          emptyMessage={emptyMessage}
          showClear
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Estado Inmueble</label>
        <Dropdown
          value={value.estado_inmueble ?? null}
          options={catalogos.estado_inmueble}
          optionLabel="description"
          optionValue="code"
          onChange={(e) => onChange('estado_inmueble', e.value ?? null)}
          className="form-dropdown-presto w-full"
          disabled={disabled}
          loading={loadingCatalogos}
          placeholder="Seleccione"
          emptyMessage={emptyMessage}
          showClear
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Descripción Estado Inmueble</label>
        <InputText
          value={value.descripcion_estado_inmueble ?? ''}
          onChange={(e) => onChange('descripcion_estado_inmueble', e.target.value)}
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="Ingrese descripción del estado del inmueble"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Código Proyecto</label>
        <InputText
          value={value.codigo_proyecto ?? ''}
          onChange={(e) => onChange('codigo_proyecto', e.target.value)}
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="Ingrese código de proyecto"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Descripción Proyecto</label>
        <InputText
          value={value.descripcion_proyecto ?? ''}
          onChange={(e) => onChange('descripcion_proyecto', e.target.value)}
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="Ingrese descripción de proyecto"
        />
      </div>
    </div>
  );
}
