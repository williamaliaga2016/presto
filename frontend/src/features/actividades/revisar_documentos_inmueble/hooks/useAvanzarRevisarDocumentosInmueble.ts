import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDocumentosInmuebleService } from '../api/revisarDocumentosInmuebleService';

export function useAvanzarRevisarDocumentosInmueble() {
  return useMutation<ApiResponse<unknown>, Error, number>({
    mutationFn: (id_expediente) => revisarDocumentosInmuebleService.avanzar(id_expediente),
  });
}
