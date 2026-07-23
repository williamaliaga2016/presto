# BBV-92 — Receta de Implementación: Realizar Entrega EP Firmada

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Analista de Vivienda.

---

## Flujo

```
Firmar Rep. Legal (concepto "Escritura firmada Conforme")
    → Realizar Entrega EP Firmada ← ESTA HU
        → Siempre: "Realizar Recepción Boleta" (Analista de Vivienda)
        → Si aplica excepción: también "Realizar Excepción Desembolso" (Comercial) en paralelo
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv92_realizar_entrega_ep_firmada.sql`

### 1.1 Tabla

```sql
-- BBV-92 — Realizar Entrega EP Firmada (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_entrega_ep_firmada CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_entrega_ep_firmada (
    id                  BIGSERIAL PRIMARY KEY,
    id_expediente       BIGINT NOT NULL,
    id_actividad        VARCHAR(100),
    entregado_a         VARCHAR(200),
    aplica_excepcion    VARCHAR(2),
    observaciones       VARCHAR(500),
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    row_status          BOOLEAN NOT NULL DEFAULT TRUE,
    created_by          INTEGER NOT NULL,
    created_date        TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by         INTEGER,
    modified_date       TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_entrega_ep_firmada_expediente
    ON public.realizar_entrega_ep_firmada (id_expediente)
    WHERE is_active = true AND row_status = true;

-- Stored Procedure — Consulta por expediente
CREATE OR REPLACE FUNCTION public.usp_select_realizar_entrega_ep_firmada_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.realizar_entrega_ep_firmada
LANGUAGE sql
STABLE
AS $$
    SELECT actividad.*
    FROM public.realizar_entrega_ep_firmada actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE
      AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC
    LIMIT 1;
$$;

-- Permisos
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_entrega_ep_firmada TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_entrega_ep_firmada_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_entrega_ep_firmada_bbva(BIGINT) TO multibanca;
```

### 1.2 Insert / Update

> **NO se necesita SP para insert/update.** Se usa EF Core con `Create()` y `Update()` del
> `MultibancaGenericApplication` (mismo patrón que BBV-86 y BBV-91).

### 1.3 Registro en cat_actividades_ws (bandeja)

> **OBLIGATORIO** para que la actividad aparezca en la bandeja de trabajo.

```sql
-- Realizar Entrega EP Firmada (esta HU)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Entrega EP Firmada', 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA', 'WP_BBVA_ESCRITURACION', 'Escrituración', 1, 'actividad', 'realizar_entrega_ep_firmada', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA');

-- Destino: Realizar Recepción Boleta
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Recepción Boleta', 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA', 'WP_BBVA_ESCRITURACION', 'Escrituración', 1, 'actividad', 'realizar_recepcion_boleta', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA');

-- Destino: Realizar Excepción Desembolso
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Excepción Desembolso', 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'WP_BBVA_ESCRITURACION', 'Escrituración', 1, 'actividad', 'realizar_excepcion_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO');
```

### 1.4 No se necesitan catálogos nuevos

> El campo `¿Aplica Excepción?` es calculado automáticamente en backend (no es un dropdown).
> Se almacena como "SI" / "NO" basado en la lógica de CA02.

---

