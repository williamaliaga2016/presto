import { InputText } from 'primereact/inputtext';

import type { CargaOperacionBancoDatosComercial } from '../models/carga_operacion_banco';

interface DatosComercialSectionProps {
  value: CargaOperacionBancoDatosComercial;
  disabled: boolean;
  onChange: <K extends keyof CargaOperacionBancoDatosComercial>(
    field: K,
    value: CargaOperacionBancoDatosComercial[K],
  ) => void;
}

export default function DatosComercialSection({
  value,
  disabled,
  onChange,
}: DatosComercialSectionProps) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Correo Declarativo Cliente</label>
        <InputText
          value={value.correo_declarativo_cliente ?? ''}
          onChange={(e) => onChange('correo_declarativo_cliente', e.target.value)}
          type="email"
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="correo@ejemplo.com"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="font-semibold text-sm">Número Teléfono Declarativo</label>
        <InputText
          value={value.numero_telefono_declarativo ?? ''}
          onChange={(e) => onChange('numero_telefono_declarativo', e.target.value)}
          className="form-input-presto w-full"
          disabled={disabled}
          placeholder="300 000 0000"
        />
      </div>
    </div>
  );
}
