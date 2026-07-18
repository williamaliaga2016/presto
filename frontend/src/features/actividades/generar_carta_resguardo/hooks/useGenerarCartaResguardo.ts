import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarCartaResguardo } from '../models/generar_carta_resguardo';
import { generarCartaResguardoService } from '../api/generarCartaResguardoService';

export function useGenerarCartaResguardo(id_expediente: number) {
  return useQuery<ApiResponse<GenerarCartaResguardo | null>>({
    queryKey: ['generar_carta_resguardo', id_expediente],
    queryFn: () => generarCartaResguardoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
