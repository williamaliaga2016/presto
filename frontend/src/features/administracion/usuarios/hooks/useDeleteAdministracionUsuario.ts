import { useMutation } from '@tanstack/react-query';

import { administracionUsuarioService } from '../api/administracionUsuarioService';

export const useDeleteAdministracionUsuario = () => {
  const mutation = useMutation({
    mutationFn: administracionUsuarioService.delete,
  });

  return {
    deleteUser: mutation.mutateAsync,

    response: mutation.data,

    loading: mutation.isPending,

    error: mutation.error,
  };
};
