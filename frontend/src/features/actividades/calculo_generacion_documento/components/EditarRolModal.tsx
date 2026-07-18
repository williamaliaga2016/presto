import { useState, useEffect, useRef } from "react";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import { Dropdown } from "primereact/dropdown";
import { InputText } from "primereact/inputtext";
import { InputNumber } from "primereact/inputnumber";
import { Toast } from "primereact/toast";
import { Checkbox } from "primereact/checkbox";
import type { CalculoGeneracionDocumento } from "../models/calculo_generacion_documento";
import type { ControlesPropiedad } from "../models/catalogo";
import { calculoGeneracionDocumentoService } from "../api/calculoGeneracionDocumentoService";

interface PropiedadEditData {
  id_expediente: number;
  tipo_propiedad: string | null;
  tipo_direccion: string | null;
  direccion: string | null;
  region: string | null;
  comuna: string | null;
  existe_rol_avaluo: boolean | null;
  rol_avaluo: string | null;
  valor_avaluo_pesos: number | null;
}

interface EditarRolModalProps {
  visible: boolean;
  idExpediente: number;
  currentData: PropiedadEditData;
  completeForm: CalculoGeneracionDocumento;
  controles: ControlesPropiedad;
  onHide: () => void;
  onSuccess: (data: CalculoGeneracionDocumento) => void;
}

