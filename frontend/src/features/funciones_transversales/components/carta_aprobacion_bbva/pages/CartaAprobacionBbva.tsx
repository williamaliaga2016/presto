import { useRef } from 'react';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Toast } from 'primereact/toast';
import type { ToastMessage } from 'primereact/toast';
import { useGenerarCartaAprobacion } from '../hooks/useGenerarCartaAprobacion';
import { useHistoricoCartaAprobacion } from '../hooks/useHistoricoCartaAprobacion';
import type { CartaAprobacionBbva } from '../api/cartaAprobacionBbvaService';

type Props = {
  id_expediente: number;
};

function formatFecha(value: string | null | undefined): string {
  if (!value) return '';
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return date.toLocaleDateString('es-CO', { day: '2-digit', month: '2-digit', year: 'numeric' });
}

function formatHora(value: string | null | undefined): string {
  if (!value) return '';
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return '';
  return date.toLocaleTimeString('es-CO', { hour: '2-digit', minute: '2-digit', hour12: true });
}

export default function CartaAprobacionBbvaPage({ id_expediente }: Props) {
  const toast = useRef<Toast>(null);

  const { data: historicoResponse, isLoading } = useHistoricoCartaAprobacion(id_expediente);
  const registros: CartaAprobacionBbva[] = historicoResponse?.status
    ? (historicoResponse.detail ?? [])
    : [];

  const { mutate: generarCarta, isPending } = useGenerarCartaAprobacion(id_expediente);

  const handleGenerar = () => {
    generarCarta(undefined, {
      onSuccess: (res) => {
        const msg: ToastMessage = res.status
          ? { severity: 'success', summary: 'Éxito', detail: res.message ?? 'Carta generada correctamente.', life: 5000 }
          : { severity: 'warn', summary: 'Aviso', detail: res.message ?? 'No se pudo generar la carta.', life: 5000 };
        toast.current?.show(msg);
      },
      onError: (error: unknown) => {
        const detail = error instanceof Error ? error.message : 'Error al generar la carta de aprobación.';
        toast.current?.show({ severity: 'error', summary: 'Error', detail, life: 6000 });
      },
    });
  };

  const archivoBodyTemplate = (rowData: CartaAprobacionBbva) => {
    if (!rowData.nombre_archivo_pdf) return null;
    return (
      <span className="flex items-center gap-2 text-red-600 text-sm font-medium">
        <i className="pi pi-file-pdf text-red-500" />
        {rowData.nombre_archivo_pdf}
      </span>
    );
  };

  return (
    <>
      <Toast ref={toast} />

      <div className="flex flex-col gap-4">
        <Card className="w-full shadow-md">
          <div className="flex items-center justify-between gap-4">
            <div className="flex items-center gap-3">
              <i className="pi pi-file-edit text-blue-900 text-2xl" />
              <div className="flex flex-col gap-1">
                <p className="font-semibold text-blue-900 text-base m-0">
                  Generar Carta de Aprobación
                </p>
                <p className="text-sm text-gray-500 m-0">
                  Haga clic en el botón para generar una nueva carta de aprobación o renovar la existente en el folio.
                </p>
              </div>
            </div>
            <Button
              label={isPending ? 'Generando...' : 'Generar / Renovar Carta'}
              icon={isPending ? 'pi pi-spin pi-spinner' : 'pi pi-refresh'}
              disabled={isPending || id_expediente <= 0}
              onClick={handleGenerar}
            />
          </div>
        </Card>

        <Card className="w-full shadow-md">
          <div className="flex items-center justify-between mb-3">
            <h3 className="font-semibold text-gray-800 text-base m-0">
              Histórico de Cartas de Aprobación
            </h3>
            <span className="text-sm bg-gray-100 text-gray-600 px-3 py-1 rounded-full font-medium">
              {registros.length} {registros.length === 1 ? 'Registro' : 'Registros'}
            </span>
          </div>

          <DataTable
            value={registros}
            className="text-sm"
            loading={isLoading}
            emptyMessage="No hay registros generados en el historial."
            size="small"
          >
            <Column
              header="FECHA"
              body={(row: CartaAprobacionBbva) => formatFecha(row.modified_date ?? row.created_date)}
            />
            <Column
              header="HORA"
              body={(row: CartaAprobacionBbva) => formatHora(row.modified_date ?? row.created_date)}
            />
            <Column
              header="CARTA DE APROBACIÓN"
              body={archivoBodyTemplate}
            />
          </DataTable>
        </Card>
      </div>
    </>
  );
}
