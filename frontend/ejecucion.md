## CONTEXTO DEL PROYECTO

Eres un desarrollador senior React/TypeScript trabajando en el proyecto
**BBVA Colombia — Presto Legalización**.

Este frontend es una adaptación del sistema **Multibanca Chile** al mercado
colombiano para BBVA.

**Stack:** React 18, TypeScript, Vite, TanStack Query (React Query),
Axios, PrimeReact, Tailwind CSS, React Router v6.

**Estructura de features** (patrón establecido — NO cambiar):
```
src/features/{nombre_feature}/
├── api/          → servicios Axios
├── hooks/        → useQuery / useMutation
├── models/       → interfaces TypeScript
├── components/   → componentes de secciones
└── pages/        → página principal (usa EncabezadoActividad + FuncionesTransversales)
```

---

## OBJETIVO GENERAL

Adaptar el frontend para que consuma la nueva BD BBVA Colombia
**sin romper las pantallas existentes**.

**Principio guía:** Backward compatibility primero. Todo lo nuevo se agrega,
nada se elimina. Los campos de Chile quedan como opcionales (`?`).

---

## PASO 1 — LEE ESTOS ARCHIVOS ANTES DE TOCAR NADA

Lee los siguientes archivos en este orden. Confirma que entendiste la
estructura antes de continuar.

```
1.  src/features/encabezado/models/encabezado.ts
2.  src/features/encabezado/api/encabezadoService.ts
3.  src/features/encabezado/hooks/useEncabezado.ts
4.  src/features/encabezado/pages/EncabezadoActividad.tsx
5.  src/features/funciones_transversales/pages/FuncionesTransversales.tsx
6.  src/features/actividades/carga_operacion_banco/models/carga_operacion_banco.ts
7.  src/features/actividades/carga_operacion_banco/models/catalogo.ts
8.  src/features/actividades/carga_operacion_banco/api/cargaOperacionBancoService.ts
9.  src/features/actividades/carga_operacion_banco/api/catalogoService.ts
10. src/features/actividades/carga_operacion_banco/pages/carga_operacion_banco_page.tsx
11. src/features/actividades/carga_operacion_banco/components/AntecedentesCompradorSection.tsx
12. src/features/actividades/carga_operacion_banco/components/AntecedenteCreditoSection.tsx
13. src/features/actividades/carga_operacion_banco/components/DatosOperacionSection.tsx
14. src/features/actividades/carga_operacion_banco/hooks/useUpsertCargaOperacionBanco.ts
15. src/routes/Routes.tsx
16. src/shared/components/layout/Menu.tsx
17. src/shared/models/ControlBaseDTO.ts
18. src/core/api/axiosClient.ts
```

---

## PASO 2 — ACTUALIZAR MODELOS EXISTENTES

### 2.1 src/features/encabezado/models/encabezado.ts

La interfaz `EncabezadoDTO` tiene campos de Chile (rut, nro_mutuo, banco_alzante,
region, etc.). NO los elimines — mantenlos como opcionales para backward
compatibility. Agrega los campos Colombia al final:

```typescript
// ============================================================
// CAMPOS BBVA COLOMBIA — agregados para Presto Legalización
// Los campos de Chile se mantienen para compatibilidad.
// ============================================================

// Identificación Colombia
id_scoring?: string | null;

// Titular T1 Colombia
tipo_documento_id_t1?: string | null;
numero_identificacion_t1?: string | null;
nombre_completo_t1?: string | null;
celular_t1?: string | null;

// Titular T2 Colombia
numero_identificacion_t2?: string | null;
nombre_completo_t2?: string | null;

// Datos laborales
cliente_nomina?: boolean | null;
situacion_laboral?: string | null;
correo_declarativo?: string | null;
telefono_declarativo?: string | null;

// Oficina y asesor BBVA
codigo_oficina_bbva?: string | null;
descripcion_oficina_bbva?: string | null;
codigo_asesor_bbva?: string | null;

// Crédito Colombia
id_tipo_sub_producto?: string | null;
monto_otorgado?: number | null;
canal_originacion?: string | null;
fecha_aprobacion?: string | null;

// Inmueble Colombia
codigo_proyecto?: string | null;
descripcion_proyecto?: string | null;
estado_inmueble?: string | null;
tipo_inmueble?: string | null;
condiciones_organismo_decisor?: string | null;
```

### 2.2 src/features/actividades/carga_operacion_banco/models/carga_operacion_banco.ts

En la interfaz `CargaOperacionBancoDatosOperacion`, agrega al final:

```typescript
// BBVA Colombia
id_scoring?: string | null;
codigo_asesor?: string | null;
codigo_oficina?: string | null;
descripcion_oficina?: string | null;
canal_originacion?: string | null;
tipo_inmueble?: string | null;
estado_inmueble?: string | null;
codigo_proyecto?: string | null;
descripcion_proyecto?: string | null;
```

En la interfaz `CargaOperacionBancoAntecedenteComprador`, agrega al final:

