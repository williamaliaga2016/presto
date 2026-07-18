import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AsignarEscritura } from '../models/asignar_escritura';
import { asignarEscrituraService } from '../api/asignarEscrituraService';

export function useAsignarEscritura(id_expediente: number) {
  return useQuery<ApiResponse<AsignarEscritura | null>>({
    queryKey: ['asignar_escritura', id_expediente],
    queryFn: () => asignarEscrituraService.getByIdExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
