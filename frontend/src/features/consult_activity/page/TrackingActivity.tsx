import 'react-circular-progressbar/dist/styles.css';
import './consultActivity.css';

import { useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  buildStyles,
  CircularProgressbar,
} from 'react-circular-progressbar';

import { useTrackingActivity } from '../hooks/useTrackingActivity';
import type { ActividadDTO } from '../models/ConsultActivity';

type TrackingActivityPageProps = {
  id_expediente: number;
  onClose?: () => void;
  onNavigateActivity?: (actividad: ActividadDTO) => void;
};

type TrackingVisualConfig = {
  percent: number;
  label: string;
  pathColor: string;
  trailColor: string;
  textColor: string;
};

function normalizeEstado(estado?: string | null): string {
  return (estado ?? '').trim();
}

function getTitle(actividad: ActividadDTO): string {
  if (Array.isArray(actividad.title) && actividad.title.length > 0) {
    return actividad.title.join(' ');
  }

  return actividad.actividad ?? '';
}

function getTrackingConfig(estado?: string | null): TrackingVisualConfig {
  const normalized = normalizeEstado(estado);

  if (normalized === 'Done') {
    return {
      percent: 100,
      label: 'Completada',
      pathColor: '#8FD815',
      trailColor: '#EBFFC9',
      textColor: '#374151',
    };
  }

  if (normalized === 'InProgress') {
    return {
      percent: 50,
      label: 'En progreso',
      pathColor: '#F4CF02',
      trailColor: '#FEF3B4',
      textColor: '#374151',
    };
  }

  return {
    percent: 0,
    label: 'No iniciada',
    pathColor: '#D8D8D8',
    trailColor: '#F3F4F6',
    textColor: '#374151',
  };
}

export default function TrackingActivityPage({
  id_expediente,
  onClose,
  onNavigateActivity,
}: TrackingActivityPageProps) {
  const navigate = useNavigate();

  const expediente_id = Number(id_expediente ?? 0);

  const {
    data: response,
    isLoading,
    isError,
    error,
  } = useTrackingActivity(expediente_id);

  const actividades = useMemo(() => {
    const etapas = response?.status ? response.detail ?? [] : [];
    const primera_etapa = etapas[0];

    return Array.isArray(primera_etapa?.actividades)
      ? primera_etapa.actividades
      : [];
  }, [response]);

  const renderActivity = (actividad: ActividadDTO) => {
    onClose?.();

    if (onNavigateActivity) {
      onNavigateActivity(actividad);
      return;
    }

    //const url = `${actividad.url_act}/${expediente_id}/true`;
    const url = `../${actividad.url_act}/${expediente_id}`;
    navigate(url);
  };

  if (!expediente_id || expediente_id <= 0) {
    return (
      <div className="rounded-md border border-yellow-200 bg-yellow-50 px-4 py-3 text-sm text-yellow-800">
        No se recibió un expediente válido para consultar el seguimiento.
      </div>
    );
  }

  if (isLoading) {
    return <div className="py-4 text-center text-sm">Cargando seguimiento...</div>;
  }

  if (isError) {
    return (
      <div className="rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
        Ocurrió un error al cargar el seguimiento.
        {error instanceof Error ? ` ${error.message}` : ''}
      </div>
    );
  }

  if (!actividades.length) {
    return (
      <div className="rounded-md border border-gray-200 bg-gray-50 px-4 py-3 text-sm text-gray-700">
        No hay actividades registradas para este expediente.
      </div>
    );
  }

  return (
    <div className="tracking-activity-grid">
      {actividades.map((actividad) => {
        const config = getTrackingConfig(actividad.estado);
        const title = getTitle(actividad);

        return (
          <button
            key={`${actividad.id_actividad}-${actividad.id_etapa}`}
            type="button"
            className="tracking-activity-card"
            onClick={() => renderActivity(actividad)}
            title={title}
          >
            <div className="tracking-activity-circle">
              <CircularProgressbar
                value={config.percent}
                text={`${config.percent}%`}
                styles={buildStyles({
                  pathColor: config.pathColor,
                  trailColor: config.trailColor,
                  textColor: config.textColor,
                  textSize: '16px',
                  pathTransitionDuration: 1,
                })}
              />
            </div>

            <div className="tracking-activity-title">{title}</div>
            <div className="tracking-activity-subtitle">{config.label}</div>
          </button>
        );
      })}
    </div>
  );
}
