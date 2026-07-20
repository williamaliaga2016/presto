import { useQuery } from '@tanstack/react-query';
import { devolucionVbComercialService } from
  '../api/devolucionVbComercialService';

export function useControlesDevolucionVbComercial() {
  return useQuery({
    queryKey: ['devolucion-vb-comercial-controles'],
    queryFn: () =>
      devolucionVbComercialService.getControles(),
  });
}
