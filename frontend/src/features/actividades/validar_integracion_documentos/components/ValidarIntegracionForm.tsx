import { Button } from "primereact/button";
import { Dropdown } from "primereact/dropdown";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { SelectButton } from "primereact/selectbutton";

import type { ValidarIntegracionDocumentosFormulario } from "../models/validar_integracion_documentos";
import { ValidarIntegracionFormControles } from "../models/catalogo";
import { useDropdownTooltip } from "@/shared/hooks/useDropdownTooltip";

interface ValidarIntegracionFormProps {
  form: ValidarIntegracionDocumentosFormulario;
  isDisabled: boolean;
  updateField: <K extends keyof ValidarIntegracionDocumentosFormulario>(field: K, value: ValidarIntegracionDocumentosFormulario[K]) => void;
  controles: ValidarIntegracionFormControles;
}

export default function ValidarIntegracionForm({
  form,
  isDisabled,
  updateField,
  controles
}: ValidarIntegracionFormProps) {

  const { itemTemplate, valueTemplate } = useDropdownTooltip('description');

  return (
    <div className="w-full">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-5">

        {/* Selector ¿Documentos Correctos? (Funcionamiento Toggle) */}
        <div className="flex flex-col gap-1">
          <div className="flex items-center justify-between mb-1">
            <label className="font-semibold text-sm">¿Documentos Correctos? *</label>
            <label
              className={`relative inline-flex items-center ${isDisabled ? "opacity-60" : "cursor-pointer"}`}
            >
              <input 
                type="checkbox" 
                id="toggle-val-integracion-docs" 
                checked={form.documentos_correctos ?? false}
                disabled={isDisabled}
                onChange={(e) => updateField("documentos_correctos", e.target.checked)}
                className="sr-only peer"
              />
              <div className="w-9 h-5 bg-gray-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-4 after:w-4 after:transition-all peer-checked:bg-blue-600 disabled:opacity-60"></div>
            </label>
          </div>
        </div>

        {/* Condicional: Dropdown de Motivo de Devolución con tus estilos originales */}
        {form.documentos_correctos === false && (
          <div className="flex flex-col gap-1 md:col-span-3">
            <label className="font-semibold text-sm">Motivo de Devolución *</label>
            <Dropdown
              value={form.motivo_devolucion}
              options={controles.motivo_devolucion}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateField("motivo_devolucion", e.value)}
              placeholder="Seleccione un motivo"
              className="w-full md:w-64 form-dropdown-presto"
              disabled={isDisabled}
              itemTemplate={itemTemplate}
              valueTemplate={valueTemplate}
            />
          </div>
        )}

        {/* Selector ¿Crédito Condicionado? (Funcionamiento Select HTML con tus fuentes) */}
        <div className="flex flex-col gap-1 md:col-span-3">
          <label className="font-semibold text-sm">¿Crédito Condicionado? *</label>
          <Dropdown
            id="select-credito-condicionado"
            value={form.credito_condicionado}
            options={[
              { label: 'Sí', value: true },
              { label: 'No', value: false }
            ]}
            optionLabel="label"
            optionValue="value"
            disabled={isDisabled}
            onChange={(e) => updateField("credito_condicionado", e.value)}
            placeholder="Seleccione..."
            className="w-full md:w-64 form-dropdown-presto"
          />
        </div>

        {/* Cuadro de texto de Observaciones original */}
        <div className="flex flex-col gap-1 md:col-span-3">
          <label className="font-semibold text-sm">Observaciones *</label>
          <InputTextarea
            value={form.observaciones ?? ""}
            onChange={(e) => updateField("observaciones", e.target.value)}
            rows={4}
            autoResize
            className="form-textarea-presto w-full"
            disabled={isDisabled}
            placeholder="Escriba los comentarios o aclaraciones finales sobre el expediente..."
          />
        </div>
      </div>
    </div>
  );
}