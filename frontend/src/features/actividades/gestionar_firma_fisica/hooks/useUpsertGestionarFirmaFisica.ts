import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionarFirmaFisica } from '../models/gestionar_firma_fisica';
import { gestionarFirmaFisicaService } from '../api/gestionarFirmaFisicaService';

export function useUpsertGestionarFirmaFisica() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<GestionarFirmaFisica>,
    unknown,
    GestionarFirmaFisica
  >({
    mutationFn: (payload) => gestionarFirmaFisicaService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['gestionar_firma_fisica', variables.id_expediente],
      });
    },
  });
}
