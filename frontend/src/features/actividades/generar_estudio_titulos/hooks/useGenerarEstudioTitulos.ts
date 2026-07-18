import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarEstudioTitulos } from '../models/generar_estudio_titulos';
import { generarEstudioTitulosService } from '../api/generarEstudioTitulosService';

export function useGenerarEstudioTitulos(id_expediente: number) {
  return useQuery<ApiResponse<GenerarEstudioTitulos | null>>({
    queryKey: ['generar_estudio_titulos', id_expediente],
    queryFn: () => generarEstudioTitulosService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}