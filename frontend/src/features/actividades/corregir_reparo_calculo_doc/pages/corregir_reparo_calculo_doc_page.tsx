import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox } from "primereact/checkbox";
import { InputNumber } from "primereact/inputnumber";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { RadioButton } from "primereact/radiobutton";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

import type { CorregirReparoCalculoDoc } from "../models/corregir_reparo_calculo_doc";
import type { CatalogoOption, ControlesPropiedad } from "../models/catalogo";
import { EMPTY_CONTROLES_PROPIEDAD } from "../models/catalogo";
import { useCorregirReparoCalculoDoc } from "../hooks/useCorregirReparoCalculoDoc";
import { useUpsertCorregirReparoCalculoDoc } from "../hooks/useUpsertCorregirReparoCalculoDoc";
import { useAvanzarCorregirReparoCalculoDoc } from "../hooks/useAvanzarCorregirReparoCalculoDoc";
import { useControlesPropiedad } from "../hooks/useControlesPropiedad";

const ACTIVITY_ID = "_J6wLpT4zEelNy2_Y1mD7x";

const build_initial_state = (id_expediente: number): CorregirReparoCalculoDoc => ({
  id_corregir_reparo_calculo_doc: 0,
  id_expediente,
  id_usuario_solicitante: 0,
  is_subsanar: false,
  observaciones: null,
  existe_rol_avaluo: null,
  rol_avaluo_editado: null,
  valor_avaluo_pesos: null,
  solicitante: "",
  observaciones_reparo: "",
  fecha_ingreso: null,
  tipo_propiedad: "",
  tipo_direccion: "",
  direccion: "",
  region: "",
  comuna: "",
  rol_avaluo: "",
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalize = (
  source: Partial<CorregirReparoCalculoDoc> | null | undefined,
  id_expediente_fallback: number,
): CorregirReparoCalculoDoc => ({
  id_corregir_reparo_calculo_doc: Number(source?.id_corregir_reparo_calculo_doc ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  id_usuario_solicitante: Number(source?.id_usuario_solicitante ?? 0),
  is_subsanar: Boolean(source?.is_subsanar ?? false),
  observaciones: source?.observaciones ?? null,
  existe_rol_avaluo: source?.existe_rol_avaluo ?? null,
  rol_avaluo_editado: source?.rol_avaluo_editado ?? null,
  valor_avaluo_pesos:
    source?.valor_avaluo_pesos === undefined ? null : source?.valor_avaluo_pesos,
  solicitante: source?.solicitante ?? "",
  observaciones_reparo: source?.observaciones_reparo ?? "",
  fecha_ingreso: source?.fecha_ingreso ?? null,
  tipo_propiedad: source?.tipo_propiedad ?? "",
  tipo_direccion: source?.tipo_direccion ?? "",
  direccion: source?.direccion ?? "",
  region: source?.region ?? "",
  comuna: source?.comuna ?? "",
  rol_avaluo: source?.rol_avaluo ?? "",
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? new Date().toISOString(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

const format_date = (value: string | null | undefined): string => {
  if (!value) return "";
  return new Date(value).toLocaleDateString("es-CL", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  });
};

const is_existe_rol_si = (value: string | null | undefined): boolean =>
  value === "SI" || value === "Sí";

const resolveLabel = (options: CatalogoOption[], value?: string | null): string => {
  if (!value || !value.trim()) return "-";
  const match = options.find((o) => o.code === value);
  if (match && match.description) return match.description;
  return value;
};

export default function CorregirReparoCalculoDocPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: id_param } = useParams();
  const id_expediente = Number(id_param ?? 0);
  const has_valid_expediente = id_expediente > 0;

  const [form, set_form] = useState<CorregirReparoCalculoDoc>(build_initial_state(id_expediente));
  const [is_disabled, set_is_disabled] = useState(true);
  const [can_advance, set_can_advance] = useState(false);
  const [is_busy, set_is_busy] = useState(false);
  const [error_msg, set_error_msg] = useState("");
  const [success_msg, set_success_msg] = useState("");

  const has_hydrated = useRef(false);
  const current_expediente = useRef(id_expediente);

  const { data, isLoading } = useCorregirReparoCalculoDoc(id_expediente);
  const save_mutation = useUpsertCorregirReparoCalculoDoc();
  const avanzar_mutation = useAvanzarCorregirReparoCalculoDoc();
  const { data: controles_data } = useControlesPropiedad();
  const controles = controles_data?.detail ?? EMPTY_CONTROLES_PROPIEDAD;

  useEffect(() => {
    if (current_expediente.current !== id_expediente) {
      current_expediente.current = id_expediente;
      has_hydrated.current = false;

      set_form(build_initial_state(id_expediente));
      set_error_msg("");
      set_success_msg("");
      set_is_disabled(true);
      set_can_advance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (has_hydrated.current) return;

    if (data?.status && data.detail) {
      const loaded = normalize(data.detail, id_expediente);
      set_form(loaded);
      set_is_disabled(Number(data.detail.id_corregir_reparo_calculo_doc) > 0);
      set_can_advance(false);
      has_hydrated.current = true;
      return;
    }

    if (!has_valid_expediente) {
      set_form(build_initial_state(0));
      set_is_disabled(false);
      set_can_advance(false);
      has_hydrated.current = true;
      return;
    }

    if (data) {
      set_form(build_initial_state(id_expediente));
      set_is_disabled(false);
      set_can_advance(false);
      has_hydrated.current = true;
    }
  }, [data, id_expediente, has_valid_expediente]);

  const toggle_subsanar = (checked: boolean) => {
    set_form((prev) => ({
      ...prev,
      is_subsanar: checked,
    }));
  };

  const change_existe_rol_avaluo = (next_value: string) => {
    set_form((prev) =>
      is_existe_rol_si(next_value)
        ? { ...prev, existe_rol_avaluo: next_value }
        : {
            ...prev,
            existe_rol_avaluo: next_value,
            rol_avaluo_editado: null,
            valor_avaluo_pesos: null,
          },
    );
  };

  const handle_editar = () => {
    set_error_msg("");
    set_success_msg("");
    set_is_disabled(false);
    set_can_advance(false);
  };

  const validate_form = (): string => {
    if (!form.observaciones || !form.observaciones.trim()) {
      return "Debe ingresar las Observaciones finales.";
    }

    if (!form.existe_rol_avaluo) {
      return "Debe seleccionar si ¿Existe Rol Avalúo? (Sí o E/T).";
    }

    if (is_existe_rol_si(form.existe_rol_avaluo)) {
      if (!form.rol_avaluo_editado || !String(form.rol_avaluo_editado).trim()) {
        return "Rol Avalúo es obligatorio cuando ¿Existe Rol Avalúo? = Sí.";
      }
      if (
        form.valor_avaluo_pesos === null ||
        form.valor_avaluo_pesos === undefined ||
        Number.isNaN(Number(form.valor_avaluo_pesos))
      ) {
        return "Valor Avalúo Pesos es obligatorio cuando ¿Existe Rol Avalúo? = Sí.";
      }
    }

    return "";
  };

  const handle_guardar = async () => {
    set_error_msg("");
    set_success_msg("");

    const validation_msg = validate_form();
    if (validation_msg) {
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: validation_msg,
        life: 3000,
      });
      return;
    }

    set_is_busy(true);
    try {
      const payload = normalize({ ...form, id_expediente }, id_expediente);
      const response = await save_mutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Información guardada correctamente",
          life: 3000,
        });

        const saved = normalize(response.detail ?? payload, payload.id_expediente);
        set_form(saved);
        set_is_disabled(true);
        set_can_advance(true);
        has_hydrated.current = true;
      } else {
        toast.current?.show({
          severity: "warn",
          summary: "Atención",
          detail: response.message || "No se pudo guardar",
          life: 3000,
        });
      }
    } catch (error) {
      console.error("ERROR GUARDAR CORREGIR REPARO CÁLCULO Y GENERACIÓN DOCUMENTO", error);
      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: "Ocurrió un error al guardar",
        life: 3000,
      });
    } finally {
      set_is_busy(false);
    }
  };

  const handle_avanzar = async () => {
    set_error_msg("");
    set_success_msg("");

    if (!has_valid_expediente) {
      const msg = "No existe un id_expediente válido para avanzar.";
      set_error_msg(msg);
      toast.current?.show({ severity: "warn", summary: "Validación", detail: msg, life: 3000 });
      return;
    }

    if (!form.is_subsanar) {
      const msg = "Debe marcar Subsanar para avanzar la actividad.";
      set_error_msg(msg);
      toast.current?.show({ severity: "warn", summary: "Validación", detail: msg, life: 3000 });
      return;
    }

    set_is_busy(true);
    try {
      const response = await avanzar_mutation.mutateAsync(id_expediente);
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
        set_error_msg(msg);
        toast.current?.show({ severity: "warn", summary: "Atención", detail: msg, life: 3000 });
      }
    } catch (error) {
      console.error("ERROR AVANZAR CORREGIR REPARO CÁLCULO Y GENERACIÓN DOCUMENTO", error);
      const msg = "Ocurrió un error al avanzar.";
      set_error_msg(msg);
      toast.current?.show({ severity: "error", summary: "Error", detail: msg, life: 3000 });
    } finally {
      set_is_busy(false);
    }
  };

  const handle_salir = () => navigate("/home/bandeja");

  const existe_rol_yes = is_existe_rol_si(form.existe_rol_avaluo);

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Corregir Reparo Cálculo y Generación Documento
      </h2>

      <Accordion multiple activeIndex={[2]}>
        <AccordionTab header="Información del Expediente" disabled={!has_valid_expediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!has_valid_expediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Corregir Reparo Cálculo y Generación Documento">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && has_valid_expediente && (
              <div className="mb-4 text-sm text-blue-600">Cargando información...</div>
            )}

            {error_msg && (
              <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                {error_msg}
              </div>
            )}

            {success_msg && (
              <div className="mb-4 rounded-md border border-green-200 bg-green-50 px-4 py-3 text-sm text-green-700">
                {success_msg}
              </div>
            )}

            <div className="flex flex-col gap-6">
              <ReparoTabla
                form={form}
                is_disabled={is_disabled}
                on_toggle_subsanar={toggle_subsanar}
              />

              <RolAvaluoTabla form={form} controles={controles} />

              <RolAvaluoEditor
                form={form}
                is_disabled={is_disabled}
                existe_rol_yes={existe_rol_yes}
                on_change_existe_rol={change_existe_rol_avaluo}
                on_change_rol_editado={(value) =>
                  set_form((prev) => ({ ...prev, rol_avaluo_editado: value }))
                }
                on_change_valor={(value) =>
                  set_form((prev) => ({ ...prev, valor_avaluo_pesos: value }))
                }
              />

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Observaciones Finales *</label>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) =>
                    set_form((prev) => ({ ...prev, observaciones: e.target.value }))
                  }
                  rows={4}
                  autoResize
                  className="form-textarea-presto w-full"
                  disabled={is_disabled}
                  placeholder="Ingrese observaciones finales relevantes para la operación"
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
                onClick={handle_editar}
                disabled={is_busy || !is_disabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={save_mutation.isPending ? "Guardando..." : "Guardar"}
                icon="pi pi-save"
                severity="success"
                onClick={handle_guardar}
                disabled={is_busy || is_disabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={avanzar_mutation.isPending ? "Avanzando..." : "Avanzar"}
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handle_avanzar}
                disabled={is_busy || !can_advance}
                className="btn-responsive"
              />

              <Button
                type="button"
                label="Salir"
                icon="pi pi-sign-out"
                severity="secondary"
                outlined
                onClick={handle_salir}
                disabled={is_busy}
                className="btn-responsive"
              />
            </div>
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}

