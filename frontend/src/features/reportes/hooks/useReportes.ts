import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ReporteDTO } from '../models/reporte';
import { getReportes } from '../api/reportesService';

export function useReportes() {
  return useQuery<ApiResponse<ReporteDTO[]>>({
    queryKey: ['reportes'],
    queryFn: () => getReportes(),
  });
}