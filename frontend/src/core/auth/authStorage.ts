const TOKEN_KEY = 'token_Multibanca';
const USER_KEY = 'user';

export const authStorage = {
  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  },

  setToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
  },

  removeToken() {
    localStorage.removeItem(TOKEN_KEY);
  },

  getUser<T>(): T | null {
    const raw = localStorage.getItem(USER_KEY);
    return raw ? (JSON.parse(raw) as T) : null;
  },

  setUser<T>(user: T) {
    localStorage.setItem(USER_KEY, JSON.stringify(user));
  },

  clear() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
  },
};