interface ReparoTablaProps {
  form: CorregirReparoCalculoDoc;
  is_disabled: boolean;
  on_toggle_subsanar: (checked: boolean) => void;
}

function ReparoTabla({ form, is_disabled, on_toggle_subsanar }: ReparoTablaProps) {
  return (
    <div>
      <div className="bg-blue-700 text-white px-4 py-2 font-semibold text-sm uppercase">
        Detalles del Reparo
      </div>
      <div className="w-full overflow-x-auto">
        <table className="w-full border-collapse">
          <thead>
            <tr className="bg-gray-50">
              <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left w-1/4">
                Solicitante
              </th>
              <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                Observaciones del Reparo
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
              <td className="px-3 py-2 text-sm border border-gray-200">{form.solicitante || "-"}</td>
              <td className="px-3 py-2 text-sm border border-gray-200">
                {form.observaciones_reparo || "-"}
              </td>
              <td className="px-3 py-2 text-sm border border-gray-200 text-center">
                {format_date(form.fecha_ingreso)}
              </td>
              <td className="px-3 py-2 border border-gray-200 text-center">
                <Checkbox
                  inputId="is_subsanar"
                  checked={form.is_subsanar}
                  onChange={(e) => on_toggle_subsanar(!!e.checked)}
                  disabled={is_disabled}
                />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  );
}