```typescript
// BBVA Colombia
numero_identificacion?: string | null;
tipo_documento_id?: string | null;
nombre_completo?: string | null;
celular?: string | null;
departamento?: string | null;
municipio?: string | null;
situacion_laboral?: string | null;
cliente_nomina?: boolean | null;
tipo_titular?: string | null;  // T1, T2, T3
```

En la interfaz `CargaOperacionBancoAntecedenteCredito`, agrega al final:

```typescript
// BBVA Colombia
id_tipo_sub_producto?: string | null;
monto_otorgado?: number | null;
fecha_aprobacion?: string | null;
condiciones_organismo_decisor?: string | null;
aplica_fast_track?: boolean | null;
aplica_leasing?: boolean | null;
aplica_cobertura?: boolean | null;
aplica_compra_cartera?: boolean | null;
aplica_remodelacion?: boolean | null;
gente_bbva?: boolean | null;
```

En la interfaz `CargaOperacionBancoDatosComercial`, agrega al final:

```typescript
// BBVA Colombia
correo_declarativo_cliente?: string | null;
numero_telefono_declarativo?: string | null;
```

### 2.3 src/features/actividades/carga_operacion_banco/models/catalogo.ts

Agrega las nuevas interfaces de catálogos Colombia al final del archivo:

```typescript
// ============================================================
// CATÁLOGOS BBVA COLOMBIA
// ============================================================

export interface ControlesDatosOperacionBBVA {
  tipo_credito: CatalogoOption[];
  segmento: CatalogoOption[];
  canal_originacion: CatalogoOption[];
  tipo_inmueble: CatalogoOption[];
  estado_inmueble: CatalogoOption[];
  departamento: CatalogoOption[];
  municipio: CatalogoOption[];
}

export interface ControlesAntecedenteCompradorBBVA {
  tipo_documento_id: CatalogoOption[];
  genero: CatalogoOption[];
  estado_civil: CatalogoOption[];
  relacion_titular: CatalogoOption[];
  departamento: CatalogoOption[];
  municipio: CatalogoOption[];
  situacion_laboral: CatalogoOption[];
}

export interface ControlesCreditoBBVA {
  tipo_subproducto: CatalogoOption[];
  tipo_financiamiento: CatalogoOption[];
  tipo_tasa: CatalogoOption[];
}

export interface ControlesValidarInformacion {
  tipo_documento_id: CatalogoOption[];
  estatus_general: CatalogoOption[];
  motivo_devolucion: CatalogoOption[];
  tipo_inmueble: CatalogoOption[];
  departamento: CatalogoOption[];
  municipio: CatalogoOption[];
  situacion_laboral: CatalogoOption[];
  canal_contacto: CatalogoOption[];
  resultado_contacto: CatalogoOption[];
}
```

---

## PASO 3 — ACTUALIZAR ENCABEZADO (el cambio más visible)

### 3.1 src/features/encabezado/pages/EncabezadoActividad.tsx

Lee el archivo actual. Muestra campos como `rut`, `nro_mutuo`, `banco_alzante`,
`segmento`, `inmobiliaria`, etc. con la función `LabelItem`.

**Estrategia:** Agrega una nueva sección de campos Colombia DESPUÉS de los
existentes, usando el mismo componente `LabelItem`. Los campos de Chile los
dejamos pero los ocultamos condicionalmente si el campo Colombia tiene valor.

Agrega esta sección al JSX, después de los `LabelItem` existentes y antes del
cierre del `Card`:

```tsx
{/* ── CAMPOS BBVA COLOMBIA ────────────────────────────────── */}
{(encabezado.id_scoring || encabezado.numero_identificacion_t1) && (
  <>
    <div className="col-span-full border-t border-gray-200 mt-2 pt-2">
      <span className="text-xs font-semibold text-blue-700 uppercase tracking-wide">
        Información BBVA Colombia
      </span>
    </div>
    {encabezado.id_scoring && (
      <LabelItem label="ID Scoring" value={encabezado.id_scoring} />
    )}
    {encabezado.id_tipo_sub_producto && (
      <LabelItem label="SubProducto" value={encabezado.id_tipo_sub_producto} />
    )}
    {encabezado.nombre_completo_t1 && (
      <LabelItem label="Titular Principal" value={encabezado.nombre_completo_t1} />
    )}
    {encabezado.numero_identificacion_t1 && (
      <LabelItem
        label={encabezado.tipo_documento_id_t1 ?? 'Documento T1'}
        value={encabezado.numero_identificacion_t1}
      />
    )}
    {encabezado.celular_t1 && (
      <LabelItem label="Celular" value={encabezado.celular_t1} />
    )}
    {encabezado.nombre_completo_t2 && (
      <LabelItem label="Titular 2" value={encabezado.nombre_completo_t2} />
    )}
    {encabezado.monto_otorgado != null && (
      <LabelItem
        label="Monto Otorgado"
        value={formatImporte(encabezado.monto_otorgado)}
      />
    )}
    {encabezado.canal_originacion && (
      <LabelItem label="Canal Originación" value={encabezado.canal_originacion} />
    )}
    {encabezado.descripcion_proyecto && (
      <LabelItem label="Proyecto" value={encabezado.descripcion_proyecto} />
    )}
    {encabezado.tipo_inmueble && (
      <LabelItem label="Tipo Inmueble" value={encabezado.tipo_inmueble} />
    )}
    {encabezado.codigo_oficina_bbva && (
      <LabelItem
        label="Oficina"
        value={`${encabezado.codigo_oficina_bbva} — ${encabezado.descripcion_oficina_bbva ?? ''}`}
      />
    )}
    {encabezado.fecha_aprobacion && (
      <LabelItem label="Fecha Aprobación" value={formatFecha(encabezado.fecha_aprobacion)} />
    )}
  </>
)}
```

