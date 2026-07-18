import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import type { ExpedienteDigital } from '../models/ExpedienteDigital';

const baseUrl = '/api/ExpedienteDigital';

function getFileNameFromContentDisposition(
  contentDisposition?: string,
): string | null {
  if (!contentDisposition) return null;

  const utf8FileNameMatch = contentDisposition.match(
    /filename\*=UTF-8''([^;]+)/i,
  );
  if (utf8FileNameMatch?.[1]) {
    return decodeURIComponent(utf8FileNameMatch[1].replace(/"/g, ''));
  }

  const fileNameMatch = contentDisposition.match(/filename="?([^"]+)"?/i);
  return fileNameMatch?.[1] ?? null;
}

export const expedienteDigitalService = {
  /**
   * Consulta los archivos del expediente digital y permite filtrar por actividad cuando una vista no debe exponer todo el expediente.
   *
   * @param id_expediente Identificador del expediente.
   * @param activity_id Actividad workflow usada como filtro opcional del listado.
   * @returns Respuesta con los archivos visibles para el expediente y filtro solicitado.
   */
  async getFilesExpedienteDigital(
    id_expediente: number,
    activity_id?: string,
  ): Promise<ApiResponse<ExpedienteDigital[]>> {
    const response = await axiosClient.get<ApiResponse<ExpedienteDigital[]>>(
      `${baseUrl}/GetFilesExpedienteDigital/${id_expediente}`,
      {
        params: activity_id ? { activityId: activity_id } : undefined,
      },
    );

    return response.data;
  },

  async getFilesByActividad(
    id_expediente: number,
    actividad_id: string,
  ): Promise<ApiResponse<ExpedienteDigital[]>> {
    const response = await axiosClient.get<ApiResponse<ExpedienteDigital[]>>(
      `${baseUrl}/GetFilesByActividad/${id_expediente}/${actividad_id}`,
    );

    return response.data;
  },

  async getCatalogoDocumentos(
    id_expediente: number,
    id_expediente_digital: number,
  ): Promise<ApiResponse<ControlBaseDTO[]>> {
    const response = await axiosClient.get<ApiResponse<ControlBaseDTO[]>>(
      `${baseUrl}/GetCatalogoDocumentos/${id_expediente}/${id_expediente_digital}`,
    );

    return response.data;
  },

  async getControlCatExpedienteDigital(): Promise<
    ApiResponse<ControlBaseDTO[]>
  > {
    const response = await axiosClient.get<ApiResponse<ControlBaseDTO[]>>(
      `${baseUrl}/GetControlCatExpedienteDigital`,
    );

    return response.data;
  },

  async uploadFile(params: {
    file: File;
    id_expediente: number;
    activity_id?: string;
    doc_name?: string;
    work_flow_process_id?: string;
  }): Promise<ApiResponse<boolean>> {
    const formData = new FormData();

    formData.append('idExpedienteStr', String(params.id_expediente));
    formData.append('activityID', params.activity_id ?? '');
    formData.append('docName', params.doc_name ?? '');
    formData.append('workFlowProcessID', params.work_flow_process_id ?? '');
    formData.append('expedienteDigitalFormData', params.file);

    const response = await axiosClient.post<ApiResponse<boolean>>(
      `${baseUrl}/UploadFile`,
      formData,
      {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      },
    );

    return response.data;
  },

  async saveExpedienteDigital(
    payload: ExpedienteDigital,
  ): Promise<ApiResponse<ExpedienteDigital>> {
    const response = await axiosClient.post<ApiResponse<ExpedienteDigital>>(
      `${baseUrl}/Save`,
      payload,
    );

    return response.data;
  },

  async updateEstadoDocumento(
    payload: ExpedienteDigital,
  ): Promise<ApiResponse<ExpedienteDigital>> {
    const response = await axiosClient.post<ApiResponse<ExpedienteDigital>>(
      `${baseUrl}/UpdEstadoDocumento`,
      payload,
    );

    return response.data;
  },

  async getTemplateFileName(
    id_cat_expediente_digital: number,
    id_cat_expediente_digital_documento: number,
  ): Promise<ApiResponse<string>> {
    const response = await axiosClient.get<ApiResponse<string>>(
      `${baseUrl}/GetTemplateFileName/${id_cat_expediente_digital}/${id_cat_expediente_digital_documento}`,
    );

    return response.data;
  },

  async getCatalogoTipoDocumentoValido(): Promise<
    ApiResponse<ControlBaseDTO[]>
  > {
    const response = await axiosClient.get<ApiResponse<ControlBaseDTO[]>>(
      `${baseUrl}/GetCatalogoTipoDocumentoValido`,
    );

    return response.data;
  },

  async downloadFile(
    id_document: number,
  ): Promise<{ blob: Blob; file_name: string | null }> {
    const response = await axiosClient.get<Blob>(
      `${baseUrl}/DownloadFile/${id_document}`,
      {
        responseType: 'blob',
      },
    );

    return {
      blob: response.data,
      file_name: getFileNameFromContentDisposition(
        response.headers?.['content-disposition'],
      ),
    };
  },

  async downloadTemplateFile(
    template_file_name: string,
  ): Promise<{ blob: Blob; file_name: string | null }> {
    const response = await axiosClient.get<Blob>(
      `${baseUrl}/DownloadTemplateFile/${encodeURIComponent(template_file_name)}`,
      {
        responseType: 'blob',
      },
    );

    return {
      blob: response.data,
      file_name:
        getFileNameFromContentDisposition(
          response.headers?.['content-disposition'],
        ) ?? template_file_name,
    };
  },
};
