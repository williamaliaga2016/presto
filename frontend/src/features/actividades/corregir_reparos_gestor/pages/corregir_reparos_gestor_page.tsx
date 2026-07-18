import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox } from "primereact/checkbox";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

import type { CorregirReparosGestor } from "../models/corregir_reparos_gestor";
import { useCorregirReparosGestor } from "../hooks/useCorregirReparosGestor";
import { useUpsertCorregirReparosGestor } from "../hooks/useUpsertCorregirReparosGestor";
import { useAvanzarCorregirReparosGestor } from "../hooks/useAvanzarCorregirReparosGestor";

const ACTIVITY_ID = "";

const build_initial_state = (id_expediente: number): CorregirReparosGestor => ({
  id_corregir_reparos_gestor: 0,
  id_expediente,
  id_usuario_solicitante: 0,
  is_subsanar: false,
  observaciones: null,
  estatus_general: null,
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
  source: Partial<CorregirReparosGestor> | null | undefined,
  id_expediente_fallback: number,
): CorregirReparosGestor => ({
  id_corregir_reparos_gestor: Number(source?.id_corregir_reparos_gestor ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  id_usuario_solicitante: Number(source?.id_usuario_solicitante ?? 0),
  is_subsanar: Boolean(source?.is_subsanar ?? false),
  observaciones: source?.observaciones ?? null,
  estatus_general: source?.estatus_general ?? null,
  solicitante: source?.solicitante ?? "",
  observaciones_reparo: source?.observaciones_reparo ?? "",
  fecha_ingreso: source?.fecha_ingreso ?? null,
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

export default function CorregirReparosGestorPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: id_param } = useParams();
  const id_expediente = Number(id_param ?? 0);
  const has_valid_expediente = id_expediente > 0;

  const [form, set_form] = useState<CorregirReparosGestor>(build_initial_state(id_expediente));
  const [is_disabled, set_is_disabled] = useState(true);
  const [can_advance, set_can_advance] = useState(false);
  const [is_busy, set_is_busy] = useState(false);
  const [error_msg, set_error_msg] = useState("");
  const [success_msg, set_success_msg] = useState("");

  const has_hydrated = useRef(false);
  const current_expediente = useRef(id_expediente);

  const { data, isLoading } = useCorregirReparosGestor(id_expediente);
  const save_mutation = useUpsertCorregirReparosGestor();
  const avanzar_mutation = useAvanzarCorregirReparosGestor();

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
      set_is_disabled(Number(data.detail.id_corregir_reparos_gestor) > 0);
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
    set_form((prev) => ({ ...prev, is_subsanar: checked }));
  };

  const handle_editar = () => {
    set_error_msg("");
    set_success_msg("");
    set_is_disabled(false);
    set_can_advance(false);
  };

  const validate_form = (): string => {
    if (!form.observaciones || !form.observaciones.trim()) {
      return "Debe ingresar las Observaciones.";
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
        summary: "Validacion",
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
          summary: "Exito",
          detail: "Informacion guardada correctamente",
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
          summary: "Atencion",
          detail: response.message || "No se pudo guardar",
          life: 3000,
        });
      }
    } catch (error) {
      console.error("ERROR GUARDAR CORREGIR REPAROS GESTOR", error);
      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: "Ocurrio un error al guardar",
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
      const msg = "No existe un id_expediente valido para avanzar.";
      set_error_msg(msg);
      toast.current?.show({ severity: "warn", summary: "Validacion", detail: msg, life: 3000 });
      return;
    }

    if (!form.is_subsanar) {
      const msg = "Debe marcar Subsanar para avanzar la actividad.";
      set_error_msg(msg);
      toast.current?.show({ severity: "warn", summary: "Validacion", detail: msg, life: 3000 });
      return;
    }

    if (!form.estatus_general || !form.estatus_general.trim()) {
      const msg = "El campo Estatus General es obligatorio para avanzar.";
      set_error_msg(msg);
      toast.current?.show({ severity: "warn", summary: "Validacion", detail: msg, life: 3000 });
      return;
    }

    set_is_busy(true);
    try {
      const response = await avanzar_mutation.mutateAsync(id_expediente);
      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Exito",
          detail: "Actividad avanzada correctamente",
          life: 2000,
        });
        navigate("/home/bandeja");
      } else {
        const msg = response.message || "No se pudo avanzar la actividad.";
        set_error_msg(msg);
        toast.current?.show({ severity: "warn", summary: "Atencion", detail: msg, life: 3000 });
      }
    } catch (error) {
      console.error("ERROR AVANZAR CORREGIR REPAROS GESTOR", error);
      const msg = "Ocurrio un error al avanzar.";
      set_error_msg(msg);
      toast.current?.show({ severity: "error", summary: "Error", detail: msg, life: 3000 });
    } finally {
      set_is_busy(false);
    }
  };

  const handle_salir = () => navigate("/home/bandeja");

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">Corregir Reparos por Gestor</h2>

      <Accordion multiple activeIndex={[2]}>
        <AccordionTab header="Informacion del Expediente" disabled={!has_valid_expediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!has_valid_expediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Corregir Reparos por Gestor">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && has_valid_expediente && (
              <div className="mb-4 text-sm text-blue-600">Cargando informacion...</div>
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

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Estatus General *</label>
                <InputText
                  value={form.estatus_general ?? ""}
                  onChange={(e) =>
                    set_form((prev) => ({ ...prev, estatus_general: e.target.value }))
                  }
                  className="form-input-presto w-full"
                  disabled={is_disabled}
                  placeholder="Ingrese el estatus general"
                />
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Observaciones *</label>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) =>
                    set_form((prev) => ({ ...prev, observaciones: e.target.value }))
                  }
                  rows={4}
                  autoResize
                  className="form-textarea-presto w-full"
                  disabled={is_disabled}
                  placeholder="Ingrese observaciones relevantes para la operacion"
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
                disabled={is_busy || !can_advance || !form.estatus_general?.trim()}
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
  form: CorregirReparosGestor;
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
              <td className="px-3 py-2 text-sm border border-gray-200">{form.observaciones_reparo || "-"}</td>
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
