/* eslint-disable */

import '../api/boldReportsSetup';

import '@boldreports/javascript-reporting-controls/Content/v2.0/tailwind-light/bold.report-viewer.min.css';
import '@boldreports/javascript-reporting-controls/Scripts/v2.0/common/bold.reports.common.min';
import '@boldreports/javascript-reporting-controls/Scripts/v2.0/common/bold.reports.widgets.min';
import '@boldreports/javascript-reporting-controls/Scripts/v2.0/bold.report-viewer.min';
import '@boldreports/react-reporting-components/Scripts/bold.reports.react.min';

import { useMemo } from 'react';
import { useReportViewer } from '../hooks/useReportViewer';
import { authStorage } from '@/core/auth/authStorage';

declare const BoldReportViewerComponent: any;

interface ReportViewerPageProps {
  report_path: string;
  parameters?: { name: string; values: string[] }[];
}

export default function ReportViewerPage({ report_path, parameters }: ReportViewerPageProps) {
  const { report_service_url, report_path: reportPath } = useReportViewer(report_path);

  const viewerStyle = useMemo<React.CSSProperties>(
    () => ({
      height: 'calc(100vh - 190px)',
      minHeight: '650px',
      width: '100%',
    }),
    []
  );

  if (!reportPath) {
    return (
      <div className="p-4 text-sm text-gray-600">
        Seleccione un reporte para visualizar.
      </div>
    );
  }

  return (
    <div style={viewerStyle}>
      <BoldReportViewerComponent
        id={`report-viewer-${reportPath.replace(/[^a-zA-Z0-9]/g, '-')}`}
        reportServiceUrl={report_service_url}
        serviceAuthorizationToken={`Bearer ${authStorage.getToken() ?? ''}`}
        reportPath={reportPath}
        parameters={parameters}
        toolbarSettings={{
          showToolbar: true,
        }}
      />
    </div>
  );
}
