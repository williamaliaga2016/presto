import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { InputSwitch } from 'primereact/inputswitch';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import GestionarFirmaElectronicaSection from '../components/GestionarFirmaElectronicaSection';
import GestionarFirmaProgramacionSection from '../components/GestionarFirmaProgramacionSection';
import type { GestionarFirma } from '../models/gestionar_firma';
import { useAvanzarGestionarFirma } from '../hooks/useAvanzarGestionarFirma';
import { useGestionarFirma } from '../hooks/useGestionarFirma';
import { useUpsertGestionarFirma } from '../hooks/useUpsertGestionarFirma';

/**
 * TODO:
 * Reemplazar este valor por el id real de la actividad en el motor/BPMN
 * cuando ya este registrado el catalogo de actividades.
 */
const ACTIVITY_ID = '_qBTPVMV-EeypJfcPB6uWpg';

const buildInitialState = (id_expediente: number): GestionarFirma => ({
  id: 0,
  id_expediente,
  id_actividad: 'ACT_GESTIONAR_FIRMA',
  requiere_firma_electronica: false,
  firma_electronica_realizada: false,
  nombre_cliente_firma: null,
  nombre_solicitante_firma: null,
  franja_horaria: '',
  fecha_programacion: null,
  ciudad_cliente: '',
  direccion_firma: '',
  descripcion_tramite: '',
  tipo_credito_firma: null,
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

const normalizeGestionarFirma = (
  source: Partial<GestionarFirma> | null | undefined,
  id_expediente_fallback: number,
): GestionarFirma => ({
  id: Number(source?.id ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
  id_actividad: source?.id_actividad ?? 'ACT_GESTIONAR_FIRMA',
  requiere_firma_electronica: Boolean(source?.requiere_firma_electronica ?? false),
  firma_electronica_realizada: Boolean(source?.firma_electronica_realizada ?? false),
  nombre_cliente_firma: source?.nombre_cliente_firma ?? null,
  nombre_solicitante_firma: source?.nombre_solicitante_firma ?? null,
  franja_horaria: source?.franja_horaria ?? '',
  fecha_programacion: normalizeDate(source?.fecha_programacion),
  ciudad_cliente: source?.ciudad_cliente ?? '',
  direccion_firma: source?.direccion_firma ?? '',
  descripcion_tramite: source?.descripcion_tramite ?? '',
  tipo_credito_firma: source?.tipo_credito_firma ?? null,
  observaciones: source?.observaciones ?? null,
});

const extractFormularioFromDetail = (
  detail: unknown,
): Partial<GestionarFirma> | null => {
  if (!detail || typeof detail !== 'object') {
    return null;
  }

  const detailRecord = detail as Record<string, unknown>;
  const nestedFormulario = detailRecord.formulario;

  if (nestedFormulario && typeof nestedFormulario === 'object') {
    return nestedFormulario as Partial<GestionarFirma>;
  }

  return detail as Partial<GestionarFirma>;
};

export default function GestionarFirmaPage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<GestionarFirma>(buildInitialState(id_expediente));
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useGestionarFirma(id_expediente);
  const saveMutation = useUpsertGestionarFirma();
  const avanzarMutation = useAvanzarGestionarFirma();

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
      const loadedEntity = normalizeGestionarFirma(formulario, id_expediente);

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
      setForm((prev) =>
        normalizeGestionarFirma(
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

  const updateField = <K extends keyof GestionarFirma>(
    field: K,
    value: GestionarFirma[K],
  ) => {
    setForm((prev) => ({
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

    if (form.requiere_firma_electronica) {
      if (!form.firma_electronica_realizada) {
        return 'Debe marcar Firma Electronica Realizada.';
      }

      return '';
    }

    if (!form.franja_horaria?.trim()) {
      return 'Debe ingresar la franja horaria.';
    }

    if (!form.fecha_programacion) {
      return 'Debe ingresar la fecha de programacion de diligencia.';
    }

    if (!form.ciudad_cliente?.trim()) {
      return 'Debe ingresar la ciudad ubicada del cliente.';
    }

    if (!form.direccion_firma?.trim()) {
      return 'Debe ingresar la direccion.';
    }

    if (!form.descripcion_tramite?.trim()) {
      return 'Debe ingresar la descripcion del tramite.';
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

      const payload: GestionarFirma = normalizeGestionarFirma(
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
          detail: 'Gestion de firma guardada correctamente',
          life: 3000,
        });

        const formularioGuardado = extractFormularioFromDetail(response.detail ?? payload);
        const savedEntity = normalizeGestionarFirma(formularioGuardado, payload.id_expediente);

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
      console.error('ERROR GUARDAR GESTIONAR FIRMA', error);

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
      console.error('ERROR AVANZAR GESTIONAR FIRMA', error);
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

      <h2 className="text-2xl font-bold text-gray-900 mb-6">Gestionar Firma</h2>

      <Accordion activeIndex={[0, 2]} multiple>
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

        <AccordionTab header="Gestionar Firma">
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

            <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-4">
              <div className="md:col-span-3 flex items-center pt-1 pb-2 min-h-[42px]">
                <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
                  ¿Requiere Firma Electronica?
                </label>
                <div className="ml-8 md:ml-56 flex items-center h-8">
                  <InputSwitch
                    checked={Boolean(form.requiere_firma_electronica)}
                    onChange={(e) => {
                      const checked = Boolean(e.value);

                      setForm((prev) => ({
                        ...prev,
                        requiere_firma_electronica: checked,
                        firma_electronica_realizada: checked
                          ? prev.firma_electronica_realizada
                          : false,
                      }));
                    }}
                    disabled={isDisabled}
                  />
                </div>
              </div>

              <div className="md:col-span-3 border-t border-slate-200 my-1" />

              {form.requiere_firma_electronica ? (
                <GestionarFirmaElectronicaSection
                  form={form}
                  isDisabled={isDisabled}
                  updateField={updateField}
                />
              ) : (
                <GestionarFirmaProgramacionSection
                  form={form}
                  isDisabled={isDisabled}
                  updateField={updateField}
                />
              )}
            </div>

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
