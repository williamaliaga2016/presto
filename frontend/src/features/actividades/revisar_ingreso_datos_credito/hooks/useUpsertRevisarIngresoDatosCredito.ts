import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarIngresoDatosCredito } from '../models/revisar_ingreso_datos_credito';
import { revisarIngresoDatosCreditoService } from '../api/revisarIngresoDatosCreditoService';

export function useUpsertRevisarIngresoDatosCredito() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RevisarIngresoDatosCredito>, unknown, RevisarIngresoDatosCredito>({
    mutationFn: (payload) => revisarIngresoDatosCreditoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_ingreso_datos_credito', variables.id_expediente],
      });
    },
  });
}
