// src/features/generar_borrador_escritura/hooks/useFiadoresGarantes.ts

import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FiadorGarante } from '../models/generarBorradorEscritura';
import { datosOperacionFiadorGaranteService } from '../api/datosOperacionFiadorGaranteService';

export const FIADORES_GARANTES_QUERY_KEY = 'fiadores_garantes';

/**
 * Hook para obtener fiadores/garantes por expediente
 */
export function useFiadoresGarantes(id_expediente: number, enabled = true) {
  return useQuery<ApiResponse<FiadorGarante[] | null>>({
    queryKey: [FIADORES_GARANTES_QUERY_KEY, id_expediente],
    queryFn: () => datosOperacionFiadorGaranteService.getFiadoresGarantesByExpediente(id_expediente),
    enabled: enabled && id_expediente > 0,
    staleTime: 1000 * 60 * 5, // 5 minutos
  });
}