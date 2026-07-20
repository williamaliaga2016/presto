import { useState } from 'react';
import { Button } from 'primereact/button';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { Checkbox } from 'primereact/checkbox';
import type { TitularBBVA, ValidarInformacionBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';
import { useAgregarTitular, useTitulares } from '../hooks/useTitulares';

type Props = {
  id_expediente?: number;
  data: ValidarInformacionBBVA;
  controles: ControlesValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
  invalidFields?: Set<string>;
};

export default function DatosTitularSection({
  id_expediente, data, controles, isEditing, onChange, invalidFields,
}: Props) {
  const inv = (field: string) => invalidFields?.has(field) ?? false;
  const expedienteTitulares = id_expediente ?? data.id_expediente;
  const [visible, setVisible] = useState(false);
  const [nuevoTitular, setNuevoTitular] = useState<TitularBBVA>({
    id_expediente: expedienteTitulares,
    id_actividad: 'ACT_VALIDAR_INFO',
  });
  const { data: titularesData } = useTitulares(expedienteTitulares);
  const agregarTitular = useAgregarTitular();
  const titularesRegistrados = titularesData?.detail ?? [];
  const puedeAgregarTitular = titularesRegistrados.length < 10;
  const titulares = [
    { label: 'Titular 1', prefix: 't1' as const },
    { label: 'Titular 2', prefix: 't2' as const },
    { label: 'Titular 3', prefix: 't3' as const },
  ];

  const updateNuevo = (field: keyof TitularBBVA, value: unknown) => {
    setNuevoTitular((current) => ({ ...current, [field]: value }));
  };

  const guardarNuevoTitular = () => {
    agregarTitular.mutate(
      {
        ...nuevoTitular,
        id_expediente: expedienteTitulares,
        id_actividad: 'ACT_VALIDAR_INFO',
      },
      {
        onSuccess: () => {
          setVisible(false);
          setNuevoTitular({
            id_expediente: expedienteTitulares,
            id_actividad: 'ACT_VALIDAR_INFO',
          });
        },
      },
    );
  };

  const titularIncompleto =
    !nuevoTitular.tipo_identificacion ||
    !nuevoTitular.numero_identificacion ||
    !nuevoTitular.nombre_completo ||
    !nuevoTitular.celular_cliente ||
    !nuevoTitular.telefono_residente ||
    !nuevoTitular.email ||
    !nuevoTitular.direccion_residencia;

  return (
    <div className="flex flex-col gap-5">
      <div className="flex justify-end">
        <Button
          label="Agregar Titular"
          icon="pi pi-plus"
          size="small"
          disabled={!puedeAgregarTitular}
          onClick={() => setVisible(true)}
        />
      </div>

      <DataTable
        value={titularesRegistrados}
        size="small"
        emptyMessage="Sin titulares adicionales registrados"
      >
        <Column field="numero_titular" header="#" style={{ width: '70px' }} />
        <Column field="tipo_identificacion" header="Tipo ID" />
        <Column field="numero_identificacion" header="Numero ID" />
        <Column field="nombre_completo" header="Nombre" />
        <Column field="celular_cliente" header="Celular" />
        <Column field="telefono_residente" header="Telefono" />
        <Column field="email" header="Email" />
      </DataTable>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {titulares.map(({ label, prefix }) => (
          <div key={prefix} className="col-span-full border rounded-lg p-4">
            <h4 className="font-semibold text-blue-800 mb-3">{label}</h4>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Tipo Documento</label>
              <Dropdown
                value={data[`tipo_id_${prefix}` as keyof ValidarInformacionBBVA]}
                options={controles.tipo_documento_id}
                optionLabel="description"
                optionValue="code"
                disabled={!isEditing}
                onChange={(e) => onChange(`tipo_id_${prefix}` as keyof ValidarInformacionBBVA, e.value)}
                placeholder="Seleccionar..."
                className={`w-full${inv(`tipo_id_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Número Documento</label>
              <InputText
                value={(data[`numero_id_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`numero_id_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`numero_id_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1 col-span-full">
              <label className="text-xs text-gray-500">Nombre Completo</label>
              <InputText
                value={(data[`nombre_completo_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`nombre_completo_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`nombre_completo_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Celular</label>
              <InputText
                value={(data[`celular_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`celular_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`celular_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Telefono Residente</label>
              <InputText
                value={(data[`telefono_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`telefono_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`telefono_${prefix}`) ? ' p-invalid' : ''}`}
              />
              {inv(`telefono_${prefix}`) && (
                <small className="p-error">El telefono residente es requerido</small>
              )}
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Email</label>
              <InputText
                value={(data[`email_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`email_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`email_${prefix}`) ? ' p-invalid' : ''}`}
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Direccion Residencia</label>
              <InputText
                value={(data[`direccion_${prefix}` as keyof ValidarInformacionBBVA] as string) ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`direccion_${prefix}` as keyof ValidarInformacionBBVA, e.target.value)}
                className={`w-full${inv(`direccion_${prefix}`) ? ' p-invalid' : ''}`}
              />
              {inv(`direccion_${prefix}`) && (
                <small className="p-error">La direccion de residencia es requerida</small>
              )}
            </div>
            {prefix === 't1' && (
              <>
                <div className="flex flex-col gap-1">
                  <label className="text-xs text-gray-500">Situación Laboral</label>
                  <Dropdown
                    value={data.situacion_laboral_t1}
                    options={controles.situacion_laboral}
                    optionLabel="description"
                    optionValue="code"
                    disabled={!isEditing}
                    onChange={(e) => onChange('situacion_laboral_t1', e.value)}
                    placeholder="Seleccionar..."
                    className="w-full"
                  />
                </div>
                <div className="flex items-center gap-2 mt-4">
                  <Checkbox
                    className="calculo-checkbox-visible"
                    checked={data.cliente_nomina_t1 ?? false}
                    disabled={!isEditing}
                    onChange={(e) => onChange('cliente_nomina_t1', e.checked)}
                    inputId="cliente_nomina"
                  />
                  <label htmlFor="cliente_nomina" className="text-sm">
                    Cliente Nómina BBVA
                  </label>
                </div>
              </>
            )}
            </div>
          </div>
        ))}
      </div>

      <Dialog
        header={`Agregar Titular (${titularesRegistrados.length}/10)`}
        visible={visible}
        onHide={() => setVisible(false)}
        style={{ width: '560px' }}
      >
        <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Tipo Identificacion *</label>
            <Dropdown
              value={nuevoTitular.tipo_identificacion}
              options={controles.tipo_documento_id}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateNuevo('tipo_identificacion', e.value)}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Numero Identificacion *</label>
            <InputText
              value={nuevoTitular.numero_identificacion ?? ''}
              onChange={(e) => updateNuevo('numero_identificacion', e.target.value)}
            />
          </div>
          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="text-xs text-gray-500">Nombre Completo *</label>
            <InputText
              value={nuevoTitular.nombre_completo ?? ''}
              onChange={(e) => updateNuevo('nombre_completo', e.target.value)}
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Celular *</label>
            <InputText
              value={nuevoTitular.celular_cliente ?? ''}
              onChange={(e) => updateNuevo('celular_cliente', e.target.value)}
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Telefono Residente *</label>
            <InputText
              value={nuevoTitular.telefono_residente ?? ''}
              onChange={(e) => updateNuevo('telefono_residente', e.target.value)}
            />
          </div>
          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="text-xs text-gray-500">Email *</label>
            <InputText
              value={nuevoTitular.email ?? ''}
              onChange={(e) => updateNuevo('email', e.target.value)}
            />
          </div>
          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="text-xs text-gray-500">Direccion Residencia *</label>
            <InputText
              value={nuevoTitular.direccion_residencia ?? ''}
              onChange={(e) => updateNuevo('direccion_residencia', e.target.value)}
            />
          </div>
          <div className="flex justify-end gap-2 md:col-span-2 pt-2">
            <Button
              label="Cancelar"
              severity="secondary"
              outlined
              onClick={() => setVisible(false)}
            />
            <Button
              label="Guardar"
              loading={agregarTitular.isPending}
              disabled={titularIncompleto}
              onClick={guardarNuevoTitular}
            />
          </div>
        </div>
      </Dialog>
    </div>
  );
}
