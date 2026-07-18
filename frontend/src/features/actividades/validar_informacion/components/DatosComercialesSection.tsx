import { InputText } from 'primereact/inputtext';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import type { EncabezadoValidarInformacion } from '../models/encabezado_validar_informacion';

type Props = {
  data: ValidarInformacionBBVA;
  encabezado: EncabezadoValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
  invalidFields?: Set<string>;
};

export default function DatosComercialesSection({
  data, encabezado, isEditing, onChange, invalidFields,
}: Props) {
  const inv = (field: string) => invalidFields?.has(field) ?? false;

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Código Oficina</label>
        <p className="text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {encabezado.codigo_oficina ?? '—'}
        </p>
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Descripción Oficina</label>
        <p className="text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {encabezado.descripcion_oficina ?? '—'}
        </p>
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Código Asesor</label>
        <p className="text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
          {encabezado.codigo_asesor ?? '—'}
        </p>
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Correo Declarativo *</label>
        <InputText
          value={data.correo_declarativo ?? ''}
          disabled={!isEditing}
          onChange={(e) => onChange('correo_declarativo', e.target.value)}
          className={`w-full${inv('correo_declarativo') ? ' p-invalid' : ''}`}
          placeholder="correo@ejemplo.com"
        />
        {inv('correo_declarativo') && (
          <small className="p-error">El correo declarativo es requerido</small>
        )}
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Teléfono Declarativo *</label>
        <InputText
          value={data.telefono_declarativo ?? ''}
          disabled={!isEditing}
          onChange={(e) => onChange('telefono_declarativo', e.target.value)}
          className={`w-full${inv('telefono_declarativo') ? ' p-invalid' : ''}`}
          placeholder="300 000 0000"
        />
        {inv('telefono_declarativo') && (
          <small className="p-error">El teléfono declarativo es requerido</small>
        )}
      </div>
    </div>
  );
}
