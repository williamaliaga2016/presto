import { useEffect, useMemo, useRef, useState } from 'react';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import { DataTable } from 'primereact/datatable';
import { Dropdown } from 'primereact/dropdown';
import { InputTextarea } from 'primereact/inputtextarea';

import { expedienteDigitalService } from '../api/expedienteDigitalService';
import { useArchivosExpedienteDigital } from '../hooks/useArchivosExpedienteDigital';
import { useCategoriasExpedienteDigital } from '../hooks/useCategoriasExpedienteDigital';
import { useDocumentosExpedienteDigital } from '../hooks/useDocumentosExpedienteDigital';
import { useSaveExpedienteDigital } from '../hooks/useSaveExpedienteDigital';
import { useTemplateFileName } from '../hooks/useTemplateFileName';
import { useUploadExpedienteDigitalFile } from '../hooks/useUploadExpedienteDigitalFile';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import type {
  ExpedienteDigital
} from '../models/ExpedienteDigital';

type ExpedienteDigitalPageProps = {
  id_expediente: number;
  activity_id?: string;
  files_activity_id?: string;
  filter_by_activity?: boolean;
  locked_categoria_id?: number;
  locked_documento_id?: number;
  read_only?: boolean;
  onDocumentUploaded?: () => void;
  onDocumentsLoaded?: (docs: { id_tipo_documento: number; estado: string }[]) => void;
};

type SelectOption = {
  label: string;
  value: number;
};

const MAX_COMENTARIOS = 500;

function getControlId(item: ControlBaseDTO): number {
  return Number(item.id ?? item.code ?? 0);
}

function getControlDescription(item: ControlBaseDTO): string {
  return String(item.description ?? '');
}

function getExtension(fileName: string): string {
  const parts = fileName.split('.');
  return parts.length > 1 ? `.${parts.pop()}` : '';
}

function formatFecha(value?: string | null): string {
  if (!value) return '';

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;

  return date.toLocaleString('es-PE', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    hour12: false,
  });
}

function downloadBlob(blob: Blob, fileName: string) {
  const url = window.URL.createObjectURL(blob);
  const anchor = document.createElement('a');

  document.body.appendChild(anchor);
  anchor.style.display = 'none';
  anchor.href = url;
  anchor.download = fileName;
  anchor.click();

  window.URL.revokeObjectURL(url);
  anchor.remove();
}

function openBlob(blob: Blob) {
  const url = window.URL.createObjectURL(blob);
  window.open(url, '_blank', 'noopener,noreferrer');
}

