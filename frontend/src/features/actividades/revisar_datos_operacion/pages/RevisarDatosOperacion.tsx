import { useEffect, useMemo, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Checkbox } from 'primereact/checkbox';
import { InputTextarea } from 'primereact/inputtextarea';
import { TabPanel, TabView } from 'primereact/tabview';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import { useControlesFiadorGarante } from '@/features/actividades/datos_operacion/hooks/useControlesFiadorGarante';
import CompradorSection from '../components/CompradorSection';
import DatosCreditoSection from '../components/DatosCreditoSection';
import FiadorGaranteSection from '../components/FiadorGaranteSection';
import PropiedadSection from '../components/PropiedadSection';
import RevisarDatosOperacionBancoSection from '../components/RevisarDatosOperacionBancoSection';
import VendedorSection from '../components/VendedorSection';
import { useAvanzarRevisarDatosOperacion } from '../hooks/useAvanzarRevisarDatosOperacion';
import { useControlesComprador } from '../hooks/useControlesComprador';
import { useControlesCredito } from '../hooks/useControlesCredito';
import { useControlesPropiedad } from '../hooks/useControlesPropiedad';
import { useControlesRevisarDatosOperacionBanco } from '../hooks/useControlesRevisarDatosOperacionBanco';
import { useControlesVendedor } from '../hooks/useControlesVendedor';
import { useRevisarDatosOperacion } from '../hooks/useRevisarDatosOperacion';
import { useRevisarDatosOperacionFiadores } from '../hooks/useRevisarDatosOperacionFiadores';
import { useUpsertRevisarDatosOperacion } from '../hooks/useUpsertRevisarDatosOperacion';
import {
  EMPTY_CONTROLES_COMPRADOR,
  EMPTY_CONTROLES_CREDITO,
  EMPTY_CONTROLES_PROPIEDAD,
  EMPTY_CONTROLES_REVISAR_DATOS_OPERACION_BANCO,
  EMPTY_CONTROLES_VENDEDOR,
  type CatalogoOption,
} from '../models/catalogo';
import {
  buildCreditoEmpty,
  buildDatosOperacionEmpty,
  buildPropiedadEmpty,
  buildRevisarDatosOperacionBancoEmpty,
  type RevisarDatosOperacion,
  type RevisarDatosOperacionBanco,
  type RevisarDatosOperacionComprador,
  type RevisarDatosOperacionCredito,
  type RevisarDatosOperacionFiadorGarante,
  type RevisarDatosOperacionPropiedad,
  type RevisarDatosOperacionVendedor,
} from '../models/revisar_datos_operacion';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';

const ACTIVITY_ID = '_P1rFxU9vEeiJs6_Y4tW0n';
const CREDITO_TAB_INDEX = 0;
const COMPRADOR_TAB_INDEX = 1;
const VENDEDOR_TAB_INDEX = 2;
const FIADOR_TAB_INDEX = 3;
const BANCO_TAB_INDEX = 4;
const PROPIEDAD_TAB_INDEX = 5;

const isEmptyValue = (value: unknown): boolean =>
  value === null || value === undefined || (typeof value === 'string' && value.trim() === '');

const isMissingBoolean = (value?: boolean | null): boolean =>
  value === null || value === undefined;

type ValidationResult = {
  isValid: boolean;
  tabIndex: number;
  message: string;
};

const validResult: ValidationResult = {
  isValid: true,
  tabIndex: 0,
  message: '',
};

type Props = {
  idExpediente?: number;
  idActividad?: string;
};

