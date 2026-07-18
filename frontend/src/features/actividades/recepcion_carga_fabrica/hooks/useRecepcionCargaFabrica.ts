import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecepcionCargaFabrica } from '../models/recepcion_carga_fabrica';
import { recepcionCargaFabricaService } from '../api/recepcionCargaFabricaService';

export function useRecepcionCargaFabrica(id_expediente: number) {
  return useQuery<ApiResponse<RecepcionCargaFabrica | null>>({
    queryKey: ['recepcion_carga_fabrica', id_expediente],
    queryFn: () => recepcionCargaFabricaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
