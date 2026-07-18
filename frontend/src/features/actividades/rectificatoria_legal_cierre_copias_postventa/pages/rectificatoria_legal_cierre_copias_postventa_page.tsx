import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";

import type { RectificatoriaLegalCierreCopiasPostventa } from "../models/rectificatoria_legal_cierre_copias_postventa";
import { useRectificatoriaLegalCierreCopiasPostventa } from "../hooks/useRectificatoriaLegalCierreCopiasPostventa";
import { useUpsertRectificatoriaLegalCierreCopiasPostventa } from "../hooks/useUpsertRectificatoriaLegalCierreCopiasPostventa";
import { useAvanzarRectificatoriaLegalCierreCopiasPostventa } from "../hooks/useAvanzarRectificatoriaLegalCierreCopiasPostventa";

const ACTIVITY_ID = "";

const build_initial_state = (id_expediente: number): RectificatoriaLegalCierreCopiasPostventa => ({
  id_rectificatoria_legal_cierre_copias_postventa: 0,
  id_expediente,
  id_usuario_solicitante: 0,
  fecha_firma: null,
  observaciones: null,
  nombre_banco_alzante: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const normalize = (
  source: Partial<RectificatoriaLegalCierreCopiasPostventa> | null | undefined,
  id_expediente_fallback: number,
): RectificatoriaLegalCierreCopiasPostventa => ({
  id_rectificatoria_legal_cierre_copias_postventa: Number(source?.id_rectificatoria_legal_cierre_copias_postventa ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  id_usuario_solicitante: Number(source?.id_usuario_solicitante ?? 0),
  fecha_firma: source?.fecha_firma ?? null,
  observaciones: source?.observaciones ?? null,
  nombre_banco_alzante: source?.nombre_banco_alzante ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? new Date().toISOString(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

export default function RectificatoriaLegalCierreCopiasPostventaPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: id_param } = useParams();
  const id_expediente = Number(id_param ?? 0);
  const has_valid_expediente = id_expediente > 0;

  const [form, set_form] = useState<RectificatoriaLegalCierreCopiasPostventa>(build_initial_state(id_expediente));
  const [is_disabled, set_is_disabled] = useState(true);
  const [can_advance, set_can_advance] = useState(false);
  const [is_busy, set_is_busy] = useState(false);
  const [error_msg, set_error_msg] = useState("");

  const has_hydrated = useRef(false);
  const current_expediente = useRef(id_expediente);

  const { data, isLoading } = useRectificatoriaLegalCierreCopiasPostventa(id_expediente);
  const save_mutation = useUpsertRectificatoriaLegalCierreCopiasPostventa();
  const avanzar_mutation = useAvanzarRectificatoriaLegalCierreCopiasPostventa();

  useEffect(() => {
    if (current_expediente.current !== id_expediente) {
      current_expediente.current = id_expediente;
      has_hydrated.current = false;
      set_form(build_initial_state(id_expediente));
      set_error_msg("");
      set_is_disabled(true);
      set_can_advance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (has_hydrated.current) return;

    if (!has_valid_expediente) {
      set_form(build_initial_state(0));
      set_is_disabled(false);
      set_can_advance(false);
      has_hydrated.current = true;
      return;
    }

    if (data?.status && data.detail) {
      const loaded = normalize(data.detail, id_expediente);
      const is_new = loaded.id_rectificatoria_legal_cierre_copias_postventa === 0;
      set_form(loaded);
      set_is_disabled(!is_new);
      set_can_advance(!is_new && !!loaded.fecha_firma);
      has_hydrated.current = true;
    }
  }, [data, id_expediente, has_valid_expediente]);

  const handle_editar = () => {
    set_error_msg("");
    set_is_disabled(false);
    set_can_advance(false);
  };

  const validate_form = (): string => {
    if (!form.fecha_firma) {
      return "La fecha de firma es obligatoria.";
    }
    if (!form.observaciones || !form.observaciones.trim()) {
      return "Las observaciones son obligatorias.";
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
      const payload = normalize({ ...form, id_expediente }, id_expediente);
      const response = await save_mutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Información guardada correctamente",
          life: 3000,
        });
        const saved = normalize(
          { ...(response.detail ?? payload), nombre_banco_alzante: payload.nombre_banco_alzante },
          payload.id_expediente,
        );
        set_form(saved);
        set_is_disabled(true);
        set_can_advance(!!saved.fecha_firma);
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
      console.error("ERROR GUARDAR RECTIFICATORIA LEGAL CIERRE DE COPIAS POSTVENTA", error);
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
    } catch (error) {
      console.error("ERROR AVANZAR RECTIFICATORIA LEGAL CIERRE DE COPIAS POSTVENTA", error);
      const msg = "Ocurrió un error al avanzar.";
      set_error_msg(msg);
      toast.current?.show({ severity: "error", summary: "Error", detail: msg, life: 3000 });
    } finally {
      set_is_busy(false);
    }
  };

  const handle_salir = () => navigate("/home/bandeja");

  const fecha_as_date = form.fecha_firma ? new Date(form.fecha_firma) : null;

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Rectificatoria Legal - Cierre de Copias Post Venta
      </h2>

      <Accordion multiple activeIndex={[2]}>
        <AccordionTab header="Información del Expediente" disabled={!has_valid_expediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!has_valid_expediente}>
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Rectificatoria Legal - Cierre de Copias Post Venta">
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
              <div className="overflow-x-auto">
                <table className="w-full border-collapse border border-gray-300 text-sm">
                  <thead>
                    <tr className="bg-[#03298e] text-white">
                      <th className="border border-gray-300 px-3 py-2 text-center font-semibold">
                        Nombre Banco Alzante *
                      </th>
                      <th className="border border-gray-300 px-3 py-2 text-center font-semibold">
                        Fecha de Firma *
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td className="border border-gray-300 px-3 py-2 align-middle">
                        <InputText
                          value={form.nombre_banco_alzante ?? ""}
                          readOnly
                          disabled
                          className="form-input-presto w-full"
                        />
                      </td>
                      <td className="border border-gray-300 px-3 py-2 align-middle">
                        <Calendar
                          value={fecha_as_date}
                          onChange={(e) => {
                            const val = e.value as Date | null;
                            set_form((prev) => ({
                              ...prev,
                              fecha_firma: val ? val.toISOString() : null,
                            }));
                          }}
                          dateFormat="dd/mm/yy"
                          showIcon
                          disabled={is_disabled}
                          className="w-full"
                          inputClassName="form-input-presto w-full"
                        />
                      </td>
                    </tr>
                  </tbody>
                </table>
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
                  maxLength={2000}
                  className="form-textarea-presto w-full"
                  disabled={is_disabled}
                  placeholder="Ingrese una observación relevante para la operación"
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
