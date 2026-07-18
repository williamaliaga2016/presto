export interface ApiResponse<T> {
  status: boolean;
  detail: T;
  message?: string;
}
