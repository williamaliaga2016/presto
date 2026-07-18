import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosDelVendedorService } from '../api/datosDelVendedorService';
import type { DatosOperacion } from '@/features/actividades/datos_operacion/models/datos_operacion';

export function useUpsertDatosDelVendedor() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<DatosOperacion>, unknown, DatosOperacion>({
    mutationFn: (payload) => datosDelVendedorService.saveVendedores(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['datos_del_vendedor_5_11_3', variables.id_expediente],
      });
      queryClient.invalidateQueries({
        queryKey: ['datos_operacion_5_4', variables.id_expediente],
      });
    },
  });
}
