import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { registrarFirmaCompradorService } from '../api/registrarFirmaCompradorService';

export function useAvanzarRegistrarFirmaComprador() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => registrarFirmaCompradorService.avanzar(id_expediente),
  });
}
