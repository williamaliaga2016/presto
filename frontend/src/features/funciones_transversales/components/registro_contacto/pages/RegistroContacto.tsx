import { useState } from 'react';
import { Button } from 'primereact/button';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';
import { SelectButton } from 'primereact/selectbutton';
import { Calendar } from 'primereact/calendar';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import {
  EMPTY_REGISTRO_CONTACTO,
  type RegistroContactoBBVA,
} from '../models/registroContacto';
import {
  EMPTY_CONTROLES_REGISTRO_CONTACTO,
} from '../models/catalogo';
import {
  useControlesRegistroContacto,
  useCrearRegistroContacto,
  useRegistrosContacto,
} from '../hooks/useRegistroContacto';

type Props = {
  id_expediente: number;
  id_actividad: string;
};

const SI_NO_OPTIONS = [
  { label: 'Si', value: true },
  { label: 'No', value: false },
];

const formatFecha = (value?: string | null) =>
  value ? new Date(value).toLocaleDateString('es-CO') : '-';

const parseDate = (value?: string | null): Date | null => {
  if (!value) return null;
  const parsedDate = new Date(value);
  return Number.isNaN(parsedDate.getTime()) ? null : parsedDate;
};

const toIso = (value: Date | null | undefined): string | null =>
  value ? value.toISOString() : null;

const formatSiNo = (value?: boolean | null) => {
  if (value === true) return 'Si';
  if (value === false) return 'No';
  return '-';
};

const formatCatalogo = (
  options: ControlBaseDTO[],
  code?: string | null,
) => {
  if (!code) return '-';
  return options.find((option) => option.code === code)?.description ?? code;
};

/**
 * Funcion transversal para registrar y consultar contactos del expediente.
 *
 * @param props Expediente y actividad actual para trazabilidad del contacto creado.
 * @returns Pestaña transversal Registro Contacto.
 */
