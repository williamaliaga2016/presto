import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VoboGerenciaCohResponse } from '../models/vobo_gerencia_coh';
import { realizarVoboGerenciaCohService } from '../api/realizarVoboGerenciaCohService';

export function useVoboGerenciaCoh(id_expediente: number) {
  return useQuery<ApiResponse<VoboGerenciaCohResponse | null>>({
    queryKey: ['vobo_gerencia_coh', id_expediente],
    queryFn: () => realizarVoboGerenciaCohService.getByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
