import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  AsignarFirmasControles,
  AsignarFirmasAvanzarResponse,
  AsignarFirmasForm,
  CalcularAsignacionRequest,
  ResultadoAsignacion,
} from '../models/asignarFirmas';

const baseUrl = '/api/asignar-firmas';

function parseChecklist(value: unknown): string[] {
  if (Array.isArray(value)) {
    return value.filter(
      (item): item is string => typeof item === 'string',
    );
  }

  if (typeof value !== 'string' || !value.trim()) {
    return [];
  }

  try {
    const parsed = JSON.parse(value);

    return Array.isArray(parsed)
      ? parsed.filter(
        (item): item is string => typeof item === 'string',
      )
      : [];
  } catch {
    return value
      .split(',')
      .map((item) => item.trim())
      .filter(Boolean);
  }
}

export const asignarFirmasService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<AsignarFirmasForm | null>> {
    const { data } = await axiosClient.get<
      ApiResponse<AsignarFirmasForm | null>
    >(`${baseUrl}/GetByExpediente/${id_expediente}`);

    if (data.detail) {
      data.detail.checklist_documentos_solicitar =
        parseChecklist(
          data.detail.checklist_documentos_solicitar,
        );
    }

    return data;
  },

  async getControles(): Promise<
    ApiResponse<AsignarFirmasControles>
  > {
    const { data } = await axiosClient.get<
      ApiResponse<AsignarFirmasControles>
    >(`${baseUrl}/Controles`);

    return data;
  },

  async guardar(
    payload: AsignarFirmasForm,
  ): Promise<ApiResponse<AsignarFirmasForm>> {
    const serverPayload = {
      ...payload,
      checklist_documentos_solicitar:
        JSON.stringify(
          payload.checklist_documentos_solicitar ?? [],
        ),
    };

    const { data } = await axiosClient.post<
      ApiResponse<AsignarFirmasForm>
    >(`${baseUrl}/Save`, serverPayload);

    if (data.detail) {
      data.detail.checklist_documentos_solicitar =
        parseChecklist(
          data.detail.checklist_documentos_solicitar,
        );
    }

    return data;
  },

  async calcular(
    id_expediente: number,
    payload: CalcularAsignacionRequest,
  ): Promise<ApiResponse<ResultadoAsignacion>> {
    const { data } = await axiosClient.post<
      ApiResponse<ResultadoAsignacion>
    >(
      `${baseUrl}/CalcularAsignacion/${id_expediente}`,
      payload,
    );

    return data;
  },

  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<AsignarFirmasAvanzarResponse>> {
    const { data } = await axiosClient.post<
      ApiResponse<AsignarFirmasAvanzarResponse>
    >(`${baseUrl}/Avanzar/${id_expediente}`);

    return data;
  },
};
