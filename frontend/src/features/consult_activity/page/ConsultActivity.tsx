import './consultActivity.css';

import { useMemo, useState } from 'react';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';

import { useCatalogoTipoBusqueda } from '../hooks/useCatalogoTipoBusqueda';
import { useConsultActivity } from '../hooks/useConsultActivity';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import type {
  ConsultActivityDTO,
  SearchCriteriaDTO,
} from '../models/ConsultActivity';
import TrackingActivityPage from './TrackingActivity';

export default function ConsultActivityPage() {
  const [option, setOption] = useState<string>('');
  const [search_criteria, setSearchCriteria] = useState<string>('');
  const [rows, setRows] = useState<ConsultActivityDTO[]>([]);
  const [selectedExpediente, setSelectedExpediente] =
    useState<ConsultActivityDTO | null>(null);
  const [trackingVisible, setTrackingVisible] = useState(false);
  const [formError, setFormError] = useState<string>('');

  const {
    data: catalogoResponse,
    isLoading: isLoadingCatalogo,
    isError: isErrorCatalogo,
  } = useCatalogoTipoBusqueda();

  const consultActivityMutation = useConsultActivity();

  const tipoBusquedaOptions = useMemo(() => {
    const catalogos = catalogoResponse?.status
      ? catalogoResponse.detail ?? []
      : [];

    return catalogos.map((item: ControlBaseDTO) => ({
      label: item.description ?? item.code,
      value: item.code,
    }));
  }, [catalogoResponse]);

  const selectedTipoBusquedaLabel = useMemo(() => {
    const item = tipoBusquedaOptions.find((x) => x.value === option);
    return item?.label ?? 'criterio';
  }, [option, tipoBusquedaOptions]);

  const handleSearch = async () => {
    setFormError('');

    if (!option) {
      setFormError('Seleccione un tipo de búsqueda.');
      return;
    }

    if (!search_criteria.trim()) {
      setFormError('Ingrese el criterio de búsqueda.');
      return;
    }

    const payload: SearchCriteriaDTO = {
      option,
      search_criteria: search_criteria.trim(),
    };

    try {
      const response = await consultActivityMutation.mutateAsync(payload);
      setRows(response.status ? response.detail ?? [] : []);
    } catch (error) {
      console.error('Error al consultar actividades:', error);
      setRows([]);
      setFormError('Ocurrió un error al consultar las actividades.');
    }
  };

  const clearForm = () => {
    setOption('');
    setSearchCriteria('');
    setRows([]);
    setFormError('');
  };

  const openTracking = (row: ConsultActivityDTO) => {
    setSelectedExpediente(row);
    setTrackingVisible(true);
  };

  const formatFecha = (value?: string | null) => {
    if (!value) return '';

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return value;

    return date.toLocaleDateString('es-PE', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
  };

  const renderResponsiveCell = (
    value: React.ReactNode,
    titulo: string,
  ) => <div data-titulo={titulo}>{value ?? ''}</div>;

  const idExpedienteBodyTemplate = (rowData: ConsultActivityDTO) =>
    renderResponsiveCell(rowData.id_expediente, 'Solicitud: ');

  const descripcionBodyTemplate = (rowData: ConsultActivityDTO) =>
    renderResponsiveCell(rowData.descripcion, 'Actividad: ');

  const statusBodyTemplate = (rowData: ConsultActivityDTO) =>
    renderResponsiveCell(rowData.status, 'Estado: ');

  const rolBodyTemplate = (rowData: ConsultActivityDTO) =>
    renderResponsiveCell(rowData.descripcion_rol, 'Rol: ');

  const usuarioBodyTemplate = (rowData: ConsultActivityDTO) =>
    renderResponsiveCell(rowData.usuario, 'Usuario: ');

  const fechaAsignacionBodyTemplate = (rowData: ConsultActivityDTO) =>
    renderResponsiveCell(formatFecha(rowData.fecha_asignacion), 'Fecha Asignación: ');

  const actionBodyTemplate = (rowData: ConsultActivityDTO) => (
    <div className="flex justify-center">
      <Button
        type="button"
        icon="pi pi-eye"
        rounded
        text
        severity="secondary"
        tooltip="Ver detalle"
        onClick={() => openTracking(rowData)}
      />
    </div>
  );

  return (
    <div className="consult-activity-page">
      <Card className="w-full shadow-md card-presto-form mb-6">
        <form
          className="form-grid"
          onSubmit={(event) => {
            event.preventDefault();
            handleSearch();
          }}
        >
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Tipo de búsqueda
            </label>

            <Dropdown
              value={option}
              options={tipoBusquedaOptions}
              onChange={(event) => setOption(event.value)}
              loading={isLoadingCatalogo}
              placeholder="Seleccione"
              className="form-dropdown-presto w-full"
              filter
            />

            {isErrorCatalogo && (
              <small className="text-red-600">
                No se pudo cargar el catálogo de búsqueda.
              </small>
            )}
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              {selectedTipoBusquedaLabel}
            </label>

            <InputText
              value={search_criteria}
              onChange={(event) => setSearchCriteria(event.target.value)}
              placeholder={`Ingrese ${selectedTipoBusquedaLabel}`}
              className="form-input-presto w-full"
            />
          </div>

          <div className="consult-activity-actions">
            <Button
              type="submit"
              label={
                consultActivityMutation.isPending ? 'Buscando...' : 'Buscar'
              }
              icon="pi pi-search"
              outlined
              disabled={consultActivityMutation.isPending}
              className="btn-responsive"
            />

            <Button
              type="button"
              label="Limpiar"
              icon="pi pi-times"
              severity="secondary"
              outlined
              onClick={clearForm}
              disabled={consultActivityMutation.isPending}
              className="btn-responsive"
            />
          </div>
        </form>

        {formError && (
          <div className="mt-3 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
            {formError}
          </div>
        )}

        {!consultActivityMutation.isPending &&
          consultActivityMutation.data &&
          !consultActivityMutation.data.status && (
            <div className="mt-3 rounded-md border border-yellow-200 bg-yellow-50 px-4 py-3 text-sm text-yellow-800">
              {consultActivityMutation.data.message ||
                'No se encontraron actividades.'}
            </div>
          )}

        <div className="w-full mt-6 overflow-x-auto">
          <DataTable
            value={Array.isArray(rows) ? rows : []}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            className="text-sm"
            loading={consultActivityMutation.isPending}
            emptyMessage="No se encontraron resultados"
            size="small"
            sortField="id_expediente"
            sortOrder={-1}
          >
            <Column
              field="id_expediente"
              header="Nro. Solicitud"
              body={idExpedienteBodyTemplate}
            />
            <Column
              field="descripcion"
              header="Actividad"
              body={descripcionBodyTemplate}
            />
            <Column
              field="status"
              header="Estado"
              body={statusBodyTemplate}
            />
            <Column
              field="descripcion_rol"
              header="Rol Asignado"
              body={rolBodyTemplate}
            />
            <Column
              field="usuario"
              header="Usuario"
              body={usuarioBodyTemplate}
            />
            <Column
              field="fecha_asignacion"
              header="Fecha Asignación"
              body={fechaAsignacionBodyTemplate}
            />
            <Column body={actionBodyTemplate} header="Acciones" />
          </DataTable>
        </div>
      </Card>

      <Dialog
        header={
          selectedExpediente
            ? `Solicitud: ${selectedExpediente.id_expediente}`
            : 'Seguimiento de actividad'
        }
        visible={trackingVisible}
        onHide={() => setTrackingVisible(false)}
        style={{ width: '95vw' }}
        modal
        maximizable
      >
        {selectedExpediente && (
          <TrackingActivityPage
            id_expediente={selectedExpediente.id_expediente}
            onClose={() => setTrackingVisible(false)}
          />
        )}
      </Dialog>
    </div>
  );
}
