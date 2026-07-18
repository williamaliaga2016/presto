import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarCorreccionEscritura } from '../models/verificar_correccion_escritura';
import { verificarCorreccionEscrituraService } from '../api/verificarCorreccionEscrituraService';

export function useVerificarCorreccionEscritura(id_expediente: number) {
  return useQuery<ApiResponse<VerificarCorreccionEscritura | null>>({
    queryKey: ['verificar_correccion_escritura', id_expediente],
    queryFn: () => verificarCorreccionEscrituraService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}