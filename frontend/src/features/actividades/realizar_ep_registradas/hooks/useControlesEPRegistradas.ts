import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesEPRegistradas } from '../models/controles';
import { realizarEPRegistradasService } from '../api/realizarEPRegistradasService';

export function useControlesEPRegistradas() {
  return useQuery<ApiResponse<ControlesEPRegistradas>>({
    queryKey: ['controles_ep_registradas'],
    queryFn: () => realizarEPRegistradasService.getControles(),
  });
}
