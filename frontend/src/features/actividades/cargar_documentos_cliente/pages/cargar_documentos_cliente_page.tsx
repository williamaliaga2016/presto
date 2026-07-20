import { useCallback, useMemo, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Checkbox } from 'primereact/checkbox';
import { InputTextarea } from 'primereact/inputtextarea';
import { Toast } from 'primereact/toast';
import type { Toast as ToastRef } from 'primereact/toast';
import { useAuth } from '@/app/providers/AuthProvider';
import { authStorage } from '@/core/auth/authStorage';
import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import ExpedienteDigitalPage from
  '@/features/funciones_transversales/components/expediente_digital/pages/ExpedienteDigital';
import {
  EMPTY_CARGAR_DOCUMENTOS_CLIENTE,
  type CargarDocumentosCliente,
} from '../models/cargar_documentos_cliente';
import {
  useAvanzarCargarDocumentosCliente,
  useCargarDocumentosCliente,
  useCargarDocumentosClienteInfo,
  useGuardarCargarDocumentosCliente,
} from '../hooks/useCargarDocumentosCliente';

const ACTIVITY_ID = 'BBVA_CONTACTO_CARGAR_DOCUMENTOS_CLIENTE_CBF7A738';
const MAX_OBSERVACIONES = 500;

interface FormDraftState {
  key: string;
  value: Partial<CargarDocumentosCliente>;
}

interface AccordionState {
  standard: number[];
  temporal: number[];
}

/**
 * Renderiza la actividad Cargar Documentos Cliente para que el cliente externo cargue documentos y confirme el envio.
 *
 * @returns Pantalla de carga documental de cliente.
 */
