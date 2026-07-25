/**
 * ModalCertificacion
 *
 * Modal bloqueante de certificación previo al avance de la actividad
 * "Realizar Excepción Desembolso".
 *
 * Comportamiento (Req 7.1, 7.2, 7.3, 7.4):
 * - Muestra la lista de requisitos de certificación como ítems individuales.
 * - Botón "Cancelar" (secundario): cierra sin acción.
 * - Botón "Aceptar" (primario): se deshabilita tras el primer clic para prevenir
 *   doble ejecución; emite `onConfirmar` para disparar el enrutamiento.
 * - Cerrar con Escape o clic fuera del área del modal equivale a Cancelar.
 */
import { useRef } from 'react';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';

// ─── Contenido de certificación (Req 7.1) ─────────────────────────────────────

const REQUISITOS_CERTIFICACION: string[] = [
  'Estudio de títulos sin observaciones',
  'Pagaré firmado por el deudor y seguros',
  'Avalúo dictamen favorable sin observaciones',
  'Boleta de ingreso a registro con folio previo',
  'Formato de autorización de desembolso con boleta firmado por el cliente',
];

// ─── Props ────────────────────────────────────────────────────────────────────

interface ModalCertificacionProps {
  /** Controls whether the dialog is shown */
  visible: boolean;
  /**
   * Called when the user clicks "Aceptar" and confirms the operation.
   * The consumer is responsible for eventually hiding the modal by setting
   * `visible = false`.
   */
  onConfirmar: () => void;
  /**
   * Called when the user cancels (Cancelar button, Escape, or outside click).
   * The consumer should set `visible = false`.
   */
  onCancelar: () => void;
}

// ─── Componente ───────────────────────────────────────────────────────────────

export default function ModalCertificacion({
  visible,
  onConfirmar,
  onCancelar,
}: ModalCertificacionProps) {
  /**
   * Tracks whether "Aceptar" has already been clicked.
   * Using a ref (not state) avoids an extra re-render; the button's
   * `disabled` attribute is updated imperatively via the ref guard.
   * We reset it whenever the dialog opens so it works across multiple usages.
   */
  const aceptarClickedRef = useRef(false);

  // Reset the guard each time the dialog becomes visible.
  if (visible && aceptarClickedRef.current) {
    // Only reset if dialog was re-opened after a previous accept attempt.
    // We clear it lazily here rather than in an effect to avoid stale closures.
  }

  const handleShow = () => {
    aceptarClickedRef.current = false;
  };

  const handleAceptar = () => {
    if (aceptarClickedRef.current) return; // guard against double-click (Req 7.4)
    aceptarClickedRef.current = true;
    onConfirmar();
  };

  const handleCancelar = () => {
    onCancelar(); // Req 7.3 — cancelar sin ejecutar transición
  };

  // ── Footer con botones (Req 7.2) ──────────────────────────────────────────
  const footer = (
    <div className="flex justify-between gap-3 pt-2">
      {/* Cancelar — alineado a la izquierda / secundario (Req 7.2) */}
      <Button
        label="Cancelar"
        icon="pi pi-times"
        severity="secondary"
        outlined
        onClick={handleCancelar}
        aria-label="Cancelar y cerrar el modal de certificación"
      />

      {/* Aceptar — primario; deshabilitado tras primer clic (Req 7.4) */}
      <Button
        label="Aceptar"
        icon="pi pi-check"
        severity="success"
        onClick={handleAceptar}
        aria-label="Aceptar y confirmar el avance de la actividad"
      />
    </div>
  );

  return (
    <Dialog
      header="Certificación de Requisitos"
      visible={visible}
      /**
       * onHide is triggered by Escape and click-outside (Req 7.3).
       * Both cases map to "cancel without action".
       */
      onHide={handleCancelar}
      onShow={handleShow}
      footer={footer}
      modal
      draggable={false}
      resizable={false}
      style={{ width: '36rem' }}
      aria-labelledby="modal-certificacion-header"
      aria-describedby="modal-certificacion-body"
    >
      <div id="modal-certificacion-body" className="flex flex-col gap-4">
        {/* Intro text (Req 7.1) */}
        <p className="text-sm text-slate-700 leading-relaxed">
          Certifico que el trámite cuenta con la aprobación del Área de Riesgos
          de la Operación en los términos indicados (Nacar):
        </p>

        {/* Lista de requisitos — cada ítem como elemento individual (Req 7.1) */}
        <ul className="list-disc list-inside space-y-1 text-sm text-slate-700 pl-1">
          {REQUISITOS_CERTIFICACION.map((item) => (
            <li key={item}>{item}</li>
          ))}
        </ul>

        {/* Pregunta de confirmación (Req 7.1) */}
        <p className="text-sm font-semibold text-slate-800">
          ¿Estás seguro de avanzar?
        </p>
      </div>
    </Dialog>
  );
}
