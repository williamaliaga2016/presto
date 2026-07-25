import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { excepcionDesembolsoService } from '../api/excepcionDesembolsoService';

export function useAvanzarExcepcionDesembolso() {
  return useMutation<
    ApiResponse<{ actividad_destino: string } | null>,
    unknown,
    number
  >({
    mutationFn: (id_expediente) =>
      excepcionDesembolsoService.avanzar(id_expediente),
  });
}
