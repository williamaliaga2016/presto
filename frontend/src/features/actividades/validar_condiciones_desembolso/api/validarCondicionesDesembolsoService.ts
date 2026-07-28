import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarCondicionesDesembolso, GetByExpedienteResponse } from '../models/validar_condiciones_desembolso';

const PATH_URL = '/api/validar-condiciones-desembolso';

export const validarCondicionesDesembolsoService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<GetByExpedienteResponse | null>> {
    const response = await axiosClient.get<ApiResponse<GetByExpedienteResponse | null>>(`${PATH_URL}/GetByIdExpediente/${id_expediente}`);
    return response.data;
  },
  async guardar(payload: ValidarCondicionesDesembolso): Promise<ApiResponse<ValidarCondicionesDesembolso>> {
    const response = await axiosClient.post<ApiResponse<ValidarCondicionesDesembolso>>(`${PATH_URL}/Save`, payload);
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(`${PATH_URL}/avanzar/${id_expediente}`);
    return response.data;
  },
  async suspender(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(`${PATH_URL}/suspender/${id_expediente}`);
    return response.data;
  },
};
