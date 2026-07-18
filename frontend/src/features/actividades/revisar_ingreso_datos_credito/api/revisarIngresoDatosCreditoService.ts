import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarIngresoDatosCredito } from '../models/revisar_ingreso_datos_credito';
import { ControlesDatosCredito } from '../models/catalogo';

export const revisarIngresoDatosCreditoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarIngresoDatosCredito | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarIngresoDatosCredito | null>>(
      `/api/RevisarIngresoDatosCredito/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: RevisarIngresoDatosCredito,
  ): Promise<ApiResponse<RevisarIngresoDatosCredito>> {
    const response = await axiosClient.post<ApiResponse<RevisarIngresoDatosCredito>>(
      `/api/RevisarIngresoDatosCredito/Save`,
      payload,
    );
    return response.data;
  },
  
  async getControlesDatosCredito(): Promise<ApiResponse<ControlesDatosCredito>> {
    const response = await axiosClient.get<ApiResponse<ControlesDatosCredito>>(
      `/api/RevisarIngresoDatosCredito/GetControlesDatosCredito`,
    );

    return response.data;
  },
};