## 2. Backend — Archivos a crear

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `Data.Repository.Interfaces/Entities/Multibanca/BBVA/Escrituracion/realizar_entrega_ep_firmada_entity.cs` | Clase con todas las columnas |
| 2 | Entity Config | `Data.Repository.Implementations/EntityConfig/Multibanca/BBVA/Escrituracion/realizar_entrega_ep_firmada_entity_config.cs` | Fluent API → `ToTable("realizar_entrega_ep_firmada")` |
| 3 | DbContext | `MultibancaDBContext.cs` | Agregar `DbSet<realizar_entrega_ep_firmada_entity>` + config |
| 4 | Repo Interface | `Data.Repository.Interfaces/Repositories/Multibanca/BBVA/Escrituracion/IRealizarEntregaEpFirmadaRepository.cs` | `GetByExpediente` |
| 5 | Repo Impl | `Data.Repository.Implementations/Repositories/Multibanca/BBVA/Escrituracion/RealizarEntregaEpFirmadaRepository.cs` | Query con EF |
| 6 | Domain Model | `Multibanca.Domain.Models/Multibanca/BBVA/Escrituracion/realizar_entrega_ep_firmada.cs` | Modelo de dominio |
| 7 | App Interface | `Multibanca.Application.Interfaces/Multibanca/BBVA/Escrituracion/IRealizarEntregaEpFirmadaApplication.cs` | Métodos: GetByExpediente, GetControles, Avanzar |
| 8 | App Impl | `Multibanca.Application.Implementations/Multibanca/BBVA/Escrituracion/RealizarEntregaEpFirmadaApplication.cs` | Lógica de excepción + enrutamiento |
| 9 | Controller | `Multibanca.Backend.Api/Controllers/Multibanca/BBVA/Escrituracion/RealizarEntregaEpFirmadaController.cs` | Ruta: `api/realizar-entrega-ep-firmada` |
| 10 | Constants | `Multibanca.Common/Constants.cs` | Actividad + transiciones |
| 11 | IoC | `IoCRegisterMultibanca.cs` | Registrar repository + application |
| 12 | AutoMapper | `AutoMapperProfileMultibanca.cs` | `CreateMap<entity, domain>().ReverseMap()` |

---

## 3. Backend — Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/realizar-entrega-ep-firmada/GetByIdExpediente/{id_expediente}` | Consulta registro + calcula aplica_excepcion |
| POST | `/api/realizar-entrega-ep-firmada/Save` | Crea o actualiza |
| POST | `/api/realizar-entrega-ep-firmada/avanzar/{id_expediente}` | Valida + transiciona workflow + bitácora |

> **Nota:** No se necesita endpoint `/controles` porque no hay catálogos dependientes.
> El campo `aplica_excepcion` se calcula en backend al hacer `GetByExpediente`.

### 3.1 Patrón de manejo de errores (HandleException)

Mismo patrón que BBV-91 con `campos_faltantes`.

---

## 4. Backend — Lógica de Avanzar

### 4.1 Constantes (Constants.cs)

```csharp
// En ActividadesBBVA
public const string EscrituracionRealizarEntregaEpFirmada = "BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA";

// En TransicionesBBVA
public const string EntregaEpFirmadaRecepcionBoleta = "BBVA_ESCRITURACION_TR_ENTREGA_EP_RECEPCION_BOLETA";
public const string EntregaEpFirmadaExcepcionDesembolso = "BBVA_ESCRITURACION_TR_ENTREGA_EP_EXCEPCION_DESEMBOLSO";
```

### 4.2 Dependencias del Application (inyección)

```csharp
public RealizarEntregaEpFirmadaApplication(
    MultibancaDBContext multibancaDBContext,
    IRealizarEntregaEpFirmadaRepository repository,
    IMapper mapper,
    ICommonApplication commonApplication,           // → GetCatalogoByType (tipos de crédito)
    IWorkflowApplication workflowApplication,       // → GetTransitions, CapturarDatosFolio, AvanzarActividad
    IBitacoraApplication bitacoraApplication)       // → Create
    : base(multibancaDBContext, repository, mapper)
```

### 4.3 Lógica de cálculo de `aplica_excepcion` (CA02/CA05)

```csharp
// Obtener tipo_credito desde validar_informacion_bbva (mismo patrón que BBV-86)
var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
string? tipoCredito = validarInfo?.tipo_credito;

string aplica = CalcularAplicaExcepcion(tipoCredito);
```

```csharp
private static readonly string[] TiposExcepcionDesembolso = new[]
{
    "CONSTRUCTOR_INDIVIDUAL",
    "HIPOTECARIO_CXI",
    "HIPOTECARIO_USADO",
    "LEASING_NUEVO",
    "LEASING_USADO",
    "LEASING_CXI",
    "REMODELACION_AMPLIAR_HIPOTECAR"
};

private static string CalcularAplicaExcepcion(string? tipoCredito)
{
    if (string.IsNullOrWhiteSpace(tipoCredito))
        return "NO";

    return TiposExcepcionDesembolso.Contains(tipoCredito, StringComparer.OrdinalIgnoreCase)
        ? "SI"
        : "NO";
}
```

