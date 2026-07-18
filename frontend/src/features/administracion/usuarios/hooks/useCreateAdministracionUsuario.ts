import { useMutation } from '@tanstack/react-query';

import { administracionUsuarioService } from '../api/administracionUsuarioService';

export const useCreateAdministracionUsuario = () => {
  const mutation = useMutation({
    mutationFn: administracionUsuarioService.create,
  });

  return {
    createUser: mutation.mutateAsync,

    response: mutation.data,

    loading: mutation.isPending,

    error: mutation.error,
  };
};
