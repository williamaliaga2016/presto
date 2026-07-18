import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ValidacionRectificatoriaLegal,
  ValidacionRectificatoriaLegalDatosPersonales,
} from '../models/validacion_rectificatoria_legal';

const baseUrl = '/api/ValidacionRectificatoriaLegal';

const buildEmptyValidacionRectificatoriaLegal = (
  id_expediente: number,
): ValidacionRectificatoriaLegal => ({
  id_validacion_rectificatoria_legal: 0,
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
  validacion_rectificatoria_legal_datos_personales: [],
});

export const validacionRectificatoriaLegalService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidacionRectificatoriaLegal | null>> {
    const response = await axiosClient.get<
      ApiResponse<ValidacionRectificatoriaLegal | null>
    >(`${baseUrl}/GetByExpediente/${id_expediente}`);

    return response.data;
  },


  async getValidacionRectificatoriaLegalDatosPersonalesByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidacionRectificatoriaLegalDatosPersonales[] | null>> {
    const response = await axiosClient.get<
      ApiResponse<ValidacionRectificatoriaLegalDatosPersonales[] | null>
    >(`${baseUrl}/GetFaltantesFirmaByExpediente/${id_expediente}`);

    return response.data;
  },

  async getFullByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidacionRectificatoriaLegal | null>> {
    const [
      validacionRectificatoriaLegal,
      ValidacionRectificatoriaLegalDatosPersonales,
    ] = await Promise.all([
      this.getByExpediente(id_expediente),
      this.getValidacionRectificatoriaLegalDatosPersonalesByExpediente(id_expediente),
    ]);

    const cabecera =
      validacionRectificatoriaLegal.detail ?? buildEmptyValidacionRectificatoriaLegal(id_expediente);

    return {
      status:
        validacionRectificatoriaLegal.status &&
        ValidacionRectificatoriaLegalDatosPersonales.status,
      message:
        validacionRectificatoriaLegal.message ??
        ValidacionRectificatoriaLegalDatosPersonales.message ??
        'Carga Operación Banco obtenida correctamente.',
      detail: {
        ...cabecera,
        id_expediente,
        validacion_rectificatoria_legal_datos_personales:
          ValidacionRectificatoriaLegalDatosPersonales.detail ??
          cabecera.validacion_rectificatoria_legal_datos_personales ??
          buildEmptyValidacionRectificatoriaLegal(id_expediente).validacion_rectificatoria_legal_datos_personales,
      },
    };
  },


  async save(
    payload: ValidacionRectificatoriaLegal,
  ): Promise<ApiResponse<ValidacionRectificatoriaLegal>> {
    const response = await axiosClient.post<ApiResponse<ValidacionRectificatoriaLegal>>(
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