export default function ExpedienteDigitalPage({
  id_expediente,
  activity_id,
  files_activity_id,
  filter_by_activity = false,
  locked_categoria_id,
  locked_documento_id,
  read_only = false,
  onDocumentUploaded,
  onDocumentsLoaded,
}: ExpedienteDigitalPageProps) {
  const expediente_id = Number(id_expediente ?? 0);

  const fileInputRef = useRef<HTMLInputElement | null>(null);

  const [id_categoria, setIdCategoria] = useState<number>(locked_categoria_id ?? 0);
  const [id_documento, setIdDocumento] = useState<number>(locked_documento_id ?? 0);
  const [comentarios, setComentarios] = useState<string>('');
  const [archivo, setArchivo] = useState<File | null>(null);
  const [selected_archivos, setSelectedArchivos] = useState<ExpedienteDigital[]>([]);

  const {
    data: categorias_response,
    isLoading: isLoadingCategorias,
  } = useCategoriasExpedienteDigital();

  const categorias = useMemo(
    () => (categorias_response?.status ? categorias_response.detail ?? [] : []),
    [categorias_response],
  );

  const categoria_options = useMemo<SelectOption[]>(() => {
    const options = categorias.map((categoria) => ({
      label: getControlDescription(categoria),
      value: getControlId(categoria),
    }));

    if (locked_categoria_id) {
      return options.filter((option) => option.value === locked_categoria_id);
    }

    return options;
  }, [categorias, locked_categoria_id]);

  const effective_id_categoria =
    locked_categoria_id ?? (id_categoria || (categoria_options[0]?.value ?? 0));

  const {
    data: documentos_response,
    isLoading: isLoadingDocumentos,
  } = useDocumentosExpedienteDigital(expediente_id, effective_id_categoria);

  const documentos = useMemo(
    () => (documentos_response?.status ? documentos_response.detail ?? [] : []),
    [documentos_response],
  );

  const effective_files_activity_id =
    files_activity_id ?? (filter_by_activity ? activity_id : undefined);

  const {
    data: archivos_response,
    isLoading: isLoadingArchivos,
    refetch: refetchArchivos,
  } = useArchivosExpedienteDigital(expediente_id, effective_files_activity_id);

  const archivos = useMemo<ExpedienteDigital[]>(() => {
    const detail = archivos_response?.status ? archivos_response.detail ?? [] : [];
    return detail.map((item) => ({
      ...item,
      is_checked: selected_archivos.some(
        (selected) => selected.id_archivo === item.id_archivo,
      ),
    }));
  }, [archivos_response, selected_archivos]);

  // Notificar al padre cuando la lista de archivos cambia (CA5)
  useEffect(() => {
    if (!onDocumentsLoaded) return;
    const mapped = archivos.map((a) => ({
      id_tipo_documento: Number(a.id_documento ?? 0),
      estado: a.is_active ? 'CARGADO' : 'INACTIVO',
    }));
    onDocumentsLoaded(mapped);
  }, [archivos, onDocumentsLoaded]);

  const documento_options = useMemo<SelectOption[]>(() => {
    const options = documentos.map((documento) => ({
      label: getControlDescription(documento),
      value: getControlId(documento),
    }));

    if (locked_documento_id) {
      return options.filter((option) => option.value === locked_documento_id);
    }

    return options;
  }, [documentos, locked_documento_id]);

  const effective_id_documento = locked_documento_id ?? id_documento;

  const documento_seleccionado = useMemo(
    () =>
      documentos.find(
        (documento) => getControlId(documento) === effective_id_documento,
      ),
    [documentos, effective_id_documento],
  );

  const {
    data: template_response,
  } = useTemplateFileName(effective_id_categoria, effective_id_documento);

  const template_file_name = template_response?.status
    ? String(template_response.detail ?? '')
    : '';

  const uploadFileMutation = useUploadExpedienteDigitalFile();
  const saveExpedienteDigitalMutation = useSaveExpedienteDigital();

  const total_caracteres = useMemo(() => comentarios.length, [comentarios]);
  const is_saving =
    uploadFileMutation.isPending || saveExpedienteDigitalMutation.isPending;

  const limpiarFormulario = () => {
    setIdDocumento(locked_documento_id ?? 0);
    setComentarios('');
    setArchivo(null);

    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const handleCategoriaChange = (value: number) => {
    setIdCategoria(value);
    if (!locked_documento_id) {
      setIdDocumento(0);
    }
  };

  const handleDocumentoChange = (value: number) => {
    setIdDocumento(value);
  };

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const selectedFile = event.target.files?.item(0) ?? null;
    setArchivo(selectedFile);
  };

  const handleSubmit = async () => {
    if (!expediente_id || expediente_id <= 0) {
      alert('El expediente no es válido.');
      return;
    }

    if (!effective_id_categoria || effective_id_categoria <= 0) {
      alert('Seleccione una categoría.');
      return;
    }

    if (!effective_id_documento || effective_id_documento <= 0) {
      alert('Seleccione un documento.');
      return;
    }

    if (!archivo) {
      alert('Seleccione un archivo.');
      return;
    }

    const existe_archivo = archivos.some(
      (item) =>
        item.nombre_archivo_original?.trim().toLowerCase() ===
        archivo.name.trim().toLowerCase(),
    );

    if (existe_archivo) {
      alert('El archivo ya existe.');
      return;
    }

    const doc_name = documento_seleccionado
      ? getControlDescription(documento_seleccionado)
      : '';

    const uploadResponse = await uploadFileMutation.mutateAsync({
      file: archivo,
      id_expediente: expediente_id,
      activity_id,
      doc_name,
    });

    if (!uploadResponse.status) {
      alert(uploadResponse.message ?? 'No se pudo subir el archivo.');
      return;
    }

    const payload: ExpedienteDigital = {
      id_expediente: expediente_id,
      activity_id: activity_id ?? null,
      id_documento: effective_id_documento,
      id_usuario: null,
      nombre_archivo: '',
      nombre_archivo_original: archivo.name,
      extension: getExtension(archivo.name),
      version_archivo: 0,
      fecha_alta: null,
      comentarios: comentarios.trim(),
      is_active: true,
    };

    const saveResponse = await saveExpedienteDigitalMutation.mutateAsync(payload);

    if (!saveResponse.status) {
      alert(saveResponse.message ?? 'No se pudo guardar el expediente digital.');
      return;
    }

    limpiarFormulario();
    setSelectedArchivos([]);
    await refetchArchivos();
    // CA5: Notificar al padre que un documento fue cargado exitosamente
    onDocumentUploaded?.();
  };

  const confirmarLimpiar = () => {
    const is_documento_dirty = locked_documento_id
      ? id_documento !== locked_documento_id
      : id_documento !== 0;

    if (!comentarios.trim() && !archivo && !is_documento_dirty) {
      limpiarFormulario();
      return;
    }

    confirmDialog({
      message: '¿Deseas limpiar el formulario?',
      header: 'Confirmación',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Sí',
      rejectLabel: 'No',
      accept: limpiarFormulario,
    });
  };

  const handleDownloadFile = async (file: ExpedienteDigital) => {
    if (!file.id_archivo) return;

    try {
      const response = await expedienteDigitalService.downloadFile(file.id_archivo);
      downloadBlob(
        response.blob,
        response.file_name ?? file.nombre_archivo_original ?? 'archivo',
      );
    } catch (error) {
      console.error('Error al descargar archivo:', error);
      alert('El archivo no existe o no pudo ser descargado.');
    }
  };

  const handleViewFile = async (file: ExpedienteDigital) => {
    if (!file.id_archivo) return;

    const extension = file.nombre_archivo_original?.split('.').pop()?.toLowerCase();

    if (extension !== 'pdf' && extension !== 'xml') {
      alert('Visor no disponible.');
      return;
    }

    try {
      const response = await expedienteDigitalService.downloadFile(file.id_archivo);
      openBlob(response.blob);
    } catch (error) {
      console.error('Error al visualizar archivo:', error);
      alert('El archivo no existe o no pudo ser visualizado.');
    }
  };

  const handleDownloadTemplate = async () => {
    if (!template_file_name) return;

    try {
      const response = await expedienteDigitalService.downloadTemplateFile(
        template_file_name,
      );

      downloadBlob(response.blob, response.file_name ?? template_file_name);
    } catch (error) {
      console.error('Error al descargar plantilla:', error);
      alert('No se pudo descargar la plantilla.');
    }
  };

  const handleDownloadSelected = async () => {
    if (selected_archivos.length === 0) {
      alert('Seleccione un archivo.');
      return;
    }

    for (const file of selected_archivos) {
      await handleDownloadFile(file);
    }
  };

  const renderResponsiveCell = (
    value: React.ReactNode,
    titulo: string,
  ) => <div data-titulo={titulo}>{value ?? ''}</div>;

  const documentoBodyTemplate = (rowData: ExpedienteDigital) =>
    renderResponsiveCell(rowData.nombre_archivo, 'Documento: ');

  const archivoBodyTemplate = (rowData: ExpedienteDigital) =>
    renderResponsiveCell(rowData.nombre_archivo_original, 'Archivo: ');

  function formatEstado(is_activate?: boolean | null): string {
    return is_activate ? 'Activo' : 'Inactivo';
  }

  const estadoBodyTemplate = (rowData: ExpedienteDigital) =>
  renderResponsiveCell(formatEstado(rowData.is_active), 'Estado: ');

  const fechaBodyTemplate = (rowData: ExpedienteDigital) =>
    renderResponsiveCell(formatFecha(rowData.fecha_alta), 'Fecha Digitalización: ');

  const comentariosBodyTemplate = (rowData: ExpedienteDigital) =>
    renderResponsiveCell(rowData.comentarios, 'Comentarios: ');

  const actionBodyTemplate = (rowData: ExpedienteDigital) => (
    <div className="flex gap-2 justify-content-center">
      <Button
        type="button"
        icon="pi pi-download"
        rounded
        outlined
        severity="info"
        tooltip="Descargar"
        onClick={() => handleDownloadFile(rowData)}
      />
      <Button
        type="button"
        icon="pi pi-eye"
        rounded
        outlined
        severity="secondary"
        tooltip="Ver"
        onClick={() => handleViewFile(rowData)}
      />
    </div>
  );

  if (!expediente_id || expediente_id <= 0) {
    return (
      <>
        <div className="mt-2 rounded-md border border-yellow-200 bg-yellow-50 px-4 py-3 text-sm text-yellow-800">
          El expediente digital se habilitará cuando exista un expediente válido.
        </div>
        <ConfirmDialog />
      </>
    );
  }

  return (
    <>
      {!read_only && (
      <Card className="w-full shadow-md card-presto-form mb-6">
        <form
          onSubmit={(event) => {
            event.preventDefault();
            handleSubmit();
          }}
          className="flex flex-col gap-2"
        >
          <div className="form-grid">
            <div className="flex flex-col">
              <label className="text-sm font-medium text-gray-700">
                Categoría
              </label>

              <Dropdown
                value={effective_id_categoria}
                options={categoria_options}
                onChange={(event) => handleCategoriaChange(Number(event.value))}
                placeholder="Seleccione..."
                className="form-dropdown-presto w-full"
                loading={isLoadingCategorias}
                disabled={!!locked_categoria_id}
              />
            </div>

            <div className="flex flex-col">
              <label className="text-sm font-medium text-gray-700">
                Documento
              </label>

              <Dropdown
                value={effective_id_documento}
                options={documento_options}
                onChange={(event) => handleDocumentoChange(Number(event.value))}
                placeholder="Seleccione..."
                className="form-dropdown-presto w-full"
                loading={isLoadingDocumentos}
                disabled={!effective_id_categoria || !!locked_documento_id}
              />

              {template_file_name && (
                <div className="flex justify-content-end pt-2">
                  <Button
                    type="button"
                    label="Descargar Plantilla"
                    icon="pi pi-download"
                    size="small"
                    onClick={handleDownloadTemplate}
                  />
                </div>
              )}
            </div>

            <div className="flex flex-col form-col-full">
              <label className="text-sm font-medium text-gray-700">
                Comentario
              </label>

              <InputTextarea
                value={comentarios}
                onChange={(event) => setComentarios(event.target.value)}
                rows={4}
                maxLength={MAX_COMENTARIOS}
                autoResize
                className="form-textarea-presto w-full"
              />

              <small
                className={
                  total_caracteres > MAX_COMENTARIOS - 20
                    ? 'text-red-500'
                    : 'text-gray-500'
                }
              >
                Máximo: {total_caracteres}/{MAX_COMENTARIOS} caracteres
              </small>
            </div>

            <div className="flex flex-col form-col-full">
              <label className="text-sm font-medium text-gray-700">
                Archivo
              </label>

              <input
                ref={fileInputRef}
                type="file"
                accept="*"
                hidden
                onChange={handleFileChange}
              />

              <div className="flex gap-2 align-items-center flex-wrap">
                <Button
                  type="button"
                  label="Adjuntar Archivo"
                  icon="pi pi-paperclip"
                  size="small"
                  onClick={() => fileInputRef.current?.click()}
                />

                <span className="text-sm text-gray-700">
                  {archivo ? (
                    <>
                      <strong>Archivo:</strong> {archivo.name}
                    </>
                  ) : (
                    'Ningún archivo seleccionado'
                  )}
                </span>
              </div>
            </div>
          </div>

          <div className="form-actions">
            <Button
              label={is_saving ? 'Guardando...' : 'Subir Archivo'}
              icon="pi pi-cloud-upload"
              severity="success"
              type="submit"
              disabled={is_saving}
            />

            <Button
              label="Limpiar"
              icon="pi pi-times"
              severity="secondary"
              type="button"
              outlined
              onClick={confirmarLimpiar}
              disabled={is_saving}
            />
          </div>
        </form>
      </Card>
      )}

      <Card className="w-full shadow-md">
        <DataTable
          value={archivos}
          dataKey="id_archivo"
          loading={isLoadingArchivos}
          paginator
          rows={5}
          rowsPerPageOptions={[5, 10, 20]}
          emptyMessage="Ningún dato disponible en esta tabla"
          selection={selected_archivos}
          onSelectionChange={(event) =>
            setSelectedArchivos(event.value as ExpedienteDigital[])
          }
          responsiveLayout="scroll"
        >
          <Column selectionMode="multiple" headerStyle={{ width: '3rem' }} />
          <Column
            field="nombre_archivo"
            header="Documento"
            body={documentoBodyTemplate}
            sortable
          />
          <Column
            field="nombre_archivo_original"
            header="Archivo"
            body={archivoBodyTemplate}
            sortable
          />
          <Column
            field="is_active"
            header="Estado"
            body={estadoBodyTemplate}
            sortable
          />
          <Column
            field="fecha_alta"
            header="Fecha Digitalización"
            body={fechaBodyTemplate}
            sortable
          />
          <Column
            field="comentarios"
            header="Comentarios"
            body={comentariosBodyTemplate}
          />
          <Column
            header="Acciones"
            body={actionBodyTemplate}
            exportable={false}
            style={{ minWidth: '8rem' }}
          />
        </DataTable>

        {selected_archivos.length > 0 && (
          <div className="mt-3">
            <Button
              type="button"
              label="Descargar seleccionados"
              icon="pi pi-download"
              size="small"
              onClick={handleDownloadSelected}
            />
          </div>
        )}
      </Card>

      <ConfirmDialog />
    </>
  );
}
