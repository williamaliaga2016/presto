import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CargarDocumentosConstructora } from '../models/cargar_documentos_constructora';
import { cargarDocumentosConstructoraService } from '../api/cargarDocumentosConstructoraService';

export function useUpsertCargarDocumentosConstructora() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CargarDocumentosConstructora>, unknown, CargarDocumentosConstructora>({
    mutationFn: (payload) => cargarDocumentosConstructoraService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['cargar_documentos_constructora', variables.id_expediente],
      });
    },
  });
}
