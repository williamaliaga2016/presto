import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from
  '@/shared/models/ControlBaseDTO';
import { revisarDocumentosInmuebleService } from
  '../api/revisarDocumentosInmuebleService';

export function useControlesRevisarDocumentosInmueble(
  id_expediente: number,
) {
  return useQuery<
    ApiResponse<{ motivo_devolucion: ControlBaseDTO[] }>
  >({
    queryKey: [
      'revisar-documentos-inmueble-controles',
      id_expediente,
    ],
    queryFn: () =>
      revisarDocumentosInmuebleService.getControles(
        id_expediente,
      ),
    enabled: id_expediente > 0,
    staleTime: 1000 * 60 * 10,
  });
}
