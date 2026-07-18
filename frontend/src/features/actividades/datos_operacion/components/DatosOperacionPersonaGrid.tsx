import { useMemo, useState } from 'react';
import { Button } from 'primereact/button';
import { Calendar } from 'primereact/calendar';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputNumber } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';

import type {
  DatosOperacionComprador,
  DatosOperacionVendedor,
} from '../models/datos_operacion';
import {
  buildCompradorEmpty,
  buildVendedorEmpty,
} from '../models/datos_operacion';
import {
  EMPTY_CONTROLES_COMPRADOR,
  EMPTY_CONTROLES_VENDEDOR,
  type CatalogoOption,
  type ControlesComprador,
  type ControlesVendedor,
} from '../models/catalogo';

type PersonaRow = DatosOperacionComprador | DatosOperacionVendedor;

type PersonaRequiredField =
  | 'rut'
  | 'razon_social'
  | 'nombres'
  | 'apellido_paterno'
  | 'apellido_materno'
  | 'direccion'
  | 'region'
  | 'comuna'
  | 'direccion_env_esc'
  | 'region_env_esc'
  | 'comuna_env_esc'
  | 'tipo_dir_dividendo'
  | 'direccion_env_div'
  | 'region_env_div'
  | 'comuna_env_div'
  | 'telefono'
  | 'telefono_comercial'
  | 'telefono_movil'
  | 'profesion'
  | 'email'
  | 'email2'
  | 'relacion_titular';


type DialogMode = 'view' | 'edit' | 'create';

type Props<T extends PersonaRow> = {
  title: string;
  type: 'comprador' | 'vendedor';
  value: T[];
  idExpediente: number;
  idDatosOperacion: number;
  disabled: boolean;
  canEdit: boolean;
  controles?: ControlesComprador | ControlesVendedor;
  loadingControles?: boolean;
  onChange: (rows: T[]) => void;
  onWarn?: (message: string) => void;
};

const parseDate = (value?: string | null): Date | null => {
  if (!value) return null;
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? null : date;
};

const toIso = (value: Date | null | undefined): string | null => {
  if (!value) return null;
  return value.toISOString();
};

const numberToCatalogCode = (value?: number | null) => {
  if (value === null || value === undefined) return null;
  return String(value).padStart(3, '0');
};

const catalogCodeToNumber = (value?: string | null) => {
  if (!value) return null;
  const numberValue = Number(value);
  return Number.isNaN(numberValue) ? null : numberValue;
};

const getRowId = (row: PersonaRow): number => {
  if ('id_datos_operacion_comprador' in row) return row.id_datos_operacion_comprador;
  return row.id_datos_operacion_vendedor;
};

const setRowId = <T extends PersonaRow>(row: T, id: number): T => {
  if ('id_datos_operacion_comprador' in row) {
    return { ...row, id_datos_operacion_comprador: id } as T;
  }

  return { ...row, id_datos_operacion_vendedor: id } as T;
};

const fullName = (row: PersonaRow) =>
  [row.nombres, row.apellido_paterno, row.apellido_materno]
    .filter(Boolean)
    .join(' ')
    .trim() || row.razon_social || row.rut || 'Registro';

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

const isJuridica = (row: PersonaRow): boolean => {
  const tipoPersona = String(row.tipo_persona ?? '').toLowerCase();
  const razonSocial = String(row.razon_social ?? '').trim();
  const nombres = String(row.nombres ?? '').trim();

  return tipoPersona.includes('jur') || (!!razonSocial && !nombres);
};

const labelTextByField: Record<string, string> = {
  rut: 'RUT',
  tipo_persona: 'Tipo Persona',
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
  tipo_dir_dividendo: 'Tipo Dirección Dividendo',
  direccion_env_div: 'Dirección Envío Dividendo',
  region_env_div: 'Región Envío Dividendo',
  comuna_env_div: 'Comuna Envío Dividendo',
  telefono: 'Teléfono',
  telefono_comercial: 'Teléfono Comercial',
  telefono_movil: 'Teléfono Móvil',
  profesion: 'Profesión',
  email: 'Email',
  email2: 'Email 2',
  relacion_titular: 'Relación Titular',
};

