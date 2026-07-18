import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CalculoGeneracionDocumento } from '../models/calculo_generacion_documento';
import type { ControlesPropiedad } from '../models/catalogo';

export const calculoGeneracionDocumentoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CalculoGeneracionDocumento | null>> {
    const response = await axiosClient.get<ApiResponse<CalculoGeneracionDocumento | null>>(
      `/api/CalculoGeneracionDocumento/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: CalculoGeneracionDocumento,
  ): Promise<ApiResponse<CalculoGeneracionDocumento>> {
    const response = await axiosClient.post<ApiResponse<CalculoGeneracionDocumento>>(
      `/api/CalculoGeneracionDocumento/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CalculoGeneracionDocumento/Avanzar/${id_expediente}`,
    );
    return response.data;
  },

  async calcularUF(fecha: string): Promise<ApiResponse<number>> {
    const response = await axiosClient.get<ApiResponse<number>>(
      `/api/CalculoGeneracionDocumento/CalcularUF`,
      { params: { fecha } },
    );
    return response.data;
  },

  async getControlesPropiedad(): Promise<ApiResponse<ControlesPropiedad>> {
    const response = await axiosClient.get<ApiResponse<ControlesPropiedad>>(
      '/api/DatosOperacion/GetControlesPropiedad',
    );
    return response.data;
  },
};
