import { useMemo, useState } from 'react';
import { Button } from 'primereact/button';
import { Calendar } from 'primereact/calendar';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';

import type { ControlesAntecedenteVendedor } from '../models/catalogo';
import type { CargaOperacionBancoAntecedenteVendedor } from '../models/carga_operacion_banco';

interface AntecedentesVendedorSectionProps {
  value: CargaOperacionBancoAntecedenteVendedor[];
  idExpediente: number;
  idCargaOperacionBanco: number;
  disabled: boolean;
  /**
   * Control explícito para habilitar acciones de la grilla.
   * Ver detalle siempre está habilitado.
   * Editar / eliminar / agregar solo se habilitan cuando canEdit = true.
   */
  canEdit?: boolean;
  controles: ControlesAntecedenteVendedor;
  loadingControles?: boolean;
  onChange: (value: CargaOperacionBancoAntecedenteVendedor[]) => void;
  onWarn?: (message: string) => void;
}

type DialogMode = 'view' | 'edit' | 'create';

const emptyMessage = 'Sin registros';

const now = () => new Date().toISOString();

const emptyVendedor = (
  idExpediente: number,
  idCargaOperacionBanco: number,
): CargaOperacionBancoAntecedenteVendedor => ({
  id_carga_operacion_banco_antecedente_vendedor: 0,
  id_carga_operacion_banco: idCargaOperacionBanco,
  id_expediente: idExpediente,
  rut: '',
  tipo_vendedor: null,
  razon_social: '',
  nombres: '',
  apellido_paterno: '',
  apellido_materno: '',
  fecha_nacimiento: null,
  genero: null,
  estado_civil: null,
  relacion_titular: null,
  direccion: '',
  region: null,
  comuna: null,
  telefono: '',
  email: '',
  nacionalidad: null,
  profesion: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

const toDate = (value?: string | null): Date | null => {
  if (!value) return null;

  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? null : date;
};

const toIsoDate = (value: Date | null | undefined): string | null => {
  if (!value) return null;
  return value.toISOString();
};

const getOptionDescription = (
  options: { code?: string | null; description?: string | null }[],
  code?: string | null,
) => options.find((option) => option.code === code)?.description ?? code ?? '';

export default function AntecedentesVendedorSection({
  value,
  idExpediente,
  idCargaOperacionBanco,
  disabled,
  canEdit,
  controles,
  loadingControles = false,
  onChange,
  onWarn,
}: AntecedentesVendedorSectionProps) {
  const [dialogVisible, setDialogVisible] = useState(false);
  const [dialogMode, setDialogMode] = useState<DialogMode>('view');
  const [editingIndex, setEditingIndex] = useState<number | null>(null);
  const [draft, setDraft] = useState<CargaOperacionBancoAntecedenteVendedor>(
    emptyVendedor(idExpediente, idCargaOperacionBanco),
  );

  /*
   * Modo de edición de acciones de grilla.
   *
   * IMPORTANTE:
   * La habilitación del lápiz y tacho NO debe depender del render inicial
   * de PrimeReact DataTable. Por eso usamos una variable explícita y luego
   * forzamos el key de la grilla/columna acciones.
   *
   * - Ver detalle siempre habilitado.
   * - Editar / eliminar / agregar solo si el botón general Editar activó canEdit.
   */
  const canModifyRows = typeof canEdit === 'boolean' ? canEdit : !disabled;

  const gridModeKey = canModifyRows ? 'edit' : 'view';

  const isDialogReadOnly = dialogMode === 'view' || !canModifyRows;

  const rows = useMemo(
    () => (value ?? []).filter((vendedor) => vendedor.row_status !== false),
    [value],
  );

  const openNew = () => {
    if (!canModifyRows) return;

    setEditingIndex(null);
    setDialogMode('create');
    setDraft(emptyVendedor(idExpediente, idCargaOperacionBanco));
    setDialogVisible(true);
  };

  const openView = (rowData: CargaOperacionBancoAntecedenteVendedor) => {
    setEditingIndex(null);
    setDialogMode('view');
    setDraft({
      ...emptyVendedor(idExpediente, idCargaOperacionBanco),
      ...rowData,
      id_expediente: rowData.id_expediente || idExpediente,
      id_carga_operacion_banco:
        rowData.id_carga_operacion_banco || idCargaOperacionBanco,
    });
    setDialogVisible(true);
  };

  const openEdit = (
    rowData: CargaOperacionBancoAntecedenteVendedor,
    rowIndex: number,
  ) => {
    if (!canModifyRows) return;

    setEditingIndex(rowIndex);
    setDialogMode('edit');
    setDraft({
      ...emptyVendedor(idExpediente, idCargaOperacionBanco),
      ...rowData,
      id_expediente: rowData.id_expediente || idExpediente,
      id_carga_operacion_banco:
        rowData.id_carga_operacion_banco || idCargaOperacionBanco,
    });
    setDialogVisible(true);
  };

  const updateDraft = <K extends keyof CargaOperacionBancoAntecedenteVendedor>(
    field: K,
    fieldValue: CargaOperacionBancoAntecedenteVendedor[K],
  ) => {
    if (isDialogReadOnly) return;

    setDraft((prev) => ({
      ...prev,
      [field]: fieldValue,
    }));
  };

  const validateDraft = () => {
    if (!draft.rut?.trim()) return 'Debe ingresar RUT del vendedor.';
    if (!draft.tipo_vendedor) return 'Debe seleccionar Tipo de Vendedor.';

    const tipo = draft.tipo_vendedor?.toString().toUpperCase();

    if (tipo === 'JURIDICO' || tipo === 'JURÍDICO' || tipo === '02') {
      if (!draft.razon_social?.trim()) {
        return 'Debe ingresar Razón Social del vendedor jurídico.';
      }
    } else {
      if (!draft.nombres?.trim()) return 'Debe ingresar Nombres del vendedor.';
      if (!draft.apellido_paterno?.trim()) {
        return 'Debe ingresar Apellido Paterno del vendedor.';
      }
    }

    return '';
  };

  const saveDraft = () => {
    if (isDialogReadOnly) return;

    const validationMessage = validateDraft();

    if (validationMessage) {
      onWarn?.(validationMessage);
      return;
    }

    const normalizedDraft: CargaOperacionBancoAntecedenteVendedor = {
      ...draft,
      id_expediente: idExpediente || draft.id_expediente,
      id_carga_operacion_banco:
        idCargaOperacionBanco || draft.id_carga_operacion_banco,
      is_active: draft.is_active ?? true,
      row_status: draft.row_status ?? true,
      created_date: draft.created_date ?? now(),
    };

    if (dialogMode === 'create') {
      onChange([...(value ?? []), normalizedDraft]);
    } else if (editingIndex !== null) {
      onChange(
        (value ?? []).map((item, index) =>
          index === editingIndex ? normalizedDraft : item,
        ),
      );
    }

    setDialogVisible(false);
  };

  const executeRemoveRow = (
    rowData: CargaOperacionBancoAntecedenteVendedor,
    rowIndex: number,
  ) => {
    if (rowData.id_carga_operacion_banco_antecedente_vendedor > 0) {
      onChange(
        (value ?? []).map((item, index) =>
          index === rowIndex
            ? {
                ...item,
                row_status: false,
                is_active: false,
              }
            : item,
        ),
      );
      return;
    }

    onChange((value ?? []).filter((_, index) => index !== rowIndex));
  };

  const confirmRemoveRow = (
    rowData: CargaOperacionBancoAntecedenteVendedor,
    rowIndex: number,
  ) => {
    if (!canModifyRows) return;

    const vendedorLabel =
      nombreVendedorBodyTemplate(rowData) || rowData.rut || 'este registro';

    confirmDialog({
      header: 'Confirmar eliminación',
      message: `¿Está seguro de querer eliminar ${vendedorLabel}?`,
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Sí, eliminar',
      rejectLabel: 'Cancelar',
      acceptClassName: 'p-button-danger',
      accept: () => executeRemoveRow(rowData, rowIndex),
    });
  };

  const actionBodyTemplate = (
    rowData: CargaOperacionBancoAntecedenteVendedor,
    options: { rowIndex: number },
  ) => (
    <div className="flex gap-2 justify-center">
      <Button
        type="button"
        icon="pi pi-eye"
        severity="secondary"
        outlined
        size="small"
        onClick={() => openView(rowData)}
        tooltip="Ver detalle"
      />
      <Button
        key={`editar-${gridModeKey}-${options.rowIndex}`}
        type="button"
        icon="pi pi-pencil"
        severity="info"
        outlined
        size="small"
        disabled={canModifyRows ? false : true}
        onClick={() => openEdit(rowData, options.rowIndex)}
        tooltip="Editar"
      />
      <Button
        key={`eliminar-${gridModeKey}-${options.rowIndex}`}
        type="button"
        icon="pi pi-trash"
        severity="danger"
        outlined
        size="small"
        disabled={canModifyRows ? false : true}
        onClick={() => confirmRemoveRow(rowData, options.rowIndex)}
        tooltip="Eliminar"
      />
    </div>
  );

  const tipoVendedorBodyTemplate = (
    rowData: CargaOperacionBancoAntecedenteVendedor,
  ) => getOptionDescription(controles.tipo_vendedor, rowData.tipo_vendedor);

  const nombreVendedorBodyTemplate = (
    rowData: CargaOperacionBancoAntecedenteVendedor,
  ) => {
    if (rowData.razon_social?.trim()) return rowData.razon_social;

    return [rowData.nombres, rowData.apellido_paterno, rowData.apellido_materno]
      .filter(Boolean)
      .join(' ');
  };

  const getDialogTitle = () => {
    if (dialogMode === 'view') return 'Detalle del Vendedor';
    if (dialogMode === 'edit') return 'Editar Vendedor';
    return 'Agregar Vendedor / Constructora';
  };

  const dialogFooter =
    dialogMode === 'view' ? (
      <div className="flex justify-end gap-2">
        <Button
          type="button"
          label="Cerrar"
          icon="pi pi-times"
          severity="secondary"
          outlined
          onClick={() => setDialogVisible(false)}
        />
      </div>
    ) : (
      <div className="flex justify-end gap-2">
        <Button
          type="button"
          label="Cancelar"
          icon="pi pi-times"
          severity="secondary"
          outlined
          onClick={() => setDialogVisible(false)}
        />
        <Button
          type="button"
          label="Aceptar"
          icon="pi pi-check"
          severity="success"
          onClick={saveDraft}
          disabled={!canModifyRows}
        />
      </div>
    );

  return (
    <div className="flex flex-col gap-4">
      <ConfirmDialog />
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-3">
        <div>
          <h3 className="text-lg font-semibold text-gray-800">
            Datos del Vendedor / Constructora
          </h3>
          <p className="text-sm text-gray-500">
            Agregue uno o más vendedores asociados a la operación.
          </p>
        </div>

        <Button
          type="button"
          label="Agregar Vendedor / Constructora"
          icon="pi pi-plus"
          severity="success"
          outlined
          onClick={openNew}
          disabled={!canModifyRows}
          className="btn-responsive"
        />
      </div>

      <DataTable
        key={`antecedentes-vendedor-grid-${gridModeKey}`}
        value={rows}
        emptyMessage={emptyMessage}
        paginator={rows.length > 5}
        rows={5}
        stripedRows
        responsiveLayout="scroll"
          size="small"
        className="text-sm datatable-presto"
      >
        <Column field="rut" header="N° Identificación / NIT" />
        <Column
          field="tipo_vendedor"
          header="Tipo"
          body={tipoVendedorBodyTemplate}
        />
        <Column header="Vendedor" body={nombreVendedorBodyTemplate} />
        <Column field="telefono" header="Teléfono" />
        <Column field="email" header="Email" />
        <Column
          key={`acciones-${gridModeKey}`}
          header="Acciones"
          body={actionBodyTemplate}
          style={{ width: '170px' }}
        />
      </DataTable>

      <Dialog
        header={getDialogTitle()}
        visible={dialogVisible}
        onHide={() => setDialogVisible(false)}
        footer={dialogFooter}
        modal
        className="w-full md:w-10/12 lg:w-8/12"
      >
        <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">N° Identificación / NIT *</label>
            <InputText
              value={draft.rut ?? ''}
              onChange={(e) => updateDraft('rut', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese N° Identificación / NIT"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Tipo Vendedor *</label>
            <Dropdown
              value={draft.tipo_vendedor ?? null}
              options={controles.tipo_vendedor}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('tipo_vendedor', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Razón Social</label>
            <InputText
              value={draft.razon_social ?? ''}
              onChange={(e) => updateDraft('razon_social', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese razón social"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Nombres</label>
            <InputText
              value={draft.nombres ?? ''}
              onChange={(e) => updateDraft('nombres', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese nombres"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Apellido Paterno</label>
            <InputText
              value={draft.apellido_paterno ?? ''}
              onChange={(e) => updateDraft('apellido_paterno', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese apellido paterno"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Apellido Materno</label>
            <InputText
              value={draft.apellido_materno ?? ''}
              onChange={(e) => updateDraft('apellido_materno', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese apellido materno"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Fecha Nacimiento</label>
            <Calendar
              value={toDate(draft.fecha_nacimiento)}
              onChange={(e) =>
                updateDraft('fecha_nacimiento', toIsoDate(e.value as Date | null))
              }
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              dateFormat="dd/mm/yy"
              showIcon
              placeholder="Seleccione fecha"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Género</label>
            <Dropdown
              value={draft.genero ?? null}
              options={controles.genero}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('genero', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Estado Civil</label>
            <Dropdown
              value={draft.estado_civil ?? null}
              options={controles.estado_civil}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('estado_civil', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Relación Titular</label>
            <Dropdown
              value={draft.relacion_titular ?? null}
              options={controles.relacion_titular}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('relacion_titular', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
            />
          </div>

          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="font-semibold text-sm">Dirección</label>
            <InputText
              value={draft.direccion ?? ''}
              onChange={(e) => updateDraft('direccion', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese dirección"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Departamento</label>
            <Dropdown
              value={draft.region ?? null}
              options={controles.region}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('region', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
              filter
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Municipio</label>
            <Dropdown
              value={draft.comuna ?? null}
              options={controles.comuna}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('comuna', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
              filter
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Teléfono</label>
            <InputText
              value={draft.telefono ?? ''}
              onChange={(e) => updateDraft('telefono', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese teléfono"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Email</label>
            <InputText
              value={draft.email ?? ''}
              onChange={(e) => updateDraft('email', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese email"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Nacionalidad</label>
            <Dropdown
              value={draft.nacionalidad ?? null}
              options={controles.nacionalidad}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('nacionalidad', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
              filter
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Profesión</label>
            <InputText
              value={draft.profesion ?? ''}
              onChange={(e) => updateDraft('profesion', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese profesión"
            />
          </div>
        </div>
      </Dialog>
    </div>
  );
}
