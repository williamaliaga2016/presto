import React, { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

import { useAprobacionComercialLegalCdR } from "../hooks/useAprobacionComercialLegalCdR";
import { useUpsertAprobacionComercialLegalCdR } from "../hooks/useUpsertAprobacionComercialLegalCdR";
import { useAvanzarAprobacionComercialLegalCdR } from "../hooks/useAvanzarAprobacionComercialLegalCdR";
import type { AprobacionComercialLegalCdR } from "../models/aprobacion_comercial_legal_cdr";

const ACTIVITY_ID = "_Hn1D_Q8sYeaFu4_N6kR9v";

export default function AprobacionComercialLegalCdRPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: id_param } = useParams();
  const id_expediente = Number(id_param ?? 0);
  const has_valid_expediente = id_expediente > 0;

  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);
  const [errorMsg, setErrorMsg] = useState("");

  const [enviarReparo, setEnviarReparo] = useState<boolean>(false);
  const [observaciones, setObservaciones] = useState<string>("");
  const [recordId, setRecordId] = useState<number>(0);

  const hasHydrated = useRef(false);
  const currentExpediente = useRef(id_expediente);

  const { data, isLoading } = useAprobacionComercialLegalCdR(id_expediente);
  const saveMutation = useUpsertAprobacionComercialLegalCdR();
  const avanzarMutation = useAvanzarAprobacionComercialLegalCdR();

  useEffect(() => {
    if (currentExpediente.current !== id_expediente) {
      currentExpediente.current = id_expediente;
      hasHydrated.current = false;

      setEnviarReparo(false);
      setObservaciones("");
      setRecordId(0);
      setErrorMsg("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydrated.current) return;

    if (data?.status && data.detail) {
      const loaded = data.detail;
      setRecordId(loaded.id_aprobacion_comercial_legal_cdr);
      setEnviarReparo(loaded.enviar_a_reparo ?? false);
      setObservaciones(loaded.observaciones ?? "");
      setIsDisabled(Number(data.detail.id_aprobacion_comercial_legal_cdr) > 0);
      setCanAdvance(true);
      hasHydrated.current = true;
      return;
    }

    if (!has_valid_expediente) {
      setEnviarReparo(false);
      setObservaciones("");
      setRecordId(0);
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydrated.current = true;
      return;
    }

    if (data) {
      setEnviarReparo(false);
      setObservaciones("");
      setRecordId(0);
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydrated.current = true;
    }
  }, [data, id_expediente, has_valid_expediente]);

  const handleEditar = () => {
    setErrorMsg("");
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const validateForm = (): string => {
    return "";
  };

  const handleGuardar = async () => {
    setErrorMsg("");

    const validationMsg = validateForm();
    if (validationMsg) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: validationMsg,
        life: 3000,
      });
      return;
    }

    setIsBusy(true);
    try {
      const payload: AprobacionComercialLegalCdR = {
        id_aprobacion_comercial_legal_cdr: recordId,
        id_expediente,
        enviar_a_reparo: enviarReparo,
        observaciones: observaciones || null,
        is_active: true,
        row_status: true,
        created_by: 0,
        created_date: new Date().toISOString(),
        modified_by: null,
        modified_date: null,
      };

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Información guardada correctamente",
          life: 3000,
        });

        const saved = response.detail ?? payload;
        setRecordId(saved.id_aprobacion_comercial_legal_cdr);
        setEnviarReparo(saved.enviar_a_reparo ?? false);
        setObservaciones(saved.observaciones ?? "");
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydrated.current = true;
      } else {
        toast.current?.show({
          severity: "warn",
          summary: "Atención",
          detail: response.message || "No se pudo guardar",
          life: 3000,
        });
      }
    } catch {
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
    setErrorMsg("");

    if (!has_valid_expediente) {
      const msg = "No existe un id_expediente válido para avanzar.";
      setErrorMsg(msg);
      toast.current?.show({ severity: "warn", summary: "Validación", detail: msg, life: 3000 });
      return;
    }

    setIsBusy(true);
    try {
      const response = await avanzarMutation.mutateAsync(id_expediente);
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
        setErrorMsg(msg);
        toast.current?.show({ severity: "warn", summary: "Atención", detail: msg, life: 3000 });
      }
    } catch {
      const msg = "Ocurrió un error al avanzar.";
      setErrorMsg(msg);
      toast.current?.show({ severity: "error", summary: "Error", detail: msg, life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => navigate("/home/bandeja");

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">Realizar Aprobación Comercial / Legal CdR</h2>

      <Accordion multiple activeIndex={[0, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!has_valid_expediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!has_valid_expediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Aprobación Comercial / Legal Carta de Resguardo">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && has_valid_expediente && (
              <div className="mb-4 text-sm text-blue-600">Cargando información...</div>
            )}

            {errorMsg && (
              <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                {errorMsg}
              </div>
            )}

            <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
              <div className="flex flex-col gap-2 md:col-span-3">
                <label className="font-semibold text-sm">¿Enviar a Reparo?</label>
                <div className="flex items-center gap-2 h-11">
                  <Checkbox
                    inputId="enviar_reparo"
                    checked={enviarReparo}
                    onChange={(e: CheckboxChangeEvent) => setEnviarReparo(Boolean(e.checked))}
                    disabled={isDisabled}
                  />
                  <label htmlFor="enviar_reparo" className="text-sm">
                    Enviar a reparo
                  </label>
                </div>
              </div>

              <div className="flex flex-col gap-1 md:col-span-3">
                <label className="font-semibold text-sm">Observaciones</label>
                <InputTextarea
                  value={observaciones}
                  onChange={(e) => setObservaciones(e.target.value)}
                  rows={4}
                  autoResize
                  className="w-full"
                  disabled={isDisabled}
                  placeholder="Ingrese observaciones relevantes"
                />
              </div>
            </div>

            <div className="form-actions mt-6 flex gap-3">
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
