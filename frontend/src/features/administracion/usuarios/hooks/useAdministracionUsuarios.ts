import { useQuery } from '@tanstack/react-query';

import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AdministracionUsuario } from '../models/AdministracionUsuariosDTO';

import { administracionUsuarioService } from '../api/administracionUsuarioService';

export const useAdministracionUsuarios = () => {
  const query = useQuery<ApiResponse<AdministracionUsuario[]>, Error>({
    queryKey: ['administracion-usuarios'],
    queryFn: administracionUsuarioService.getUsers,
  });

  return {
    users: query.data?.detail ?? [],
    response: query.data,

    loading: query.isLoading,
    fetching: query.isFetching,

    error: query.error,

    reloadUsers: query.refetch,
  };
};
