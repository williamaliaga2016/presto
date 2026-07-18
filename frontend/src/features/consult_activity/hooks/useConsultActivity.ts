import { useMutation } from '@tanstack/react-query';

import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { consultActivityService } from '../api/consultActivityService';
import type {
  ConsultActivityDTO,
  SearchCriteriaDTO,
} from '../models/ConsultActivity';

export function useConsultActivity() {
  return useMutation<
    ApiResponse<ConsultActivityDTO[]>,
    unknown,
    SearchCriteriaDTO
  >({
    mutationFn: (payload) => consultActivityService.getConsultActivity(payload),
  });
}
