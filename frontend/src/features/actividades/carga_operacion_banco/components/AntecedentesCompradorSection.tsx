import { useMemo, useState } from 'react';
import { Button } from 'primereact/button';
import { Checkbox } from 'primereact/checkbox';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';

import type { ControlesAntecedenteComprador } from '../models/catalogo';
import type { CargaOperacionBancoAntecedenteComprador } from '../models/carga_operacion_banco';

interface AntecedentesCompradorSectionProps {
  value: CargaOperacionBancoAntecedenteComprador[];
  idExpediente: number;
  idCargaOperacionBanco: number;
  disabled: boolean;
  /**
   * Control explícito para habilitar acciones de la grilla.
   * Ver detalle siempre está habilitado.
   * Editar / eliminar / agregar solo se habilitan cuando canEdit = true.
   */
  canEdit?: boolean;
  controles: ControlesAntecedenteComprador;
  loadingControles?: boolean;
  onChange: (value: CargaOperacionBancoAntecedenteComprador[]) => void;
  onWarn?: (message: string) => void;
}

type DialogMode = 'view' | 'edit' | 'create';

const emptyMessage = 'Sin registros';

const now = () => new Date().toISOString();

const emptyComprador = (
  idExpediente: number,
  idCargaOperacionBanco: number,
): CargaOperacionBancoAntecedenteComprador => ({
  id_carga_operacion_banco_antecedente_comprador: 0,
  id_carga_operacion_banco: idCargaOperacionBanco,
  id_expediente: idExpediente,
  numero_identificacion: '',
  tipo_documento_id: null,
  nombre_completo: '',
  celular: '',
  direccion: '',
  telefono: '',
  email: '',
  situacion_laboral: null,
  cliente_nomina: false,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

const getOptionDescription = (
  options: { code?: string | null; description?: string | null }[],
  code?: string | null,
) => options.find((option) => option.code === code)?.description ?? code ?? '';

export default function AntecedentesCompradorSection({
  value,
  idExpediente,
  idCargaOperacionBanco,
  disabled,
  canEdit,
  controles,
  loadingControles = false,
  onChange,
  onWarn,
}: AntecedentesCompradorSectionProps) {
  const [dialogVisible, setDialogVisible] = useState(false);
  const [dialogMode, setDialogMode] = useState<DialogMode>('view');
  const [editingIndex, setEditingIndex] = useState<number | null>(null);
  const [draft, setDraft] = useState<CargaOperacionBancoAntecedenteComprador>(
    emptyComprador(idExpediente, idCargaOperacionBanco),
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
    () => (value ?? []).filter((comprador) => comprador.row_status !== false),
    [value],
  );

  const openNew = () => {
    if (!canModifyRows) return;

    setEditingIndex(null);
    setDialogMode('create');
    setDraft(emptyComprador(idExpediente, idCargaOperacionBanco));
    setDialogVisible(true);
  };

  const openView = (rowData: CargaOperacionBancoAntecedenteComprador) => {
    setEditingIndex(null);
    setDialogMode('view');
    setDraft({
      ...emptyComprador(idExpediente, idCargaOperacionBanco),
      ...rowData,
      id_expediente: rowData.id_expediente || idExpediente,
      id_carga_operacion_banco:
        rowData.id_carga_operacion_banco || idCargaOperacionBanco,
    });
    setDialogVisible(true);
  };

  const openEdit = (
    rowData: CargaOperacionBancoAntecedenteComprador,
    rowIndex: number,
  ) => {
    if (!canModifyRows) return;

    setEditingIndex(rowIndex);
    setDialogMode('edit');
    setDraft({
      ...emptyComprador(idExpediente, idCargaOperacionBanco),
      ...rowData,
      id_expediente: rowData.id_expediente || idExpediente,
      id_carga_operacion_banco:
        rowData.id_carga_operacion_banco || idCargaOperacionBanco,
    });
    setDialogVisible(true);
  };

  const updateDraft = <K extends keyof CargaOperacionBancoAntecedenteComprador>(
    field: K,
    fieldValue: CargaOperacionBancoAntecedenteComprador[K],
  ) => {
    if (isDialogReadOnly) return;

    setDraft((prev) => ({
      ...prev,
      [field]: fieldValue,
    }));
  };

  const validateDraft = () => {
    if (!draft.tipo_documento_id) return 'Debe seleccionar el Tipo de Identificación.';
    if (!draft.numero_identificacion?.trim()) {
      return 'Debe ingresar el N° de Identificación.';
    }
    if (!draft.nombre_completo?.trim()) return 'Debe ingresar el Nombre Completo.';

    return '';
  };

  const saveDraft = () => {
    if (isDialogReadOnly) return;

    const validationMessage = validateDraft();

    if (validationMessage) {
      onWarn?.(validationMessage);
      return;
    }

    const normalizedDraft: CargaOperacionBancoAntecedenteComprador = {
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
    rowData: CargaOperacionBancoAntecedenteComprador,
    rowIndex: number,
  ) => {
    if (rowData.id_carga_operacion_banco_antecedente_comprador > 0) {
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
    rowData: CargaOperacionBancoAntecedenteComprador,
    rowIndex: number,
  ) => {
    if (!canModifyRows) return;

    const compradorLabel = rowData.nombre_completo || rowData.numero_identificacion || 'este registro';

    confirmDialog({
      header: 'Confirmar eliminación',
      message: `¿Está seguro de querer eliminar ${compradorLabel}?`,
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Sí, eliminar',
      rejectLabel: 'Cancelar',
      acceptClassName: 'p-button-danger',
      accept: () => executeRemoveRow(rowData, rowIndex),
    });
  };

  const actionBodyTemplate = (
    rowData: CargaOperacionBancoAntecedenteComprador,
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

  const tipoDocumentoBodyTemplate = (
    rowData: CargaOperacionBancoAntecedenteComprador,
  ) => getOptionDescription(controles.tipo_documento_id, rowData.tipo_documento_id);

  const getDialogTitle = () => {
    if (dialogMode === 'view') return 'Detalle del Titular';
    if (dialogMode === 'edit') return 'Editar Titular';
    return 'Agregar Titular';
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
            Datos de los Titulares
          </h3>
          <p className="text-sm text-gray-500">
            Agregue el Titular 1 y, si aplica, los co-titulares asociados al crédito.
          </p>
        </div>

        <Button
          type="button"
          label="Agregar Titular"
          icon="pi pi-plus"
          severity="success"
          outlined
          onClick={openNew}
          disabled={!canModifyRows}
          className="btn-responsive"
        />
      </div>

      <DataTable
        key={`antecedentes-comprador-grid-${gridModeKey}`}
        value={rows}
        emptyMessage={emptyMessage}
        paginator={rows.length > 5}
        rows={5}
        stripedRows
        responsiveLayout="scroll"
          size="small"
        className="text-sm datatable-presto"
      >
        <Column field="numero_identificacion" header="N° Identificación" />
        <Column
          field="tipo_documento_id"
          header="Tipo Identificación"
          body={tipoDocumentoBodyTemplate}
        />
        <Column field="nombre_completo" header="Nombre Completo" />
        <Column field="celular" header="Celular" />
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
            <label className="font-semibold text-sm">Tipo de Identificación *</label>
            <Dropdown
              value={draft.tipo_documento_id ?? null}
              options={controles.tipo_documento_id}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('tipo_documento_id', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">N° Identificación *</label>
            <InputText
              value={draft.numero_identificacion ?? ''}
              onChange={(e) => updateDraft('numero_identificacion', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese N° de identificación"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Nombre Completo *</label>
            <InputText
              value={draft.nombre_completo ?? ''}
              onChange={(e) => updateDraft('nombre_completo', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese nombre completo"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Celular Cliente</label>
            <InputText
              value={draft.celular ?? ''}
              onChange={(e) => updateDraft('celular', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese celular"
            />
          </div>

          <div className="flex flex-col gap-1 md:col-span-2">
            <label className="font-semibold text-sm">Dirección Residencia</label>
            <InputText
              value={draft.direccion ?? ''}
              onChange={(e) => updateDraft('direccion', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese dirección de residencia"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">Teléfono Residente</label>
            <InputText
              value={draft.telefono ?? ''}
              onChange={(e) => updateDraft('telefono', e.target.value)}
              disabled={isDialogReadOnly}
              className="form-input-presto w-full"
              placeholder="Ingrese teléfono residente"
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
            <label className="font-semibold text-sm">Situación Laboral</label>
            <Dropdown
              value={draft.situacion_laboral ?? null}
              options={controles.situacion_laboral}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateDraft('situacion_laboral', e.value ?? null)}
              disabled={isDialogReadOnly}
              className="form-dropdown-presto w-full"
              loading={loadingControles}
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
            />
          </div>

          <div className="flex items-center gap-2 mt-4">
            <Checkbox
              inputId="cliente_nomina"
              checked={draft.cliente_nomina ?? false}
              onChange={(e) => updateDraft('cliente_nomina', e.checked ?? false)}
              disabled={isDialogReadOnly}
            />
            <label htmlFor="cliente_nomina" className="text-sm cursor-pointer">
              Cliente Nómina
            </label>
          </div>
        </div>
      </Dialog>
    </div>
  );
}
