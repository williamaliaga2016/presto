import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import type { RegistroContactoBBVA } from '../models/registroContacto';
import { registroContactoService } from '../api/registroContactoService';

export function useControlesRegistroContacto() {
  return useQuery({
    queryKey: ['registro-contacto-controles'],
    queryFn: () => registroContactoService.getControles(),
  });
}

/**
 * Consulta el historico global de contactos del expediente.
 *
 * @param id_expediente Expediente asociado al historico.
 * @returns Query con registros de contacto ordenados por el backend.
 */
export function useRegistrosContacto(id_expediente: number) {
  return useQuery({
    queryKey: ['registro-contacto', id_expediente],
    queryFn: () => registroContactoService.getRegistrosContacto(id_expediente),
    enabled: id_expediente > 0,
  });
}

/**
 * Crea un registro de contacto e invalida el historico global del expediente.
 *
 * @returns Mutacion para persistir contactos del cliente.
 */
export function useCrearRegistroContacto() {
  const queryClient = useQueryClient();
  return useMutation<unknown, Error, RegistroContactoBBVA>({
    mutationFn: (payload) =>
      registroContactoService.crearRegistroContacto(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['registro-contacto', variables.id_expediente],
      });
    },
  });
}
