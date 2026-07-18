import { useQuery } from '@tanstack/react-query';

import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { consultActivityService } from '../api/consultActivityService';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

export function useCatalogoTipoBusqueda() {
  return useQuery<ApiResponse<ControlBaseDTO[]>>({
    queryKey: ['consult-activity', 'catalogo-tipo-busqueda'],
    queryFn: () => consultActivityService.getCatalogoTipoBusqueda(),
  });
}
