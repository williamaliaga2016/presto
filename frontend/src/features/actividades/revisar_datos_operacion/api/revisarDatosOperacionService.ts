import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesComprador,
  ControlesCredito,
  ControlesPropiedad,
  ControlesRevisarDatosOperacionBanco,
  ControlesVendedor,
} from '../models/catalogo';
import {
  buildDatosOperacionEmpty,
  type RevisarDatosOperacion,
  type RevisarDatosOperacionBanco,
  type RevisarDatosOperacionComprador,
  type RevisarDatosOperacionCredito,
  type RevisarDatosOperacionFiadorGarante,
  type RevisarDatosOperacionPropiedad,
  type RevisarDatosOperacionVendedor,
} from '../models/revisar_datos_operacion';

const baseUrl = '/api/RevisarDatosOperacion';

const ok = <T>(response: ApiResponse<T> | null | undefined): T | null => {
  if (!response?.status) return null;
  return response.detail ?? null;
};

export const revisarDatosOperacionService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<RevisarDatosOperacion | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarDatosOperacion | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getPropiedadByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarDatosOperacionPropiedad | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarDatosOperacionPropiedad | null>>(
      `${baseUrl}/GetPropiedadByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getRevisarDatosOperacionBancoByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarDatosOperacionBanco | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarDatosOperacionBanco | null>>(
      `${baseUrl}/GetRevisarDatosOperacionByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getCreditoByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarDatosOperacionCredito | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarDatosOperacionCredito | null>>(
      `${baseUrl}/GetCreditoByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getVendedoresByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarDatosOperacionVendedor[]>> {
    const response = await axiosClient.get<ApiResponse<RevisarDatosOperacionVendedor[]>>(
      `${baseUrl}/GetVendedoresByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getCompradoresByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarDatosOperacionComprador[]>> {
    const response = await axiosClient.get<ApiResponse<RevisarDatosOperacionComprador[]>>(
      `${baseUrl}/GetCompradoresByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getFiadoresByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarDatosOperacionFiadorGarante[]>> {
    const response = await axiosClient.get<ApiResponse<RevisarDatosOperacionFiadorGarante[]>>(
      `${baseUrl}/GetFiadoresByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async getFullByExpediente(id_expediente: number): Promise<ApiResponse<RevisarDatosOperacion | null>> {
    const safe = <T,>(p: Promise<ApiResponse<T>>, fallback: T): Promise<ApiResponse<T>> =>
      p.catch(() => ({ status: false, detail: fallback, message: '' } as ApiResponse<T>));

    const [
      cabeceraResponse,
      creditoResponse,
      propiedadResponse,
      bancoResponse,
      compradoresResponse,
      vendedoresResponse,
      fiadoresResponse,
    ] = await Promise.all([
      safe(this.getByExpediente(id_expediente), null),
      safe(this.getCreditoByExpediente(id_expediente), null),
      safe(this.getPropiedadByExpediente(id_expediente), null),
      safe(this.getRevisarDatosOperacionBancoByExpediente(id_expediente), null),
      safe(this.getCompradoresByExpediente(id_expediente), []),
      safe(this.getVendedoresByExpediente(id_expediente), []),
      safe(this.getFiadoresByExpediente(id_expediente), []),
    ]);

    const cabecera = ok(cabeceraResponse) ?? buildDatosOperacionEmpty(id_expediente);
    const credito = ok(creditoResponse);
    const propiedad = ok(propiedadResponse);
    const banco = ok(bancoResponse);
    const compradores = compradoresResponse.status ? (compradoresResponse.detail ?? []) : [];
    const vendedores = vendedoresResponse.status ? (vendedoresResponse.detail ?? []) : [];
    const fiadores_garantes = fiadoresResponse.status ? (fiadoresResponse.detail ?? []) : [];

    const detail: RevisarDatosOperacion = {
      ...buildDatosOperacionEmpty(id_expediente),
      ...cabecera,
      id_expediente,
      credito: credito ?? cabecera.credito ?? null,
      propiedad: propiedad ?? cabecera.propiedad ?? null,
      revisar_datos_operacion_banco: banco ?? cabecera.revisar_datos_operacion_banco ?? null,
      compradores,
      vendedores,
      fiadores_garantes,
    };

    return {
      status: true,
      detail,
      message: cabeceraResponse.message ?? 'Revisar Datos Operación obtenido correctamente.',
    };
  },

  async getControlesPropiedad(): Promise<ApiResponse<ControlesPropiedad>> {
    const response = await axiosClient.get<ApiResponse<ControlesPropiedad>>(
      `${baseUrl}/GetControlesPropiedad`,
    );

    return response.data;
  },

  async getControlesVendedor(): Promise<ApiResponse<ControlesVendedor>> {
    const response = await axiosClient.get<ApiResponse<ControlesVendedor>>(
      `${baseUrl}/GetControlesVendedor`,
    );

    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesRevisarDatosOperacionBanco>> {
    const response = await axiosClient.get<ApiResponse<ControlesRevisarDatosOperacionBanco>>(
      `${baseUrl}/GetControlesRevisarDatosOperacionBanco`,
    );

    return response.data;
  },

  async getControlesCredito(): Promise<ApiResponse<ControlesCredito>> {
    const response = await axiosClient.get<ApiResponse<ControlesCredito>>(
      `${baseUrl}/GetControlesCredito`,
    );

    return response.data;
  },

  async getControlesComprador(): Promise<ApiResponse<ControlesComprador>> {
    const response = await axiosClient.get<ApiResponse<ControlesComprador>>(
      `${baseUrl}/GetControlesComprador`,
    );

    return response.data;
  },

  async saveComprador(
    payload: RevisarDatosOperacionComprador,
  ): Promise<ApiResponse<RevisarDatosOperacionComprador>> {
    const response = await axiosClient.post<ApiResponse<RevisarDatosOperacionComprador>>(
      `${baseUrl}/SaveComprador`,
      payload,
    );

    return response.data;
  },

  async deleteComprador(id: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.delete<ApiResponse<unknown>>(
      `${baseUrl}/DeleteComprador/${id}`,
    );

    return response.data;
  },

  async save(payload: RevisarDatosOperacion): Promise<ApiResponse<RevisarDatosOperacion>> {
    const response = await axiosClient.post<ApiResponse<RevisarDatosOperacion>>(
      `${baseUrl}/Save`,
      payload,
    );

    return response.data;
  },

  async saveVendedor(
    payload: RevisarDatosOperacionVendedor,
  ): Promise<ApiResponse<RevisarDatosOperacionVendedor>> {
    const response = await axiosClient.post<ApiResponse<RevisarDatosOperacionVendedor>>(
      `${baseUrl}/SaveVendedor`,
      payload,
    );

    return response.data;
  },

  async deleteVendedor(id: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.delete<ApiResponse<unknown>>(
      `${baseUrl}/DeleteVendedor/${id}`,
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
