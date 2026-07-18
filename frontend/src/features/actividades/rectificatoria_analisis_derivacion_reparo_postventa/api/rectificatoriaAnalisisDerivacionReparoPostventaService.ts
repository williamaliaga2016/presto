import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RectificatoriaAnalisisDerivacionReparoPostventa } from '../models/rectificatoria_analisis_derivacion_reparo_postventa';
import { ControlesRectificatoriaAnalisisDerivacionReparoPostventa } from '../models/catalogo';

export const rectificatoriaAnalisisDerivacionReparoPostventaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RectificatoriaAnalisisDerivacionReparoPostventa | null>> {
    const response = await axiosClient.get<ApiResponse<RectificatoriaAnalisisDerivacionReparoPostventa | null>>(
      `/api/RectificatoriaAnalisisDerivacionReparoPostventa/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: RectificatoriaAnalisisDerivacionReparoPostventa,
  ): Promise<ApiResponse<RectificatoriaAnalisisDerivacionReparoPostventa>> {
    const response = await axiosClient.post<ApiResponse<RectificatoriaAnalisisDerivacionReparoPostventa>>(
      `/api/RectificatoriaAnalisisDerivacionReparoPostventa/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RectificatoriaAnalisisDerivacionReparoPostventa/Avanzar/${id_expediente}`,
    );

    return response.data;
  },

  async getControlesRectificatoriaAnalisisDerivacionReparoPostventa(): Promise<ApiResponse<ControlesRectificatoriaAnalisisDerivacionReparoPostventa>> {
    const response = await axiosClient.get<ApiResponse<ControlesRectificatoriaAnalisisDerivacionReparoPostventa>>(
      `/api/RectificatoriaAnalisisDerivacionReparoPostventa/getControlesRectificatoriaAnalisisDerivacionReparoPostventa`,
    );

    return response.data;
  },
};
