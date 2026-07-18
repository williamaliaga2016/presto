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

import {
  buildFiadorGaranteEmpty,
  type RevisarDatosOperacionFiadorGarante,
} from '../models/revisar_datos_operacion';
import {
  EMPTY_CONTROLES_FIADOR_GARANTE,
  type CatalogoOption,
  type ControlesFiadorGarante,
} from '../models/catalogo';

type DialogMode = 'view' | 'edit' | 'create';

type Props = {
  value: RevisarDatosOperacionFiadorGarante[];
  idExpediente: number;
  idRevisarDatosOperacion: number;
  disabled: boolean;
  canEdit: boolean;
  controles?: ControlesFiadorGarante;
  loadingControles?: boolean;
  onChange: (rows: RevisarDatosOperacionFiadorGarante[]) => void;
  onWarn?: (message: string) => void;
};

const parseDate = (value?: string | null): Date | null => {
  if (!value) return null;
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? null : date;
};

const toIso = (value: Date | null | undefined): string | null =>
  value ? value.toISOString() : null;

const getName = (row: RevisarDatosOperacionFiadorGarante) =>
  [row.nombres, row.apellido_paterno, row.apellido_materno]
    .filter(Boolean)
    .join(' ')
    .trim() || row.rut || 'Registro';

const isEmptyValue = (value: unknown): boolean =>
  value === null || value === undefined || (typeof value === 'string' && value.trim() === '');

const normalizeCatalogCode = (value: unknown): string =>
  value === null || value === undefined ? '' : String(value).trim();

const getCatalogDescription = (options: CatalogoOption[], value: unknown): string => {
  const normalizedValue = normalizeCatalogCode(value);
  if (!normalizedValue) return '';

  const option = options.find((item) => {
    const normalizedCode = normalizeCatalogCode(item.code);
    return (
      normalizedCode === normalizedValue ||
      normalizedCode.replace(/^0+/, '') === normalizedValue.replace(/^0+/, '')
    );
  });

  return option?.description ?? option?.code ?? normalizedValue;
};

const requiredFiadorFields: { field: keyof RevisarDatosOperacionFiadorGarante; label: string }[] = [
  { field: 'rut', label: 'RUT' },
  { field: 'nombres', label: 'Nombres' },
  { field: 'apellido_paterno', label: 'Apellido Paterno' },
  { field: 'apellido_materno', label: 'Apellido Materno' },
  { field: 'fecha_nacimiento', label: 'Fecha Nacimiento' },
  { field: 'genero', label: 'Género' },
  { field: 'estado_civil', label: 'Estado Civil' },
  { field: 'nacionalidad', label: 'Nacionalidad' },
  { field: 'profesion', label: 'Profesión' },
  { field: 'relacion_titular', label: 'Relación Titular' },
  { field: 'telefono_fijo', label: 'Teléfono Fijo' },
  { field: 'telefono_movil', label: 'Teléfono Móvil' },
  { field: 'email', label: 'Email' },
  { field: 'direccion', label: 'Dirección' },
  { field: 'region', label: 'Región' },
  { field: 'comuna', label: 'Comuna' },
  { field: 'observaciones', label: 'Observaciones' },
];

const getMissingFiadorField = (row: RevisarDatosOperacionFiadorGarante): string | null => {
  const missing = requiredFiadorFields.find(({ field }) => isEmptyValue(row[field]));
  return missing?.label ?? null;
};