export default function RevisarDatosOperacionPage({
  idExpediente: idExpedienteProp,
  idActividad = ACTIVITY_ID,
}: Props) {
  const navigate = useNavigate();
  const toast = useRef<Toast>(null);
  const { id_expediente: idParam } = useParams();

  const idExpediente = Number(idExpedienteProp ?? idParam ?? 0);
  const hasValidExpediente = idExpediente > 0;
  const resolvedActividadId = idActividad || ACTIVITY_ID;

  const [activeTabIndex, setActiveTabIndex] = useState(CREDITO_TAB_INDEX);
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(Boolean(resolvedActividadId));
  const [isSavingAll, setIsSavingAll] = useState(false);
  const [compradoresDirty, setCompradoresDirty] = useState(false);
  const [vendedoresDirty, setVendedoresDirty] = useState(false);
  const [fiadoresDirty, setFiadoresDirty] = useState(false);
  const [form, setForm] = useState<RevisarDatosOperacion>(() =>
    buildDatosOperacionEmpty(idExpediente),
  );

  const revisarDatosOperacionQuery = useRevisarDatosOperacion(idExpediente);
  const fiadoresQuery = useRevisarDatosOperacionFiadores(idExpediente);
  const saveMutation = useUpsertRevisarDatosOperacion();
  const avanzarMutation = useAvanzarRevisarDatosOperacion();
  const controlesCreditoQuery = useControlesCredito(hasValidExpediente);
  const controlesBancoQuery = useControlesRevisarDatosOperacionBanco(hasValidExpediente);
  const controlesPropiedadQuery = useControlesPropiedad(hasValidExpediente);
  const controlesCompradorQuery = useControlesComprador(hasValidExpediente);
  const controlesVendedorQuery = useControlesVendedor(hasValidExpediente);
  const controlesFiadorQuery = useControlesFiadorGarante(hasValidExpediente);

  const controlesCredito = controlesCreditoQuery.data?.status
    ? controlesCreditoQuery.data.detail ?? EMPTY_CONTROLES_CREDITO
    : EMPTY_CONTROLES_CREDITO;

  const isLoadingControlesCredito = controlesCreditoQuery.isLoading;

  const controlesBanco = controlesBancoQuery.data?.status
    ? controlesBancoQuery.data.detail ?? EMPTY_CONTROLES_REVISAR_DATOS_OPERACION_BANCO
    : EMPTY_CONTROLES_REVISAR_DATOS_OPERACION_BANCO;

  const controlesPropiedad = controlesPropiedadQuery.data?.status
    ? controlesPropiedadQuery.data.detail ?? EMPTY_CONTROLES_PROPIEDAD
    : EMPTY_CONTROLES_PROPIEDAD;

  const controlesComprador = controlesCompradorQuery.data?.status
    ? controlesCompradorQuery.data.detail ?? EMPTY_CONTROLES_COMPRADOR
    : EMPTY_CONTROLES_COMPRADOR;

  const isLoadingControlesComprador = controlesCompradorQuery.isLoading;

  const controlesVendedor = controlesVendedorQuery.data?.status
    ? controlesVendedorQuery.data.detail ?? EMPTY_CONTROLES_VENDEDOR
    : EMPTY_CONTROLES_VENDEDOR;

  const isLoadingControlesBanco = controlesBancoQuery.isLoading;
  const isLoadingControlesPropiedad = controlesPropiedadQuery.isLoading;
  const isLoadingControlesVendedor = controlesVendedorQuery.isLoading;
  const isLoadingControlesFiador = controlesFiadorQuery.isLoading;
  const controlesFiador = controlesFiadorQuery.data?.status
    ? controlesFiadorQuery.data.detail ?? undefined
    : undefined;
  const isLoading = revisarDatosOperacionQuery.isLoading;
  const isBusy = isSavingAll || avanzarMutation.isPending;

  const normalizeForm = (
    data: RevisarDatosOperacion | null | undefined,
  ): RevisarDatosOperacion => {
    const cabecera = data ?? buildDatosOperacionEmpty(idExpediente);
    const idRevisarDatosOperacion = cabecera.id_revisar_datos_operacion ?? 0;

    return {
      ...buildDatosOperacionEmpty(idExpediente),
      ...cabecera,
      id_expediente: cabecera.id_expediente || idExpediente,
      credito:
        cabecera.credito ??
        buildCreditoEmpty(idExpediente, idRevisarDatosOperacion),
      revisar_datos_operacion_banco:
        cabecera.revisar_datos_operacion_banco ??
        buildRevisarDatosOperacionBancoEmpty(idExpediente, idRevisarDatosOperacion),
      propiedad:
        cabecera.propiedad ??
        buildPropiedadEmpty(idExpediente, idRevisarDatosOperacion),
      compradores: cabecera.compradores ?? [],
      vendedores: cabecera.vendedores ?? [],
      fiadores_garantes: cabecera.fiadores_garantes ?? [],
    };
  };

  useEffect(() => {
    if (!hasValidExpediente) {
      setForm(buildDatosOperacionEmpty(0));
      return;
    }

    if (revisarDatosOperacionQuery.data?.status && revisarDatosOperacionQuery.data.detail) {
      const normalized = normalizeForm(revisarDatosOperacionQuery.data.detail);
      setForm((prev) => ({
        ...normalized,
        fiadores_garantes: prev.fiadores_garantes?.length ? prev.fiadores_garantes : normalized.fiadores_garantes,
      }));
      setIsDisabled(true);
      setCanAdvance(Boolean(resolvedActividadId));
    }
  }, [revisarDatosOperacionQuery.data, hasValidExpediente, resolvedActividadId]);

  useEffect(() => {
    if (!fiadoresQuery.data?.status || !fiadoresQuery.data.detail) return;
    setForm((prev) => ({ ...prev, fiadores_garantes: fiadoresQuery.data.detail }));
  }, [fiadoresQuery.data]);

  const showToast = (
    severity: 'success' | 'info' | 'warn' | 'error',
    summary: string,
    detail: string,
  ) => {
    toast.current?.show({
      severity,
      summary,
      detail,
      life: 3500,
    });
  };

  const handleEditar = () => {
    if (!hasValidExpediente) {
      showToast('warn', 'Validación', 'El expediente es obligatorio.');
      return;
    }

    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  const normalizeEntityId = (value: number | null | undefined): number => {
    const id = Number(value ?? 0);
    return id > 0 ? id : 0;
  };

  const resolveCatalogDescription = (
    options: CatalogoOption[],
    value?: string | null,
  ): string | null => {
    if (!value) return null;

    const normalizedValue = String(value).trim();
    const option = options.find(
      (item) =>
        String(item.code ?? '').trim() === normalizedValue ||
        String(item.description ?? '').trim() === normalizedValue,
    );

    return option?.description ?? normalizedValue;
  };

  const compradoresNeedSave = () =>
    compradoresDirty ||
    form.compradores.some((c) => c.row_status !== false && c.id_revisar_datos_operacion_comprador <= 0);

  const vendedoresNeedSave = () =>
    vendedoresDirty ||
    form.vendedores.some((v) => v.row_status !== false && v.id_revisar_datos_operacion_vendedor <= 0);

  const fiadoresNeedSave = () =>
    fiadoresDirty ||
    (form.fiadores_garantes ?? []).some(
      (f) => f.row_status !== false && f.id_revisar_datos_operacion_fiador_garante <= 0,
    );

  const preparePayload = (): RevisarDatosOperacion => {
    const idRevisarDatosOperacion = normalizeEntityId(form.id_revisar_datos_operacion);
    const idExp = form.id_expediente || idExpediente;

    return {
      ...form,
      id_revisar_datos_operacion: idRevisarDatosOperacion,
      id_expediente: idExp,
      enviar_reparo: form.enviar_reparo ?? false,
      is_active: form.is_active ?? true,
      row_status: form.row_status ?? true,
      fiadores_garantes: fiadoresNeedSave() ? form.fiadores_garantes : [],
      credito: form.credito
        ? {
            ...form.credito,
            id_revisar_datos_operacion_credito: normalizeEntityId(
              form.credito.id_revisar_datos_operacion_credito,
            ),
            id_revisar_datos_operacion: idRevisarDatosOperacion,
            id_expediente: idExp,
            is_active: form.credito.is_active ?? true,
            row_status: form.credito.row_status ?? true,
          }
        : null,
      revisar_datos_operacion_banco: form.revisar_datos_operacion_banco
        ? {
            ...form.revisar_datos_operacion_banco,
            id_revisar_datos_operacion_banco: normalizeEntityId(
              form.revisar_datos_operacion_banco.id_revisar_datos_operacion_banco,
            ),
            id_revisar_datos_operacion: idRevisarDatosOperacion,
            id_expediente: idExp,
            is_active: form.revisar_datos_operacion_banco.is_active ?? true,
            row_status: form.revisar_datos_operacion_banco.row_status ?? true,
          }
        : null,
      propiedad: form.propiedad
        ? {
            ...form.propiedad,
            id_revisar_datos_operacion_propiedad: normalizeEntityId(
              form.propiedad.id_revisar_datos_operacion_propiedad,
            ),
            id_revisar_datos_operacion: idRevisarDatosOperacion,
            id_expediente: idExp,
            region: resolveCatalogDescription(controlesPropiedad.region, form.propiedad.region),
            comuna: resolveCatalogDescription(controlesPropiedad.comuna, form.propiedad.comuna),
          }
        : null,
    };
  };

  const validateCredito = (): ValidationResult => {
    const credito = form.credito;

    if (!credito) {
      return {
        isValid: false,
        tabIndex: CREDITO_TAB_INDEX,
        message: 'Debe completar la pestaña Datos del Crédito.',
      };
    }

    if (isEmptyValue(credito.tipo_operacion)) {
      return {
        isValid: false,
        tabIndex: CREDITO_TAB_INDEX,
        message: 'Debe completar el campo Tipo Operación en la pestaña Datos del Crédito.',
      };
    }

    if (credito.monto_credito_afecto == null || credito.monto_credito_afecto <= 0) {
      return {
        isValid: false,
        tabIndex: CREDITO_TAB_INDEX,
        message: 'Debe ingresar el Monto Crédito Afecto en la pestaña Datos del Crédito.',
      };
    }

    return validResult;
  };

  const validateBanco = (): ValidationResult => {
    const banco = form.revisar_datos_operacion_banco;

    if (!banco) {
      return {
        isValid: false,
        tabIndex: BANCO_TAB_INDEX,
        message: 'Debe completar la pestaña Datos del Banco.',
      };
    }

    const requiredFields: { value: unknown; label: string; boolean?: boolean }[] = [
      { value: banco.cuenta_carta_resguardo, label: 'Cuenta Carta de Resguardo', boolean: true },
      { value: banco.institucion, label: 'Institución' },
      { value: banco.rut_banco_acreedor, label: 'RUT Banco Acreedor' },
    ];

    const missing = requiredFields.find(({ value, boolean }) =>
      boolean ? isMissingBoolean(value as boolean | null | undefined) : isEmptyValue(value),
    );

    if (missing) {
      return {
        isValid: false,
        tabIndex: BANCO_TAB_INDEX,
        message: `Debe completar el campo ${missing.label}.`,
      };
    }

    return validResult;
  };

  const validatePropiedad = (): ValidationResult => {
    const propiedad = form.propiedad;

    if (!propiedad) {
      return {
        isValid: false,
        tabIndex: PROPIEDAD_TAB_INDEX,
        message: 'Debe completar la pestaña Datos de la Propiedad.',
      };
    }

    const requiredFields: { value: unknown; label: string }[] = [
      { value: propiedad.tipo_propiedad, label: 'Tipo Propiedad' },
      { value: propiedad.estado, label: 'Estado' },
      { value: propiedad.tipo_venta, label: 'Tipo Venta' },
      { value: propiedad.tipo_construccion, label: 'Tipo Construcción' },
      { value: propiedad.tipo_direccion, label: 'Tipo Dirección' },
      { value: propiedad.direccion, label: 'Dirección' },
      { value: propiedad.villa_condominio, label: 'Villa / Condominio' },
      { value: propiedad.numero, label: 'Número' },
      { value: propiedad.numero_casa_habitantes, label: 'N° Casa Habitantes' },
      { value: propiedad.conjunto, label: 'Conjunto' },
      { value: propiedad.manzana, label: 'Manzana' },
      { value: propiedad.lote, label: 'Lote' },
      { value: propiedad.region, label: 'Región' },
      { value: propiedad.comuna, label: 'Comuna' },
      { value: propiedad.existe_rol_avaluo, label: 'Existe Rol Avalúo' },
      { value: propiedad.rol_avaluo_1, label: 'Rol Avalúo 1' },
      { value: propiedad.rol_avaluo_2, label: 'Rol Avalúo 2' },
      { value: propiedad.valor_avaluo_pesos, label: 'Valor Avalúo Pesos' },
    ];

    const missing = requiredFields.find(({ value }) => isEmptyValue(value));

    if (missing) {
      return {
        isValid: false,
        tabIndex: PROPIEDAD_TAB_INDEX,
        message: `Debe completar el campo ${missing.label}.`,
      };
    }

    return validResult;
  };

  const validateRequiredFields = (): ValidationResult => {
    const validations = [validateCredito(), validateBanco(), validatePropiedad()];
    return validations.find((validation) => !validation.isValid) ?? validResult;
  };

  const handleGuardar = async () => {
    if (!hasValidExpediente) {
      showToast('warn', 'Validación', 'El expediente es obligatorio.');
      return;
    }

    const validation = validateRequiredFields();
    if (!validation.isValid) {
      setActiveTabIndex(validation.tabIndex);
      showToast('warn', 'Validación', validation.message);
      return;
    }

    setIsSavingAll(true);
    try {
      const response = await saveMutation.mutateAsync(preparePayload());

      if (!response.status) {
        showToast('error', 'Error', response.message ?? 'No se pudo guardar la actividad.');
        return;
      }

      const idRevisarDatosOperacion =
        response.detail?.id_revisar_datos_operacion ?? form.id_revisar_datos_operacion ?? 0;
      const idExp = form.id_expediente || idExpediente;

      let savedCompradores: RevisarDatosOperacionComprador[] = form.compradores;
      if (compradoresNeedSave()) {
        const deletedCompradores = form.compradores.filter(
          (c) => c.row_status === false && c.id_revisar_datos_operacion_comprador > 0,
        );
        const activeCompradores = form.compradores.filter((c) => c.row_status !== false);

        for (const c of deletedCompradores) {
          await revisarDatosOperacionService.deleteComprador(c.id_revisar_datos_operacion_comprador);
        }

        savedCompradores = [];
        for (const c of activeCompradores) {
          try {
            const payload: RevisarDatosOperacionComprador = {
              ...c,
              id_revisar_datos_operacion_comprador:
                c.id_revisar_datos_operacion_comprador > 0
                  ? c.id_revisar_datos_operacion_comprador
                  : 0,
              id_revisar_datos_operacion: idRevisarDatosOperacion,
              id_expediente: idExp,
              is_active: true,
              row_status: true,
            };
            const res = await revisarDatosOperacionService.saveComprador(payload);
            savedCompradores.push(res.status && res.detail ? res.detail : c);
          } catch {
            savedCompradores.push(c);
          }
        }
      }

      let savedVendedores: RevisarDatosOperacionVendedor[] = form.vendedores;
      if (vendedoresNeedSave()) {
        const deletedVendedores = form.vendedores.filter(
          (v) => v.row_status === false && v.id_revisar_datos_operacion_vendedor > 0,
        );
        const activeVendedores = form.vendedores.filter((v) => v.row_status !== false);

        for (const v of deletedVendedores) {
          await revisarDatosOperacionService.deleteVendedor(v.id_revisar_datos_operacion_vendedor);
        }

        savedVendedores = [];
        for (const v of activeVendedores) {
          try {
            const payload: RevisarDatosOperacionVendedor = {
              ...v,
              id_revisar_datos_operacion_vendedor:
                v.id_revisar_datos_operacion_vendedor > 0
                  ? v.id_revisar_datos_operacion_vendedor
                  : 0,
              id_revisar_datos_operacion: idRevisarDatosOperacion,
              id_expediente: idExp,
              is_active: true,
              row_status: true,
            };
            const res = await revisarDatosOperacionService.saveVendedor(payload);
            savedVendedores.push(res.status && res.detail ? res.detail : v);
          } catch {
            savedVendedores.push(v);
          }
        }
      }

      const savedFiadores = fiadoresNeedSave()
        ? (response.detail?.fiadores_garantes ?? form.fiadores_garantes ?? [])
        : (form.fiadores_garantes ?? []);

      setCompradoresDirty(false);
      setVendedoresDirty(false);
      setFiadoresDirty(false);

      setForm(normalizeForm({ ...(response.detail ?? form), compradores: savedCompradores, vendedores: savedVendedores, fiadores_garantes: savedFiadores }));
      setIsDisabled(true);
      setCanAdvance(Boolean(resolvedActividadId));
      showToast(
        'success',
        'Guardado',
        response.message ?? 'Revisar Datos Operación guardado correctamente.',
      );
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Error inesperado al guardar.';
      showToast('error', 'Error', message);
    } finally {
      setIsSavingAll(false);
    }
  };

  const handleAvanzar = async () => {
    if (!hasValidExpediente) {
      showToast('warn', 'Validación', 'El expediente es obligatorio.');
      return;
    }

    if (!resolvedActividadId) {
      showToast('warn', 'Validación', 'La actividad no tiene id de workflow válido.');
      return;
    }

    if (!canAdvance) {
      showToast('warn', 'Validación', 'Debe guardar los cambios antes de avanzar.');
      return;
    }

    const validation = validateRequiredFields();
    if (!validation.isValid) {
      setActiveTabIndex(validation.tabIndex);
      showToast('warn', 'Validación', validation.message);
      return;
    }

    try {
      const response = await avanzarMutation.mutateAsync(idExpediente);

      if (!response.status) {
        showToast('error', 'Error', response.message ?? 'No se pudo avanzar la actividad.');
        return;
      }

      showToast('success', 'Avance', response.message ?? 'Actividad avanzada correctamente.');
      navigate('/home/bandeja');
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Error inesperado al avanzar.';
      showToast('error', 'Error', message);
    }
  };

  const updateCreditoField = <K extends keyof RevisarDatosOperacionCredito>(
    field: K,
    value: RevisarDatosOperacionCredito[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      credito: {
        ...(prev.credito ?? buildCreditoEmpty(idExpediente, prev.id_revisar_datos_operacion)),
        [field]: value,
      },
    }));
  };

  const updateBancoField = <K extends keyof RevisarDatosOperacionBanco>(
    field: K,
    value: RevisarDatosOperacionBanco[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      revisar_datos_operacion_banco: {
        ...(prev.revisar_datos_operacion_banco ??
          buildRevisarDatosOperacionBancoEmpty(idExpediente, prev.id_revisar_datos_operacion)),
        [field]: value,
      },
    }));
  };

  const updatePropiedadField = <K extends keyof RevisarDatosOperacionPropiedad>(
    field: K,
    value: RevisarDatosOperacionPropiedad[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      propiedad: {
        ...(prev.propiedad ?? buildPropiedadEmpty(idExpediente, prev.id_revisar_datos_operacion)),
        [field]: value,
      },
    }));
  };

  const updateCompradores = (rows: RevisarDatosOperacionComprador[]) => {
    setCompradoresDirty(true);
    setForm((prev) => ({ ...prev, compradores: rows }));
  };

  const updateVendedores = (rows: RevisarDatosOperacionVendedor[]) => {
    setVendedoresDirty(true);
    setForm((prev) => ({ ...prev, vendedores: rows }));
  };

  const updateFiadoresGarantes = (rows: RevisarDatosOperacionFiadorGarante[]) => {
    setFiadoresDirty(true);
    setForm((prev) => ({ ...prev, fiadores_garantes: rows }));
  };

  const tabHeader = useMemo(() => {
    if (isLoading) return 'Cargando Revisar Datos Operación...';
    return 'Revisar Datos Operación';
  }, [isLoading]);

  return (
    <div>
      <Toast ref={toast} />

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad idExpediente={idExpediente} activityID={resolvedActividadId} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales
            idExpediente={idExpediente}
            idActividad={resolvedActividadId}
          />
        </AccordionTab>

        <AccordionTab header={tabHeader} disabled={!hasValidExpediente}>
          {!hasValidExpediente ? (
            <Card>
              <div className="text-center text-600">
                No se recibió un expediente válido para esta actividad.
              </div>
            </Card>
          ) : (
            <div className="rounded-xl border border-gray-200 bg-white p-3 sm:p-4 shadow-sm">
              <div className="mb-4 flex flex-col gap-1 border-b border-gray-100 pb-3">
                <h2 className="text-lg font-semibold text-gray-800">
                  Revisar Datos Operación
                </h2>
              </div>

              <TabView
                className="mi-tabview"
                activeIndex={activeTabIndex}
                onTabChange={(e) => setActiveTabIndex(e.index)}
              >
                <TabPanel header="Datos del Crédito">
                  <DatosCreditoSection
                    value={form.credito}
                    disabled={isDisabled || isBusy}
                    controles={controlesCredito}
                    loadingControles={isLoadingControlesCredito}
                    onChange={updateCreditoField}
                  />
                </TabPanel>

                <TabPanel header="Datos del Comprador">
                  <CompradorSection
                    value={form.compradores}
                    idExpediente={idExpediente}
                    idRevisarDatosOperacion={form.id_revisar_datos_operacion ?? 0}
                    disabled={isDisabled || isBusy}
                    controles={controlesComprador}
                    loadingControles={isLoadingControlesComprador}
                    onChange={updateCompradores}
                    onWarn={(msg) => showToast('warn', 'Validación', msg)}
                  />
                </TabPanel>

                <TabPanel header="Datos del Vendedor">
                  <VendedorSection
                    value={form.vendedores}
                    idExpediente={idExpediente}
                    idRevisarDatosOperacion={form.id_revisar_datos_operacion ?? 0}
                    disabled={isDisabled || isBusy}
                    controles={controlesVendedor}
                    loadingControles={isLoadingControlesVendedor}
                    onChange={updateVendedores}
                    onWarn={(msg) => showToast('warn', 'Validación', msg)}
                  />
                </TabPanel>

                <TabPanel header="Datos del Fiador / Garante">
                  <FiadorGaranteSection
                    value={form.fiadores_garantes ?? []}
                    idExpediente={idExpediente}
                    idRevisarDatosOperacion={form.id_revisar_datos_operacion ?? 0}
                    disabled={isDisabled || isBusy}
                    canEdit={!isDisabled}
                    controles={controlesFiador}
                    loadingControles={isLoadingControlesFiador}
                    onChange={updateFiadoresGarantes}
                    onWarn={(msg) => showToast('warn', 'Validación', msg)}
                  />
                </TabPanel>

                <TabPanel header="Datos del Banco">
                  <RevisarDatosOperacionBancoSection
                    value={form.revisar_datos_operacion_banco}
                    disabled={isDisabled || isBusy}
                    controles={controlesBanco}
                    loadingControles={isLoadingControlesBanco}
                    onChange={updateBancoField}
                  />
                </TabPanel>

                <TabPanel header="Datos de la Propiedad">
                  <PropiedadSection
                    value={form.propiedad}
                    disabled={isDisabled || isBusy}
                    controles={controlesPropiedad}
                    loadingControles={isLoadingControlesPropiedad}
                    onChange={updatePropiedadField}
                  />
                </TabPanel>
              </TabView>

              <div className="mt-6">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Estatus de la Actividad
                </div>
                <div className="flex items-center gap-3">
                  <Checkbox
                    inputId="enviar_reparo"
                    className="form-checkbox-presto"
                    checked={form.enviar_reparo ?? false}
                    disabled={isDisabled || isBusy}
                    onChange={(e) =>
                      setForm((prev) => ({ ...prev, enviar_reparo: e.checked ?? false }))
                    }
                  />
                  <label htmlFor="enviar_reparo" className="font-semibold text-sm">
                    ¿Enviar a Reparo?
                  </label>
                </div>

                <div className="mt-4 flex flex-col gap-1">
                  <label htmlFor="observaciones_global" className="font-semibold text-sm">
                    Observaciones
                  </label>
                  <InputTextarea
                    id="observaciones_global"
                    value={form.observaciones ?? ''}
                    disabled={isDisabled || isBusy}
                    className="form-input-presto w-full"
                    rows={4}
                    autoResize
                    placeholder="Ingrese observaciones"
                    onChange={(e) =>
                      setForm((prev) => ({ ...prev, observaciones: e.target.value }))
                    }
                  />
                </div>
              </div>

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
                  label={isSavingAll ? 'Guardando...' : 'Guardar'}
                  icon="pi pi-save"
                  severity="success"
                  disabled={isBusy || isDisabled}
                  loading={isSavingAll}
                  onClick={handleGuardar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'}
                  icon="pi pi-arrow-right"
                  severity="warning"
                  disabled={isBusy || !canAdvance || !resolvedActividadId}
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
            </div>
          )}
        </AccordionTab>
      </Accordion>
    </div>
  );
}
