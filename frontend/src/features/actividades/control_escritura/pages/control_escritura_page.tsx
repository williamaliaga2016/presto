import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox } from "primereact/checkbox";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import { useAuth } from "@/app/providers/AuthProvider";

import type { ControlEscritura } from "../models/control_escritura";
import { useControlEscritura } from "../hooks/useControlEscritura";
import { useUpsertControlEscritura } from "../hooks/useUpsertControlEscritura";
import { useAvanzarControlEscritura } from "../hooks/useAvanzarControlEscritura";

const ACTIVITY_ID = "";

const build_initial_state = (
  id_expediente: number,
  id_usuario: number,
): ControlEscritura => ({
  id_control_escritura: 0,
  id_expediente,
  id_usuario_solicitante: id_usuario,
  is_enviar_reparo: false,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalize = (
  source: Partial<ControlEscritura> | null | undefined,
  id_expediente_fallback: number,
  id_usuario_fallback: number,
): ControlEscritura => ({
  id_control_escritura: Number(source?.id_control_escritura ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  id_usuario_solicitante: Number(source?.id_usuario_solicitante ?? id_usuario_fallback),
  is_enviar_reparo: Boolean(source?.is_enviar_reparo ?? false),
  observaciones: source?.observaciones ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? new Date().toISOString(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

export default function ControlEscrituraPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { user } = useAuth();
  const { id_expediente: id_param } = useParams();
  const id_expediente = Number(id_param ?? 0);
  const id_usuario = Number(user?.user_id ?? 0);
  const has_valid_expediente = id_expediente > 0;

  const [form, set_form] = useState<ControlEscritura>(
    build_initial_state(id_expediente, id_usuario),
  );
  const [is_disabled, set_is_disabled] = useState(true);
  const [can_advance, set_can_advance] = useState(false);
  const [is_busy, set_is_busy] = useState(false);
  const [error_msg, set_error_msg] = useState("");

  const has_hydrated = useRef(false);
  const current_expediente = useRef(id_expediente);

  const { data, isLoading } = useControlEscritura(id_expediente);
  const save_mutation = useUpsertControlEscritura();
  const avanzar_mutation = useAvanzarControlEscritura();

  useEffect(() => {
    if (current_expediente.current !== id_expediente) {
      current_expediente.current = id_expediente;
      has_hydrated.current = false;
      set_form(build_initial_state(id_expediente, id_usuario));
      set_error_msg("");
      set_is_disabled(true);
      set_can_advance(false);
    }
  }, [id_expediente, id_usuario]);

  useEffect(() => {
    if (has_hydrated.current) return;

    if (data?.status && data.detail) {
      set_form(normalize(data.detail, id_expediente, id_usuario));
      set_is_disabled(true);
      set_can_advance(true);
      has_hydrated.current = true;
      return;
    }

    if (!has_valid_expediente) {
      set_form(build_initial_state(0, id_usuario));
      set_is_disabled(false);
      set_can_advance(false);
      has_hydrated.current = true;
      return;
    }

    if (data) {
      set_form(build_initial_state(id_expediente, id_usuario));
      set_is_disabled(false);
      set_can_advance(false);
      has_hydrated.current = true;
    }
  }, [data, id_expediente, id_usuario, has_valid_expediente]);

  const handle_editar = () => {
    set_error_msg("");
    set_is_disabled(false);
    set_can_advance(false);
  };

  const validate_form = (): string => {
    if (!form.observaciones || !form.observaciones.trim()) {
      return "Las Observaciones son obligatorias.";
    }
    return "";
  };

  const handle_guardar = async () => {
    set_error_msg("");

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
      const payload = normalize(
        { ...form, id_expediente, id_usuario_solicitante: id_usuario },
        id_expediente,
        id_usuario,
      );
      const response = await save_mutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Control de Escritura guardado correctamente",
          life: 3000,
        });
        set_form(normalize(response.detail ?? payload, payload.id_expediente, id_usuario));
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
    } catch {
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

    if (!has_valid_expediente) {
      const msg = "No existe un id_expediente válido para avanzar.";
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
    } catch {
      const msg = "Ocurrió un error al avanzar.";
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

      <h2 className="text-2xl font-bold text-gray-900 mb-6">Control de Escritura</h2>

      <Accordion multiple activeIndex={[2]}>
        <AccordionTab header="Información del Expediente" disabled={!has_valid_expediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!has_valid_expediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Control de Escritura">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && has_valid_expediente && (
              <div className="mb-4 text-sm text-blue-600">Cargando información...</div>
            )}

            {error_msg && (
              <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                {error_msg}
              </div>
            )}

            <div className="flex flex-col gap-6">
              <div className="flex items-center gap-3">
                <Checkbox
                  inputId="is_enviar_reparo"
                  checked={form.is_enviar_reparo}
                  onChange={(e) =>
                    set_form((prev) => ({ ...prev, is_enviar_reparo: !!e.checked }))
                  }
                  disabled={is_disabled}
                />
                <label htmlFor="is_enviar_reparo" className="font-semibold text-sm cursor-pointer">
                  ¿Enviar a Reparo?
                </label>
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Observaciones *</label>
                <InputTextarea
                  value={form.observaciones ?? ""}
                  onChange={(e) =>
                    set_form((prev) => ({ ...prev, observaciones: e.target.value }))
                  }
                  rows={5}
                  autoResize
                  className="form-textarea-presto w-full"
                  disabled={is_disabled}
                  placeholder="Ingrese observaciones relevantes para el control de escritura"
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
