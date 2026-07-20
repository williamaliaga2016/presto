import {
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { encabezadoService } from
  '@/features/encabezado/api/encabezadoService';
import { validarInformacionService } from
  '../../validar_informacion/api/validarInformacionService';
import type {
  AsignarFirmasConEncabezado,
  AsignarFirmasForm,
  CalcularAsignacionRequest,
} from '../models/asignarFirmas';
import { asignarFirmasService } from
  '../api/asignarFirmasService';

const ACTIVITY_ID = 'ACT_ASIGNAR_FIRMAS';

export function useAsignarFirmas(id_expediente: number) {
  return useQuery({
    queryKey: ['asignar-firmas', id_expediente],
    queryFn: async () => {
      const [
        formularioResponse,
        validarInformacionResponse,
        encabezadoResponse,
      ] = await Promise.all([
        asignarFirmasService.getByExpediente(id_expediente),
        validarInformacionService.getByExpediente(id_expediente),
        encabezadoService.getInfoEncabezado(
          id_expediente,
          ACTIVITY_ID,
        ),
      ]);

      const encabezado = encabezadoResponse.detail;

      const detail: AsignarFirmasConEncabezado = {
        formulario: formularioResponse.detail,
        datos_heredados:
          validarInformacionResponse.detail ?? {
            id_expediente,
          },
        encabezado: {
          scoring: encabezado?.id_scoring ?? null,
          id_tipo_sub_producto:
            encabezado?.id_tipo_sub_producto ?? null,
          monto_otorgado_original:
            encabezado?.monto_otorgado ?? null,
          condiciones_organismo_decisor:
            encabezado?.condiciones_organismo_decisor ?? null,
          codigo_oficina:
            encabezado?.codigo_oficina_bbva ?? null,
          descripcion_oficina:
            encabezado?.descripcion_oficina_bbva ?? null,
          codigo_asesor:
            encabezado?.codigo_asesor_bbva ?? null,
          correo_declarativo_original:
            encabezado?.correo_declarativo ?? null,
          telefono_declarativo_original:
            encabezado?.telefono_declarativo ?? null,
        },
        datos_folio: {
          fecha_asignacion:
            encabezado?.fecha_asignacion ?? null,
          tipo_inmueble:
            encabezado?.tipo_inmueble ?? null,
          constructora:
            validarInformacionResponse.detail?.constructora ?? null,
          proyecto:
            encabezado?.descripcion_proyecto ?? null,
          identificacion_cliente:
            encabezado?.numero_identificacion_t1 ?? null,
          nombre_cliente:
            encabezado?.nombre_completo_t1 ?? null,
          departamento_predio:
            validarInformacionResponse.detail
              ?.departamento_inmueble ?? null,
          ciudad_predio:
            validarInformacionResponse.detail
              ?.municipio_inmueble ?? null,
          direccion_predio:
            validarInformacionResponse.detail
              ?.direccion_inmueble ?? null,
          valor_comercial_predio:
            validarInformacionResponse.detail
              ?.valor_comercial_inmueble ?? null,
          usuario_solicitante:
            encabezado?.usuario_asignado ?? null,
        },
      };

      return {
        ...formularioResponse,
        detail,
      };
    },
    enabled: id_expediente > 0,
  });
}

export function useControlesAsignarFirmas() {
  return useQuery({
    queryKey: ['asignar-firmas-controles'],
    queryFn: () => asignarFirmasService.getControles(),
  });
}

export function useGuardarAsignarFirmas() {
  const client = useQueryClient();

  return useMutation({
    mutationFn: (payload: AsignarFirmasForm) =>
      asignarFirmasService.guardar(payload),
    onSuccess: (_, payload) =>
      client.invalidateQueries({
        queryKey: [
          'asignar-firmas',
          payload.id_expediente,
        ],
      }),
  });
}

export function useCalcularAsignacion(
  id_expediente: number,
) {
  return useMutation({
    mutationFn: (payload: CalcularAsignacionRequest) =>
      asignarFirmasService.calcular(
        id_expediente,
        payload,
      ),
  });
}

export function useAvanzarAsignarFirmas() {
  return useMutation({
    mutationFn: (id_expediente: number) =>
      asignarFirmasService.avanzar(id_expediente),
  });
}