const getRequiredPersonaFields = (
  type: 'comprador' | 'vendedor',
  row: PersonaRow,
): PersonaRequiredField[] => {
  const juridica = isJuridica(row);

  if (type === 'comprador') {
    const baseFields: PersonaRequiredField[] = [
      'rut',
      juridica ? 'razon_social' : 'nombres',
      ...(juridica ? [] : (['apellido_paterno', 'apellido_materno'] as PersonaRequiredField[])),
      'direccion',
      'region',
      'comuna',
      'direccion_env_esc',
      'region_env_esc',
      'comuna_env_esc',
      'tipo_dir_dividendo',
      'direccion_env_div',
      'region_env_div',
      'comuna_env_div',
      'telefono',
      'email',
      'relacion_titular',
    ];

    return juridica
      ? baseFields
      : [
          ...baseFields,
          'telefono_comercial',
          'telefono_movil',
          'profesion',
          'email2',
        ];
  }

  const baseFields: PersonaRequiredField[] = [
    'rut',
    juridica ? 'razon_social' : 'nombres',
    ...(juridica ? [] : (['apellido_paterno', 'apellido_materno'] as PersonaRequiredField[])),
    'direccion',
    'region',
    'comuna',
    'telefono',
    'email',
    'relacion_titular',
  ];

  return juridica
    ? [
        ...baseFields,
        'direccion_env_esc',
        'region_env_esc',
        'comuna_env_esc',
      ]
    : [
        ...baseFields,
        'telefono_comercial',
        'telefono_movil',
        'profesion',
        'email2',
      ];
};

const getMissingPersonaField = (
  type: 'comprador' | 'vendedor',
  row: PersonaRow,
): string | null => {
  const requiredFields = getRequiredPersonaFields(type, row);

  for (const field of requiredFields) {
    if (isEmptyValue(row[field])) {
      return labelTextByField[String(field)] ?? String(field);
    }
  }

  return null;
};

