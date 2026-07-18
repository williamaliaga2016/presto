import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { GestionRectificatoriaService } from '../api/gestionRectificatoriaService';

export function useAvanzarGestionRectificatoria() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => GestionRectificatoriaService.avanzar(id_expediente),
  });
}
