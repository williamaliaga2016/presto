import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Card } from "primereact/card";
import { Checkbox } from "primereact/checkbox";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { InputTextarea } from "primereact/inputtextarea";
import { RadioButton } from "primereact/radiobutton";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import EditarRolModal from "../components/EditarRolModal";

import type { CalculoGeneracionDocumento } from "../models/calculo_generacion_documento";
import type { CatalogoOption, ControlesPropiedad } from "../models/catalogo";
import { EMPTY_CONTROLES_PROPIEDAD } from "../models/catalogo";
import { useCalculoGeneracionDocumento } from "../hooks/useCalculoGeneracionDocumento";
import { useControlesPropiedad } from "../hooks/useControlesPropiedad";
import { useUpsertCalculoGeneracionDocumento } from "../hooks/useUpsertCalculoGeneracionDocumento";
import { useAvanzarCalculoGeneracionDocumento } from "../hooks/useAvanzarCalculoGeneracionDocumento";
import { useCalcularUF } from "../hooks/useCalcularUF";
import { calculoGeneracionDocumentoService } from "../api/calculoGeneracionDocumentoService";

const ACTIVITY_ID = "5.13.CalculoGeneracionDocumento";

const buildInitialState = (id_expediente: number): CalculoGeneracionDocumento => ({
  id_calculo_generacion_documento: 0,
  id_expediente,
  tipo_propiedad: null,
  tipo_direccion: null,
  direccion: null,
  region: null,
  comuna: null,
  existe_rol_avaluo: false,
  rol_avaluo: null,
  valor_avaluo_pesos: null,
  revision_rol_propiedad: null,
  valor_uf_fecha_hoy: null,
  fecha_calculo: null,
  valor_uf_fecha_calculo: null,
  is_enviar_reparo: false,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalizeForm = (
  source: Partial<CalculoGeneracionDocumento> | null | undefined,
  id_expediente_fallback: number,
): CalculoGeneracionDocumento => ({
  id_calculo_generacion_documento: Number(source?.id_calculo_generacion_documento ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  tipo_propiedad: source?.tipo_propiedad ?? null,
  tipo_direccion: source?.tipo_direccion ?? null,
  direccion: source?.direccion ?? null,
  region: source?.region ?? null,
  comuna: source?.comuna ?? null,
  existe_rol_avaluo: Boolean(source?.existe_rol_avaluo ?? false),
  rol_avaluo: source?.rol_avaluo ?? null,
  valor_avaluo_pesos: source?.valor_avaluo_pesos ?? null,
  revision_rol_propiedad: source?.revision_rol_propiedad ?? null,
  valor_uf_fecha_hoy: source?.valor_uf_fecha_hoy ?? null,
  fecha_calculo: source?.fecha_calculo ?? null,
  valor_uf_fecha_calculo: source?.valor_uf_fecha_calculo ?? null,
  is_enviar_reparo: Boolean(source?.is_enviar_reparo ?? false),
  observaciones: source?.observaciones ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? new Date().toISOString(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

const toDate = (value?: string | null): Date | null => {
  if (!value) return null;

  // Handle date-only values without timezone shifts (yyyy-MM-dd)
  if (/^\d{4}-\d{2}-\d{2}$/.test(value)) {
    const [y, m, d] = value.split("-").map(Number);
    return new Date(y, m - 1, d);
  }

  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? null : parsed;
};

const toDateOnlyString = (value: Date | Date[] | null | undefined): string | null => {
  if (!value || Array.isArray(value)) return null;
  const y = value.getFullYear();
  const m = String(value.getMonth() + 1).padStart(2, "0");
  const d = String(value.getDate()).padStart(2, "0");
  return `${y}-${m}-${d}`;
};

const getTodayDateString = (): string => {
  const d = new Date();
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}-${String(d.getDate()).padStart(2, "0")}`;
};

const resolveLabel = (options: CatalogoOption[], value?: string | null): string => {
  if (!value || !value.trim()) return "-";
  const match = options.find((o) => o.code === value);
  if (match && match.description) return match.description;
  return value;
};

export default function CalculoGeneracionDocumentoPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idParam } = useParams();

  const id_expediente = Number(idParam ?? 0);

  const [form, setForm] = useState<CalculoGeneracionDocumento>(
    buildInitialState(id_expediente),
  );
  const [errorMessage, setErrorMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);
  const [isCalculatingUF, setIsCalculatingUF] = useState(false);
  const [editarRolModalVisible, setEditarRolModalVisible] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useCalculoGeneracionDocumento(id_expediente);
  const saveMutation = useUpsertCalculoGeneracionDocumento();
  const avanzarMutation = useAvanzarCalculoGeneracionDocumento();
  const { data: ufHoyResponse } = useCalcularUF(getTodayDateString());
  const { data: controlesData } = useControlesPropiedad();
  const controles: ControlesPropiedad = controlesData?.detail ?? EMPTY_CONTROLES_PROPIEDAD;

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
      setForm(buildInitialState(id_expediente));
      setErrorMessage("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      setForm(normalizeForm(data.detail, id_expediente));
      setIsDisabled(Number(data.detail.id_calculo_generacion_documento) > 0);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (!id_expediente || id_expediente <= 0) {
      setForm(buildInitialState(0));
      setIsDisabled(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm((prev) =>
        normalizeForm({ ...prev, id_expediente }, id_expediente),
      );
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  useEffect(() => {
    if (ufHoyResponse?.status && ufHoyResponse.detail != null) {
      setForm((prev) =>
        prev.valor_uf_fecha_hoy == null
          ? { ...prev, valor_uf_fecha_hoy: ufHoyResponse.detail }
          : prev,
      );
    }
  }, [ufHoyResponse]);

  const updateField = <K extends keyof CalculoGeneracionDocumento>(
    field: K,
    value: CalculoGeneracionDocumento[K],
  ) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleCalcularUF = async () => {
    if (!form.fecha_calculo) {
      showToast("warn", "Validación", "Seleccione una Fecha de Cálculo.");
      return;
    }

    try {
      setIsCalculatingUF(true);
      const response = await calculoGeneracionDocumentoService.calcularUF(
        form.fecha_calculo,
      );

      if (response.status) {
        updateField("valor_uf_fecha_calculo", response.detail);
        showToast(
          "success",
          "UF Calculada",
          `Valor UF: ${response.detail.toLocaleString("es-CL")}`,
        );
      } else {
        showToast("warn", "Atención", response.message || "No se pudo calcular la UF.");
      }
    } catch {
      showToast("error", "Error", "Error al calcular la UF.");
    } finally {
      setIsCalculatingUF(false);
    }
  };

  const handleEditar = () => {
    setErrorMessage("");
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleEnviarReparoChange = (checked: boolean) => {
    setForm((prev) => ({
      ...prev,
      is_enviar_reparo: checked,
      observaciones: checked ? prev.observaciones : "",
    }));
  };

  const validateForm = (): string => {
    if (!id_expediente || id_expediente <= 0)
      return "No existe un id_expediente válido.";
    if (!form.revision_rol_propiedad)
      return "Debe seleccionar la Revisión del Rol de Propiedad.";
    if (form.is_enviar_reparo && !form.observaciones?.trim())
      return "Debe ingresar Observaciones cuando se envía a reparo.";
    return "";
  };

  const handleGuardar = async () => {
    setErrorMessage("");

    const msg = validateForm();
    if (msg) {
      showToast("warn", "Validación", msg);
      return;
    }

    try {
      setIsBusy(true);
      const payload = normalizeForm(
        { ...form, id_expediente },
        id_expediente,
      );
      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        showToast("success", "Éxito", "Cálculo y Generación Documento guardado correctamente.");
        setForm(normalizeForm(response.detail ?? payload, id_expediente));
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
      } else {
        showToast("warn", "Atención", response.message || "No se pudo guardar.");
      }
    } catch {
      showToast("error", "Error", "Ocurrió un error al guardar.");
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async () => {
    setErrorMessage("");

    const msg = validateForm();
    if (msg) {
      showToast("warn", "Validación", msg);
      return;
    }

    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(id_expediente);

      if (response.status) {
        showToast("success", "Éxito", "Actividad avanzada correctamente.", 2000);
        navigate("/home/bandeja");
      } else {
        const detail = response.message || "No se pudo avanzar.";
        setErrorMessage(detail);
        showToast("warn", "Atención", detail);
      }
    } catch {
      const detail = "Ocurrió un error al avanzar.";
      setErrorMessage(detail);
      showToast("error", "Error", detail);
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => navigate("/home/bandeja");

  const handleEditarRolSuccess = (updatedData: CalculoGeneracionDocumento) => {
    setForm(updatedData);
    setIsDisabled(true);
    setCanAdvance(false);
    hasHydratedRef.current = true;
    showToast("success", "Éxito", "Datos de la propiedad guardados correctamente en la base de datos.");
  };

  const rolPropiedadRows = [
    {
      tipo_propiedad: resolveLabel(controles.tipo_propiedad, form.tipo_propiedad),
      tipo_direccion: resolveLabel(controles.tipo_direccion, form.tipo_direccion),
      direccion: form.direccion ?? "â€”",
      region: resolveLabel(controles.region, form.region),
      comuna: resolveLabel(controles.comuna, form.comuna),
      rol_avaluo: form.rol_avaluo ?? "â€”",
    },
  ];

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Cálculo y Generación Documento
      </h2>

      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab
          header="Información del Expediente"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <EncabezadoActividad
            idExpediente={id_expediente}
            activityID={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab header="Cálculo y Generación Documento">
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

            <div className="mb-5">
              <div className="bg-blue-700 text-white font-semibold px-3 py-2 text-sm mb-2">
                ROL DE PROPIEDAD
              </div>
              <DataTable
                value={rolPropiedadRows}
                size="small"
                showGridlines
                className="text-sm"
              >
                <Column field="tipo_propiedad" header="Tipo de Propiedad" />
                <Column field="tipo_direccion" header="Tipo de Dirección" />
                <Column field="direccion" header="Dirección" />
                <Column field="region" header="Región" />
                <Column field="comuna" header="Comuna" />
                <Column field="rol_avaluo" header="Rol Avalúo" />
                <Column
                  header="Editar"
                  body={() => (
                    <Button
                      label="Editar Rol"
                      size="small"
                      severity="info"
                      outlined
                      disabled={!id_expediente || id_expediente <= 0 || isBusy}
                      onClick={() => {
                        if (isDisabled) {
                          handleEditar();
                        }
                        setEditarRolModalVisible(true);
                      }}
                    />
                  )}
                  style={{ width: "130px" }}
                />
              </DataTable>
            </div>

            <div className="flex items-center gap-6 mb-5 flex-wrap">
              <span className="font-semibold text-sm">
                Revisión Rol Propiedad:
              </span>
              <div className="flex items-center gap-2">
                <RadioButton
                  inputId="rev_incorrecto"
                  value="ROL_INCORRECTO"
                  checked={form.revision_rol_propiedad === "ROL_INCORRECTO"}
                  onChange={() =>
                    updateField("revision_rol_propiedad", "ROL_INCORRECTO")
                  }
                  disabled={isDisabled}
                />
                <label htmlFor="rev_incorrecto" className="text-sm">
                  Rol Incorrecto
                </label>
              </div>
              <div className="flex items-center gap-2">
                <RadioButton
                  inputId="rev_aceptar"
                  value="ACEPTAR"
                  checked={form.revision_rol_propiedad === "ACEPTAR"}
                  onChange={() =>
                    updateField("revision_rol_propiedad", "ACEPTAR")
                  }
                  disabled={isDisabled}
                />
                <label htmlFor="rev_aceptar" className="text-sm">
                  Aceptar
                </label>
              </div>
            </div>

            <div className="mb-5">
              <div className="bg-blue-700 text-white font-semibold px-3 py-2 text-sm mb-3">
                ANTECEDENTES VARIOS
              </div>
              <div className="grid grid-cols-1 md:grid-cols-[1.2fr_1.4fr_0.95fr_1.2fr] gap-4 items-end md:items-center">
                <div className="flex flex-col gap-1">
                  <span className="font-semibold text-sm">
                    Valor UF Fecha de hoy
                  </span>
                  <span className="text-sm font-medium text-gray-700 h-8 flex items-center">
                    {form.valor_uf_fecha_hoy !== null
                      ? form.valor_uf_fecha_hoy.toLocaleString("es-CL")
                      : "â€”"}
                  </span>
                </div>

                <div className="flex flex-col gap-1">
                  <label className="font-semibold text-sm">
                    Fecha de Cálculo
                  </label>
                  <Calendar
                    value={toDate(form.fecha_calculo)}
                    onChange={(e) =>
                      updateField("fecha_calculo", toDateOnlyString(e.value))
                    }
                    dateFormat="dd/mm/yy"
                    showIcon
                    inputClassName="form-input-presto w-full"
                    className="w-full"
                    disabled={isDisabled}
                    placeholder="dd/mm/aaaa"
                  />
                </div>

                <div className="flex flex-col gap-1 justify-end md:justify-center">
                  <span className="font-semibold text-sm opacity-0 select-none">
                    Acción
                  </span>
                  <Button
                    label={isCalculatingUF ? "Calculando..." : "Calcular UF"}
                    icon="pi pi-calculator"
                    severity="info"
                    size="small"
                    onClick={handleCalcularUF}
                    disabled={isDisabled || isCalculatingUF || !form.fecha_calculo}
                    className="btn-responsive px-3 whitespace-nowrap calcular-uf-button w-full md:w-auto"
                  />
                </div>

                <div className="flex flex-col gap-1">
                  <span className="font-semibold text-sm">
                    Valor UF Fecha de Cálculo
                  </span>
                  <span className="text-sm font-medium text-gray-700 h-8 flex items-center">
                    {form.valor_uf_fecha_calculo !== null
                      ? form.valor_uf_fecha_calculo.toLocaleString("es-CL")
                      : "â€”"}
                  </span>
                </div>
              </div>
            </div>

            <div className="mb-5">
              <div className="bg-blue-700 text-white font-semibold px-3 py-2 text-sm mb-3">
                ESTATUS DE LA ACTIVIDAD
              </div>

              <div className="flex items-center gap-2 mb-4 h-10">
                <Checkbox
                  inputId="is_enviar_reparo"
                  checked={form.is_enviar_reparo}
                  className="calculo-checkbox-visible"
                  onChange={(e) => handleEnviarReparoChange(Boolean(e.checked))}
                  disabled={isDisabled}
                />
                <label htmlFor="is_enviar_reparo" className="text-sm font-semibold">
                  ¿Enviar a Reparo?
                </label>
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">
                  Observaciones{form.is_enviar_reparo ? " *" : ""}
                </label>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) => updateField("observaciones", e.target.value)}
                  rows={4}
                  autoResize
                  className="form-textarea-presto w-full"
                  disabled={isDisabled}
                  placeholder={
                    form.is_enviar_reparo
                      ? "Ingrese el motivo del reparo"
                      : "Ingrese observaciones (opcional)"
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

      <EditarRolModal
        visible={editarRolModalVisible}
        idExpediente={id_expediente}
        currentData={{
          id_expediente,
          tipo_propiedad: form.tipo_propiedad,
          tipo_direccion: form.tipo_direccion,
          direccion: form.direccion,
          region: form.region,
          comuna: form.comuna,
          existe_rol_avaluo: form.existe_rol_avaluo,
          rol_avaluo: form.rol_avaluo,
          valor_avaluo_pesos: form.valor_avaluo_pesos,
        }}
        completeForm={form}
        controles={controles}
        onHide={() => setEditarRolModalVisible(false)}
        onSuccess={handleEditarRolSuccess}
      />
    </>
  );
}
