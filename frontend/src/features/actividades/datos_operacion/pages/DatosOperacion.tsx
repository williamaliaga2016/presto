import { useEffect, useMemo, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { TabPanel, TabView } from 'primereact/tabview';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import BancoAcreedorSection from '../components/BancoAcreedorSection';
import DatosCreditoSection from '../components/DatosCreditoSection';
import DatosOperacionPersonaGrid from '../components/DatosOperacionPersonaGrid';
import FiadorGaranteSection from '../components/FiadorGaranteSection';
import PropiedadSection from '../components/PropiedadSection';
import { useAvanzarDatosOperacion } from '../hooks/useAvanzarDatosOperacion';
import { useControlesBancoAcreedor } from '../hooks/useControlesBancoAcreedor';
import { useControlesComprador } from '../hooks/useControlesComprador';
import { useControlesDatosCredito } from '../hooks/useControlesDatosCredito';
import { useControlesFiadorGarante } from '../hooks/useControlesFiadorGarante';
import { useControlesPropiedad } from '../hooks/useControlesPropiedad';
import { useControlesVendedor } from '../hooks/useControlesVendedor';
import {
  EMPTY_CONTROLES_BANCO_ACREEDOR,
  EMPTY_CONTROLES_COMPRADOR,
  EMPTY_CONTROLES_DATOS_CREDITO,
  EMPTY_CONTROLES_FIADOR_GARANTE,
  EMPTY_CONTROLES_PROPIEDAD,
  EMPTY_CONTROLES_VENDEDOR,
} from '../models/catalogo';
import { useDatosOperacion } from '../hooks/useDatosOperacion';
import { useUpsertDatosOperacion } from '../hooks/useUpsertDatosOperacion';
import { Checkbox } from 'primereact/checkbox';
import { InputTextarea } from 'primereact/inputtextarea';
import {
  buildBancoAcreedorEmpty,
  buildDatosCreditoEmpty,
  buildDatosOperacionEmpty,
  buildPropiedadEmpty,
  type DatosOperacion,
  type DatosOperacionBancoAcreedor,
  type DatosOperacionComprador,
  type DatosOperacionDatosCredito,
  type DatosOperacionFiadorGarante,
  type DatosOperacionPropiedad,
  type DatosOperacionVendedor,
} from '../models/datos_operacion';

/**
 * Actividad 5.4 Datos de Operación.
 * Debe coincidir con Constants.Actividades.DatosOperacion en backend/workflow.
 */
const ACTIVITY_ID = '_LCDxkE_wEeWmubI992vDXg';

const isEmptyValue = (value: unknown): boolean =>
  value === null || value === undefined || (typeof value === 'string' && value.trim() === '');

const isMissingBoolean = (value?: boolean | null): boolean =>
  value === null || value === undefined;

const isJuridicaPersona = (
  row: DatosOperacionComprador | DatosOperacionVendedor,
): boolean => {
  const tipoPersona = String(row.tipo_persona ?? '').toLowerCase();
  const razonSocial = String(row.razon_social ?? '').trim();
  const nombres = String(row.nombres ?? '').trim();

  return tipoPersona.includes('jur') || (!!razonSocial && !nombres);
};

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

export default function DatosOperacionPage({
  idExpediente: idExpedienteProp,
  idActividad = ACTIVITY_ID,
}: Props) {
  const navigate = useNavigate();
  const toast = useRef<Toast>(null);
  const { id_expediente: idParam } = useParams();

  const idExpediente = Number(idExpedienteProp ?? idParam ?? 0);
  const hasValidExpediente = idExpediente > 0;

  const [activeTabIndex, setActiveTabIndex] = useState(0);
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(true);
  const [form, setForm] = useState<DatosOperacion>(() =>
    buildDatosOperacionEmpty(idExpediente),
  );

  const datosOperacionQuery = useDatosOperacion(idExpediente);
  const saveMutation = useUpsertDatosOperacion();
  const avanzarMutation = useAvanzarDatosOperacion();

  const controlesDatosCreditoQuery = useControlesDatosCredito(hasValidExpediente);
  const controlesCompradorQuery = useControlesComprador(hasValidExpediente);
  const controlesVendedorQuery = useControlesVendedor(hasValidExpediente);
  const controlesFiadorGaranteQuery = useControlesFiadorGarante(hasValidExpediente);
  const controlesBancoAcreedorQuery = useControlesBancoAcreedor(hasValidExpediente);
  const controlesPropiedadQuery = useControlesPropiedad(hasValidExpediente);

  const controlesDatosCredito = controlesDatosCreditoQuery.data?.status
    ? controlesDatosCreditoQuery.data.detail ?? EMPTY_CONTROLES_DATOS_CREDITO
    : EMPTY_CONTROLES_DATOS_CREDITO;

  const controlesComprador = controlesCompradorQuery.data?.status
    ? controlesCompradorQuery.data.detail ?? EMPTY_CONTROLES_COMPRADOR
    : EMPTY_CONTROLES_COMPRADOR;

  const controlesVendedor = controlesVendedorQuery.data?.status
    ? controlesVendedorQuery.data.detail ?? EMPTY_CONTROLES_VENDEDOR
    : EMPTY_CONTROLES_VENDEDOR;

  const controlesCompradorConTipoPersona = useMemo(
    () => ({
      ...controlesComprador,
      tipo_vendedor: controlesVendedor.tipo_vendedor,
    }),
    [controlesComprador, controlesVendedor.tipo_vendedor],
  );

  const controlesFiadorGarante = controlesFiadorGaranteQuery.data?.status
    ? controlesFiadorGaranteQuery.data.detail ?? EMPTY_CONTROLES_FIADOR_GARANTE
    : EMPTY_CONTROLES_FIADOR_GARANTE;

  const controlesBancoAcreedor = controlesBancoAcreedorQuery.data?.status
    ? controlesBancoAcreedorQuery.data.detail ?? EMPTY_CONTROLES_BANCO_ACREEDOR
    : EMPTY_CONTROLES_BANCO_ACREEDOR;

  const controlesPropiedad = controlesPropiedadQuery.data?.status
    ? controlesPropiedadQuery.data.detail ?? EMPTY_CONTROLES_PROPIEDAD
    : EMPTY_CONTROLES_PROPIEDAD;

  const isLoadingControlesDatosCredito = controlesDatosCreditoQuery.isLoading;
  const isLoadingControlesComprador = controlesCompradorQuery.isLoading;
  const isLoadingControlesVendedor = controlesVendedorQuery.isLoading;
  const isLoadingControlesFiadorGarante = controlesFiadorGaranteQuery.isLoading;
  const isLoadingControlesBancoAcreedor = controlesBancoAcreedorQuery.isLoading;
  const isLoadingControlesPropiedad = controlesPropiedadQuery.isLoading;

  const isLoading = datosOperacionQuery.isLoading;
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
        cabecera.datos_credito ??
        buildDatosCreditoEmpty(idExpediente, idDatosOperacion),
      compradores: cabecera.compradores ?? [],
      vendedores: cabecera.vendedores ?? [],
      fiadores_garantes: cabecera.fiadores_garantes ?? [],
      banco_acreedor:
        cabecera.banco_acreedor ??
        buildBancoAcreedorEmpty(idExpediente, idDatosOperacion),
      propiedad:
        cabecera.propiedad ??
        buildPropiedadEmpty(idExpediente, idDatosOperacion),
    };
  };

  useEffect(() => {
    if (!hasValidExpediente) {
      setForm(buildDatosOperacionEmpty(0));
      return;
    }

    if (datosOperacionQuery.data?.status) {
      setForm(normalizeForm(datosOperacionQuery.data.detail));
      setIsDisabled(true);
      setCanAdvance(true);
    }
  }, [datosOperacionQuery.data, hasValidExpediente]);

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

  const preparePayload = (): DatosOperacion => {
    const idDatosOperacion = normalizeEntityId(form.id_datos_operacion);
    const idExp = form.id_expediente || idExpediente;

    return {
      ...form,
      id_datos_operacion: idDatosOperacion,
      id_expediente: idExp,
      is_active: form.is_active ?? true,
      row_status: form.row_status ?? true,
      enviar_reparo: form.enviar_reparo ?? false,
      observaciones: form.observaciones ?? '',
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
        id_datos_operacion_fiador_garante: normalizeEntityId(
          item.id_datos_operacion_fiador_garante,
        ),
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
            id_datos_operacion_propiedad: normalizeEntityId(
              form.propiedad.id_datos_operacion_propiedad,
            ),
            id_datos_operacion: idDatosOperacion,
            id_expediente: idExp,
          }
        : null,
    };
  };

  const validateDatosCredito = (): ValidationResult => {
    const datosCredito = form.datos_credito;

    if (!datosCredito) {
      return {
        isValid: false,
        tabIndex: 0,
        message: 'Debe completar la pestaña Datos del Crédito.',
      };
    }

    const requiredFields: { value: unknown; label: string }[] = [
      { value: datosCredito.ubicacion, label: 'Ubicación' },
      { value: datosCredito.tipo_operacion, label: 'Tipo Operación' },
      { value: datosCredito.fines_generales, label: 'Fines Generales' },
      { value: datosCredito.nombre_proyecto, label: 'Nombre Proyecto' },
      { value: datosCredito.credito_segunda_vivienda, label: 'Crédito Segunda Vivienda' },
      { value: datosCredito.inmobiliaria, label: 'Inmobiliaria' },
      { value: datosCredito.porcentaje_impuesto, label: '% Impuesto' },
      { value: datosCredito.monto_credito_afecto_impuesto, label: 'Monto Crédito Afecto Impuesto' },
      { value: datosCredito.impuesto_a_pagar, label: 'Impuesto a Pagar' }
    ];

    const missing = requiredFields.find(({ value }) =>
      typeof value === 'boolean' ? isMissingBoolean(value) : isEmptyValue(value),
    );

    if (missing) {
      return {
        isValid: false,
        tabIndex: 0,
        message: `Debe completar el campo ${missing.label}.`,
      };
    }

    return validResult;
  };

  const getMissingPersonaField = (
    row: DatosOperacionComprador | DatosOperacionVendedor,
    type: 'comprador' | 'vendedor',
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

    const compradorFields: { value: unknown; label: string }[] = [
      { value: row.direccion_env_esc, label: 'Dirección Envío Escritura' },
      { value: row.region_env_esc, label: 'Región Envío Escritura' },
      { value: row.comuna_env_esc, label: 'Comuna Envío Escritura' },
      { value: row.tipo_dir_dividendo, label: 'Tipo Dirección Dividendo' },
      { value: row.direccion_env_div, label: 'Dirección Envío Dividendo' },
      { value: row.region_env_div, label: 'Región Envío Dividendo' },
      { value: row.comuna_env_div, label: 'Comuna Envío Dividendo' },
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

    const fields = [
      ...commonFields,
      ...(type === 'comprador' ? compradorFields : []),
      ...naturalFields,
      ...vendedorJuridicoFields,
    ];

    return fields.find(({ value }) => isEmptyValue(value))?.label ?? null;
  };

  const validatePersonas = (
    rows: (DatosOperacionComprador | DatosOperacionVendedor)[] | undefined,
    type: 'comprador' | 'vendedor',
    tabIndex: number,
    tabName: string,
    requireAtLeastOne = false,
  ): ValidationResult => {
    const activeRows = (rows ?? []).filter((row) => row.row_status !== false);

    if (requireAtLeastOne && activeRows.length === 0) {
      return {
        isValid: false,
        tabIndex,
        message: `Debe registrar al menos un registro en ${tabName}.`,
      };
    }

    for (let index = 0; index < activeRows.length; index += 1) {
      const missingField = getMissingPersonaField(activeRows[index], type);

      if (missingField) {
        return {
          isValid: false,
          tabIndex,
          message: `${tabName}, registro ${index + 1}: debe completar el campo ${missingField}.`,
        };
      }
    }

    return validResult;
  };

  const validateFiadoresGarantes = (): ValidationResult => {
    const activeRows = (form.fiadores_garantes ?? []).filter((row) => row.row_status !== false);

    for (let index = 0; index < activeRows.length; index += 1) {
      const row = activeRows[index];
      const requiredFields: { value: unknown; label: string }[] = [
        { value: row.rut, label: 'RUT' },
        { value: row.fecha_nacimiento, label: 'Fecha Nacimiento' },
        { value: row.genero, label: 'Género' },
        { value: row.nombres, label: 'Nombres' },
        { value: row.apellido_paterno, label: 'Apellido Paterno' },
        { value: row.apellido_materno, label: 'Apellido Materno' },
        { value: row.nacionalidad, label: 'Nacionalidad' },
        { value: row.estado_civil, label: 'Estado Civil' },
        { value: row.profesion, label: 'Profesión' },
        { value: row.direccion, label: 'Dirección' },
        { value: row.region, label: 'Región' },
        { value: row.comuna, label: 'Comuna' },
        { value: row.telefono_fijo, label: 'Teléfono Fijo' },
        { value: row.telefono_movil, label: 'Teléfono Móvil' },
        { value: row.email, label: 'Email' },
        { value: row.relacion_titular, label: 'Relación Titular' }
      ];

      const missing = requiredFields.find(({ value }) => isEmptyValue(value));

      if (missing) {
        return {
          isValid: false,
          tabIndex: 3,
          message: `Datos del Fiador / Garante, registro ${index + 1}: debe completar el campo ${missing.label}.`,
        };
      }
    }

    return validResult;
  };

  const validateBancoAcreedor = (): ValidationResult => {
    const banco = form.banco_acreedor;

    if (!banco) {
      return {
        isValid: false,
        tabIndex: 4,
        message: 'Debe completar la pestaña Datos del Banco Acreedor.',
      };
    }

    const requiredFields: { value: unknown; label: string }[] = [
      { value: banco.cuenta_carta_resguardo, label: 'Cuenta Carta de Resguardo' },
      { value: banco.institucion, label: 'Institución' },
      { value: banco.rut_banco_acreedor, label: 'RUT Banco Acreedor' }
    ];

    const missing = requiredFields.find(({ value }) =>
      typeof value === 'boolean' ? isMissingBoolean(value) : isEmptyValue(value),
    );

    if (missing) {
      return {
        isValid: false,
        tabIndex: 4,
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
        tabIndex: 5,
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
      { value: propiedad.valor_avaluo_pesos, label: 'Valor Avalúo Pesos' }
    ];

    const missing = requiredFields.find(({ value }) => isEmptyValue(value));

    if (missing) {
      return {
        isValid: false,
        tabIndex: 5,
        message: `Debe completar el campo ${missing.label}.`,
      };
    }

    return validResult;
  };

  const validateRequiredFields = (): ValidationResult => {
    const validations = [
      validateDatosCredito(),
      validatePersonas(form.compradores, 'comprador', 1, 'Datos del Comprador', true),
      validatePersonas(form.vendedores, 'vendedor', 2, 'Datos del Vendedor'),
      validateFiadoresGarantes(),
      validateBancoAcreedor(),
      validatePropiedad(),
    ];

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

    try {
      const response = await saveMutation.mutateAsync(preparePayload());

      if (!response.status) {
        showToast('error', 'Error', response.message ?? 'No se pudo guardar la actividad.');
        return;
      }

      setForm(normalizeForm(response.detail));
      setIsDisabled(true);
      setCanAdvance(true);
      showToast('success', 'Guardado', response.message ?? 'Datos de Operación guardados correctamente.');
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Error inesperado al guardar.';
      showToast('error', 'Error', message);
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

  const updateDatosCreditoField = <K extends keyof DatosOperacionDatosCredito>(
    field: K,
    value: DatosOperacionDatosCredito[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      datos_credito: {
        ...(prev.datos_credito ?? buildDatosCreditoEmpty(idExpediente, prev.id_datos_operacion)),
        [field]: value,
      },
    }));
  };

  const updateBancoAcreedorField = <K extends keyof DatosOperacionBancoAcreedor>(
    field: K,
    value: DatosOperacionBancoAcreedor[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      banco_acreedor: {
        ...(prev.banco_acreedor ?? buildBancoAcreedorEmpty(idExpediente, prev.id_datos_operacion)),
        [field]: value,
      },
    }));
  };

  const updatePropiedadField = <K extends keyof DatosOperacionPropiedad>(
    field: K,
    value: DatosOperacionPropiedad[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      propiedad: {
        ...(prev.propiedad ?? buildPropiedadEmpty(idExpediente, prev.id_datos_operacion)),
        [field]: value,
      },
    }));
  };

  const updateCompradores = (compradores: DatosOperacionComprador[]) => {
    setForm((prev) => ({
      ...prev,
      compradores,
    }));
  };

  const updateVendedores = (vendedores: DatosOperacionVendedor[]) => {
    setForm((prev) => ({
      ...prev,
      vendedores,
    }));
  };

  const updateFiadoresGarantes = (fiadores_garantes: DatosOperacionFiadorGarante[]) => {
    setForm((prev) => ({
      ...prev,
      fiadores_garantes,
    }));
  };

  const updateField = <K extends keyof DatosOperacion>(
    field: K,
    value: DatosOperacion[K]
  ) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const tabHeader = useMemo(() => {
    if (isLoading) return 'Cargando Datos de Operación...';
    return 'Datos de Operación';
  }, [isLoading]);

  return (
    <div>
      <Toast ref={toast} />

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad idExpediente={idExpediente} activityID={idActividad || ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales
            idExpediente={idExpediente}
            idActividad={idActividad || ACTIVITY_ID}
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
                <h2 className="text-lg font-semibold text-gray-800">Datos de Operación</h2>
                <p className="text-sm text-gray-500">
                  Complete la información por pestañas. Use el botón general Guardar para persistir toda la actividad.
                </p>
              </div>

              <TabView
                className="mi-tabview"
                activeIndex={activeTabIndex}
                onTabChange={(e) => setActiveTabIndex(e.index)}
              >
                <TabPanel header="Datos del Crédito">
                  <DatosCreditoSection
                    value={form.datos_credito}
                    disabled={isDisabled || isBusy}
                    controles={controlesDatosCredito}
                    loadingControles={isLoadingControlesDatosCredito}
                    onChange={updateDatosCreditoField}
                  />
                </TabPanel>

                <TabPanel header="Datos del Comprador">
                  <DatosOperacionPersonaGrid
                    title="Datos del Comprador"
                    type="comprador"
                    value={form.compradores ?? []}
                    idExpediente={idExpediente}
                    idDatosOperacion={form.id_datos_operacion ?? 0}
                    disabled={isDisabled || isBusy}
                    canEdit={canEditSections}
                    controles={controlesCompradorConTipoPersona}
                    loadingControles={isLoadingControlesComprador || isLoadingControlesVendedor}
                    onChange={updateCompradores}
                    onWarn={(message) => showToast('warn', 'Validación', message)}
                  />
                </TabPanel>

                <TabPanel header="Datos del Vendedor">
                  <DatosOperacionPersonaGrid
                    title="Datos del Vendedor"
                    type="vendedor"
                    value={form.vendedores ?? []}
                    idExpediente={idExpediente}
                    idDatosOperacion={form.id_datos_operacion ?? 0}
                    disabled={isDisabled || isBusy}
                    canEdit={canEditSections}
                    controles={controlesVendedor}
                    loadingControles={isLoadingControlesVendedor}
                    onChange={updateVendedores}
                    onWarn={(message) => showToast('warn', 'Validación', message)}
                  />
                </TabPanel>

                <TabPanel header="Datos del Fiador / Garante">
                  <FiadorGaranteSection
                    value={form.fiadores_garantes ?? []}
                    idExpediente={idExpediente}
                    idDatosOperacion={form.id_datos_operacion ?? 0}
                    disabled={isDisabled || isBusy}
                    canEdit={canEditSections}
                    controles={controlesFiadorGarante}
                    loadingControles={isLoadingControlesFiadorGarante}
                    onChange={updateFiadoresGarantes}
                    onWarn={(message) => showToast('warn', 'Validación', message)}
                  />
                </TabPanel>

                <TabPanel header="Datos del Banco Acreedor">
                  <BancoAcreedorSection
                    value={form.banco_acreedor}
                    disabled={isDisabled || isBusy}
                    controles={controlesBancoAcreedor}
                    loadingControles={isLoadingControlesBancoAcreedor}
                    onChange={updateBancoAcreedorField}
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
              {/* AGREGAR AQUÍ LOS NUEVOS CAMPOS */}
              <div className="mt-6 space-y-6">
                {/* ESTATUS DE LA ACTIVIDAD */}
                <div>
                  <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                    Estatus de la Actividad
                  </div>
                  <div className="flex items-center gap-3">
                    <Checkbox
                      inputId="enviar_reparo"
                      checked={form.enviar_reparo ?? false}
                      onChange={(e) => updateField("enviar_reparo", e.checked ?? false)}
                      disabled={isDisabled}
                    />
                    <label htmlFor="enviar_reparo" className="font-semibold text-sm">
                      ¿Enviar a Reparo?
                    </label>
                  </div>
                </div>

                {/* OBSERVACIONES */}
                <div>
                  <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                    Observaciones
                  </div>
                  <InputTextarea
                    id="observaciones"
                    value={form.observaciones || ""}
                    onChange={(e) => updateField("observaciones", e.target.value)}
                    rows={3}
                    className="w-full"
                    placeholder="Escriba aquí sus observaciones..."
                    disabled={isDisabled}
                  />
                </div>
              </div>
              {/* FIN DE LOS NUEVOS CAMPOS */}
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
            </div>
          )}
        </AccordionTab>
      </Accordion>
    </div>
  );
}
