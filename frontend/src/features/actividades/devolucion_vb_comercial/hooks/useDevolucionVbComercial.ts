import { useQuery } from '@tanstack/react-query';
import { devolucionVbComercialService } from '../api/devolucionVbComercialService';

export const devolucionVbComercialQueryKeys = {
  actividad: (id_expediente: number) => ['devolucion-vb-comercial', id_expediente] as const,
  controles: (id_expediente: number) => ['devolucion-vb-comercial-controles', id_expediente] as const,
};

export function useDevolucionVbComercial(id_expediente: number) {
  return useQuery({
    queryKey: devolucionVbComercialQueryKeys.actividad(id_expediente),
    queryFn: () => devolucionVbComercialService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
