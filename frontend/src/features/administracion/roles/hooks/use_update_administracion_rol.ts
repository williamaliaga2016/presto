import { useMutation } from '@tanstack/react-query';

import { administracion_roles_service } from '../api/administracion_roles_service';

export const use_update_administracion_rol = () => {
  const mutation = useMutation({
    mutationFn: administracion_roles_service.update,
  });

  return {
    update_role: mutation.mutateAsync,

    response: mutation.data,

    loading: mutation.isPending,

    error: mutation.error,
  };
};
