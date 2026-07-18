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

import type { CorregirReparoCdr } from "../models/corregir_reparo_cdr";
import type { ReparoCdrDetalle } from "../models/reparo_cdr_detalle";
import { useCorregirReparoCdr } from "../hooks/useCorregirReparoCdr";
import { useUpsertCorregirReparoCdr } from "../hooks/useUpsertCorregirReparoCdr";
import { useAvanzarCorregirReparoCdr } from "../hooks/useAvanzarCorregirReparoCdr";

const ACTIVITY_ID = "_Jr6P_L3mZebGt2_P8xH5q";

const build_initial_state = (id_expediente: number): CorregirReparoCdr => ({
  id_corregir_reparo_cdr: 0,
  id_expediente,
  observaciones: null,
  reparo: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalize = (
  source: Partial<CorregirReparoCdr> | null | undefined,
  id_expediente_fallback: number,
): CorregirReparoCdr => ({
  id_corregir_reparo_cdr: Number(source?.id_corregir_reparo_cdr ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  observaciones: source?.observaciones ?? null,
  reparo: source?.reparo ?? null,
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

export default function CorregirReparoCdrPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: id_param } = useParams();
  const id_expediente = Number(id_param ?? 0);
  const has_valid_expediente = id_expediente > 0;

  const [form, set_form] = useState<CorregirReparoCdr>(
    build_initial_state(id_expediente),
  );
  const [is_disabled, set_is_disabled] = useState(true);
  const [can_advance, set_can_advance] = useState(false);
  const [is_busy, set_is_busy] = useState(false);
  const [error_msg, set_error_msg] = useState("");
  const [success_msg, set_success_msg] = useState("");

  const has_hydrated = useRef(false);
  const current_expediente = useRef(id_expediente);

  const { data, isLoading } = useCorregirReparoCdr(id_expediente);
  const save_mutation = useUpsertCorregirReparoCdr();
  const avanzar_mutation = useAvanzarCorregirReparoCdr();

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
      const is_existing_record = loaded.id_corregir_reparo_cdr > 0;
      set_form(loaded);
      set_is_disabled(is_existing_record);
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
      reparo: prev.reparo ? { ...prev.reparo, subsanar: checked } : prev.reparo,
    }));
  };

  const handle_editar = () => {
    set_error_msg("");
    set_success_msg("");
    set_is_disabled(false);
    set_can_advance(false);
  };

  const validate_form = (): string => {
    if (!form.observaciones || !form.observaciones.trim()) {
      return "Debe ingresar las Observaciones generales.";
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
      console.error("ERROR GUARDAR CORREGIR REPARO CDR", error);
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

    if (form.reparo && !form.reparo.subsanar) {
      const msg = "Debe marcar el reparo como subsanado.";
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
      console.error("ERROR AVANZAR CORREGIR REPARO CDR", error);
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

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Corregir Reparo CdR
      </h2>

      <Accordion multiple activeIndex={[2]}>
        <AccordionTab header="Información del Expediente" disabled={!has_valid_expediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!has_valid_expediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Corregir Reparo CdR">
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
                reparo={form.reparo}
                is_disabled={is_disabled}
                on_toggle_subsanar={toggle_subsanar}
              />

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
                  placeholder="Ingrese observaciones relevantes para la operación"
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
  reparo: ReparoCdrDetalle | null;
  is_disabled: boolean;
  on_toggle_subsanar: (checked: boolean) => void;
}

function ReparoTabla({ reparo, is_disabled, on_toggle_subsanar }: ReparoTablaProps) {
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
                Observaciones
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
            {!reparo ? (
              <tr>
                <td
                  colSpan={4}
                  className="px-3 py-4 text-sm text-gray-400 text-center border border-gray-200"
                >
                  Sin reparo registrado
                </td>
              </tr>
            ) : (
              <tr className="bg-white">
                <td className="px-3 py-2 text-sm border border-gray-200">{reparo.solicitante}</td>
                <td className="px-3 py-2 text-sm border border-gray-200">{reparo.observaciones}</td>
                <td className="px-3 py-2 text-sm border border-gray-200 text-center">
                  {format_date(reparo.fecha_ingreso)}
                </td>
                <td className="px-3 py-2 border border-gray-200 text-center">
                  <Checkbox
                    checked={reparo.subsanar}
                    onChange={(e) => on_toggle_subsanar(!!e.checked)}
                    disabled={is_disabled}
                  />
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
