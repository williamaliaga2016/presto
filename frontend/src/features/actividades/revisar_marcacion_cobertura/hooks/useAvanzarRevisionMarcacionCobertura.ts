import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarMarcacionCoberturaService } from '../api/revisarMarcacionCoberturaService';

export function useAvanzarRevisionMarcacionCobertura() {
  return useMutation<
    ApiResponse<{ actividad_destino: string } | null>,
    unknown,
    number
  >({
    mutationFn: (id_expediente) =>
      revisarMarcacionCoberturaService.avanzar(id_expediente),
  });
}
