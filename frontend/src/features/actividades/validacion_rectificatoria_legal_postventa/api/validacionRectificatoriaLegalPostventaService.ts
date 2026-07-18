import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ValidacionRectificatoriaLegalPostventa,
  ValidacionRectificatoriaLegalPostventaDatosPersonales,
} from '../models/validacion_rectificatoria_legal_postventa';

const baseUrl = '/api/ValidacionRectificatoriaLegalPostventa';

const buildEmptyValidacionRectificatoriaLegalPostventa = (
  id_expediente: number,
): ValidacionRectificatoriaLegalPostventa => ({
  id_validacion_rectificatoria_legal_postventa: 0,
  id_expediente,
  id_usuario_solicitante:0,
  is_subsanar: false,
  fecha_ingreso: null,
  
  require_documentacion:null,
  realiza_pago: null,
  encargado_firma: false,
  requiere_inscripcion_cbr: false,
  observaciones: null,
  observaciones_reparo: null,
  solicitante: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
  antecedentes_comprador: [],
  antecedentes_vendedor: [],
  validacion_rectificatoria_legal_postventa_datos_personales: [],
});

export const validacionRectificatoriaLegalPostventaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidacionRectificatoriaLegalPostventa | null>> {
    const response = await axiosClient.get<
      ApiResponse<ValidacionRectificatoriaLegalPostventa | null>
    >(`${baseUrl}/GetByExpediente/${id_expediente}`);

    return response.data;
  },


  async getValidacionRectificatoriaLegalPostventaDatosPersonalesByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidacionRectificatoriaLegalPostventaDatosPersonales[] | null>> {
    const response = await axiosClient.get<
      ApiResponse<ValidacionRectificatoriaLegalPostventaDatosPersonales[] | null>
    >(`${baseUrl}/GetFaltantesFirmaByExpediente/${id_expediente}`);

    return response.data;
  },

  async getFullByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidacionRectificatoriaLegalPostventa | null>> {
    const [
      ValidacionRectificatoriaLegalPostventa,
      ValidacionRectificatoriaLegalPostventaDatosPersonales,
    ] = await Promise.all([
      this.getByExpediente(id_expediente),
      this.getValidacionRectificatoriaLegalPostventaDatosPersonalesByExpediente(id_expediente),
    ]);

    const cabecera =
      ValidacionRectificatoriaLegalPostventa.detail ?? buildEmptyValidacionRectificatoriaLegalPostventa(id_expediente);

    return {
      status:
        ValidacionRectificatoriaLegalPostventa.status &&
        ValidacionRectificatoriaLegalPostventaDatosPersonales.status,
      message:
        ValidacionRectificatoriaLegalPostventa.message ??
        ValidacionRectificatoriaLegalPostventaDatosPersonales.message ??
        'Carga Operación Banco obtenida correctamente.',
      detail: {
        ...cabecera,
        id_expediente,
        validacion_rectificatoria_legal_postventa_datos_personales:
          ValidacionRectificatoriaLegalPostventaDatosPersonales.detail ??
          cabecera.validacion_rectificatoria_legal_postventa_datos_personales ??
          buildEmptyValidacionRectificatoriaLegalPostventa(id_expediente).validacion_rectificatoria_legal_postventa_datos_personales,
      },
    };
  },


  async save(
    payload: ValidacionRectificatoriaLegalPostventa,
  ): Promise<ApiResponse<ValidacionRectificatoriaLegalPostventa>> {
    const response = await axiosClient.post<ApiResponse<ValidacionRectificatoriaLegalPostventa>>(
      `${baseUrl}/Save`,
      payload,
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
