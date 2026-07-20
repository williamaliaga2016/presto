import { useEffect, useMemo, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '@/app/providers/AuthProvider';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import AntecedenteCreditoSection from '../components/AntecedenteCreditoSection';
import AntecedentesCompradorSection from '../components/AntecedentesCompradorSection';
import {
  CanalOriginacionField,
  OficinaAsesorFields,
  ProyectoInmuebleFields,
} from '../components/DatosOperacionSection';
import { useAvanzarCargaOperacionBanco } from '../hooks/useAvanzarCargaOperacionBanco';
import { useCargaOperacionBanco } from '../hooks/useCargaOperacionBanco';
import { useControlesAntecedenteComprador } from '../hooks/useControlesAntecedenteComprador';
import { useControlesAntecedenteCredito } from '../hooks/useControlesAntecedenteCredito';
import { useControlesDatosOperacion } from '../hooks/useControlesDatosOperacion';
import { useUpsertCargaOperacionBanco } from '../hooks/useUpsertCargaOperacionBanco';
import {
  EMPTY_CONTROLES_ANTECEDENTE_COMPRADOR,
  EMPTY_CONTROLES_ANTECEDENTE_CREDITO,
  EMPTY_CONTROLES_DATOS_OPERACION,
  type ControlesAntecedenteComprador,
  type ControlesAntecedenteCredito,
  type ControlesDatosOperacion,
} from '../models/catalogo';
import type {
  CargaOperacionBanco,
  CargaOperacionBancoAntecedenteComprador,
  CargaOperacionBancoAntecedenteCredito,
  CargaOperacionBancoDatosComercial,
  CargaOperacionBancoDatosOperacion,
} from '../models/carga_operacion_banco';

const now = () => new Date().toISOString();

const buildDatosOperacionInitialState = (
  id_expediente: number,
  codigo_asesor_default: string | null = null,
): CargaOperacionBancoDatosOperacion => ({
  id_carga_operacion_banco_datos_operacion: 0,
  id_carga_operacion_banco: 0,
  id_expediente,
  id_scoring: null,
  codigo_asesor: codigo_asesor_default,
  codigo_oficina: null,
  descripcion_oficina: null,
  canal_originacion: null,
  tipo_inmueble: null,
  estado_inmueble: null,
  descripcion_estado_inmueble: null,
  codigo_proyecto: null,
  descripcion_proyecto: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

const buildCompradorInitialState = (
  id_expediente: number,
  id_carga_operacion_banco = 0,
): CargaOperacionBancoAntecedenteComprador => ({
  id_carga_operacion_banco_antecedente_comprador: 0,
  id_carga_operacion_banco,
  id_expediente,
  direccion: '',
  telefono: '',
  email: '',
  numero_identificacion: '',
  tipo_documento_id: null,
  nombre_completo: '',
  celular: '',
  situacion_laboral: null,
  cliente_nomina: false,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

const buildAntecedenteCreditoInitialState = (
  id_expediente: number,
  id_carga_operacion_banco = 0,
): CargaOperacionBancoAntecedenteCredito => ({
  id_carga_operacion_banco_antecedente_credito: 0,
  id_carga_operacion_banco,
  id_expediente,
  factor_conversion_uf: null,
  tasa: null,
  plazo: null,
  id_tipo_sub_producto: null,
  monto_otorgado: null,
  fecha_aprobacion: null,
  condiciones_organismo_decisor: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

const buildDatosComercialInitialState = (
  id_expediente: number,
  id_carga_operacion_banco = 0,
): CargaOperacionBancoDatosComercial => ({
  id_carga_operacion_banco_datos_comercial: 0,
  id_carga_operacion_banco,
  id_expediente,
  correo_declarativo_cliente: null,
  numero_telefono_declarativo: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

const buildInitialState = (
  id_expediente: number,
  codigo_asesor_default: string | null = null,
): CargaOperacionBanco => ({
  id_carga_operacion_banco: 0,
  id_expediente,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
  datos_operacion: buildDatosOperacionInitialState(id_expediente, codigo_asesor_default),
  antecedentes_comprador: [],
  antecedente_credito: buildAntecedenteCreditoInitialState(id_expediente),
  datos_comercial: buildDatosComercialInitialState(id_expediente),
});

const normalizeDatosOperacion = (
  source: Partial<CargaOperacionBancoDatosOperacion> | null | undefined,
  id_expediente_fallback: number,
  id_carga_operacion_banco_fallback = 0,
  codigo_asesor_default: string | null = null,
): CargaOperacionBancoDatosOperacion => ({
  id_carga_operacion_banco_datos_operacion: Number(
    source?.id_carga_operacion_banco_datos_operacion ?? 0,
  ),
  id_carga_operacion_banco: Number(
    source?.id_carga_operacion_banco ?? id_carga_operacion_banco_fallback ?? 0,
  ),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
  id_scoring: source?.id_scoring ?? null,
  codigo_asesor: source?.codigo_asesor ?? codigo_asesor_default,
  codigo_oficina: source?.codigo_oficina ?? null,
  descripcion_oficina: source?.descripcion_oficina ?? null,
  canal_originacion: source?.canal_originacion ?? null,
  tipo_inmueble: source?.tipo_inmueble ?? null,
  estado_inmueble: source?.estado_inmueble ?? null,
  // NUEVO (BBV-76): aún no confirmado en backend. Descomentar cuando la API soporte esta columna.
  // descripcion_estado_inmueble: source?.descripcion_estado_inmueble ?? null,
  codigo_proyecto: source?.codigo_proyecto ?? null,
  descripcion_proyecto: source?.descripcion_proyecto ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? now(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

const normalizeAntecedenteComprador = (
  source: Partial<CargaOperacionBancoAntecedenteComprador> | null | undefined,
  id_expediente_fallback: number,
  id_carga_operacion_banco_fallback = 0,
): CargaOperacionBancoAntecedenteComprador => ({
  ...buildCompradorInitialState(
    Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    Number(source?.id_carga_operacion_banco ?? id_carga_operacion_banco_fallback ?? 0),
  ),
  id_carga_operacion_banco_antecedente_comprador: Number(
    source?.id_carga_operacion_banco_antecedente_comprador ?? 0,
  ),
  id_carga_operacion_banco: Number(
    source?.id_carga_operacion_banco ?? id_carga_operacion_banco_fallback ?? 0,
  ),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
  direccion: source?.direccion ?? '',
  telefono: source?.telefono ?? '',
  email: source?.email ?? '',
  numero_identificacion: source?.numero_identificacion ?? '',
  tipo_documento_id: source?.tipo_documento_id ?? null,
  nombre_completo: source?.nombre_completo ?? '',
  celular: source?.celular ?? '',
  situacion_laboral: source?.situacion_laboral ?? null,
  cliente_nomina: source?.cliente_nomina ?? false,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? now(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

const normalizeAntecedentesComprador = (
  source: Partial<CargaOperacionBancoAntecedenteComprador>[] | null | undefined,
  id_expediente_fallback: number,
  id_carga_operacion_banco_fallback = 0,
): CargaOperacionBancoAntecedenteComprador[] =>
  (source ?? []).map((comprador) =>
    normalizeAntecedenteComprador(
      comprador,
      id_expediente_fallback,
      id_carga_operacion_banco_fallback,
    ),
  );

const normalizeAntecedenteCredito = (
  source: Partial<CargaOperacionBancoAntecedenteCredito> | null | undefined,
  id_expediente_fallback: number,
  id_carga_operacion_banco_fallback = 0,
): CargaOperacionBancoAntecedenteCredito => ({
  ...buildAntecedenteCreditoInitialState(
    Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    Number(source?.id_carga_operacion_banco ?? id_carga_operacion_banco_fallback ?? 0),
  ),
  id_carga_operacion_banco_antecedente_credito: Number(
    source?.id_carga_operacion_banco_antecedente_credito ?? 0,
  ),
  id_carga_operacion_banco: Number(
    source?.id_carga_operacion_banco ?? id_carga_operacion_banco_fallback ?? 0,
  ),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
  factor_conversion_uf:
    source?.factor_conversion_uf === undefined || source?.factor_conversion_uf === null
      ? null
      : Number(source.factor_conversion_uf),
  tasa: source?.tasa === undefined || source?.tasa === null ? null : Number(source.tasa),
  plazo: source?.plazo === undefined || source?.plazo === null ? null : Number(source.plazo),
  id_tipo_sub_producto: source?.id_tipo_sub_producto ?? null,
  monto_otorgado:
    source?.monto_otorgado === undefined || source?.monto_otorgado === null
      ? null
      : Number(source.monto_otorgado),
  fecha_aprobacion: source?.fecha_aprobacion ?? null,
  condiciones_organismo_decisor: source?.condiciones_organismo_decisor ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? now(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

const normalizeDatosComercial = (
  source: Partial<CargaOperacionBancoDatosComercial> | null | undefined,
  id_expediente_fallback: number,
  id_carga_operacion_banco_fallback = 0,
): CargaOperacionBancoDatosComercial => ({
  ...buildDatosComercialInitialState(
    Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    Number(source?.id_carga_operacion_banco ?? id_carga_operacion_banco_fallback ?? 0),
  ),
  id_carga_operacion_banco_datos_comercial: Number(
    source?.id_carga_operacion_banco_datos_comercial ?? 0,
  ),
  id_carga_operacion_banco: Number(
    source?.id_carga_operacion_banco ?? id_carga_operacion_banco_fallback ?? 0,
  ),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
  correo_declarativo_cliente: source?.correo_declarativo_cliente ?? null,
  numero_telefono_declarativo: source?.numero_telefono_declarativo ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? now(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

const normalizeCargaOperacionBanco = (
  source: Partial<CargaOperacionBanco> | null | undefined,
  id_expediente_fallback: number,
  codigo_asesor_default: string | null = null,
): CargaOperacionBanco => {
  const idCargaOperacionBanco = Number(source?.id_carga_operacion_banco ?? 0);
  const idExpediente = Number(source?.id_expediente ?? id_expediente_fallback ?? 0);

  return {
    id_carga_operacion_banco: idCargaOperacionBanco,
    id_expediente: idExpediente,
    is_active: source?.is_active ?? true,
    row_status: source?.row_status ?? true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? now(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
    datos_operacion: normalizeDatosOperacion(
      source?.datos_operacion,
      idExpediente,
      idCargaOperacionBanco,
      codigo_asesor_default,
    ),
    antecedentes_comprador: normalizeAntecedentesComprador(
      source?.antecedentes_comprador,
      idExpediente,
      idCargaOperacionBanco,
    ),
    antecedente_credito: normalizeAntecedenteCredito(
      source?.antecedente_credito,
      idExpediente,
      idCargaOperacionBanco,
    ),
    datos_comercial: normalizeDatosComercial(
      source?.datos_comercial,
      idExpediente,
      idCargaOperacionBanco,
    ),
  };
};

export default function CargaOperacionBancoPage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const { user } = useAuth();

  const id_expediente = Number(idExpedienteParam ?? 0);
  const codigoAsesorDefault = user?.user_name?.trim() || null;

  const [form, setForm] = useState<CargaOperacionBanco>(
    buildInitialState(id_expediente, codigoAsesorDefault),
  );
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const idemKeyRef = useRef<string | null>(null);

  const { data, isLoading } = useCargaOperacionBanco(id_expediente);
  const saveMutation = useUpsertCargaOperacionBanco();
  const avanzarMutation = useAvanzarCargaOperacionBanco();

  const controlesDatosOperacionQuery = useControlesDatosOperacion();
  const controlesAntecedenteCompradorQuery = useControlesAntecedenteComprador();
  const controlesAntecedenteCreditoQuery = useControlesAntecedenteCredito();

  const catalogos: ControlesDatosOperacion = useMemo(() => {
    if (!controlesDatosOperacionQuery.data?.status) {
      return EMPTY_CONTROLES_DATOS_OPERACION;
    }

    return (
      controlesDatosOperacionQuery.data.detail ??
      EMPTY_CONTROLES_DATOS_OPERACION
    );
  }, [controlesDatosOperacionQuery.data]);

  const controlesComprador: ControlesAntecedenteComprador = useMemo(() => {
    if (!controlesAntecedenteCompradorQuery.data?.status) {
      return EMPTY_CONTROLES_ANTECEDENTE_COMPRADOR;
    }

    return (
      controlesAntecedenteCompradorQuery.data.detail ??
      EMPTY_CONTROLES_ANTECEDENTE_COMPRADOR
    );
  }, [controlesAntecedenteCompradorQuery.data]);

  const controlesCredito: ControlesAntecedenteCredito = useMemo(() => {
    if (!controlesAntecedenteCreditoQuery.data?.status) {
      return EMPTY_CONTROLES_ANTECEDENTE_CREDITO;
    }

    return (
      controlesAntecedenteCreditoQuery.data.detail ??
      EMPTY_CONTROLES_ANTECEDENTE_CREDITO
    );
  }, [controlesAntecedenteCreditoQuery.data]);

  const isLoadingCatalogos = controlesDatosOperacionQuery.isLoading;
  const isLoadingControlesComprador =
    controlesAntecedenteCompradorQuery.isLoading;
  const isLoadingControlesCredito =
    controlesAntecedenteCreditoQuery.isLoading;

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialState(id_expediente, codigoAsesorDefault));
      setIsDisabled(true);
      setCanAdvance(false);
      setErrorMessage('');
      setSuccessMessage('');
    }
  }, [id_expediente, codigoAsesorDefault]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (!id_expediente || id_expediente <= 0) {
      setForm(buildInitialState(0, codigoAsesorDefault));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data?.status && data.detail) {
      setForm(
        normalizeCargaOperacionBanco(data.detail, id_expediente, codigoAsesorDefault),
      );
      setIsDisabled(Number(data.detail.id_carga_operacion_banco) > 0);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm(buildInitialState(id_expediente, codigoAsesorDefault));
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente, codigoAsesorDefault]);

  const updateDatosOperacionField = <
    K extends keyof CargaOperacionBancoDatosOperacion,
  >(
    field: K,
    value: CargaOperacionBancoDatosOperacion[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      datos_operacion: {
        ...(prev.datos_operacion ??
          buildDatosOperacionInitialState(prev.id_expediente, codigoAsesorDefault)),
        [field]: value,
      },
    }));
  };

  const updateAntecedenteCreditoField = <
    K extends keyof CargaOperacionBancoAntecedenteCredito,
  >(
    field: K,
    value: CargaOperacionBancoAntecedenteCredito[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      antecedente_credito: {
        ...(prev.antecedente_credito ??
          buildAntecedenteCreditoInitialState(
            prev.id_expediente,
            prev.id_carga_operacion_banco,
          )),
        [field]: value,
      },
    }));
  };

  const updateAntecedentesComprador = (
    antecedentes_comprador: CargaOperacionBancoAntecedenteComprador[],
  ) => {
    setForm((prev) => ({
      ...prev,
      antecedentes_comprador: normalizeAntecedentesComprador(
        antecedentes_comprador,
        prev.id_expediente,
        prev.id_carga_operacion_banco,
      ),
    }));
  };

  const handleEditar = () => {
    setErrorMessage('');
    setSuccessMessage('');
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const validateForm = () => {
    if (id_expediente > 0 && (!form.id_expediente || form.id_expediente <= 0)) {
      return 'No existe un id_expediente válido.';
    }

    const datosOperacion =
      form.datos_operacion ?? buildDatosOperacionInitialState(form.id_expediente);

    if (!datosOperacion.codigo_oficina) return 'Debe ingresar Código Oficina.';
    if (!datosOperacion.codigo_asesor) return 'Debe ingresar Código Asesor.';

    const compradoresActivos =
      form.antecedentes_comprador?.filter((comprador) => comprador.row_status !== false) ??
      [];

    if (compradoresActivos.length === 0) {
      return 'Debe agregar al menos el Titular 1.';
    }

    const compradorInvalido = compradoresActivos.find(
      (comprador) =>
        !comprador.tipo_documento_id ||
        !comprador.numero_identificacion?.trim() ||
        !comprador.nombre_completo?.trim(),
    );

    if (compradorInvalido) {
      return 'Hay titulares con información obligatoria pendiente.';
    }

    const antecedenteCredito =
      form.antecedente_credito ??
      buildAntecedenteCreditoInitialState(
        form.id_expediente,
        form.id_carga_operacion_banco,
      );

    if (!antecedenteCredito.id_tipo_sub_producto) {
      return 'Debe seleccionar Id SubProducto.';
    }
    if (!antecedenteCredito.monto_otorgado) {
      return 'Debe ingresar el Monto Otorgado.';
    }

    return '';
  };

  const buildPayload = () => {
    const expedienteId = Number(form.id_expediente || id_expediente || 0);
    const idCargaOperacionBanco = Number(form.id_carga_operacion_banco ?? 0);

    return normalizeCargaOperacionBanco(
      {
        ...form,
        id_expediente: expedienteId,
        datos_operacion: normalizeDatosOperacion(
          form.datos_operacion,
          expedienteId,
          idCargaOperacionBanco,
        ),
        antecedentes_comprador: normalizeAntecedentesComprador(
          form.antecedentes_comprador,
          expedienteId,
          idCargaOperacionBanco,
        ),
        antecedente_credito: normalizeAntecedenteCredito(
          form.antecedente_credito,
          expedienteId,
          idCargaOperacionBanco,
        ),
        datos_comercial: normalizeDatosComercial(
          form.datos_comercial,
          expedienteId,
          idCargaOperacionBanco,
        ),
      },
      expedienteId,
    );
  };

  const handleGuardar = async () => {
    setErrorMessage('');
    setSuccessMessage('');

    const validationMessage = validateForm();

    if (validationMessage) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: validationMessage,
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const payload = buildPayload();
      const guardarKey = crypto.randomUUID();

      const response = await saveMutation.mutateAsync({
        payload,
        idempotencyKey: guardarKey,
      });

      if (response.status) {
        const savedEntity = normalizeCargaOperacionBanco(
          response.detail ?? payload,
          payload.id_expediente,
        );

        setForm(savedEntity);
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;

        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Carga Operación Banco guardada correctamente.',
          life: 3000,
        });
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No se pudo guardar.',
          life: 3000,
        });
      }
    } catch (error) {
      console.error('ERROR GUARDAR CARGA OPERACION BANCO', error);

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'Ocurrió un error al guardar.',
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async () => {
    // Dedupe guard: evitar re-despachar si ya hay una petición en vuelo.
    if (saveMutation.isPending || avanzarMutation.isPending) {
      return;
    }

    setErrorMessage('');
    setSuccessMessage('');

    const validationMessage = validateForm();

    if (validationMessage) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: validationMessage,
        life: 3000,
      });
      return;
    }

    // Generar clave de idempotencia una sola vez por intento de avance.
    // Se reutiliza en save + avanzar para que el backend pueda detectar reintentos.
    if (idemKeyRef.current === null) {
      idemKeyRef.current = crypto.randomUUID();
    }
    const currentIdemKey = idemKeyRef.current;

    try {
      setIsBusy(true);

      const payload = buildPayload();

      const saveResponse = await saveMutation.mutateAsync({
        payload,
        idempotencyKey: currentIdemKey,
      });

      if (!saveResponse.status) {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: saveResponse.message || 'No se pudo guardar antes de avanzar.',
          life: 3000,
        });
        return;
      }

      const savedEntity = normalizeCargaOperacionBanco(
        saveResponse.detail ?? payload,
        payload.id_expediente,
      );

      setForm(savedEntity);

      const expedienteId = Number(savedEntity.id_expediente ?? 0);

      if (!expedienteId || expedienteId <= 0) {
        toast.current?.show({
          severity: 'warn',
          summary: 'Validación',
          detail: 'No existe un id_expediente válido para avanzar.',
          life: 3000,
        });
        return;
      }

      const response = await avanzarMutation.mutateAsync(expedienteId);

      if (response.status) {
        // Resetear la clave para habilitar un nuevo intento fresco si el usuario
        // inicia otra operación desde la misma sesión de pantalla.
        idemKeyRef.current = null;

        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: response.message,
          life: 5000,
        });

        await new Promise((resolve) => setTimeout(resolve, 1000));

        navigate('/home/bandeja');
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No se pudo avanzar la actividad.',
          life: 3000,
        });
      }
    } catch (error) {
      console.error('ERROR AVANZAR CARGA OPERACION BANCO', error);

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'Ocurrió un error al avanzar.',
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  const canEditAntecedentesComprador = !isDisabled && !isBusy;

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Radicar Crédito
      </h2>

      <Accordion activeIndex={[0]} multiple>
        <AccordionTab header="Radicar Crédito">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && (
              <div className="mb-4 text-sm text-blue-600">
                Cargando información...
              </div>
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

            {/* Card 1: Datos Generales y Financieros */}
            <div className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden mb-6">
              <div className="bg-gray-50 px-6 py-4 border-b border-gray-200">
                <h3 className="font-semibold text-gray-800 text-sm">
                  Datos Generales y Financieros
                </h3>
              </div>
              <div className="p-6">
                <AntecedenteCreditoSection
                  value={
                    form.antecedente_credito ??
                    buildAntecedenteCreditoInitialState(
                      form.id_expediente,
                      form.id_carga_operacion_banco,
                    )
                  }
                  disabled={isDisabled || isBusy}
                  controles={controlesCredito}
                  loadingControles={isLoadingControlesCredito}
                  onChange={updateAntecedenteCreditoField}
                  canalOriginacionSlot={
                    <CanalOriginacionField
                      value={
                        form.datos_operacion ??
                        buildDatosOperacionInitialState(form.id_expediente)
                      }
                      disabled={isDisabled || isBusy}
                      catalogos={catalogos}
                      loadingCatalogos={isLoadingCatalogos}
                      onChange={updateDatosOperacionField}
                    />
                  }
                />
              </div>
            </div>

            {/* Cards 2a y 2b: Proyecto/Inmueble y Oficina/Asesor, lado a lado en desktop */}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-6">
              <div className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
                <div className="bg-gray-50 px-6 py-4 border-b border-gray-200">
                  <h3 className="font-semibold text-gray-800 text-sm">
                    Datos del Proyecto e Inmueble
                  </h3>
                </div>
                <div className="p-6">
                  <ProyectoInmuebleFields
                    value={
                      form.datos_operacion ??
                      buildDatosOperacionInitialState(form.id_expediente)
                    }
                    disabled={isDisabled || isBusy}
                    catalogos={catalogos}
                    loadingCatalogos={isLoadingCatalogos}
                    onChange={updateDatosOperacionField}
                  />
                </div>
              </div>

              <div className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
                <div className="bg-gray-50 px-6 py-4 border-b border-gray-200">
                  <h3 className="font-semibold text-gray-800 text-sm">
                    Datos Comerciales
                  </h3>
                </div>
                <div className="p-6">
                  <OficinaAsesorFields
                    value={
                      form.datos_operacion ??
                      buildDatosOperacionInitialState(form.id_expediente)
                    }
                    disabled={isDisabled || isBusy}
                    catalogos={catalogos}
                    loadingCatalogos={isLoadingCatalogos}
                    onChange={updateDatosOperacionField}
                  />
                </div>
              </div>
            </div>

            {/* Card 3: Titulares (incluye datos declarativos del cliente) */}
            <div className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden mb-6">
              <div className="bg-gray-50 px-6 py-4 border-b border-gray-200">
                <h3 className="font-semibold text-gray-800 text-sm">Titulares</h3>
              </div>
              <div className="p-6">
                <AntecedentesCompradorSection
                  key={`antecedentes-comprador-${canEditAntecedentesComprador ? 'edit' : 'view'}`}
                  value={form.antecedentes_comprador ?? []}
                  idExpediente={Number(form.id_expediente || id_expediente || 0)}
                  idCargaOperacionBanco={Number(form.id_carga_operacion_banco ?? 0)}
                  disabled={isDisabled || isBusy}
                  canEdit={canEditAntecedentesComprador}
                  controles={controlesComprador}
                  loadingControles={isLoadingControlesComprador}
                  onChange={updateAntecedentesComprador}
                  onWarn={(message) =>
                    toast.current?.show({
                      severity: 'warn',
                      summary: 'Validación',
                      detail: message,
                      life: 3000,
                    })
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
                label={saveMutation.isPending || avanzarMutation.isPending ? 'Procesando...' : 'Avanzar'}
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handleAvanzar}
                disabled={
                  isBusy ||
                  saveMutation.isPending ||
                  avanzarMutation.isPending ||
                  (!canAdvance && !form.id_carga_operacion_banco)
                }
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
