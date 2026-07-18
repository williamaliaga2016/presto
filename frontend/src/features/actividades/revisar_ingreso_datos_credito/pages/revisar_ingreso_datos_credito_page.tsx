import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Toast } from "primereact/toast";

import type { RevisarIngresoDatosCredito } from "../models/revisar_ingreso_datos_credito";
import { useRevisarIngresoDatosCredito } from "../hooks/useRevisarIngresoDatosCredito";
import { useUpsertRevisarIngresoDatosCredito } from "../hooks/useUpsertRevisarIngresoDatosCredito";
import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import { InputText } from "primereact/inputtext";
import { Dropdown } from "primereact/dropdown";
import { EMPTY_CONTROLES_DATOS_CREDITO } from "../models/catalogo";
import { now } from "../../datos_operacion/models/datos_operacion";
import { useControlesDatosCredito } from "../hooks/useControlesDatosCredito";
import { RadioButton } from "primereact/radiobutton";

const ACTIVITY_ID = "_P1rFxU9vEeiJs6_Y4tW0n";

const buildInitialState = (id_expediente: number): RevisarIngresoDatosCredito => ({
  id_revisar_ingreso_datos_credito: 0,
  id_datos_operacion: 1,
  id_expediente,
  ubicacion: '',
  tipo_operacion: null,
  fines_generales: null,
  nombre_proyecto: '',
  credito_segunda_vivienda: false,
  inmobiliaria: null,
  vivienda_social: null,
  dfl2: null,
  propietario_dfl2: null,
  recepcion_final_mayor_2_anios: null,
  porcentaje_impuesto: null,
  monto_credito_afecto_impuesto: null,
  impuesto_a_pagar: null,
  enviar_a_reparo: null,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

const normalizeRevisarIngresoDatosCredito = (
  source: Partial<RevisarIngresoDatosCredito> | null | undefined,
  id_expediente_fallback: number,
): RevisarIngresoDatosCredito => {
  return {
    id_revisar_ingreso_datos_credito: Number(source?.id_revisar_ingreso_datos_credito ?? 0),
    id_datos_operacion: Number(source?.id_datos_operacion ?? 0),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    ubicacion: source?.ubicacion ?? "",
    tipo_operacion: source?.tipo_operacion ?? "",
    fines_generales: Boolean(source?.fines_generales ?? false),
    nombre_proyecto: source?.nombre_proyecto ?? "",
    credito_segunda_vivienda: Boolean(source?.credito_segunda_vivienda ?? false),
    inmobiliaria: source?.inmobiliaria ?? "",
    vivienda_social: Boolean(source?.vivienda_social ?? false),
    dfl2: Boolean(source?.dfl2 ?? false),
    propietario_dfl2: Boolean(source?.propietario_dfl2 ?? false),
    recepcion_final_mayor_2_anios: Boolean(source?.recepcion_final_mayor_2_anios ?? false),
    porcentaje_impuesto: Number(source?.porcentaje_impuesto ?? 0),
    monto_credito_afecto_impuesto: Number(source?.monto_credito_afecto_impuesto ?? 0),
    impuesto_a_pagar: Number(source?.impuesto_a_pagar ?? 0),
    enviar_a_reparo: Boolean(source?.enviar_a_reparo ?? false),
    observaciones: source?.observaciones ?? "",
    is_active: source?.is_active ?? true,
    row_status: source?.row_status ?? true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? new Date().toISOString(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,

  };
};

const boolToCatalogCode = (value?: boolean | null) => {
  if (value === null || value === undefined) return null;
  return value ? '001' : '002';
};

const catalogCodeToBool = (value?: string | null) => {
  if (!value) return null;
  return ['001', '01', '1', 'S'].includes(value);
};

const fieldClass = 'flex flex-col gap-1';
const labelClass = 'font-semibold text-sm text-gray-700';
const inputClass = 'form-input-presto w-full';
const dropdownClass = 'form-dropdown-presto w-full';

export default function RevisarIngresoDatosCreditoPage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);
  const [form, setForm] = useState<RevisarIngresoDatosCredito>(
    buildInitialState(id_expediente),
  );
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);

  const hasValidExpediente = id_expediente > 0;

  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRevisarIngresoDatosCredito(id_expediente);
  const controlesDatosCreditoQuery = useControlesDatosCredito(hasValidExpediente);
  const controles = controlesDatosCreditoQuery.data?.status
    ? controlesDatosCreditoQuery.data.detail ?? EMPTY_CONTROLES_DATOS_CREDITO
    : EMPTY_CONTROLES_DATOS_CREDITO;
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const saveMutation = useUpsertRevisarIngresoDatosCredito();

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
      const loadedEntity = normalizeRevisarIngresoDatosCredito(
        data.detail,
        id_expediente,
      );

      setForm(loadedEntity);
      setIsDisabled(Number(data.detail.id_revisar_ingreso_datos_credito) > 0);
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
        normalizeRevisarIngresoDatosCredito(
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

  const updateField = <K extends keyof RevisarIngresoDatosCredito>(
    field: K,
    value: RevisarIngresoDatosCredito[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };



  const handleEnviarReparoChange = (e: CheckboxChangeEvent) => {
    const checked = Boolean(e.checked);

    setForm((prev) => ({
      ...prev,
      enviar_a_reparo: checked,
      observaciones: checked ? prev.observaciones : "",
    }));
  };

  const handleEditar = () => {
    setErrorMessage("");
    setSuccessMessage("");
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const validateForm = () => {
    if (form.id_expediente < 0) {
      return "No existe un id_expediente válido.";
    }

    if (form.enviar_a_reparo && !form.observaciones?.trim()) {
      return "Debe ingresar observaciones cuando se envía a reparo.";
    }

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

      const payload: RevisarIngresoDatosCredito = normalizeRevisarIngresoDatosCredito(
        {
          ...form,
          id_revisar_ingreso_datos_credito: Number(
            form.id_revisar_ingreso_datos_credito ?? 0,
          ),
          id_expediente: Number(form.id_expediente || id_expediente || 0),
        },
        id_expediente,
      );

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Recepción carga fábrica guardada correctamente",
          life: 3000,
        });

        const savedEntity = normalizeRevisarIngresoDatosCredito(
          response.detail ?? payload,
          payload.id_expediente,
        );

        setForm(savedEntity);
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

  const handleSalir = () => {
    navigate("/home/bandeja");
  };

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Recepción Carga Fábrica
      </h2>

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

        <AccordionTab header="Recepción Carga Fábrica">
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

            <div className="space-y-5">
              <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
                <div className="mb-4 border-b border-gray-100 pb-3">
                  <h3 className="text-base font-semibold text-gray-800">Datos del Crédito</h3>
                  <p className="text-sm text-gray-500">Información general del crédito asociado al expediente.</p>
                </div>

                <div className="grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
                  <div className={fieldClass}>
                    <label className={labelClass}>Ubicación *</label>
                    <InputText
                      value={form?.ubicacion ?? ''}
                      disabled={isDisabled}
                      className={inputClass}
                      onChange={(e) => updateField('ubicacion', e.target.value)}
                    />
                  </div>

                  <div className={fieldClass}>
                    <label className={labelClass}>Tipo Operación *</label>
                    <Dropdown
                      value={form?.tipo_operacion ?? null}
                      options={controles.tipo_operacion}
                      optionLabel="description"
                      optionValue="code"
                      disabled={isDisabled}
                      className={dropdownClass}
                      placeholder="Seleccione"
                      onChange={(e) => updateField('tipo_operacion', e.value)}
                      showClear
                    />
                  </div>


                  <div className={fieldClass}>
                    <label className={labelClass}>Fines Generales *</label>

                    <div className="flex items-center gap-6 h-10">
                      <div className="flex items-center gap-2">
                        <RadioButton
                          className="form-radio-presto"
                          inputId="fines_generales_si"
                          checked={form?.fines_generales === true}
                          disabled={isDisabled}
                          onChange={() => updateField('fines_generales', true)}
                        />

                        <label
                          htmlFor="fines_generales_si"
                          className="text-sm text-gray-700"
                        >
                          Sí
                        </label>
                      </div>

                      <div className="flex items-center gap-2">
                        <RadioButton
                          className="form-radio-presto"
                          inputId="fines_generales_no"
                          checked={form?.fines_generales === false}
                          disabled={isDisabled}
                          onChange={() => updateField('fines_generales', false)}
                        />

                        <label
                          htmlFor="fines_generales_no"
                          className="text-sm text-gray-700"
                        >
                          No
                        </label>
                      </div>
                    </div>
                  </div>
                  <div className={fieldClass}>
                    <label className={labelClass}>Nombre Proyecto *</label>
                    <InputText
                      value={form?.nombre_proyecto ?? ''}
                      disabled={isDisabled}
                      className={inputClass}
                      onChange={(e) => updateField('nombre_proyecto', e.target.value)}
                    />
                  </div>

                  <div className={fieldClass}>
                    <label className={labelClass}>
                      Crédito Segunda Vivienda *
                    </label>

                    <div className="flex items-center h-10">
                      <div className="flex items-center gap-2">
                        <Checkbox
                          inputId="credito_segunda_vivienda"
                          className="form-checkbox-presto"
                          checked={!!form?.credito_segunda_vivienda}
                          disabled={isDisabled}
                          onChange={(e) =>
                            updateField(
                              'credito_segunda_vivienda',
                              !!e.checked
                            )
                          }
                        />

                        <label
                          htmlFor="credito_segunda_vivienda"
                          className="text-sm text-gray-700"
                        >
                          Sí
                        </label>
                      </div>
                    </div>
                  </div>
                  <div className={fieldClass}>
                    <label className={labelClass}>Inmobiliaria *</label>
                    <InputText
                      value={form?.inmobiliaria ?? ''}
                      disabled={isDisabled}
                      className={inputClass}
                      onChange={(e) => updateField('inmobiliaria', e.target.value)}
                    />
                  </div>
                </div>
              </div>

              <div className="rounded-xl border border-gray-200 bg-white p-4 sm:p-5 shadow-sm">
                <div className="mb-4 border-b border-gray-100 pb-3">
                  <h3 className="text-base font-semibold text-gray-800">Condiciones de la Propiedad</h3>
                  <p className="text-sm text-gray-500">Indicadores y datos tributarios del crédito.</p>
                </div>
                <div className="overflow-hidden border border-gray-200 bg-white">
                  <table className="w-full border-collapse">
                    <thead>
                      <tr className="bg-blue-600 text-white">
                        <th className="border border-gray-300 px-3 py-2 text-left">
                          Descripción
                        </th>

                        <th className="border border-gray-300 px-3 py-2 text-center">
                          Sí
                        </th>

                        <th className="border border-gray-300 px-3 py-2 text-center">
                          No
                        </th>
                      </tr>
                    </thead>

                    <tbody>
                      <tr>
                        <td className="border border-gray-300 px-3 py-2">
                          Propiedad adquirida es Vivienda Social
                        </td>

                        <td className="border border-gray-300 text-center">
                          <RadioButton
                            className="form-radio-presto"
                            inputId="vivienda_social_si"
                            checked={form?.vivienda_social === true}
                            disabled={isDisabled}
                            onChange={() => updateField('vivienda_social', true)}
                          />
                        </td>

                        <td className="border border-gray-300 text-center">
                          <RadioButton
                            className="form-radio-presto"
                            inputId="vivienda_social_no"
                            checked={form?.vivienda_social === false}
                            disabled={isDisabled}
                            onChange={() => updateField('vivienda_social', false)}
                          />
                        </td>
                      </tr>

                      <tr>
                        <td className="border border-gray-300 px-3 py-2">
                          Propiedad adquirida es DFL 2
                        </td>

                        <td className="border border-gray-300 text-center">
                          <RadioButton
                            className="form-radio-presto"
                            inputId="dfl2_si"
                            checked={form?.dfl2 === true}
                            disabled={isDisabled}
                            onChange={() => updateField('dfl2', true)}
                          />
                        </td>

                        <td className="border border-gray-300 text-center">
                          <RadioButton
                            className="form-radio-presto"
                            inputId="dfl2_no"
                            checked={form?.dfl2 === false}
                            disabled={isDisabled}
                            onChange={() => updateField('dfl2', false)}
                          />
                        </td>
                      </tr>

                      <tr>
                        <td className="border border-gray-300 px-3 py-2">
                          Comprador propietario 0 ó 1 vivienda DFL 2
                        </td>

                        <td className="border border-gray-300 text-center">
                          <RadioButton
                            className="form-radio-presto"
                            inputId="propietario_dfl2_si"
                            checked={form?.propietario_dfl2 === true}
                            disabled={isDisabled}
                            onChange={() => updateField('propietario_dfl2', true)}
                          />
                        </td>

                        <td className="border border-gray-300 text-center">
                          <RadioButton
                            className="form-radio-presto"
                            inputId="propietario_dfl2_no"
                            checked={form?.propietario_dfl2 === false}
                            disabled={isDisabled}
                            onChange={() => updateField('propietario_dfl2', false)}
                          />
                        </td>
                      </tr>

                      <tr>
                        <td className="border border-gray-300 px-3 py-2">
                          Recepción Final Mayor a 2 Años
                        </td>

                        <td className="border border-gray-300 text-center">
                          <RadioButton
                            className="form-radio-presto"
                            inputId="recepcion_si"
                            checked={form?.recepcion_final_mayor_2_anios === true}
                            disabled={isDisabled}
                            onChange={() =>
                              updateField('recepcion_final_mayor_2_anios', true)
                            }
                          />
                        </td>

                        <td className="border border-gray-300 text-center">
                          <RadioButton
                            className="form-radio-presto"
                            inputId="recepcion_no"
                            checked={form?.recepcion_final_mayor_2_anios === false}
                            disabled={isDisabled}
                            onChange={() =>
                              updateField('recepcion_final_mayor_2_anios', false)
                            }
                          />
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                {/* <div className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-4">
          <div className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 p-3">
            <Checkbox
                  className="form-checkbox-presto"
              checked={!!form?.vivienda_social}
              disabled={isDisabled}
              onChange={(e) => updateField('vivienda_social', !!e.checked)}
            />
            <label className="text-sm font-medium text-gray-700">Propiedad adquirida es Vivienda Social *</label>
          </div>

          <div className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 p-3">
            <Checkbox
              checked={!!form?.dfl2}
              disabled={isDisabled}
              onChange={(e) => updateField('dfl2', !!e.checked)}
            />
            <label className="text-sm font-medium text-gray-700">Propiedad adquirida es DFL 2 *</label>
          </div>

          <div className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 p-3">
            <Checkbox
              checked={!!form?.propietario_dfl2}
              disabled={isDisabled}
              onChange={(e) => updateField('propietario_dfl2', !!e.checked)}
            />
            <label className="text-sm font-medium text-gray-700">Comprador propietario 0 ó 1 vivienda DFL 2 *</label>
          </div>

          <div className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 p-3">
            <Checkbox
              checked={!!form?.recepcion_final_mayor_2_anios}
              disabled={isDisabled}
              onChange={(e) => updateField('recepcion_final_mayor_2_anios', !!e.checked)}
            />
            <label className="text-sm font-medium text-gray-700">Recepción Final Mayor a 2 Años *</label>
          </div>
        </div> */}

                <div className="mt-5 grid grid-cols-1 gap-5 md:grid-cols-2 xl:grid-cols-3">
                  <div className={fieldClass}>
                    <label className={labelClass}>% Impuesto *</label>
                    <InputNumber
                      value={form?.porcentaje_impuesto ?? null}
                      disabled={isDisabled}
                      className={inputClass}
                      inputClassName="w-full"
                      mode="decimal"
                      minFractionDigits={0}
                      maxFractionDigits={6}
                      onValueChange={(e) => updateField('porcentaje_impuesto', e.value ?? null)}
                    />
                  </div>

                  <div className={fieldClass}>
                    <label className={labelClass}>Monto Crédito Afecto Impuesto *</label>
                    <InputNumber
                      value={form?.monto_credito_afecto_impuesto ?? null}
                      disabled={isDisabled}
                      className={inputClass}
                      inputClassName="w-full"
                      mode="decimal"
                      minFractionDigits={0}
                      maxFractionDigits={6}
                      onValueChange={(e) => updateField('monto_credito_afecto_impuesto', e.value ?? null)}
                    />
                  </div>

                  <div className={fieldClass}>
                    <label className={labelClass}>Impuesto a Pagar *</label>
                    <InputNumber
                      value={form?.impuesto_a_pagar ?? null}
                      disabled={isDisabled}
                      className={inputClass}
                      inputClassName="w-full"
                      mode="decimal"
                      minFractionDigits={0}
                      maxFractionDigits={6}
                      onValueChange={(e) => updateField('impuesto_a_pagar', e.value ?? null)}
                    />
                  </div>

                  <div className={fieldClass}>
                    <label className={labelClass}>Enviar a Reparo *</label>
                    <Dropdown
                      value={boolToCatalogCode(form?.enviar_a_reparo)}
                      options={controles.si_no}
                      optionLabel="description"
                      optionValue="code"
                      disabled={isDisabled}
                      className={dropdownClass}
                      placeholder="Seleccione"
                      onChange={(e) => updateField('enviar_a_reparo', catalogCodeToBool(e.value))}
                      showClear
                    />
                  </div>

                  <div className="flex flex-col gap-1 md:col-span-2">
                    <label className={labelClass}>Observaciones *</label>
                    <InputTextarea
                      value={form?.observaciones ?? ''}
                      disabled={isDisabled}
                      className="form-input-presto w-full"
                      rows={4}
                      autoResize
                      placeholder="Ingrese observaciones"
                      onChange={(e) => updateField('observaciones', e.target.value)}
                    />
                  </div>
                </div>
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

              {/* <Button
                type="button"
                label={avanzarMutation.isPending ? "Avanzando..." : "Avanzar"}
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handleAvanzar}
                disabled={isBusy || !canAdvance}
                className="btn-responsive"
              /> */}

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
