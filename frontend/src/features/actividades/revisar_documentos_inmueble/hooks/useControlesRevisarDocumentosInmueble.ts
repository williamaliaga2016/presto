import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import { revisarDocumentosInmuebleService } from '../api/revisarDocumentosInmuebleService';
import { revisarDocumentosInmuebleQueryKeys } from './useRevisarDocumentosInmueble';

export function useControlesRevisarDocumentosInmueble(id_expediente: number) {
  return useQuery<ApiResponse<{ motivo_devolucion: ControlBaseDTO[] }>>({
    queryKey: revisarDocumentosInmuebleQueryKeys.controles(id_expediente),
    queryFn: () =>
      revisarDocumentosInmuebleService.getControles(id_expediente),
    enabled: id_expediente > 0,
  });
}
