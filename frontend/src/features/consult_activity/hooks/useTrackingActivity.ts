import { useQuery } from '@tanstack/react-query';

import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { consultActivityService } from '../api/consultActivityService';
import type { EtapaDTO } from '../models/ConsultActivity';

export function useTrackingActivity(id_expediente: number) {
  return useQuery<ApiResponse<EtapaDTO[]>>({
    queryKey: ['consult-activity', 'tracking', id_expediente],
    queryFn: () => consultActivityService.getConsultTrackingActivity(id_expediente),
    enabled: id_expediente > 0,
  });
}
