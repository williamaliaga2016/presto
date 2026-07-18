// src/features/generar_borrador_escritura/hooks/useGenerarBorradorEscritura.ts

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarBorradorEscritura } from '../models/generarBorradorEscritura';
import { generarBorradorEscrituraService } from '../api/generarBorradorEscrituraService';

export const GENERAR_BORRADOR_QUERY_KEY = 'generar_borrador_escritura';

/**
 * Hook para obtener los datos de Generar Borrador Escritura por expediente
 */
export function useGenerarBorradorEscritura(id_expediente: number, enabled = true) {
  return useQuery<ApiResponse<GenerarBorradorEscritura | null>>({
    queryKey: [GENERAR_BORRADOR_QUERY_KEY, id_expediente],
    queryFn: () => generarBorradorEscrituraService.getByExpediente(id_expediente),
    enabled: enabled && id_expediente > 0,
    staleTime: 1000 * 60 * 5,
  });
}

/**
 * Hook para guardar (crear o actualizar) Generar Borrador Escritura
 */
export function useSaveGenerarBorradorEscritura() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (payload: GenerarBorradorEscritura) =>
      generarBorradorEscrituraService.save(payload),
    onSuccess: (data, variables) => {
      if (variables.id_expediente) {
        queryClient.invalidateQueries({
          queryKey: [GENERAR_BORRADOR_QUERY_KEY, variables.id_expediente],
        });
      }
    },
  });
}

/**
 * Hook para avanzar la actividad
 */
export function useAvanzarGenerarBorradorEscritura() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id_expediente: number) =>
      generarBorradorEscrituraService.avanzar(id_expediente),
    onSuccess: (_, id_expediente) => {
      queryClient.invalidateQueries({
        queryKey: [GENERAR_BORRADOR_QUERY_KEY, id_expediente],
      });
    },
  });
}