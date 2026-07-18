import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  DatosOperacion,
  DatosOperacionBancoAcreedor,
  DatosOperacionComprador,
  DatosOperacionDatosCredito,
  DatosOperacionFiadorGarante,
  DatosOperacionPropiedad,
  DatosOperacionVendedor,
} from '../models/datos_operacion';
import type {
  ControlesBancoAcreedor,
  ControlesComprador,
  ControlesDatosCredito,
  ControlesFiadorGarante,
  ControlesPropiedad,
  ControlesVendedor,
} from '../models/catalogo';
import {
  buildBancoAcreedorEmpty,
  buildDatosCreditoEmpty,
  buildDatosOperacionEmpty,
  buildPropiedadEmpty,
} from '../models/datos_operacion';


const baseUrl = '/api/DatosOperacion';

const ok = <T>(response: ApiResponse<T> | null | undefined): T | null => {
  if (!response?.status) return null;
  return response.detail ?? null;
};

export const datosOperacionService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<DatosOperacion | null>> {
    const response = await axiosClient.get<ApiResponse<DatosOperacion | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getDatosCreditoByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<DatosOperacionDatosCredito | null>> {
    const response = await axiosClient.get<ApiResponse<DatosOperacionDatosCredito | null>>(
      `${baseUrl}/GetDatosCreditoByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getCompradoresByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<DatosOperacionComprador[] | null>> {
    const response = await axiosClient.get<ApiResponse<DatosOperacionComprador[] | null>>(
      `${baseUrl}/GetCompradoresByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getVendedoresByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<DatosOperacionVendedor[] | null>> {
    const response = await axiosClient.get<ApiResponse<DatosOperacionVendedor[] | null>>(
      `${baseUrl}/GetVendedoresByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getFiadoresGarantesByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<DatosOperacionFiadorGarante[] | null>> {
    const response = await axiosClient.get<ApiResponse<DatosOperacionFiadorGarante[] | null>>(
      `${baseUrl}/GetFiadoresGarantesByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getBancoAcreedorByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<DatosOperacionBancoAcreedor | null>> {
    const response = await axiosClient.get<ApiResponse<DatosOperacionBancoAcreedor | null>>(
      `${baseUrl}/GetBancoAcreedorByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getPropiedadByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<DatosOperacionPropiedad | null>> {
    const response = await axiosClient.get<ApiResponse<DatosOperacionPropiedad | null>>(
      `${baseUrl}/GetPropiedadByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getFullByExpediente(id_expediente: number): Promise<ApiResponse<DatosOperacion | null>> {
    const [
      cabeceraResponse,
      datosCreditoResponse,
      compradoresResponse,
      vendedoresResponse,
      fiadoresResponse,
      bancoAcreedorResponse,
      propiedadResponse,
    ] = await Promise.all([
      this.getByExpediente(id_expediente),
      this.getDatosCreditoByExpediente(id_expediente),
      this.getCompradoresByExpediente(id_expediente),
      this.getVendedoresByExpediente(id_expediente),
      this.getFiadoresGarantesByExpediente(id_expediente),
      this.getBancoAcreedorByExpediente(id_expediente),
      this.getPropiedadByExpediente(id_expediente),
    ]);

    const cabecera = ok(cabeceraResponse) ?? buildDatosOperacionEmpty(id_expediente);

    const detail: DatosOperacion = {
      ...cabecera,
      id_expediente,
      datos_credito:
        ok(datosCreditoResponse) ??
        cabecera.datos_credito ??
        buildDatosCreditoEmpty(id_expediente, cabecera.id_datos_operacion),
      compradores: ok(compradoresResponse) ?? cabecera.compradores ?? [],
      vendedores: ok(vendedoresResponse) ?? cabecera.vendedores ?? [],
      fiadores_garantes: ok(fiadoresResponse) ?? cabecera.fiadores_garantes ?? [],
      banco_acreedor:
        ok(bancoAcreedorResponse) ??
        cabecera.banco_acreedor ??
        buildBancoAcreedorEmpty(id_expediente, cabecera.id_datos_operacion),
      propiedad:
        ok(propiedadResponse) ??
        cabecera.propiedad ??
        buildPropiedadEmpty(id_expediente, cabecera.id_datos_operacion),
    };

    return {
      status: true,
      detail,
      message: cabeceraResponse.message ?? 'Datos de Operación obtenidos correctamente.',
    };
  },


  async getControlesDatosCredito(): Promise<ApiResponse<ControlesDatosCredito>> {
    const response = await axiosClient.get<ApiResponse<ControlesDatosCredito>>(
      `${baseUrl}/GetControlesDatosCredito`,
    );

    return response.data;
  },

  async getControlesComprador(): Promise<ApiResponse<ControlesComprador>> {
    const response = await axiosClient.get<ApiResponse<ControlesComprador>>(
      `${baseUrl}/GetControlesComprador`,
    );

    return response.data;
  },

  async getControlesVendedor(): Promise<ApiResponse<ControlesVendedor>> {
    const response = await axiosClient.get<ApiResponse<ControlesVendedor>>(
      `${baseUrl}/GetControlesVendedor`,
    );

    return response.data;
  },

  async getControlesFiadorGarante(): Promise<ApiResponse<ControlesFiadorGarante>> {
    const response = await axiosClient.get<ApiResponse<ControlesFiadorGarante>>(
      `${baseUrl}/GetControlesFiadorGarante`,
    );

    return response.data;
  },

  async getControlesBancoAcreedor(): Promise<ApiResponse<ControlesBancoAcreedor>> {
    const response = await axiosClient.get<ApiResponse<ControlesBancoAcreedor>>(
      `${baseUrl}/GetControlesBancoAcreedor`,
    );

    return response.data;
  },

  async getControlesPropiedad(): Promise<ApiResponse<ControlesPropiedad>> {
    const response = await axiosClient.get<ApiResponse<ControlesPropiedad>>(
      `${baseUrl}/GetControlesPropiedad`,
    );

    return response.data;
  },

  async save(payload: DatosOperacion): Promise<ApiResponse<DatosOperacion>> {
    const response = await axiosClient.post<ApiResponse<DatosOperacion>>(
      `${baseUrl}/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.get<ApiResponse<unknown>>(
      `${baseUrl}/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
