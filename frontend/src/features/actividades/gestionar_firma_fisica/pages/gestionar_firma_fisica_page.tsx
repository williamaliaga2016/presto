import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import ResultadoFirmasSection from '../components/ResultadoFirmasSection';
import type { GestionarFirmaFisica } from '../models/gestionar_firma_fisica';
import { useGestionarFirmaFisica } from '../hooks/useGestionarFirmaFisica';
import { useUpsertGestionarFirmaFisica } from '../hooks/useUpsertGestionarFirmaFisica';
import { useAvanzarGestionarFirmaFisica } from '../hooks/useAvanzarGestionarFirmaFisica';
import { useControlesGestionarFirmaFisica } from '../hooks/useControlesGestionarFirmaFisica';
import {
  DEFAULT_RESULTADO_GESTORIA_OPTIONS,
  EMPTY_CONTROLES_GESTIONAR_FIRMA_FISICA,
} from '../models/controles';

const ACTIVITY_ID = 'ACT_FIRMA_FISICA';

const buildInitialState = (id_expediente: number): GestionarFirmaFisica => ({
  id: 0,
  id_expediente,
  id_actividad: ACTIVITY_ID,
  motorizado_asignado: null,
  fecha_gestoria: null,
  resultado_gestoria: null,
  observaciones: null,
});

const normalizeDate = (value?: string | null): string | null => {
  if (!value) return null;

  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return null;
  }

  const yyyy = parsed.getFullYear();
  const mm = String(parsed.getMonth() + 1).padStart(2, '0');
  const dd = String(parsed.getDate()).padStart(2, '0');

  return `${yyyy}-${mm}-${dd}`;
};

