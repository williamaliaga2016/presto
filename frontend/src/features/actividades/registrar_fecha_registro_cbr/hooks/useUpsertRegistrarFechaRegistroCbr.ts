import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarFechaRegistroCbr } from '../models/registrar_fecha_registro_cbr';
import { registrarFechaRegistroCbrService } from '../api/registrarFechaRegistroCbrService';

export function useUpsertRegistrarFechaRegistroCbr() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RegistrarFechaRegistroCbr>, unknown, RegistrarFechaRegistroCbr>({
    mutationFn: (payload) => registrarFechaRegistroCbrService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['registrar_fecha_registro_cbr', variables.id_expediente],
      });
    },
  });
}
