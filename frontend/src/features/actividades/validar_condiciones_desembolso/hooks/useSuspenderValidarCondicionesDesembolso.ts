import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validarCondicionesDesembolsoService } from '../api/validarCondicionesDesembolsoService';

export function useSuspenderValidarCondicionesDesembolso(id_expediente: number) {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<boolean>, unknown, void>({
    mutationFn: () => validarCondicionesDesembolsoService.suspender(id_expediente),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['validar_condiciones_desembolso', id_expediente] });
    },
  });
}
