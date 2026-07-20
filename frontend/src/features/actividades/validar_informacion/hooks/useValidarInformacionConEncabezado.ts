import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { encabezadoService } from '@/features/encabezado/api/encabezadoService';
import type { EncabezadoDTO } from '@/features/encabezado/models/encabezado';
import type {
  EncabezadoValidarInformacion,
  ValidarInformacionConEncabezadoDTO,
} from '../models/encabezado_validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

const ACTIVITY_ID = 'ACT_VALIDAR_INFO';

function mapEncabezado(
  source: EncabezadoDTO | null | undefined,
): EncabezadoValidarInformacion {
  return {
    scoring: source?.id_scoring ?? null,
    id_tipo_sub_producto: source?.id_tipo_sub_producto ?? null,
    monto_otorgado_original: source?.monto_otorgado ?? null,
    plazo_meses: null,
    tasa: null,
    condiciones_organismo_decisor:
      source?.condiciones_organismo_decisor ?? null,
    codigo_oficina: source?.codigo_oficina_bbva ?? null,
    descripcion_oficina: source?.descripcion_oficina_bbva ?? null,
    codigo_asesor: source?.codigo_asesor_bbva ?? null,
    correo_declarativo_original: source?.correo_declarativo ?? null,
    telefono_declarativo_original: source?.telefono_declarativo ?? null,
  };
}

export function useValidarInformacionConEncabezado(
  id_expediente: number,
) {
  return useQuery<ApiResponse<ValidarInformacionConEncabezadoDTO>>({
    queryKey: [
      'validar-informacion-con-encabezado',
      id_expediente,
    ],
    queryFn: async () => {
      const [formularioResponse, encabezadoResponse] =
        await Promise.all([
          validarInformacionService.getByExpediente(id_expediente),
          encabezadoService.getInfoEncabezado(
            id_expediente,
            ACTIVITY_ID,
          ),
        ]);

      return {
        status: formularioResponse.status,
        message: formularioResponse.message,
        detail: {
          formulario: formularioResponse.detail ?? {
            id: 0,
            id_expediente,
            id_actividad: ACTIVITY_ID,
          },
          encabezado: mapEncabezado(encabezadoResponse.detail),
        },
      };
    },
    enabled: id_expediente > 0,
  });
}
