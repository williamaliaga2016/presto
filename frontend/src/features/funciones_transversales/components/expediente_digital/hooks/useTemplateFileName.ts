import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { expedienteDigitalService } from '../api/expedienteDigitalService';

export function useTemplateFileName(
  id_cat_expediente_digital: number,
  id_cat_expediente_digital_documento: number,
) {
  return useQuery<ApiResponse<string>>({
    queryKey: [
      'expediente_digital',
      'template',
      id_cat_expediente_digital,
      id_cat_expediente_digital_documento,
    ],
    queryFn: () =>
      expedienteDigitalService.getTemplateFileName(
        id_cat_expediente_digital,
        id_cat_expediente_digital_documento,
      ),
    enabled:
      id_cat_expediente_digital > 0 &&
      id_cat_expediente_digital_documento > 0,
  });
}
