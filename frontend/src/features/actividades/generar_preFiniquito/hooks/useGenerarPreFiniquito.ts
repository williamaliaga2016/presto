import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarPreFiniquito } from '../models/generar_preFiniquito';
import { generarPreFiniquitoService } from '../api/generarPreFiniquitoService';

export function useGenerarPreFiniquito(id_expediente: number) {
  return useQuery<ApiResponse<GenerarPreFiniquito | null>>({
    queryKey: ['generar_preFiniquito', id_expediente],
    queryFn: () => generarPreFiniquitoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}