import { useMemo } from "react";
import { Button } from "primereact/button";
import { Column } from "primereact/column";
import { ColumnGroup } from "primereact/columngroup";
import { DataTable } from "primereact/datatable";
import { Row } from "primereact/row";
import { confirmDialog, ConfirmDialog } from "primereact/confirmdialog";

import type { TasacionDetalle } from "../models/tasacion_detalle";

interface TasacionesRegistradasTableProps {
  rows: TasacionDetalle[];
  disabled: boolean;
  canEdit: boolean;
  onView: (detalle: TasacionDetalle, index: number) => void;
  onEdit: (detalle: TasacionDetalle, index: number) => void;
  onDelete: (detalle: TasacionDetalle, index: number) => void;
}

const formatNumber = (value: number | null | undefined, fraction = 4): string => {
  if (value === null || value === undefined) return "-";
  return value.toLocaleString("es-CL", {
    minimumFractionDigits: 2,
    maximumFractionDigits: fraction,
  });
};

const formatPesos = (value: number | null | undefined): string => {
  if (value === null || value === undefined) return "-";
  return value.toLocaleString("es-CL", {
    style: "currency",
    currency: "CLP",
    maximumFractionDigits: 0,
  });
};

const formatDate = (value: string | null | undefined): string => {
  if (!value) return "-";
  const date = new Date(value);
  return Number.isNaN(date.getTime())
    ? "-"
    : date.toLocaleDateString("es-CL", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
      });
};

const nroTasacionTemplate = (rowData: TasacionDetalle): string => {
  return [
    rowData.nro_tasacion_p1,
    rowData.nro_tasacion_p2,
    rowData.nro_tasacion_p3,
  ]
    .filter(Boolean)
    .join(" - ");
};

const tipoTasacionTemplate = (rowData: TasacionDetalle): string => {
  if (!rowData.tipo_tasacion) return "-";
  return rowData.tipo_tasacion === "COLECTIVA"
    ? "Asociada (Colectiva)"
    : "Particular (Individual)";
};

