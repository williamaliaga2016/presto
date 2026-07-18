import type { ReactNode } from 'react';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';

import { useHistorialExpediente } from '../hooks/useHistorialExpediente';
import type { HistorialExpediente } from '../models/historialExpediente';

type HistorialExpedientePageProps = {
  id_expediente: number;
};

export default function HistorialExpedientePage({
  id_expediente,
}: HistorialExpedientePageProps) {
  const expediente_id = Number(id_expediente ?? 0);

  const {
    data: response,
    isLoading,
    isError,
    error,
  } = useHistorialExpediente(expediente_id);

  const historial: HistorialExpediente[] = response?.status
    ? response.detail ?? []
    : [];

  const formatFecha = (value?: string | null) => {
    if (!value) return '';

    const date = new Date(value);

    if (Number.isNaN(date.getTime())) return value;

    return date.toLocaleString('es-PE', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false,
    });
  };

  const renderResponsiveCell = (
    value: ReactNode,
    titulo: string,
  ) => {
    return <div data-titulo={titulo}>{value ?? ''}</div>;
  };

  const ordenBodyTemplate = (rowData: HistorialExpediente) =>
    renderResponsiveCell(rowData.orden, 'Orden: ');

  const actividadBodyTemplate = (rowData: HistorialExpediente) =>
    renderResponsiveCell(rowData.actividad, 'Actividad: ');

  const statusBodyTemplate = (rowData: HistorialExpediente) =>
    renderResponsiveCell(rowData.status, 'Estado: ');

  const usuarioBodyTemplate = (rowData: HistorialExpediente) =>
    renderResponsiveCell(rowData.usuario, 'Usuario: ');

  const rolBodyTemplate = (rowData: HistorialExpediente) =>
    renderResponsiveCell(rowData.rol, 'Rol: ');

  const fechaInicioBodyTemplate = (rowData: HistorialExpediente) =>
    renderResponsiveCell(
      formatFecha(rowData.fecha_inicio),
      'Fecha Inicio: ',
    );

  const fechaTerminoBodyTemplate = (rowData: HistorialExpediente) =>
    renderResponsiveCell(
      formatFecha(rowData.fecha_termino),
      'Fecha Término: ',
    );

  if (!expediente_id || expediente_id <= 0) {
    return (
      <div className="mt-2 rounded-md border border-yellow-200 bg-yellow-50 px-4 py-3 text-sm text-yellow-800">
        El historial se habilitará cuando exista un expediente válido.
      </div>
    );
  }

  return (
    <Card className="w-full shadow-md card-presto-form mb-6">
      <div className="w-full overflow-x-auto">
        <DataTable
          value={historial}
          paginator
          rows={5}
          rowsPerPageOptions={[5, 10, 20]}
          className="text-sm"
          loading={isLoading}
          emptyMessage="No hay registros de historial para este expediente"
          size="small"
        >
          <Column
            field="orden"
            header="Orden"
            body={ordenBodyTemplate}
          />

          <Column
            field="actividad"
            header="Actividad"
            body={actividadBodyTemplate}
          />

          <Column
            field="status"
            header="Estado"
            body={statusBodyTemplate}
          />

          <Column
            field="usuario"
            header="Usuario"
            body={usuarioBodyTemplate}
          />

          <Column
            field="rol"
            header="Rol"
            body={rolBodyTemplate}
          />

          <Column
            field="fecha_inicio"
            header="Fecha Inicio"
            body={fechaInicioBodyTemplate}
          />

          <Column
            field="fecha_termino"
            header="Fecha Término"
            body={fechaTerminoBodyTemplate}
          />
        </DataTable>
      </div>

      {isError && (
        <div className="mt-4 text-sm text-red-600">
          Ocurrió un error al cargar el historial del expediente.
          {error instanceof Error ? ` ${error.message}` : ''}
        </div>
      )}

      {!isLoading && response && !response.status && (
        <div className="mt-4 text-sm text-red-600">
          {response.message || 'No se pudo obtener el historial del expediente'}
        </div>
      )}
    </Card>
  );
}