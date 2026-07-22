import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmarEscrituraCliente } from '../models/firmar_escritura_cliente';
import { firmarEscrituraClienteService } from '../api/firmarEscrituraClienteService';

export function useFirmarEscrituraCliente(id_expediente: number) {
  return useQuery<ApiResponse<FirmarEscrituraCliente | null>>({
    queryKey: ['firmar_escritura_cliente', id_expediente],
    queryFn: () => firmarEscrituraClienteService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
