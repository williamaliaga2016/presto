import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { cargarDocumentosConstructoraService } from '../api/cargarDocumentosConstructoraService';
import { cargarDocumentosConstructoraQueryKeys } from './useCargarDocumentosConstructora';

export function useAvanzarCargarDocumentosConstructora() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<unknown>, Error, number>({
    mutationFn: (id_expediente) =>
      cargarDocumentosConstructoraService.avanzar(id_expediente),
    onSuccess: (_, id_expediente) => {
      void queryClient.invalidateQueries({
        queryKey: cargarDocumentosConstructoraQueryKeys.actividad(id_expediente),
      });
    },
  });
}
