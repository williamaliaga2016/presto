import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirReparoFabricaService } from '../api/corregirReparoFabricaService';

export function useAvanzarCorregirReparoFabrica() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoFabricaService.avanzar(id_expediente),
  });
}
