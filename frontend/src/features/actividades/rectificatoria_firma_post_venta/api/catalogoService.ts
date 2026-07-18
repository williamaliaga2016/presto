import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesRectificatoriaFirmaPostVenta
} from '../models/catalogo';


const rectificatoriaFirmaPostVentaBaseUrl = '/api/RectificatoriaFirmaPostVenta';

export const catalogoService = {
  async getControlesRectificatoriaFirmaPostVenta(): Promise<
    ApiResponse<ControlesRectificatoriaFirmaPostVenta>
  > {
    const response = await axiosClient.get<
      ApiResponse<ControlesRectificatoriaFirmaPostVenta>
    >(`${rectificatoriaFirmaPostVentaBaseUrl}/GetControlesRectificatoriaFirmaPostVenta`);

    return response.data;
  },

  
};
