import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { DevolucionVbComercialData } from '../models/devolucion_vb_comercial';
import type { DevolucionVbComercialCatalogos } from '../models/catalogos';
import type {
  AvanzarDevolucionVbComercialResponse,
  DevolucionVbComercialApiDetail,
} from '../models/devolucion_vb_comercial.response';

export const devolucionVbComercialService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<DevolucionVbComercialApiDetail | null>> {
    const response = await axiosClient.get<
      ApiResponse<DevolucionVbComercialApiDetail | null>
    >(`/api/DevolucionVbComercial/${id_expediente}`);

    return response.data;
  },

  async getControles(
    id_expediente: number,
  ): Promise<ApiResponse<DevolucionVbComercialCatalogos>> {
    const response = await axiosClient.get<
      ApiResponse<DevolucionVbComercialCatalogos>
    >(`/api/DevolucionVbComercial/${id_expediente}/controles`);

    return response.data;
  },

  async save(
    payload: DevolucionVbComercialData,
  ): Promise<ApiResponse<DevolucionVbComercialData>> {
    const response = await axiosClient.post<
      ApiResponse<DevolucionVbComercialData>
    >('/api/DevolucionVbComercial/guardar', payload);

    return response.data;
  },

  async avanzar(
    id_expediente: number,
    confirmar_cierre: boolean,
  ): Promise<ApiResponse<AvanzarDevolucionVbComercialResponse>> {
    const response = await axiosClient.post<
      ApiResponse<AvanzarDevolucionVbComercialResponse>
    >(
      `/api/DevolucionVbComercial/${id_expediente}/avanzar?confirmar_cierre=${confirmar_cierre}`,
      {},
    );

    return response.data;
  },
};
