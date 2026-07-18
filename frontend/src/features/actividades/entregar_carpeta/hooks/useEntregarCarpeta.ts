import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { EntregarCarpeta } from '../models/entregar_carpeta';
import { entregarCarpetaService } from '../api/entregarCarpetaService';

export function useEntregarCarpeta(id_expediente: number) {
  return useQuery<ApiResponse<EntregarCarpeta | null>>({
    queryKey: ['entregar_carpeta', id_expediente],
    queryFn: () => entregarCarpetaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
