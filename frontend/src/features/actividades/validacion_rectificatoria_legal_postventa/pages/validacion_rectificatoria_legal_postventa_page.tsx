import { useEffect, useMemo, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import ValidacionRectificatoriaLegalPostventaSection from '../components/ValidacionRectificatoriaLegalPostventaSection';
import { useAvanzarValidacionRectificatoriaLegalPostventa } from '../hooks/useAvanzarValidacionRectificatoriaLegalPostventa';
import { useValidacionRectificatoriaLegalPostventa } from '../hooks/useValidacionRectificatoriaLegalPostventa';
import { useControlesValidacionRectificatoriaLegalPostventa } from '../hooks/useControlesValidacionRectificatoriaLegalPostventa';
import { useUpsertValidacionRectificatoriaLegalPostventa } from '../hooks/useUpsertValidacionRectificatoriaLegalPostventa';
import {
    EMPTY_CONTROLES_VALIDACION_RECTIFICATORIA_LEGAL_POSTVENTA,
    type ControlesValidacionRectificatoriaLegalPostventa
} from '../models/catalogo';
import type {
    ValidacionRectificatoriaLegalPostventa,
    ValidacionRectificatoriaLegalPostventaDatosPersonales
} from '../models/validacion_rectificatoria_legal_postventa';
import { Checkbox } from 'primereact/checkbox';
import { InputTextarea } from 'primereact/inputtextarea';
import { RadioButton } from 'primereact/radiobutton';
import { Dropdown } from 'primereact/dropdown';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';

/**
 * TODO:
 * Reemplazar por el id real de la actividad 6.64
 * cuando esté configurado en el workflow.
 */
const ACTIVITY_ID = '_4X3k8H6kEeeGvo3jOJ1wYw';

const now = () => new Date().toISOString();

const buildValidacionRectificatoriaLegalPostventaDatosPersonalesInitialState = (
    id_expediente: number,
    id_validacion_rectificatoria_legal_postventa = 0,
): ValidacionRectificatoriaLegalPostventaDatosPersonales => ({
    id_validacion_rectificatoria_legal_postventa_datos_personales: 0,
    id_validacion_rectificatoria_legal_postventa,
    id_expediente,
    rut: '',
    fecha_nacimiento: null,
    genero: null,
    nombres: '',
    apellido_paterno: '',
    apellido_materno: '',
    nacionalidad: null,
    relacion_titular: null,
    profesion: '',
    direccion: '',
    estado_civil: null,
    telefono: '',
    region: null,
    comuna: null,
    email: '',
    rol_comparecencia: '',
    is_active: true,
    row_status: true,
    created_by: 0,
    created_date: now(),
    modified_by: null,
    modified_date: null,
});


const buildInitialState = (id_expediente: number): ValidacionRectificatoriaLegalPostventa => ({
    id_validacion_rectificatoria_legal_postventa: 0,
    id_expediente,
    id_usuario_solicitante: 0,
    is_subsanar: false,
    observaciones: null,
    solicitante: null,
    observaciones_reparo: '',
    fecha_ingreso: null,
    require_documentacion: null,
    realiza_pago: null,
    encargado_firma: false,
    requiere_inscripcion_cbr: false,
    is_active: true,
    row_status: true,
    created_by: 0,
    created_date: now(),
    modified_by: null,
    modified_date: null,

    antecedentes_comprador: [],
    antecedentes_vendedor: [],
    validacion_rectificatoria_legal_postventa_datos_personales: [buildValidacionRectificatoriaLegalPostventaDatosPersonalesInitialState(id_expediente)],
});



const normalizeValidacionRectificatoriaLegalPostventaDatosPersonales = (
    source: Partial<ValidacionRectificatoriaLegalPostventaDatosPersonales> | null | undefined,
    id_expediente_fallback: number,
    id_validacion_rectificatoria_legal_postventa_fallback = 0,
): ValidacionRectificatoriaLegalPostventaDatosPersonales => ({
    ...buildValidacionRectificatoriaLegalPostventaDatosPersonalesInitialState(
        Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
        Number(source?.id_validacion_rectificatoria_legal_postventa ?? id_validacion_rectificatoria_legal_postventa_fallback ?? 0),
    ),
    id_validacion_rectificatoria_legal_postventa_datos_personales: Number(
        source?.id_validacion_rectificatoria_legal_postventa_datos_personales ?? 0,
    ),
    id_validacion_rectificatoria_legal_postventa: Number(
        source?.id_validacion_rectificatoria_legal_postventa ?? id_validacion_rectificatoria_legal_postventa_fallback ?? 0,
    ),
    id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
    rut: source?.rut ?? '',
    rol_comparecencia: source?.rol_comparecencia ?? '',
    nombres: source?.nombres ?? '',
    apellido_paterno: source?.apellido_paterno ?? '',
    apellido_materno: source?.apellido_materno ?? '',
    fecha_nacimiento: source?.fecha_nacimiento ?? null,
    genero: source?.genero ?? null,
    estado_civil: source?.estado_civil ?? null,
    relacion_titular: source?.relacion_titular ?? null,
    direccion: source?.direccion ?? '',
    region: source?.region ?? null,
    comuna: source?.comuna ?? null,
    telefono: source?.telefono ?? '',
    email: source?.email ?? '',
    nacionalidad: source?.nacionalidad ?? null,
    profesion: source?.profesion ?? '',
    is_active: source?.is_active ?? true,
    row_status: source?.row_status ?? true,
    created_by: Number(source?.created_by ?? 0),
    created_date: source?.created_date ?? now(),
    modified_by: source?.modified_by ?? null,
    modified_date: source?.modified_date ?? null,
});

const normalizeValidacionRectificatoriaLegalPostventaDatoPersonal = (
    source: Partial<ValidacionRectificatoriaLegalPostventaDatosPersonales>[] | null | undefined,
    id_expediente_fallback: number,
    id_validacion_rectificatoria_legal_postventa_fallback = 0,
): ValidacionRectificatoriaLegalPostventaDatosPersonales[] =>
    (source ?? []).map((vendedor) =>
        normalizeValidacionRectificatoriaLegalPostventaDatosPersonales(
            vendedor,
            id_expediente_fallback,
            id_validacion_rectificatoria_legal_postventa_fallback,
        ),
    );



const normalizeValidacionRectificatoriaLegalPostventa = (
    source: Partial<ValidacionRectificatoriaLegalPostventa> | null | undefined,
    id_expediente_fallback: number,
): ValidacionRectificatoriaLegalPostventa => {
    const idValidacionRectificatoriaLegalPostventa = Number(source?.id_validacion_rectificatoria_legal_postventa ?? 0);
    const idExpediente = Number(source?.id_expediente ?? id_expediente_fallback ?? 0);

    return {
        id_validacion_rectificatoria_legal_postventa: idValidacionRectificatoriaLegalPostventa,
        id_expediente: idExpediente,
        id_usuario_solicitante: Number(source?.id_usuario_solicitante ?? 0),
        is_subsanar: source?.is_subsanar ?? true,
        observaciones: source?.observaciones ?? null,
        solicitante: source?.solicitante ?? null,
        observaciones_reparo: source?.observaciones_reparo ?? null,
        fecha_ingreso: source?.fecha_ingreso ?? null,
        require_documentacion: source?.require_documentacion ?? null,
        realiza_pago: source?.realiza_pago ?? null,
        encargado_firma: source?.encargado_firma ?? false,
        requiere_inscripcion_cbr: source?.requiere_inscripcion_cbr ?? false,
        is_active: source?.is_active ?? true,
        row_status: source?.row_status ?? true,
        created_by: Number(source?.created_by ?? 0),
        created_date: source?.created_date ?? now(),
        modified_by: source?.modified_by ?? null,
        modified_date: source?.modified_date ?? null,
        antecedentes_comprador: source?.antecedentes_comprador ?? [],
        antecedentes_vendedor: source?.antecedentes_vendedor ?? [],
        validacion_rectificatoria_legal_postventa_datos_personales: normalizeValidacionRectificatoriaLegalPostventaDatoPersonal(
            source?.validacion_rectificatoria_legal_postventa_datos_personales,
            idExpediente,
            idValidacionRectificatoriaLegalPostventa,
        ),
    };
};

const format_date = (value: string | null | undefined): string => {
    if (!value) return "";

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return value;

    return date.toLocaleDateString("es-CL", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
    });
};

