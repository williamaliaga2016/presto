import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirNotariaReparoAbogados } from '../models/corregir_notaria_reparo_abogados';

export const corregirNotariaReparoAbogadosService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirNotariaReparoAbogados | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirNotariaReparoAbogados | null>>(
      `/api/CorregirNotariaReparoAbogados/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirNotariaReparoAbogados,
  ): Promise<ApiResponse<CorregirNotariaReparoAbogados>> {
    const response = await axiosClient.post<ApiResponse<CorregirNotariaReparoAbogados>>(
      `/api/CorregirNotariaReparoAbogados/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirNotariaReparoAbogados/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
