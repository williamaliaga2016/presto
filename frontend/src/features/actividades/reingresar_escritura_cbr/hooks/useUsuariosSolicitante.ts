import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { UserLookup } from '../models/user_lookup';
import { userLookupService } from '../api/userLookupService';

export function useUsuariosSolicitante() {
  return useQuery<ApiResponse<UserLookup[]>>({
    queryKey: ['usuarios_solicitante'],
    queryFn: () => userLookupService.getUsers(),
    staleTime: 5 * 60 * 1000,
  });
}
