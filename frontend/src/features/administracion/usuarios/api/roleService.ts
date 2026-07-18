import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import { API_BASE_URL } from '../../../../config';

const BASE_URL = `${API_BASE_URL}/api/Role`;

export const roleService = {
  async getRoles(): Promise<ControlBaseDTO[]> {
    const response = await axiosClient.get<ApiResponse<ControlBaseDTO[]>>(
      `${BASE_URL}/GetControlRole`,
    );

    return response.data.detail ?? [];
  },
};
