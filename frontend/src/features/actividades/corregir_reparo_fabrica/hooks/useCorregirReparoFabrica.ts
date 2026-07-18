import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoFabrica } from '../models/corregir_reparo_fabrica';
import { corregirReparoFabricaService } from '../api/corregirReparoFabricaService';

export function useCorregirReparoFabrica(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoFabrica | null>>({
    queryKey: ['corregir_reparo_fabrica', id_expediente],
    queryFn: () => corregirReparoFabricaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