---

## PASO 4 — NUEVOS MODELOS BBVA

Crea el archivo `src/features/actividades/validar_informacion/models/validar_informacion.ts`:

```typescript
// ============================================================
// BBV-43 — Validar Información BBVA Colombia
// ============================================================

export interface ValidarInformacionBBVA {
  id?: number;
  id_expediente: number;
  id_actividad: string;

  // Titular T1
  tipo_id_t1?: string | null;
  numero_id_t1?: string | null;
  nombre_completo_t1?: string | null;
  celular_t1?: string | null;
  telefono_t1?: string | null;
  email_t1?: string | null;
  direccion_t1?: string | null;
  departamento_t1?: string | null;
  municipio_t1?: string | null;
  situacion_laboral_t1?: string | null;
  cliente_nomina_t1?: boolean | null;

  // Titular T2
  tipo_id_t2?: string | null;
  numero_id_t2?: string | null;
  nombre_completo_t2?: string | null;
  celular_t2?: string | null;
  email_t2?: string | null;

  // Titular T3
  tipo_id_t3?: string | null;
  numero_id_t3?: string | null;
  nombre_completo_t3?: string | null;
  celular_t3?: string | null;
  email_t3?: string | null;

  // Inmueble
  inmueble_definido?: boolean | null;
  tipo_inmueble?: string | null;
  estado_inmueble?: string | null;
  constructora?: string | null;
  es_constructora_vip?: boolean | null;
  codigo_proyecto?: string | null;
  descripcion_proyecto?: string | null;
  departamento_inmueble?: string | null;
  municipio_inmueble?: string | null;

  // Estatus y flags
  estatus_general?: string | null;
  requiere_definir_inmueble?: boolean | null;
  requiere_carga_cliente?: boolean | null;
  requiere_carga_constructora?: boolean | null;

  // Auditoría
  is_active?: boolean;
  row_status?: boolean;
  created_by?: number;
  created_date?: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RegistroContactoBBVA {
  id?: number;
  id_expediente: number;
  id_actividad: string;
  id_usuario?: number;
  canal_contacto: string;
  resultado_contacto: string;
  observaciones?: string | null;
  fecha_contacto?: string;
}

export const EMPTY_VALIDAR_INFORMACION = (
  id_expediente: number,
): ValidarInformacionBBVA => ({
  id_expediente,
  id_actividad: 'ACT_VALIDAR_INFO',
  inmueble_definido: false,
  estatus_general: 'SIN_INM',
  requiere_definir_inmueble: false,
  requiere_carga_cliente: false,
  requiere_carga_constructora: false,
});
```

Crea el archivo `src/features/actividades/validar_informacion/models/catalogo.ts`:

```typescript
import type { CatalogoOption } from
  '@/features/actividades/carga_operacion_banco/models/catalogo';

export interface ControlesValidarInformacion {
  tipo_documento_id: CatalogoOption[];
  estatus_general: CatalogoOption[];
  motivo_devolucion: CatalogoOption[];
  tipo_inmueble: CatalogoOption[];
  departamento: CatalogoOption[];
  municipio: CatalogoOption[];
  situacion_laboral: CatalogoOption[];
  canal_contacto: CatalogoOption[];
  resultado_contacto: CatalogoOption[];
}

export const EMPTY_CONTROLES_VALIDAR_INFORMACION: ControlesValidarInformacion = {
  tipo_documento_id:  [],
  estatus_general:    [],
  motivo_devolucion:  [],
  tipo_inmueble:      [],
  departamento:       [],
  municipio:          [],
  situacion_laboral:  [],
  canal_contacto:     [],
  resultado_contacto: [],
};
```

---

## PASO 5 — NUEVA FEATURE: validar_informacion

Crea la estructura completa en `src/features/actividades/validar_informacion/`.
Sigue EXACTAMENTE el mismo patrón de `carga_operacion_banco`.

### 5.1 api/validarInformacionService.ts

