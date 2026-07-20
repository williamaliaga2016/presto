import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox } from "primereact/checkbox";
import type { CheckboxChangeEvent } from "primereact/checkbox";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import { useValidarInformacion } from "@/features/actividades/validar_informacion/hooks/useValidarInformacion";
import type { CargarDocumentosConstructora } from "../models/cargar_documentos_constructora";
import {
  buildInitialState,
  normalizeCargarDocumentosConstructora,
  validateAvanzarFields,
} from "../models/cargar_documentos_constructora";
import { useCargarDocumentosConstructora } from "../hooks/useCargarDocumentosConstructora";
import { useUpsertCargarDocumentosConstructora } from "../hooks/useUpsertCargarDocumentosConstructora";
import { useAvanzarCargarDocumentosConstructora } from "../hooks/useAvanzarCargarDocumentosConstructora";

const ACTIVIDAD_ID = 'BBVA_CONTACTO_CARGAR_DOCUMENTOS_CONSTRUCTORA_45B34EE0';
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
  // CA5: Controla si el documento "Promesa de Compraventa" fue cargado
  const [promesaCargada, setPromesaCargada] = useState(false);

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
      setPromesaCargada(false);
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

  // CA5: Validar que la Promesa de Compraventa esté cargada antes de marcar el check
  const handleAvanzarValidarDocumentosChange = (e: CheckboxChangeEvent) => {
    if (e.checked && !promesaCargada) {
      updateField("avanzar_validar_documentos", false);
      toast.current?.show({
        severity: "error",
        summary: "Documento Faltante",
        detail: "Debe cargar la Promesa de Compraventa en el Expediente Digital antes de avanzar",
        life: 5000,
      });
      return;
    }
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

    // CA5: Verificar documento cargado antes de avanzar
    if (!promesaCargada) {
      toast.current?.show({
        severity: "error",
        summary: "Documento Faltante",
        detail: "Debe cargar la Promesa de Compraventa en el Expediente Digital antes de avanzar",
        life: 5000,
      });
      return;
    }

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

      {/* Alertas de estado del folio */}
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

      <Accordion activeIndex={[0, 2]} multiple>

        {/* CA2: Acordeón 1 - Información General */}
        <AccordionTab
          header="Información General"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <EncabezadoActividad
            idExpediente={id_expediente}
            activityID={ACTIVIDAD_ID}
          />
        </AccordionTab>

        {/* CA3: Acordeón 2 - Funciones Transversales / Expediente Digital */}
        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVIDAD_ID}
            filter_by_activity={true}
            locked_categoria_id={LOCKED_CATEGORIA_ID}
            locked_documento_id={LOCKED_DOCUMENTO_ID}
            show_bitacora={false}
            show_carta_aprobacion={false}
            show_carta_compromiso={false}
            onDocumentUploaded={() => setPromesaCargada(true)}
            onDocumentsLoaded={(docs) =>
              setPromesaCargada(
                docs?.some((d) =>
                  d.id_tipo_documento === LOCKED_DOCUMENTO_ID && d.estado === "CARGADO"
                ) ?? false
              )
            }
          />
        </AccordionTab>

        {/* CA4: Acordeón 3 - Gestión de Actividad */}
        <AccordionTab header="Gestión de Actividad">
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

            <div className="grid grid-cols-1 gap-5">

              {/* CA4: Check de Confirmación obligatorio */}
              <div className="flex flex-col gap-2">
                <label className="font-semibold text-sm">
                  Documentos Adjuntos <span className="text-red-500">*</span>
                </label>
                <div
                  className={`flex items-center gap-2 h-11 rounded-md px-2 ${
                    isInvalid("avanzar_validar_documentos")
                      ? "border border-red-500 bg-red-50"
                      : "border border-gray-200"
                  }`}
                >
                  <Checkbox
                    className="form-checkbox-presto"
                    inputId="avanzar_validar_documentos"
                    checked={form.avanzar_validar_documentos}
                    onChange={handleAvanzarValidarDocumentosChange}
                    disabled={isDisabled || isFolioBlocked}
                  />
                  <label htmlFor="avanzar_validar_documentos" className="text-sm cursor-pointer">
                    Avanzar a Validar Documentos
                  </label>
                </div>
                {isInvalid("avanzar_validar_documentos") && (
                  <small className="text-red-600">Campo obligatorio</small>
                )}
              </div>

              {/* CA4: Observaciones obligatorio */}
              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">
                  Observaciones <span className="text-red-500">*</span>
                </label>
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

            {/* CA6: Botones de acción */}
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
        </AccordionTab>

      </Accordion>
    </>
  );
}
