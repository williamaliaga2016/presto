import { useQuery } from '@tanstack/react-query';

import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { administracion_rol } from '../models/administracion_roles_dto';

import { administracion_roles_service } from '../api/administracion_roles_service';

export const use_administracion_roles = () => {
  const query = useQuery<
    ApiResponse<administracion_rol[]>,
    Error
  >({
    queryKey: ['administracion-roles'],
    queryFn: administracion_roles_service.get_roles,
  });

  return {
    roles: query.data?.detail ?? [],
    response: query.data,

    loading: query.isLoading,
    fetching: query.isFetching,

    error: query.error,

    reload_roles: query.refetch,
  };
};
