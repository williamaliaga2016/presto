import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CargarDocumentosConstructora } from '../models/cargar_documentos_constructora';
import { cargarDocumentosConstructoraService } from '../api/cargarDocumentosConstructoraService';
import { cargarDocumentosConstructoraQueryKeys } from './useCargarDocumentosConstructora';

export function useUpsertCargarDocumentosConstructora() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<CargarDocumentosConstructora>,
    Error,
    CargarDocumentosConstructora
  >({
    mutationFn: (payload) => cargarDocumentosConstructoraService.guardar(payload),
    onSuccess: (_, variables) => {
      void queryClient.invalidateQueries({
        queryKey: cargarDocumentosConstructoraQueryKeys.actividad(
          variables.id_expediente,
        ),
      });
    },
  });
}
