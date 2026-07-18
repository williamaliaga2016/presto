import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarReparoCbr } from '../models/verificar_reparo_cbr';
import { verificarReparoCbrService } from '../api/verificarReparoCbrService';

export function useVerificarReparoCbr(id_expediente: number) {
  return useQuery<ApiResponse<VerificarReparoCbr | null>>({
    queryKey: ['verificar_reparo_cbr', id_expediente],
    queryFn: () => verificarReparoCbrService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
