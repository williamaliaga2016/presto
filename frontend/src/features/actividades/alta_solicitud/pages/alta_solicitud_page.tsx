import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { Dropdown, DropdownChangeEvent } from "primereact/dropdown";
import { Calendar } from "primereact/calendar";
import { InputText } from "primereact/inputtext";
import {
  InputNumber,
  InputNumberValueChangeEvent,
} from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";

import type { AltaSolicitud } from "../models/alta_solicitud";
import { useAltaSolicitud } from "../hooks/useAltaSolicitud";
import { useUpsertAltaSolicitud } from "../hooks/useUpsertAltaSolicitud";
import { useAvanzarAltaSolicitud } from "../hooks/useAvanzarAltaSolicitud";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import { Accordion, AccordionTab } from "primereact/accordion";
import BitacoraPage from "@/features/funciones_transversales/components/bitacora/pages/Bitacora";
import { Toast } from "primereact/toast";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

const ACTIVITY_ID = "_4X3k8H6kEeeGvo3jOJ1wYw";

const buildInitialState = (id_expediente: number): AltaSolicitud => ({
  id_alta_solicitud: 0,
  id_expediente,
  id_tipo_moneda: null,
  id_tipo_documento: null,
  numero_documento: "",
  nombre_razon_social: "",
  fecha_emision: null,
  nro_comprobante: "",
  comprobante_detalle: "",
  importe: 0,
  fecha_recepcion_factura: null,
  fecha_vencimiento: null,
  observaciones: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeAltaSolicitud = (
  source: Partial<AltaSolicitud> | null | undefined,
  id_expediente_fallback: number,
): AltaSolicitud => {
  return {
    id_alta_solicitud: Number(source?.id_alta_solicitud ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    id_tipo_moneda: source?.id_tipo_moneda ?? null,
    id_tipo_documento: source?.id_tipo_documento ?? null,
    numero_documento: source?.numero_documento ?? "",
    nombre_razon_social: source?.nombre_razon_social ?? "",
    fecha_emision: source?.fecha_emision ?? null,
    nro_comprobante: source?.nro_comprobante ?? "",
    comprobante_detalle: source?.comprobante_detalle ?? "",
    importe: Number(source?.importe ?? 0),
    fecha_recepcion_factura: source?.fecha_recepcion_factura ?? null,
    fecha_vencimiento: source?.fecha_vencimiento ?? null,
    observaciones: source?.observaciones ?? "",
    is_active: source?.is_active ?? true,
    row_status: source?.row_status ?? true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
  };
};

export default function AltaSolicitudPage() {

  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<AltaSolicitud>(buildInitialState(id_expediente));
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useAltaSolicitud(id_expediente);
  const saveMutation = useUpsertAltaSolicitud();
  const avanzarMutation = useAvanzarAltaSolicitud();

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;

      setForm(buildInitialState(id_expediente));
      setErrorMessage("");
      setSuccessMessage("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const loadedEntity = normalizeAltaSolicitud(data.detail, id_expediente);

      setForm(loadedEntity);
      setIsDisabled(Number(data.detail.id_alta_solicitud) > 0);

      // IMPORTANTE:
      // Al entrar a una actividad existente, Avanzar debe seguir deshabilitado
      // y solo habilitarse después de guardar correctamente.
      setCanAdvance(false);

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
        normalizeAltaSolicitud(
          {
            ...prev,
            id_expediente,
          },
          id_expediente,
        ),
      );
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  const tipoMonedaOptions = [
    { label: "Soles", value: 1 },
    { label: "Dólares", value: 2 },
  ];

  const tipoDocumentoOptions = [
    { label: "Factura", value: 1 },
    { label: "Boleta", value: 2 },
    { label: "Recibo por Honorarios", value: 3 },
    { label: "Nota de Crédito", value: 4 },
  ];

  const toDate = (value?: string | null) => {
    return value ? new Date(value) : null;
  };

  const toIsoString = (value: Date | Date[] | null | undefined) => {
    if (!value || Array.isArray(value)) return null;
    return value.toISOString();
  };

  const updateField = <K extends keyof AltaSolicitud>(
    field: K,
    value: AltaSolicitud[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleImporteChange = (e: InputNumberValueChangeEvent) => {
    updateField("importe", e.value ?? 0);
  };

  const handleEditar = () => {
    setErrorMessage("");
    setSuccessMessage("");
    setIsDisabled(false);

    // IMPORTANTE:
    // Al editar, Avanzar debe seguir deshabilitado
    setCanAdvance(false);
  };

  const validateForm = () => {
    if (!form.id_tipo_moneda) return "Debe seleccionar tipo moneda.";
    if (!form.id_tipo_documento) return "Debe seleccionar tipo documento.";
    if (!form.numero_documento?.trim()) return "Debe ingresar número documento.";
    if (!form.nombre_razon_social?.trim()) return "Debe ingresar nombre o razón social.";
    if (!form.importe || form.importe <= 0) return "Debe ingresar un importe mayor a 0.";
    return "";
  };

  const handleGuardar = async () => {
    setErrorMessage("");
    setSuccessMessage("");

    const validationMessage = validateForm();
    if (validationMessage) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: validationMessage,
        life: 3000,
      });
      return;
    }


    try {
      setIsBusy(true);

      const payload: AltaSolicitud = normalizeAltaSolicitud(
        {
          ...form,
          id_alta_solicitud: Number(form.id_alta_solicitud ?? 0),
          id_expediente: Number(form.id_expediente ?? 0),
        },
        id_expediente,
      );

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Alta solicitud guardada correctamente",
          life: 3000,
        });

        const savedEntity = normalizeAltaSolicitud(
          response.detail ?? payload,
          payload.id_expediente,
        );

        setForm(savedEntity);
        setIsDisabled(true);

        // IMPORTANTE:
        // Solo aquí se habilita Avanzar
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
    } catch (error) {
      console.error("ERROR GUARDAR", error);

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
    setSuccessMessage("");

    const expedienteId = Number(form.id_expediente ?? 0);

    if (!expedienteId || expedienteId <= 0) {
      setErrorMessage("No existe un id_expediente válido para avanzar.");
      return;
    }

    try {

      const response = await avanzarMutation.mutateAsync(expedienteId);

      const msg = "Actividad avanzada correctamente";
      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: msg,
          life: 2000,
        });
        navigate("/home/bandeja");
      } else {
        const msg = response.message || "No se pudo avanzar la actividad.";

        setErrorMessage(msg);
        setSuccessMessage("");

        toast.current?.show({
          severity: "warn",
          summary: "Atención",
          detail: msg,
          life: 3000,
        });
      }
    } catch (error) {
      console.error("ERROR AVANZAR", error);
      const msg = "Ocurrió un error al avanzar.";

      setErrorMessage(msg);
      setSuccessMessage("");

      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: msg,
        life: 3000,
      });
    }
  };

  const handleSalir = () => {
    navigate("/home/bandeja");
  };

  return (
    <>

      <Toast ref={toast} />
      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Alta de Solicitud
      </h2>

      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab disabled={!id_expediente || id_expediente <= 0}
          header="Información del Expediente">
          <EncabezadoActividad
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            activityID={ACTIVITY_ID}
          />
        </AccordionTab>

        {/* ===== FUNCIONES TRANSVERSALES ===== */}
        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            idActividad={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab header="Alta de Solicitud">
          <Card className="w-full shadow-md card-presto-form mb-6">
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

            {successMessage && (
              <div className="mb-4 rounded-md border border-green-200 bg-green-50 px-4 py-3 text-sm text-green-700">
                {successMessage}
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

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Tipo Moneda *</label>
                <Dropdown
                  value={form.id_tipo_moneda}
                  options={tipoMonedaOptions}
                  onChange={(e: DropdownChangeEvent) =>
                    updateField("id_tipo_moneda", e.value)
                  }
                  placeholder="Seleccione tipo moneda"
                  className="form-dropdown-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Tipo Documento *</label>
                <Dropdown
                  value={form.id_tipo_documento}
                  options={tipoDocumentoOptions}
                  onChange={(e: DropdownChangeEvent) =>
                    updateField("id_tipo_documento", e.value)
                  }
                  placeholder="Seleccione tipo documento"
                  className="form-dropdown-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Número Documento *</label>
                <InputText
                  value={form.numero_documento ?? ""}
                  onChange={(e) => updateField("numero_documento", e.target.value)}
                  className="form-input-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1 md:col-span-2">
                <label className="font-semibold text-sm">Nombre / Razón Social *</label>
                <InputText
                  value={form.nombre_razon_social ?? ""}
                  onChange={(e) => updateField("nombre_razon_social", e.target.value)}
                  className="form-input-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Fecha Emisión</label>
                <Calendar
                  value={toDate(form.fecha_emision)}
                  onChange={(e) => updateField("fecha_emision", toIsoString(e.value))}
                  dateFormat="dd/mm/yy"
                  showIcon
                  className="form-input-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Nro. Comprobante</label>
                <InputText
                  value={form.nro_comprobante ?? ""}
                  onChange={(e) => updateField("nro_comprobante", e.target.value)}
                  className="form-input-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Comprobante Detalle</label>
                <InputText
                  value={form.comprobante_detalle ?? ""}
                  onChange={(e) => updateField("comprobante_detalle", e.target.value)}
                  className="form-input-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Importe *</label>
                <InputNumber
                  value={form.importe}
                  onValueChange={handleImporteChange}
                  className="form-input-presto w-full"
                  mode="currency"
                  currency="PEN"
                  locale="es-PE"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Fecha Recepción Factura</label>
                <Calendar
                  value={toDate(form.fecha_recepcion_factura)}
                  onChange={(e) =>
                    updateField("fecha_recepcion_factura", toIsoString(e.value))
                  }
                  dateFormat="dd/mm/yy"
                  showIcon
                  className="form-input-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Fecha Vencimiento</label>
                <Calendar
                  value={toDate(form.fecha_vencimiento)}
                  onChange={(e) =>
                    updateField("fecha_vencimiento", toIsoString(e.value))
                  }
                  dateFormat="dd/mm/yy"
                  showIcon
                  className="form-input-presto w-full"
                  disabled={isDisabled}
                />
              </div>

              <div className="flex flex-col gap-1 md:col-span-3">
                <label className="font-semibold text-sm">Observaciones</label>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) => updateField("observaciones", e.target.value)}
                  rows={5}
                  className="form-textarea-presto w-full"
                  disabled={isDisabled}
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
    </>
  );
}