interface RolAvaluoTablaProps {
  form: CorregirReparoCalculoDoc;
  controles: ControlesPropiedad;
}

function RolAvaluoTabla({ form, controles }: RolAvaluoTablaProps) {
  return (
    <div>
      <div className="bg-blue-700 text-white px-4 py-2 font-semibold text-sm uppercase">
        Datos de la Propiedad
      </div>
      <div className="w-full overflow-x-auto">
        <table className="w-full border-collapse">
          <thead>
            <tr className="bg-gray-50">
              <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                Tipo de Propiedad
              </th>
              <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                Tipo de Dirección
              </th>
              <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                Dirección
              </th>
              <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                Región
              </th>
              <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                Comuna
              </th>
              <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                Rol Avalúo
              </th>
            </tr>
          </thead>
          <tbody>
            <tr className="bg-white">
              <td className="px-3 py-2 text-sm border border-gray-200">
                {resolveLabel(controles.tipo_propiedad, form.tipo_propiedad)}
              </td>
              <td className="px-3 py-2 text-sm border border-gray-200">
                {resolveLabel(controles.tipo_direccion, form.tipo_direccion)}
              </td>
              <td className="px-3 py-2 text-sm border border-gray-200">{form.direccion || "-"}</td>
              <td className="px-3 py-2 text-sm border border-gray-200">{resolveLabel(controles.region, form.region)}</td>
              <td className="px-3 py-2 text-sm border border-gray-200">{resolveLabel(controles.comuna, form.comuna)}</td>
              <td className="px-3 py-2 text-sm border border-gray-200">{form.rol_avaluo || "-"}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  );
}

