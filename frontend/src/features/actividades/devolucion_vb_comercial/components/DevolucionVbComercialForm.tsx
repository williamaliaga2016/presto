import { Dropdown } from "primereact/dropdown";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { SelectButton } from "primereact/selectbutton";

import type { DevolucionVbComercialData } from "../models/devolucion_vb_comercial";
import { useDropdownTooltip } from "@/shared/hooks/useDropdownTooltip";
import { DevolucionVbComercialFormControles } from "../models/catalogos";

interface DevolucionVbComercialFormProps {
  form: DevolucionVbComercialData;
  isDisabled: boolean;
  updateField: <K extends keyof DevolucionVbComercialData>(field: K, value: DevolucionVbComercialData[K]) => void;
  controles: DevolucionVbComercialFormControles;
}

export default function DevolucionVbComercialForm({
  form,
  isDisabled,
  updateField,
  controles
}: DevolucionVbComercialFormProps) {
  
  const opcionesBinarias = [
    { label: 'Sí', value: true },
    { label: 'No', value: false }
  ];

  const { itemTemplate, valueTemplate } = useDropdownTooltip('description');

  return (
    <div className="w-full">
      <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
        {/* Campo ID Expediente (Solo Lectura) */}
        <div className="flex flex-col gap-1">
          <label className="font-semibold text-sm">Expediente</label>
          <InputNumber value={form.idExpediente} className="form-input-presto w-full" useGrouping={false} disabled />
        </div>

        {/* Selector ¿Cliente Desiste? */}
        <div className="flex flex-col gap-1 md:col-span-3">
          <label className="font-semibold text-sm">¿El cliente desiste del caso? *</label>
          <SelectButton 
            value={form.clienteDesiste}
            options={opcionesBinarias} 
            onChange={(e) => updateField("clienteDesiste", e.value)}
            disabled={isDisabled}
          />
        </div>

        {/* Condicional: Dropdown de Motivo de Cierre L12 */}
        {form.clienteDesiste === true && (
          <div className="flex flex-col gap-1 md:col-span-3">
            <label className="font-semibold text-sm">Motivo de Cierre *</label>
            <Dropdown
              value={form.motivoCierre}
              options={controles.motivo_cierre}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateField("motivoCierre", e.value)}
              placeholder="Seleccione un motivo de cierre"
              className="w-full md:w-64 form-dropdown-presto"
              disabled={isDisabled}
              itemTemplate={itemTemplate}
              valueTemplate={valueTemplate}
            />
          </div>
        )}

        {/* Cuadro de texto de Observaciones */}
        <div className="flex flex-col gap-1 md:col-span-3">
          <label className="font-semibold text-sm">Observaciones *</label>
          <InputTextarea
            value={form.observaciones ?? ""}
            onChange={(e) => updateField("observaciones", e.target.value)}
            rows={4}
            autoResize
            className="form-textarea-presto w-full"
            disabled={isDisabled}
            placeholder="Registre el resultado de las llamadas con el cliente..."
          />
        </div>
      </div>
    </div>
  );
}