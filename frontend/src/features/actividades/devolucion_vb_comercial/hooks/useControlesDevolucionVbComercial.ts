import { useQuery } from '@tanstack/react-query';
import { devolucionVbComercialService } from '../api/devolucionVbComercialService';
import { devolucionVbComercialQueryKeys } from './useDevolucionVbComercial';

export function useControlesDevolucionVbComercial(id_expediente: number) {
  return useQuery({
    queryKey: devolucionVbComercialQueryKeys.controles(id_expediente),
    queryFn: () => devolucionVbComercialService.getControles(id_expediente),
    enabled: id_expediente > 0,
  });
}
