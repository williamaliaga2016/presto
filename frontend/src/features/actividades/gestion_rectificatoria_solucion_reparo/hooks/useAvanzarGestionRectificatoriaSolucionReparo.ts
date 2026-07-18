import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { GestionRectificatoriaSolucionReparoService } from '../api/gestionRectificatoriaSolucionReparoService';

export function useAvanzarGestionRectificatoriaSolucionReparo() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => GestionRectificatoriaSolucionReparoService.avanzar(id_expediente),
  });
}