export default function TasacionesRegistradasTable({
  rows,
  disabled,
  canEdit,
  onView,
  onEdit,
  onDelete,
}: TasacionesRegistradasTableProps) {
  const canModifyRows = canEdit && !disabled;
  const gridModeKey = canModifyRows ? "edit" : "view";

  const visibleRows = useMemo(
    () => rows.filter((row) => row.row_status !== false),
    [rows],
  );

  const totals = useMemo(() => {
    return visibleRows.reduce(
      (acc, row) => {
        acc.valor_tasacion_uf += row.valor_tasacion_uf ?? 0;
        acc.valor_tasacion_pesos += row.valor_tasacion_pesos ?? 0;
        acc.valor_liquidacion_uf += row.valor_liquidacion_uf ?? 0;
        acc.valor_liquidacion_pesos += row.valor_liquidacion_pesos ?? 0;
        acc.monto_asegurable_uf += row.monto_asegurable_uf ?? 0;
        acc.monto_asegurable_pesos += row.monto_asegurable_pesos ?? 0;
        return acc;
      },
      {
        valor_tasacion_uf: 0,
        valor_tasacion_pesos: 0,
        valor_liquidacion_uf: 0,
        valor_liquidacion_pesos: 0,
        monto_asegurable_uf: 0,
        monto_asegurable_pesos: 0,
      },
    );
  }, [visibleRows]);

  const confirmDelete = (rowData: TasacionDetalle, index: number) => {
    if (!canModifyRows) return;

    const label = nroTasacionTemplate(rowData) || "esta tasación";

    confirmDialog({
      header: "Confirmar eliminación",
      message: `¿Está seguro de eliminar ${label}?`,
      icon: "pi pi-exclamation-triangle",
      acceptLabel: "Sí, eliminar",
      rejectLabel: "Cancelar",
      acceptClassName: "p-button-danger",
      accept: () => onDelete(rowData, index),
    });
  };

  const actionBodyTemplate = (
    rowData: TasacionDetalle,
    options: { rowIndex: number },
  ) => (
    <div className="flex gap-2 justify-center">
      <Button
        type="button"
        icon="pi pi-eye"
        severity="secondary"
        outlined
        size="small"
        onClick={() => onView(rowData, options.rowIndex)}
        tooltip="Ver detalle"
      />
      <Button
        key={`editar-${gridModeKey}-${options.rowIndex}`}
        type="button"
        icon="pi pi-pencil"
        severity="info"
        outlined
        size="small"
        disabled={canModifyRows ? false : true}
        onClick={() => onEdit(rowData, options.rowIndex)}
        tooltip="Editar"
      />
      <Button
        key={`eliminar-${gridModeKey}-${options.rowIndex}`}
        type="button"
        icon="pi pi-trash"
        severity="danger"
        outlined
        size="small"
        disabled={canModifyRows ? false : true}
        onClick={() => confirmDelete(rowData, options.rowIndex)}
        tooltip="Eliminar"
      />
    </div>
  );

  const totalFooterTemplate = (
    label: string,
    value: number,
    isCurrency: boolean,
  ) => (
    <div className="text-right font-semibold">
      <small className="block text-xs text-gray-500">{label}</small>
      {isCurrency ? formatPesos(value) : formatNumber(value)}
    </div>
  );

  const footerGroup = visibleRows.length > 0 && (
    <ColumnGroup>
      <Row>
        <Column
          footer="Suma Total"
          colSpan={4}
          footerStyle={{ textAlign: "right", fontWeight: 700 }}
        />
        <Column
          footer={() => totalFooterTemplate("Total UF", totals.valor_tasacion_uf, false)}
        />
        <Column
          footer={() => totalFooterTemplate("Total $", totals.valor_tasacion_pesos, true)}
        />
        <Column
          footer={() => totalFooterTemplate("Total UF", totals.valor_liquidacion_uf, false)}
        />
        <Column
          footer={() => totalFooterTemplate("Total $", totals.valor_liquidacion_pesos, true)}
        />
        <Column
          footer={() => totalFooterTemplate("Total UF", totals.monto_asegurable_uf, false)}
        />
        <Column
          footer={() => totalFooterTemplate("Total $", totals.monto_asegurable_pesos, true)}
        />
        <Column colSpan={3} />
      </Row>
    </ColumnGroup>
  );

  return (
    <div className="flex flex-col gap-2">
      <ConfirmDialog />
      <DataTable
        key={`tasacion-registrada-grid-${gridModeKey}`}
        value={visibleRows}
        dataKey="id_tasacion_detalle"
        emptyMessage="Sin tasaciones registradas"
        responsiveLayout="scroll"
        stripedRows
        size="small"
        className="datatable-presto"
        scrollable
        scrollHeight="flex"
        footerColumnGroup={footerGroup || undefined}
      >
        <Column header="N° Tasación" body={nroTasacionTemplate} style={{ minWidth: "180px" }} />
        <Column header="Tipo" body={tipoTasacionTemplate} style={{ minWidth: "180px" }} />
        <Column
          header="Sup. Edificada"
          field="superficie_edificada"
          body={(row: TasacionDetalle) => row.superficie_edificada ?? "-"}
          style={{ minWidth: "120px" }}
        />
        <Column
          header="Sup. Terreno"
          field="superficie_terreno"
          body={(row: TasacionDetalle) => row.superficie_terreno ?? "-"}
          style={{ minWidth: "120px" }}
        />
        <Column
          header="Valor Tasación (UF)"
          body={(row: TasacionDetalle) => (
            <div className="text-right">{formatNumber(row.valor_tasacion_uf)}</div>
          )}
          style={{ minWidth: "130px" }}
        />
        <Column
          header="Valor Tasación ($)"
          body={(row: TasacionDetalle) => (
            <div className="text-right">{formatPesos(row.valor_tasacion_pesos)}</div>
          )}
          style={{ minWidth: "150px" }}
        />
        <Column
          header="V. Liquidación (UF)"
          body={(row: TasacionDetalle) => (
            <div className="text-right">{formatNumber(row.valor_liquidacion_uf)}</div>
          )}
          style={{ minWidth: "130px" }}
        />
        <Column
          header="V. Liquidación ($)"
          body={(row: TasacionDetalle) => (
            <div className="text-right">{formatPesos(row.valor_liquidacion_pesos)}</div>
          )}
          style={{ minWidth: "150px" }}
        />
        <Column
          header="Monto Aseg. (UF)"
          body={(row: TasacionDetalle) => (
            <div className="text-right">{formatNumber(row.monto_asegurable_uf)}</div>
          )}
          style={{ minWidth: "130px" }}
        />
        <Column
          header="Monto Aseg. ($)"
          body={(row: TasacionDetalle) => (
            <div className="text-right">{formatPesos(row.monto_asegurable_pesos)}</div>
          )}
          style={{ minWidth: "150px" }}
        />
        <Column
          header="F. Informe"
          body={(row: TasacionDetalle) => formatDate(row.fecha_informe_tasacion)}
          style={{ minWidth: "110px" }}
        />
        <Column
          header="F. Recepción"
          body={(row: TasacionDetalle) => formatDate(row.fecha_recepcion_tasacion)}
          style={{ minWidth: "110px" }}
        />
        <Column
          key={`acciones-${gridModeKey}`}
          header="Acciones"
          body={actionBodyTemplate}
          style={{ width: "180px", textAlign: "center" }}
        />
      </DataTable>
    </div>
  );
}
