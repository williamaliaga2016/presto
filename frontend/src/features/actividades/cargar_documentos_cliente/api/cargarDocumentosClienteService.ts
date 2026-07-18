import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  CargarDocumentosCliente,
  CargarDocumentosClienteInfo,
} from '../models/cargar_documentos_cliente';

const baseUrl = '/api/cargar-documentos-cliente';

/**
 * Agrupa las llamadas HTTP requeridas por la actividad Cargar Documentos Cliente.
 */
export const cargarDocumentosClienteService = {
  /**
   * Consulta el registro guardado de confirmacion documental para el expediente.
   *
   * @param id_expediente Identificador del expediente en Presto.
   * @returns Respuesta API con el registro activo o `null`.
   */
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargarDocumentosCliente | null>> {
    const response = await axiosClient.get<ApiResponse<CargarDocumentosCliente | null>>(
      `${baseUrl}/${id_expediente}`,
    );
    return response.data;
  },

  /**
   * Consulta informacion general de cliente y analista para la pantalla externa.
   *
   * @param id_expediente Identificador del expediente en Presto.
   * @returns Respuesta API con informacion general de Cargar Documentos Cliente.
   */
  async getInfoCliente(
    id_expediente: number,
  ): Promise<ApiResponse<CargarDocumentosClienteInfo>> {
    const response = await axiosClient.get<ApiResponse<CargarDocumentosClienteInfo>>(
      `${baseUrl}/${id_expediente}/info-cliente`,
    );
    return response.data;
  },

  /**
   * Persiste la confirmacion documental sin avanzar el workflow.
   *
   * @param payload Datos de confirmacion y observaciones capturados en Cargar Documentos Cliente.
   * @returns Respuesta API con el registro creado o actualizado.
   */
  async guardar(
    payload: CargarDocumentosCliente,
  ): Promise<ApiResponse<CargarDocumentosCliente>> {
    const response = await axiosClient.post<ApiResponse<CargarDocumentosCliente>>(
      `${baseUrl}/guardar`,
      payload,
    );
    return response.data;
  },

  /**
   * Solicita el avance de Cargar Documentos Cliente hacia revision documental.
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
