import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import type {
  ConsultActivityDTO,
  EtapaDTO,
  SearchCriteriaDTO,
} from '../models/ConsultActivity';

export const consultActivityService = {
  async getCatalogoTipoBusqueda(): Promise<ApiResponse<ControlBaseDTO[]>> {
    const response = await axiosClient.get<ApiResponse<ControlBaseDTO[]>>(
      '/api/ConsultActivity/GetCatalogoTipoBusqueda',
    );

    return response.data;
  },

  async getConsultActivity(
    payload: SearchCriteriaDTO,
  ): Promise<ApiResponse<ConsultActivityDTO[]>> {
    const response = await axiosClient.post<ApiResponse<ConsultActivityDTO[]>>(
      '/api/ConsultActivity/GetConsultActivity',
      payload,
    );

    return response.data;
  },

  async getConsultTrackingActivity(
    id_expediente: number,
  ): Promise<ApiResponse<EtapaDTO[]>> {
    const response = await axiosClient.get<ApiResponse<EtapaDTO[]>>(
      `/api/ConsultActivity/GetConsultTrackinActivity/${id_expediente}`,
    );

    return response.data;
  },
};
