import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import type { RevisarDesembolso } from "../models/revisar_desembolso";
import { useRevisarDesembolso } from "../hooks/useRevisarDesembolso";
import { useUpsertRevisarDesembolso } from "../hooks/useUpsertRevisarDesembolso";
import { useAvanzarRevisarDesembolso } from "../hooks/useAvanzarRevisarDesembolso";

import { Accordion, AccordionTab } from "primereact/accordion";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

// const ACTIVITY_ID = ""; /*pendiente asignar cuando llegue el ID del workflow*/
const ACTIVITY_ID = '_4X3k8H6kEeeGvo3jOJ1wYw';//por ahora se usara el mismo 

const buildInitialState = (id_expediente: number): RevisarDesembolso => ({
  id_revisar_desembolso: 0,
  id_expediente,
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeRevisarDesembolso = (
  source: Partial<RevisarDesembolso> | null | undefined,
  id_expediente_fallback: number,
): RevisarDesembolso => {
  return {
    id_revisar_desembolso: Number(source?.id_revisar_desembolso ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    observaciones: source?.observaciones ?? "",
    is_active: source?.is_active ?? true,
    row_status: source?.row_status ?? true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
  };
};

export default function RevisarDesembolsoPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RevisarDesembolso>(buildInitialState(id_expediente));
  const [errorMessage, setErrorMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);/*esto debe cambiar luego a true si fuera necesario en el flujo*/
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRevisarDesembolso(id_expediente);
  const saveMutation = useUpsertRevisarDesembolso();
  const avanzarMutation = useAvanzarRevisarDesembolso();

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialState(id_expediente));
      setErrorMessage("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      setForm(normalizeRevisarDesembolso(data.detail, id_expediente));
      setIsDisabled(Number(data.detail.id_revisar_desembolso) > 0);   // tiene datos â†’ bloquear hasta que editen
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (!id_expediente || id_expediente <= 0) {
      setForm(buildInitialState(0));
      setIsDisabled(false);  // sin expediente â†’ habilitado
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm((prev) => normalizeRevisarDesembolso({ ...prev, id_expediente }, id_expediente));
      setIsDisabled(false);  // <-- aquí el cambio, expediente existe pero sin registro â†’ habilitado
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  const handleEditar = () => {
    setErrorMessage("");
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    setErrorMessage("");
    try {
      setIsBusy(true);
      const payload = normalizeRevisarDesembolso(
        { ...form, id_expediente: id_expediente },
        id_expediente,
      );
      const response = await saveMutation.mutateAsync(payload);
      if (response.status) {
        toast.current?.show({ severity: "success", summary: "Éxito", detail: "Pre Finiquito guardado correctamente", life: 3000 });
        setForm(normalizeRevisarDesembolso(response.detail ?? payload, payload.id_expediente));
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({ severity: "warn", summary: "Atención", detail: response.message || "No se pudo guardar", life: 3000 });
      }
    } catch {
      toast.current?.show({ severity: "error", summary: "Error", detail: "Ocurrió un error al guardar", life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async () => {
    setErrorMessage("");
    const expedienteId = Number(form.id_expediente ?? 0);
    if (!expedienteId || expedienteId <= 0) {
      toast.current?.show({ severity: "warn", summary: "Validación", detail: "No existe un id_expediente válido para avanzar.", life: 3000 });
      return;
    }
    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(expedienteId);
      if (response.status) {
        toast.current?.show({ severity: "success", summary: "Éxito", detail: "Actividad avanzada correctamente", life: 2000 });
        navigate("/home/bandeja");
      } else {
        toast.current?.show({ severity: "warn", summary: "Atención", detail: response.message || "No se pudo avanzar.", life: 3000 });
      }
    } catch {
      toast.current?.show({ severity: "error", summary: "Error", detail: "Ocurrió un error al avanzar.", life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => navigate("/home/bandeja");

  return (
    <>
      <Toast ref={toast} />
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Generar Pre Finiquito</h2>

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

        <AccordionTab header="Generar Pre Finiquito">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && (
              <div className="mb-4 text-sm text-blue-600">Cargando información...</div>
            )}
            {errorMessage && (
              <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                {errorMessage}
              </div>
            )}
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
            <div className="form-actions">
              <Button type="button" label="Editar" icon="pi pi-pencil" severity="info" outlined onClick={handleEditar} disabled={isBusy || !isDisabled} className="btn-responsive" />
              <Button type="button" label={saveMutation.isPending ? "Guardando..." : "Guardar"} icon="pi pi-save" severity="success" onClick={handleGuardar} disabled={isBusy || isDisabled} className="btn-responsive" />
              <Button type="button" label={avanzarMutation.isPending ? "Avanzando..." : "Avanzar"} icon="pi pi-arrow-right" severity="warning" onClick={handleAvanzar} disabled={isBusy || !canAdvance} className="btn-responsive" />
              <Button type="button" label="Salir" icon="pi pi-sign-out" severity="secondary" outlined onClick={handleSalir} disabled={isBusy} className="btn-responsive" />
            </div>
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}