import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { GestionReparoService } from '../api/gestionReparoService';

export function useAvanzarGestionReparo() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => GestionReparoService.avanzar(id_expediente),
  });
}