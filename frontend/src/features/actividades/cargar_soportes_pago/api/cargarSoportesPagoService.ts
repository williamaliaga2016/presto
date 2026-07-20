import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  CargarSoportesPago,
  CargarSoportesPagoInfo,
} from '../models/cargar_soportes_pago';

const baseUrl = '/api/cargar-soportes-pago';

/**
 * Agrupa las llamadas HTTP requeridas por la actividad Cargar Soportes de Pago.
 */
export const cargarSoportesPagoService = {
  /**
   * Consulta el registro guardado de confirmacion documental para el expediente.
   *
   * @param id_expediente Identificador del expediente en Presto.
   * @returns Respuesta API con el registro activo o `null`.
   */
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargarSoportesPago | null>> {
    const response = await axiosClient.get<ApiResponse<CargarSoportesPago | null>>(
      `${baseUrl}/${id_expediente}`,
    );
    return response.data;
  },

  /**
   * Consulta informacion general de cliente y analista para la pantalla externa.
   *
   * @param id_expediente Identificador del expediente en Presto.
   * @returns Respuesta API con informacion general de Cargar Soportes de Pago.
   */
  async getInfoCliente(
    id_expediente: number,
  ): Promise<ApiResponse<CargarSoportesPagoInfo>> {
    const response = await axiosClient.get<ApiResponse<CargarSoportesPagoInfo>>(
      `${baseUrl}/${id_expediente}/info-cliente`,
    );
    return response.data;
  },

  /**
   * Persiste la confirmacion documental sin avanzar el workflow.
   *
   * @param payload Datos de confirmacion y observaciones capturados en Cargar Soportes de Pago.
   * @returns Respuesta API con el registro creado o actualizado.
   */
  async guardar(
    payload: CargarSoportesPago,
  ): Promise<ApiResponse<CargarSoportesPago>> {
    const response = await axiosClient.post<ApiResponse<CargarSoportesPago>>(
      `${baseUrl}/guardar`,
      payload,
    );
    return response.data;
  },

  /**
   * Solicita el avance de Cargar Soportes de Pago hacia Gestionar Firma.
   *
   * @param id_expediente Identificador del expediente en Presto.
   * @returns Respuesta API con el resultado del avance workflow.
   */
  async avanzar(id_expediente: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.post<ApiResponse<unknown>>(
      `${baseUrl}/${id_expediente}/avanzar`,
    );
    return response.data;
  },
};