export default function ValidacionRectificatoriaLegalPostventaPage() {
    const toast = useRef<Toast>(null);

    const navigate = useNavigate();
    const { id_expediente: idExpedienteParam } = useParams();

    const id_expediente = Number(idExpedienteParam ?? 0);

    const [form, setForm] = useState<ValidacionRectificatoriaLegalPostventa>(
        buildInitialState(id_expediente),
    );


    const [isDisabled, setIsDisabled] = useState(true);
    const [canAdvance, setCanAdvance] = useState(false);
    const [isBusy, setIsBusy] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    const hasHydratedRef = useRef(false);
    const currentExpedienteRef = useRef<number>(id_expediente);
    const [rolComparecenciaSeleccionado, setRolComparecenciaSeleccionado] =
        useState<string | null>(null);

    const [seleccionados, setSeleccionados] = useState<
        ValidacionRectificatoriaLegalPostventaDatosPersonales[]
    >([]);
    const { data, isLoading } = useValidacionRectificatoriaLegalPostventa(id_expediente);
    const saveMutation = useUpsertValidacionRectificatoriaLegalPostventa();
    const avanzarMutation = useAvanzarValidacionRectificatoriaLegalPostventa();
    const toggle_subsanar = (checked: boolean) => {
        setForm((prev) => ({
            ...prev,
            is_subsanar: checked,
        }));

        setCanAdvance(false);
    };

    const updateField = (field: string, value: any) => {
        setForm((prev) => ({
            ...prev,
            [field]: value,
        }));
    };
    const getDescripcionCatalogo = (
        lista: any[],
        code: string | number | null | undefined,
    ) => {
        return lista.find((x) => x.code === code)?.description ?? code;
    };
    const antecedentesFiltrados = useMemo(() => {
        switch (rolComparecenciaSeleccionado) {
            case '1':
                return (form.antecedentes_comprador ?? []).map((x) => ({
                    ...x,
                    rol_comparecencia: '1',
                }));

            case '2':
                return (form.antecedentes_vendedor ?? []).map((x) => ({
                    ...x,
                    rol_comparecencia: '2',
                }));
            case '3':
                return [
                    ...(form.antecedentes_comprador ?? []).map((x) => ({
                        ...x,
                        rol_comparecencia: '3',
                    })),
                    ...(form.antecedentes_vendedor ?? []).map((x) => ({
                        ...x,
                        rol_comparecencia: '3',
                    })),
                ].filter(
                    (x) => x.estado_civil === '02'
                );

            case '4':
                return [
                    ...(form.antecedentes_comprador ?? []).map((x) => ({
                        ...x,
                        rol_comparecencia: '4',
                    })),
                    ...(form.antecedentes_vendedor ?? []).map((x) => ({
                        ...x,
                        rol_comparecencia: '4',
                    })),
                ].filter((x) => {
                    const nacionalidad = x.nacionalidad;

                    return nacionalidad !== 'CL';
                });

            default:
                return [];
        }
    }, [
        rolComparecenciaSeleccionado,
        form.antecedentes_comprador,
        form.antecedentes_vendedor,
    ]);
    const controlesValidacionRectificatoriaLegalPostventaQuery = useControlesValidacionRectificatoriaLegalPostventa();

    const catalogo: ControlesValidacionRectificatoriaLegalPostventa = useMemo(() => {
        if (!controlesValidacionRectificatoriaLegalPostventaQuery.data?.status) {
            return EMPTY_CONTROLES_VALIDACION_RECTIFICATORIA_LEGAL_POSTVENTA;
        }

        return (
            controlesValidacionRectificatoriaLegalPostventaQuery.data.detail ??
            EMPTY_CONTROLES_VALIDACION_RECTIFICATORIA_LEGAL_POSTVENTA
        );
    }, [controlesValidacionRectificatoriaLegalPostventaQuery.data]);
    useEffect(() => {
        if (currentExpedienteRef.current !== id_expediente) {
            currentExpedienteRef.current = id_expediente;
            hasHydratedRef.current = false;
            setForm(buildInitialState(id_expediente));
            setIsDisabled(true);
            setCanAdvance(false);
            setErrorMessage('');
            setSuccessMessage('');
        }
    }, [id_expediente]);

    useEffect(() => {
        if (hasHydratedRef.current) return;

        if (!id_expediente || id_expediente <= 0) {
            setForm(buildInitialState(0));
            setIsDisabled(false);
            setCanAdvance(false);
            hasHydratedRef.current = true;
            return;
        }

        if (data?.status && data.detail) {
            setForm(normalizeValidacionRectificatoriaLegalPostventa(data.detail, id_expediente));
            setIsDisabled(Number(data.detail.id_validacion_rectificatoria_legal_postventa) > 0);
            setCanAdvance(false);
            hasHydratedRef.current = true;
            return;
        }

        if (data) {
            setForm(buildInitialState(id_expediente));
            setIsDisabled(true);
            setCanAdvance(false);
            hasHydratedRef.current = true;
        }
    }, [data, id_expediente]);


    const updateValidacionRectificatoriaLegalPostventaDatosPersonales = (
        validacion_rectificatoria_legal_postventa_datos_personales: ValidacionRectificatoriaLegalPostventaDatosPersonales[],
    ) => {
        setForm((prev) => ({
            ...prev,
            validacion_rectificatoria_legal_postventa_datos_personales: normalizeValidacionRectificatoriaLegalPostventaDatoPersonal(
                validacion_rectificatoria_legal_postventa_datos_personales,
                prev.id_expediente,
                prev.id_validacion_rectificatoria_legal_postventa,
            ),
        }));
    };
    const handleAgregarSeleccionados = () => {
        const existentes =
            form.validacion_rectificatoria_legal_postventa_datos_personales ?? [];

        const nuevos = seleccionados.filter(
            (nuevo) =>
                !existentes.some(
                    (x) =>
                        x.rut === nuevo.rut &&
                        x.rol_comparecencia === rolComparecenciaSeleccionado,
                ),
        );

        updateValidacionRectificatoriaLegalPostventaDatosPersonales([
            ...existentes,
            ...nuevos.map((x) => ({
                ...x,
                rol_comparecencia: rolComparecenciaSeleccionado ?? '',
            })),
        ]);

        setSeleccionados([]);
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


        return '';
    };

    const buildPayload = () => {
        const expedienteId = Number(form.id_expediente || id_expediente || 0);
        const idValidacionRectificatoriaLegalPostventa = Number(form.id_validacion_rectificatoria_legal_postventa ?? 0);

        return normalizeValidacionRectificatoriaLegalPostventa(
            {
                ...form,
                id_expediente: expedienteId,
                validacion_rectificatoria_legal_postventa_datos_personales: normalizeValidacionRectificatoriaLegalPostventaDatoPersonal(

                    form.validacion_rectificatoria_legal_postventa_datos_personales,
                    expedienteId,
                    idValidacionRectificatoriaLegalPostventa,
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

            const response = await saveMutation.mutateAsync(payload);

            if (response.status) {
                const savedEntity = normalizeValidacionRectificatoriaLegalPostventa(
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

            const saveResponse = await saveMutation.mutateAsync(payload);

            if (!saveResponse.status) {
                toast.current?.show({
                    severity: 'warn',
                    summary: 'Atención',
                    detail: saveResponse.message || 'No se pudo guardar antes de avanzar.',
                    life: 3000,
                });
                return;
            }

            const savedEntity = normalizeValidacionRectificatoriaLegalPostventa(
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
                toast.current?.show({
                    severity: 'success',
                    summary: 'Éxito',
                    detail: 'Actividad avanzada correctamente.',
                    life: 2000,
                });

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
                Validación Rectificatoria Legal
            </h2>

            <Accordion activeIndex={[0, 2]} multiple>
                <AccordionTab
                    disabled={!id_expediente || id_expediente <= 0}
                    header="Información del Expediente"
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

                <AccordionTab header="Validación Rectificatoria Legal">


                    <div className="flex flex-col gap-6">
                        <ReparoTabla
                            form={form}
                            is_disabled={isDisabled}
                            on_toggle_subsanar={toggle_subsanar}
                        />
                    </div>
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


                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5 mb-4">
                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">Rol Comparecencia</label>
                                <Dropdown
                                    value={rolComparecenciaSeleccionado ?? null}
                                    options={catalogo.rol_comparecencia}
                                    optionLabel="description"
                                    optionValue="code"
                                    onChange={(e) => setRolComparecenciaSeleccionado(e.value)}
                                    disabled={isDisabled}
                                    className="form-dropdown-presto w-full"
                                    loading={isLoading}
                                    placeholder="Seleccione"
                                    emptyMessage="Sin resultados"
                                    showClear
                                    filter
                                />
                            </div>
                        </div>
                        <DataTable
                            value={antecedentesFiltrados}
                            selection={seleccionados}
                            onSelectionChange={(e) => setSeleccionados(e.value)}
                            dataKey="rut"
                        >
                            <Column
                                header="Rol Comparecencia"
                                body={(rowData) =>
                                    getDescripcionCatalogo(
                                        catalogo.rol_comparecencia,
                                        rowData.rol_comparecencia
                                    )
                                }
                            />
                            <Column
                                header="Relación Titular"
                                body={(rowData) =>
                                    getDescripcionCatalogo(
                                        catalogo.relacion_titular,
                                        rowData.relacion_titular
                                    )
                                }
                            />
                            <Column field="rut" header="Rut" />
                            <Column field="nombres" header="Nombres" />
                            <Column field="apellido_paterno" header="Apellido Paterno" />
                            <Column field="apellido_materno" header="Apellido Materno" />
                            <Column selectionMode="multiple" header="Falta firmar" />

                        </DataTable>
                        <div className="flex justify-center mt-4">
                            <Button
                                type="button"
                                label="Grabar"
                                icon="pi pi-check"
                                severity="info"
                                onClick={handleAgregarSeleccionados}
                                disabled={
                                    isDisabled ||
                                    seleccionados.length === 0
                                }
                            />
                        </div>
                        <div className="my-8 border-t border-gray-200" />

                        <ValidacionRectificatoriaLegalPostventaSection
                            key={`antecedentes-comprador-${canEditAntecedentesComprador ? 'edit' : 'view'}`}
                            value={form.validacion_rectificatoria_legal_postventa_datos_personales ?? []}
                            idExpediente={Number(form.id_expediente || id_expediente || 0)}
                            idValidacionRectificatorialegalPostventa={Number(form.id_validacion_rectificatoria_legal_postventa ?? 0)}
                            disabled={isDisabled || isBusy}
                            canEdit={canEditAntecedentesComprador}
                            controles={catalogo}
                            loadingControles={isLoading}
                            onChange={updateValidacionRectificatoriaLegalPostventaDatosPersonales}
                            onWarn={(message) =>
                                toast.current?.show({
                                    severity: 'warn',
                                    summary: 'Validación',
                                    detail: message,
                                    life: 3000,
                                })
                            }
                        />


                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">

                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">Requiere solicitar documentación</label>

                                <div className="flex gap-4">
                                    {(catalogo.tipo_requerimiento_documentacion ?? []).map((item) => (
                                        <div key={item.code} className="flex items-center gap-2">
                                            <RadioButton
                                                inputId={`require_documentacion-${item.code}`}
                                                name="require_documentacion"
                                                value={item.code}

                                                onChange={(e) => updateField('require_documentacion', e.value ?? null)}
                                                checked={form.require_documentacion === item.code}
                                                disabled={isDisabled}
                                                className="form-radio-presto"
                                            />
                                            <label htmlFor={`require_documentacion-${item.code}`}>
                                                {item.description}
                                            </label>
                                        </div>
                                    ))}
                                </div>
                            </div>
                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">¿Quien realiza el pago?</label>

                                <div className="flex gap-4">
                                    {(catalogo.realiza_pago ?? []).map((item) => (
                                        <div key={item.code} className="flex items-center gap-2">
                                            <RadioButton
                                                inputId={`realiza_pago-${item.code}`}
                                                name="realiza_pago"
                                                value={item.code}
                                                onChange={(e) => updateField('realiza_pago', e.value ?? null)}
                                                checked={form.realiza_pago === item.code}
                                                disabled={isDisabled}
                                                className="form-radio-presto"
                                            />
                                            <label htmlFor={`realiza_pago-${item.code}`}>
                                                {item.description}
                                            </label>
                                        </div>
                                    ))}
                                </div>
                            </div>

                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">¿Quién firma?</label>

                                <div className="flex items-center gap-6 h-10">
                                    <div className="flex items-center gap-2">
                                        <RadioButton
                                            className="form-radio-presto"
                                            inputId="encargado_firma_si"
                                            checked={form?.encargado_firma === true}
                                            disabled={isDisabled}
                                            onChange={() => updateField('encargado_firma', true)}
                                        />

                                        <label
                                            htmlFor="encargado_firma_si"
                                            className="text-sm text-gray-700"
                                        >
                                            Otras partes
                                        </label>
                                    </div>

                                    <div className="flex items-center gap-2">
                                        <RadioButton
                                            className="form-radio-presto"
                                            inputId="encargado_firma_no"
                                            checked={form?.encargado_firma === false}
                                            disabled={isDisabled}
                                            onChange={() => updateField('encargado_firma', false)}
                                        />

                                        <label
                                            htmlFor="encargado_firma_no"
                                            className="text-sm text-gray-700"
                                        >
                                            Solo banco
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">¿Requiere Inscripción CBR?</label>

                                <div className="flex items-center gap-6 h-10">
                                    <div className="flex items-center gap-2">
                                        <RadioButton
                                            className="form-radio-presto"
                                            inputId="requiere_inscripcion_cbr_si"
                                            checked={form?.requiere_inscripcion_cbr === true}
                                            disabled={isDisabled}
                                            onChange={() => updateField('requiere_inscripcion_cbr', true)}
                                        />

                                        <label
                                            htmlFor="requiere_inscripcion_cbr_si"
                                            className="text-sm text-gray-700"
                                        >
                                            Sí
                                        </label>
                                    </div>

                                    <div className="flex items-center gap-2">
                                        <RadioButton
                                            className="form-radio-presto"
                                            inputId="requiere_inscripcion_cbr_no"
                                            checked={form?.requiere_inscripcion_cbr === false}
                                            disabled={isDisabled}
                                            onChange={() => updateField('requiere_inscripcion_cbr', false)}
                                        />

                                        <label
                                            htmlFor="requiere_inscripcion_cbr_no"
                                            className="text-sm text-gray-700"
                                        >
                                            No
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div className="flex flex-col gap-6">

                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">Observaciones *</label>
                                <InputTextarea
                                    value={form.observaciones ?? ""}
                                    onChange={(e) =>
                                        setForm((prev) => ({ ...prev, observaciones: e.target.value }))
                                    }
                                    rows={4}
                                    autoResize
                                    className="form-textarea-presto w-full"
                                    disabled={isDisabled}
                                    placeholder="Ingrese observaciones relevantes para la operación"
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
                                label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'}
                                icon="pi pi-arrow-right"
                                severity="warning"
                                onClick={handleAvanzar}
                                disabled={isBusy || (!canAdvance && !form.id_validacion_rectificatoria_legal_postventa)}
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
interface ReparoTablaProps {
    form: ValidacionRectificatoriaLegalPostventa;
    is_disabled: boolean;
    on_toggle_subsanar: (checked: boolean) => void;
}

function ReparoTabla({
    form,
    is_disabled,
    on_toggle_subsanar,
}: ReparoTablaProps) {
    return (
        <div>
            <div className="bg-blue-700 text-white px-4 py-2 font-semibold text-sm uppercase">
                Detalles del Reparo
            </div>

            <div className="w-full overflow-x-auto">
                <table className="w-full border-collapse">
                    <thead>
                        <tr className="bg-gray-50">
                            <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left w-1/4">
                                Solicitante
                            </th>
                            <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-left">
                                Observaciones del Reparo
                            </th>
                            <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-center w-32">
                                Fecha de Ingreso
                            </th>
                            <th className="px-3 py-2 text-sm font-semibold text-gray-700 border border-gray-200 text-center w-24">
                                Subsanar
                            </th>
                        </tr>
                    </thead>

                    <tbody>
                        <tr className="bg-white">
                            <td className="px-3 py-2 text-sm border border-gray-200">
                                {form.solicitante || "-"}
                            </td>
                            <td className="px-3 py-2 text-sm border border-gray-200">
                                {form.observaciones_reparo || "-"}
                            </td>
                            <td className="px-3 py-2 text-sm border border-gray-200 text-center">
                                {format_date(form.fecha_ingreso)}
                            </td>
                            <td className="px-3 py-2 border border-gray-200 text-center">
                                <Checkbox
                                    inputId="is_subsanar"
                                    className="form-checkbox-presto"
                                    checked={form.is_subsanar}
                                    onChange={(e) => on_toggle_subsanar(!!e.checked)}
                                    disabled={is_disabled}
                                />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    );
}