> **Fuente del tipo de crédito:** tabla `validar_informacion_bbva`, campo `tipo_credito`.
> Se accede vía `IValidarInformacionRepository.GetByExpediente(idExpediente)`.
> Es el mismo patrón que usa `FirmarEscrituraClienteApplication.GetByExpediente`.
>
> **TODO:** La HU menciona también validar la "paramétrica de constructoras" con
> `tipo_desembolso = 'ESCRITURA'`. Si esa tabla existe, agregar la validación.
> Por ahora, la lógica se basa solo en el tipo de crédito.

### 4.4 Flujo de Avanzar

```
1. Leer registro del expediente
2. Validar campos obligatorios:
     - entregado_a obligatorio siempre
3. Obtener transiciones y folio
4. SIEMPRE avanzar hacia "Realizar Recepción Boleta" (Analista de Vivienda)
5. SI aplica_excepcion == "SI":
     - Avanzar EN PARALELO hacia "Realizar Excepción Desembolso" (Comercial)
6. Registrar bitácora
```

### 4.5 Registro en Bitácora

```csharp
var observacionesBitacora = $"Avance de Realizar Entrega EP Firmada. " +
    $"Entregado a: {formulario.entregado_a}. " +
    $"¿Aplica Excepción?: {formulario.aplica_excepcion}. " +
    $"Destino principal: [Realizar Recepción Boleta].";

if (formulario.aplica_excepcion == "SI")
    observacionesBitacora += " Destino paralelo: [Realizar Excepción Desembolso].";

if (!string.IsNullOrWhiteSpace(formulario.observaciones))
    observacionesBitacora += $" Observaciones: {formulario.observaciones}";
```

### 4.6 Validación de campos obligatorios

```csharp
private static void ValidarCamposObligatorios(realizar_entrega_ep_firmada formulario)
{
    var camposFaltantes = new List<string>();

    if (string.IsNullOrWhiteSpace(formulario.entregado_a))
        camposFaltantes.Add("Entregado a");

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
| 1 | `features/actividades/realizar_entrega_ep_firmada/models/realizar_entrega_ep_firmada.ts` | Interface + factory EMPTY |
| 2 | `features/actividades/realizar_entrega_ep_firmada/api/realizarEntregaEpFirmadaService.ts` | 3 llamadas HTTP |
| 3 | `features/actividades/realizar_entrega_ep_firmada/hooks/useRealizarEntregaEpFirmada.ts` | useQuery consulta |
| 4 | `features/actividades/realizar_entrega_ep_firmada/hooks/useUpsertRealizarEntregaEpFirmada.ts` | useMutation guardar |
| 5 | `features/actividades/realizar_entrega_ep_firmada/hooks/useAvanzarRealizarEntregaEpFirmada.ts` | useMutation avanzar |
| 6 | `features/actividades/realizar_entrega_ep_firmada/components/DatosHeredadosSection.tsx` | Solo lectura (datos Firmar Rep. Legal) |
| 7 | `features/actividades/realizar_entrega_ep_firmada/components/EntregaEpSection.tsx` | Campos editables |
| 8 | `features/actividades/realizar_entrega_ep_firmada/pages/realizar_entrega_ep_firmada_page.tsx` | Página principal |
| 9 | `routes/Routes.tsx` | Ruta: `realizar_entrega_ep_firmada/:id_expediente` |

### 5.1 Service — Llamadas HTTP

```typescript
import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarEntregaEpFirmada } from '../models/realizar_entrega_ep_firmada';

const PATH_URL = '/api/realizar-entrega-ep-firmada';

