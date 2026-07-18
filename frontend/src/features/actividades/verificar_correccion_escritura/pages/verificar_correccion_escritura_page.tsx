import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";
import { ProgressSpinner } from "primereact/progressspinner";

import type { VerificarCorreccionEscritura } from "../models/verificar_correccion_escritura";
import { useVerificarCorreccionEscritura } from "../hooks/useVerificarCorreccionEscritura";
import { useUpsertVerificarCorreccionEscritura } from "../hooks/useUpsertVerificarCorreccionEscritura";
import { useAvanzarVerificarCorreccionEscritura } from "../hooks/useAvanzarVerificarCorreccionEscritura";

import { Accordion, AccordionTab } from "primereact/accordion";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

const ACTIVITY_ID = "_Ny8T_P4tDebLr2_T6qH1x";

const buildInitialState = (id_expediente: number): VerificarCorreccionEscritura => ({
  id_verificar_correccion_escritura: 0,
  id_expediente,
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeVerificarCorreccionEscritura = (
  source: Partial<VerificarCorreccionEscritura> | null | undefined,
  id_expediente_fallback: number,
): VerificarCorreccionEscritura => {
  return {
    id_verificar_correccion_escritura: Number(source?.id_verificar_correccion_escritura ?? 0),
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

export default function VerificarCorreccionEscrituraPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);
  const hasValidExpediente = id_expediente > 0;

  const [form, setForm] = useState<VerificarCorreccionEscritura>(buildInitialState(id_expediente));
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useVerificarCorreccionEscritura(id_expediente);
  const saveMutation = useUpsertVerificarCorreccionEscritura();
  const avanzarMutation = useAvanzarVerificarCorreccionEscritura();

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
      setForm(normalizeVerificarCorreccionEscritura(data.detail, id_expediente));
      setIsDisabled(Number(data.detail.id_verificar_correccion_escritura) > 0);
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
      setForm((prev) => normalizeVerificarCorreccionEscritura({ ...prev, id_expediente }, id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente, hasValidExpediente]);

  const handleEditar = () => {
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    try {
      setIsBusy(true);
      const payload = normalizeVerificarCorreccionEscritura(
        { ...form, id_expediente: id_expediente },
        id_expediente,
      );
      const response = await saveMutation.mutateAsync(payload);
      
      if (response.status) {
        toast.current?.show({ 
          severity: "success", 
          summary: "Éxito", 
          detail: "Verificación de corrección guardada correctamente", 
          life: 3000 
        });
        
        setForm(normalizeVerificarCorreccionEscritura(response.detail ?? payload, payload.id_expediente));
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

  const handleAvanzar = async () => {
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

  const handleSalir = () => navigate('/home/bandeja');

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
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Verificar Corrección Escritura</h2>

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

        <AccordionTab header="Verificar Corrección Escritura" disabled={!hasValidExpediente}>
          {!hasValidExpediente ? (
            <Card>
              <div className="text-center text-600">
                No se recibió un expediente válido para esta actividad.
              </div>
            </Card>
          ) : (
            <Card className="w-full shadow-md card-presto-form mb-6">
              <div className="grid grid-cols-1 gap-5">
                <div className="flex flex-col gap-1">
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