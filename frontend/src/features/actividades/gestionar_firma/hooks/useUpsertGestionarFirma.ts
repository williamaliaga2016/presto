import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionarFirma } from '../models/gestionar_firma';
import { gestionarFirmaService } from '../api/gestionarFirmaService';

export function useUpsertGestionarFirma() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GestionarFirma>, unknown, GestionarFirma>({
    mutationFn: (payload) => gestionarFirmaService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['gestionar_firma', variables.id_expediente],
      });
    },
  });
}
