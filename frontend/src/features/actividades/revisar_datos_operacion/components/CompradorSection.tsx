import { useMemo, useState } from 'react';
import { Button } from 'primereact/button';
import { Calendar } from 'primereact/calendar';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';

import { EMPTY_CONTROLES_COMPRADOR, type CatalogoOption, type ControlesComprador } from '../models/catalogo';
import {
  buildCompradorEmpty,
  type RevisarDatosOperacionComprador,
} from '../models/revisar_datos_operacion';

type DialogMode = 'view' | 'edit' | 'create';

type Props = {
  value: RevisarDatosOperacionComprador[];
  idExpediente: number;
  idRevisarDatosOperacion: number;
  disabled: boolean;
  controles?: ControlesComprador;
  loadingControles?: boolean;
  onChange: (rows: RevisarDatosOperacionComprador[]) => void;
  onWarn?: (message: string) => void;
};

const parseDate = (value?: string | null): Date | null => {
  if (!value) return null;
  const d = new Date(value);
  return isNaN(d.getTime()) ? null : d;
};

const toIso = (value: Date | null | undefined): string | null =>
  value ? value.toISOString() : null;

const numberToCatalogCode = (value?: number | null) => {
  if (value === null || value === undefined) return null;
  return String(value).padStart(3, '0');
};

const catalogCodeToNumber = (value?: string | null) => {
  if (!value) return null;
  const n = Number(value);
  return isNaN(n) ? null : n;
};

const isEmptyValue = (v: unknown): boolean =>
  v === null || v === undefined || (typeof v === 'string' && v.trim() === '');

const normalizeCatalogCode = (v: unknown): string =>
  v === null || v === undefined ? '' : String(v).trim();

const getCatalogDescription = (options: CatalogoOption[], value: unknown): string => {
  const normalized = normalizeCatalogCode(value);
  if (!normalized) return '';
  const opt = options.find((item) => {
    const code = normalizeCatalogCode(item.code);
    return code === normalized || code.replace(/^0+/, '') === normalized.replace(/^0+/, '');
  });
  return opt?.description ?? opt?.code ?? normalized;
};

const isJuridica = (row: RevisarDatosOperacionComprador): boolean => {
  const tipo = String(row.tipo_persona ?? '').toLowerCase();
  return tipo.includes('jur') || (!!String(row.razon_social ?? '').trim() && !String(row.nombres ?? '').trim());
};

const fullName = (row: RevisarDatosOperacionComprador): string =>
  [row.nombres, row.apellido_paterno, row.apellido_materno]
    .filter(Boolean)
    .join(' ')
    .trim() || row.razon_social || row.rut || 'Registro';

const labelByField: Record<string, string> = {
  rut: 'RUT',
  razon_social: 'Razón Social',
  nombres: 'Nombres',
  apellido_paterno: 'Apellido Paterno',
  apellido_materno: 'Apellido Materno',
  direccion: 'Dirección',
  region: 'Región',
  comuna: 'Comuna',
  direccion_env_esc: 'Dirección Envío Escritura',
  region_env_esc: 'Región Envío Escritura',
  comuna_env_esc: 'Comuna Envío Escritura',
  telefono: 'Teléfono',
  telefono_comercial: 'Teléfono Comercial',
  telefono_movil: 'Teléfono Móvil',
  profesion: 'Profesión',
  email: 'Email',
  email2: 'Email 2',
  relacion_titular: 'Relación Titular',
};

type RequiredField = keyof RevisarDatosOperacionComprador;

const getRequiredFields = (row: RevisarDatosOperacionComprador): RequiredField[] => {
  const juridica = isJuridica(row);
  const base: RequiredField[] = [
    'rut',
    juridica ? 'razon_social' : 'nombres',
    ...(!juridica ? (['apellido_paterno', 'apellido_materno'] as RequiredField[]) : []),
    'direccion',
    'region',
    'comuna',
    'telefono',
    'email',
    'relacion_titular',
  ];
  return juridica
    ? [...base, 'direccion_env_esc', 'region_env_esc', 'comuna_env_esc']
    : [...base, 'telefono_comercial', 'telefono_movil', 'profesion', 'email2'];
};

const getMissingField = (row: RevisarDatosOperacionComprador): string | null => {
  for (const field of getRequiredFields(row)) {
    if (isEmptyValue(row[field])) return labelByField[field] ?? String(field);
  }
  return null;
};

