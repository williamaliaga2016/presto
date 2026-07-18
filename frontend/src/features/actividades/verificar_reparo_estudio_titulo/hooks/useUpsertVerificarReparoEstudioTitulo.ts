import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarReparoEstudioTitulo } from '../models/verificar_reparo_estudio_titulo';
import { verificarReparoEstudioTituloService } from '../api/verificarReparoEstudioTituloService';

export function useUpsertVerificarReparoEstudioTitulo() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<VerificarReparoEstudioTitulo>, unknown, VerificarReparoEstudioTitulo>({
    mutationFn: (payload) => verificarReparoEstudioTituloService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['verificar_reparo_estudio_titulo', variables.id_expediente],
      });
    },
  });
}
