import { useMutation } from '@tanstack/react-query';

import { administracion_roles_service } from '../api/administracion_roles_service';

export const use_delete_administracion_rol = () => {
  const mutation = useMutation({
    mutationFn: administracion_roles_service.delete,
  });

  return {
    delete_role: mutation.mutateAsync,

    response: mutation.data,

    loading: mutation.isPending,

    error: mutation.error,
  };
};
