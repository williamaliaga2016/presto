# BBV-95 — Receta de Implementación: Realizar EP Registradas

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Analista de Vivienda.
> Actividad lineal sin ramificaciones — destino único: Realizar VB Final Abogado.

---

## Flujo

```
Realizar Recepción Boleta (BBV-93)
    → Realizar EP Registradas ← ESTA HU
        → Siempre (único destino): "Realizar VB Final Abogado" (Abogado)
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv95_realizar_ep_registradas.sql`

### 1.1 Tabla

```sql
-- BBV-95 — Realizar EP Registradas (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_ep_registradas CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_ep_registradas (
    id                          BIGSERIAL PRIMARY KEY,
    id_expediente               BIGINT NOT NULL,
    id_actividad                VARCHAR(100),
    finalizacion                DATE,
    causal                      VARCHAR(200),
    fecha_finalizacion          DATE,
    tipologias_garantias        TEXT,
    confirmacion_ep_registrada  BOOLEAN NOT NULL DEFAULT FALSE,
    observaciones               VARCHAR(500),
    is_active                   BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                  BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                  INTEGER NOT NULL,
    created_date                TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                 INTEGER,
    modified_date               TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_ep_registradas_expediente
    ON public.realizar_ep_registradas (id_expediente)
    WHERE is_active = true AND row_status = true;

-- Stored Procedure — Consulta por expediente
CREATE OR REPLACE FUNCTION public.usp_select_realizar_ep_registradas_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.realizar_ep_registradas
LANGUAGE sql
STABLE
AS $$
    SELECT actividad.*
    FROM public.realizar_ep_registradas actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE
      AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC
    LIMIT 1;
$$;

-- Permisos
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_ep_registradas TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_ep_registradas_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_ep_registradas_bbva(BIGINT) TO multibanca;
```

### 1.2 Insert / Update

> **NO se necesita SP para insert/update.** Se usa EF Core con `Create()` y `Update()` del
> `MultibancaGenericApplication` (mismo patrón que BBV-92, BBV-93).

### 1.3 Registro en cat_actividades_ws (bandeja)

```sql
-- Realizar EP Registradas (ya debería existir del script de BBV-93)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar EP Registradas', 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_ep_registradas', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS');

-- Destino: Realizar VB Final Abogado
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar VB Final Abogado', 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_vb_final_abogado', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO');
```

### 1.4 No se necesitan catálogos nuevos

> La paramétrica L53 (Tipologías Garantías) fue removida del alcance.
> No se necesita endpoint `/controles`.

---

## 2. Backend — Archivos a crear

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `Data.Repository.Interfaces/Entities/Multibanca/BBVA/Escrituracion/realizar_ep_registradas_entity.cs` | Clase con todas las columnas |
| 2 | Entity Config | `Data.Repository.Implementations/EntityConfig/Multibanca/BBVA/Escrituracion/realizar_ep_registradas_entity_config.cs` | Fluent API → `ToTable("realizar_ep_registradas")` |
| 3 | DbContext | `MultibancaDBContext.cs` | Agregar `DbSet<realizar_ep_registradas_entity>` + config |
| 4 | Repo Interface | `Data.Repository.Interfaces/Repositories/Multibanca/BBVA/Escrituracion/IRealizarEPRegistradasRepository.cs` | `GetByExpediente` |
| 5 | Repo Impl | `Data.Repository.Implementations/Repositories/Multibanca/BBVA/Escrituracion/RealizarEPRegistradasRepository.cs` | Query con EF |
| 6 | Domain Model | `Multibanca.Domain.Models/Multibanca/BBVA/Escrituracion/realizar_ep_registradas.cs` | Modelo de dominio |
| 7 | App Interface | `Multibanca.Application.Interfaces/Multibanca/BBVA/Escrituracion/IRealizarEPRegistradasApplication.cs` | Métodos: GetByExpediente, GetControles, Avanzar |
| 8 | App Impl | `Multibanca.Application.Implementations/Multibanca/BBVA/Escrituracion/RealizarEPRegistradasApplication.cs` | Lógica lineal + bitácora |
| 9 | Controller | `Multibanca.Backend.Api/Controllers/Multibanca/BBVA/Escrituracion/RealizarEPRegistradasController.cs` | Ruta: `api/realizar-ep-registradas` |
| 10 | Constants | `Multibanca.Common/Constants.cs` | Actividad + transición + catálogo |
| 11 | IoC | `IoCRegisterMultibanca.cs` | Registrar repository + application |
| 12 | AutoMapper | `AutoMapperProfileMultibanca.cs` | `CreateMap<entity, domain>().ReverseMap()` |

