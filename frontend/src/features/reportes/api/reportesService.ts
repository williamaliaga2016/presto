import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ReporteDTO } from '../models/reporte';

const REPORT_VIEWER_PATH = '/api/ReportViewer';

export async function getReportes(): Promise<ApiResponse<ReporteDTO[]>> {
  const response = await axiosClient.get<ApiResponse<ReporteDTO[]>>(
    '/api/Reports/GetAll',
  );
  return response.data;
}

export function getReportServiceUrl(): string {
  const api_base_url = import.meta.env.VITE_API_BASE_URL as string | undefined;

  if (!api_base_url) {
    return REPORT_VIEWER_PATH;
  }

  return `${api_base_url.replace(/\/$/, '')}${REPORT_VIEWER_PATH}`;
}
