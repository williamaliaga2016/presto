import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarRecepcionBoleta, GetByExpedienteResponse } from '../models/realizar_recepcion_boleta';
import type { ControlesRecepcionBoleta } from '../models/controles';

const PATH_URL = '/api/realizar-recepcion-boleta';

export const realizarRecepcionBoletaService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<GetByExpedienteResponse | null>> {
    const response = await axiosClient.get<ApiResponse<GetByExpedienteResponse | null>>(
      `${PATH_URL}/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesRecepcionBoleta>> {
    const response = await axiosClient.get<ApiResponse<ControlesRecepcionBoleta>>(
      `${PATH_URL}/controles`,
    );
    return response.data;
  },

  async guardar(payload: RealizarRecepcionBoleta): Promise<ApiResponse<RealizarRecepcionBoleta>> {
    const response = await axiosClient.post<ApiResponse<RealizarRecepcionBoleta>>(
      `${PATH_URL}/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(
      `${PATH_URL}/avanzar/${id_expediente}`,
    );
    return response.data;
  },

  async ejecutarVUR(id_expediente: number): Promise<ApiResponse<RealizarRecepcionBoleta>> {
    const response = await axiosClient.post<ApiResponse<RealizarRecepcionBoleta>>(
      `${PATH_URL}/ejecutar-vur/${id_expediente}`,
    );
    return response.data;
  },
};
