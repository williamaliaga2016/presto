import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  AsignarFirmasConEncabezado,
  AsignarFirmasControles,
  AsignarFirmasAvanzarResponse,
  AsignarFirmasForm,
  CalcularAsignacionRequest,
  ResultadoAsignacion,
} from '../models/asignarFirmas';

const baseUrl = '/api/asignar-firmas';

function parseChecklist(value: unknown): string[] {
  if (Array.isArray(value)) return value.filter((item): item is string => typeof item === 'string');
  if (typeof value !== 'string' || !value.trim()) return [];
  try {
    const parsed = JSON.parse(value);
    return Array.isArray(parsed) ? parsed.filter((item): item is string => typeof item === 'string') : [];
  } catch {
    return value.split(',').map((item) => item.trim()).filter(Boolean);
  }
}

export const asignarFirmasService = {
  async getConEncabezado(id: number) {
    const { data } = await axiosClient.get<ApiResponse<AsignarFirmasConEncabezado>>(
      `${baseUrl}/${id}/con-encabezado`,
    );
    if (data.detail?.formulario) {
      data.detail.formulario.checklist_documentos_solicitar = parseChecklist(
        data.detail.formulario.checklist_documentos_solicitar,
      );
    }
    return data;
  },
  async getControles(id: number) {
    const { data } = await axiosClient.get<ApiResponse<AsignarFirmasControles>>(
      `${baseUrl}/${id}/controles`,
    );
    return data;
  },
  async guardar(payload: AsignarFirmasForm) {
    const serverPayload = {
      ...payload,
      checklist_documentos_solicitar: JSON.stringify(payload.checklist_documentos_solicitar ?? []),
    };
    const { data } = await axiosClient.post<ApiResponse<AsignarFirmasForm>>(
      `${baseUrl}/guardar`, serverPayload,
    );
    if (data.detail) {
      data.detail.checklist_documentos_solicitar = parseChecklist(
        data.detail.checklist_documentos_solicitar,
      );
    }
    return data;
  },
  async calcular(id: number, payload: CalcularAsignacionRequest) {
    const { data } = await axiosClient.post<ApiResponse<ResultadoAsignacion>>(
      `${baseUrl}/${id}/calcular-asignacion`, payload,
    );
    return data;
  },
  async avanzar(id: number) {
    const { data } = await axiosClient.post<ApiResponse<AsignarFirmasAvanzarResponse>>(
      `${baseUrl}/${id}/avanzar`,
    );
    return data;
  },
};
