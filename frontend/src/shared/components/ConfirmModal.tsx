import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';

interface ConfirmModalProps {
  visible: boolean;
  title?: string;
  message: string;
  icon?: string;
  confirmLabel?: string;
  cancelLabel?: string;
  onConfirm: () => void;
  onCancel: () => void;
}

export default function ConfirmModal({
  visible,
  title = 'Confirmación',
  message,
  icon = 'pi pi-exclamation-triangle',
  confirmLabel = 'SÍ',
  cancelLabel = 'NO',
  onConfirm,
  onCancel,
}: ConfirmModalProps) {
  const footer = (
    <div className="flex justify-end gap-2">
      <Button
        label={cancelLabel}
        icon="pi pi-times"
        severity="secondary"
        outlined
        onClick={onCancel}
      />
      <Button
        label={confirmLabel}
        icon="pi pi-check"
        severity="success"
        onClick={onConfirm}
      />
    </div>
  );

  return (
    <Dialog
      header={title}
      visible={visible}
      onHide={onCancel}
      footer={footer}
      modal
      draggable={false}
      resizable={false}
      style={{ width: '30rem' }}
    >
      <div className="flex items-center gap-3">
        <i className={`${icon} text-3xl text-yellow-500`} />
        <p className="text-base">{message}</p>
      </div>
    </Dialog>
  );
}
