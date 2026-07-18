import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";
import { ProgressSpinner } from "primereact/progressspinner";
import { Checkbox } from "primereact/checkbox";

import type { GestionRectificatoriaSolucionReparo } from "../models/gestion_rectificatoria_solucion_reparo";
import { useGestionRectificatoriaSolucionReparo } from "../hooks/useGestionRectificatoriaSolucionReparo";
import { useUpsertGestionRectificatoriaSolucionReparo } from "../hooks/useUpsertGestionRectificatoriaSolucionReparo";
import { useAvanzarGestionRectificatoriaSolucionReparo } from "../hooks/useAvanzarGestionRectificatoriaSolucionReparo";

import { Accordion, AccordionTab } from "primereact/accordion";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

const ACTIVITY_ID = "gestion_rectificatoria_solucion_reparo";

const buildInitialState = (id_expediente: number): GestionRectificatoriaSolucionReparo => ({
  id_gestion_rectificatoria_solucion_reparo: 0,
  id_gestion_rectificatoria: 0,
  id_expediente,
  id_usuario_solicitante: 0,
  modificar_datos_memo: false,
  descontabilizar_operacion: false,
  subsanar: false,
  observaciones: "",
  id_solicitud: 0,
  id_solicitante: 0,
  solicitante: "",
  observaciones_reparo: "",
  fecha_ingreso: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalize = (
  source: Partial<GestionRectificatoriaSolucionReparo> | null | undefined,
  id_expediente_fallback: number,
): GestionRectificatoriaSolucionReparo => {
  const isExistingRecord = Number(source?.id_gestion_rectificatoria_solucion_reparo ?? 0) > 0;
  
  return {
    id_gestion_rectificatoria_solucion_reparo: Number(source?.id_gestion_rectificatoria_solucion_reparo ?? 0),
    id_gestion_rectificatoria: Number(source?.id_gestion_rectificatoria ?? source?.id_solicitud ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    id_usuario_solicitante: Number(source?.id_usuario_solicitante ?? source?.id_solicitante ?? 0),
    modificar_datos_memo: source?.modificar_datos_memo ?? false,
    descontabilizar_operacion: source?.descontabilizar_operacion ?? false,
    subsanar: source?.subsanar ?? false,
    observaciones: source?.observaciones ?? "",
    id_solicitud: Number(source?.id_solicitud ?? 0),
    id_solicitante: Number(source?.id_solicitante ?? 0),
    solicitante: source?.solicitante ?? "",
    observaciones_reparo: source?.observaciones_reparo ?? "",
    fecha_ingreso: source?.fecha_ingreso ?? null,
    is_active: isExistingRecord ? (source?.is_active ?? false) : true,
    row_status: isExistingRecord ? (source?.row_status ?? false) : true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
  };
};

const formatDate = (dateStr: string | null | undefined): string => {
  if (!dateStr) return "-";
  return new Date(dateStr).toLocaleDateString("es-CL", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  });
};

export default function GestionRectificatoriaSolucionReparoPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);
  const hasValidExpediente = id_expediente > 0;

  const [form, setForm] = useState<GestionRectificatoriaSolucionReparo>(buildInitialState(id_expediente));
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);
  const isAfterSaveRef = useRef(false);

  const { data, isLoading, refetch } = useGestionRectificatoriaSolucionReparo(id_expediente);
  const saveMutation = useUpsertGestionRectificatoriaSolucionReparo();
  const avanzarMutation = useAvanzarGestionRectificatoriaSolucionReparo();

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
      const loaded = normalize(data.detail, id_expediente);
      setForm(loaded);
      setIsDisabled(Number(data.detail.id_gestion_rectificatoria_solucion_reparo) > 0);
      
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
      setForm((prev) => normalize({ ...prev, id_expediente }, id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente, hasValidExpediente]);

  const updateField = (field: keyof GestionRectificatoriaSolucionReparo, value: any) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleOptionChange = (option: 'modificar_datos_memo' | 'descontabilizar_operacion') => {
    if (option === 'modificar_datos_memo') {
      setForm((prev) => ({
        ...prev,
        modificar_datos_memo: !prev.modificar_datos_memo,
        descontabilizar_operacion: false,
      }));
    } else {
      setForm((prev) => ({
        ...prev,
        descontabilizar_operacion: !prev.descontabilizar_operacion,
        modificar_datos_memo: false,
      }));
    }
  };

  const handleEditar = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();

    if (form.subsanar && !form.observaciones?.trim()) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: "Las observaciones son obligatorias cuando se subsana el reparo.",
        life: 3000,
      });
      return;
    }

    if (!form.id_solicitante || form.id_solicitante === 0) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: "No se ha identificado el solicitante del reparo.",
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);
      const payload = normalize({ 
        ...form, 
        id_expediente,
        id_usuario_solicitante: form.id_solicitante,
        id_gestion_rectificatoria: form.id_solicitud,
      }, id_expediente);
      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: response.message || "Solución de Reparo guardada correctamente",
          life: 3000,
        });

        if (response.detail) {
          const savedData = normalize(response.detail, id_expediente);
          setForm({
            ...savedData,
            solicitante: form.solicitante,
            observaciones_reparo: form.observaciones_reparo,
            fecha_ingreso: form.fecha_ingreso,
            id_solicitud: form.id_solicitud,
            id_solicitante: form.id_solicitante,
          });
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

  if (isLoading && hasValidExpediente && !hasHydratedRef.current) {
    return (
      <div className="flex justify-center items-center h-64">
        <ProgressSpinner />
      </div>
    );
  }

  return (
    <div>
      <Toast ref={toast} />
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Gestión Rectificatoria - Solución de Reparo</h2>

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Gestión Rectificatoria Solución de Reparo" disabled={!hasValidExpediente}>
          {!hasValidExpediente ? (
            <Card>
              <div className="text-center text-600">
                No se recibió un expediente válido para esta actividad.
              </div>
            </Card>
          ) : (
            <Card className="w-full shadow-md card-presto-form mb-6">
              {/* Radio Buttons - Solo una opción */}
              <div className="mb-6">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Acciones
                </div>
                <div className="flex items-center gap-6 px-4">
                  <label className="flex items-center gap-2">
                    <input
                      type="radio"
                      name="accion"
                      checked={form.modificar_datos_memo === true}
                      onChange={() => handleOptionChange('modificar_datos_memo')}
                      disabled={isDisabled}
                      className="w-4 h-4"
                    />
                    <span className="font-semibold text-gray-700">Modificar datos de memo</span>
                  </label>
                  <label className="flex items-center gap-2">
                    <input
                      type="radio"
                      name="accion"
                      checked={form.descontabilizar_operacion === true}
                      onChange={() => handleOptionChange('descontabilizar_operacion')}
                      disabled={isDisabled}
                      className="w-4 h-4"
                    />
                    <span className="font-semibold text-gray-700">Descontabilizar operación</span>
                  </label>
                </div>
              </div>

              {/* Tabla de Detalles del Reparo */}
              <div className="mb-6">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Detalles del Reparo
                </div>
                <div className="w-full overflow-x-auto">
                  <table className="w-full border-collapse">
                    <thead>
                      <tr className="bg-gray-50">
                        <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                          Solicitante
                        </th>
                        <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                          Observaciones
                        </th>
                        <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-center w-32">
                          Fecha de Ingreso
                        </th>
                        <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-center w-24">
                          Subsanar
                        </th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr className="bg-white">
                        <td className="px-3 py-2 text-sm border border-gray-200">
                          {form.solicitante || "-"}
                        </td>
                        <td className="px-3 py-2 text-sm border border-gray-200">
                          {form.observaciones_reparo || "-"}
                        </td>
                        <td className="px-3 py-2 text-sm border border-gray-200 text-center">
                          {formatDate(form.fecha_ingreso)}
                        </td>
                        <td className="px-3 py-2 border border-gray-200 text-center">
                          <Checkbox
                            inputId="subsanar"
                            checked={form.subsanar || false}
                            onChange={(e) => updateField("subsanar", e.checked)}
                            disabled={isDisabled}
                          />
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              {/* Observaciones */}
              <div className="mt-6">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Observaciones {form.subsanar && <span className="text-yellow-200">*</span>}
                </div>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) => updateField("observaciones", e.target.value)}
                  rows={4}
                  autoResize
                  disabled={isDisabled}
                  placeholder={form.subsanar ? "Ingrese observaciones (obligatorio)" : "Ingrese observaciones"}
                  className="w-full"
                />
              </div>

              {/* Botones */}
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