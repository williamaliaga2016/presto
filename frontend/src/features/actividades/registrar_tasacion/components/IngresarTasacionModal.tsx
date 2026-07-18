import { useEffect, useMemo, useRef, useState } from "react";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Dialog } from "primereact/dialog";
import { InputNumber } from "primereact/inputnumber";
import { InputText } from "primereact/inputtext";
import { RadioButton } from "primereact/radiobutton";
import { Toast } from "primereact/toast";

import type { TipoTasacion } from "../models/registrar_tasacion";
import type { TasacionDetalle } from "../models/tasacion_detalle";

interface IngresarTasacionModalProps {
  visible: boolean;
  idExpediente: number;
  factorConversionUF: number | null;
  initialData?: TasacionDetalle | null;
  readOnly?: boolean;
  onHide: () => void;
  onConfirm: (detalle: TasacionDetalle) => void;
}

const buildEmptyDetalle = (id_expediente: number): TasacionDetalle => ({
  id_tasacion_detalle: 0,
  id_tasacion: 0,
  id_expediente,
  tipo_tasacion: null,
  nro_tasacion_p1: null,
  nro_tasacion_p2: null,
  nro_tasacion_p3: null,
  superficie_edificada: null,
  superficie_terreno: null,
  fecha_informe_tasacion: null,
  fecha_solicitud_tasacion: null,
  fecha_recepcion_tasacion: null,
  valor_tasacion_uf: null,
  valor_tasacion_pesos: null,
  valor_liquidacion_uf: null,
  valor_liquidacion_pesos: null,
  monto_asegurable_uf: null,
  monto_asegurable_pesos: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

const toDate = (value: string | null | undefined): Date | null => {
  if (!value) return null;
  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? null : parsed;
};

const toIsoString = (value: Date | Date[] | null | undefined): string | null => {
  if (!value || Array.isArray(value)) return null;
  return value.toISOString();
};

const round4 = (value: number | null): number | null => {
  if (value === null || value === undefined) return null;
  return Math.round(value * 10000) / 10000;
};

export default function IngresarTasacionModal({
  visible,
  idExpediente,
  factorConversionUF,
  initialData,
  readOnly = false,
  onHide,
  onConfirm,
}: IngresarTasacionModalProps) {
  const toastRef = useRef<Toast>(null);
  const [draft, setDraft] = useState<TasacionDetalle>(
    buildEmptyDetalle(idExpediente),
  );

  const isEditMode = useMemo(
    () => !!initialData && initialData.id_tasacion_detalle > 0,
    [initialData],
  );

  useEffect(() => {
    if (!visible) return;

    if (initialData) {
      setDraft({ ...initialData, id_expediente: idExpediente });
    } else {
      setDraft(buildEmptyDetalle(idExpediente));
    }
  }, [visible, initialData, idExpediente]);

  const showToast = (
    severity: "success" | "info" | "warn" | "error",
    summary: string,
    detail: string,
  ) => {
    toastRef.current?.show({ severity, summary, detail, life: 3000 });
  };

  const updateField = <K extends keyof TasacionDetalle>(
    field: K,
    value: TasacionDetalle[K],
  ) => {
    setDraft((prev) => ({ ...prev, [field]: value }));
  };

  const updatePesosAndCalcUF = (
    pesosField:
      | "valor_tasacion_pesos"
      | "valor_liquidacion_pesos"
      | "monto_asegurable_pesos",
    ufField:
      | "valor_tasacion_uf"
      | "valor_liquidacion_uf"
      | "monto_asegurable_uf",
    pesosValue: number | null,
  ) => {
    setDraft((prev) => ({
      ...prev,
      [pesosField]: pesosValue,
      [ufField]:
        pesosValue !== null && factorConversionUF
          ? round4(pesosValue * factorConversionUF)
          : null,
    }));
  };

  const validate = (): string => {
    if (!draft.tipo_tasacion) return "Debe seleccionar el Tipo de Tasación.";
    if (!draft.nro_tasacion_p1?.trim()) return "Debe ingresar el N° Tasación (parte 1).";
    if (!draft.nro_tasacion_p2?.trim()) return "Debe ingresar el N° Tasación (parte 2).";
    if (!draft.nro_tasacion_p3?.trim()) return "Debe ingresar el N° Tasación (parte 3).";
    if (!draft.superficie_edificada?.trim()) return "Debe ingresar la Superficie Edificada.";
    if (!draft.superficie_terreno?.trim()) return "Debe ingresar la Superficie Terreno.";
    if (!draft.fecha_informe_tasacion) return "Debe ingresar la Fecha de Informe de Tasación.";
    if (!draft.fecha_solicitud_tasacion) return "Debe ingresar la Fecha de Solicitud de Tasación.";
    if (!draft.fecha_recepcion_tasacion) return "Debe ingresar la Fecha de Recepción de Tasación.";
    if (draft.valor_tasacion_pesos === null || draft.valor_tasacion_pesos <= 0)
      return "Debe ingresar el Valor de Tasación (Pesos).";
    if (draft.valor_liquidacion_pesos === null || draft.valor_liquidacion_pesos <= 0)
      return "Debe ingresar el Valor de Liquidación (Pesos).";
    if (draft.monto_asegurable_pesos === null || draft.monto_asegurable_pesos <= 0)
      return "Debe ingresar el Monto Asegurable (Pesos).";
    if (!factorConversionUF || factorConversionUF <= 0)
      return "No se ha definido el Factor de conversión UF en la actividad Carga Operación Banco. Comuníquese con el administrador.";

    return "";
  };

  const handleAgregar = () => {
    const msg = validate();
    if (msg) {
      showToast("warn", "Validación", msg);
      return;
    }

    onConfirm({
      ...draft,
      id_expediente: idExpediente,
    });
  };

  const handleCancelar = () => {
    setDraft(buildEmptyDetalle(idExpediente));
    onHide();
  };

  const footer = readOnly ? (
    <div className="flex justify-end gap-2">
      <Button
        type="button"
        label="Cerrar"
        icon="pi pi-times"
        severity="secondary"
        outlined
        onClick={handleCancelar}
      />
    </div>
  ) : (
    <div className="flex justify-end gap-2">
      <Button
        type="button"
        label="Cancelar"
        icon="pi pi-times"
        severity="secondary"
        outlined
        onClick={handleCancelar}
      />
      <Button
        type="button"
        label={isEditMode ? "Actualizar" : "Agregar"}
        icon={isEditMode ? "pi pi-check" : "pi pi-plus"}
        severity="info"
        onClick={handleAgregar}
      />
    </div>
  );

  const getHeader = () => {
    if (readOnly) return "Detalle de la Tasación";
    return isEditMode ? "Editar Tasación" : "Ingresar Tasación";
  };

  return (
    <>
      <Toast ref={toastRef} />
      <Dialog
        header={getHeader()}
        visible={visible}
        onHide={handleCancelar}
        modal
        dismissableMask={false}
        closeOnEscape={readOnly}
        draggable={false}
        resizable={false}
        className="w-full md:w-11/12 lg:w-10/12"
        footer={footer}
      >
        <div className="bg-blue-700 text-white font-semibold px-3 py-2 text-sm mb-4">
          INGRESAR TASACIÓN
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-x-6 gap-y-4">
          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="font-semibold text-sm">Tipo de Tasación *</label>
            <div className="flex items-center gap-6 h-10">
<div className="flex items-center gap-2">
  <RadioButton
    className="form-radio-presto"
    inputId="tipo_colectiva"
    value={true}
    checked={draft.tipo_tasacion === true}
    onChange={() => updateField("tipo_tasacion", true)}
    disabled={readOnly}
  />
  <label htmlFor="tipo_colectiva" className="text-sm">
    Colectiva
  </label>
</div>

<div className="flex items-center gap-2">
  <RadioButton
    className="form-radio-presto"
    inputId="tipo_individual"
    value={false}
    checked={draft.tipo_tasacion === false}
    onChange={() => updateField("tipo_tasacion", false)}
    disabled={readOnly}
  />
  <label htmlFor="tipo_individual" className="text-sm">
    Individual
  </label>
</div>
            </div>
          </div>

          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="font-semibold text-sm">N° Tasación *</label>
            <div className="flex items-center gap-2">
              <InputText
                value={draft.nro_tasacion_p1 ?? ""}
                onChange={(e) => updateField("nro_tasacion_p1", e.target.value)}
                className="form-input-presto w-20"
                maxLength={20}
                disabled={readOnly}
              />
              <span className="font-semibold">-</span>
              <InputText
                value={draft.nro_tasacion_p2 ?? ""}
                onChange={(e) => updateField("nro_tasacion_p2", e.target.value)}
                className="form-input-presto w-20"
                maxLength={20}
                disabled={readOnly}
              />
              <span className="font-semibold">-</span>
              <InputText
                value={draft.nro_tasacion_p3 ?? ""}
                onChange={(e) => updateField("nro_tasacion_p3", e.target.value)}
                className="form-input-presto flex-1"
                maxLength={30}
                disabled={readOnly}
              />
            </div>
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Superficie Edificada *</label>
            <InputText
              value={draft.superficie_edificada ?? ""}
              onChange={(e) =>
                updateField("superficie_edificada", e.target.value)
              }
              className="form-input-presto w-full"
              maxLength={200}
              placeholder="Ej: 120 m2"
              disabled={readOnly}
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Superficie Terreno *</label>
            <InputText
              value={draft.superficie_terreno ?? ""}
              onChange={(e) =>
                updateField("superficie_terreno", e.target.value)
              }
              className="form-input-presto w-full"
              maxLength={200}
              placeholder="Ej: 200 m2"
              disabled={readOnly}
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Fecha Informe Tasación *
            </label>
            <Calendar
              value={toDate(draft.fecha_informe_tasacion)}
              onChange={(e) =>
                updateField("fecha_informe_tasacion", toIsoString(e.value))
              }
              dateFormat="dd/mm/yy"
              showIcon
              inputClassName="form-input-presto w-full"
              className="w-full"
              placeholder="dd/mm/aaaa"
              disabled={readOnly}
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Fecha Solicitud Tasación *
            </label>
            <Calendar
              value={toDate(draft.fecha_solicitud_tasacion)}
              onChange={(e) =>
                updateField("fecha_solicitud_tasacion", toIsoString(e.value))
              }
              dateFormat="dd/mm/yy"
              showIcon
              inputClassName="form-input-presto w-full"
              className="w-full"
              placeholder="dd/mm/aaaa"
              disabled={readOnly}
            />
          </div>

          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="font-semibold text-sm">
              Fecha Recepción Tasación *
            </label>
            <Calendar
              value={toDate(draft.fecha_recepcion_tasacion)}
              onChange={(e) =>
                updateField("fecha_recepcion_tasacion", toIsoString(e.value))
              }
              dateFormat="dd/mm/yy"
              showIcon
              inputClassName="form-input-presto w-full"
              className="w-full md:max-w-md"
              placeholder="dd/mm/aaaa"
              disabled={readOnly}
            />
          </div>
        </div>

        <div className="bg-blue-700 text-white font-semibold px-3 py-2 text-sm mt-6 mb-4">
          MONTOS (UF / PESOS)
        </div>

        {!factorConversionUF && (
          <div className="mb-3 rounded-md border border-yellow-300 bg-yellow-50 px-3 py-2 text-xs text-yellow-800">
            No se ha definido el <b>Factor de conversión UF</b> en la actividad
            3.1. Carga Operación Banco. Los valores UF no se podrán calcular
            automáticamente.
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-[1fr_1fr] gap-x-6 gap-y-4">
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Valor Tasación (Pesos) *
            </label>
            <InputNumber
              value={draft.valor_tasacion_pesos}
              onValueChange={(e) =>
                updatePesosAndCalcUF(
                  "valor_tasacion_pesos",
                  "valor_tasacion_uf",
                  e.value ?? null,
                )
              }
              className="form-input-presto w-full"
              locale="es-CL"
              mode="currency"
              currency="CLP"
              min={0}
              placeholder="$ 0"
              disabled={readOnly}
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Valor Tasación (UF)
            </label>
            <InputNumber
              value={draft.valor_tasacion_uf}
              className="form-input-presto w-full"
              locale="es-CL"
              minFractionDigits={2}
              maxFractionDigits={4}
              disabled
              placeholder="0,00"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Valor Liquidación (Pesos) *
            </label>
            <InputNumber
              value={draft.valor_liquidacion_pesos}
              onValueChange={(e) =>
                updatePesosAndCalcUF(
                  "valor_liquidacion_pesos",
                  "valor_liquidacion_uf",
                  e.value ?? null,
                )
              }
              className="form-input-presto w-full"
              locale="es-CL"
              mode="currency"
              currency="CLP"
              min={0}
              placeholder="$ 0"
              disabled={readOnly}
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Valor Liquidación (UF)
            </label>
            <InputNumber
              value={draft.valor_liquidacion_uf}
              className="form-input-presto w-full"
              locale="es-CL"
              minFractionDigits={2}
              maxFractionDigits={4}
              disabled
              placeholder="0,00"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Monto Asegurable (Pesos) *
            </label>
            <InputNumber
              value={draft.monto_asegurable_pesos}
              onValueChange={(e) =>
                updatePesosAndCalcUF(
                  "monto_asegurable_pesos",
                  "monto_asegurable_uf",
                  e.value ?? null,
                )
              }
              className="form-input-presto w-full"
              locale="es-CL"
              mode="currency"
              currency="CLP"
              min={0}
              placeholder="$ 0"
              disabled={readOnly}
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Monto Asegurable (UF)
            </label>
            <InputNumber
              value={draft.monto_asegurable_uf}
              className="form-input-presto w-full"
              locale="es-CL"
              minFractionDigits={2}
              maxFractionDigits={4}
              disabled
              placeholder="0,00"
            />
          </div>
        </div>

        {factorConversionUF !== null && factorConversionUF > 0 && (
          <div className="mt-3 text-xs text-gray-600">
            Factor de conversión UF utilizado:{" "}
            <b>{factorConversionUF.toLocaleString("es-CL", {
              minimumFractionDigits: 2,
              maximumFractionDigits: 6,
            })}</b>{" "}
            (cargado en la actividad Carga Operación Banco).
          </div>
        )}
      </Dialog>
    </>
  );
}