```typescript
import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionBBVA, RegistroContactoBBVA } from
  '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';

const baseUrl = '/api/validar-informacion';

export const validarInformacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidarInformacionBBVA | null>> {
    const response = await axiosClient.get<ApiResponse<ValidarInformacionBBVA | null>>(
      `${baseUrl}/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(
    payload: ValidarInformacionBBVA,
  ): Promise<ApiResponse<ValidarInformacionBBVA>> {
    const response = await axiosClient.post<ApiResponse<ValidarInformacionBBVA>>(
      `${baseUrl}/guardar`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(
      `${baseUrl}/${id_expediente}/avanzar`,
    );
    return response.data;
  },

  async getControles(
    id_expediente: number,
  ): Promise<ApiResponse<ControlesValidarInformacion>> {
    const response = await axiosClient.get<ApiResponse<ControlesValidarInformacion>>(
      `${baseUrl}/${id_expediente}/controles`,
    );
    return response.data;
  },

  async getRegistrosContacto(
    id_expediente: number,
    id_actividad: string,
  ): Promise<ApiResponse<RegistroContactoBBVA[]>> {
    const response = await axiosClient.get<ApiResponse<RegistroContactoBBVA[]>>(
      `/api/registro-contacto/${id_expediente}/${id_actividad}`,
    );
    return response.data;
  },

  async crearRegistroContacto(
    payload: RegistroContactoBBVA,
  ): Promise<ApiResponse<RegistroContactoBBVA>> {
    const response = await axiosClient.post<ApiResponse<RegistroContactoBBVA>>(
      '/api/registro-contacto',
      payload,
    );
    return response.data;
  },
};
```

### 5.2 hooks/useValidarInformacion.ts

```typescript
import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

export function useValidarInformacion(id_expediente: number) {
  return useQuery<ApiResponse<ValidarInformacionBBVA | null>>({
    queryKey: ['validar-informacion', id_expediente],
    queryFn: () => validarInformacionService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
```

### 5.3 hooks/useUpsertValidarInformacion.ts

```typescript
import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

export function useUpsertValidarInformacion() {
  const queryClient = useQueryClient();
  return useMutation<
    ApiResponse<ValidarInformacionBBVA>,
    Error,
    ValidarInformacionBBVA
  >({
    mutationFn: (payload) => validarInformacionService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['validar-informacion', variables.id_expediente],
      });
    },
  });
}
```

### 5.4 hooks/useAvanzarValidarInformacion.ts

```typescript
import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validarInformacionService } from '../api/validarInformacionService';

export function useAvanzarValidarInformacion() {
  return useMutation<ApiResponse<boolean>, Error, number>({
    mutationFn: (id_expediente) =>
      validarInformacionService.avanzar(id_expediente),
  });
}
```

### 5.5 hooks/useControlesValidarInformacion.ts

```typescript
import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesValidarInformacion } from '../models/catalogo';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from '../models/catalogo';
import { validarInformacionService } from '../api/validarInformacionService';

export function useControlesValidarInformacion(id_expediente: number) {
  return useQuery<ApiResponse<ControlesValidarInformacion>>({
    queryKey: ['controles-validar-informacion', id_expediente],
    queryFn: () => validarInformacionService.getControles(id_expediente),
    enabled: id_expediente > 0,
    placeholderData: {
      data: EMPTY_CONTROLES_VALIDAR_INFORMACION,
      success: true,
      message: '',
    },
  });
}
```

### 5.6 hooks/useRegistroContacto.ts

```typescript
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import type { RegistroContactoBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

export function useRegistrosContacto(
  id_expediente: number,
  id_actividad: string,
) {
  return useQuery({
    queryKey: ['registro-contacto', id_expediente, id_actividad],
    queryFn: () =>
      validarInformacionService.getRegistrosContacto(id_expediente, id_actividad),
    enabled: id_expediente > 0,
  });
}

export function useCrearRegistroContacto() {
  const queryClient = useQueryClient();
  return useMutation<unknown, Error, RegistroContactoBBVA>({
    mutationFn: (payload) =>
      validarInformacionService.crearRegistroContacto(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: [
          'registro-contacto',
          variables.id_expediente,
          variables.id_actividad,
        ],
      });
    },
  });
}
```

### 5.7 components/DatosTitularSection.tsx

```tsx
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { Checkbox } from 'primereact/checkbox';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';

type Props = {
  data: ValidarInformacionBBVA;
  controles: ControlesValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
};

