import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesValidacionRectificatoriaLegalPostventa
} from '../models/catalogo';

/**
 * Los catálogos ya no se piden por tipo desde frontend.
 * El backend expone los controles por sección y resuelve los tipos usando Constants.Catalogos.
 */
const validacionRectificatoriaLegalPostventaBaseUrl = '/api/ValidacionRectificatoriaLegalPostventa';

export const catalogoService = {
  async getControlesValidacionRectificatoriaLegalPostventa(): Promise<
    ApiResponse<ControlesValidacionRectificatoriaLegalPostventa>
  > {
    const response = await axiosClient.get<
      ApiResponse<ControlesValidacionRectificatoriaLegalPostventa>
    >(`${validacionRectificatoriaLegalPostventaBaseUrl}/GetControlesValidacionRectificatoriaLegalPostventa`);

    return response.data;
  },

};