export default function EditarRolModal({
  visible,
  idExpediente,
  currentData,
  completeForm,
  controles,
  onHide,
  onSuccess,
}: EditarRolModalProps) {
  const toastRef = useRef<Toast>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [draft, setDraft] = useState<PropiedadEditData>(currentData);
  const [isModified, setIsModified] = useState(false);

  useEffect(() => {
    if (visible) {
      setDraft(currentData);
      setIsModified(false);
    }
  }, [visible, currentData]);

  const handleInputChange = <K extends keyof PropiedadEditData>(
    field: K,
    value: PropiedadEditData[K],
  ) => {
    setDraft((prev) => ({ ...prev, [field]: value }));
    setIsModified(true);
  };

  const showToast = (
    severity: "success" | "info" | "warn" | "error",
    summary: string,
    detail: string,
  ) => {
    toastRef.current?.show({ severity, summary, detail, life: 3000 });
  };

  const validateDraft = (): string => {
    if (!draft.tipo_propiedad?.trim()) {
      return "Tipo de Propiedad es obligatorio.";
    }
    if (!draft.tipo_direccion?.trim()) {
      return "Tipo de Dirección es obligatorio.";
    }
    if (!draft.direccion?.trim()) {
      return "Dirección es obligatoria.";
    }
    if (!draft.region?.trim()) {
      return "Región es obligatoria.";
    }
    if (!draft.comuna?.trim()) {
      return "Comuna es obligatoria.";
    }
    if (draft.existe_rol_avaluo && !draft.rol_avaluo?.trim()) {
      return "Rol Avalúo es obligatorio cuando existe rol.";
    }
    if (draft.existe_rol_avaluo && (draft.valor_avaluo_pesos === null || draft.valor_avaluo_pesos === undefined || draft.valor_avaluo_pesos <= 0)) {
      return "Valor Avalúo Pesos debe ser mayor a 0.";
    }
    return "";
  };

  const handleSave = async () => {
    const validationError = validateDraft();
    if (validationError) {
      showToast("warn", "Validación", validationError);
      return;
    }

    try {
      setIsLoading(true);
      
      // Merge edited property data into complete form
      const payload: CalculoGeneracionDocumento = {
        ...completeForm,
        tipo_propiedad: draft.tipo_propiedad,
        tipo_direccion: draft.tipo_direccion,
        direccion: draft.direccion,
        region: draft.region,
        comuna: draft.comuna,
        existe_rol_avaluo: draft.existe_rol_avaluo,
        rol_avaluo: draft.rol_avaluo,
        valor_avaluo_pesos: draft.valor_avaluo_pesos,
      };

      // Save directly to DB
      const response = await calculoGeneracionDocumentoService.save(payload);

      if (response.status) {
        onSuccess(response.detail || payload);
        onHide();
        showToast(
          "success",
          "Éxito",
          "Datos de la propiedad guardados en la base de datos."
        );
      } else {
        showToast(
          "error",
          "Error",
          response.message || "No se pudieron guardar los cambios."
        );
      }
    } catch (error: any) {
      showToast(
        "error",
        "Error",
        error?.message || "No se pudieron guardar los cambios."
      );
    } finally {
      setIsLoading(false);
    }
  };

  const dialogFooter = (
    <div className="flex justify-end gap-2">
      <Button
        type="button"
        label="Cancelar"
        icon="pi pi-times"
        severity="secondary"
        outlined
        onClick={onHide}
        disabled={isLoading}
      />
      <Button
        type="button"
        label={isLoading ? "Guardando..." : "Guardar"}
        icon="pi pi-save"
        severity="success"
        onClick={handleSave}
        disabled={isLoading || !isModified}
      />
    </div>
  );

  return (
    <>
      <Toast ref={toastRef} />
      <Dialog
        header="Editar Datos de la Propiedad"
        visible={visible}
        onHide={onHide}
        modal
        dismissableMask={false}
        closeOnEscape={false}
        draggable={false}
        resizable={false}
        className="w-full md:w-10/12 lg:w-9/12"
        footer={dialogFooter}
      >
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Tipo de Propiedad *
            </label>
            <Dropdown
              value={draft.tipo_propiedad ?? null}
              options={controles.tipo_propiedad}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => handleInputChange("tipo_propiedad", e.value)}
              className="form-input-presto w-full"
              placeholder="Seleccione"
              showClear
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Tipo de Dirección *
            </label>
            <Dropdown
              value={draft.tipo_direccion ?? null}
              options={controles.tipo_direccion}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => handleInputChange("tipo_direccion", e.value)}
              className="form-input-presto w-full"
              placeholder="Seleccione"
              showClear
            />
          </div>

          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="font-semibold text-sm">Dirección *</label>
            <InputText
              value={draft.direccion ?? ""}
              onChange={(e) => handleInputChange("direccion", e.target.value)}
              className="form-input-presto w-full"
              placeholder="Ingrese dirección completa"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Región *</label>
            <Dropdown
              value={draft.region ?? null}
              options={controles.region}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => handleInputChange("region", e.value)}
              className="form-input-presto w-full"
              placeholder="Seleccione"
              showClear
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Comuna *</label>
            <Dropdown
              value={draft.comuna ?? null}
              options={controles.comuna}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => handleInputChange("comuna", e.value)}
              className="form-input-presto w-full"
              placeholder="Seleccione"
              showClear
            />
          </div>

          <div className="md:col-span-2 border-t pt-4 mt-2">
            <div className="flex items-center gap-3 mb-4">
              <Checkbox
                inputId="existe_rol_avaluo"
                checked={Boolean(draft.existe_rol_avaluo)}
                onChange={(e) =>
                  handleInputChange("existe_rol_avaluo", Boolean(e.checked))
                }
              />
              <label htmlFor="existe_rol_avaluo" className="font-semibold text-sm">
                ¿Existe Rol Avalúo?
              </label>
            </div>

            {draft.existe_rol_avaluo && (
              <>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="flex flex-col gap-1">
                    <label className="font-semibold text-sm">
                      Rol Avalúo *
                    </label>
                    <InputText
                      value={draft.rol_avaluo ?? ""}
                      onChange={(e) =>
                        handleInputChange("rol_avaluo", e.target.value)
                      }
                      className="form-input-presto w-full"
                      placeholder="Ej: 12.345-001"
                    />
                  </div>

                  <div className="flex flex-col gap-1">
                    <label className="font-semibold text-sm">
                      Valor Avalúo Pesos *
                    </label>
                    <InputNumber
                      value={draft.valor_avaluo_pesos ?? null}
                      onValueChange={(e) =>
                        handleInputChange("valor_avaluo_pesos", e.value)
                      }
                      className="form-input-presto w-full"
                      placeholder="Ingrese valor"
                      locale="es-CL"
                      mode="currency"
                      currency="CLP"
                      min={0}
                    />
                  </div>
                </div>
              </>
            )}
          </div>

        </div>
      </Dialog>
    </>
  );
}
