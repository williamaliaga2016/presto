import { useState } from 'react';
import { Button } from 'primereact/button';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputTextarea } from 'primereact/inputtextarea';
import type { RegistroContactoBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';
import {
  useRegistrosContacto,
  useCrearRegistroContacto,
} from '../hooks/useRegistroContacto';

type Props = {
  id_expediente: number;
  id_actividad: string;
  controles: ControlesValidarInformacion;
};

const EMPTY_REGISTRO = (
  id_expediente: number,
  id_actividad: string,
): RegistroContactoBBVA => ({
  id_expediente,
  id_actividad,
  canal_contacto: '',
  resultado_contacto: '',
  observaciones: '',
});

export default function RegistroContactoSection({
  id_expediente, id_actividad, controles,
}: Props) {
  const [visible, setVisible] = useState(false);
  const [form, setForm] = useState<RegistroContactoBBVA>(
    EMPTY_REGISTRO(id_expediente, id_actividad),
  );

  const { data: registros } = useRegistrosContacto(id_expediente, id_actividad);
  const { mutate: crearRegistro, isPending } = useCrearRegistroContacto();

  const handleGuardar = () => {
    crearRegistro(form, {
      onSuccess: () => {
        setVisible(false);
        setForm(EMPTY_REGISTRO(id_expediente, id_actividad));
      },
    });
  };

  const formatFecha = (value?: string | null) =>
    value ? new Date(value).toLocaleString('es-CO') : '—';

  return (
    <div>
      <div className="flex justify-end mb-3">
        <Button
          label="Registrar Contacto"
          icon="pi pi-plus"
          size="small"
          onClick={() => setVisible(true)}
        />
      </div>

      <DataTable
        value={registros?.detail ?? []}
        size="small"
        emptyMessage="Sin registros de contacto"
      >
        <Column
          field="fecha_contacto"
          header="Fecha"
          body={(r: RegistroContactoBBVA) => formatFecha(r.fecha_contacto)}
          style={{ width: '160px' }}
        />
        <Column field="canal_contacto" header="Canal" style={{ width: '120px' }} />
        <Column field="resultado_contacto" header="Resultado" style={{ width: '140px' }} />
        <Column field="observaciones" header="Observaciones" />
      </DataTable>

      <Dialog
        header="Registrar Contacto con Cliente"
        visible={visible}
        onHide={() => setVisible(false)}
        style={{ width: '480px' }}
      >
        <div className="flex flex-col gap-4 p-2">
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Canal de Contacto</label>
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
            <label className="text-xs text-gray-500">Resultado</label>
            <Dropdown
              value={form.resultado_contacto}
              options={controles.resultado_contacto}
              optionLabel="description"
              optionValue="code"
              onChange={(e) =>
                setForm((f) => ({ ...f, resultado_contacto: e.value }))
              }
              placeholder="Seleccionar..."
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
              disabled={!form.canal_contacto || !form.resultado_contacto}
            />
          </div>
        </div>
      </Dialog>
    </div>
  );
}
