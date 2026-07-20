import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  CargaOperacionBanco,
  CargaOperacionBancoAntecedenteComprador,
  CargaOperacionBancoAntecedenteCredito,
  CargaOperacionBancoDatosComercial,
  CargaOperacionBancoDatosOperacion,
} from '../models/carga_operacion_banco';

const baseUrl = '/api/CargaOperacionBanco';

const buildEmptyCargaOperacionBanco = (
  id_expediente: number,
): CargaOperacionBanco => ({
  id_carga_operacion_banco: 0,
  id_expediente,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
  datos_operacion: {
    id_carga_operacion_banco_datos_operacion: 0,
    id_carga_operacion_banco: 0,
    id_expediente,
    is_active: true,
    row_status: true,
    created_by: 0,
    created_date: new Date().toISOString(),
    modified_by: null,
    modified_date: null,
  },
  antecedentes_comprador: [],
  antecedente_credito: null,
  datos_comercial: null,
});

export const cargaOperacionBancoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargaOperacionBanco | null>> {
    const response = await axiosClient.get<
      ApiResponse<CargaOperacionBanco | null>
    >(`${baseUrl}/GetByExpediente/${id_expediente}`);

    return response.data;
  },

  async getDatosOperacionByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargaOperacionBancoDatosOperacion | null>> {
    const response = await axiosClient.get<
      ApiResponse<CargaOperacionBancoDatosOperacion | null>
    >(`${baseUrl}/GetDatosOperacionByExpediente/${id_expediente}`);

    return response.data;
  },

  async getAntecedentesCompradorByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargaOperacionBancoAntecedenteComprador[] | null>> {
    const response = await axiosClient.get<
      ApiResponse<CargaOperacionBancoAntecedenteComprador[] | null>
    >(`${baseUrl}/GetAntecedentesCompradorByExpediente/${id_expediente}`);

    return response.data;
  },

  async getAntecedenteCreditoByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargaOperacionBancoAntecedenteCredito | null>> {
    const response = await axiosClient.get<
      ApiResponse<CargaOperacionBancoAntecedenteCredito | null>
    >(`${baseUrl}/GetAntecedenteCreditoByExpediente/${id_expediente}`);

    return response.data;
  },

  async getDatosComercialByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargaOperacionBancoDatosComercial | null>> {
    const response = await axiosClient.get<
      ApiResponse<CargaOperacionBancoDatosComercial | null>
    >(`${baseUrl}/GetDatosComercialByExpediente/${id_expediente}`);

    return response.data;
  },

  async getFullByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargaOperacionBanco | null>> {
    const [
      cabeceraResponse,
      datosOperacionResponse,
      antecedentesCompradorResponse,
      antecedenteCreditoResponse,
      datosComercialResponse,
    ] = await Promise.all([
      this.getByExpediente(id_expediente),
      this.getDatosOperacionByExpediente(id_expediente),
      this.getAntecedentesCompradorByExpediente(id_expediente),
      this.getAntecedenteCreditoByExpediente(id_expediente),
      this.getDatosComercialByExpediente(id_expediente),
    ]);

    const cabecera =
      cabeceraResponse.detail ?? buildEmptyCargaOperacionBanco(id_expediente);

    return {
      status:
        cabeceraResponse.status &&
        datosOperacionResponse.status &&
        antecedentesCompradorResponse.status &&
        antecedenteCreditoResponse.status &&
        datosComercialResponse.status,
      message:
        cabeceraResponse.message ??
        datosOperacionResponse.message ??
        antecedentesCompradorResponse.message ??
        antecedenteCreditoResponse.message ??
        datosComercialResponse.message ??
        'Carga Operación Banco obtenida correctamente.',
      detail: {
        ...cabecera,
        id_expediente,
        datos_operacion:
          datosOperacionResponse.detail ??
          cabecera.datos_operacion ??
          buildEmptyCargaOperacionBanco(id_expediente).datos_operacion,
        antecedentes_comprador:
          antecedentesCompradorResponse.detail ??
          cabecera.antecedentes_comprador ??
          [],
        antecedente_credito:
          antecedenteCreditoResponse.detail ??
          cabecera.antecedente_credito ??
          null,
        datos_comercial:
          datosComercialResponse.detail ??
          cabecera.datos_comercial ??
          null,
      },
    };
  },

  async save(
    payload: CargaOperacionBanco,
    idempotencyKey?: string,
  ): Promise<ApiResponse<CargaOperacionBanco>> {
    const headers: Record<string, string> = {};
    if (idempotencyKey) {
      headers['Idempotency-Key'] = idempotencyKey;
    }

    const response = await axiosClient.post<ApiResponse<CargaOperacionBanco>>(
      `${baseUrl}/Save`,
      payload,
      { headers },
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `${baseUrl}/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
