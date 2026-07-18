import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesAntecedenteComprador,
  ControlesAntecedenteCredito,
  ControlesDatosOperacion,
} from '../models/catalogo';

/**
 * Los catálogos ya no se piden por tipo desde frontend.
 * El backend expone los controles por sección y resuelve los tipos usando Constants.Catalogos.
 */
const cargaOperacionBancoBaseUrl = '/api/CargaOperacionBanco';

export const catalogoService = {
  async getControlesDatosOperacion(): Promise<
    ApiResponse<ControlesDatosOperacion>
  > {
    const response = await axiosClient.get<
      ApiResponse<ControlesDatosOperacion>
    >(`${cargaOperacionBancoBaseUrl}/GetControlesDatosOperacion`);

    return response.data;
  },

  async getControlesAntecedenteComprador(): Promise<
    ApiResponse<ControlesAntecedenteComprador>
  > {
    const response = await axiosClient.get<
      ApiResponse<ControlesAntecedenteComprador>
    >(`${cargaOperacionBancoBaseUrl}/GetControlesAntecedenteComprador`);

    return response.data;
  },

  async getControlesAntecedenteCredito(): Promise<
    ApiResponse<ControlesAntecedenteCredito>
  > {
    const response = await axiosClient.get<
      ApiResponse<ControlesAntecedenteCredito>
    >(`${cargaOperacionBancoBaseUrl}/GetControlesAntecedenteCredito`);

    return response.data;
  },
};