---

## 3. Backend — Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/realizar-ep-registradas/GetByIdExpediente/{id_expediente}` | Consulta registro + datos heredados |
| POST | `/api/realizar-ep-registradas/Save` | Crea o actualiza |
| POST | `/api/realizar-ep-registradas/avanzar/{id_expediente}` | Valida + transiciona workflow + bitácora |

---

## 4. Backend — Lógica de Avanzar

### 4.1 Constantes (Constants.cs)

```csharp
// En ActividadesBBVA (ya existe de BBV-93)
public const string EscrituracionRealizarEPRegistradas = "BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS";
public const string EscrituracionRealizarVBFinalAbogado = "BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO";

// En TransicionesBBVA
public const string EPRegistradasVBFinalAbogado = "BBVA_ESCRITURACION_TR_EP_REGISTRADAS_VB_FINAL_ABOGADO";
```

### 4.2 Dependencias del Application (inyección)

```csharp
public RealizarEPRegistradasApplication(
    MultibancaDBContext multibancaDBContext,
    IRealizarEPRegistradasRepository repository,
    IMapper mapper,
    ICommonApplication commonApplication,
    IWorkflowApplication workflowApplication,
    IBitacoraApplication bitacoraApplication,
    IRealizarRecepcionBoletaRepository recepcionBoletaRepository,
    IFirmarEscrituraClienteRepository firmarEscrituraClienteRepository,
    IValidarInformacionRepository validarInformacionRepository)
    : base(multibancaDBContext, repository, mapper)
```

### 4.3 Flujo de Avanzar (CA05 — Lineal)

```
1. Leer registro del expediente
2. Validar campos obligatorios (CA03, CA04, CA06, CA07):
     - finalizacion (fecha)
     - causal
     - fecha_finalizacion (fecha)
     - tipologias_garantias (al menos una selección)
     - confirmacion_ep_registrada == true (CA04/CA07)
3. Obtener transiciones y folio
4. Avanzar SIEMPRE hacia "Realizar VB Final Abogado" (Abogado) — CA05/CA08
5. Registrar bitácora
```

### 4.4 Registro en Bitácora

```csharp
var observacionesBitacora = $"Avance de Realizar EP Registradas. " +
    $"Finalización: {formulario.finalizacion:yyyy-MM-dd}. " +
    $"Causal: {formulario.causal}. " +
    $"Fecha Finalización: {formulario.fecha_finalizacion:yyyy-MM-dd}. " +
    $"Confirmación EP: Sí. " +
    $"Destino: [Realizar VB Final Abogado].";

if (!string.IsNullOrWhiteSpace(formulario.observaciones))
    observacionesBitacora += $" Observaciones: {formulario.observaciones}";
```

### 4.5 Validación de campos obligatorios

```csharp
private static void ValidarCamposObligatorios(realizar_ep_registradas formulario)
{
    var camposFaltantes = new List<string>();

    if (!formulario.finalizacion.HasValue)
        camposFaltantes.Add("Finalización");

    if (string.IsNullOrWhiteSpace(formulario.causal))
        camposFaltantes.Add("Causal");

    if (!formulario.fecha_finalizacion.HasValue)
        camposFaltantes.Add("Fecha Finalización");

    if (string.IsNullOrWhiteSpace(formulario.tipologias_garantias))
        camposFaltantes.Add("Tipologías Garantías");

    if (!formulario.confirmacion_ep_registrada)
        camposFaltantes.Add("Confirmación de EP Registrada (debe estar marcada)");

    if (camposFaltantes.Count > 0)
    {
        throw new InvalidOperationException(
            $"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
    }
}
```

---

## 5. Frontend — Archivos a crear

