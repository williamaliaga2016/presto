import { useState } from 'react';
import { Dialog } from 'primereact/dialog';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';

import { useReportes } from '../hooks/useReportes';
import { ReporteDTO } from '../models/reporte';
import ReportViewerPage from './ReportViewerPage';

export default function ReportesPage() {
  const { data, isLoading, isError } = useReportes();
  const reportes = data?.detail ?? [];
  const [reporteSeleccionado, setReporteSeleccionado] = useState<ReporteDTO | null>(null);
  const [mostrarVisor, setMostrarVisor] = useState(false);

  const renderResponsiveCell = (value: React.ReactNode, titulo: string) => {
    return <div data-titulo={titulo}>{value ?? ''}</div>;
  };

  const nombreBodyTemplate = (rowData: ReporteDTO) =>
    renderResponsiveCell(rowData.nombre, 'Nombre: ');

  const descripcionBodyTemplate = (rowData: ReporteDTO) =>
    renderResponsiveCell(rowData.descripcion ?? '', 'Descripción: ');

  const accionBodyTemplate = (rowData: ReporteDTO) => {
    return (
      <div data-titulo="Acción: ">
        <button
          type="button"
          onClick={() => {
            setReporteSeleccionado(rowData);
            setMostrarVisor(true);
          }}
          className="flex items-center justify-center 
                     bg-[#03298e] text-white 
                     w-9 h-9 rounded-lg 
                     hover:bg-[#1d46cc] transition-all"
          title="Ver reporte"
        >
          <i className="pi pi-eye text-sm"></i>
        </button>
      </div>
    );
  };

  return (
    <>
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Reportes</h2>

      <div className="w-full overflow-hidden rounded-md border border-gray-200">
        <DataTable
          value={reportes}
          paginator
          rows={5}
          rowsPerPageOptions={[5, 10, 20]}
          className="w-full"
          size="small"
          emptyMessage="No se encontraron reportes."
          responsiveLayout="scroll"
        >
          <Column field="nombre" header="Nombre" body={nombreBodyTemplate} />
          <Column field="descripcion" header="Descripción" body={descripcionBodyTemplate} />
          <Column
            header="Acción"
            body={accionBodyTemplate}
            style={{ width: '120px' }}
          />
        </DataTable>
      </div>

      <Dialog
        header={reporteSeleccionado?.nombre ?? 'Reporte'}
        visible={mostrarVisor}
        maximizable
        modal
        style={{ width: '95vw' }}
        contentStyle={{ height: 'calc(100vh - 150px)', padding: 0 }}
        onHide={() => setMostrarVisor(false)}
      >
        {reporteSeleccionado && (
          <ReportViewerPage report_path={reporteSeleccionado.report_path} />
        )}
      </Dialog>
    </>
  );
}