export default function FiadorGaranteSection({
  value,
  idExpediente,
  idRevisarDatosOperacion,
  disabled,
  canEdit,
  controles = EMPTY_CONTROLES_FIADOR_GARANTE,
  loadingControles = false,
  onChange,
  onWarn,
}: Props) {
  const generoOptions = controles.genero;
  const estadoCivilOptions = controles.estado_civil;
  const nacionalidadOptions = controles.nacionalidad;
  const relacionTitularOptions = controles.relacion_titular;
  const regionOptions = controles.region;
  const comunaOptions = controles.comuna;

  const [dialogVisible, setDialogVisible] = useState(false);
  const [dialogMode, setDialogMode] = useState<DialogMode>('view');
  const [selected, setSelected] = useState<RevisarDatosOperacionFiadorGarante | null>(null);

  const canEditActions = canEdit && !disabled;
  const rows = useMemo(() => value?.filter((item) => item.row_status !== false) ?? [], [value]);

  const openCreate = () => {
    if (!canEditActions) return;
    setSelected(buildFiadorGaranteEmpty(idExpediente, idRevisarDatosOperacion));
    setDialogMode('create');
    setDialogVisible(true);
  };

  const openView = (row: RevisarDatosOperacionFiadorGarante) => {
    setSelected({ ...row });
    setDialogMode('view');
    setDialogVisible(true);
  };

  const openEdit = (row: RevisarDatosOperacionFiadorGarante) => {
    if (!canEditActions) return;
    setSelected({ ...row });
    setDialogMode('edit');
    setDialogVisible(true);
  };

  const closeDialog = () => {
    setDialogVisible(false);
    setSelected(null);
  };

  const updateField = <K extends keyof RevisarDatosOperacionFiadorGarante>(
    field: K,
    fieldValue: RevisarDatosOperacionFiadorGarante[K],
  ) => {
    if (!selected) return;
    setSelected({ ...selected, [field]: fieldValue });
  };

  const saveRow = () => {
    if (!selected) return;

    const missingField = getMissingFiadorField(selected);
    if (missingField) {
      onWarn?.(`Debe completar el campo ${missingField}.`);
      return;
    }

    const normalized: RevisarDatosOperacionFiadorGarante = {
      ...selected,
      id_revisar_datos_operacion: idRevisarDatosOperacion,
      id_expediente: idExpediente,
      is_active: true,
      row_status: true,
    };

    if (dialogMode === 'create' || normalized.id_revisar_datos_operacion_fiador_garante <= 0) {
      onChange([
        ...value,
        {
          ...normalized,
          id_revisar_datos_operacion_fiador_garante:
            normalized.id_revisar_datos_operacion_fiador_garante < 0
              ? normalized.id_revisar_datos_operacion_fiador_garante
              : -Date.now(),
        },
      ]);
      closeDialog();
      return;
    }

    onChange(
      value.map((item) =>
        item.id_revisar_datos_operacion_fiador_garante ===
        normalized.id_revisar_datos_operacion_fiador_garante
          ? normalized
          : item,
      ),
    );
    closeDialog();
  };

  const deleteRow = (row: RevisarDatosOperacionFiadorGarante) => {
    const id = row.id_revisar_datos_operacion_fiador_garante;

    if (id <= 0) {
      onChange(value.filter((item) => item.id_revisar_datos_operacion_fiador_garante !== id));
      return;
    }

    onChange(
      value.map((item) =>
        item.id_revisar_datos_operacion_fiador_garante === id
          ? { ...item, is_active: false, row_status: false }
          : item,
      ),
    );
  };

  const confirmDelete = (row: RevisarDatosOperacionFiadorGarante) => {
    if (!canEditActions) return;

    confirmDialog({
      message: `¿Está seguro de querer eliminar ${getName(row)}?`,
      header: 'Confirmar eliminación',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Sí, eliminar',
      rejectLabel: 'Cancelar',
      acceptClassName: 'p-button-danger',
      accept: () => deleteRow(row),
    });
  };

  const actionsTemplate = (row: RevisarDatosOperacionFiadorGarante) => (
    <div className="flex align-items-center justify-content-center gap-2">
      <Button type="button" icon="pi pi-eye" rounded text severity="info" onClick={() => openView(row)} />
      <Button type="button" icon="pi pi-pencil" rounded text severity="warning" disabled={!canEditActions} onClick={() => openEdit(row)} />
      <Button type="button" icon="pi pi-trash" rounded text severity="danger" disabled={!canEditActions} onClick={() => confirmDelete(row)} />
    </div>
  );

  const readOnlyDialog = dialogMode === 'view' || !canEditActions;
  const controlDisabled = readOnlyDialog || loadingControles;

  const footer = (
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
    <Card title="Datos del Fiador / Garante" className="shadow-sm border border-gray-200 rounded-xl">
      <ConfirmDialog />
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-3 mb-3">
        <span className="text-sm text-gray-500">Registros asociados al expediente.</span>
        <Button
          type="button"
          label="Agregar"
          icon="pi pi-plus"
          severity="success"
          outlined
          disabled={!canEditActions}
          onClick={openCreate}
          className="btn-responsive"
        />
      </div>

      <DataTable
        key={`fiador-${canEditActions ? 'edit' : 'view'}`}
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
        <Column header="Nombre" body={(row) => getName(row)} />
        <Column
          header="Relación Titular"
          body={(row) => getCatalogDescription(relacionTitularOptions, row.relacion_titular)}
        />
        <Column field="telefono_movil" header="Teléfono Móvil" />
        <Column field="email" header="Email" />
        <Column
          key={`acciones-fiador-${canEditActions ? 'edit' : 'view'}`}
          header="Acciones"
          body={actionsTemplate}
          style={{ width: '11rem', textAlign: 'center' }}
        />
      </DataTable>

      <Dialog
        header={`${dialogMode === 'view' ? 'Detalle' : dialogMode === 'edit' ? 'Editar' : 'Nuevo'} Fiador / Garante`}
        visible={dialogVisible}
        modal
        className="w-full md:w-10/12 lg:w-8/12"
        footer={footer}
        onHide={closeDialog}
      >
        {selected && (
          <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">RUT *</label>
              <InputText className="form-input-presto w-full" value={selected.rut ?? ''} disabled={controlDisabled} onChange={(e) => updateField('rut', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Nombres *</label>
              <InputText className="form-input-presto w-full" value={selected.nombres ?? ''} disabled={controlDisabled} onChange={(e) => updateField('nombres', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Apellido Paterno *</label>
              <InputText className="form-input-presto w-full" value={selected.apellido_paterno ?? ''} disabled={controlDisabled} onChange={(e) => updateField('apellido_paterno', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Apellido Materno *</label>
              <InputText className="form-input-presto w-full" value={selected.apellido_materno ?? ''} disabled={controlDisabled} onChange={(e) => updateField('apellido_materno', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Fecha Nacimiento *</label>
              <Calendar className="form-input-presto w-full" value={parseDate(selected.fecha_nacimiento)} disabled={controlDisabled} showIcon dateFormat="dd/mm/yy" onChange={(e) => updateField('fecha_nacimiento', toIso(e.value as Date | null))} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Género *</label>
              <Dropdown className="form-dropdown-presto w-full" value={selected.genero ?? null} options={generoOptions} optionLabel="description" optionValue="code" disabled={controlDisabled} placeholder="Seleccione" onChange={(e) => updateField('genero', e.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Estado Civil *</label>
              <Dropdown className="form-dropdown-presto w-full" value={selected.estado_civil ?? null} options={estadoCivilOptions} optionLabel="description" optionValue="code" disabled={controlDisabled} placeholder="Seleccione" onChange={(e) => updateField('estado_civil', e.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Nacionalidad *</label>
              <Dropdown className="form-dropdown-presto w-full" value={selected.nacionalidad ?? null} options={nacionalidadOptions} optionLabel="description" optionValue="code" disabled={controlDisabled} placeholder="Seleccione" onChange={(e) => updateField('nacionalidad', e.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Profesión *</label>
              <InputText className="form-input-presto w-full" value={selected.profesion ?? ''} disabled={controlDisabled} onChange={(e) => updateField('profesion', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Relación Titular *</label>
              <Dropdown className="form-dropdown-presto w-full" value={selected.relacion_titular ?? null} options={relacionTitularOptions} optionLabel="description" optionValue="code" disabled={controlDisabled} placeholder="Seleccione" onChange={(e) => updateField('relacion_titular', e.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Teléfono Fijo *</label>
              <InputText className="form-input-presto w-full" value={selected.telefono_fijo ?? ''} disabled={controlDisabled} onChange={(e) => updateField('telefono_fijo', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Teléfono Móvil *</label>
              <InputText className="form-input-presto w-full" value={selected.telefono_movil ?? ''} disabled={controlDisabled} onChange={(e) => updateField('telefono_movil', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Email *</label>
              <InputText className="form-input-presto w-full" value={selected.email ?? ''} disabled={controlDisabled} onChange={(e) => updateField('email', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Dirección *</label>
              <InputText className="form-input-presto w-full" value={selected.direccion ?? ''} disabled={controlDisabled} onChange={(e) => updateField('direccion', e.target.value)} />
            </div>
            <div className="flex flex-col gap-1 md:col-span-1 xl:col-span-1">
              <label className="font-semibold text-sm">Región *</label>
              <Dropdown className="form-dropdown-presto w-full" value={selected.region ?? null} options={regionOptions} optionLabel="description" optionValue="code" disabled={controlDisabled} placeholder="Seleccione" onChange={(e) => updateField('region', e.value)} />
            </div>
            <div className="flex flex-col gap-1 md:col-span-1 xl:col-span-1">
              <label className="font-semibold text-sm">Comuna *</label>
              <Dropdown className="form-dropdown-presto w-full" value={selected.comuna ?? null} options={comunaOptions} optionLabel="description" optionValue="code" disabled={controlDisabled} placeholder="Seleccione" onChange={(e) => updateField('comuna', e.value)} />
            </div>
            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Observaciones *</label>
              <InputTextarea className="form-input-presto w-full" value={selected.observaciones ?? ''} rows={3} autoResize disabled={controlDisabled} onChange={(e) => updateField('observaciones', e.target.value)} />
            </div>
          </div>
        )}
      </Dialog>
    </Card>
  );
}
