import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { UserLookup } from '../models/user_lookup';

const buildNameComplete = (user: UserLookup): string => {
  const fullName = [
    user.name,
    user.last_name_first,
    user.last_name_second,
  ]
    .filter(Boolean)
    .join(' ')
    .trim();

  return fullName || user.user_name || user.email || `Usuario ${user.user_id}`;
};

export const userLookupService = {
  async getUsers(): Promise<ApiResponse<UserLookup[]>> {
    const response = await axiosClient.get<ApiResponse<UserLookup[]>>(
      '/api/User/GetUsers',
    );

    const detail = (response.data.detail ?? []).map((user) => ({
      ...user,
      name_complete: user.name_complete || buildNameComplete(user),
    }));

    return {
      ...response.data,
      detail,
    };
  },
};
