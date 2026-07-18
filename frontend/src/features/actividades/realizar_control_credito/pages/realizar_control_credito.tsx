import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Toast } from "primereact/toast";

import type { RealizarControlCredito } from "../models/realizar_control_credito";
import { useRealizarControlCredito } from "../hooks/useRealizarControlCredito";
import { useUpsertRealizarControlCredito } from "../hooks/useUpsertRealizarControlCredito";
import { useAvanzarRealizarControlCredito } from "../hooks/useAvanzarRealizarControlCredito";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

/**
 * TODO:
 * Reemplazar este valor por el id real de la actividad en el motor/BPMN
 * cuando ya esté registrado el catálogo de actividades.
 */
const ACTIVITY_ID = "_Cy5N_R7pUebKd9_G3mL8t";

const buildInitialState = (id_expediente: number): RealizarControlCredito => ({
  id_realizar_control_credito: 0,
  id_expediente,
  id_usuario_solicitante: 0,
  enviar_reparo: false,
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeRealizarControlCredito = (
  source: Partial<RealizarControlCredito> | null | undefined,
  id_expediente_fallback: number,
): RealizarControlCredito => {
  return {
    id_realizar_control_credito: Number(source?.id_realizar_control_credito ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    id_usuario_solicitante: Number(source?.id_usuario_solicitante ?? 0),
    enviar_reparo: Boolean(source?.enviar_reparo ?? false),
    observaciones: source?.observaciones ?? "",
    is_active: source?.is_active ?? true,
    row_status: source?.row_status ?? true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
  };
};

export default function RealizarControlCreditoPage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RealizarControlCredito>(
    buildInitialState(id_expediente),
  );
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRealizarControlCredito(id_expediente);
  const saveMutation = useUpsertRealizarControlCredito();
  const avanzarMutation = useAvanzarRealizarControlCredito();

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
      const loadedEntity = normalizeRealizarControlCredito(
        data.detail,
        id_expediente,
      );

      setForm(loadedEntity);
      setIsDisabled(Number(data.detail.id_realizar_control_credito) > 0);
      setCanAdvance(!loadedEntity.enviar_reparo);

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
        normalizeRealizarControlCredito(
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

  const updateField = <K extends keyof RealizarControlCredito>(
    field: K,
    value: RealizarControlCredito[K],
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
      enviar_reparo: checked,
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
    if (form.id_expediente <= 0) {
      return "No existe un id_expediente válido.";
    }

    if (form.enviar_reparo && !form.observaciones?.trim()) {
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

      const payload: RealizarControlCredito = normalizeRealizarControlCredito(
        {
          ...form,
          id_realizar_control_credito: Number(
            form.id_realizar_control_credito ?? 0,
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
          detail: "Control de crédito guardado correctamente",
          life: 3000,
        });

        const savedEntity = normalizeRealizarControlCredito(
          response.detail ?? payload,
          payload.id_expediente,
        );

        setForm(savedEntity);
        setIsDisabled(true);
        setCanAdvance(!savedEntity.enviar_reparo);
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
      console.error("ERROR GUARDAR CONTROL CREDITO", error);

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

    if (!expedienteId || expedienteId <= 0) {
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
      console.error("ERROR AVANZAR CONTROL CREDITO", error);
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
        Realizar Control de Crédito
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

        <AccordionTab header="Realizar Control de Crédito">
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

            {/* ESTATUS DE LA ACTIVIDAD */}
              <div className="md:col-span-4 mt-2">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Estatus de la Actividad
                </div>
                <div className="flex items-center gap-3">
                  <Checkbox
                    inputId="enviar_reparo"
                    checked={form.enviar_reparo}
                    onChange={(e) => updateField("enviar_reparo", e.checked ?? false)}
                    disabled={isDisabled}
                  />
                  <label htmlFor="enviar_reparo" className="font-semibold text-sm">
                    ¿Enviar a Reparo?
                  </label>
                </div>
              </div>

              {/* OBSERVACIONES */}
              <div className="md:col-span-4 mt-2">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Observaciones
                </div>
                <textarea
                  id="observaciones"
                  value={form.observaciones || ""}
                  onChange={(e) => updateField("observaciones", e.target.value)}
                  rows={3}
                  className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
                  placeholder="Escriba aquí sus observaciones..."
                  disabled={isDisabled}
                />
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