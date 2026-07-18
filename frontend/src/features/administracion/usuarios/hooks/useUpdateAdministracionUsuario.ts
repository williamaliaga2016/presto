import { useMutation } from '@tanstack/react-query';

import { administracionUsuarioService } from '../api/administracionUsuarioService';

export const useUpdateAdministracionUsuario = () => {
  const mutation = useMutation({
    mutationFn: administracionUsuarioService.update,
  });

  return {
    updateUser: mutation.mutateAsync,

    response: mutation.data,

    loading: mutation.isPending,

    error: mutation.error,
  };
};
