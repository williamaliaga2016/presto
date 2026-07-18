import { useMutation, useQueryClient } from '@tanstack/react-query';
import { cartaAprobacionBbvaService } from '../api/cartaAprobacionBbvaService';

export function useGenerarCartaAprobacion(idExpediente: number) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: () => cartaAprobacionBbvaService.generar(idExpediente),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ['carta-aprobacion-historico', idExpediente],
      });
    },
  });
}
