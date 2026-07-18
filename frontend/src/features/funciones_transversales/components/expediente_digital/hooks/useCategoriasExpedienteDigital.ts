import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { expedienteDigitalService } from '../api/expedienteDigitalService';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

export function useCategoriasExpedienteDigital() {
  return useQuery<ApiResponse<ControlBaseDTO[]>>({
    queryKey: ['expediente_digital', 'categorias'],
    queryFn: () => expedienteDigitalService.getControlCatExpedienteDigital(),
  });
}
