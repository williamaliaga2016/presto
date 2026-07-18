import { useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { DataTable, type DataTableRowClickEvent } from "primereact/datatable";
import { Column } from "primereact/column";
import { useActividadDashboard } from "../hooks/useActividadDashboard";
import { useUpdateStatusActividad } from "../hooks/useUpdateStatusActividad";
import type { ActividadDTO } from "../models/ActividadDTO";

export default function BandejaActividades() {
  const navigate = useNavigate();
  const { data, isLoading, isError } = useActividadDashboard();
  const updateStatusActividad = useUpdateStatusActividad();

  const actividades = useMemo<ActividadDTO[]>(() => {
    if (!data?.status || !data.detail) return [];
    return data.detail;
  }, [data]);

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

  const expedienteBodyTemplate = (rowData: ActividadDTO) =>
    renderResponsiveCell(rowData.id_expediente, "Nro Expediente: ");

  const actividadBodyTemplate = (rowData: ActividadDTO) =>
    renderResponsiveCell(rowData.actividad, "Actividad: ");

  const usuarioBodyTemplate = (rowData: ActividadDTO) =>
    renderResponsiveCell(rowData.nombre_responsable, "Usuario: ");

  const perfilBodyTemplate = (rowData: ActividadDTO) =>
    renderResponsiveCell(rowData.rol, "Perfil: ");

  const estadoBodyTemplate = (rowData: ActividadDTO) =>
    renderResponsiveCell(rowData.estado, "Estado: ");

  const fechaBodyTemplate = (rowData: ActividadDTO) =>
    renderResponsiveCell(
      formatFecha(rowData.fecha_asignacion),
      "Fec Asignación: "
    );

  const handleRowClick = async (event: DataTableRowClickEvent) => {
    const row = event.data as ActividadDTO;

    if (!row?.url_act) return;
    if (!row?.id_expediente) return;

    try {
      await updateStatusActividad.mutateAsync(row);
      navigate(`/home/${row.url_act}/${row.id_expediente}`);
    } catch (error) {
      console.error("Error al actualizar el estado de la actividad.", error);
    }
  };

  return (
    <>
      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Bandeja de Actividades
      </h2>

      {isError && (
        <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          Ocurrió un error al cargar la bandeja de actividades.
        </div>
      )}

      <div className="w-full overflow-x-auto rounded-md border border-gray-200">
        <DataTable
          value={actividades}
          paginator
          rows={5}
          rowsPerPageOptions={[5, 10, 20]}
          onRowClick={handleRowClick}
          className="text-sm"
          size="small"
          loading={isLoading}
          emptyMessage="No se encontraron actividades."
        >
          <Column
            field="id_expediente"
            header="Nro Expediente"
            body={expedienteBodyTemplate}
          />
          <Column
            field="actividad"
            header="Actividad"
            body={actividadBodyTemplate}
          />
          <Column
            field="nombre_responsable"
            header="Usuario"
            body={usuarioBodyTemplate}
          />
          <Column
            field="rol"
            header="Perfil"
            body={perfilBodyTemplate}
          />
          <Column
            field="estado"
            header="Estado"
            body={estadoBodyTemplate}
          />
          <Column
            field="fecha_asignacion"
            header="Fec Asignación"
            body={fechaBodyTemplate}
          />
        </DataTable>
      </div>
    </>
  );
}