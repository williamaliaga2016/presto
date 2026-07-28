import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GetByExpedienteResponse } from '../models/validar_condiciones_desembolso';
import { validarCondicionesDesembolsoService } from '../api/validarCondicionesDesembolsoService';

export function useValidarCondicionesDesembolso(id_expediente: number) {
  return useQuery<ApiResponse<GetByExpedienteResponse | null>>({
    queryKey: ['validar_condiciones_desembolso', id_expediente],
    queryFn: () => validarCondicionesDesembolsoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
