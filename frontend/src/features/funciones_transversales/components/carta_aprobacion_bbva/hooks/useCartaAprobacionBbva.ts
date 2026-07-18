import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import {
  cartaAprobacionBbvaService,
  type CartaAprobacionBbva,
} from '../api/cartaAprobacionBbvaService';

export function useCartaAprobacionBbva(idExpediente: number) {
  return useQuery<ApiResponse<CartaAprobacionBbva | null>>({
    queryKey: ['carta-aprobacion-bbva', idExpediente],
    queryFn: () => cartaAprobacionBbvaService.getByExpediente(idExpediente),
    enabled: idExpediente > 0,
  });
}