export default function DatosTitularSection({
  data, controles, isEditing, onChange,
}: Props) {
  const titulares = [
    { label: 'Titular 1', prefix: 't1' as const },
    { label: 'Titular 2', prefix: 't2' as const },
    { label: 'Titular 3', prefix: 't3' as const },
  ];

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      {titulares.map(({ label, prefix }) => (
        <div key={prefix} className="col-span-full border rounded-lg p-4">
          <h4 className="font-semibold text-blue-800 mb-3">{label}</h4>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Tipo Documento</label>
              <Dropdown
                value={data[`tipo_id_${prefix}`]}
                options={controles.tipo_documento_id}
                optionLabel="description"
                optionValue="code"
                disabled={!isEditing}
                onChange={(e) => onChange(`tipo_id_${prefix}`, e.value)}
                placeholder="Seleccionar..."
                className="w-full"
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Número Documento</label>
              <InputText
                value={data[`numero_id_${prefix}`] ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`numero_id_${prefix}`, e.target.value)}
                className="w-full"
              />
            </div>
            <div className="flex flex-col gap-1 col-span-full">
              <label className="text-xs text-gray-500">Nombre Completo</label>
              <InputText
                value={data[`nombre_completo_${prefix}`] ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`nombre_completo_${prefix}`, e.target.value)}
                className="w-full"
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Celular</label>
              <InputText
                value={data[`celular_${prefix}`] ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`celular_${prefix}`, e.target.value)}
                className="w-full"
              />
            </div>
            <div className="flex flex-col gap-1">
              <label className="text-xs text-gray-500">Email</label>
              <InputText
                value={data[`email_${prefix}`] ?? ''}
                disabled={!isEditing}
                onChange={(e) => onChange(`email_${prefix}`, e.target.value)}
                className="w-full"
              />
            </div>
            {prefix === 't1' && (
              <>
                <div className="flex flex-col gap-1">
                  <label className="text-xs text-gray-500">Situación Laboral</label>
                  <Dropdown
                    value={data.situacion_laboral_t1}
                    options={controles.situacion_laboral}
                    optionLabel="description"
                    optionValue="code"
                    disabled={!isEditing}
                    onChange={(e) => onChange('situacion_laboral_t1', e.value)}
                    placeholder="Seleccionar..."
                    className="w-full"
                  />
                </div>
                <div className="flex items-center gap-2 mt-4">
                  <Checkbox
                    checked={data.cliente_nomina_t1 ?? false}
                    disabled={!isEditing}
                    onChange={(e) => onChange('cliente_nomina_t1', e.checked)}
                    inputId="cliente_nomina"
                  />
                  <label htmlFor="cliente_nomina" className="text-sm">
                    Cliente Nómina BBVA
                  </label>
                </div>
              </>
            )}
          </div>
        </div>
      ))}
    </div>
  );
}
```

### 5.8 components/DatosInmuebleSection.tsx

```tsx
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { Checkbox } from 'primereact/checkbox';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';

type Props = {
  data: ValidarInformacionBBVA;
  controles: ControlesValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
};

export default function DatosInmuebleSection({
  data, controles, isEditing, onChange,
}: Props) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div className="flex items-center gap-2">
        <Checkbox
          checked={data.inmueble_definido ?? false}
          disabled={!isEditing}
          onChange={(e) => onChange('inmueble_definido', e.checked)}
          inputId="inmueble_definido"
        />
        <label htmlFor="inmueble_definido" className="text-sm font-medium">
          ¿Cliente tiene Inmueble Definido?
        </label>
      </div>

      {data.inmueble_definido && (
        <>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Tipo Inmueble</label>
            <Dropdown
              value={data.tipo_inmueble}
              options={controles.tipo_inmueble}
              optionLabel="description"
              optionValue="code"
              disabled={!isEditing}
              onChange={(e) => onChange('tipo_inmueble', e.value)}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Constructora</label>
            <InputText
              value={data.constructora ?? ''}
              disabled={!isEditing}
              onChange={(e) => onChange('constructora', e.target.value)}
              className="w-full"
            />
          </div>
          <div className="flex items-center gap-2">
            <Checkbox
              checked={data.es_constructora_vip ?? false}
              disabled={!isEditing}
              onChange={(e) => onChange('es_constructora_vip', e.checked)}
              inputId="es_constructora_vip"
            />
            <label htmlFor="es_constructora_vip" className="text-sm">
              Constructora VIP
            </label>
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Código Proyecto</label>
            <InputText
              value={data.codigo_proyecto ?? ''}
              disabled={!isEditing}
              onChange={(e) => onChange('codigo_proyecto', e.target.value)}
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Descripción Proyecto</label>
            <InputText
              value={data.descripcion_proyecto ?? ''}
              disabled={!isEditing}
              onChange={(e) => onChange('descripcion_proyecto', e.target.value)}
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Departamento Inmueble</label>
            <Dropdown
              value={data.departamento_inmueble}
              options={controles.departamento}
              optionLabel="description"
              optionValue="code"
              disabled={!isEditing}
              onChange={(e) => onChange('departamento_inmueble', e.value)}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Municipio Inmueble</label>
            <Dropdown
              value={data.municipio_inmueble}
              options={controles.municipio}
              optionLabel="description"
              optionValue="code"
              disabled={!isEditing}
              onChange={(e) => onChange('municipio_inmueble', e.value)}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
        </>
      )}
    </div>
  );
}
```

### 5.9 components/EstatusGeneralSection.tsx

```tsx
import { Dropdown } from 'primereact/dropdown';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';

