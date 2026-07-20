import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import type { TitularBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

export function useTitulares(id_expediente: number) {
  return useQuery({
    queryKey: ['validar-informacion-titulares', id_expediente],
    queryFn: () => validarInformacionService.getTitulares(id_expediente),
    enabled: id_expediente > 0,
  });
}

export function useAgregarTitular() {
  const queryClient = useQueryClient();
  return useMutation<unknown, Error, TitularBBVA>({
    mutationFn: (payload) => validarInformacionService.agregarTitular(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['validar-informacion-titulares', variables.id_expediente],
      });
    },
  });
}
