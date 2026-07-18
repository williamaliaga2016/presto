import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import type { RegistroContactoBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';
import { validarInformacionQueryKeys } from './useValidarInformacion';

export function useRegistrosContacto(
  id_expediente: number,
  id_actividad: string,
) {
  return useQuery({
    queryKey: validarInformacionQueryKeys.registroContacto(
      id_expediente,
      id_actividad,
    ),
    queryFn: () =>
      validarInformacionService.getRegistrosContacto(id_expediente, id_actividad),
    enabled: id_expediente > 0 && id_actividad.trim().length > 0,
  });
}

export function useCrearRegistroContacto() {
  const queryClient = useQueryClient();

  return useMutation<unknown, Error, RegistroContactoBBVA>({
    mutationFn: (payload) =>
      validarInformacionService.crearRegistroContacto(payload),
    onSuccess: (_, variables) => {
      void queryClient.invalidateQueries({
        queryKey: validarInformacionQueryKeys.registroContacto(
          variables.id_expediente,
          variables.id_actividad,
        ),
      });
    },
  });
}
