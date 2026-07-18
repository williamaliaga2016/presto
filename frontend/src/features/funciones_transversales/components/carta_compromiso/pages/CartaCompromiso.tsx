import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';

type CartaCompromiso = {
  id?: number;
  fecha: string;
  hora: string;
  nombre_archivo: string;
  url_pdf?: string;
};

type Props = {
  id_expediente: number;
};

export default function CartaCompromisoPage({ id_expediente: _ }: Props) {
  const registros: CartaCompromiso[] = [];

  const archivoBodyTemplate = (rowData: CartaCompromiso) => (
    <a
      href={rowData.url_pdf ?? '#'}
      target="_blank"
      rel="noopener noreferrer"
      className="flex items-center gap-2 text-red-600 hover:text-red-800 text-sm font-medium"
    >
      <i className="pi pi-file-pdf text-red-500" />
      {rowData.nombre_archivo}
    </a>
  );

  return (
    <div className="flex flex-col gap-4">
      <Card className="w-full shadow-md">
        <div className="flex items-center justify-between gap-4">
          <div className="flex items-center gap-3">
            <i className="pi pi-users text-blue-900 text-2xl" />
            <div className="flex flex-col gap-1">
              <p className="font-semibold text-blue-900 text-base m-0">
                Generar Carta de Compromiso
              </p>
              <p className="text-sm text-gray-500 m-0">
                Haga clic en el botón para generar una nueva carta de compromiso o renovar la existente en el folio.
              </p>
            </div>
          </div>
          <Button
            label="Generar / Renovar Carta"
            icon="pi pi-refresh"
          />
        </div>
      </Card>

      <Card className="w-full shadow-md">
        <div className="flex items-center justify-between mb-3">
          <h3 className="font-semibold text-gray-800 text-base m-0">
            Histórico de Cartas de Compromiso
          </h3>
          <span className="text-sm bg-gray-100 text-gray-600 px-3 py-1 rounded-full font-medium">
            {registros.length} {registros.length === 1 ? 'Registro' : 'Registros'}
          </span>
        </div>

        <DataTable
          value={registros}
          className="text-sm"
          emptyMessage="No hay registros generados en el historial."
          size="small"
        >
          <Column field="fecha" header="FECHA" />
          <Column field="hora" header="HORA" />
          <Column
            field="nombre_archivo"
            header="CARTA DE COMPROMISO"
            body={archivoBodyTemplate}
          />
        </DataTable>
      </Card>
    </div>
  );
}
