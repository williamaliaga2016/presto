import { useMemo, useState } from "react";
import { Button } from "primereact/button";
import { Column } from "primereact/column";
import { ConfirmDialog, confirmDialog } from "primereact/confirmdialog";
import { DataTable } from "primereact/datatable";
import { InputTextarea } from "primereact/inputtextarea";

import { useBitacoras } from "../hooks/useBitacoras";
import { useCreateBitacora } from "../hooks/useCreateBitacora";
import type { Bitacora } from "../models/Bitacora";
import { Card } from "primereact/card";

type BitacoraPageProps = {
  id_expediente: number;
  id_actividad?: string;
};

export default function BitacoraPage({
  id_expediente,
  id_actividad,
}: BitacoraPageProps) {
  const [observaciones, setObservaciones] = useState("");

  const expediente_id = Number(id_expediente ?? 0);
  const actividad_id = String(id_actividad ?? "").trim();

  const {
    data: response,
    isLoading,
    isError,
    error,
  } = useBitacoras(expediente_id);

  const entries = response?.status ? response.detail ?? [] : [];
  const createBitacoraMutation = useCreateBitacora();

  const total_caracteres = useMemo(() => observaciones.length, [observaciones]);

  const limpiarFormulario = () => {
    setObservaciones("");
  };

  const handleSubmit = async () => {
    if (!observaciones.trim()) return;
    if (!expediente_id || expediente_id <= 0) return;

    const nueva_entrada: Bitacora = {
      id_expediente: expediente_id,
      id_actividad: actividad_id,
      observaciones: observaciones.trim(),
    };

    try {
      await createBitacoraMutation.mutateAsync(nueva_entrada);
      limpiarFormulario();
    } catch (err) {
      console.error("Error al crear la bitácora:", err);
    }
  };

  const confirmarCancelar = () => {
    if (!observaciones.trim()) {
      limpiarFormulario();
      return;
    }

    confirmDialog({
      message: "¿Deseas limpiar el formulario?",
      header: "Confirmación",
      icon: "pi pi-exclamation-triangle",
      acceptLabel: "Sí",
      rejectLabel: "No",
      accept: limpiarFormulario,
    });
  };

    const formatFecha = (value?: string | null) => {
    if (!value) return "";

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return value;

    return date.toLocaleString("es-PE", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit",
      hour12: false,
    });
  };

  const renderResponsiveCell = (
    value: React.ReactNode,
    titulo: string
  ) => {
    return <div data-titulo={titulo}>{value ?? ""}</div>;
  };

    const expedienteBodyTemplate = (rowData: Bitacora) =>
      renderResponsiveCell(rowData.id_expediente, "Nro Expediente: ");
    
    const usuarioBodyTemplate = (rowData: Bitacora) =>
        renderResponsiveCell(rowData.usuario, "Usuario: ");
    
    const observacionesBodyTemplate = (rowData: Bitacora) =>
        renderResponsiveCell(rowData.observaciones, "Observaciones: ");
    
  const fechaAltaBodyTemplate = (rowData: Bitacora) =>
    renderResponsiveCell(
      formatFecha(rowData.fecha_alta),
      "Fec Alta: "
    );

  
  const actionBodyTemplate = (row: Bitacora) => {
    return (
      <div className="flex gap-2">
        <Button
          type="button"
          icon="pi pi-eye"
          rounded
          outlined
          severity="info"
          tooltip="Ver detalle"
          onClick={() => {}}
        />
      </div>
    );
  };

  if (!expediente_id || expediente_id <= 0) {
    return (
      <>
        <div className="mt-2 rounded-md border border-yellow-200 bg-yellow-50 px-4 py-3 text-sm text-yellow-800">
          La bitácora se habilitará cuando exista un expediente válido.
        </div>
        <ConfirmDialog />
      </>
    );
  }

return (
  <>
    <Card className="w-full shadow-md card-presto-form mb-6">
      <form
        onSubmit={(e) => {
          e.preventDefault();
          handleSubmit();
        }}
        className="flex flex-col gap-4"
      >
        <div className="form-grid">
          <div className="flex flex-col form-col-full">
            <label className="text-sm font-medium text-gray-700">
              Observaciones
            </label>

            <InputTextarea
              value={observaciones}
              onChange={(e) => setObservaciones(e.target.value)}
              rows={4}
              maxLength={500}
              autoResize
              className="form-textarea-presto w-full"
            />

            <small className="text-gray-500">
              {total_caracteres}/500 caracteres
            </small>
          </div>
        </div>

        <div className="form-actions">
          <Button
            label={
              createBitacoraMutation.isPending ? "Guardando..." : "Guardar"
            }
            icon="pi pi-save"
            severity="success"
            type="submit"
            disabled={
              createBitacoraMutation.isPending ||
              !observaciones.trim() ||
              !expediente_id
            }
            className="btn-responsive flex items-center gap-2"
          />

          <Button
            type="button"
            label="Cancelar"
            icon="pi pi-times"
            severity="secondary"
            outlined
            onClick={confirmarCancelar}
            className="btn-responsive flex items-center gap-2"
          />
        </div>
      </form>

      <div className="w-full mt-6 overflow-x-auto">
        <DataTable
          value={Array.isArray(entries) ? entries : []}
          paginator
          rows={5}
          rowsPerPageOptions={[5, 10, 20]}
          className="text-sm"
          loading={isLoading}
          emptyMessage="No hay registros de bitácora"
          size="small"
        >
          <Column field="id_expediente" header="ID Expediente" 
            body={expedienteBodyTemplate}/>
          <Column field="usuario" header="Usuario" body={usuarioBodyTemplate}/>
          <Column field="observaciones" header="Observaciones" body={observacionesBodyTemplate}/>
          <Column
            field="fecha_alta"
            header="Fecha Alta"
            body={fechaAltaBodyTemplate}
          />
          <Column body={actionBodyTemplate} header="Acciones" />
        </DataTable>
      </div>

      {isError && (
        <div className="mt-4 text-sm text-red-600">
          Ocurrió un error al cargar la bitácora.
          {error instanceof Error ? ` ${error.message}` : ""}
        </div>
      )}

      {!isLoading && response && !response.status && (
        <div className="mt-4 text-sm text-red-600">
          {response.message || "No se pudo obtener la bitácora"}
        </div>
      )}
    </Card>

    <ConfirmDialog />
  </>
);
}