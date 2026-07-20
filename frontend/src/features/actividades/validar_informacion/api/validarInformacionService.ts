import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionBBVA } from
  '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';
import type { ValidarInformacionConEncabezadoDTO } from
  '../models/encabezado_validar_informacion';
import type { AvanzarValidarInformacionResponse } from
  '../models/avanzar_validar_informacion';

const baseUrl = '/api/validar-informacion';

export const validarInformacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidarInformacionBBVA | null>> {
    const response = await axiosClient.get<ApiResponse<ValidarInformacionBBVA | null>>(
      `${baseUrl}/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(
    payload: ValidarInformacionBBVA,
  ): Promise<ApiResponse<ValidarInformacionBBVA>> {
    const response = await axiosClient.post<ApiResponse<ValidarInformacionBBVA>>(
      `${baseUrl}/guardar`,
      payload,
    );
    return response.data;
  },

  /**
   * Avanza Validar Informacion y retorna el link temporal cuando el backend lo genera.
   *
   * @param id_expediente - Expediente que se debe avanzar.
   * @returns Resultado del workflow y acceso temporal opcional.
   */
  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<AvanzarValidarInformacionResponse>> {
    const response = await axiosClient.post<
      ApiResponse<AvanzarValidarInformacionResponse>
    >(
      `${baseUrl}/${id_expediente}/avanzar`,
    );
    return response.data;
  },

  async getConEncabezado(
    id_expediente: number,
  ): Promise<ApiResponse<ValidarInformacionConEncabezadoDTO>> {
    const response = await axiosClient.get<ApiResponse<ValidarInformacionConEncabezadoDTO>>(
      `${baseUrl}/${id_expediente}/con-encabezado`,
    );
    return response.data;
  },

  async getControles(
    id_expediente: number,
  ): Promise<ApiResponse<ControlesValidarInformacion>> {
    const response = await axiosClient.get<ApiResponse<ControlesValidarInformacion>>(
      `${baseUrl}/${id_expediente}/controles`,
    );
    return response.data;
  },

};
