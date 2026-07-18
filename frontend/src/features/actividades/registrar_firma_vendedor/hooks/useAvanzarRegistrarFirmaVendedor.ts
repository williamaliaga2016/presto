import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { registrarFirmaVendedorService } from '../api/registrarFirmaVendedorService';

export function useAvanzarRegistrarFirmaVendedor() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => registrarFirmaVendedorService.avanzar(id_expediente),
  });
}
