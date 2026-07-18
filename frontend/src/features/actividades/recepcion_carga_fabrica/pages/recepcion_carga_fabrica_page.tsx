import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Toast } from "primereact/toast";

import type { RecepcionCargaFabrica } from "../models/recepcion_carga_fabrica";
import { useRecepcionCargaFabrica } from "../hooks/useRecepcionCargaFabrica";
import { useUpsertRecepcionCargaFabrica } from "../hooks/useUpsertRecepcionCargaFabrica";
import { useAvanzarRecepcionCargaFabrica } from "../hooks/useAvanzarRecepcionCargaFabrica";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

/**
 * TODO:
 * Reemplazar este valor por el id real de la actividad en el motor/BPMN
 * cuando ya esté registrado el catálogo de actividades.
 */
const ACTIVITY_ID = "_qBTPVMV-EeypJfcPB6uWpg";

const buildInitialState = (id_expediente: number): RecepcionCargaFabrica => ({
  id_recepcion_carga_fabrica: 0,
  id_expediente,
  id_usuario_solicitante: 0,
  is_enviar_reparo: false,
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeRecepcionCargaFabrica = (
  source: Partial<RecepcionCargaFabrica> | null | undefined,
  id_expediente_fallback: number,
): RecepcionCargaFabrica => {
  return {
    id_recepcion_carga_fabrica: Number(source?.id_recepcion_carga_fabrica ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    id_usuario_solicitante: Number(source?.id_usuario_solicitante ?? 0),
    is_enviar_reparo: Boolean(source?.is_enviar_reparo ?? false),
    observaciones: source?.observaciones ?? "",
    is_active: source?.is_active ?? true,
    row_status: source?.row_status ?? true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
  };
};

export default function RecepcionCargaFabricaPage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RecepcionCargaFabrica>(
    buildInitialState(id_expediente),
  );
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRecepcionCargaFabrica(id_expediente);
  const saveMutation = useUpsertRecepcionCargaFabrica();
  const avanzarMutation = useAvanzarRecepcionCargaFabrica();

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;

      setForm(buildInitialState(id_expediente));
      setErrorMessage("");
      setSuccessMessage("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const loadedEntity = normalizeRecepcionCargaFabrica(
        data.detail,
        id_expediente,
      );

      setForm(loadedEntity);
      setIsDisabled(Number(data.detail.id_recepcion_carga_fabrica) > 0);
      setCanAdvance(false);

      hasHydratedRef.current = true;
      return;
    }

    if (!id_expediente || id_expediente <= 0) {
      setForm(buildInitialState(0));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm((prev) =>
        normalizeRecepcionCargaFabrica(
          {
            ...prev,
            id_expediente,
          },
          id_expediente,
        ),
      );
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  const updateField = <K extends keyof RecepcionCargaFabrica>(
    field: K,
    value: RecepcionCargaFabrica[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleEnviarReparoChange = (e: CheckboxChangeEvent) => {
    const checked = Boolean(e.checked);

    setForm((prev) => ({
      ...prev,
      is_enviar_reparo: checked,
      observaciones: checked ? prev.observaciones : "",
    }));
  };

  const handleEditar = () => {
    setErrorMessage("");
    setSuccessMessage("");
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const validateForm = () => {
    if (form.id_expediente < 0) {
      return "No existe un id_expediente válido.";
    }

    if (form.is_enviar_reparo && !form.observaciones?.trim()) {
      return "Debe ingresar observaciones cuando se envía a reparo.";
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

      const payload: RecepcionCargaFabrica = normalizeRecepcionCargaFabrica(
        {
          ...form,
          id_recepcion_carga_fabrica: Number(
            form.id_recepcion_carga_fabrica ?? 0,
          ),
          id_expediente: Number(form.id_expediente || id_expediente || 0),
        },
        id_expediente,
      );

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Recepción carga fábrica guardada correctamente",
          life: 3000,
        });

        const savedEntity = normalizeRecepcionCargaFabrica(
          response.detail ?? payload,
          payload.id_expediente,
        );

        setForm(savedEntity);
        setIsDisabled(true);
        setCanAdvance(true);
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
      console.error("ERROR GUARDAR RECEPCION CARGA FABRICA", error);

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

  const handleAvanzar = async () => {
    setErrorMessage("");
    setSuccessMessage("");

    const expedienteId = Number(form.id_expediente ?? 0);

    if (!expedienteId || expedienteId < 0) {
      const msg = "No existe un id_expediente válido para avanzar.";
      setErrorMessage(msg);
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: msg,
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const response = await avanzarMutation.mutateAsync(expedienteId);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Actividad avanzada correctamente",
          life: 2000,
        });

        navigate("/home/bandeja");
      } else {
        const msg = response.message || "No se pudo avanzar la actividad.";

        setErrorMessage(msg);
        setSuccessMessage("");

        toast.current?.show({
          severity: "warn",
          summary: "Atención",
          detail: msg,
          life: 3000,
        });
      }
    } catch (error) {
      console.error("ERROR AVANZAR RECEPCION CARGA FABRICA", error);
      const msg = "Ocurrió un error al avanzar.";

      setErrorMessage(msg);
      setSuccessMessage("");

      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: msg,
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => {
    navigate("/home/bandeja");
  };

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Recepción Carga Fábrica
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

        <AccordionTab header="Recepción Carga Fábrica">
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

            <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Expediente</label>
                <InputNumber
                  value={form.id_expediente}
                  className="form-input-presto w-full"
                  useGrouping={false}
                  disabled
                />
              </div>

              <div className="flex flex-col gap-2 md:col-span-2">
                <label className="font-semibold text-sm">
                  ¿Enviar a Reparo?
                </label>

                <div className="flex items-center gap-2 h-11">
                  <Checkbox
                    className="form-checkbox-presto"
                    inputId="is_enviar_reparo"
                    checked={form.is_enviar_reparo}
                    onChange={handleEnviarReparoChange}
                    disabled={isDisabled}
                  />
                  <label htmlFor="is_enviar_reparo" className="text-sm">
                    Enviar esta recepción a reparo
                  </label>
                </div>
              </div>

              <div className="flex flex-col gap-1 md:col-span-3">
                <label className="font-semibold text-sm">
                  Observaciones {form.is_enviar_reparo ? "*" : ""}
                </label>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) => updateField("observaciones", e.target.value)}
                  rows={5}
                  autoResize
                  className="form-textarea-presto w-full"
                  disabled={isDisabled}
                  placeholder={
                    form.is_enviar_reparo
                      ? "Ingrese el motivo del reparo"
                      : "Ingrese observaciones"
                  }
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
                label={avanzarMutation.isPending ? "Avanzando..." : "Avanzar"}
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handleAvanzar}
                disabled={isBusy || !canAdvance}
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
