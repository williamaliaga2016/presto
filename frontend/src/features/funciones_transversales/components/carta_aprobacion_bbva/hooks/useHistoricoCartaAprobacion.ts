import { useQuery } from '@tanstack/react-query';
import { cartaAprobacionBbvaService } from '../api/cartaAprobacionBbvaService';

export function useHistoricoCartaAprobacion(idExpediente: number) {
  return useQuery({
    queryKey: ['carta-aprobacion-historico', idExpediente],
    queryFn: () => cartaAprobacionBbvaService.getHistorico(idExpediente),
    enabled: idExpediente > 0,
  });
}