type Props = {
  data: ValidarInformacionBBVA;
  controles: ControlesValidarInformacion;
  isEditing: boolean;
  onChange: (field: keyof ValidarInformacionBBVA, value: unknown) => void;
};

export default function EstatusGeneralSection({
  data, controles, isEditing, onChange,
}: Props) {
  const estatusActual = controles.estatus_general.find(
    (e) => e.code === data.estatus_general,
  );

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div className="flex flex-col gap-1">
        <label className="text-xs text-gray-500">Estatus General del Folio</label>
        <Dropdown
          value={data.estatus_general}
          options={controles.estatus_general}
          optionLabel="description"
          optionValue="code"
          disabled={!isEditing}
          onChange={(e) => onChange('estatus_general', e.value)}
          placeholder="Seleccionar estatus..."
          className="w-full"
        />
      </div>
      {estatusActual && (
        <div className="flex items-center">
          <span
            className={`px-3 py-1 rounded-full text-xs font-semibold ${
              data.estatus_general === 'LISTO'
                ? 'bg-green-100 text-green-800'
                : data.estatus_general === 'BLOQUEADO'
                  ? 'bg-red-100 text-red-800'
                  : 'bg-yellow-100 text-yellow-800'
            }`}
          >
            {estatusActual.description}
          </span>
        </div>
      )}
      <div className="col-span-full">
        <label className="text-xs text-gray-500 block mb-2">
          Decisiones de Avance
        </label>
        <div className="flex flex-col gap-2">
          <label className="flex items-center gap-2 text-sm">
            <input
              type="checkbox"
              checked={data.requiere_definir_inmueble ?? false}
              disabled={!isEditing}
              onChange={(e) => onChange('requiere_definir_inmueble', e.target.checked)}
            />
            Requiere Definir Inmueble (crea tarea BBV-44)
          </label>
          <label className="flex items-center gap-2 text-sm">
            <input
              type="checkbox"
              checked={data.requiere_carga_cliente ?? false}
              disabled={!isEditing}
              onChange={(e) => onChange('requiere_carga_cliente', e.target.checked)}
            />
            Requiere Carga de Documentos al Cliente
          </label>
          <label className="flex items-center gap-2 text-sm">
            <input
              type="checkbox"
              checked={data.requiere_carga_constructora ?? false}
              disabled={!isEditing}
              onChange={(e) =>
                onChange('requiere_carga_constructora', e.target.checked)
              }
            />
            Requiere Carga de Documentos a Constructora VIP
          </label>
        </div>
      </div>
    </div>
  );
}
```

### 5.10 components/RegistroContactoSection.tsx

```tsx
import { useState } from 'react';
import { Button } from 'primereact/button';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputTextarea } from 'primereact/inputtextarea';
import type { RegistroContactoBBVA } from '../models/validar_informacion';
import type { ControlesValidarInformacion } from '../models/catalogo';
import {
  useRegistrosContacto,
  useCrearRegistroContacto,
} from '../hooks/useRegistroContacto';

type Props = {
  id_expediente: number;
  id_actividad: string;
  controles: ControlesValidarInformacion;
};

const EMPTY_REGISTRO = (
  id_expediente: number,
  id_actividad: string,
): RegistroContactoBBVA => ({
  id_expediente,
  id_actividad,
  canal_contacto: '',
  resultado_contacto: '',
  observaciones: '',
});

