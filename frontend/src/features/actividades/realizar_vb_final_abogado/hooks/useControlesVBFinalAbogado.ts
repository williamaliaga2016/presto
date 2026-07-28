import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesVBFinalAbogado } from '../models/controles';
import { realizarVBFinalAbogadoService } from '../api/realizarVBFinalAbogadoService';

export function useControlesVBFinalAbogado() {
  return useQuery<ApiResponse<ControlesVBFinalAbogado>>({
    queryKey: ['controles_vb_final_abogado'],
    queryFn: () => realizarVBFinalAbogadoService.getControles(),
  });
}
