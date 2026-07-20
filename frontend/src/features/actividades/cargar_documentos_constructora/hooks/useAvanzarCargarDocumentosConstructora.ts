import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { cargarDocumentosConstructoraService } from '../api/cargarDocumentosConstructoraService';

export function useAvanzarCargarDocumentosConstructora() {
  return useMutation<ApiResponse<unknown>, unknown, number>({
    mutationFn: (id_expediente) => cargarDocumentosConstructoraService.avanzar(id_expediente),
  });
}
