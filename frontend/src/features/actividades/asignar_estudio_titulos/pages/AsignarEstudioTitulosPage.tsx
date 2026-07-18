import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Dropdown, DropdownChangeEvent } from "primereact/dropdown";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import { useAsignarEstudioTitulos } from "../hooks/useAsignarEstudioTitulos";
import { useCatalogoAbogado } from "../hooks/useCatalogoAbogado";
import { useUpsertAsignarEstudioTitulos } from "../hooks/useUpsertAsignarEstudioTitulos";
import type { AsignarEstudioTitulos } from "../models/asignar_estudio_titulos";

const ACTIVITY_ID = "_R2kWdF8sEedMu7_Z6pT3x";

const buildInitialState = (id_expediente: number): AsignarEstudioTitulos => ({
  id_asignar_estudio_titulos: 0,
  id_expediente,
  id_actividad: ACTIVITY_ID,
  abogado: "",
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeAsignarEstudioTitulos = (
  source: Partial<AsignarEstudioTitulos> | null | undefined,
  id_expediente_fallback: number,
): AsignarEstudioTitulos => {
  return {
    id_asignar_estudio_titulos: Number(source?.id_asignar_estudio_titulos ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    id_actividad: ACTIVITY_ID,
    abogado: source?.abogado ?? "",
    observaciones: source?.observaciones ?? "",
    is_active: source?.is_active ?? true,
    row_status: source?.row_status ?? true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
  };
};

export default function AsignarEstudioTitulosPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<AsignarEstudioTitulos>(
    buildInitialState(id_expediente),
  );
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useAsignarEstudioTitulos(id_expediente);
  const { data: catalogoAbogadoData } = useCatalogoAbogado();
  const saveMutation = useUpsertAsignarEstudioTitulos();

  const abogadoOptions = useMemo(() => {
    return catalogoAbogadoData?.detail ?? [];
  }, [catalogoAbogadoData]);

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;

      setForm(buildInitialState(id_expediente));
      setErrorMessage("");
      setSuccessMessage("");
      setIsDisabled(true);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const loadedEntity = normalizeAsignarEstudioTitulos(data.detail, id_expediente);

      setForm(loadedEntity);
      setIsDisabled(Number(data.detail.id_asignar_estudio_titulos) > 0);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm((prev) =>
        normalizeAsignarEstudioTitulos(
          {
            ...prev,
            id_expediente,
          },
          id_expediente,
        ),
      );
      setIsDisabled(true);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  const updateField = <K extends keyof AsignarEstudioTitulos>(
    field: K,
    value: AsignarEstudioTitulos[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleEditar = () => {
    setErrorMessage("");
    setSuccessMessage("");
    setIsDisabled(false);
  };

  const validateForm = () => {
    if (!form.id_expediente || form.id_expediente <= 0) {
      return "No existe un id_expediente válido.";
    }

    if (!form.abogado?.trim()) {
      return "Debe seleccionar abogado.";
    }

    if (!form.observaciones?.trim()) {
      return "Debe ingresar observaciones.";
    }

    return "";
  };

  const handleGuardar = async () => {
    setErrorMessage("");
    setSuccessMessage("");

    const validationMessage = validateForm();
    if (validationMessage) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: validationMessage,
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const payload = normalizeAsignarEstudioTitulos(
        {
          ...form,
          id_expediente: Number(form.id_expediente || id_expediente || 0),
          id_actividad: ACTIVITY_ID,
        },
        id_expediente,
      );

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Asignar Estudio de Títulos guardado correctamente",
          life: 3000,
        });

        const savedEntity = normalizeAsignarEstudioTitulos(
          response.detail ?? payload,
          payload.id_expediente,
        );

        setForm(savedEntity);
        setIsDisabled(true);
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({
          severity: "warn",
          summary: "Atención",
          detail: response.message || "No se pudo guardar",
          life: 3000,
        });
      }
    } catch (error) {
      console.error("ERROR GUARDAR ASIGNAR ESTUDIO DE TITULOS", error);

      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: "Ocurrió un error al guardar",
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = () => {
    toast.current?.show({
      severity: "info",
      summary: "Pendiente",
      detail: "Funcionalidad pendiente de workflow",
      life: 3000,
    });
  };

  const handleSalir = () => {
    navigate("/home/bandeja");
  };

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Asignar Estudio de Títulos
      </h2>

      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab
          disabled={!id_expediente || id_expediente <= 0}
          header="Información del Expediente"
        >
          <EncabezadoActividad
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            activityID={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            idActividad={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab header="Asignar Estudio de Títulos">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && (
              <div className="mb-4 text-sm text-blue-600">
                Cargando información...
              </div>
            )}

            {errorMessage && (
              <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                {errorMessage}
              </div>
            )}

            {successMessage && (
              <div className="mb-4 rounded-md border border-green-200 bg-green-50 px-4 py-3 text-sm text-green-700">
                {successMessage}
              </div>
            )}

            <fieldset className="border border-gray-200 rounded-md p-4 mb-5">
              <legend className="px-2 text-sm font-semibold text-gray-700">
                ASIGNAR ABOGADO
              </legend>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
                <div className="flex flex-col gap-1 md:col-span-1">
                  <label className="font-semibold text-sm">Abogado *</label>
                  <Dropdown
                    value={form.abogado}
                    options={abogadoOptions}
                    optionLabel="description"
                    optionValue="description"
                    onChange={(e: DropdownChangeEvent) =>
                      updateField("abogado", e.value)
                    }
                    placeholder="Seleccione abogado"
                    className="form-dropdown-presto w-full"
                    disabled={isDisabled}
                    filter
                    showClear
                  />
                </div>
              </div>
            </fieldset>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
              <div className="flex flex-col gap-1 md:col-span-3">
                <label className="font-semibold text-sm">Observaciones *</label>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) => updateField("observaciones", e.target.value)}
                  rows={5}
                  autoResize
                  className="form-textarea-presto w-full"
                  disabled={isDisabled}
                />
              </div>
            </div>

            <div className="form-actions">
              <Button
                type="button"
                label="Editar"
                icon="pi pi-pencil"
                severity="info"
                outlined
                onClick={handleEditar}
                disabled={isBusy || !isDisabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={saveMutation.isPending ? "Guardando..." : "Guardar"}
                icon="pi pi-save"
                severity="success"
                onClick={handleGuardar}
                disabled={isBusy || isDisabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label="Avanzar"
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handleAvanzar}
                disabled={isBusy}
                className="btn-responsive"
              />

              <Button
                type="button"
                label="Salir"
                icon="pi pi-sign-out"
                severity="secondary"
                outlined
                onClick={handleSalir}
                disabled={isBusy}
                className="btn-responsive"
              />
            </div>
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}
