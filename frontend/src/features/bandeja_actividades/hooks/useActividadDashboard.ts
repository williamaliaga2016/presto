import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ActividadDTO } from '../models/ActividadDTO';
import { actividadDashboardService } from '../api/actividadDashboardService';

export function useActividadDashboard() {
  return useQuery<ApiResponse<ActividadDTO[]>>({
    queryKey: ['actividad_dashboard'],
    queryFn: () => actividadDashboardService.getInfoActivityByUser(),
  });
}