export const realizarEntregaEpFirmadaService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<RealizarEntregaEpFirmada | null>> {
    const response = await axiosClient.get<ApiResponse<RealizarEntregaEpFirmada | null>>(
      `${PATH_URL}/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(payload: RealizarEntregaEpFirmada): Promise<ApiResponse<RealizarEntregaEpFirmada>> {
    const response = await axiosClient.post<ApiResponse<RealizarEntregaEpFirmada>>(
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

### 5.2 Hooks

Mismo patrón que BBV-91:
- `useRealizarEntregaEpFirmada` → `useQuery` con `queryKey: ['realizar_entrega_ep_firmada', id_expediente]`
- `useUpsertRealizarEntregaEpFirmada` → `useMutation` + `invalidateQueries`
- `useAvanzarRealizarEntregaEpFirmada` → `useMutation`

### 5.3 Componentes

| Componente | Props | Descripción |
|---|---|---|
| `DatosHeredadosSection` | `datosHeredados` | Muestra concepto firma del Rep. Legal (readonly) |
| `EntregaEpSection` | `form`, `isDisabled`, `updateField` | Campo "Entregado a" + "¿Aplica Excepción?" (readonly) + Observaciones |

---

## 6. Frontend — Estructura de la página

### 6.1 Funciones Transversales

| Función transversal | ¿Aplica? | Notas |
|---|---|---|
| Expediente Digital | ✅ Sí | Carga/visualización de documentos |
| Trazabilidad / Bitácora | ✅ Sí | Historial de acciones |
| Carta de Aprobación | ❌ No | No aplica |
| Registro de Contacto | ❌ No | No aplica |

### 6.2 Wireframe

```
┌──────────────────────────────────────────────────┐
│ Título: "Realizar Entrega EP Firmada"            │
├──────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente           │
│   └─ EncabezadoActividad (solo lectura)          │
├──────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales              │
│   └─ Expediente Digital + Trazabilidad           │
├──────────────────────────────────────────────────┤
│ Acordeón 3: Realizar Entrega EP Firmada          │
│                                                  │
│   ┌─ DatosHeredadosSection (solo lectura) ─────┐│
│   │  Concepto Firma Rep. Legal: "Firmada..."   ││
│   │  Datos Notaría heredados                    ││
│   └────────────────────────────────────────────┘│
│                                                  │
│   ┌─ EntregaEpSection (editable) ─────────────┐│
│   │                                            ││
│   │  Entregado a: InputText *                  ││
│   │  ¿Aplica Excepción?: [SI/NO] (readonly)   ││
│   │  Observaciones: textarea (opcional)        ││
│   │                                            ││
│   └────────────────────────────────────────────┘│
│                                                  │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]  │
└──────────────────────────────────────────────────┘
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

  if (!form.entregado_a?.trim())
    missing.push('Entregado a');

  return missing;
};
```

---

## 8. Modelo TypeScript

```typescript
import type { Auditoria } from '@/models/Auditoria';

export interface RealizarEntregaEpFirmada extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  entregado_a: string | null;
  aplica_excepcion: string | null;   // "SI" | "NO" — readonly, calculado en backend
  observaciones: string | null;
}

export const EMPTY_REALIZAR_ENTREGA_EP_FIRMADA = (
  id_expediente: number,
): RealizarEntregaEpFirmada => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA',
  entregado_a: null,
  aplica_excepcion: null,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
```

---

## 9. Lo que NO se crea

- ❌ No se crean catálogos nuevos (el campo excepción es calculado, no es dropdown)
- ❌ No se necesita endpoint `/controles`
- ❌ No se necesita Registro de Contacto
- ❌ No se necesita Carta de Aprobación
- ❌ No se necesita `IActividadesApplication` (la lógica de paralelo usa solo tipo de crédito)
- ❌ No se usa EF migrations (se usa script SQL directo)

---

## 10. Orden de ejecución

1. Ejecutar script SQL contra `BBVA_LEGALIZACION`
2. Crear Entity + EntityConfig + agregar al DbContext
3. Crear Repository (interface + impl)
4. Crear Domain Model + registrar AutoMapper
5. Crear Application (interface + impl con lógica de excepción)
6. Crear Controller
7. Registrar en IoC + agregar constantes en Constants.cs
8. Frontend: model → service → hooks → components → page
9. Agregar ruta en Routes.tsx
10. Probar flujo completo: nuevo → guardar → editar → avanzar

---

## 11. Datos heredados (solo lectura) — CA03

| Bloque | Fuente | Implementación |
|---|---|---|
| Concepto Firma Rep. Legal | tabla `firmar_rep_legal` | ✅ Leer por id_expediente |
| Datos Notaría | tabla `firmar_escritura_cliente` | ✅ Ya existe (BBV-86) |
| Nro. Escritura / Fecha | tabla `firmar_escritura_cliente` | ✅ Ya existe |

> El `GetByExpediente` puede devolver un objeto `datos_heredados` con la info
> de `firmar_rep_legal` y `firmar_escritura_cliente`, o se puede hacer en un
> endpoint separado.

---

## 12. Script de Workflow (DBWFBBVA)

```sql
-- Actividad: Realizar Entrega EP Firmada (ya existe del script anterior)
-- Verificar con: SELECT * FROM xpdl_activities WHERE activity_id = 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA';