export default function DatosOperacionPersonaGrid<T extends PersonaRow>({
  title,
  type,
  value,
  idExpediente,
  idDatosOperacion,
  disabled,
  canEdit,
  controles,
  loadingControles = false,
  onChange,
  onWarn,
}: Props<T>) {
  const resolvedControles =
    controles ??
    (type === 'vendedor' ? EMPTY_CONTROLES_VENDEDOR : EMPTY_CONTROLES_COMPRADOR);

  const tipoPersonaOptions = (resolvedControles as ControlesVendedor).tipo_vendedor ?? [];
  const generoOptions = resolvedControles.genero;
  const estadoCivilOptions = resolvedControles.estado_civil;
  const nacionalidadOptions = resolvedControles.nacionalidad;
  const relacionTitularOptions = resolvedControles.relacion_titular;
  const tipoDireccionDividendoOptions =
    'tipo_direccion_dividendo' in resolvedControles
      ? resolvedControles.tipo_direccion_dividendo
      : [];
  const regionOptions = resolvedControles.region;
  const comunaOptions = resolvedControles.comuna;
  const [dialogVisible, setDialogVisible] = useState(false);
  const [dialogMode, setDialogMode] = useState<DialogMode>('view');
  const [selected, setSelected] = useState<T | null>(null);

  const canEditActions = canEdit && !disabled;

  const rows = useMemo(() => value?.filter((item) => item.row_status !== false) ?? [], [value]);

  const buildEmpty = (): T => {
    if (type === 'comprador') {
      return buildCompradorEmpty(idExpediente, idDatosOperacion) as T;
    }

    return buildVendedorEmpty(idExpediente, idDatosOperacion) as T;
  };

  const openCreate = () => {
    if (!canEditActions) return;
    setSelected(buildEmpty());
    setDialogMode('create');
    setDialogVisible(true);
  };

  const openView = (row: T) => {
    setSelected({ ...row });
    setDialogMode('view');
    setDialogVisible(true);
  };

  const openEdit = (row: T) => {
    if (!canEditActions) return;
    setSelected({ ...row });
    setDialogMode('edit');
    setDialogVisible(true);
  };

  const closeDialog = () => {
    setDialogVisible(false);
    setSelected(null);
  };

  const updateField = <K extends keyof T>(field: K, fieldValue: T[K]) => {
    if (!selected) return;
    setSelected({ ...selected, [field]: fieldValue });
  };

  const saveRow = () => {
    if (!selected) return;

    const missingField = getMissingPersonaField(type, selected);
    if (missingField) {
      onWarn?.(`Debe completar el campo ${missingField}.`);
      return;
    }

    const selectedId = getRowId(selected);
    const normalized = {
      ...selected,
      id_datos_operacion: idDatosOperacion,
      id_expediente: idExpediente,
      is_active: true,
      row_status: true,
    } as T;

    if (dialogMode === 'create' || selectedId <= 0) {
      const localId = selectedId < 0 ? selectedId : -Date.now();
      onChange([...value, setRowId(normalized, localId)]);
      closeDialog();
      return;
    }

    onChange(value.map((item) => (getRowId(item) === selectedId ? normalized : item)));
    closeDialog();
  };

  const deleteRow = (row: T) => {
    const rowId = getRowId(row);

    if (rowId <= 0) {
      onChange(value.filter((item) => getRowId(item) !== rowId));
      return;
    }

    onChange(
      value.map((item) =>
        getRowId(item) === rowId
          ? {
              ...item,
              is_active: false,
              row_status: false,
            }
          : item,
      ),
    );
  };

  const confirmDelete = (row: T) => {
    if (!canEditActions) return;

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

  const actionsTemplate = (row: T) => (
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
        disabled={!canEditActions}
        onClick={() => openEdit(row)}
      />

      <Button
        type="button"
        icon="pi pi-trash"
        rounded
        text
        severity="danger"
        tooltip="Eliminar"
        disabled={!canEditActions}
        onClick={() => confirmDelete(row)}
      />
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
    <Card title={title} className="shadow-sm border border-gray-200 rounded-xl">
      <ConfirmDialog />

      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-3 mb-3">
        <span className="text-sm text-gray-500">
          Registros asociados al expediente.
        </span>
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
        key={`${type}-${canEditActions ? 'edit' : 'view'}`}
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
        <Column header="Tipo Persona" body={(row) => getCatalogDescription(tipoPersonaOptions, row.tipo_persona)} />
        <Column field="razon_social" header="Razón Social" />
        <Column header="Nombre" body={(row) => fullName(row)} />
        <Column header="Relación Titular" body={(row) => getCatalogDescription(relacionTitularOptions, row.relacion_titular)} />
        <Column
          key={`acciones-${type}-${canEditActions ? 'edit' : 'view'}`}
          header="Acciones"
          body={actionsTemplate}
          style={{ width: '11rem', textAlign: 'center' }}
        />
      </DataTable>

      <Dialog
        header={`${dialogMode === 'view' ? 'Detalle' : dialogMode === 'edit' ? 'Editar' : 'Nuevo'} ${title}`}
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
              <InputText className="form-dropdown-presto w-full"
                value={selected.rut ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('rut' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Tipo Persona</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.tipo_persona ?? null}
                options={tipoPersonaOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('tipo_persona' as keyof T, e.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-1 xl:col-span-1">
              <label className="font-semibold text-sm">Razón Social *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.razon_social ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('razon_social' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Nombres *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.nombres ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('nombres' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Apellido Paterno *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.apellido_paterno ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('apellido_paterno' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Apellido Materno *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.apellido_materno ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('apellido_materno' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Fecha Nacimiento</label>
              <Calendar className="form-dropdown-presto w-full"
                value={parseDate(selected.fecha_nacimiento)}
                disabled={controlDisabled}
                showIcon
                dateFormat="dd/mm/yy"
                onChange={(e) =>
                  updateField('fecha_nacimiento' as keyof T, toIso(e.value as Date | null) as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Género</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.genero ?? null}
                options={generoOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('genero' as keyof T, e.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Estado Civil</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.estado_civil ?? null}
                options={estadoCivilOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('estado_civil' as keyof T, e.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Nacionalidad</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.nacionalidad ?? null}
                options={nacionalidadOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('nacionalidad' as keyof T, e.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Profesión *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.profesion ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('profesion' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Relación Titular *</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.relacion_titular ?? null}
                options={relacionTitularOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('relacion_titular' as keyof T, e.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Tipo Dirección Dividendo *</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={numberToCatalogCode(selected.tipo_dir_dividendo)}
                options={tipoDireccionDividendoOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) =>
                  updateField('tipo_dir_dividendo' as keyof T, catalogCodeToNumber(e.value) as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Dirección *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.direccion ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('direccion' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Región *</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.region ?? null}
                options={regionOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('region' as keyof T, e.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Comuna *</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.comuna ?? null}
                options={comunaOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) => updateField('comuna' as keyof T, e.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Teléfono *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.telefono ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('telefono' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Teléfono Comercial *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.telefono_comercial ?? ''}
                disabled={controlDisabled}
                onChange={(e) =>
                  updateField('telefono_comercial' as keyof T, e.target.value as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Teléfono Móvil *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.telefono_movil ?? ''}
                disabled={controlDisabled}
                onChange={(e) =>
                  updateField('telefono_movil' as keyof T, e.target.value as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Email *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.email ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('email' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1">
              <label className="font-semibold text-sm">Email 2 *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.email2 ?? ''}
                disabled={controlDisabled}
                onChange={(e) => updateField('email2' as keyof T, e.target.value as T[keyof T])}
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Dirección Envío Escritura *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.direccion_env_esc ?? ''}
                disabled={controlDisabled}
                onChange={(e) =>
                  updateField('direccion_env_esc' as keyof T, e.target.value as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-1 xl:col-span-1">
              <label className="font-semibold text-sm">Región Envío Escritura *</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.region_env_esc ?? null}
                options={regionOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) =>
                  updateField('region_env_esc' as keyof T, e.value as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-1 xl:col-span-1">
              <label className="font-semibold text-sm">Comuna Envío Escritura *</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.comuna_env_esc ?? null}
                options={comunaOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) =>
                  updateField('comuna_env_esc' as keyof T, e.value as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-2 xl:col-span-3">
              <label className="font-semibold text-sm">Dirección Envío Dividendo *</label>
              <InputText className="form-dropdown-presto w-full"
                value={selected.direccion_env_div ?? ''}
                disabled={controlDisabled}
                onChange={(e) =>
                  updateField('direccion_env_div' as keyof T, e.target.value as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-1 xl:col-span-1">
              <label className="font-semibold text-sm">Región Envío Dividendo *</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.region_env_div ?? null}
                options={regionOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) =>
                  updateField('region_env_div' as keyof T, e.value as T[keyof T])
                }
              />
            </div>

            <div className="flex flex-col gap-1 md:col-span-1 xl:col-span-1">
              <label className="font-semibold text-sm">Comuna Envío Dividendo *</label>
              <Dropdown className="form-dropdown-presto w-full"
                value={selected.comuna_env_div ?? null}
                options={comunaOptions}
                optionLabel="description"
                optionValue="code"
                disabled={controlDisabled}
                placeholder="Seleccione"
                onChange={(e) =>
                  updateField('comuna_env_div' as keyof T, e.value as T[keyof T])
                }
              />
            </div>
          </div>
        )}
      </Dialog>
    </Card>
  );
}
