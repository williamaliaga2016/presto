import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoTasacion } from '../models/corregir_reparo_tasacion';
import { corregirReparoTasacionService } from '../api/corregirReparoTasacionService';

export function useCorregirReparoTasacion(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoTasacion | null>>({
    queryKey: ['corregir_reparo_tasacion', id_expediente],
    queryFn: () => corregirReparoTasacionService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
