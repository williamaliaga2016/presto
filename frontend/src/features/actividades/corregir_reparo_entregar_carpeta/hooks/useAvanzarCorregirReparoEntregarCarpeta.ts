import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirReparoEntregarCarpetaService } from '../api/corregirReparoEntregarCarpetaService';

export function useAvanzarCorregirReparoEntregarCarpeta() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoEntregarCarpetaService.avanzar(id_expediente),
  });
}
