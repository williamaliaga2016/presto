import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Dialog } from 'primereact/dialog';
import { InputText } from 'primereact/inputtext';
import { Toast } from 'primereact/toast';
import type { Toast as ToastRef } from 'primereact/toast';
import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from
  '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosTitularSection from '../components/DatosTitularSection';
import DatosCreditoSection from '../components/DatosCreditoSection';
import CondicionesFinancierasSection from '../components/CondicionesFinancierasSection';
import DatosInmuebleSection from '../components/DatosInmuebleSection';
import DatosComercialesSection from '../components/DatosComercialesSection';
import EstatusGeneralSection from '../components/EstatusGeneralSection';
import RegistroContactoSection from '../components/RegistroContactoSection';
import { useValidarInformacion } from '../hooks/useValidarInformacion';
import { useEncabezado } from '@/features/encabezado/hooks/useEncabezado';
import { useUpsertValidarInformacion } from '../hooks/useUpsertValidarInformacion';
import { useAvanzarValidarInformacion } from '../hooks/useAvanzarValidarInformacion';
import { useControlesValidarInformacion } from '../hooks/useControlesValidarInformacion';
import {
  EMPTY_VALIDAR_INFORMACION,
  type ValidarInformacionBBVA,
} from '../models/validar_informacion';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from '../models/catalogo';

const ACTIVIDAD_ID = 'ACT_VALIDAR_INFO';

function validateRequiredFields(
  form: ValidarInformacionBBVA,
  montoOriginal: number | null | undefined,
): Set<string> {
  const missing = new Set<string>();

  if (!form.tipo_id_t1) missing.add('tipo_id_t1');
  if (!form.numero_id_t1) missing.add('numero_id_t1');
  if (!form.nombre_completo_t1) missing.add('nombre_completo_t1');
  if (!form.celular_t1) missing.add('celular_t1');
  if (!form.email_t1) missing.add('email_t1');

  if (!form.tipo_credito) missing.add('tipo_credito');
  if (form.tiene_garantia === null || form.tiene_garantia === undefined) {
    missing.add('tiene_garantia');
  }

  if (form.monto_otorgado_vi === null || form.monto_otorgado_vi === undefined) {
    missing.add('monto_otorgado_vi');
  } else if (
    montoOriginal !== null &&
    montoOriginal !== undefined &&
    form.monto_otorgado_vi > montoOriginal
  ) {
    missing.add('monto_otorgado_vi');
  }

  if (!form.correo_declarativo) missing.add('correo_declarativo');
  if (!form.telefono_declarativo) missing.add('telefono_declarativo');
  if (!form.estatus_general) missing.add('estatus_general');

  if (form.estatus_general === 'ESCALAR_COMERCIAL' && !form.motivo_devolucion) {
    missing.add('motivo_devolucion');
  }

  return missing;
}

