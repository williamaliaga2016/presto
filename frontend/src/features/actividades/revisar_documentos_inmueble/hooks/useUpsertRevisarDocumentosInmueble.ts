import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarDocumentosInmuebleFormulario } from '../models/revisar_documentos_inmueble';
import { revisarDocumentosInmuebleService } from '../api/revisarDocumentosInmuebleService';

export function useUpsertRevisarDocumentosInmueble() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<RevisarDocumentosInmuebleFormulario>,
    Error,
    RevisarDocumentosInmuebleFormulario
  >({
    mutationFn: (payload) => revisarDocumentosInmuebleService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar-documentos-inmueble', variables.id_expediente],
      });
    },
  });
}