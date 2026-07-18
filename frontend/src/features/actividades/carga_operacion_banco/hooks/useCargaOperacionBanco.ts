import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CargaOperacionBanco } from '../models/carga_operacion_banco';
import { cargaOperacionBancoService } from '../api/cargaOperacionBancoService';

export function useCargaOperacionBanco(id_expediente: number) {
  return useQuery<ApiResponse<CargaOperacionBanco | null>>({
    queryKey: ['carga_operacion_banco', id_expediente],
    queryFn: () => cargaOperacionBancoService.getFullByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
