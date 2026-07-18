import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";
import { ProgressSpinner } from "primereact/progressspinner";

import type { GestionRectificatoria } from "../models/gestion_rectificatoria";
import { useGestionRectificatoria } from "../hooks/useGestionRectificatoria";
import { useUpsertGestionRectificatoria } from "../hooks/useUpsertGestionRectificatoria";
import { useAvanzarGestionRectificatoria } from "../hooks/useAvanzarGestionRectificatoria";
import { useControlesGestionRectificatoria } from "../hooks/useControlesGestionRectificatoria";

import { Accordion, AccordionTab } from "primereact/accordion";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

const ACTIVITY_ID = "gestion_rectificatoria";

const buildInitialState = (id_expediente: number): GestionRectificatoria => ({
  id_gestion_rectificatoria: 0,
  id_expediente,
  enviar_tipo_reparo: "",
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeGestionRectificatoria = (
  source: Partial<GestionRectificatoria> | null | undefined,
  id_expediente_fallback: number,
): GestionRectificatoria => {
  const isExistingRecord = Number(source?.id_gestion_rectificatoria ?? 0) > 0;
  
  return {
    id_gestion_rectificatoria: Number(source?.id_gestion_rectificatoria ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    enviar_tipo_reparo: source?.enviar_tipo_reparo ?? "",
    observaciones: source?.observaciones ?? "",
    is_active: isExistingRecord ? (source?.is_active ?? false) : true,
    row_status: isExistingRecord ? (source?.row_status ?? false) : true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
  };
};

export default function GestionRectificatoriaPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);
  const hasValidExpediente = id_expediente > 0;

  const [form, setForm] = useState<GestionRectificatoria>(buildInitialState(id_expediente));
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);
  const isAfterSaveRef = useRef(false);

  const { data: controlesResponse, isLoading: isLoadingControles } = useControlesGestionRectificatoria(hasValidExpediente);
  
  const tipoReparoOptions = controlesResponse?.detail?.tipo_reparo ?? [];

  const { data, isLoading, refetch } = useGestionRectificatoria(id_expediente);
  const saveMutation = useUpsertGestionRectificatoria();
  const avanzarMutation = useAvanzarGestionRectificatoria();

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
      const loaded = normalizeGestionRectificatoria(data.detail, id_expediente);
      setForm(loaded);
      setIsDisabled(Number(data.detail.id_gestion_rectificatoria) > 0);
      
      if (isAfterSaveRef.current) {
        setCanAdvance(true);
        isAfterSaveRef.current = false;
      } else {
        setCanAdvance(false);
      }
      
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
      setForm((prev) => normalizeGestionRectificatoria({ ...prev, id_expediente }, id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente, hasValidExpediente]);

  const updateField = (field: keyof GestionRectificatoria, value: any) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleEditar = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    if (!form.enviar_tipo_reparo) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: "Debe seleccionar un tipo de reparo",
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);
      const payload = normalizeGestionRectificatoria(
        { ...form, id_expediente: id_expediente },
        id_expediente,
      );
      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Gestión Rectificatoria guardada correctamente",
          life: 3000,
        });

        if (response.detail) {
          const savedData = normalizeGestionRectificatoria(response.detail, id_expediente);
          setForm(savedData);
        }
        
        setCanAdvance(true);
        setIsDisabled(true);
        
        isAfterSaveRef.current = true;
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
      console.error("Error al guardar:", error);
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

  const handleAvanzar = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    const expedienteId = Number(form.id_expediente ?? 0);
    if (!expedienteId || expedienteId <= 0) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: "No existe un id_expediente válido para avanzar.",
        life: 3000,
      });
      return;
    }

    if (!canAdvance) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: "Debe guardar los cambios antes de avanzar.",
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
        toast.current?.show({
          severity: "warn",
          summary: "Atención",
          detail: response.message || "No se pudo avanzar.",
          life: 3000,
        });
      }
    } catch (error) {
      console.error("Error al avanzar:", error);
      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: "Ocurrió un error al avanzar.",
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    navigate("/home/bandeja");
  };

  // Loading combinado
  if ((isLoading || isLoadingControles) && hasValidExpediente && !hasHydratedRef.current) {
    return (
      <div className="flex justify-center items-center h-64">
        <ProgressSpinner />
      </div>
    );
  }

  return (
    <div>
      <Toast ref={toast} />
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Gestión Rectificatoria</h2>

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Gestión Rectificatoria" disabled={!hasValidExpediente}>
          {!hasValidExpediente ? (
            <Card>
              <div className="text-center text-600">
                No se recibió un expediente válido para esta actividad.
              </div>
            </Card>
          ) : (
            <Card className="w-full shadow-md card-presto-form mb-6">
              {/* Radio Buttons DINÁMICOS desde el catálogo */}
              <div className="mb-6">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Estatus de la actividad
                </div>
                <div className="flex items-center gap-4 px-4 flex-wrap">
                  <span className="font-semibold text-gray-700">Enviar a:</span>
                  
                  {isLoadingControles ? (
                    <span className="text-gray-500 text-sm">Cargando opciones...</span>
                  ) : tipoReparoOptions.length > 0 ? (
                    tipoReparoOptions.map((option) => (
                      <label key={option.code} className="flex items-center gap-2">
                        <input
                          type="radio"
                          name="enviar_tipo_reparo"
                          value={option.code || ""}
                          checked={form.enviar_tipo_reparo === option.code}
                          onChange={(e) => updateField("enviar_tipo_reparo", e.target.value)}
                          disabled={isDisabled}
                          className="w-4 h-4"
                        />
                        <span>{option.description}</span>
                      </label>
                    ))
                  ) : (
                    <span className="text-red-500 text-sm">
                      No hay tipos de reparo disponibles
                    </span>
                  )}
                </div>
              </div>

              {/* Observaciones */}
              <div className="mt-6">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Observaciones
                </div>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) => updateField("observaciones", e.target.value)}
                  rows={4}
                  autoResize
                  disabled={isDisabled}
                  placeholder="Ingrese observaciones"
                  className="w-full"
                />
              </div>

              {/* Botones */}
              <div className="form-actions mt-6 flex gap-3 flex-wrap">
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