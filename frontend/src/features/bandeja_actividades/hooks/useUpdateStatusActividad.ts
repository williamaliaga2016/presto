import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { actividadDashboardService } from '../api/actividadDashboardService';
import type { ActividadDTO } from '../models/ActividadDTO';

export function useUpdateStatusActividad() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ActividadDTO>, Error, ActividadDTO>({
    mutationFn: (actividad) => actividadDashboardService.updateStatus(actividad),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['actividad_dashboard'] });
    },
  });
}