| # | Archivo | Descripción |
|---|---|---|
| 1 | `features/actividades/realizar_ep_registradas/models/realizar_ep_registradas.ts` | Interface + factory EMPTY |
| 2 | `features/actividades/realizar_ep_registradas/models/controles.ts` | Interface controles (tipologias_garantias) |
| 3 | `features/actividades/realizar_ep_registradas/api/realizarEPRegistradasService.ts` | 4 llamadas HTTP |
| 4 | `features/actividades/realizar_ep_registradas/hooks/useRealizarEPRegistradas.ts` | useQuery consulta |
| 5 | `features/actividades/realizar_ep_registradas/hooks/useControlesEPRegistradas.ts` | useQuery catálogos |
| 6 | `features/actividades/realizar_ep_registradas/hooks/useUpsertRealizarEPRegistradas.ts` | useMutation guardar |
| 7 | `features/actividades/realizar_ep_registradas/hooks/useAvanzarRealizarEPRegistradas.ts` | useMutation avanzar |
| 8 | `features/actividades/realizar_ep_registradas/components/DatosHeredadosSection.tsx` | Solo lectura (CA02) |
| 9 | `features/actividades/realizar_ep_registradas/components/EPRegistradasSection.tsx` | Campos editables |
| 10 | `features/actividades/realizar_ep_registradas/pages/realizar_ep_registradas_page.tsx` | Página principal |
| 11 | `routes/Routes.tsx` | Ruta: `realizar_ep_registradas/:id_expediente` |

### 5.1 Service — Llamadas HTTP

```typescript
import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarEPRegistradas, GetByExpedienteResponse } from '../models/realizar_ep_registradas';
import type { ControlesEPRegistradas } from '../models/controles';

const PATH_URL = '/api/realizar-ep-registradas';

export const realizarEPRegistradasService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<GetByExpedienteResponse | null>> {
    const response = await axiosClient.get<ApiResponse<GetByExpedienteResponse | null>>(
      `${PATH_URL}/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesEPRegistradas>> {
    const response = await axiosClient.get<ApiResponse<ControlesEPRegistradas>>(
      `${PATH_URL}/controles`,
    );
    return response.data;
  },

  async guardar(payload: RealizarEPRegistradas): Promise<ApiResponse<RealizarEPRegistradas>> {
    const response = await axiosClient.post<ApiResponse<RealizarEPRegistradas>>(
      `${PATH_URL}/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(
      `${PATH_URL}/avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
```

---

## 6. Frontend — Estructura de la página

### 6.1 Funciones Transversales

| Función transversal | ¿Aplica? | Notas |
|---|---|---|
| Expediente Digital | ✅ Sí | Visualización de documentos |
| Trazabilidad / Bitácora | ✅ Sí | Historial de acciones |
| Carta de Aprobación | ❌ No | |
| Registro de Contacto | ❌ No | |

### 6.2 Wireframe (basado en HTML de referencia)

```
┌──────────────────────────────────────────────────────┐
│ Título: "Realizar EP Registradas"                    │
├──────────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente               │
│   └─ EncabezadoActividad (solo lectura)              │
├──────────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales                  │
│   └─ Expediente Digital + Trazabilidad               │
├──────────────────────────────────────────────────────┤
│ Acordeón 3: Realizar EP Registradas                  │
│                                                      │
│   ┌─ DatosHeredadosSection (solo lectura) ─────────┐│
│   │  Datos Cliente: Tipo Doc, Nro, Nombre, Crédito ││
│   │  Datos Notaría: Ciudad, Nro, Escritura         ││
│   │  Datos Boleta: Radicado, Fecha, Tipo, Oficina, ││
│   │               Matrícula                         ││
│   └────────────────────────────────────────────────┘│
│                                                      │
│   ┌─ EPRegistradasSection (editable) ──────────────┐│
│   │                                                ││
│   │  Finalización: Calendar *                      ││
│   │  Causal: InputText *                           ││
│   │  Fecha Finalización: Calendar *                ││
│   │  Tipologías Garantías: MultiSelect (L53) *     ││
│   │  ☑ Confirmación de EP Registrada (toggle) *    ││
│   │  Observaciones: textarea (opcional)            ││
│   │                                                ││
│   └────────────────────────────────────────────────┘│
│                                                      │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]     │
└──────────────────────────────────────────────────────┘
```

### 6.3 Flujo de estados UI

```
Estado inicial (carga desde backend):
  ┌─ Si tiene registro (id > 0): campos bloqueados, canAdvance=false
  └─ Si no tiene registro: campos editables, canAdvance=false

[Editar] → isDisabled=false, canAdvance=false
[Guardar] → POST Save → si éxito: isDisabled=true, canAdvance=true
[Avanzar] → solo habilitado si canAdvance=true → valida → POST avanzar → navegar a bandeja
[Salir] → navigate('/home/bandeja')
```

---

## 7. Frontend — Validación (antes de Avanzar)

```typescript
const validateAvanzar = (): string[] => {
  const missing: string[] = [];

  if (!form.finalizacion) missing.push('Finalización');
  if (!form.causal?.trim()) missing.push('Causal');
  if (!form.fecha_finalizacion) missing.push('Fecha Finalización');
  if (!form.tipologias_garantias?.trim()) missing.push('Tipologías Garantías');
  if (!form.confirmacion_ep_registrada) missing.push('Confirmación de EP Registrada');

  return missing;
};
```

---

## 8. Modelo TypeScript

```typescript
import type { Auditoria } from '@/models/Auditoria';

export interface RealizarEPRegistradas extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  finalizacion: string | null;
  causal: string | null;
  fecha_finalizacion: string | null;
  tipologias_garantias: string | null;   // JSON array o string separado por comas
  confirmacion_ep_registrada: boolean;
  observaciones: string | null;
}

