import { useEffect, useRef, useState } from 'react';
import { AxiosError } from 'axios';
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
import { useValidarInformacionConEncabezado } from
  '../hooks/useValidarInformacionConEncabezado';
import { useUpsertValidarInformacion } from '../hooks/useUpsertValidarInformacion';
import { useAvanzarValidarInformacion } from '../hooks/useAvanzarValidarInformacion';
import { useControlesValidarInformacion } from '../hooks/useControlesValidarInformacion';
import {
  EMPTY_VALIDAR_INFORMACION,
  type ValidarInformacionBBVA,
} from '../models/validar_informacion';
import {
  EMPTY_ENCABEZADO_VALIDAR_INFORMACION,
} from '../models/encabezado_validar_informacion';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from '../models/catalogo';

const ACTIVIDAD_ID = 'ACT_VALIDAR_INFO';

type RequiredFieldsError = {
  detail?: string;
  message?: string;
  campos_faltantes?: string[];
};

function getRequiredFieldsFromError(error: unknown): string[] {
  const axiosError = error as AxiosError<RequiredFieldsError>;
  const data = axiosError.response?.data;

  if (Array.isArray(data?.campos_faltantes)) {
    return data.campos_faltantes;
  }

  const detail = data?.detail ?? data?.message ?? axiosError.message;
  const match = detail?.match(/Datos Obligatorios Faltantes:\s*(.+)$/i);
  if (!match?.[1]) return [];

  return match[1]
    .split(',')
    .map((field) => field.trim())
    .filter(Boolean);
}

function formatFieldName(field: string): string {
  const names: Record<string, string> = {
    telefono_t1: 'Teléfono Residente T1',
    direccion_t1: 'Dirección Residencia T1',
    tipo_id_t1: 'Tipo de Identificación T1',
    numero_id_t1: 'Número de Identificación T1',
    nombre_completo_t1: 'Nombre Completo T1',
    celular_t1: 'Celular Cliente T1',
    email_t1: 'Email T1',
    tipo_credito: 'Tipo de Crédito',
    tiene_garantia: '¿Tiene Garantía Constituida?',
    monto_otorgado_vi: 'Monto Otorgado para Vivienda',
    correo_declarativo: 'Correo Declarativo',
    telefono_declarativo: 'Teléfono Declarativo',
    estatus_general: 'Estatus General',
    motivo_devolucion: 'Motivo de Devolución',
  };

  return names[field] ?? field.replaceAll('_', ' ');
}

function validateRequiredFields(
  form: ValidarInformacionBBVA,
  montoOriginal: number | null | undefined,
): Set<string> {
  const missing = new Set<string>();

  if (!form.tipo_id_t1) missing.add('tipo_id_t1');
  if (!form.numero_id_t1) missing.add('numero_id_t1');
  if (!form.nombre_completo_t1) missing.add('nombre_completo_t1');
  if (!form.celular_t1) missing.add('celular_t1');
  if (!form.telefono_t1) missing.add('telefono_t1');
  if (!form.email_t1) missing.add('email_t1');
  if (!form.direccion_t1) missing.add('direccion_t1');

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

  const { data: conEncabezadoData, isLoading } =
    useValidarInformacionConEncabezado(id_expediente);
  const { data: controlesData } = useControlesValidarInformacion(id_expediente);
  const { mutate: guardar, isPending: isGuardando } = useUpsertValidarInformacion();
  const { mutate: avanzar, isPending: isAvanzando } = useAvanzarValidarInformacion();

  const encabezado =
    conEncabezadoData?.detail?.encabezado ?? EMPTY_ENCABEZADO_VALIDAR_INFORMACION;
  const controles = controlesData?.detail ?? EMPTY_CONTROLES_VALIDAR_INFORMACION;

  useEffect(() => {
    const formulario = conEncabezadoData?.detail?.formulario;
    if (formulario) {
      // La pantalla edita una copia local del formulario recibido desde el endpoint.
      // eslint-disable-next-line react-hooks/set-state-in-effect
      setForm(formulario);
    }
  }, [conEncabezadoData]);

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
    const missing = validateRequiredFields(form, encabezado.monto_otorgado_original);
    if (missing.size > 0) {
      setInvalidFields(missing);
      toast.current?.show({
        severity: 'warn',
        summary: 'Datos Obligatorios Faltantes',
        detail: Array.from(missing).map(formatFieldName).join(', '),
        life: 7000,
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
        const backendMissing = getRequiredFieldsFromError(error);
        if (backendMissing.length > 0) {
          setInvalidFields(new Set(backendMissing));
          toast.current?.show({
            severity: 'warn',
            summary: 'Datos Obligatorios Faltantes',
            detail: backendMissing.map(formatFieldName).join(', '),
            life: 7000,
          });
          return;
        }

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

  if (isLoading) return <div className="p-4">Cargando...</div>;

  return (
    <div className="flex flex-col gap-4 p-4">
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Validar Información
      </h2>

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

      <div className="grid grid-cols-1 gap-4">
        <div>
          <Accordion multiple activeIndex={[0]}>
            <AccordionTab header="Información General">
              <EncabezadoActividad
                idExpediente={id_expediente}
                activityID={ACTIVIDAD_ID}
              />
            </AccordionTab>
            <AccordionTab header="Funciones Transversales">
              <FuncionesTransversales
                idExpediente={id_expediente}
                idActividad={ACTIVIDAD_ID}
              />
            </AccordionTab>
            <AccordionTab header="Datos Titular">
              <DatosTitularSection
                id_expediente={id_expediente}
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
        </div>
      </div>
    </div>
  );
}