export default function ValidarInformacionPage() {
  const { id_expediente: idParam } = useParams();
  const navigate = useNavigate();
  const id_expediente = Number(idParam ?? 0);
  const toast = useRef<ToastRef>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [invalidFields, setInvalidFields] = useState<Set<string>>(new Set());
  const [accessLink, setAccessLink] = useState<string | null>(null);
  const [form, setForm] = useState<ValidarInformacionBBVA>(
    EMPTY_VALIDAR_INFORMACION(id_expediente),
  );

  const { data: actividadData, isLoading: isLoadingActividad } =
    useValidarInformacion(id_expediente);
  const { data: encabezadoData, isLoading: isLoadingEncabezado } =
    useEncabezado(id_expediente, ACTIVIDAD_ID);
  const { data: controlesData } = useControlesValidarInformacion(id_expediente);
  const { mutate: guardar, isPending: isGuardando } = useUpsertValidarInformacion();
  const { mutate: avanzar, isPending: isAvanzando } = useAvanzarValidarInformacion();

  const encabezado = encabezadoData?.detail ?? null;
  const controles = controlesData?.detail ?? EMPTY_CONTROLES_VALIDAR_INFORMACION;

  useEffect(() => {
    const formulario = actividadData?.detail;
    if (formulario) {
      setForm(formulario);
    }
  }, [actividadData]);

  const updateField = (field: keyof ValidarInformacionBBVA, value: unknown) => {
    setForm((prev) => ({ ...prev, [field]: value }));
    if (!isEditing) setIsEditing(true);
    if (invalidFields.has(field as string)) {
      setInvalidFields((prev) => {
        const next = new Set(prev);
        next.delete(field as string);
        return next;
      });
    }
  };

  const handleGuardar = () => {
    guardar(form, {
      onSuccess: () => {
        setIsEditing(false);
        toast.current?.show({
          severity: 'success',
          summary: 'Guardado',
          detail: 'Información guardada correctamente',
          life: 3000,
        });
      },
      onError: (error) => {
        toast.current?.show({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
          life: 5000,
        });
      },
    });
  };

  /**
   * Avanza la actividad y muestra el link temporal solo cuando el backend lo genera.
   *
   * @returns No retorna valor; actualiza estado visual y notificaciones.
   */
  const handleAvanzar = () => {
    const missing = validateRequiredFields(form, encabezado?.monto_otorgado);
    if (missing.size > 0) {
      setInvalidFields(missing);
      toast.current?.show({
        severity: 'warn',
        summary: 'Campos requeridos',
        detail: 'Complete todos los campos obligatorios antes de avanzar.',
        life: 5000,
      });
      return;
    }
    setInvalidFields(new Set());
    avanzar(id_expediente, {
      onSuccess: (response) => {
        const temporalUrl = response.detail?.acceso_temporal?.url;
        if (temporalUrl) {
          setAccessLink(temporalUrl);
          toast.current?.show({
            severity: 'success',
            summary: 'Avanzado',
            detail: 'Folio avanzado a la siguiente etapa',
            life: 3000,
          });
          return;
        }

        toast.current?.show({
          severity: 'success',
          summary: 'Avanzado',
          detail: 'Folio avanzado a la siguiente etapa',
          life: 3000,
        });
        navigate('/home/bandeja');
      },
      onError: (error) => {
        toast.current?.show({
          severity: 'error',
          summary: 'Error al avanzar',
          detail: error.message,
          life: 5000,
        });
      },
    });
  };

  /**
   * Copia el link temporal visible para facilitar su envio al usuario externo.
   *
   * @returns Promesa resuelta cuando el navegador procesa la copia.
   */
  const handleCopyAccessLink = async () => {
    if (!accessLink) return;

    try {
      await navigator.clipboard.writeText(accessLink);
      toast.current?.show({
        severity: 'success',
        summary: 'Link copiado',
        detail: 'El link de acceso temporal fue copiado.',
        life: 3000,
      });
    } catch {
      toast.current?.show({
        severity: 'warn',
        summary: 'No fue posible copiar',
        detail: 'Selecciona el link y copialo manualmente.',
        life: 5000,
      });
    }
  };

  if (isLoadingActividad || isLoadingEncabezado) {
    return <div className="p-4">Cargando...</div>;
  }

  return (
    <div className="flex flex-col gap-4 p-4">
      <Toast ref={toast} />

      <Dialog
        header="Link de acceso temporal"
        visible={Boolean(accessLink)}
        modal
        className="w-[92vw] max-w-2xl"
        onHide={() => setAccessLink(null)}
        footer={(
          <div className="flex justify-end gap-2">
            <Button
              label="Copiar link"
              icon="pi pi-copy"
              onClick={handleCopyAccessLink}
            />
            <Button
              label="Cerrar"
              icon="pi pi-times"
              severity="secondary"
              onClick={() => setAccessLink(null)}
            />
          </div>
        )}
      >
        <div className="flex flex-col gap-3">
          <p className="m-0 text-sm text-gray-700">
            Este es el link de acceso temporal, compartelo con el usuario externo
            para proceder con la carga de documentos y continuar con el proceso
          </p>
          <InputText
            value={accessLink ?? ''}
            readOnly
            className="w-full"
            onFocus={(event) => event.currentTarget.select()}
          />
        </div>
      </Dialog>

      <Card>
        <Accordion multiple activeIndex={[0]}>
          <AccordionTab header="Información del Expediente">
            <EncabezadoActividad
              idExpediente={id_expediente}
              activityID={ACTIVIDAD_ID}
            />
          </AccordionTab>
        </Accordion>
      </Card>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
        <div className="lg:col-span-2">
          <Card>
            <Accordion multiple>
              <AccordionTab header="Datos Titular">
                <DatosTitularSection
                  data={form}
                  controles={controles}
                  isEditing={true}
                  onChange={updateField}
                  invalidFields={invalidFields}
                />
              </AccordionTab>

              <AccordionTab header="Datos del Crédito">
                <DatosCreditoSection
                  data={form}
                  encabezado={encabezado}
                  controles={controles}
                  isEditing={true}
                  onChange={updateField}
                  invalidFields={invalidFields}
                />
              </AccordionTab>

              <AccordionTab header="Condiciones Financieras">
                <CondicionesFinancierasSection
                  data={form}
                  encabezado={encabezado}
                  isEditing={true}
                  onChange={updateField}
                  invalidFields={invalidFields}
                />
              </AccordionTab>

              <AccordionTab header="Datos del Inmueble">
                <DatosInmuebleSection
                  data={form}
                  controles={controles}
                  isEditing={true}
                  onChange={updateField}
                />
              </AccordionTab>

              <AccordionTab header="Datos Comerciales">
                <DatosComercialesSection
                  data={form}
                  encabezado={encabezado}
                  isEditing={true}
                  onChange={updateField}
                  invalidFields={invalidFields}
                />
              </AccordionTab>

              <AccordionTab header="Registro Contacto">
                <RegistroContactoSection
                  id_expediente={id_expediente}
                  id_actividad={ACTIVIDAD_ID}
                  controles={controles}
                />
              </AccordionTab>

              <AccordionTab header="Estatus General">
                <EstatusGeneralSection
                  data={form}
                  controles={controles}
                  isEditing={true}
                  onChange={updateField}
                  invalidFields={invalidFields}
                />
              </AccordionTab>
            </Accordion>

            <div className="flex justify-end gap-3 mt-4 pt-4 border-t">
              <Button
                label="Guardar"
                icon="pi pi-save"
                severity="secondary"
                loading={isGuardando}
                disabled={!isEditing}
                onClick={handleGuardar}
              />
              <Button
                label="Avanzar"
                icon="pi pi-arrow-right"
                loading={isAvanzando}
                disabled={isEditing}
                onClick={handleAvanzar}
              />
            </div>
          </Card>
        </div>

        <div className="lg:col-span-1">
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVIDAD_ID}
          />
        </div>
      </div>
    </div>
  );
}