export default function CargarDocumentosClientePage() {
  const { id_expediente: idParam } = useParams();
  const { isTemporalAccess } = useAuth();
  const id_expediente = Number(idParam ?? 0);
  const hasValidExpediente = id_expediente > 0;
  const accordionMode = isTemporalAccess ? 'temporal' : 'standard';
  const navigate = useNavigate();
  const toast = useRef<ToastRef>(null);
  const [accordionState, setAccordionState] = useState<AccordionState>({
    standard: [0, 1, 2],
    temporal: [0, 1],
  });
  const [formDraftState, setFormDraftState] = useState<FormDraftState>({
    key: '',
    value: {},
  });
  const [showDocumentsConfirmationError, setShowDocumentsConfirmationError] =
    useState(false);
  const [temporalSubmissionCompleted, setTemporalSubmissionCompleted] =
    useState(false);

  const { data: registroResponse, isLoading: isLoadingRegistro } =
    useCargarDocumentosCliente(id_expediente);
  const { data: infoResponse, isLoading: isLoadingInfo } =
    useCargarDocumentosClienteInfo(id_expediente);
  const guardarMutation = useGuardarCargarDocumentosCliente(id_expediente);
  const avanzarMutation = useAvanzarCargarDocumentosCliente();

  const persistedForm = useMemo(
    () =>
      registroResponse?.status && registroResponse.detail
        ? registroResponse.detail
        : EMPTY_CARGAR_DOCUMENTOS_CLIENTE(id_expediente),
    [id_expediente, registroResponse],
  );
  const formDraftKey = `${id_expediente}:${persistedForm.id ?? 'new'}`;
  const formDraft = useMemo(
    () => (formDraftState.key === formDraftKey ? formDraftState.value : {}),
    [formDraftKey, formDraftState],
  );
  const form = useMemo<CargarDocumentosCliente>(
    () => ({
      ...persistedForm,
      ...formDraft,
      id_expediente,
      id_actividad: ACTIVITY_ID,
    }),
    [formDraft, id_expediente, persistedForm],
  );

  const info = infoResponse?.detail;
  const isSaving = guardarMutation.isPending || avanzarMutation.isPending;
  const activeAccordionIndexes = accordionState[accordionMode];

  const handleAccordionChange = (index: number | number[] | null) => {
    const nextIndexes = Array.isArray(index) ? index : [];

    setAccordionState((current) => ({
      ...current,
      [accordionMode]: nextIndexes,
    }));
  };

  /**
   * Aplica cambios del formulario sobre el borrador local sin perder la base consultada.
   *
   * @param updater Funcion que recibe el formulario actual y retorna el siguiente estado.
   */
  const updateForm = useCallback(
    (updater: (current: CargarDocumentosCliente) => CargarDocumentosCliente) => {
      setFormDraftState((currentDraftState) => {
        const currentDraft =
          currentDraftState.key === formDraftKey ? currentDraftState.value : {};
        const currentForm = {
          ...persistedForm,
          ...currentDraft,
          id_expediente,
          id_actividad: ACTIVITY_ID,
        };

        return {
          key: formDraftKey,
          value: updater(currentForm),
        };
      });
    },
    [formDraftKey, id_expediente, persistedForm],
  );

  /**
   * Muestra errores funcionales de guardado o avance sin exponer detalles tecnicos.
   *
   * @param message Mensaje seguro retornado por el backend.
   */
  const showError = (message?: string) => {
    toast.current?.show({
      severity: 'error',
      summary: 'No fue posible completar la accion',
      detail: message ?? 'Intente nuevamente.',
      life: 5000,
    });
  };

  /**
   * Persiste la confirmacion documental y conserva Cargar Documentos Cliente como origen del registro.
   *
   * @param showSuccessMessage Indica si debe notificarse visualmente el guardado exitoso.
   * @returns `true` cuando el guardado fue exitoso; `false` cuando el backend rechaza la operacion.
   */
  const handleGuardar = async (showSuccessMessage = true) => {
    const response = await guardarMutation.mutateAsync({
      ...form,
      id_expediente,
      id_actividad: ACTIVITY_ID,
      observaciones: form.observaciones?.trim() || null,
    });

    if (!response.status) {
      showError(response.message);
      return false;
    }

    if (showSuccessMessage) {
      toast.current?.show({
        severity: 'success',
        summary: 'Registro guardado',
        detail: 'La informacion fue guardada.',
        life: 3500,
      });
    }

    return true;
  };

  /**
   * Valida la confirmacion obligatoria, guarda el registro y avanza la actividad hacia revision documental.
   *
   * @returns Promesa resuelta al terminar la validacion local y la operacion de avance.
   */
  const handleAvanzar = async () => {
    if (!form.documentos_adjuntos) {
      setShowDocumentsConfirmationError(true);
      toast.current?.show({
        severity: 'warn',
        summary: 'Datos Obligatorios Faltantes',
        detail:
          "La casilla 'Confirmo que adjunte los documentos solicitados' debe ser marcada para poder enviar los documentos.",
        life: 5000,
      });
      return;
    }

    const response = await avanzarMutation.mutateAsync(id_expediente);

    if (!response.status) {
      showError(response.message);
      return;
    }

    if (isTemporalAccess) {
      authStorage.clear();
      setTemporalSubmissionCompleted(true);
      return;
    }

    toast.current?.show({
      severity: 'success',
      summary: 'Actividad avanzada',
      detail: 'La carga documental fue enviada a revision.',
      life: 3500,
    });
    navigate('/home/bandeja');
  };

  const informacionGeneralContent = (
      <Card className="w-full shadow-md card-presto-form mb-6">
        {(isLoadingInfo || isLoadingRegistro) && (
          <div className="mb-4 text-sm text-blue-600">Cargando informacion...</div>
        )}

        <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
          <div>
            <div className="text-sm font-semibold text-gray-600">Folio Presto</div>
            <div className="text-base text-gray-900">
              {info?.id_expediente ?? id_expediente}
            </div>
          </div>

          <div>
            <div className="text-sm font-semibold text-gray-600">Analista de Vivienda</div>
            <div className="text-base text-gray-900">
              {info?.nombre_analista ?? 'Sin informacion'}
            </div>
          </div>

          <div>
            <div className="text-sm font-semibold text-gray-600">Nombre del Cliente</div>
            <div className="text-base text-gray-900">
              {info?.nombre_completo_t1 ?? info?.nombre_cliente ?? 'Sin informacion'}
            </div>
          </div>
        </div>
      </Card>
  );

  const informacionGeneralTab = (
    <AccordionTab header="Informacion General" disabled={!hasValidExpediente}>
      {informacionGeneralContent}
    </AccordionTab>
  );

  const funcionesTransversalesTab = (
    <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
      <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
    </AccordionTab>
  );

  const confirmacionDocumentosContent = (
    <div className="mt-6 space-y-4">
      {isLoadingRegistro && (
        <div className="text-sm text-blue-600">Cargando confirmacion...</div>
      )}

      <div
        className={`rounded-md border p-3 ${
          showDocumentsConfirmationError
            ? 'border-red-500 bg-red-50'
            : 'border-transparent'
        }`}
      >
        <Checkbox
          inputId="documentos-adjuntos"
          checked={form.documentos_adjuntos}
          className={showDocumentsConfirmationError ? 'p-invalid' : undefined}
          onChange={(event) => {
            const checked = event.checked ?? false;
            if (checked) {
              setShowDocumentsConfirmationError(false);
            }

            updateForm((current) => ({
              ...current,
              documentos_adjuntos: checked,
            }));
          }}
        />
        <label
          htmlFor="documentos-adjuntos"
          className={`ml-2 text-sm font-semibold ${
            showDocumentsConfirmationError ? 'text-red-700' : 'text-gray-700'
          }`}
        >
          Confirmo que adjunte los documentos solicitados
        </label>
        {showDocumentsConfirmationError && (
          <small className="mt-2 block text-red-700">
            La casilla debe ser marcada para poder enviar los documentos.
          </small>
        )}
      </div>

      <div>
        <label
          htmlFor="observaciones-docs-cliente"
          className="mb-2 block text-sm font-semibold text-gray-700"
        >
          Observaciones
        </label>
        <InputTextarea
          id="observaciones-docs-cliente"
          value={form.observaciones ?? ''}
          onChange={(event) =>
            updateForm((current) => ({
              ...current,
              observaciones: event.target.value,
            }))
          }
          rows={4}
          maxLength={MAX_OBSERVACIONES}
          autoResize
          className="form-textarea-presto w-full"
        />
        <small className="text-gray-500">
          Maximo: {(form.observaciones ?? '').length}/{MAX_OBSERVACIONES} caracteres
        </small>
      </div>
    </div>
  );

  const documentosClienteActividadTab = (
    <AccordionTab header="Documentos Cliente" disabled={!hasValidExpediente}>
      {confirmacionDocumentosContent}
    </AccordionTab>
  );

  const documentosClienteTab = (header = 'Documentos Cliente') => (
    <AccordionTab header={header} disabled={!hasValidExpediente}>
      <ExpedienteDigitalPage
        id_expediente={id_expediente}
        activity_id={ACTIVITY_ID}
        files_activity_id={isTemporalAccess ? ACTIVITY_ID : undefined}
      />
    </AccordionTab>
  );

  if (temporalSubmissionCompleted) {
    return (
      <>
        <Toast ref={toast} />
        <div className="flex min-h-[60vh] items-center justify-center px-4">
          <Card className="w-full max-w-xl text-center shadow-md card-presto-form">
            <i className="pi pi-check-circle mb-4 text-5xl text-green-600" />
            <h2 className="mb-3 text-2xl font-bold text-gray-900">
              Los documentos han sido enviados exitosamente.
            </h2>
            <p className="text-sm text-gray-600">
              La carga documental fue recibida y el enlace temporal ya fue consumido.
            </p>
          </Card>
        </div>
      </>
    );
  }

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Cargar Documentos Cliente
      </h2>

      {isTemporalAccess ? (
        <Accordion
          multiple
          activeIndex={activeAccordionIndexes}
          onTabChange={(event) => handleAccordionChange(event.index)}
        >
          {informacionGeneralTab}
          {documentosClienteTab()}
        </Accordion>
      ) : (
        <Accordion
          multiple
          activeIndex={activeAccordionIndexes}
          onTabChange={(event) => handleAccordionChange(event.index)}
        >
          <AccordionTab header="Informacion del Expediente" disabled={!hasValidExpediente}>
            <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
          </AccordionTab>
          {funcionesTransversalesTab}
          {documentosClienteActividadTab}
        </Accordion>
      )}

      {isTemporalAccess && confirmacionDocumentosContent}

      <div className="mt-6 flex flex-wrap justify-end gap-3">
        <Button
          label="Guardar"
          icon="pi pi-save"
          onClick={handleGuardar}
          loading={guardarMutation.isPending}
          disabled={!hasValidExpediente || isSaving}
        />
        <Button
          label={isTemporalAccess ? 'Enviar Documentos' : 'Avanzar'}
          icon="pi pi-arrow-right"
          severity="success"
          onClick={handleAvanzar}
          loading={avanzarMutation.isPending}
          disabled={!hasValidExpediente || isSaving}
        />
      </div>
    </>
  );
}
