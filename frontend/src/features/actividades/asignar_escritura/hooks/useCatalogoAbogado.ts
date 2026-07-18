import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import { asignarEscrituraService } from '../api/asignarEscrituraService';

export function useCatalogoAbogado() {
  return useQuery<ApiResponse<ControlBaseDTO[]>>({
    queryKey: ['asignar_escritura', 'catalogo_abogado'],
    queryFn: () => asignarEscrituraService.getCatalogoAbogado(),
  });
}
