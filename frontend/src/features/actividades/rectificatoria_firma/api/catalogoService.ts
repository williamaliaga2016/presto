import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesRectificatoriaFirma
} from '../models/catalogo';


const rectificatoriaFirmaBaseUrl = '/api/RectificatoriaFirma';

export const catalogoService = {
  async getControlesRectificatoriaFirma(): Promise<
    ApiResponse<ControlesRectificatoriaFirma>
  > {
    const response = await axiosClient.get<
      ApiResponse<ControlesRectificatoriaFirma>
    >(`${rectificatoriaFirmaBaseUrl}/GetControlesRectificatoriaFirma`);

    return response.data;
  },

  
};
