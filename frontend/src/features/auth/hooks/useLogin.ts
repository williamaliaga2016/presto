import { useMutation } from '@tanstack/react-query';
import { authService } from '../api/authService';
import { useAuth } from '@/app/providers/AuthProvider';

export function useLogin() {
  const { login } = useAuth();

  return useMutation({
    mutationFn: authService.login,
    onSuccess: (response) => {
      if (response.status && response.detail?.token_multibanca) {
        login(response.detail.token_multibanca, response.detail);
      }
    },
  });
}
