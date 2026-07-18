import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparosGestor } from '../models/corregir_reparos_gestor';
import { corregirReparosGestorService } from '../api/corregirReparosGestorService';

export function useCorregirReparosGestor(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparosGestor | null>>({
    queryKey: ['corregir_reparos_gestor', id_expediente],
    queryFn: () => corregirReparosGestorService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
