import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesGenerarBorradorEscritura
} from '../models/catalogo';

/**
 * Los catálogos ya no se piden por tipo desde frontend.
 * El backend expone los controles por sección y resuelve los tipos usando Constants.Catalogos.
 */
const cargaOperacionBancoBaseUrl = '/api/GenerarBorradorEscritura';

export const catalogoService = {
  async getControlesGenerarBorradorEscritura(): Promise<
    ApiResponse<ControlesGenerarBorradorEscritura>
  > {
    const response = await axiosClient.get<
      ApiResponse<ControlesGenerarBorradorEscritura>
    >(`${cargaOperacionBancoBaseUrl}/GetControlesGenerarBorradorEscritura`);

    return response.data;
  },

  
};
