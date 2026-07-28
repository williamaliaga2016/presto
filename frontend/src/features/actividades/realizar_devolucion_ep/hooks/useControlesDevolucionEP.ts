import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarDevolucionEPService } from '../api/realizarDevolucionEPService';

export function useControlesDevolucionEP() {
  return useQuery<ApiResponse<any>>({
    queryKey: ['controles_devolucion_ep'],
    queryFn: () => realizarDevolucionEPService.getControles(),
  });
}
