export function getErrorMessage(error: unknown): string {
  if (typeof error === 'string') return error;

  if (typeof error === 'object' && error !== null && 'response' in error) {
    const axiosError = error as {
      response?: {
        data?: {
          message?: string;
          detail?: string;
        };
      };
    };

    return (
      axiosError.response?.data?.message ||
      axiosError.response?.data?.detail ||
      'Ocurrió un error inesperado'
    );
  }

  return 'Ocurrió un error inesperado';
}
