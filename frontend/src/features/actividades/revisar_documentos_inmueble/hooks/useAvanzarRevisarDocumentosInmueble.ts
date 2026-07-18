import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDocumentosInmuebleService } from '../api/revisarDocumentosInmuebleService';
import { revisarDocumentosInmuebleQueryKeys } from './useRevisarDocumentosInmueble';

export function useAvanzarRevisarDocumentosInmueble() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<unknown>, Error, number>({
    mutationFn: (id_expediente) =>
      revisarDocumentosInmuebleService.avanzar(id_expediente),
    onSuccess: (_, id_expediente) => {
      void queryClient.invalidateQueries({
        queryKey: revisarDocumentosInmuebleQueryKeys.actividad(id_expediente),
      });
    },
  });
}
