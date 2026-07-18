import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosOperacionPersonaGrid from '@/features/actividades/datos_operacion/components/DatosOperacionPersonaGrid';
import { useControlesVendedor } from '@/features/actividades/datos_operacion/hooks/useControlesVendedor';
import {
  buildBancoAcreedorEmpty,
  buildDatosCreditoEmpty,
  buildDatosOperacionEmpty,
  buildPropiedadEmpty,
  type DatosOperacion,
  type DatosOperacionComprador,
  type DatosOperacionVendedor,
} from '@/features/actividades/datos_operacion/models/datos_operacion';
import { EMPTY_CONTROLES_VENDEDOR } from '@/features/actividades/datos_operacion/models/catalogo';
import { useDatosDelVendedor } from '../hooks/useDatosDelVendedor';
import { useUpsertDatosDelVendedor } from '../hooks/useUpsertDatosDelVendedor';
import { useAvanzarDatosDelVendedor } from '../hooks/useAvanzarDatosDelVendedor';

const ACTIVITY_ID = '';

const isEmptyValue = (value: unknown): boolean =>
  value === null || value === undefined || (typeof value === 'string' && value.trim() === '');

type ValidationResult = { isValid: boolean; message: string };
const validResult: ValidationResult = { isValid: true, message: '' };

const isJuridicaPersona = (row: DatosOperacionComprador | DatosOperacionVendedor): boolean => {
  const tipoPersona = String(row.tipo_persona ?? '').toLowerCase();
  const razonSocial = String(row.razon_social ?? '').trim();
  const nombres = String(row.nombres ?? '').trim();
  return tipoPersona.includes('jur') || (!!razonSocial && !nombres);
};

const getMissingPersonaField = (
  row: DatosOperacionComprador | DatosOperacionVendedor,
  type: 'vendedor',
): string | null => {
  const juridica = isJuridicaPersona(row);
  const commonFields: { value: unknown; label: string }[] = [
    { value: row.rut, label: 'RUT' },
    { value: juridica ? row.razon_social : row.nombres, label: juridica ? 'Razón Social' : 'Nombres' },
    ...(juridica
      ? []
      : [
          { value: row.apellido_paterno, label: 'Apellido Paterno' },
          { value: row.apellido_materno, label: 'Apellido Materno' },
        ]),
    { value: row.direccion, label: 'Dirección' },
    { value: row.region, label: 'Región' },
    { value: row.comuna, label: 'Comuna' },
    { value: row.telefono, label: 'Teléfono' },
    { value: row.email, label: 'Email' },
    { value: row.relacion_titular, label: 'Relación Titular' },
  ];
  const naturalFields: { value: unknown; label: string }[] = juridica
    ? []
    : [
        { value: row.telefono_comercial, label: 'Teléfono Comercial' },
        { value: row.telefono_movil, label: 'Teléfono Móvil' },
        { value: row.profesion, label: 'Profesión' },
        { value: row.email2, label: 'Email 2' },
      ];
  const vendedorJuridicoFields: { value: unknown; label: string }[] =
    type === 'vendedor' && juridica
      ? [
          { value: row.direccion_env_esc, label: 'Dirección Envío Escritura' },
          { value: row.region_env_esc, label: 'Región Envío Escritura' },
          { value: row.comuna_env_esc, label: 'Comuna Envío Escritura' },
        ]
      : [];
  const fields = [...commonFields, ...naturalFields, ...vendedorJuridicoFields];
  return fields.find(({ value }) => isEmptyValue(value))?.label ?? null;
};

const validateVendedores = (rows: DatosOperacionVendedor[] | undefined): ValidationResult => {
  const activeRows = (rows ?? []).filter((row) => row.row_status !== false);
  for (let index = 0; index < activeRows.length; index += 1) {
    const missingField = getMissingPersonaField(activeRows[index], 'vendedor');
    if (missingField) {
      return {
        isValid: false,
        message: `Datos del Vendedor, registro ${index + 1}: debe completar el campo ${missingField}.`,
      };
    }
  }
  return validResult;
};

