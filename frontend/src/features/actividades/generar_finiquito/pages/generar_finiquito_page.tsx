import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";
import { ProgressSpinner } from "primereact/progressspinner";

import type { GenerarFiniquito } from "../models/generar_finiquito";
import { useGenerarFiniquito } from "../hooks/useGenerarFiniquito";
import { useUpsertGenerarFiniquito } from "../hooks/useUpsertGenerarFiniquito";
import { useAvanzarGenerarFiniquito } from "../hooks/useAvanzarGenerarFiniquito";

import { Accordion, AccordionTab } from "primereact/accordion";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

const ACTIVITY_ID = "_Ym6W_T2dNebVw9_D1aU8h";

const buildInitialState = (id_expediente: number): GenerarFiniquito => ({
  id_generar_finiquito: 0,
  id_expediente,
  tipo_tasacion: "",
  fojas_propiedad: "",
  numero_propiedad: "",
  año_propiedad: "",
  fojas_hipoteca: "",
  numero_hipoteca: "",
  año_hipoteca: "",
  fojas_prohibicion: "",
  numero_prohibicion: "",
  año_prohibicion: "",
  fojas_hipoteca_2grado: "",
  numero_hipoteca_2grado: "",
  año_hipoteca_2grado: "",
  observaciones: "",
  tipo_propiedad: "",
  direccion: "",
  comuna: "",
  rol_avaluo: "",
  fecha_informe_tasacion: null,
  fecha_solicitud_tasacion: null,
  fecha_recepcion_tasacion: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeGenerarFiniquito = (
  source: Partial<GenerarFiniquito> | null | undefined,
  id_expediente_fallback: number,
): GenerarFiniquito => {
  const isExistingRecord = Number(source?.id_generar_finiquito ?? 0) > 0;
  
  return {
    id_generar_finiquito: Number(source?.id_generar_finiquito ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    tipo_tasacion: source?.tipo_tasacion ?? "",
    fojas_propiedad: source?.fojas_propiedad ?? "",
    numero_propiedad: source?.numero_propiedad ?? "",
    año_propiedad: source?.año_propiedad ?? "",
    fojas_hipoteca: source?.fojas_hipoteca ?? "",
    numero_hipoteca: source?.numero_hipoteca ?? "",
    año_hipoteca: source?.año_hipoteca ?? "",
    fojas_prohibicion: source?.fojas_prohibicion ?? "",
    numero_prohibicion: source?.numero_prohibicion ?? "",
    año_prohibicion: source?.año_prohibicion ?? "",
    fojas_hipoteca_2grado: source?.fojas_hipoteca_2grado ?? "",
    numero_hipoteca_2grado: source?.numero_hipoteca_2grado ?? "",
    año_hipoteca_2grado: source?.año_hipoteca_2grado ?? "",
    observaciones: source?.observaciones ?? "",
    tipo_propiedad: source?.tipo_propiedad ?? "",
    direccion: source?.direccion ?? "",
    comuna: source?.comuna ?? "",
    rol_avaluo: source?.rol_avaluo ?? "",
    fecha_informe_tasacion: source?.fecha_informe_tasacion ?? null,
    fecha_solicitud_tasacion: source?.fecha_solicitud_tasacion ?? null,
    fecha_recepcion_tasacion: source?.fecha_recepcion_tasacion ?? null,
    is_active: isExistingRecord ? (source?.is_active ?? false) : true,
    row_status: isExistingRecord ? (source?.row_status ?? false) : true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
  };
};

export default function GenerarFiniquitoPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);
  const hasValidExpediente = id_expediente > 0;

  const [form, setForm] = useState<GenerarFiniquito>(buildInitialState(id_expediente));
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);
  const isAfterSaveRef = useRef(false); // ðŸ”¥ Bandera para saber si es después de guardar

  const { data, isLoading, refetch } = useGenerarFiniquito(id_expediente);
  const saveMutation = useUpsertGenerarFiniquito();
  const avanzarMutation = useAvanzarGenerarFiniquito();

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
      const loaded = normalizeGenerarFiniquito(data.detail, id_expediente);
      setForm(loaded);
      setIsDisabled(Number(data.detail.id_generar_finiquito) > 0);
      
      // ðŸ”¥ Si es después de un guardado, mantener canAdvance en true
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
      setForm((prev) => normalizeGenerarFiniquito({ ...prev, id_expediente }, id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente, hasValidExpediente]);

  const updateField = (field: keyof GenerarFiniquito, value: any) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleEditar = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    try {
      setIsBusy(true);
      const payload = normalizeGenerarFiniquito(
        { ...form, id_expediente: id_expediente },
        id_expediente,
      );
      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Generar Finiquito guardado correctamente",
          life: 3000,
        });

        // ðŸ”¥ Marcar que es después de un guardado exitoso
        isAfterSaveRef.current = true;
        
        await refetch();
        hasHydratedRef.current = false;
        
        setIsDisabled(true);
        // No setear canAdvance aquí, se hará en el useEffect
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

  const formatDate = (dateStr: string | null | undefined): string => {
    if (!dateStr) return "-";
    return new Date(dateStr).toLocaleDateString("es-CL");
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
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Generar Finiquito</h2>

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Generar Finiquito" disabled={!hasValidExpediente}>
          {!hasValidExpediente ? (
            <Card>
              <div className="text-center text-600">
                No se recibió un expediente válido para esta actividad.
              </div>
            </Card>
          ) : (
            <Card className="w-full shadow-md card-presto-form mb-6">
              <div className="w-full overflow-x-auto">
                <table className="w-full border-collapse">
                  <thead>
                    <tr className="bg-gray-50">
                      <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200">Datos Propiedad</th>
                      <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200">Datos Tasación</th>
                      <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200">Propiedad</th>
                      <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200">Hipoteca</th>
                      <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200">Prohibición</th>
                      <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200">Hipoteca 2do Grado</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr className="bg-white align-top">
                      <td className="px-3 py-2 text-sm border border-gray-200">
                        <div><strong>Tipo:</strong> {form.tipo_propiedad || "-"}</div>
                        <div><strong>Dirección:</strong> {form.direccion || "-"}</div>
                        <div><strong>Rol Avalúo:</strong> {form.rol_avaluo || "-"}</div>
                        <div><strong>Comuna:</strong> {form.comuna || "-"}</div>
                      </td>
                      <td className="px-3 py-2 text-sm border border-gray-200">
                        <div><strong>Tipo Tasación:</strong> {form.tipo_tasacion || "-"}</div>
                        <div><strong>Fecha Informe:</strong> {formatDate(form.fecha_informe_tasacion)}</div>
                        <div><strong>Fecha Solicitud:</strong> {formatDate(form.fecha_solicitud_tasacion)}</div>
                        <div><strong>Fecha Recepción:</strong> {formatDate(form.fecha_recepcion_tasacion)}</div>
                      </td>
                      <td className="px-3 py-2 text-sm border border-gray-200">
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Fojas</label>
                          <input type="text" value={form.fojas_propiedad} onChange={(e) => updateField("fojas_propiedad", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Número</label>
                          <input type="text" value={form.numero_propiedad} onChange={(e) => updateField("numero_propiedad", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Año</label>
                          <input type="text" value={form.año_propiedad} onChange={(e) => updateField("año_propiedad", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                       </td>
                      <td className="px-3 py-2 text-sm border border-gray-200">
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Fojas</label>
                          <input type="text" value={form.fojas_hipoteca} onChange={(e) => updateField("fojas_hipoteca", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Número</label>
                          <input type="text" value={form.numero_hipoteca} onChange={(e) => updateField("numero_hipoteca", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Año</label>
                          <input type="text" value={form.año_hipoteca} onChange={(e) => updateField("año_hipoteca", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                       </td>
                      <td className="px-3 py-2 text-sm border border-gray-200">
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Fojas</label>
                          <input type="text" value={form.fojas_prohibicion} onChange={(e) => updateField("fojas_prohibicion", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Número</label>
                          <input type="text" value={form.numero_prohibicion} onChange={(e) => updateField("numero_prohibicion", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Año</label>
                          <input type="text" value={form.año_prohibicion} onChange={(e) => updateField("año_prohibicion", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                       </td>
                      <td className="px-3 py-2 text-sm border border-gray-200">
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Fojas</label>
                          <input type="text" value={form.fojas_hipoteca_2grado} onChange={(e) => updateField("fojas_hipoteca_2grado", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Número</label>
                          <input type="text" value={form.numero_hipoteca_2grado} onChange={(e) => updateField("numero_hipoteca_2grado", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                        <div className="mb-2">
                          <label className="font-semibold text-xs">Año</label>
                          <input type="text" value={form.año_hipoteca_2grado} onChange={(e) => updateField("año_hipoteca_2grado", e.target.value)} disabled={isDisabled} className="w-full px-2 py-1 text-sm border border-gray-300 rounded mt-1" />
                        </div>
                       </td>
                    </tr>
                  </tbody>
                </table>
              </div>

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