export interface DatosHeredadosEPRegistradas {
  // Datos Cliente
  tipo_documento?: string | null;
  numero_documento?: string | null;
  nombre_completo?: string | null;
  tipo_credito?: string | null;
  // Datos Notaría
  ciudad_notaria?: string | null;
  numero_notaria?: number | null;
  numero_escritura?: string | null;
  // Datos Recepción Boleta
  numero_boleta?: string | null;
  fecha_boleta?: string | null;
  tipo_boleta?: string | null;
  oficina_registro?: string | null;
  numero_matricula?: string | null;
}

export interface GetByExpedienteResponse {
  formulario: RealizarEPRegistradas;
  datos_heredados: DatosHeredadosEPRegistradas;
}

export const EMPTY_REALIZAR_EP_REGISTRADAS = (
  id_expediente: number,
): RealizarEPRegistradas => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS',
  finalizacion: null,
  causal: null,
  fecha_finalizacion: null,
  tipologias_garantias: null,
  confirmacion_ep_registrada: false,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
```

### 8.1 Interface de Controles

```typescript
import type { ControlBaseDTO } from '@/core/api/models/ControlBaseDTO';

export interface ControlesEPRegistradas {
  tipologias_garantias: ControlBaseDTO[];
}

export const EMPTY_CONTROLES_EP_REGISTRADAS: ControlesEPRegistradas = {
  tipologias_garantias: [],
};
```

---

## 9. Lo que NO se crea

- ❌ No se usa EF Migrations (script SQL directo)
- ❌ No hay devoluciones ni desvíos (CA08 — flujo lineal)
- ❌ No hay campo "¿Aplica Excepción?" (no aplica para esta actividad)
- ❌ No hay integración VUR (los campos VUR vienen precargados de la actividad anterior como datos heredados)
- ❌ No se necesita Registro de Contacto ni Carta de Aprobación

---

## 10. Orden de ejecución

1. Ejecutar script SQL contra `BBVA_LEGALIZACION` (tabla + catálogo L53 + cat_actividades_ws)
2. Crear Entity + EntityConfig + agregar al DbContext
3. Crear Repository (interface + impl)
4. Crear Domain Model + registrar AutoMapper
5. Crear Application (interface + impl con lógica lineal)
6. Crear Controller
7. Registrar en IoC + agregar constantes en Constants.cs
8. Frontend: model → controles → service → hooks → components → page
9. Agregar ruta en Routes.tsx
10. Probar flujo completo: carga → guardar → avanzar

---

## 11. Datos heredados (solo lectura) — CA02

| Bloque | Campo | Fuente | Tabla |
|---|---|---|---|
| Datos Cliente | Tipo Documento | `validar_informacion_bbva` | tipo_id_t1 (resolver vs catálogo) |
| Datos Cliente | Nro Documento | `validar_informacion_bbva` | numero_id_t1 |
| Datos Cliente | Nombre Completo | `validar_informacion_bbva` | nombre_completo_t1 |
| Datos Cliente | Tipo Crédito | `validar_informacion_bbva` | tipo_credito |
| Datos Notaría | Ciudad Notaría | `firmar_escritura_cliente` | ciudad_notaria |
| Datos Notaría | Nro Notaría | `firmar_escritura_cliente` | numero_notaria |
| Datos Notaría | Nro Escritura | `firmar_escritura_cliente` | numero_escritura |
| Datos Boleta | Radicado | `realizar_recepcion_boleta` | numero_boleta |
| Datos Boleta | Fecha Ingreso | `realizar_recepcion_boleta` | fecha_boleta |
| Datos Boleta | Tipo Boleta | `realizar_recepcion_boleta` | tipo_boleta |
| Datos Boleta | Oficina Registro | `realizar_recepcion_boleta` | oficina_registro |
| Datos Boleta | Matrícula | `realizar_recepcion_boleta` | numero_matricula |

> Reutilizar el patrón de BBV-93: consultar `validarInformacionRepository`, `firmarEscrituraClienteRepository`
> y agregar `realizarRecepcionBoletaRepository` para los datos de la boleta.

---

## 12. Script de Workflow (DBWFBBVA)

```sql
-- Actividad: Realizar EP Registradas (ya debería existir de BBV-93)
-- Verificar: SELECT * FROM xpdl_activities WHERE activity_id = 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS';

