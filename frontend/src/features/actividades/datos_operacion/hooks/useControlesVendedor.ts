import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '../api/datosOperacionService';
import type { ControlesVendedor } from '../models/catalogo';

export function useControlesVendedor(enabled = true) {
  return useQuery<ApiResponse<ControlesVendedor>>({
    queryKey: ['datos_operacion_controles_vendedor'],
    queryFn: () => datosOperacionService.getControlesVendedor(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
