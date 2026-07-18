import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox } from "primereact/checkbox";
import type { CheckboxChangeEvent } from "primereact/checkbox";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { TabView, TabPanel } from "primereact/tabview";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import ExpedienteDigitalPage from "@/features/funciones_transversales/components/expediente_digital/pages/ExpedienteDigital";
import { useValidarInformacion } from "@/features/actividades/validar_informacion/hooks/useValidarInformacion";
import type { CargarDocumentosConstructora } from "../models/cargar_documentos_constructora";
import {
  buildInitialState,
  normalizeCargarDocumentosConstructora,
  validateAvanzarFields,
} from "../models/cargar_documentos_constructora.form";
import { useCargarDocumentosConstructora } from "../hooks/useCargarDocumentosConstructora";
import { useUpsertCargarDocumentosConstructora } from "../hooks/useUpsertCargarDocumentosConstructora";
import { useAvanzarCargarDocumentosConstructora } from "../hooks/useAvanzarCargarDocumentosConstructora";

const ACTIVIDAD_ID = 'ACT_DOCS_CONSTRUCTORA';
const LOCKED_CATEGORIA_ID = 100;
const LOCKED_DOCUMENTO_ID = 1001;

export default function CargarDocumentosConstructoraPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<CargarDocumentosConstructora>(
    buildInitialState(id_expediente),
  );
  const [errorMessage, setErrorMessage] = useState("");
  const [invalidFields, setInvalidFields] = useState<Set<string>>(new Set());
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useCargarDocumentosConstructora(id_expediente);
  const { data: validarInfoData, isLoading: isLoadingValidarInfo } =
    useValidarInformacion(id_expediente);
  const saveMutation = useUpsertCargarDocumentosConstructora();
  const avanzarMutation = useAvanzarCargarDocumentosConstructora();

  const isFolioBlocked = useMemo(() => {
    if (!id_expediente || id_expediente <= 0) return false;
    if (isLoadingValidarInfo) return false;

    const validarInfo = validarInfoData?.detail;
    if (!validarInfoData?.status || !validarInfo) return true;

    return validarInfo.es_constructora_vip !== true;
  }, [id_expediente, isLoadingValidarInfo, validarInfoData]);

  const isOperationDisabled = isBusy || isFolioBlocked;

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialState(id_expediente));
      setErrorMessage("");
      setInvalidFields(new Set());
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      setForm(normalizeCargarDocumentosConstructora(data.detail, id_expediente));
      setIsDisabled(true);
      setCanAdvance(Boolean(data.detail.id && data.detail.id > 0));
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
        normalizeCargarDocumentosConstructora({ ...prev, id_expediente }, id_expediente),
      );
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  const clearInvalidField = (field: string) => {
    setInvalidFields((prev) => {
      if (!prev.has(field)) return prev;
      const next = new Set(prev);
      next.delete(field);
      return next;
    });
  };

  const updateField = <K extends keyof CargarDocumentosConstructora>(
    field: K,
    value: CargarDocumentosConstructora[K],
  ) => {
    clearInvalidField(field as string);
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleAvanzarValidarDocumentosChange = (e: CheckboxChangeEvent) => {
    clearInvalidField("avanzar_validar_documentos");
    updateField("avanzar_validar_documentos", Boolean(e.checked));
  };

  const handleEditar = () => {
    setErrorMessage("");
    setInvalidFields(new Set());
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    setErrorMessage("");
    setInvalidFields(new Set());

    if (isFolioBlocked) return;

    if (!form.id_expediente || form.id_expediente <= 0) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: "No existe un id_expediente válido.",
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const payload = normalizeCargarDocumentosConstructora(
        { ...form, id: Number(form.id ?? 0) },
        id_expediente,
      );

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Cargar Documentos Constructora guardado correctamente",
          life: 3000,
        });

        setForm(
          normalizeCargarDocumentosConstructora(
            response.detail ?? payload,
            id_expediente,
          ),
        );
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
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
    setErrorMessage("");

    if (isFolioBlocked) return;

    const missing = validateAvanzarFields(form);
    if (missing.size > 0) {
      setInvalidFields(missing);
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: "Datos Obligatorios Faltantes",
        life: 4000,
      });
      return;
    }

    setInvalidFields(new Set());

    try {
      setIsBusy(true);

      const response = await avanzarMutation.mutateAsync(form.id_expediente);

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
        toast.current?.show({
          severity: "warn",
          summary: "Atención",
          detail: msg,
          life: 4000,
        });
      }
    } catch {
      const msg = "Ocurrió un error al avanzar.";
      setErrorMessage(msg);
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

  const isInvalid = (field: string) => invalidFields.has(field);

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Cargar Documentos Constructora
      </h2>

      {isLoadingValidarInfo && id_expediente > 0 && (
        <div className="mb-4 rounded-md border border-blue-200 bg-blue-50 px-4 py-3 text-sm text-blue-700">
          Validando elegibilidad del folio...
        </div>
      )}

      {isFolioBlocked && (
        <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          Este folio no corresponde a una constructora VIP. No es posible operar esta actividad.
        </div>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
          <div className="lg:col-span-2 flex flex-col gap-4">
            <TabView>
              <TabPanel header="Expediente Digital" leftIcon="pi pi-list">
                <ExpedienteDigitalPage
                  id_expediente={id_expediente}
                  activity_id={ACTIVIDAD_ID}
                  filter_by_activity={true}
                  locked_categoria_id={LOCKED_CATEGORIA_ID}
                  locked_documento_id={LOCKED_DOCUMENTO_ID}
                  read_only={isFolioBlocked}
                />
              </TabPanel>
            </TabView>

            <Card className="w-full shadow-md card-presto-form">
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

          <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Expediente</label>
              <InputNumber
                value={form.id_expediente}
                className="form-input-presto w-full"
                useGrouping={false}
                disabled
              />
            </div>

            <div className="flex flex-col gap-2 md:col-span-3">
              <label className="font-semibold text-sm">Confirmación de documentos</label>
              <div
                className={`flex items-center gap-2 h-11 rounded-md px-2 ${
                  isInvalid("avanzar_validar_documentos")
                    ? "border border-red-500 bg-red-50"
                    : ""
                }`}
              >
                <Checkbox
                  className="form-checkbox-presto"
                  inputId="avanzar_validar_documentos"
                  checked={form.avanzar_validar_documentos}
                  onChange={handleAvanzarValidarDocumentosChange}
                  disabled={isDisabled || isFolioBlocked}
                />
                <label htmlFor="avanzar_validar_documentos" className="text-sm">
                  Confirmo que los documentos de la constructora han sido cargados correctamente en el Expediente Digital
                </label>
              </div>
              {isInvalid("avanzar_validar_documentos") && (
                <small className="text-red-600">Campo obligatorio</small>
              )}
            </div>

            <div className="flex flex-col gap-1 md:col-span-3">
              <label className="font-semibold text-sm">Observaciones</label>
              <InputTextarea
                value={form.observaciones ?? ""}
                onChange={(e) => updateField("observaciones", e.target.value)}
                rows={5}
                autoResize
                className={`form-textarea-presto w-full${
                  isInvalid("observaciones") ? " p-invalid" : ""
                }`}
                disabled={isDisabled || isFolioBlocked}
                placeholder="Ingrese observaciones"
              />
              {isInvalid("observaciones") && (
                <small className="text-red-600">Campo obligatorio</small>
              )}
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
              disabled={isOperationDisabled || !isDisabled}
              className="btn-responsive"
            />

            <Button
              type="button"
              label={saveMutation.isPending ? "Guardando..." : "Guardar"}
              icon="pi pi-save"
              severity="success"
              onClick={handleGuardar}
              disabled={isOperationDisabled || isDisabled}
              className="btn-responsive"
            />

            <Button
              type="button"
              label={avanzarMutation.isPending ? "Avanzando..." : "Avanzar"}
              icon="pi pi-arrow-right"
              severity="warning"
              onClick={handleAvanzar}
              disabled={isOperationDisabled || !canAdvance}
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
          </div>

          <div className="lg:col-span-1">
            <Card className="shadow-md card-presto-form">
              <h3 className="text-lg font-semibold mb-3">Información General</h3>
              <EncabezadoActividad
                idExpediente={id_expediente}
                activityID={ACTIVIDAD_ID}
              />
            </Card>
          </div>
        </div>
    </>
  );
}

