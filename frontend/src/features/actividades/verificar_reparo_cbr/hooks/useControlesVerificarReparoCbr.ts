import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { verificarReparoCbrService } from '../api/verificarReparoCbrService';
import type { ControlesVerificarReparoCbr } from '../models/catalogo';

export function useControlesVerificarReparoCbr(enabled = true) {
  return useQuery<ApiResponse<ControlesVerificarReparoCbr>>({
    queryKey: ['controles_verificar_reparo_cbr'],
    queryFn: () => verificarReparoCbrService.getControlesVerificarReparoCbr(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
