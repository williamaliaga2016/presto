import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistroContactoBBVA } from '../models/registroContacto';
import type { ControlesRegistroContacto } from '../models/catalogo';

const baseUrl = '/api/registro-contacto';

export const registroContactoService = {
  async getControles(): Promise<ApiResponse<ControlesRegistroContacto>> {
    const response = await axiosClient.get<ApiResponse<ControlesRegistroContacto>>(
      `${baseUrl}/controles`,
    );
    return response.data;
  },

  async getRegistrosContacto(
    id_expediente: number,
  ): Promise<ApiResponse<RegistroContactoBBVA[]>> {
    const response = await axiosClient.get<ApiResponse<RegistroContactoBBVA[]>>(
      `${baseUrl}/${id_expediente}`,
    );
    return response.data;
  },

  /**
   * Persiste un contacto y conserva la actividad actual como trazabilidad.
   *
   * @param payload Datos visibles del modal de Registro Contacto.
   * @returns Registro creado por el backend.
   */
  async crearRegistroContacto(
    payload: RegistroContactoBBVA,
  ): Promise<ApiResponse<RegistroContactoBBVA>> {
    const response = await axiosClient.post<ApiResponse<RegistroContactoBBVA>>(
      baseUrl,
      payload,
    );
    return response.data;
  },
};