export default function RegistroContactoPage({
  id_expediente,
  id_actividad,
}: Props) {
  const [visible, setVisible] = useState(false);
  const [form, setForm] = useState<RegistroContactoBBVA>(
    EMPTY_REGISTRO_CONTACTO(id_expediente, id_actividad),
  );

  const { data: controlesData } = useControlesRegistroContacto();
  const { data: registros } = useRegistrosContacto(id_expediente);
  const { mutate: crearRegistro, isPending } = useCrearRegistroContacto();
  const controles = controlesData?.status
    ? (controlesData.detail ?? EMPTY_CONTROLES_REGISTRO_CONTACTO)
    : EMPTY_CONTROLES_REGISTRO_CONTACTO;
  const registrosContacto = registros?.detail ?? [];
  const siguienteContacto = registrosContacto.length > 0
    ? Math.max(...registrosContacto.map((registro) => registro.nro_contacto ?? 0)) + 1
    : 1;
  const resultadosContacto = (controles.resultado_contacto ?? []).filter(
    (resultado) => resultado.code?.startsWith('RC-'),
  );
  const detallesContacto = (controles.detalle_contacto ?? []).filter(
    (detalle) =>
      detalle.code?.startsWith('RCD-') &&
      detalle.parent_code === form.resultado_contacto,
  );

  /**
   * Abre el modal con el numero de contacto sugerido para el expediente.
   *
   * @returns No retorna valor.
   */
  const handleAbrirModal = () => {
    setForm({
      ...EMPTY_REGISTRO_CONTACTO(id_expediente, id_actividad),
      nro_contacto: siguienteContacto,
      fecha_contacto: new Date().toISOString(),
    });
    setVisible(true);
  };

  /**
   * Persiste el contacto actual y refresca la tabla historica global.
   *
   * @returns No retorna valor.
   */
  const handleGuardar = () => {
    crearRegistro(form, {
      onSuccess: () => {
        setVisible(false);
        setForm(EMPTY_REGISTRO_CONTACTO(id_expediente, id_actividad));
      },
    });
  };

  /**
   * Cambia el resultado y limpia el detalle cuando la dependencia L8/L9 cambia.
   *
   * @param resultado Codigo L8 seleccionado.
   * @returns No retorna valor.
   */
  const handleResultadoChange = (resultado: string) => {
    setForm((f) => ({
      ...f,
      resultado_contacto: resultado,
      detalle_contacto: null,
    }));
  };

  return (
    <div>
      <div className="flex justify-end mb-3">
        <Button
          label="Agregar"
          icon="pi pi-plus"
          size="small"
          onClick={handleAbrirModal}
          disabled={!id_expediente}
        />
      </div>

      <DataTable
        value={registrosContacto}
        paginator
        rows={5}
        rowsPerPageOptions={[5, 10, 20]}
        className="text-sm"
        size="small"
        emptyMessage="Sin registros de contacto"
      >
        <Column field="nro_contacto" header="Nro." style={{ width: '80px' }} />
        <Column
          field="fecha_contacto"
          header="Fecha"
          body={(r: RegistroContactoBBVA) => formatFecha(r.fecha_contacto)}
          style={{ width: '160px' }}
        />
        <Column
          field="canal_contacto"
          header="Canal"
          body={(r: RegistroContactoBBVA) =>
            formatCatalogo(controles.canal_contacto ?? [], r.canal_contacto)
          }
          style={{ width: '140px' }}
        />
        <Column
          field="resultado_contacto"
          header="Resultado"
          body={(r: RegistroContactoBBVA) =>
            formatCatalogo(controles.resultado_contacto ?? [], r.resultado_contacto)
          }
          style={{ width: '180px' }}
        />
        <Column
          field="detalle_contacto"
          header="Detalle"
          body={(r: RegistroContactoBBVA) =>
            formatCatalogo(controles.detalle_contacto ?? [], r.detalle_contacto)
          }
          style={{ width: '180px' }}
        />
        <Column
          field="inmueble_definido"
          header="Inmueble Definido"
          body={(r: RegistroContactoBBVA) => formatSiNo(r.inmueble_definido)}
          style={{ width: '150px' }}
        />
        <Column field="observaciones" header="Observaciones" />
      </DataTable>

      <Dialog
        header="Registrar Contacto con Cliente"
        visible={visible}
        onHide={() => setVisible(false)}
        style={{ width: '520px' }}
      >
        <div className="flex flex-col gap-4 p-2">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Nro. Contacto</label>
              <InputText
                value={String(form.nro_contacto ?? siguienteContacto)}
                disabled
                className="w-full"
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Fecha Contacto</label>
              <Calendar
                value={parseDate(form.fecha_contacto)}
                onChange={(e) =>
                  setForm((f) => ({
                    ...f,
                    fecha_contacto: toIso(e.value as Date | null) ?? undefined,
                  }))
                }
                dateFormat="dd/mm/yy"
                showIcon
                className="w-full"
              />
            </div>
          </div>

          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Canal de Contacto *</label>
            <Dropdown
              value={form.canal_contacto}
              options={controles.canal_contacto}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => setForm((f) => ({ ...f, canal_contacto: e.value }))}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Resultado de Contacto *</label>
            <Dropdown
              value={form.resultado_contacto}
              options={resultadosContacto}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => handleResultadoChange(e.value)}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">
              Detalle{detallesContacto.length > 0 ? ' *' : ''}
            </label>
            <Dropdown
              value={form.detalle_contacto}
              options={detallesContacto}
              optionLabel="description"
              optionValue="code"
              disabled={!form.resultado_contacto}
              onChange={(e) =>
                setForm((f) => ({ ...f, detalle_contacto: e.value }))
              }
              placeholder={
                form.resultado_contacto && detallesContacto.length === 0
                  ? 'Sin detalles configurados'
                  : 'Seleccionar...'
              }
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-2">
            <label className="text-xs text-gray-500">Inmueble Definido *</label>
            <SelectButton
              value={form.inmueble_definido}
              options={SI_NO_OPTIONS}
              onChange={(e) =>
                setForm((f) => ({ ...f, inmueble_definido: e.value }))
              }
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Observaciones</label>
            <InputTextarea
              value={form.observaciones ?? ''}
              onChange={(e) =>
                setForm((f) => ({ ...f, observaciones: e.target.value }))
              }
              rows={3}
              className="w-full"
            />
          </div>
          <div className="flex justify-end gap-2">
            <Button
              label="Cancelar"
              severity="secondary"
              outlined
              onClick={() => setVisible(false)}
            />
            <Button
              label="Guardar"
              loading={isPending}
              onClick={handleGuardar}
              disabled={
                !form.canal_contacto ||
                !form.fecha_contacto ||
                !form.resultado_contacto ||
                form.inmueble_definido == null ||
                (detallesContacto.length > 0 && !form.detalle_contacto)
              }
            />
          </div>
        </div>
      </Dialog>
    </div>
  );
}
