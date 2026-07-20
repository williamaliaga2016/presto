import { useQuery } from '@tanstack/react-query';
import { encabezadoService } from
  '@/features/encabezado/api/encabezadoService';
import { validarInformacionService } from
  '../../validar_informacion/api/validarInformacionService';
import { validarIntegracionDocumentosService } from
  '../api/validarIntegracionDocumentosService';

const ACTIVITY_ID = 'ACT_VALIDAR_INTEGRACION';

export function useValidarIntegracionDocumentos(
  id_expediente: number,
) {
  return useQuery({
    queryKey: [
      'validarIntegracionDocumentos',
      id_expediente,
    ],
    queryFn: async () => {
      const [
        formularioResponse,
        validarInformacionResponse,
        encabezadoResponse,
      ] = await Promise.all([
        validarIntegracionDocumentosService
          .getByExpediente(id_expediente),
        validarInformacionService
          .getByExpediente(id_expediente),
        encabezadoService.getInfoEncabezado(
          id_expediente,
          ACTIVITY_ID,
        ),
      ]);

      return {
        ...formularioResponse,
        detail: {
          id_expediente,
          encabezado: encabezadoResponse.detail ?? {},
          formulario: formularioResponse.detail ?? {
            id: 0,
            id_expediente,
            id_actividad: ACTIVITY_ID,
            documentos_correctos: null,
            credito_condicionado: false,
            motivo_devolucion: '',
            observaciones: '',
          },
          validar_informacion_data:
            validarInformacionResponse.detail ?? {
              id_expediente,
            },
        },
      };
    },
    enabled: id_expediente > 0,
  });
}
