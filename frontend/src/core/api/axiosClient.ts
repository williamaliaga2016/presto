import axios from 'axios';
import { env } from '../config/env';
import { authStorage } from '../auth/authStorage';
import { handle_permission_error } from './permission_axios_interceptor';

export const axiosClient = axios.create({
  baseURL: env.apiUrl,
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
});

const MUTATION_URL_PATTERNS = ['/Save', '/Avanzar/'];

function isMutationUrl(url?: string): boolean {
  if (!url) return false;
  return MUTATION_URL_PATTERNS.some((pattern) => url.includes(pattern));
}

axiosClient.interceptors.request.use((config) => {
  const token = authStorage.getToken();

  if (token) {
    config.headers.set('Authorization', `Bearer ${token}`);
  }

  return config;
});

axiosClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 403) {
      return handle_permission_error(error);
    }

    if (error.response?.status === 401) {
      const requestUrl: string | undefined = error.config?.url;

      if (isMutationUrl(requestUrl)) {
        return Promise.reject(error);
      }

      authStorage.clear();

      if (window.location.pathname !== '/login') {
        window.location.href = '/login';
      }
    }

    return Promise.reject(error);
  },
);
