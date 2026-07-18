import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesValidacionRectificatoriaLegal
} from '../models/catalogo';

/**
 * Los catálogos ya no se piden por tipo desde frontend.
 * El backend expone los controles por sección y resuelve los tipos usando Constants.Catalogos.
 */
const validacionRectificatoriaLegalBaseUrl = '/api/ValidacionRectificatoriaLegal';

export const catalogoService = {
  async getControlesValidacionRectificatoriaLegal(): Promise<
    ApiResponse<ControlesValidacionRectificatoriaLegal>
  > {
    const response = await axiosClient.get<
      ApiResponse<ControlesValidacionRectificatoriaLegal>
    >(`${validacionRectificatoriaLegalBaseUrl}/GetControlesValidacionRectificatoriaLegal`);

    return response.data;
  },

};
