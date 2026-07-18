import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { ConfirmDialog, confirmDialog } from "primereact/confirmdialog";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

import IngresarTasacionModal from "../components/IngresarTasacionModal";
import TasacionesRegistradasTable from "../components/TasacionesRegistradasTable";
import { useAvanzarRegistrarTasacion } from "../hooks/useAvanzarRegistrarTasacion";
import { useEvaluarReparoAutomatico } from "../hooks/useEvaluarReparoAutomatico";
import { useFactorConversionUF } from "../hooks/useFactorConversionUF";
import { useRegistrarTasacion } from "../hooks/useRegistrarTasacion";
import { useTasacionDetalles } from "../hooks/useTasacionDetalles";
import { useUpsertRegistrarTasacion } from "../hooks/useUpsertRegistrarTasacion";
import type { Tasacion } from "../models/registrar_tasacion";
import type { TasacionDetalle } from "../models/tasacion_detalle";

const ACTIVITY_ID = "_C8vLpS3mEecJYh5_X1nD9q";

const now = () => new Date().toISOString();

const buildEmptyTasacion = (id_expediente: number): Tasacion => ({
  id_tasacion: 0,
  id_expediente,
  is_enviar_reparo: false,
  observaciones: "",
  detalles: [],
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export default function RegistrarTasacionPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<Tasacion>(buildEmptyTasacion(id_expediente));
  const [detalles, setDetalles] = useState<TasacionDetalle[]>([]);

  const [modalVisible, setModalVisible] = useState(false);
  const [modalReadOnly, setModalReadOnly] = useState(false);
  const [editingDetalle, setEditingDetalle] = useState<TasacionDetalle | null>(
    null,
  );
  const [editingIndex, setEditingIndex] = useState<number | null>(null);

  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data: cabeceraResponse, isLoading: isLoadingCabecera } =
    useRegistrarTasacion(id_expediente);
  const { data: detallesResponse } = useTasacionDetalles(id_expediente);
  const { data: factorConversionUF } = useFactorConversionUF(id_expediente);

  const saveMutation = useUpsertRegistrarTasacion();
  const avanzarMutation = useAvanzarRegistrarTasacion();
  const evaluarReparoMutation = useEvaluarReparoAutomatico();

  const showToast = (
    severity: "success" | "info" | "warn" | "error",
    summary: string,
    detail: string,
    life = 3000,
  ) => {
    toast.current?.show({ severity, summary, detail, life });
  };

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;

      setForm(buildEmptyTasacion(id_expediente));
      setDetalles([]);
      setErrorMessage("");
      setSuccessMessage("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (
      cabeceraResponse?.status &&
      cabeceraResponse.detail &&
      Number(cabeceraResponse.detail.id_tasacion) > 0
    ) {
      const loaded = cabeceraResponse.detail;

      setForm({
        ...buildEmptyTasacion(id_expediente),
        ...loaded,
        id_expediente: Number(loaded.id_expediente) || id_expediente || 0,
        observaciones: loaded.observaciones ?? "",
      });
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (!id_expediente || id_expediente <= 0) {
      setForm(buildEmptyTasacion(0));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (cabeceraResponse) {
      setForm((prev) => ({
        ...prev,
        id_expediente,
      }));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [cabeceraResponse, id_expediente]);

  useEffect(() => {
    if (detallesResponse?.status && detallesResponse.detail) {
      setDetalles(
        detallesResponse.detail.map((d) => ({
          ...d,
          id_expediente: Number(d.id_expediente || id_expediente || 0),
        })),
      );
    }
  }, [detallesResponse, id_expediente]);

  const visibleDetalles = useMemo(
    () => detalles.filter((d) => d.row_status !== false),
    [detalles],
  );

  const visibleDetallesIndexMap = useMemo(() => {
    const map: Record<number, number> = {};
    let visibleIdx = 0;
    detalles.forEach((detalle, realIdx) => {
      if (detalle.row_status !== false) {
        map[visibleIdx] = realIdx;
        visibleIdx++;
      }
    });
    return map;
  }, [detalles]);

  const updateField = <K extends keyof Tasacion>(field: K, value: Tasacion[K]) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleEnviarReparoChange = (e: CheckboxChangeEvent) => {
    const checked = Boolean(e.checked);
    setForm((prev) => ({
      ...prev,
      is_enviar_reparo: checked,
      observaciones: checked ? prev.observaciones ?? "" : "",
    }));
  };

  const handleEditar = () => {
    setErrorMessage("");
    setSuccessMessage("");
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleAbrirModalNuevo = () => {
    if (isDisabled) return;
    setEditingDetalle(null);
    setEditingIndex(null);
    setModalReadOnly(false);
    setModalVisible(true);
  };

  const handleVerDetalle = (detalle: TasacionDetalle, visibleIndex: number) => {
    const realIdx = visibleDetallesIndexMap[visibleIndex] ?? visibleIndex;
    setEditingDetalle(detalle);
    setEditingIndex(realIdx);
    setModalReadOnly(true);
    setModalVisible(true);
  };

  const handleEditarDetalle = (detalle: TasacionDetalle, visibleIndex: number) => {
    if (isDisabled) return;
    const realIdx = visibleDetallesIndexMap[visibleIndex] ?? visibleIndex;
    setEditingDetalle(detalle);
    setEditingIndex(realIdx);
    setModalReadOnly(false);
    setModalVisible(true);
  };

  const handleEliminarDetalle = (
    detalle: TasacionDetalle,
    visibleIndex: number,
  ) => {
    if (isDisabled) return;

    const realIdx = visibleDetallesIndexMap[visibleIndex] ?? visibleIndex;

    if (detalle.id_tasacion_detalle > 0) {
      setDetalles((prev) =>
        prev.map((item, idx) =>
          idx === realIdx
            ? { ...item, row_status: false, is_active: false }
            : item,
        ),
      );
    } else {
      setDetalles((prev) => prev.filter((_, idx) => idx !== realIdx));
    }
  };

  const handleConfirmarModal = (detalle: TasacionDetalle) => {
    const normalized: TasacionDetalle = {
      ...detalle,
      id_expediente,
      id_tasacion: form.id_tasacion,
      is_active: true,
      row_status: true,
      created_date: detalle.created_date ?? now(),
    };

    if (editingIndex !== null && editingIndex >= 0) {
      setDetalles((prev) =>
        prev.map((item, idx) => (idx === editingIndex ? normalized : item)),
      );
    } else {
      setDetalles((prev) => [...prev, normalized]);
    }

    setModalVisible(false);
    setEditingDetalle(null);
    setEditingIndex(null);
  };

  const validateForm = (): string => {
    if (!id_expediente || id_expediente <= 0) {
      return "No existe un id_expediente válido.";
    }

    if (visibleDetalles.length === 0) {
      return "Debe registrar al menos una tasación.";
    }

    if (form.is_enviar_reparo && !form.observaciones?.trim()) {
      return "Debe ingresar observaciones cuando se envía a reparo.";
    }

    const tipos = new Set(visibleDetalles.map((d) => d.tipo_tasacion));
    if (tipos.size > 1) {
      return "Todas las tasaciones registradas deben ser del mismo Tipo (Particular o Asociada).";
    }

    return "";
  };

  const buildPayload = (): Tasacion => ({
    ...form,
    id_expediente,
    observaciones: form.observaciones ?? "",
    detalles: detalles.map((d) => ({
      ...d,
      id_expediente,
      id_tasacion: form.id_tasacion,
    })),
  });

  const handleGuardar = async () => {
    setErrorMessage("");
    setSuccessMessage("");

    const validationMessage = validateForm();
    if (validationMessage) {
      showToast("warn", "Validación", validationMessage);
      return;
    }

    try {
      setIsBusy(true);

      const response = await saveMutation.mutateAsync(buildPayload());

      if (response.status && response.detail) {
        const saved = response.detail;
        setForm((prev) => ({
          ...prev,
          ...saved,
          observaciones: saved.observaciones ?? "",
          id_expediente: Number(saved.id_expediente || id_expediente || 0),
        }));

        if (saved.detalles) {
          setDetalles(
            saved.detalles.map((d) => ({
              ...d,
              id_expediente: Number(d.id_expediente || id_expediente || 0),
            })),
          );
        }

        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;

        showToast("success", "Éxito", "Tasación guardada correctamente");
      } else {
        showToast(
          "warn",
          "Atención",
          response.message || "No se pudo guardar la tasación",
        );
      }
    } catch (error) {
      console.error("ERROR GUARDAR REGISTRAR TASACION", error);
      showToast("error", "Error", "Ocurrió un error al guardar");
    } finally {
      setIsBusy(false);
    }
  };

  const ejecutarAvanzar = async () => {
    try {
      const response = await avanzarMutation.mutateAsync(id_expediente);

      if (response.status) {
        showToast(
          "success",
          "Éxito",
          "Actividad avanzada correctamente",
          2000,
        );
        navigate("/home/bandeja");
      } else {
        showToast(
          "warn",
          "Atención",
          response.message || "No se pudo avanzar la actividad",
        );
      }
    } catch (error) {
      console.error("ERROR AVANZAR REGISTRAR TASACION", error);
      showToast("error", "Error", "Ocurrió un error al avanzar");
    }
  };

  const handleAvanzar = async () => {
    setErrorMessage("");
    setSuccessMessage("");

    const validationMessage = validateForm();
    if (validationMessage) {
      setErrorMessage(validationMessage);
      showToast("warn", "Validación", validationMessage);
      return;
    }

    try {
      setIsBusy(true);

      if (!form.is_enviar_reparo) {
        const evalResponse = await evaluarReparoMutation.mutateAsync(
          id_expediente,
        );
        if (
          evalResponse.status &&
          evalResponse.detail?.aplica_reparo_automatico
        ) {
          const msg =
            evalResponse.detail.mensaje ||
            "Las condiciones de la tasación no cumplen con el monto del préstamo. La actividad será enviada a Corregir Reparo Tasación.";

          confirmDialog({
            header: "Reparo automático de Tasación",
            message: msg,
            icon: "pi pi-exclamation-triangle",
            acceptLabel: "Entendido, avanzar",
            rejectLabel: "Cancelar",
            acceptClassName: "p-button-warning",
            accept: async () => {
              await ejecutarAvanzar();
            },
          });
          return;
        }
      }

      await ejecutarAvanzar();
    } catch (error) {
      console.error("ERROR EVALUAR/AVANZAR REGISTRAR TASACION", error);
      showToast("error", "Error", "Ocurrió un error al avanzar la actividad");
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
      <ConfirmDialog />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Registrar Tasación
      </h2>

      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab
          disabled={!id_expediente || id_expediente <= 0}
          header="Información del Expediente"
        >
          <EncabezadoActividad
            idExpediente={Number(form.id_expediente) || id_expediente || 0}
            activityID={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={Number(form.id_expediente) || id_expediente || 0}
            idActividad={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab header="Registrar Tasación">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoadingCabecera && id_expediente > 0 && (
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

            <div className="grid grid-cols-1 md:grid-cols-2 gap-5 mb-4">
              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">
                  Factor Conversión UF
                </label>
                <InputNumber
                  value={factorConversionUF ?? null}
                  className="form-input-presto w-full"
                  locale="es-CL"
                  minFractionDigits={2}
                  maxFractionDigits={6}
                  disabled
                  placeholder="No definido"
                />
              </div>

              <div className="flex items-end">
                <Button
                  type="button"
                  label="Ingresar Tasación"
                  icon="pi pi-plus"
                  severity="info"
                  outlined
                  onClick={handleAbrirModalNuevo}
                  disabled={isDisabled || isBusy}
                  className="btn-responsive w-full md:w-auto"
                />
              </div>
            </div>

            <div className="bg-blue-700 text-white font-semibold px-3 py-2 text-sm mb-3">
              TASACIONES REGISTRADAS
            </div>

            <TasacionesRegistradasTable
              rows={visibleDetalles}
              disabled={isBusy}
              canEdit={!isDisabled}
              onView={handleVerDetalle}
              onEdit={handleEditarDetalle}
              onDelete={handleEliminarDetalle}
            />

            <div className="grid grid-cols-1 md:grid-cols-3 gap-5 mt-6">
              <div className="flex flex-col gap-2 md:col-span-3">
                <label className="font-semibold text-sm">
                  ¿Enviar a Reparo?
                </label>

                <div className="flex items-center gap-2 h-11">
                  <Checkbox
                    className="form-checkbox-presto"
                    inputId="is_enviar_reparo_registrar_tasacion"
                    checked={form.is_enviar_reparo}
                    onChange={handleEnviarReparoChange}
                    disabled={isDisabled}
                  />
                  <label
                    htmlFor="is_enviar_reparo_registrar_tasacion"
                    className="text-sm"
                  >
                    Enviar esta tasación a Corregir Reparo Tasación
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

      <IngresarTasacionModal
        visible={modalVisible}
        idExpediente={id_expediente}
        factorConversionUF={factorConversionUF ?? null}
        initialData={editingDetalle}
        readOnly={modalReadOnly}
        onHide={() => {
          setModalVisible(false);
          setEditingDetalle(null);
          setEditingIndex(null);
          setModalReadOnly(false);
        }}
        onConfirm={handleConfirmarModal}
      />
    </>
  );
}
