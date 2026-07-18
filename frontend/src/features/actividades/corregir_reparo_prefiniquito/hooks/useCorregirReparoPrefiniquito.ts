import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoPrefiniquito } from '../models/corregir_reparo_prefiniquito';
import { corregirReparoPrefiniquitoService } from '../api/corregirReparoPrefiniquitoService';

export function useCorregirReparoPrefiniquito(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoPrefiniquito | null>>({
    queryKey: ['corregir_reparo_prefiniquito', id_expediente],
    queryFn: () => corregirReparoPrefiniquitoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
