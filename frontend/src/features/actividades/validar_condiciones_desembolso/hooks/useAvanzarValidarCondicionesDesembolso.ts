import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validarCondicionesDesembolsoService } from '../api/validarCondicionesDesembolsoService';

export function useAvanzarValidarCondicionesDesembolso() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => validarCondicionesDesembolsoService.avanzar(id_expediente),
  });
}
