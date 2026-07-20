import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { gestionarFirmaFisicaService } from '../api/gestionarFirmaFisicaService';

export function useAvanzarGestionarFirmaFisica() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) =>
      gestionarFirmaFisicaService.avanzar(id_expediente),
  });
}
