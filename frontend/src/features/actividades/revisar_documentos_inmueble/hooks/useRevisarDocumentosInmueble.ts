import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { encabezadoService } from
  '@/features/encabezado/api/encabezadoService';
import { validarInformacionService } from
  '../../validar_informacion/api/validarInformacionService';
import type {
  RevisarDocumentosInmuebleDetail,
} from '../models/revisar_documentos_inmueble';
import { revisarDocumentosInmuebleService } from
  '../api/revisarDocumentosInmuebleService';

const ACTIVITY_ID = 'ACT_REVISAR_DOCS';

export function useRevisarDocumentosInmueble(
  id_expediente: number,
) {
  return useQuery<ApiResponse<RevisarDocumentosInmuebleDetail>>({
    queryKey: [
      'revisar-documentos-inmueble',
      id_expediente,
    ],
    queryFn: async () => {
      const [
        formularioResponse,
        validarInformacionResponse,
        encabezadoResponse,
      ] = await Promise.all([
        revisarDocumentosInmuebleService
          .getByExpediente(id_expediente),

        validarInformacionService
          .getByExpediente(id_expediente),

        encabezadoService.getInfoEncabezado(
          id_expediente,
          ACTIVITY_ID,
        ),
      ]);

      const encabezado = encabezadoResponse.detail;
      const heredados = validarInformacionResponse.detail;

      return {
        status: formularioResponse.status,
        message: formularioResponse.message,
        detail: {
          informacion_general: {
            editable: false,
            responsable: encabezado?.responsable ?? null,
            actividad: encabezado?.actividad ?? null,
            estado: encabezado?.estado ?? null,
            usuario_asignado:
              encabezado?.usuario_asignado ?? null,
            fecha_alta: encabezado?.fecha_alta ?? null,
            fecha_asignacion:
              encabezado?.fecha_asignacion ?? null,
          },

          datos_heredados: {
            editable: false,

            datos_titular: {
              tipo_identificacion:
                heredados?.tipo_id_t1 ?? null,
              numero_identificacion:
                heredados?.numero_id_t1 ?? null,
              nombre_completo_t1:
                heredados?.nombre_completo_t1 ?? null,
              situacion_laboral:
                heredados?.situacion_laboral_t1 ?? null,
            },

            datos_inmueble: {
              tipo_inmueble:
                heredados?.tipo_inmueble ?? null,
              estado_inmueble:
                heredados?.estado_inmueble ?? null,
              constructora:
                heredados?.constructora ?? null,
              descripcion_proyecto:
                heredados?.descripcion_proyecto ?? null,
              departamento_inmueble:
                heredados?.departamento_inmueble ?? null,
              municipio_inmueble:
                heredados?.municipio_inmueble ?? null,
            },

            datos_credito: {
              tipo_credito:
                heredados?.tipo_credito ?? null,
              tiene_garantia:
                heredados?.tiene_garantia ?? null,
              monto_otorgado_vi:
                heredados?.monto_otorgado_vi ?? null,
            },

            condiciones_financieras: {
              scoring: encabezado?.id_scoring ?? null,
              subproducto:
                encabezado?.id_tipo_sub_producto ?? null,
              monto_otorgado:
                encabezado?.monto_otorgado ?? null,
              plazo_meses:
                encabezado?.plazo_otorgado == null
                  ? null
                  : Number(encabezado.plazo_otorgado),
              tasa:
                encabezado?.tasa == null
                  ? null
                  : Number(encabezado.tasa),
              condiciones_organismo_decisor:
                encabezado?.condiciones_organismo_decisor ??
                null,
            },
          },

          expediente_digital: [],

          documentos_obligatorios: {
            completos: true,
            faltantes: [],
          },

          formulario: formularioResponse.detail ?? {
            id: 0,
            id_expediente,
            id_actividad: ACTIVITY_ID,
            documentos_correctos: null,
            motivo_devolucion: null,
            observaciones: null,
            requiere_actualizacion_avaluos: null,
            homologacion: null,
            is_active: true,
            row_status: true,
          },
        },
      };
    },
    enabled: id_expediente > 0,
  });
}
