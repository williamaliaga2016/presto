import { useState } from 'react';
import { Button } from 'primereact/button';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { SelectButton } from 'primereact/selectbutton';

import InputTextForm from '@/shared/components/InputTextForm';
import InputCalendarForm from '@/shared/components/InputCalendarForm';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import {
  EMPTY_REGISTRO_CONTACTO,
  type RegistroContactoBBVA,
} from '@/features/funciones_transversales/components/registro_contacto/models/registroContacto';
import { EMPTY_CONTROLES_REGISTRO_CONTACTO } from '@/features/funciones_transversales/components/registro_contacto/models/catalogo';
import {
  useControlesRegistroContacto,
  useCrearRegistroContacto,
  useRegistrosContacto,
} from '@/features/funciones_transversales/components/registro_contacto/hooks/useRegistroContacto';

interface Props {
  id_expediente: number;
  id_actividad: string;
}

const SI_NO_OPTIONS = [
  { label: 'Si', value: true },
  { label: 'No', value: false },
];

const AREA_CONTACTADA_OPTIONS = [
  { code: 'NOTARIA', description: 'Notaría' },
  { code: 'CONSTRUCTORA', description: 'Constructora' },
  { code: 'CLIENTE', description: 'Cliente' },
];

const formatFecha = (value?: string | null) =>
  value ? new Date(value).toLocaleDateString('es-CO') : '-';

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
  return options.find((o) => o.code === code)?.description ?? code;
};

const formatAreaContactada = (code?: string | null) => {
  if (!code) return '-';
  return AREA_CONTACTADA_OPTIONS.find((o) => o.code === code)?.description ?? code;
};

/**
 * Registro de Contacto específico para la actividad Firmar Escritura Cliente.
 * Incluye el campo adicional "Área Contactada" (Notaría, Constructora, Cliente)
 * mapeado al campo `canal_contacto` del modelo.
 */
export default function RegistroContactoSection({
  id_expediente,
  id_actividad,
}: Props) {
  const [visible, setVisible] = useState(false);
  const [form, setForm] = useState<RegistroContactoBBVA>(
    EMPTY_REGISTRO_CONTACTO(id_expediente, id_actividad),
  );
  const [errors, setErrors] = useState<string[]>([]);

  const { data: controlesData } = useControlesRegistroContacto();
  const { data: registros } = useRegistrosContacto(id_expediente);
  const { mutate: crearRegistro, isPending } = useCrearRegistroContacto();

  const controles = controlesData?.status
    ? (controlesData.detail ?? EMPTY_CONTROLES_REGISTRO_CONTACTO)
    : EMPTY_CONTROLES_REGISTRO_CONTACTO;

  const registrosContacto = [...(registros?.detail ?? [])].sort(
    (a, b) =>
      new Date(b.fecha_contacto ?? '').getTime() -
      new Date(a.fecha_contacto ?? '').getTime(),
  );

  const siguienteContacto =
    registrosContacto.length > 0
      ? Math.max(...registrosContacto.map((r) => r.nro_contacto ?? 0)) + 1
      : 1;

  const resultadosContacto = (controles.resultado_contacto ?? []).filter(
    (r) => r.code?.startsWith('RC-'),
  );

  const detallesContacto = (controles.detalle_contacto ?? []).filter(
    (d) =>
      d.code?.startsWith('RCD-') &&
      (d as ControlBaseDTO & { parent_code?: string }).parent_code ===
        form.resultado_contacto,
  );

  const handleAbrirModal = () => {
    setForm({
      ...EMPTY_REGISTRO_CONTACTO(id_expediente, id_actividad),
      nro_contacto: siguienteContacto,
      fecha_contacto: new Date().toISOString(),
    });
    setErrors([]);
    setVisible(true);
  };

  const validate = (): string[] => {
    const missing: string[] = [];
    if (!form.fecha_contacto) missing.push('Fecha Contacto');
    if (!form.resultado_contacto) missing.push('Resultado de Contacto');
    if (!form.detalle_contacto) missing.push('Detalle');
    if (!form.canal_contacto) missing.push('Área Contactada');
    return missing;
  };

  const handleGuardar = () => {
    const validationErrors = validate();
    if (validationErrors.length > 0) {
      setErrors(validationErrors);
      return;
    }
    setErrors([]);
    crearRegistro(form, {
      onSuccess: () => {
        setVisible(false);
        setForm(EMPTY_REGISTRO_CONTACTO(id_expediente, id_actividad));
      },
    });
  };

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
        <Column field="nro_contacto" header="Nro. Contacto" style={{ width: '100px' }} />
        <Column
          field="fecha_contacto"
          header="Fecha Contacto"
          body={(r: RegistroContactoBBVA) => formatFecha(r.fecha_contacto)}
          style={{ width: '140px' }}
        />
        <Column
          field="resultado_contacto"
          header="Resultado de Contacto"
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
          style={{ width: '160px' }}
        />
        <Column
          field="inmueble_definido"
          header="¿Inmueble Definido?"
          body={(r: RegistroContactoBBVA) => formatSiNo(r.inmueble_definido)}
          style={{ width: '140px' }}
        />
        <Column
          field="canal_contacto"
          header="Área Contactada"
          body={(r: RegistroContactoBBVA) => formatAreaContactada(r.canal_contacto)}
          style={{ width: '140px' }}
        />
        <Column field="observaciones" header="Observaciones" />
      </DataTable>

      <Dialog
        header="Registro de Contacto"
        visible={visible}
        onHide={() => setVisible(false)}
        style={{ width: '560px' }}
      >
        <div className="flex flex-col gap-4 p-2">
          {errors.length > 0 && (
            <div className="bg-red-50 border border-red-300 text-red-700 p-3 rounded text-sm">
              Campos obligatorios faltantes: {errors.join(', ')}
            </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <InputTextForm
              label="Nro. Contacto"
              value={String(form.nro_contacto ?? siguienteContacto)}
              onChange={() => {}}
              disabled
            />
            <InputCalendarForm
              label="Fecha Contacto"
              value={form.fecha_contacto ?? null}
              onChange={(val) =>
                setForm((f) => ({ ...f, fecha_contacto: val ?? undefined }))
              }
              required
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
            <label className="text-xs text-gray-500">Detalle *</label>
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
                !form.resultado_contacto
                  ? 'Seleccione primero un resultado'
                  : detallesContacto.length === 0
                    ? 'Sin detalles configurados'
                    : 'Seleccionar...'
              }
              className="w-full"
            />
          </div>

          <div className="flex flex-col gap-2">
            <label className="text-xs text-gray-500">¿Inmueble Definido?</label>
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
            <label className="text-xs text-gray-500">Área Contactada *</label>
            <Dropdown
              value={form.canal_contacto}
              options={AREA_CONTACTADA_OPTIONS}
              optionLabel="description"
              optionValue="code"
              onChange={(e) =>
                setForm((f) => ({ ...f, canal_contacto: e.value }))
              }
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>

          <InputTextAreaForm
            label="Observaciones"
            value={form.observaciones ?? ''}
            onChange={(val) => setForm((f) => ({ ...f, observaciones: val }))}
            maxLength={500}
            rows={3}
          />

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
            />
          </div>
        </div>
      </Dialog>
    </div>
  );
}