export default function RegistroContactoSection({
  id_expediente, id_actividad, controles,
}: Props) {
  const [visible, setVisible] = useState(false);
  const [form, setForm] = useState<RegistroContactoBBVA>(
    EMPTY_REGISTRO(id_expediente, id_actividad),
  );

  const { data: registros } = useRegistrosContacto(id_expediente, id_actividad);
  const { mutate: crearRegistro, isPending } = useCrearRegistroContacto();

  const handleGuardar = () => {
    crearRegistro(form, {
      onSuccess: () => {
        setVisible(false);
        setForm(EMPTY_REGISTRO(id_expediente, id_actividad));
      },
    });
  };

  const formatFecha = (value?: string | null) =>
    value ? new Date(value).toLocaleString('es-CO') : '—';

  return (
    <div>
      <div className="flex justify-end mb-3">
        <Button
          label="Registrar Contacto"
          icon="pi pi-plus"
          size="small"
          onClick={() => setVisible(true)}
        />
      </div>

      <DataTable
        value={registros?.data ?? []}
        size="small"
        emptyMessage="Sin registros de contacto"
      >
        <Column
          field="fecha_contacto"
          header="Fecha"
          body={(r: RegistroContactoBBVA) => formatFecha(r.fecha_contacto)}
          style={{ width: '160px' }}
        />
        <Column field="canal_contacto" header="Canal" style={{ width: '120px' }} />
        <Column field="resultado_contacto" header="Resultado" style={{ width: '140px' }} />
        <Column field="observaciones" header="Observaciones" />
      </DataTable>

      <Dialog
        header="Registrar Contacto con Cliente"
        visible={visible}
        onHide={() => setVisible(false)}
        style={{ width: '480px' }}
      >
        <div className="flex flex-col gap-4 p-2">
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Canal de Contacto</label>
            <Dropdown
              value={form.canal_contacto}
              options={controles.canal_contacto}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => setForm((f) => ({ ...f, canal_contacto: e.value }))}
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Resultado</label>
            <Dropdown
              value={form.resultado_contacto}
              options={controles.resultado_contacto}
              optionLabel="description"
              optionValue="code"
              onChange={(e) =>
                setForm((f) => ({ ...f, resultado_contacto: e.value }))
              }
              placeholder="Seleccionar..."
              className="w-full"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Observaciones</label>
            <InputTextarea
              value={form.observaciones ?? ''}
              onChange={(e) =>
                setForm((f) => ({ ...f, observaciones: e.target.value }))
              }
              rows={3}
              className="w-full"
            />
          </div>
          <div className="flex justify-end gap-2">
            <Button
              label="Cancelar"
              severity="secondary"
              outlined
              onClick={() => setVisible(false)}
            />
            <Button
              label="Guardar"
              loading={isPending}
              onClick={handleGuardar}
              disabled={!form.canal_contacto || !form.resultado_contacto}
            />
          </div>
        </div>
      </Dialog>
    </div>
  );
}
```

### 5.11 pages/validar_informacion_page.tsx

Crea la página principal. Sigue EXACTAMENTE el mismo patrón de
`carga_operacion_banco_page.tsx`: useParams, Toast ref, Accordion,
botones Guardar/Avanzar con la misma lógica.

```tsx
import { useEffect, useRef, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';
import type { Toast as ToastRef } from 'primereact/toast';
import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from
  '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosTitularSection from '../components/DatosTitularSection';
import DatosInmuebleSection from '../components/DatosInmuebleSection';
import EstatusGeneralSection from '../components/EstatusGeneralSection';
import RegistroContactoSection from '../components/RegistroContactoSection';
import { useValidarInformacion } from '../hooks/useValidarInformacion';
import { useUpsertValidarInformacion } from '../hooks/useUpsertValidarInformacion';
import { useAvanzarValidarInformacion } from '../hooks/useAvanzarValidarInformacion';
import { useControlesValidarInformacion } from '../hooks/useControlesValidarInformacion';
import {
  EMPTY_VALIDAR_INFORMACION,
  type ValidarInformacionBBVA,
} from '../models/validar_informacion';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from '../models/catalogo';

const ACTIVIDAD_ID = 'ACT_VALIDAR_INFO';

export default function ValidarInformacionPage() {
  const { id_expediente: idParam } = useParams();
  const id_expediente = Number(idParam ?? 0);
  const toast = useRef<ToastRef>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [form, setForm] = useState<ValidarInformacionBBVA>(
    EMPTY_VALIDAR_INFORMACION(id_expediente),
  );

  const { data: queryData, isLoading } = useValidarInformacion(id_expediente);
  const { data: controlesData } = useControlesValidarInformacion(id_expediente);
  const { mutate: guardar, isPending: isGuardando } = useUpsertValidarInformacion();
  const { mutate: avanzar, isPending: isAvanzando } = useAvanzarValidarInformacion();

  const controles = controlesData?.data ?? EMPTY_CONTROLES_VALIDAR_INFORMACION;

  useEffect(() => {
    if (queryData?.data) {
      setForm(queryData.data);
    }
  }, [queryData]);

  const updateField = (field: keyof ValidarInformacionBBVA, value: unknown) => {
    setForm((prev) => ({ ...prev, [field]: value }));
    if (!isEditing) setIsEditing(true);
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

  const handleAvanzar = () => {
    avanzar(id_expediente, {
      onSuccess: () => {
        toast.current?.show({
          severity: 'success',
          summary: 'Avanzado',
          detail: 'Folio avanzado a la siguiente etapa',
          life: 3000,
        });
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

  if (isLoading) return <div className="p-4">Cargando...</div>;

  return (
    <div className="flex flex-col gap-4 p-4">
      <Toast ref={toast} />

      <EncabezadoActividad
        idExpediente={id_expediente}
        activityID={ACTIVIDAD_ID}
      />

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
        {/* Panel principal — acordeones */}
        <div className="lg:col-span-2">
          <Card>
            <Accordion multiple>
              <AccordionTab header="Datos Titular">
                <DatosTitularSection
                  data={form}
                  controles={controles}
                  isEditing={true}
                  onChange={updateField}
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
                />
              </AccordionTab>
            </Accordion>

            {/* Footer botones */}
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

        {/* Panel lateral — funciones transversales */}
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
```

---

## PASO 6 — REGISTRAR EN ROUTER Y MENÚ

### 6.1 src/routes/Routes.tsx

Lee el archivo actual. Agrega el import y la ruta de la nueva página,
siguiendo EXACTAMENTE el mismo patrón de las otras actividades.

**Import a agregar** (al final del bloque de imports de actividades):
```tsx
import ValidarInformacionPage from
  '@/features/actividades/validar_informacion/pages/validar_informacion_page';
```

**Ruta a agregar** (dentro del bloque de `<Route>` de actividades, después de
`carga_operacion_banco`):
```tsx
<Route
  path="validar_informacion/:id_expediente"
  element={<ValidarInformacionPage />}
/>
```

### 6.2 src/shared/components/layout/Menu.tsx

Lee el archivo actual. Hay un array de items del menú con `label`, `icon` y `path`.

Agrega el item de la nueva actividad en la sección de actividades BBVA.
Si no existe esa sección, agrégala al final del array de items:

```tsx
{
  label: 'Validar Información',
  icon: 'pi pi-check-circle',
  path: '/home/validar_informacion',
},
```

**Nota:** La ruta usa `:id_expediente` en el router, pero en el menú el usuario
llega desde la bandeja de actividades, así que la ruta del menú puede omitir el
parámetro. La bandeja ya sabe navegar con el ID correcto.

---

## PASO 7 — ACTUALIZAR BANDEJA PARA NAVEGAR A NUEVAS ACTIVIDADES

Lee `src/features/bandeja_actividades/pages/BandejaActividades.tsx`.

Busca la función o el switch que decide a qué URL navegar al hacer clic en
una actividad (probablemente usa el `id_actividad` o el campo `page` de
`cat_actividades_ws`).

Agrega las nuevas rutas de BBVA Colombia al mapa de navegación:

```typescript
// BBVA Colombia — Presto Legalización
'ACT_VALIDAR_INFO':          `/home/validar_informacion/${id_expediente}`,
'ACT_RADICAR_CREDITO':       `/home/carga_operacion_banco/${id_expediente}`,
// Las demás actividades se agregarán conforme se desarrollen
```

Si la bandeja usa el campo `page` de `cat_actividades_ws` directamente como URL,
este paso ya está manejado por los datos que insertamos en la BD — no hay que
tocar nada en el código.

---

## PASO 8 — VERIFICACIÓN FINAL

Después de todos los cambios:

1. Ejecuta `npm run build` (o `yarn build`) para verificar que no hay errores
   de TypeScript
2. Ejecuta `npm run dev` y navega a `/home/bandeja`
3. Verifica que el folio de prueba aparece en la bandeja
4. Haz clic en el folio → debe navegar a `/home/validar_informacion/1001`
5. Verifica que el encabezado muestra los campos BBVA Colombia del folio de prueba
6. Verifica que los 4 acordeones se renderizan sin errores
7. Verifica que los dropdowns cargan datos (requiere que el backend esté corriendo)

---

## REGLAS IMPORTANTES — LEE ANTES DE CUALQUIER CAMBIO

1. **NUNCA** elimines propiedades de interfaces existentes — solo agregar
2. **NUNCA** cambies las rutas existentes del router — solo agregar nuevas
3. **NUNCA** modifiques `FuncionesTransversales.tsx` — úsalo tal cual
4. **NUNCA** modifiques `EncabezadoActividad.tsx` más allá de agregar el
   bloque de campos Colombia indicado
5. **SIEMPRE** usa `?.` (optional chaining) al acceder a campos nuevos
   — pueden ser null en folios viejos
6. **SIEMPRE** sigue el patrón de `carga_operacion_banco` para cualquier
   cosa que no esté explícita en este prompt
7. Los `id_actividad` en el router deben coincidir EXACTAMENTE con los valores
   en `cat_actividades_ws.id_actividad` de la BD
8. Si un componente de PrimeReact que necesitas no está importado en el archivo,
   agrégalo al bloque de imports existente
9. Si algo no está claro, pregunta ANTES de hacer el cambio

---

## ARCHIVOS DE REFERENCIA — PATRONES A SEGUIR

| Necesitas ver el patrón de... | Lee este archivo |
|---|---|
| Página completa con acordeones | `src/features/actividades/carga_operacion_banco/pages/carga_operacion_banco_page.tsx` |
| Service con axios | `src/features/actividades/carga_operacion_banco/api/cargaOperacionBancoService.ts` |
| Hook useQuery | `src/features/actividades/carga_operacion_banco/hooks/useCargaOperacionBanco.ts` |
| Hook useMutation (upsert) | `src/features/actividades/carga_operacion_banco/hooks/useUpsertCargaOperacionBanco.ts` |
| Hook useMutation (avanzar) | `src/features/actividades/carga_operacion_banco/hooks/useAvanzarCargaOperacionBanco.ts` |
| Componente de sección | `src/features/actividades/carga_operacion_banco/components/AntecedentesCompradorSection.tsx` |
| Modal con DataTable | `src/features/funciones_transversales/components/bitacora/pages/Bitacora.tsx` |
| Cómo se usa EncabezadoActividad | `src/features/actividades/carga_operacion_banco/pages/carga_operacion_banco_page.tsx` |
| Cómo se usa FuncionesTransversales | Mismo archivo anterior |
