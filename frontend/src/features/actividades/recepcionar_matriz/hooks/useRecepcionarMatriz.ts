import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecepcionarMatriz } from '../models/recepcionar_matriz';
import { recepcionarMatrizService } from '../api/recepcionarMatrizService';

export function useRecepcionarMatriz(id_expediente: number) {
  return useQuery<ApiResponse<RecepcionarMatriz | null>>({
    queryKey: ['recepcionar_matriz', id_expediente],
    queryFn: () => recepcionarMatrizService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
