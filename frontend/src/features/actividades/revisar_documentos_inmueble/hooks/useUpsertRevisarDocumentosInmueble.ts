import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarDocumentosInmueble } from '../models/revisar_documentos_inmueble';
import { revisarDocumentosInmuebleService } from '../api/revisarDocumentosInmuebleService';
import { revisarDocumentosInmuebleQueryKeys } from './useRevisarDocumentosInmueble';

export function useUpsertRevisarDocumentosInmueble() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<RevisarDocumentosInmueble>,
    Error,
    RevisarDocumentosInmueble
  >({
    mutationFn: (payload) => revisarDocumentosInmuebleService.guardar(payload),
    onSuccess: (_, variables) => {
      void queryClient.invalidateQueries({
        queryKey: revisarDocumentosInmuebleQueryKeys.actividad(
          variables.id_expediente,
        ),
      });
    },
  });
}
