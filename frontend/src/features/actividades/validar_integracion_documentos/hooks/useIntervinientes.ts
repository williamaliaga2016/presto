import { useQuery } from '@tanstack/react-query';
import { validarIntegracionDocumentosService } from '../api/validarIntegracionDocumentosService';

export function useIntervinientes(id_expediente: number) {

  return useQuery({
    queryKey: [
      'intervinientes',
      id_expediente
    ],

    queryFn: () =>
      validarIntegracionDocumentosService.getIntervinientes(id_expediente),

    enabled: id_expediente > 0,
  });
}