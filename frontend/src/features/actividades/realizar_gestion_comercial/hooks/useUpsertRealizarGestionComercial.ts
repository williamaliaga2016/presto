import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarGestionComercial } from '../models/realizar_gestion_comercial';
import { realizarGestionComercialService } from '../api/realizarGestionComercialService';

export function useUpsertRealizarGestionComercial() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<RealizarGestionComercial>, unknown, RealizarGestionComercial>({
    mutationFn: (payload) => realizarGestionComercialService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['realizar_gestion_comercial', variables.id_expediente] });
    },
  });
}
