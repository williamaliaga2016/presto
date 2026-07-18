import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarEscrituraCbr } from '../models/registrar_escritura_cbr';
import { registrarEscrituraCbrService } from '../api/registrarEscrituraCbrService';

export function useRegistrarEscrituraCbr(id_expediente: number) {
  return useQuery<ApiResponse<RegistrarEscrituraCbr | null>>({
    queryKey: ['registrar_escritura_cbr', id_expediente],
    queryFn: () => registrarEscrituraCbrService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
