import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { Dialog } from "primereact/dialog";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import { expedienteDigitalService } from "@/features/funciones_transversales/components/expediente_digital/api/expedienteDigitalService";

import type { GenerarMemoEscritura } from "../models/generar_memo_escritura";
import type { MemoEscrituraVersion } from "../models/memo_escritura";
import { useGenerarMemoEscritura } from "../hooks/useGenerarMemoEscritura";
import { useUpsertGenerarMemoEscritura } from "../hooks/useUpsertGenerarMemoEscritura";
import { useVersionesMemoEscritura } from "../hooks/useVersionesMemoEscritura";
import { useGenerarPdfMemoEscritura } from "../hooks/useGenerarPdfMemoEscritura";
import { useAvanzarMemoEscritura } from "../hooks/useAvanzarMemoEscritura";

const ACTIVITY_ID = "_B3qRsV8uEemKg5_X6tP9w";

const buildInitialState = (id_expediente: number): GenerarMemoEscritura => ({
  id_generar_memo_escritura: 0,
  id_expediente,
  enviar_reparo: false,
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const formatDate = (iso?: string | null) => {
  if (!iso) return "-";
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return iso;
  return d.toLocaleDateString("es-CL");
};

const downloadBlob = (blob: Blob, fileName: string) => {
  const url = URL.createObjectURL(blob);
  const a = document.createElement("a");
  a.href = url;
  a.download = fileName;
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
  URL.revokeObjectURL(url);
};

const openBlob = (blob: Blob) => {
  const url = URL.createObjectURL(blob);
  window.open(url, "_blank", "noopener,noreferrer");
  setTimeout(() => URL.revokeObjectURL(url), 60_000);
};

export default function GenerarMemoEscrituraPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<GenerarMemoEscritura>(() => buildInitialState(id_expediente));
  const [is_disabled, setIsDisabled] = useState<boolean>(true);
  const [can_advance, setCanAdvance] = useState<boolean>(false);
  const [is_busy, setIsBusy] = useState<boolean>(false);
  const [mostrar_visor, setMostrarVisor] = useState<boolean>(false);

  const has_hydrated = useRef(false);
  const current_expediente = useRef(id_expediente);

  const { data: estadoResp, isLoading: isLoadingEstado } = useGenerarMemoEscritura(id_expediente);
  const { data: versionesResp, refetch: refetchVersiones } = useVersionesMemoEscritura(id_expediente);
  const upsertMutation = useUpsertGenerarMemoEscritura();
  const generarPdfMutation = useGenerarPdfMemoEscritura();
  const avanzarMutation = useAvanzarMemoEscritura();

  const versiones: MemoEscrituraVersion[] = versionesResp?.detail ?? [];
  const ultima_version: MemoEscrituraVersion | null = versiones.length > 0 ? versiones[0] : null;

  const showToast = (severity: "success" | "warn" | "info" | "error", summary: string, detail: string) => {
    toast.current?.show({ severity, summary, detail, life: 3500 });
  };

  useEffect(() => {
    if (current_expediente.current !== id_expediente) {
      current_expediente.current = id_expediente;
      has_hydrated.current = false;
      setForm(buildInitialState(id_expediente));
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (has_hydrated.current) return;
    if (isLoadingEstado) return;

    if (estadoResp?.status && estadoResp.detail) {
      setForm({ ...estadoResp.detail, observaciones: estadoResp.detail.observaciones ?? "" });
      setIsDisabled(Number(estadoResp.detail.id_generar_memo_escritura) > 0);
      setCanAdvance(true);
      has_hydrated.current = true;
    } else if (estadoResp) {
      setForm(buildInitialState(id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
      has_hydrated.current = true;
    }
  }, [estadoResp, isLoadingEstado, id_expediente]);

  const validate = (): string => {
    if (!id_expediente || id_expediente <= 0) return "No existe un id_expediente válido.";
    if (form.enviar_reparo && !(form.observaciones ?? "").trim())
      return "Debe ingresar observaciones cuando se envía a reparo.";
    return "";
  };

  const handleEditar = () => {
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    const error = validate();
    if (error) {
      showToast("warn", "Validación", error);
      return;
    }
    try {
      setIsBusy(true);
      const response = await upsertMutation.mutateAsync({
        ...form,
        id_expediente,
      });
      if (response.status) {
        setForm({ ...response.detail, observaciones: response.detail.observaciones ?? "" });
        setIsDisabled(true);
        setCanAdvance(true);
        showToast("success", "Guardado", response.message || "Generar Memo Escritura guardado.");
      } else {
        showToast("warn", "Atención", response.message || "No se pudo guardar.");
      }
    } catch (err) {
      console.error("ERROR GUARDAR GENERAR MEMO ESCRITURA", err);
      showToast("error", "Error", "Ocurrió un error al guardar.");
    } finally {
      setIsBusy(false);
    }
  };

  const handleGenerarPdf = async () => {
    if (!id_expediente || id_expediente <= 0) {
      showToast("warn", "Validación", "No existe un id_expediente válido.");
      return;
    }
    try {
      setIsBusy(true);
      const response = await generarPdfMutation.mutateAsync({
        id_expediente,
        observaciones: form.observaciones ?? "",
        is_enviar_reparo: form.enviar_reparo,
      });
      if (response.status) {
        showToast(
          "success",
          "PDF generado",
          `Versión ${response.detail?.version_archivo ?? "-"} indexada en el expediente digital.`,
        );
        await refetchVersiones();
      } else {
        showToast("warn", "Atención", response.message || "No se pudo generar el PDF.");
      }
    } catch (err) {
      console.error("ERROR GENERAR PDF MEMO ESCRITURA", err);
      showToast("error", "Error", "Ocurrió un error al generar el PDF.");
    } finally {
      setIsBusy(false);
    }
  };

  const handleVisualizar = async (version?: MemoEscrituraVersion | null) => {
    const target = version ?? ultima_version;
    if (!target) {
      showToast("info", "Sin versiones", "Primero genere el memo.");
      return;
    }
    try {
      setIsBusy(true);
      const result = await expedienteDigitalService.downloadFile(target.id_archivo);
      openBlob(result.blob);
    } catch {
      showToast("error", "Error", "No se pudo abrir el documento.");
    } finally {
      setIsBusy(false);
    }
  };

  const handleDescargar = async (version?: MemoEscrituraVersion | null) => {
    const target = version ?? ultima_version;
    if (!target) {
      showToast("info", "Sin versiones", "Primero genere el memo.");
      return;
    }
    try {
      setIsBusy(true);
      const result = await expedienteDigitalService.downloadFile(target.id_archivo);
      downloadBlob(result.blob, result.file_name ?? target.nombre_archivo_original ?? `Memorando.pdf`);
    } catch {
      showToast("error", "Error", "No se pudo descargar el documento.");
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async () => {
    if (!id_expediente || id_expediente <= 0) {
      showToast("warn", "Validación", "No existe un id_expediente válido.");
      return;
    }
    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(id_expediente);
      if (response.status) {
        showToast("success", "Avanzado", response.message || "Actividad avanzada.");
        navigate("/home/bandeja");
      } else {
        showToast("warn", "Atención", response.message || "No se pudo avanzar.");
      }
    } catch (err) {
      console.error("ERROR AVANZAR MEMO ESCRITURA", err);
      showToast("error", "Error", "Ocurrió un error al avanzar.");
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => navigate("/home/bandeja");

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Generar Memo Escritura
      </h2>

      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab
          disabled={!id_expediente || id_expediente <= 0}
          header="Información del Expediente"
        >
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab
          disabled={!id_expediente || id_expediente <= 0}
          header="Funciones Transversales"
        >
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Generar / Reparo / Versiones">
          <Card className="w-full shadow-md card-presto-form mb-6">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
              <div className="flex flex-col gap-2 md:col-span-1">
                <label className="font-semibold text-sm">¿Enviar a Reparo?</label>
                <div className="flex items-center gap-2 h-11">
                  <Checkbox
                    inputId="enviar_reparo"
                    checked={form.enviar_reparo}
                    onChange={(e: CheckboxChangeEvent) =>
                      setForm((prev) => ({ ...prev, enviar_reparo: Boolean(e.checked) }))
                    }
                    disabled={is_disabled}
                  />
                  <label htmlFor="enviar_reparo" className="text-sm">
                    Enviar este memo a reparo
                  </label>
                </div>
              </div>

              <div className="flex flex-col gap-1 md:col-span-2">
                <label className="font-semibold text-sm">
                  Observaciones {form.enviar_reparo ? "*" : ""}
                </label>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) =>
                    setForm((prev) => ({ ...prev, observaciones: e.target.value }))
                  }
                  rows={4}
                  autoResize
                  disabled={is_disabled}
                  className="form-textarea-presto w-full"
                  placeholder={
                    form.enviar_reparo
                      ? "Ingrese el motivo del reparo (se imprime al final del memo)"
                      : "Observaciones (se imprime al final del memo)"
                  }
                  maxLength={2000}
                />
                <small className="text-gray-500">{(form.observaciones ?? "").length}/2000</small>
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
                disabled={is_busy || !is_disabled}
                className="btn-responsive"
              />
              <Button
                type="button"
                label={upsertMutation.isPending ? "Guardando..." : "Guardar"}
                icon="pi pi-save"
                severity="success"
                onClick={handleGuardar}
                disabled={is_busy || is_disabled}
                className="btn-responsive"
              />
              <Button
                type="button"
                label={generarPdfMutation.isPending ? "Generando..." : "Generar PDF"}
                icon="pi pi-file-pdf"
                severity="help"
                onClick={handleGenerarPdf}
                disabled={is_busy || !id_expediente}
                className="btn-responsive"
                tooltip="Renderiza el memo y lo indexa en el expediente digital"
              />
              <Button
                type="button"
                label="Visualizar"
                icon="pi pi-eye"
                severity="secondary"
                outlined
                onClick={() => handleVisualizar()}
                disabled={is_busy || !ultima_version}
                className="btn-responsive"
                tooltip="Abre la última versión del memo en una nueva pestaña"
              />
              <Button
                type="button"
                label="Descargar"
                icon="pi pi-download"
                severity="secondary"
                outlined
                onClick={() => handleDescargar()}
                disabled={is_busy || !ultima_version}
                className="btn-responsive"
                tooltip="Descarga la última versión del memo"
              />
              <Button
                type="button"
                label={avanzarMutation.isPending ? "Avanzando..." : "Avanzar"}
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handleAvanzar}
                disabled={is_busy || !can_advance}
                className="btn-responsive"
              />
              <Button
                type="button"
                label="Salir"
                icon="pi pi-sign-out"
                severity="secondary"
                outlined
                onClick={handleSalir}
                disabled={is_busy}
                className="btn-responsive"
              />
            </div>
          </Card>

          <Card className="w-full shadow-md card-presto-form">
            <h3 className="text-lg font-semibold mb-3">Versiones del Memo</h3>
            {versiones.length === 0 ? (
              <div className="text-sm text-gray-600">Aún no se ha generado ninguna versión.</div>
            ) : (
              <DataTable value={versiones} size="small" stripedRows responsiveLayout="scroll">
                <Column field="version_archivo" header="Versión" style={{ width: "8rem" }} />
                <Column field="nombre_archivo_original" header="Archivo" />
                <Column header="Fecha" body={(v: MemoEscrituraVersion) => formatDate(v.fecha_alta ?? v.created_date)} />
                <Column field="comentarios" header="Observaciones" />
                <Column
                  header="Acciones"
                  body={(v: MemoEscrituraVersion) => (
                    <div className="flex gap-2">
                      <Button icon="pi pi-eye" rounded text severity="info" onClick={() => handleVisualizar(v)} disabled={is_busy} />
                      <Button icon="pi pi-download" rounded text severity="secondary" onClick={() => handleDescargar(v)} disabled={is_busy} />
                    </div>
                  )}
                  style={{ width: "10rem" }}
                />
              </DataTable>
            )}
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}
