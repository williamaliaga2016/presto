import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoEntregarCarpeta } from '../models/corregir_reparo_entregar_carpeta';
import { corregirReparoEntregarCarpetaService } from '../api/corregirReparoEntregarCarpetaService';

export function useCorregirReparoEntregarCarpeta(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoEntregarCarpeta | null>>({
    queryKey: ['corregir_reparo_entregar_carpeta', id_expediente],
    queryFn: () => corregirReparoEntregarCarpetaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
