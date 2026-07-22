import ConfirmModal from '@/shared/components/ConfirmModal';

interface Props {
  visible: boolean;
  onConfirm: () => void;
  onCancel: () => void;
}

export default function ConfirmacionCausarModal({
  visible,
  onConfirm,
  onCancel,
}: Props) {
  return (
    <ConfirmModal
      visible={visible}
      title="Confirmación de Causación"
      message="¿Estás seguro que requieres realizar causación?"
      onConfirm={onConfirm}
      onCancel={onCancel}
    />
  );
}