interface RolAvaluoEditorProps {
  form: CorregirReparoCalculoDoc;
  is_disabled: boolean;
  existe_rol_yes: boolean;
  on_change_existe_rol: (value: string) => void;
  on_change_rol_editado: (value: string) => void;
  on_change_valor: (value: number | null) => void;
}

function RolAvaluoEditor({
  form,
  is_disabled,
  existe_rol_yes,
  on_change_existe_rol,
  on_change_rol_editado,
  on_change_valor,
}: RolAvaluoEditorProps) {
  const existe_rol_et =
    form.existe_rol_avaluo === "E/T" || form.existe_rol_avaluo === "ET";

  return (
    <div className="rounded-md border border-gray-200 bg-gray-50/60 p-4">
      <p className="mb-3 text-xs font-semibold uppercase tracking-wide text-gray-500">
        Corrección Rol Avalúo
      </p>

      <div className="flex flex-col gap-4">
        <div className="flex flex-wrap items-center gap-6">
          <span className="text-sm font-semibold text-gray-800">¿Existe Rol Avalúo? *</span>
          <div className="flex flex-wrap gap-5">
            <div className="flex items-center gap-2">
              <RadioButton
                className="form-radio-presto"
                inputId="existe_rol_si"
                value="SI"
                checked={existe_rol_yes}
                onChange={() => on_change_existe_rol("SI")}
                disabled={is_disabled}
              />
              <label htmlFor="existe_rol_si" className="cursor-pointer text-sm">
                Sí
              </label>
            </div>
            <div className="flex items-center gap-2">
              <RadioButton
                className="form-radio-presto"
                inputId="existe_rol_et"
                value="E/T"
                checked={existe_rol_et}
                onChange={() => on_change_existe_rol("E/T")}
                disabled={is_disabled}
              />
              <label htmlFor="existe_rol_et" className="cursor-pointer text-sm">
                E/T
              </label>
            </div>
          </div>
        </div>

        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <div className="flex flex-col gap-1">
            <label htmlFor="rol_avaluo_editado" className="text-sm font-semibold">
              Rol Avalúo {existe_rol_yes ? "*" : ""}
            </label>
            <InputText
              id="rol_avaluo_editado"
              value={form.rol_avaluo_editado ?? ""}
              onChange={(e) => on_change_rol_editado(e.target.value)}
              className="form-input-presto w-full"
              disabled={is_disabled || !existe_rol_yes}
              placeholder={existe_rol_yes ? "Ej: 12.345-001" : "-"}
            />
          </div>

          <div className="flex flex-col gap-1">
            <label htmlFor="valor_avaluo_pesos" className="text-sm font-semibold">
              Valor Avalúo Pesos {existe_rol_yes ? "*" : ""}
            </label>
            <InputNumber
              inputId="valor_avaluo_pesos"
              value={form.valor_avaluo_pesos ?? null}
              onValueChange={(e) => on_change_valor(e.value ?? null)}
              mode="currency"
              currency="CLP"
              locale="es-CL"
              className="form-input-presto w-full"
              disabled={is_disabled || !existe_rol_yes}
              placeholder={existe_rol_yes ? "Ingrese valor" : "-"}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