export default function DatosDelVendedorPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idParam } = useParams();
  const idExpediente = Number(idParam ?? 0);
  const hasValidExpediente = idExpediente > 0;

  const [form, setForm] = useState<DatosOperacion>(() => buildDatosOperacionEmpty(idExpediente));
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(true);

  const datosQuery = useDatosDelVendedor(idExpediente);
  const saveMutation = useUpsertDatosDelVendedor();
  const avanzarMutation = useAvanzarDatosDelVendedor();
  const controlesVendedorQuery = useControlesVendedor(hasValidExpediente);

  const controlesVendedor = controlesVendedorQuery.data?.status
    ? controlesVendedorQuery.data.detail ?? EMPTY_CONTROLES_VENDEDOR
    : EMPTY_CONTROLES_VENDEDOR;

  const isLoading = datosQuery.isLoading;
  const isBusy = saveMutation.isPending || avanzarMutation.isPending;
  const canEditSections = !isDisabled && !isBusy;

  const normalizeForm = (data: DatosOperacion | null | undefined): DatosOperacion => {
    const cabecera = data ?? buildDatosOperacionEmpty(idExpediente);
    const idDatosOperacion = cabecera.id_datos_operacion ?? 0;
    return {
      ...buildDatosOperacionEmpty(idExpediente),
      ...cabecera,
      id_expediente: cabecera.id_expediente || idExpediente,
      datos_credito:
        cabecera.datos_credito ?? buildDatosCreditoEmpty(idExpediente, idDatosOperacion),
      compradores: cabecera.compradores ?? [],
      vendedores: cabecera.vendedores ?? [],
      fiadores_garantes: cabecera.fiadores_garantes ?? [],
      banco_acreedor:
        cabecera.banco_acreedor ?? buildBancoAcreedorEmpty(idExpediente, idDatosOperacion),
      propiedad: cabecera.propiedad ?? buildPropiedadEmpty(idExpediente, idDatosOperacion),
    };
  };

  useEffect(() => {
    if (!hasValidExpediente) {
      setForm(buildDatosOperacionEmpty(0));
      return;
    }
    if (datosQuery.data?.status) {
      setForm(normalizeForm(datosQuery.data.detail));
      setIsDisabled(true);
      setCanAdvance(true);
    }
  }, [datosQuery.data, hasValidExpediente]);

  const showToast = (
    severity: 'success' | 'info' | 'warn' | 'error',
    summary: string,
    detail: string,
  ) => {
    toast.current?.show({ severity, summary, detail, life: 3500 });
  };

  const normalizeEntityId = (value: number | null | undefined): number => {
    const id = Number(value ?? 0);
    return id > 0 ? id : 0;
  };

  const preparePayload = (): DatosOperacion => {
    const idDatosOperacion = normalizeEntityId(form.id_datos_operacion);
    const idExp = form.id_expediente || idExpediente;
    return {
      ...form,
      id_datos_operacion: idDatosOperacion,
      id_expediente: idExp,
      is_active: form.is_active ?? true,
      row_status: form.row_status ?? true,
      datos_credito: form.datos_credito
        ? {
            ...form.datos_credito,
            id_datos_operacion_datos_credito: normalizeEntityId(
              form.datos_credito.id_datos_operacion_datos_credito,
            ),
            id_datos_operacion: idDatosOperacion,
            id_expediente: idExp,
          }
        : null,
      compradores: (form.compradores ?? []).map((item) => ({
        ...item,
        id_datos_operacion_comprador: normalizeEntityId(item.id_datos_operacion_comprador),
        id_datos_operacion: idDatosOperacion,
        id_expediente: idExp,
      })),
      vendedores: (form.vendedores ?? []).map((item) => ({
        ...item,
        id_datos_operacion_vendedor: normalizeEntityId(item.id_datos_operacion_vendedor),
        id_datos_operacion: idDatosOperacion,
        id_expediente: idExp,
      })),
      fiadores_garantes: (form.fiadores_garantes ?? []).map((item) => ({
        ...item,
        id_datos_operacion_fiador_garante: normalizeEntityId(item.id_datos_operacion_fiador_garante),
        id_datos_operacion: idDatosOperacion,
        id_expediente: idExp,
      })),
      banco_acreedor: form.banco_acreedor
        ? {
            ...form.banco_acreedor,
            id_datos_operacion_banco_acreedor: normalizeEntityId(
              form.banco_acreedor.id_datos_operacion_banco_acreedor,
            ),
            id_datos_operacion: idDatosOperacion,
            id_expediente: idExp,
          }
        : null,
      propiedad: form.propiedad
        ? {
            ...form.propiedad,
            id_datos_operacion_propiedad: normalizeEntityId(form.propiedad.id_datos_operacion_propiedad),
            id_datos_operacion: idDatosOperacion,
            id_expediente: idExp,
          }
        : null,
    };
  };

  const handleEditar = () => {
    if (!hasValidExpediente) {
      showToast('warn', 'Validación', 'El expediente es obligatorio.');
      return;
    }
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    if (!hasValidExpediente) {
      showToast('warn', 'Validación', 'El expediente es obligatorio.');
      return;
    }
    const validation = validateVendedores(form.vendedores);
    if (!validation.isValid) {
      showToast('warn', 'Validación', validation.message);
      return;
    }
    try {
      const response = await saveMutation.mutateAsync(preparePayload());
      if (!response.status) {
        showToast('error', 'Error', response.message ?? 'No se pudo guardar.');
        return;
      }
      setForm(normalizeForm(response.detail));
      setIsDisabled(true);
      setCanAdvance(true);
      showToast('success', 'Guardado', response.message ?? 'Datos del vendedor guardados correctamente.');
    } catch (e) {
      showToast('error', 'Error', e instanceof Error ? e.message : 'Error al guardar.');
    }
  };

  const handleAvanzar = async () => {
    if (!hasValidExpediente) {
      showToast('warn', 'Validación', 'El expediente es obligatorio.');
      return;
    }
    if (!canAdvance) {
      showToast('warn', 'Validación', 'Debe guardar los cambios antes de avanzar.');
      return;
    }
    const validation = validateVendedores(form.vendedores);
    if (!validation.isValid) {
      showToast('warn', 'Validación', validation.message);
      return;
    }
    try {
      const response = await avanzarMutation.mutateAsync(idExpediente);
      if (!response.status) {
        showToast('error', 'Error', response.message ?? 'No se pudo avanzar.');
        return;
      }
      showToast('success', 'Avance', response.message ?? 'Actividad avanzada correctamente.');
      navigate('/home/bandeja');
    } catch (e) {
      showToast('error', 'Error', e instanceof Error ? e.message : 'Error al avanzar.');
    }
  };

  const updateVendedores = (vendedores: DatosOperacionVendedor[]) => {
    setForm((prev) => ({ ...prev, vendedores }));
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  const headerTitle = isLoading ? 'Cargando…' : '5.11.3 Datos del vendedor';

  return (
    <div>
      <Toast ref={toast} />
      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad idExpediente={idExpediente} activityID={ACTIVITY_ID} />
        </AccordionTab>
        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales idExpediente={idExpediente} idActividad={ACTIVITY_ID} />
        </AccordionTab>
        <AccordionTab header={headerTitle} disabled={!hasValidExpediente}>
          {!hasValidExpediente ? (
            <Card>
              <div className="text-center text-600">Indique un expediente válido en la ruta.</div>
            </Card>
          ) : (
            <Card className="w-full shadow-md card-presto-form mb-6">
              <p className="mb-4 text-sm text-gray-600">
                Registre o edite los vendedores asociados a la operación. La información se persiste en las tablas de
                datos de operación ya existentes (cabecera y detalle de vendedor), alineado con la actividad 5.4.
              </p>
              <DatosOperacionPersonaGrid
                title="Datos del Vendedor"
                type="vendedor"
                value={form.vendedores ?? []}
                idExpediente={idExpediente}
                idDatosOperacion={form.id_datos_operacion ?? 0}
                disabled={isDisabled || isBusy}
                canEdit={canEditSections}
                controles={controlesVendedor}
                loadingControles={controlesVendedorQuery.isLoading}
                onChange={updateVendedores}
                onWarn={(message) => showToast('warn', 'Validación', message)}
              />

              <div className="form-actions">
                <Button
                  type="button"
                  label="Editar"
                  icon="pi pi-pencil"
                  severity="info"
                  outlined
                  disabled={isBusy || !isDisabled}
                  onClick={handleEditar}
                  className="btn-responsive"
                />
                <Button
                  type="button"
                  label={saveMutation.isPending ? 'Guardando...' : 'Guardar'}
                  icon="pi pi-save"
                  severity="success"
                  disabled={isBusy || isDisabled}
                  loading={saveMutation.isPending}
                  onClick={handleGuardar}
                  className="btn-responsive"
                />
                <Button
                  type="button"
                  label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'}
                  icon="pi pi-arrow-right"
                  severity="warning"
                  disabled={isBusy || !canAdvance}
                  loading={avanzarMutation.isPending}
                  onClick={handleAvanzar}
                  className="btn-responsive"
                />
                <Button
                  type="button"
                  label="Salir"
                  icon="pi pi-sign-out"
                  severity="secondary"
                  outlined
                  disabled={isBusy}
                  onClick={handleSalir}
                  className="btn-responsive"
                />
              </div>
            </Card>
          )}
        </AccordionTab>
      </Accordion>
    </div>
  );
}