-- Actividades destino (placeholders)
INSERT INTO public.xpdl_activities (
    activity_id, workflow_process_id, display_name, name,
    task_type, task_form_type, task_form_uri, performer, sub_flow_id
)
SELECT
    'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA',
    'WP_BBVA_ESCRITURACION',
    'Realizar Recepción Boleta',
    'Realizar Recepción Boleta',
    'TaskUser', 'UserDefined', 'realizar_recepcion_boleta', 'ADMINISTRADOR', NULL
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_activities
    WHERE activity_id = 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA'
);

INSERT INTO public.xpdl_activities (
    activity_id, workflow_process_id, display_name, name,
    task_type, task_form_type, task_form_uri, performer, sub_flow_id
)
SELECT
    'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO',
    'WP_BBVA_ESCRITURACION',
    'Realizar Excepción Desembolso',
    'Realizar Excepción Desembolso',
    'TaskUser', 'UserDefined', 'realizar_excepcion_desembolso', 'ADMINISTRADOR', NULL
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_activities
    WHERE activity_id = 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO'
);

-- Transición 1: Siempre → Recepción Boleta
INSERT INTO public.xpdl_transitions (
    transition_id, name, from_activity, to_activity, condition, workflow_process_id
)
SELECT
    'BBVA_ESCRITURACION_TR_ENTREGA_EP_RECEPCION_BOLETA',
    'BBVA_ESCRITURACION_TR_ENTREGA_EP_RECEPCION_BOLETA',
    'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA',
    'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA',
    'Otherwise',
    'WP_BBVA_ESCRITURACION'
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_transitions
    WHERE transition_id = 'BBVA_ESCRITURACION_TR_ENTREGA_EP_RECEPCION_BOLETA'
);

-- Transición 2: Si aplica excepción → Excepción Desembolso (paralela)
INSERT INTO public.xpdl_transitions (
    transition_id, name, from_activity, to_activity, condition, workflow_process_id
)
SELECT
    'BBVA_ESCRITURACION_TR_ENTREGA_EP_EXCEPCION_DESEMBOLSO',
    'BBVA_ESCRITURACION_TR_ENTREGA_EP_EXCEPCION_DESEMBOLSO',
    'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA',
    'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO',
    'Otherwise',
    'WP_BBVA_ESCRITURACION'
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_transitions
    WHERE transition_id = 'BBVA_ESCRITURACION_TR_ENTREGA_EP_EXCEPCION_DESEMBOLSO'
);
```

---

## 13. Diferencias clave con BBV-91

| Aspecto | BBV-91 (Firmar Rep. Legal) | BBV-92 (Entrega EP Firmada) |
|---|---|---|
| Rol | Representante Legal | Analista de Vivienda |
| Campos editables | Concepto + Tipología + Casuística | Entregado a + Observaciones |
| Campos calculados | Ninguno | `aplica_excepcion` (auto en backend) |
| Enrutamiento | 2 destinos exclusivos | 1 siempre + 1 paralelo condicional |
| Dropdowns dependientes | Sí (L42 → L43) | No |
| Endpoint controles | Sí | No necesario |
| Catálogos nuevos | 3 (L41, L42, L43) | Ninguno |
