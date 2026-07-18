import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { ControlesVendedor } from '../models/catalogo';

export function useControlesVendedor(enabled = true) {
  return useQuery<ApiResponse<ControlesVendedor>>({
    queryKey: ['revisar_datos_operacion_controles_vendedor'],
    queryFn: () => revisarDatosOperacionService.getControlesVendedor(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