const normalizeGestionarFirmaFisica = (
  source: Partial<GestionarFirmaFisica> | null | undefined,
  id_expediente_fallback: number,
): GestionarFirmaFisica => ({
  id: Number(source?.id ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
  id_actividad: source?.id_actividad ?? ACTIVITY_ID,
  motorizado_asignado: source?.motorizado_asignado ?? null,
  fecha_gestoria: normalizeDate(source?.fecha_gestoria),
  resultado_gestoria: source?.resultado_gestoria ?? null,
  observaciones: source?.observaciones ?? null,
});

const extractFormularioFromDetail = (
  detail: unknown,
): Partial<GestionarFirmaFisica> | null => {
  if (!detail || typeof detail !== 'object') {
    return null;
  }

  const detailRecord = detail as Record<string, unknown>;
  const nestedFormulario = detailRecord.formulario;

  if (nestedFormulario && typeof nestedFormulario === 'object') {
    return nestedFormulario as Partial<GestionarFirmaFisica>;
  }

  return detail as Partial<GestionarFirmaFisica>;
};

export default function GestionarFirmaFisicaPage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<GestionarFirmaFisica>(buildInitialState(id_expediente));
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useGestionarFirmaFisica(id_expediente);
  const saveMutation = useUpsertGestionarFirmaFisica();
  const avanzarMutation = useAvanzarGestionarFirmaFisica();
  const { data: controlesData } = useControlesGestionarFirmaFisica(id_expediente);
  const controles = controlesData?.detail ?? EMPTY_CONTROLES_GESTIONAR_FIRMA_FISICA;
  const resultadoGestoriaOptions =
    controles.resultado_gestoria && controles.resultado_gestoria.length > 0
      ? controles.resultado_gestoria
      : DEFAULT_RESULTADO_GESTORIA_OPTIONS;

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;

      setForm(buildInitialState(id_expediente));
      setErrorMessage('');
      setSuccessMessage('');
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const formulario = extractFormularioFromDetail(data.detail);
      const loadedEntity = normalizeGestionarFirmaFisica(formulario, id_expediente);

      setForm(loadedEntity);
      setIsDisabled(Number(loadedEntity.id) > 0);
      setCanAdvance(false);

      hasHydratedRef.current = true;
      return;
    }

    if (!id_expediente || id_expediente <= 0) {
      setForm(buildInitialState(0));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm((prev: GestionarFirmaFisica) =>
        normalizeGestionarFirmaFisica(
          {
            ...prev,
            id_expediente,
          },
          id_expediente,
        ),
      );
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  const updateField = <K extends keyof GestionarFirmaFisica>(
    field: K,
    value: GestionarFirmaFisica[K],
  ) => {
    setForm((prev: GestionarFirmaFisica) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleEditar = () => {
    setErrorMessage('');
    setSuccessMessage('');
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const validateForm = () => {
    if (form.id_expediente < 0) {
      return 'No existe un id_expediente valido.';
    }

    if (!form.motorizado_asignado?.trim()) {
      return 'Debe ingresar el motorizado asignado.';
    }

    if (!form.fecha_gestoria) {
      return 'Debe ingresar la fecha de gestoría.';
    }

    if (!form.resultado_gestoria?.trim()) {
      return 'Debe seleccionar el resultado de gestoría.';
    }

    return '';
  };

  const handleGuardar = async () => {
    setErrorMessage('');
    setSuccessMessage('');

    const validationMessage = validateForm();
    if (validationMessage) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validacion',
        detail: validationMessage,
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const payload: GestionarFirmaFisica = normalizeGestionarFirmaFisica(
        {
          ...form,
          id: Number(form.id ?? 0),
          id_expediente: Number(form.id_expediente || id_expediente || 0),
        },
        id_expediente,
      );

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Exito',
          detail: 'Gestion de firma fisica guardada correctamente',
          life: 3000,
        });

        const formularioGuardado = extractFormularioFromDetail(response.detail ?? payload);
        const savedEntity = normalizeGestionarFirmaFisica(formularioGuardado, payload.id_expediente);

        setForm(savedEntity);
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atencion',
          detail: response.message || 'No se pudo guardar',
          life: 3000,
        });
      }
    } catch (error) {
      console.error('ERROR GUARDAR GESTIONAR FIRMA FISICA', error);

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'Ocurrio un error al guardar',
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async () => {
    setErrorMessage('');
    setSuccessMessage('');

    const expedienteId = Number(form.id_expediente ?? 0);

    if (!expedienteId || expedienteId < 0) {
      const msg = 'No existe un id_expediente valido para avanzar.';
      setErrorMessage(msg);
      toast.current?.show({
        severity: 'warn',
        summary: 'Validacion',
        detail: msg,
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const response = await avanzarMutation.mutateAsync(expedienteId);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Exito',
          detail: 'Actividad avanzada correctamente',
          life: 2000,
        });

        navigate('/home/bandeja');
      } else {
        const msg = response.message || 'No se pudo avanzar la actividad.';

        setErrorMessage(msg);
        setSuccessMessage('');

        toast.current?.show({
          severity: 'warn',
          summary: 'Atencion',
          detail: msg,
          life: 3000,
        });
      }
    } catch (error) {
      console.error('ERROR AVANZAR GESTIONAR FIRMA FISICA', error);
      const msg = 'Ocurrio un error al avanzar.';

      setErrorMessage(msg);
      setSuccessMessage('');

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: msg,
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">Gestionar Firma Física</h2>

      <Accordion activeIndex={[0]} multiple>
        <AccordionTab
          disabled={!id_expediente || id_expediente <= 0}
          header="Informacion del Expediente"
        >
          <EncabezadoActividad
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            activityID={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            idActividad={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab header="Gestionar Firma Fisica">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && (
              <div className="mb-4 text-sm text-blue-600">Cargando informacion...</div>
            )}

            {errorMessage && (
              <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                {errorMessage}
              </div>
            )}

            {successMessage && (
              <div className="mb-4 rounded-md border border-green-200 bg-green-50 px-4 py-3 text-sm text-green-700">
                {successMessage}
              </div>
            )}

            <ResultadoFirmasSection
              formulario={form}
              isDisabled={isDisabled}
              updateField={updateField}
              resultadoGestoriaOptions={resultadoGestoriaOptions}
            />

            <div className="form-actions">
              <Button
                type="button"
                label="Editar"
                icon="pi pi-pencil"
                severity="info"
                outlined
                onClick={handleEditar}
                disabled={isBusy || !isDisabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={saveMutation.isPending ? 'Guardando...' : 'Guardar'}
                icon="pi pi-save"
                severity="success"
                onClick={handleGuardar}
                disabled={isBusy || isDisabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'}
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handleAvanzar}
                disabled={isBusy || !canAdvance}
                className="btn-responsive"
              />

              <Button
                type="button"
                label="Salir"
                icon="pi pi-sign-out"
                severity="secondary"
                outlined
                onClick={handleSalir}
                disabled={isBusy}
                className="btn-responsive"
              />
            </div>
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}