-- Actividad destino: Realizar VB Final Abogado
INSERT INTO public.xpdl_activities (
    activity_id, workflow_process_id, display_name, name,
    task_type, task_form_type, task_form_uri, performer, sub_flow_id
)
SELECT
    'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO',
    'WP_BBVA_CONTACTO_CLIENTE',
    'Realizar VB Final Abogado',
    'Realizar VB Final Abogado',
    'TaskUser', 'UserDefined', 'realizar_vb_final_abogado', 'ABOGADO', NULL
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_activities
    WHERE activity_id = 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO'
);

-- Transición: EP Registradas → VB Final Abogado (única, lineal)
INSERT INTO public.xpdl_transitions (
    transition_id, name, from_activity, to_activity, condition, workflow_process_id
)
SELECT
    'BBVA_ESCRITURACION_TR_EP_REGISTRADAS_VB_FINAL_ABOGADO',
    'BBVA_ESCRITURACION_TR_EP_REGISTRADAS_VB_FINAL_ABOGADO',
    'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS',
    'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO',
    'Otherwise',
    'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_transitions
    WHERE transition_id = 'BBVA_ESCRITURACION_TR_EP_REGISTRADAS_VB_FINAL_ABOGADO'
);
```

---

## 13. Diferencias clave con BBV-93

| Aspecto | BBV-93 (Recepción Boleta) | BBV-95 (EP Registradas) |
|---|---|---|
| Rol | Analista de Vivienda | Analista de Vivienda |
| Campos editables | 8 campos + checkbox | 4 campos + checkbox + multiselect |
| Integración externa | Bot VUR (RPA) | Ninguna (datos VUR heredados de BBV-93) |
| Enrutamiento | 1 siempre + 1 paralelo condicional | 1 único destino (lineal) — CA08 |
| Destino | EP Registradas + Excepción | VB Final Abogado |
| Catálogos | L44, L45 | L53 (Tipologías Garantías) |
| Complejidad | Alta (VUR + excepción) | Baja (confirmación + lineal) |
| Campo multiselect | No | Sí: Tipologías Garantías (CA06) |
| Checkbox bloqueante | Boleta Recibida | Confirmación de EP Registrada (CA04/CA07) |

---

## 14. Notas de implementación

1. **Tipologías Garantías (CA06):** Usar `MultiSelect` de PrimeReact para permitir múltiples selecciones. Almacenar como string JSON (`["TGAR-1","TGAR-3"]`) en la columna `tipologias_garantias` (tipo TEXT).

2. **Confirmación de EP Registrada (CA04/CA07):** Toggle/Checkbox que bloquea el avance si no está marcado. Usar `InputSwitch` como en BBV-93 para mantener consistencia visual.

3. **Sin devoluciones (CA08):** Esta actividad es estrictamente lineal. No hay concepto favorable/desfavorable ni escalamiento comercial.

4. **Datos heredados ampliados:** Incluye datos de 3 actividades previas:
   - `validar_informacion_bbva` (cliente)
   - `firmar_escritura_cliente` (notaría)
   - `realizar_recepcion_boleta` (boleta/VUR)

5. **Reutilización de código:**
   - Patrón Application/Repository/Controller: idéntico a BBV-92/BBV-93
   - Datos heredados: mismo patrón que BBV-93 + agrega consulta a `realizar_recepcion_boleta`
   - Flujo de Avanzar: más simple que BBV-92/BBV-93 (una sola transición, sin condicionales)