export default function CompradorSection({
  value,
  idExpediente,
  idRevisarDatosOperacion,
  disabled,
  controles = EMPTY_CONTROLES_COMPRADOR,
  onChange,
  onWarn,
}: Props) {
  const [dialogVisible, setDialogVisible] = useState(false);
  const [dialogMode, setDialogMode] = useState<DialogMode>('view');
  const [selected, setSelected] = useState<RevisarDatosOperacionComprador | null>(null);

  const canEdit = !disabled;

  const rows = useMemo(
    () => (value ?? []).filter((item) => item.row_status !== false),
    [value],
  );

  const tipoPersonaOptions = controles.tipo_comprador ?? [];
  const generoOptions = controles.genero;
  const estadoCivilOptions = controles.estado_civil;
  const nacionalidadOptions = controles.nacionalidad;
  const relacionTitularOptions = controles.relacion_titular;
  const tipoDireccionDividendoOptions = controles.tipo_direccion_dividendo ?? [];
  const regionOptions = controles.region;
  const comunaOptions = controles.comuna;

  const openCreate = () => {
    if (!canEdit) return;
    setSelected(buildCompradorEmpty(idExpediente, idRevisarDatosOperacion));
    setDialogMode('create');
    setDialogVisible(true);
  };

  const openView = (row: RevisarDatosOperacionComprador) => {
    setSelected({ ...row });
    setDialogMode('view');
    setDialogVisible(true);
  };

  const openEdit = (row: RevisarDatosOperacionComprador) => {
    if (!canEdit) return;
    setSelected({ ...row });
    setDialogMode('edit');
    setDialogVisible(true);
  };

  const closeDialog = () => {
    setDialogVisible(false);
    setSelected(null);
  };

  const updateField = <K extends keyof RevisarDatosOperacionComprador>(
    field: K,
    fieldValue: RevisarDatosOperacionComprador[K],
  ) => {
    if (!selected) return;
    setSelected({ ...selected, [field]: fieldValue });
  };

  const saveRow = () => {
    if (!selected) return;
    const missing = getMissingField(selected);
    if (missing) {
      onWarn?.(`Debe completar el campo ${missing}.`);
      return;
    }

    const normalized: RevisarDatosOperacionComprador = {
      ...selected,
      id_revisar_datos_operacion: idRevisarDatosOperacion,
      id_expediente: idExpediente,
      is_active: true,
      row_status: true,
    };

    const selectedId = selected.id_revisar_datos_operacion_comprador;

    if (dialogMode === 'create' || selectedId <= 0) {
      const localId = selectedId < 0 ? selectedId : -Date.now();
      onChange([...value, { ...normalized, id_revisar_datos_operacion_comprador: localId }]);
      closeDialog();
      return;
    }

    onChange(
      value.map((item) =>
        item.id_revisar_datos_operacion_comprador === selectedId ? normalized : item,
      ),
    );
    closeDialog();
  };

  const deleteRow = (row: RevisarDatosOperacionComprador) => {
    const rowId = row.id_revisar_datos_operacion_comprador;

    if (rowId <= 0) {
      onChange(value.filter((item) => item.id_revisar_datos_operacion_comprador !== rowId));
      return;
    }

    onChange(
      value.map((item) =>
        item.id_revisar_datos_operacion_comprador === rowId
          ? { ...item, is_active: false, row_status: false }
          : item,
      ),
    );
  };

  const confirmDelete = (row: RevisarDatosOperacionComprador) => {
    if (!canEdit) return;
    confirmDialog({
      message: `¿Está seguro de querer eliminar ${fullName(row)}?`,
      header: 'Confirmar eliminación',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Sí, eliminar',
      rejectLabel: 'Cancelar',
      acceptClassName: 'p-button-danger',
      accept: () => deleteRow(row),
    });
  };

  const actionsTemplate = (row: RevisarDatosOperacionComprador) => (
    <div className="flex align-items-center justify-content-center gap-2">
      <Button
        type="button"
        icon="pi pi-eye"
        rounded
        text
        severity="info"
        tooltip="Ver detalle"
        onClick={() => openView(row)}
      />
      <Button
        type="button"
        icon="pi pi-pencil"
        rounded
        text
        severity="warning"
        tooltip="Editar"
        disabled={!canEdit}
        onClick={() => openEdit(row)}
      />
      <Button
        type="button"
        icon="pi pi-trash"
        rounded
        text
        severity="danger"
        tooltip="Eliminar"
        disabled={!canEdit}
        onClick={() => confirmDelete(row)}
      />
    </div>
  );

  const readOnlyDialog = dialogMode === 'view' || !canEdit;
  const controlDisabled = readOnlyDialog;

  const dialogFooter = (
    <div className="flex justify-end gap-2">
      <Button
        type="button"
        label={dialogMode === 'view' ? 'Cerrar' : 'Cancelar'}
        icon="pi pi-times"
        severity="secondary"
        outlined
        onClick={closeDialog}
      />
      {dialogMode !== 'view' && (
        <Button
          type="button"
          label="Aceptar"
          icon="pi pi-check"
          severity="success"
          onClick={saveRow}
        />
      )}
    </div>
  );

  return (
    <Card title="Datos del Comprador" className="shadow-sm border border-gray-200 rounded-xl">
      <ConfirmDialog />

      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-3 mb-3">
        <span className="text-sm text-gray-500">Registros asociados al expediente.</span>
        <Button
          type="button"
          label="Agregar"
          icon="pi pi-plus"
          severity="success"
          outlined
          disabled={!canEdit}
          onClick={openCreate}
          className="btn-responsive"
        />
      </div>

      <DataTable
        value={rows}
        emptyMessage="No existen registros."
        paginator={rows.length > 5}
        rows={5}
        responsiveLayout="scroll"
        stripedRows
        rowHover
        className="datatable-presto"
      >
        <Column field="rut" header="RUT" />
        <Column
          header="Tipo Persona"
          body={(row) => getCatalogDescription(tipoPersonaOptions, row.tipo_persona)}
        />
        <Column field="razon_social" header="Razón Social" />
        <Column header="Nombre" body={(row) => fullName(row)} />
        <Column
          header="Relación Titular"
          body={(row) => getCatalogDescription(relacionTitularOptions, row.relacion_titular)}
        />
        <Column
          header="Acciones"
          body={actionsTemplate}
          style={{ width: '11rem', textAlign: 'center' }}
        />
      </DataTable>

      <Dialog
        header={`${dialogMode === 'view' ? 'Detalle' : dialogMode === 'edit' ? 'Editar' : 'Nuevo'} Comprador`}
        visible={dialogVisible}
        modal
        className="w-full md:w-10/12 lg:w-8/12"
        footer={dialogFooter}
        onHide={closeDialog}
      >
        {selected && (
          <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">RUT *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.rut ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('rut', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Tipo Persona</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.tipo_persona ?? null}
                options={tipoPersonaOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('tipo_persona', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Razón Social *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.razon_social ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('razon_social', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Nombres *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.nombres ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('nombres', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Apellido Paterno *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.apellido_paterno ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('apellido_paterno', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Apellido Materno *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.apellido_materno ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('apellido_materno', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Fecha Nacimiento</label>
              <Calendar
                className="form-dropdown-presto w-full"
                value={parseDate(selected.fecha_nacimiento)}
                disabled={controlDisabled}
                showIcon
                dateFormat="dd/mm/yy"
                onChange={(e) => updateField('fecha_nacimiento', toIso(e.value as Date | null))}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Género</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.genero ?? null}
                options={generoOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('genero', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Estado Civil</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.estado_civil ?? null}
                options={estadoCivilOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('estado_civil', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Nacionalidad</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.nacionalidad ?? null}
                options={nacionalidadOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                filter
                onChange={(e) => updateField('nacionalidad', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Profesión *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.profesion ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('profesion', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Relación Titular *</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.relacion_titular ?? null}
                options={relacionTitularOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('relacion_titular', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Tipo Dirección Dividendo</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={numberToCatalogCode(selected.tipo_dir_dividendo)}
                options={tipoDireccionDividendoOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('tipo_dir_dividendo', catalogCodeToNumber(e.value))}
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Dirección *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.direccion ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('direccion', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Región *</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.region ?? null}
                options={regionOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                filter
                onChange={(e) => updateField('region', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Comuna *</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.comuna ?? null}
                options={comunaOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                filter
                onChange={(e) => updateField('comuna', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Teléfono *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.telefono ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('telefono', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Teléfono Comercial *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.telefono_comercial ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('telefono_comercial', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Teléfono Móvil *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.telefono_movil ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('telefono_movil', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Email *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.email ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('email', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Email 2 *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.email2 ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('email2', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Dirección Envío Escritura *</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.direccion_env_esc ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('direccion_env_esc', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Región Envío Escritura *</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.region_env_esc ?? null}
                options={regionOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                filter
                onChange={(e) => updateField('region_env_esc', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Comuna Envío Escritura *</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.comuna_env_esc ?? null}
                options={comunaOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                filter
                onChange={(e) => updateField('comuna_env_esc', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Dirección Envío Dividendo</label>
              <InputText
                className="form-dropdown-presto w-full"
                value={selected.direccion_env_div ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('direccion_env_div', e.target.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Región Envío Dividendo</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.region_env_div ?? null}
                options={regionOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                filter
                onChange={(e) => updateField('region_env_div', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Comuna Envío Dividendo</label>
              <Dropdown
                className="form-dropdown-presto w-full"
                value={selected.comuna_env_div ?? null}
                options={comunaOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                filter
                onChange={(e) => updateField('comuna_env_div', e.value)}
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Observaciones</label>
              <InputTextarea
                value={selected.observaciones ?? ''}
                disabled={controlDisabled}
                className="form-input-presto w-full"
                rows={3}
                autoResize
                placeholder="Ingrese observaciones"
                onChange={(e) => updateField('observaciones', e.target.value)}
              />
            </div>
          </div>
        )}
      </Dialog>
    </Card>
  );
}
