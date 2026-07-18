import { useEffect, useState } from 'react';

import { roleService } from '../api/roleService';

import type { RoleDTO } from '../models/RoleDTO';

export const useRoles = () => {
  const [roles, setRoles] = useState<RoleDTO[]>([]);

  const [loadingRoles, setLoadingRoles] = useState(false);

  const loadRoles = async () => {
    try {
      setLoadingRoles(true);

      const response = await roleService.getRoles();

      setRoles(response);
    } catch (error) {
      console.error(error);
    } finally {
      setLoadingRoles(false);
    }
  };

  useEffect(() => {
    loadRoles();
  }, []);

  return {
    roles,

    loadingRoles,

    reloadRoles: loadRoles,
  };
};
