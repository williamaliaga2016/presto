import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { gestionarFirmaService } from '../api/gestionarFirmaService';

export function useAvanzarGestionarFirma() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => gestionarFirmaService.avanzar(id_expediente),
  });
}
