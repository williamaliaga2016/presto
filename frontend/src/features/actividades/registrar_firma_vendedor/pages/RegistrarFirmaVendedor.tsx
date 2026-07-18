import { useEffect, useMemo, useRef, useState } from 'react';

import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { ConfirmDialog } from 'primereact/confirmdialog';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';
import { Calendar } from 'primereact/calendar';
import { useNavigate, useParams } from 'react-router-dom';
import { Toast } from 'primereact/toast';
import { Accordion, AccordionTab } from 'primereact/accordion';
import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import { useRegistrarFirmaVendedor } from '../hooks/useRegistrarFirmaVendedor';
import { useUpsertRegistrarFirmaVendedor } from '../hooks/useUpsertRegistrarFirmaVendedor';
import { useAvanzarRegistrarFirmaVendedor } from '../hooks/useAvanzarRegistrarFirmaVendedor';

const ACTIVITY_ID = '_Br9T_K1mQeaWp4_F8cJ2s';

type Props = {
  idExpediente?: number;
  idActividad?: string;
};

export default function RegistrarFirmaVendedorPage({
  idExpediente: idExpedienteProp,
  idActividad = ACTIVITY_ID,
}: Props) {
  const navigate = useNavigate();
  const toast = useRef<Toast>(null);
  const { id_expediente: idParam } = useParams();

  const idExpediente = Number(idExpedienteProp ?? idParam ?? 0);

  const hasValidExpediente = idExpediente > 0;
  const resolvedActividadId = idActividad || ACTIVITY_ID;
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(Boolean(resolvedActividadId));
  const [isSaving, setIsSaving] = useState(false);

  const upsertMutation = useUpsertRegistrarFirmaVendedor();
    const avanzarMutation = useAvanzarRegistrarFirmaVendedor();

  const {
    data: firmaData,
    isLoading,
  } = useRegistrarFirmaVendedor(idExpediente);

  const isBusy = isSaving || isLoading;

  const [selected, setSelected] = useState<any>({
    rut: '',
    razon_social: '',
    nombres: '',
    apellido_paterno: '',
    apellido_materno: '',
    profesion: '',
    direccion: '',
    telefono: '',
    email: '',
    observaciones: '',
  });

  const [rows, setRows] = useState<any[]>([]);

  const showToast = (
    severity: 'success' | 'info' | 'warn' | 'error',
    summary: string,
    detail: string,
  ) => {
    toast.current?.show({
      severity,
      summary,
      detail,
      life: 3500,
    });
  };

  const handleEditar = () => {
    if (!hasValidExpediente) {
      showToast('warn', 'Validación', 'El expediente es obligatorio.');
      return;
    }

    setIsDisabled(false);
    setCanAdvance(false);
  };

const handleGuardar = async () => {
  if (!hasValidExpediente) {
    showToast('warn', 'Validación', 'El expediente es obligatorio.');
    return;
  }

  if (!selected.observaciones?.trim()) {
    showToast('warn', 'Validación', 'Las observaciones son obligatorias.');
    return;
  }

  const invalidRow = rows.find((r) => !r.fechaFirma);

  if (invalidRow) {
    showToast(
      'warn',
      'Validación',
      'Todos los registros deben tener Fecha de Firma.'
    );
    return;
  }

  try {
    setIsSaving(true);

    const payload = {
      id_firma_vendedor:
        firmaData?.detail?.id_firma_vendedor ?? 0,

      id_expediente: idExpediente,

      observaciones: selected.observaciones ?? '',

      is_active: true,
      row_status: true,
      created_by: 1,
      created_date: new Date().toISOString(),

      firma_vendedor_detalle: rows.map((r) => ({
        id_firma_vendedor_detalle:
          r.id_firma_vendedor_detalle ?? 0,

        id_firma_vendedor:
          firmaData?.detail?.id_firma_vendedor ?? 0,

        id_expediente: idExpediente,

        relacion_titular: r.relacionTitular,
        rut: r.rut,
        nombres: r.nombres,
        apellido_paterno: r.apellidoPaterno,
        apellido_materno: r.apellidoMaterno,
        estado_civil: r.estadoCivil,

        fecha_firma: r.fechaFirma
          ? new Date(
              r.fechaFirma.getFullYear(),
              r.fechaFirma.getMonth(),
              r.fechaFirma.getDate()
            ).toISOString()
          : null,

        is_active: true,
        row_status: true,
        created_by: 1,
        created_date: new Date().toISOString(),
      })),
    };

    await upsertMutation.mutateAsync(payload);

    setCanAdvance(true);
    setIsDisabled(true);

    showToast(
      'success',
      'OK',
      'Guardado correctamente'
    );
  } catch (error) {
    const message =
      error instanceof Error
        ? error.message
        : 'Error inesperado al guardar.';

    showToast('error', 'Error', message);
  } finally {
    setIsSaving(false);
  }
};

  const handleAvanzar = async () => {
    if (!hasValidExpediente) {
      showToast('warn', 'Validación', 'El expediente es obligatorio.');
      return;
    }

    if (!resolvedActividadId) {
      showToast('warn', 'Validación', 'La actividad no tiene id de workflow válido.');
      return;
    }

    if (!canAdvance) {
      showToast('warn', 'Validación', 'Debe guardar los cambios antes de avanzar.');
      return;
    }

    try {

      const response = await avanzarMutation.mutateAsync(Number(idExpediente));

      if (response.status) {
                toast.current?.show({
                    severity: "success",
                    summary: "Éxito",
                    detail: "Actividad avanzada correctamente",
                    life: 2000,
                });

                navigate("/home/bandeja");}
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Error inesperado al avanzar.';
      showToast('error', 'Error', message);
    }
  };

    const handleSalir = () => {
    navigate('/home/bandeja');
  };

  const updateField = (field: string, value: any) => {
    setSelected((prev: any) => ({
      ...prev,
      [field]: value,
    }));
  };

  useEffect(() => {
  if (firmaData?.detail) {
    const d = firmaData.detail;

    setSelected({
      observaciones: d.observaciones ?? '',
    });

    setRows(
      d.firma_vendedor_detalle?.map((x) => ({
        id_firma_vendedor_detalle: x.id_firma_vendedor_detalle,
        relacionTitular: x.relacion_titular,
        rut: x.rut,
        nombres: x.nombres,
        apellidoPaterno: x.apellido_paterno,
        apellidoMaterno: x.apellido_materno,
        estadoCivil: x.estado_civil,
        fechaFirma: x.fecha_firma
          ? new Date(x.fecha_firma)
          : null,
      })) ?? []
    );
  }
}, [firmaData]);

  return (
    <div>
      <Toast ref={toast} />

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad idExpediente={idExpediente} activityID={resolvedActividadId} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales
            idExpediente={idExpediente}
            idActividad={resolvedActividadId}
          />
        </AccordionTab>

        <AccordionTab header="Registrar Firma Vendedor">
          <Card
              title="Registrar Firma Vendedor"
              className="shadow-sm border border-gray-200 rounded-xl"
            >
              <DataTable
                key={String(isDisabled)}
                value={rows}
                emptyMessage="No existen registros."
                responsiveLayout="scroll"
                stripedRows
                rowHover
          size="small"
        className="text-sm datatable-presto"
              >
                <Column field="relacionTitular" header="Relacion Titular" />
                <Column field="rut" header="RUT" />
                <Column field="nombres" header="Nombres" />
                <Column field="apellidoPaterno" header="Apellido Paterno" />
                <Column field="apellidoMaterno" header="Apellido Materno" />
                <Column field="estadoCivil" header="Estado Civil" />
                <Column
                  header="Fecha de Firma"
                  body={(rowData, options) => (
                    <Calendar
                      value={rowData.fechaFirma}
                      onChange={(e) => {
                        const value = e.value ? new Date(e.value) : null;

                        setRows((prev) =>
                          prev.map((row, i) =>
                            i === options.rowIndex
                              ? { ...row, fechaFirma: value }
                              : { ...row }
                          )
                        );
                      }}
                      disabled={isDisabled}
                      showIcon
                      dateFormat="dd/mm/yy"
                      className="w-full"
                      inputClassName="w-full"
                    />
                  )}
                />
              </DataTable>

                <div className="flex flex-col gap-1 md:col-span-3">
                    <label className="font-semibold text-sm">
                      Observaciones
                    </label>

                    <InputTextarea
                      rows={3}
                      autoResize
                      disabled={isDisabled}
                      className="form-textarea-presto w-full"
                      value={selected.observaciones}
                      onChange={(e) =>
                        updateField('observaciones', e.target.value)
                      }
                    />
                </div>
            </Card>
              <div className="form-actions">
                <Button
                  type="button"
                  label="Editar"
                  icon="pi pi-pencil"
                  severity="info"
                  outlined
                  disabled={isBusy || !isDisabled}
                  onClick={handleEditar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label={isSaving ? 'Guardando...' : 'Guardar'}
                  icon="pi pi-save"
                  severity="success"
                  disabled={isBusy || isDisabled}
                  loading={isSaving}
                  onClick={handleGuardar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label={'Avanzar'}
                  icon="pi pi-arrow-right"
                  severity="warning"
                  disabled={isBusy || !canAdvance || !resolvedActividadId}
                  loading={avanzarMutation.isPending}
                  onClick={handleAvanzar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label="Salir"
                  icon="pi pi-sign-out"
                  severity="secondary"
                  outlined
                  disabled={isBusy}
                  onClick={handleSalir}
                  className="btn-responsive"
                />
              </div>
        </AccordionTab>
      </Accordion>
    </div>
);
}
