import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";
import { ProgressSpinner } from "primereact/progressspinner";
import { Calendar } from "primereact/calendar";

import type { RegistrarFirmaApoderadoBanco } from "../models/registrar_firma_apoderado_banco";
import { useRegistrarFirmaApoderadoBanco } from "../hooks/useRegistrarFirmaApoderadoBanco";
import { useUpsertRegistrarFirmaApoderadoBanco } from "../hooks/useUpsertRegistrarFirmaApoderadoBanco";
import { useAvanzarRegistrarFirmaApoderadoBanco } from "../hooks/useAvanzarReistrarFirmaApoderadoBanco";

import { Accordion, AccordionTab } from "primereact/accordion";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

const ACTIVITY_ID = "_Pa3M_Q7uEebMs5_U9rJ4y";

// Obtener fecha de hoy
const getTodayISOString = (): string => {
  const today = new Date();
  today.setHours(12, 0, 0, 0);
  return today.toISOString();
};

// Verificar si la fecha es válida (no es 0001-01-01)
const isValidDate = (dateStr?: string | null): boolean => {
  if (!dateStr) return false;
  const year = new Date(dateStr).getFullYear();
  return year > 1900;
};

const buildInitialState = (id_expediente: number): RegistrarFirmaApoderadoBanco => ({
  id_registrar_firma_apoderado_banco: 0,
  fecha_firma: getTodayISOString(),
  id_expediente,
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeRegistrarFirmaApoderadoBanco = (
  source: Partial<RegistrarFirmaApoderadoBanco> | null | undefined,
  id_expediente_fallback: number,
): RegistrarFirmaApoderadoBanco => {
  // Si hay una fecha válida del backend, usarla; si no, usar hoy
  let fechaFirma = getTodayISOString();
  if (source?.fecha_firma && isValidDate(source.fecha_firma)) {
    fechaFirma = source.fecha_firma;
  }

  return {
    id_registrar_firma_apoderado_banco: Number(source?.id_registrar_firma_apoderado_banco ?? 0),
    fecha_firma: fechaFirma,
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    observaciones: source?.observaciones ?? "",
    is_active: true,
    row_status: true,
    created_by: 0,
    created_date: new Date().toISOString(),
    modified_by: null,
    modified_date: null,
  };
};

export default function RegistrarFirmaApoderadoBancoPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);
  const hasValidExpediente = id_expediente > 0;

  const [form, setForm] = useState<RegistrarFirmaApoderadoBanco>(buildInitialState(id_expediente));
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRegistrarFirmaApoderadoBanco(id_expediente);
  const saveMutation = useUpsertRegistrarFirmaApoderadoBanco();
  const avanzarMutation = useAvanzarRegistrarFirmaApoderadoBanco();

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialState(id_expediente));
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const loaded = normalizeRegistrarFirmaApoderadoBanco(data.detail, id_expediente);
      setForm(loaded);
      setIsDisabled(Number(data.detail.id_registrar_firma_apoderado_banco) > 0);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (!hasValidExpediente) {
      setForm(buildInitialState(0));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm((prev) => normalizeRegistrarFirmaApoderadoBanco({ ...prev, id_expediente }, id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente, hasValidExpediente]);

  const handleEditar = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    try {
      setIsBusy(true);
      const payload = {
        id_registrar_firma_apoderado_banco: form.id_registrar_firma_apoderado_banco,
        id_expediente: id_expediente,
        fecha_firma: form.fecha_firma,
        observaciones: form.observaciones,
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
          detail: "Registro de firma de apoderado banco guardado correctamente", 
          life: 3000 
        });
        
        setForm(normalizeRegistrarFirmaApoderadoBanco(response.detail ?? payload, payload.id_expediente));
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({ 
          severity: "warn", 
          summary: "Atención", 
          detail: response.message || "No se pudo guardar", 
          life: 3000 
        });
      }
    } catch (error) {
      console.error('Error al guardar:', error);
      toast.current?.show({ 
        severity: "error", 
        summary: "Error", 
        detail: "Ocurrió un error al guardar", 
        life: 3000 
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    const expedienteId = Number(form.id_expediente ?? 0);
    if (!expedienteId || expedienteId <= 0) {
      toast.current?.show({ 
        severity: "warn", 
        summary: "Validación", 
        detail: "No existe un id_expediente válido para avanzar.", 
        life: 3000 
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
          life: 2000 
        });
        navigate("/home/bandeja");
      } else {
        toast.current?.show({ 
          severity: "warn", 
          summary: "Atención", 
          detail: response.message || "No se pudo avanzar.", 
          life: 3000 
        });
      }
    } catch (error) {
      console.error('Error al avanzar:', error);
      toast.current?.show({ 
        severity: "error", 
        summary: "Error", 
        detail: "Ocurrió un error al avanzar.", 
        life: 3000 
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    navigate('/home/bandeja');
  };

  if (isLoading && hasValidExpediente) {
    return (
      <div className="flex justify-center items-center h-64">
        <ProgressSpinner />
      </div>
    );
  }

  return (
    <div>
      <Toast ref={toast} />
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Registrar Firma Apoderado Banco</h2>

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad 
            idExpediente={id_expediente} 
            activityID={ACTIVITY_ID} 
          />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab header="Registrar Firma Apoderado Banco" disabled={!hasValidExpediente}>
          {!hasValidExpediente ? (
            <Card>
              <div className="text-center text-600">
                No se recibió un expediente válido para esta actividad.
              </div>
            </Card>
          ) : (
            <Card className="w-full shadow-md card-presto-form mb-6">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Fecha Firma *</label>
                <Calendar 
                    className="form-input-presto w-full" 
                    value={form.fecha_firma ? new Date(form.fecha_firma) : new Date()}
                    disabled={isDisabled} 
                    showIcon 
                    dateFormat="dd/mm/yy" 
                    onChange={(e) => setForm((prev) => ({ 
                    ...prev, 
                    fecha_firma: e.value ? e.value.toISOString() : getTodayISOString()
                    }))}
                />
                </div>

                <div className="flex flex-col gap-1 md:col-span-2">
                  <label className="font-semibold text-sm">Observaciones</label>
                  <InputTextarea
                    value={form.observaciones ?? ""}
                    onChange={(e) => setForm((prev) => ({ ...prev, observaciones: e.target.value }))}
                    rows={5}
                    autoResize
                    className="form-textarea-presto w-full"
                    disabled={isDisabled}
                    placeholder="Ingrese observaciones"
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
                  disabled={isBusy || !isDisabled}
                  onClick={handleEditar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label={saveMutation.isPending ? "Guardando..." : "Guardar"}
                  icon="pi pi-save"
                  severity="success"
                  disabled={isBusy || isDisabled}
                  onClick={handleGuardar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label={avanzarMutation.isPending ? "Avanzando..." : "Avanzar"}
                  icon="pi pi-arrow-right"
                  severity="warning"
                  disabled={isBusy || !canAdvance}
                  onClick={handleAvanzar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label="Salir"
                  icon="pi pi-sign-out"
                  severity="secondary"
                  outlined
                  disabled={isBusy}
                  onClick={handleSalir}
                  className="btn-responsive"
                />
              </div>
            </Card>
          )}
        </AccordionTab>
      </Accordion>
    </div>
  );
}