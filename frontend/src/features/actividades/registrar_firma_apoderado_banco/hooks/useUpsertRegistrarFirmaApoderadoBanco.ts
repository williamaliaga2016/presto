import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarFirmaApoderadoBanco } from '../models/registrar_firma_apoderado_banco';
import { registrarFirmaApoderadoBancoService } from '../api/registrarFirmaApoderadoBancoService';

export function useUpsertRegistrarFirmaApoderadoBanco() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<RegistrarFirmaApoderadoBanco>, unknown, RegistrarFirmaApoderadoBanco>({
    mutationFn: (payload) => registrarFirmaApoderadoBancoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['registrar_firma_apoderado_banco', variables.id_expediente],
      });
    },
  });
}