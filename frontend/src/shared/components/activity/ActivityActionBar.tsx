import type { ReactNode } from 'react';

import { Button } from 'primereact/button';

type ActivityActionBarProps = {
  from_tracking?: boolean;
  is_busy?: boolean;
  is_disabled?: boolean;
  can_advance?: boolean;
  is_saving?: boolean;
  is_advancing?: boolean;
  on_edit: () => void;
  on_save: () => void;
  on_advance: () => void;
  on_exit: () => void;
  children?: ReactNode;
};

export function ActivityActionBar({
  from_tracking = false,
  is_busy = false,
  is_disabled = false,
  can_advance = true,
  is_saving = false,
  is_advancing = false,
  on_edit,
  on_save,
  on_advance,
  on_exit,
  children,
}: ActivityActionBarProps) {
  const is_processing = is_busy || is_saving || is_advancing;

  return (
    <div className="form-actions">
      {!from_tracking && (
        <>
          <Button
            type="button"
            label="Editar"
            icon="pi pi-pencil"
            severity="info"
            outlined
            onClick={on_edit}
            disabled={is_processing || !is_disabled}
            className="btn-responsive"
          />

          <Button
            type="button"
            label={is_saving ? 'Guardando...' : 'Guardar'}
            icon="pi pi-save"
            severity="success"
            onClick={on_save}
            disabled={is_processing || is_disabled}
            loading={is_saving}
            className="btn-responsive"
          />

          <Button
            type="button"
            label={is_advancing ? 'Avanzando...' : 'Avanzar'}
            icon="pi pi-arrow-right"
            severity="warning"
            onClick={on_advance}
            disabled={is_processing || !can_advance}
            loading={is_advancing}
            className="btn-responsive"
          />

          {children}
        </>
      )}

      <Button
        type="button"
        label="Salir"
        icon="pi pi-sign-out"
        severity="secondary"
        outlined
        onClick={on_exit}
        disabled={is_processing}
        className="btn-responsive"
      />
    </div>
  );
}

export default ActivityActionBar;
