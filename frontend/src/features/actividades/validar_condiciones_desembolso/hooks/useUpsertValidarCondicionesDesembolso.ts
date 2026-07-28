import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarCondicionesDesembolso } from '../models/validar_condiciones_desembolso';
import { validarCondicionesDesembolsoService } from '../api/validarCondicionesDesembolsoService';

export function useUpsertValidarCondicionesDesembolso() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<ValidarCondicionesDesembolso>, unknown, ValidarCondicionesDesembolso>({
    mutationFn: (payload) => validarCondicionesDesembolsoService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['validar_condiciones_desembolso', variables.id_expediente] });
    },
  });
}
