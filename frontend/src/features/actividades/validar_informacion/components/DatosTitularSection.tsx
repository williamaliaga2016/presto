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
  invalidFields?: Set<string>;
};

export default function DatosTitularSection({
  data, controles, isEditing, onChange, invalidFields,
}: Props) {
  const inv = (field: string) => invalidFields?.has(field) ?? false;
  const titulares = [
    { label: 'Titular 1', prefix: 't1' as const },
    { label: 'Titular 2', prefix: 't2' as const },
    { label: 'Titular 3', prefix: 't3' as const },
  ];

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      {titulares.map(({ label, prefix }) => (
        <div key={prefix} className="col-span-full border rounded-lg p-4">
          <h4 className="font-semibold text-blue-800 mb-3">{label}</h4>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Tipo Documento</label>
              <Dropdown
                value={data[`tipo_id_${prefix}` as keyof ValidarInformacionBBVA]}
                options={controles.tipo_documento_id}
                optionLabel="description"
                optionValue="code"
                disabled={!isEditing}
                onChange={(e) => onChange(`tipo_id_${prefix}` as keyof ValidarInformacionBBVA, e.value)}
                placeholder="Seleccionar..."
                className={`w-full${inv(`tipo_id_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Número Documento</label>
              <InputText
                value={(data[`numero_id_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`numero_id_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`numero_id_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1 col-span-full">
              <label className="text-xs text-gray-500">Nombre Completo</label>
              <InputText
                value={(data[`nombre_completo_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`nombre_completo_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`nombre_completo_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Celular</label>
              <InputText
                value={(data[`celular_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`celular_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`celular_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Email</label>
              <InputText
                value={(data[`email_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`email_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`email_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            {prefix === 't1' && (
              <>
                <div className="flex flex-col gap-1">
                  <label className="text-xs text-gray-500">Situación Laboral</label>
                  <Dropdown
                    value={data.situacion_laboral_t1}
                    options={controles.situacion_laboral}
                    optionLabel="description"
                    optionValue="code"
                    disabled={!isEditing}
                    onChange={(e) => onChange('situacion_laboral_t1', e.value)}
                    placeholder="Seleccionar..."
                    className="w-full"
                  />
                </div>
                <div className="flex items-center gap-2 mt-4">
                  <Checkbox
                    checked={data.cliente_nomina_t1 ?? false}
                    disabled={!isEditing}
                    onChange={(e) => onChange('cliente_nomina_t1', e.checked)}
                    inputId="cliente_nomina"
                  />
                  <label htmlFor="cliente_nomina" className="text-sm">
                    Cliente Nómina BBVA
                  </label>
                </div>
              </>
            )}
          </div>
        </div>
      ))}
    </div>
  );
}
