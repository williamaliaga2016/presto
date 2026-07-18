import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosDelVendedorService } from '../api/datosDelVendedorService';

export function useAvanzarDatosDelVendedor() {
  return useMutation<ApiResponse<unknown>, unknown, number>({
    mutationFn: (id_expediente) => datosDelVendedorService.avanzar(id_expediente),
  });
}
