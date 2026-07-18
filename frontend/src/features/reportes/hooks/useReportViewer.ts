import { useMemo } from 'react';
import { getReportServiceUrl } from '../api/reportesService';
import { ReportViewerConfig } from '../models/reporte';

export function useReportViewer(reportPath?: string): ReportViewerConfig {
  return useMemo(
    () => ({
      report_service_url: getReportServiceUrl(),
      report_path: reportPath ?? '',
    }),
    [reportPath]
  );
}
