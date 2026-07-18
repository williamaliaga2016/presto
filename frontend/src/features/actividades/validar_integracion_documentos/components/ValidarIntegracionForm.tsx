import { Button } from "primereact/button";
import { Dropdown } from "primereact/dropdown";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { SelectButton } from "primereact/selectbutton";

import type { ValidarIntegracionDocumentosData } from "../models/validar_integracion_documentos";
import { ValidarIntegracionFormControles } from "../models/catalogo";
import { useDropdownTooltip } from "@/shared/hooks/useDropdownTooltip";

interface ValidarIntegracionFormProps {
  form: ValidarIntegracionDocumentosData;
  isDisabled: boolean;
  updateField: <K extends keyof ValidarIntegracionDocumentosData>(field: K, value: ValidarIntegracionDocumentosData[K]) => void;
  onOpenModal: () => void;
  controles: ValidarIntegracionFormControles;
}

export default function ValidarIntegracionForm({
  form,
  isDisabled,
  updateField,
  onOpenModal,
  controles
}: ValidarIntegracionFormProps) {
  
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

        {/* Selector ¿Documentos Correctos? */}
        <div className="flex flex-col gap-1 md:col-span-3">
          <label className="font-semibold text-sm">¿Los documentos están correctos y completos? *</label>
          <SelectButton 
            value={form.documentosCorrectos}
            options={opcionesBinarias} 
            onChange={(e) => updateField("documentosCorrectos", e.value)}
            disabled={isDisabled}
          />
        </div>

        {/* Condicional: Dropdown de Motivo de Devolución */}
        {form.documentosCorrectos === false && (
          <div className="flex flex-col gap-1 md:col-span-3">
            <label className="font-semibold text-sm">Motivo de Devolución *</label>
            <Dropdown
              value={form.motivoDevolucion}
              options={controles.motivo_devolucion}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateField("motivoDevolucion", e.value)}
              placeholder="Seleccione un motivo"
              className="w-full md:w-64 form-dropdown-presto"
              disabled={isDisabled}
              itemTemplate={itemTemplate}
              valueTemplate={valueTemplate}
            />
          </div>
        )}

        {/* Selector ¿Crédito Condicionado? */}
        <div className="flex flex-col gap-1 md:col-span-3">
          <label className="font-semibold text-sm">¿Es un Crédito Condicionado? *</label>
          <SelectButton 
            value={form.creditoCondicionado} 
            options={opcionesBinarias} 
            onChange={(e) => updateField("creditoCondicionado", Boolean(e.value))}
            disabled={isDisabled}
          />
        </div>

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
            placeholder="Escriba los comentarios o aclaraciones finales sobre el expediente..."
          />
        </div>
      </div>

      {/* Botón Secundario Adicionar Interviniente */}
      <div className="mt-4">
        <Button 
          type="button" 
          label="Adicionar Interviniente/Apoderado" 
          icon="pi pi-user-plus" 
          severity="secondary" 
          outlined 
          onClick={onOpenModal} 
        />
      </div>
    </div>
  );
}