import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { registrarFirmaApoderadoBancoService } from '../api/registrarFirmaApoderadoBancoService';
export function useAvanzarRegistrarFirmaApoderadoBanco() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => registrarFirmaApoderadoBancoService.avanzar(id_expediente),
  });
}