# BBV-91 — Receta de Implementación: Firmar Rep. Legal

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Representante Legal.

---

## Flujo

```
Revisar EP Abogado (concepto favorable)
    → Firmar Rep. Legal ← ESTA HU
        → "Escritura firmada Conforme"  → Realizar Entrega EP Firmada
        → "Escritura NO firmada"        → Realizar Devolución EP
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv91_firmar_rep_legal.sql`

### 1.1 Tabla

```sql
-- BBV-91 — Firmar Rep. Legal (Escrituración y Garantías)
-- Script idempotente para crear la tabla firmar_rep_legal

CREATE TABLE IF NOT EXISTS public.firmar_rep_legal (
    id                  BIGSERIAL PRIMARY KEY,
    id_expediente       BIGINT NOT NULL,
    id_actividad        VARCHAR(100),
    concepto_firma      VARCHAR(100),
    tipologia           VARCHAR(200),
    casuistica          VARCHAR(200),
    observaciones       VARCHAR(500),
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    row_status          BOOLEAN NOT NULL DEFAULT TRUE,
    created_by          INTEGER NOT NULL,
    created_date        TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by         INTEGER,
    modified_date       TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_firmar_rep_legal_expediente
    ON public.firmar_rep_legal (id_expediente)
    WHERE is_active = true AND row_status = true;
```

> **Notas sobre alineación con BBV-86:**
> - `BIGSERIAL` genera el `id` automáticamente (auto-increment).
> - `created_by` es `NOT NULL` sin default — el Application siempre lo pasa.
> - `TIMESTAMP WITHOUT TIME ZONE` — explícito, igual que BBV-86.
> - `id_actividad` es `VARCHAR(100)` nullable — el Controller lo asigna antes de guardar.
> - Índice UNIQUE parcial para evitar duplicados activos por expediente.

### 1.2 Stored Procedure — Consulta por expediente

```sql
CREATE OR REPLACE FUNCTION public.usp_select_firmar_rep_legal_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.firmar_rep_legal
LANGUAGE sql
STABLE
AS $$
    SELECT actividad.*
    FROM public.firmar_rep_legal actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE
      AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC
    LIMIT 1;
$$;
```

### 1.3 Registro en cat_actividades_ws (bandeja)

> **OBLIGATORIO** para que la actividad aparezca en la bandeja de trabajo.

```sql
-- Firmar Rep. Legal (esta HU)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Firmar Rep. Legal', 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'WP_BBVA_ESCRITURACION', 'Escrituración', 1, 'actividad', 'firmar_rep_legal', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL');

-- Destino: Realizar Entrega EP Firmada (cuando firma es conforme)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Entrega EP Firmada', 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA', 'WP_BBVA_ESCRITURACION', 'Escrituración', 1, 'actividad', 'realizar_entrega_ep_firmada', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA');

-- Destino: Realizar Devolución EP (cuando escritura NO firmada)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Devolución EP', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'WP_BBVA_ESCRITURACION', 'Escrituración', 1, 'actividad', 'realizar_devolucion_ep', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP');
```

### 1.3 Insert / Update

> **NO se necesita SP para insert/update.**
> El proyecto usa EF Core (similar a JPA en Java): el `MultibancaGenericApplication`
> hereda métodos `Create()` y `Update()` que hacen `Repository.Insert(entity)` +
> `_dbContext.SaveChanges()` automáticamente usando el DbSet y el ChangeTracker.
> Los campos de auditoría (`created_by`, `created_date`, `modified_by`, `modified_date`)
> se setean vía reflection en el repositorio genérico.

### 1.4 Catálogos (L41, L42, L43)

> **NO se necesita SP para catálogos.**
> El proyecto ya tiene `CommonApplicationProvider.GetCatalogoByType("L41")` que consulta
> la tabla `catalogo` filtrando por tipo. Devuelve `List<ControlBaseDTO>` con `code`,
> `description` y `parent_code`. Se usa en el endpoint `/controles` del Application.
>
> Para las dependencias L42 → L43, la tabla `catalogo` usa `parent_code` para vincular
> la casuística con su tipología padre. El frontend filtra localmente.

### 1.5 Seed de datos — INSERT en tabla catálogo

```sql
-- ============================================================
-- L41 — Concepto Firma Rep. Legal
-- ============================================================
WITH l41(codigo, descripcion, orden) AS (
    VALUES
        ('CRL-1', 'Escritura firmada Conforme', 1),
        ('CRL-2', 'Escritura NO firmada', 2)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'CONCEPTO_FIRMA_REP_LEGAL', l41.descripcion, l41.codigo, NULL, true, l41.orden
FROM l41
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'CONCEPTO_FIRMA_REP_LEGAL' AND c.valor = l41.codigo
);

-- ============================================================
-- L42 — Tipología Devolución Rep. Legal
-- ============================================================
WITH l42(codigo, descripcion, orden) AS (
    VALUES
        ('TRL-1', 'Escritura', 1),
        ('TRL-2', 'Documentación', 2)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'TIPOLOGIA_REP_LEGAL', l42.descripcion, l42.codigo, NULL, true, l42.orden
FROM l42
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'TIPOLOGIA_REP_LEGAL' AND c.valor = l42.codigo
);

-- ============================================================
-- L43 — Casuística Devolución Rep. Legal (dependiente de L42)
-- ============================================================
WITH l43(codigo, descripcion, padre_codigo, orden) AS (
    VALUES
        ('CasRL-1', 'Corrección abogado', 'TRL-1', 1),
        ('CasRL-2', 'Para corregir por la notaría', 'TRL-1', 2),
        ('CasRL-3', 'Crédito vencido', 'TRL-2', 3),
        ('CasRL-4', 'Estudio de títulos con observaciones sin subsanar', 'TRL-2', 4),
        ('CasRL-5', 'Avalúo con observaciones sin subsanar', 'TRL-2', 5)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'CASUISTICA_REP_LEGAL', l43.descripcion, l43.codigo, padre.id, true, l43.orden
FROM l43
JOIN public.catalogo padre
    ON padre.tipo = 'TIPOLOGIA_REP_LEGAL'
   AND padre.valor = l43.padre_codigo
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'CASUISTICA_REP_LEGAL' AND c.valor = l43.codigo
);
```

> **Nota:** La casuística (L43) usa `id_padre` apuntando al `id` del registro de
> tipología (L42) correspondiente. El frontend recibe esto como `parent_code` y
> filtra localmente.

---

## 2. Backend — Archivos a crear

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `Data.Repository.Interfaces/Entities/Multibanca/BBVA/Escrituracion/firmar_rep_legal_entity.cs` | Clase con todas las columnas de la tabla |
| 2 | Entity Config | `Data.Repository.Implementations/EntityConfig/Multibanca/BBVA/Escrituracion/firmar_rep_legal_entity_config.cs` | Fluent API → `ToTable("firmar_rep_legal")` |
| 3 | DbContext | `MultibancaDBContext.cs` | Agregar `DbSet<firmar_rep_legal_entity>` + config |
| 4 | Repo Interface | `Data.Repository.Interfaces/Repositories/Multibanca/BBVA/Escrituracion/IFirmarRepLegalRepository.cs` | `GetByExpediente`, `Insert`, `Update`, `GetControles` |
| 5 | Repo Impl | `Data.Repository.Implementations/Repositories/Multibanca/BBVA/Escrituracion/FirmarRepLegalRepository.cs` | Llama los SPs vía `DbCommand` |
| 6 | Domain Model | `Multibanca.Domain.Models/Multibanca/BBVA/Escrituracion/FirmarRepLegal.cs` | `firmar_rep_legal : base_auditoria` |
| 7 | DTO | `Multibanca.DTO/Multibanca/BBVA/Escrituracion/FirmarRepLegalControlesDTO.cs` | Listas L41, L42, L43 |
| 8 | App Interface | `Multibanca.Application.Interfaces/Multibanca/BBVA/Escrituracion/IFirmarRepLegalApplication.cs` | Métodos: GetByExpediente, GetControles, Avanzar |
| 9 | App Impl | `Multibanca.Application.Implementations/Multibanca/BBVA/Escrituracion/FirmarRepLegalApplication.cs` | Lógica de enrutamiento + bitácora |
| 10 | Controller | `Multibanca.Backend.Api/Controllers/Multibanca/BBVA/Escrituracion/FirmarRepLegalController.cs` | Ruta: `api/firmar-rep-legal` |
| 11 | Constants | `Multibanca.Common/Constants.cs` | `ActividadesBBVA.FirmarRepLegal` |
| 12 | IoC | `IoCRegisterMultibanca.cs` | Registrar repository + application |
| 13 | AutoMapper | `AutoMapperProfileMultibanca.cs` | `CreateMap<entity, domain>().ReverseMap()` |

---

## 3. Backend — Endpoints

> **Convención:** Seguir la nomenclatura ya establecida en BBV-86 (`GetByIdExpediente`, `Save`).

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/firmar-rep-legal/controles` | Devuelve catálogos L41, L42, L43 |
| GET | `/api/firmar-rep-legal/GetByIdExpediente/{id_expediente}` | Consulta registro (SP select) |
| POST | `/api/firmar-rep-legal/Save` | Crea (EF insert) o actualiza (EF update) |
| POST | `/api/firmar-rep-legal/avanzar/{id_expediente}` | Valida + transiciona workflow + bitácora |

### 3.1 Patrón de manejo de errores (HandleException)

El Controller debe implementar `HandleException` siguiendo el patrón de BBV-86:

```csharp
private IActionResult HandleException(Exception ex)
{
    if (ex.Message.StartsWith("Campos obligatorios faltantes:", StringComparison.OrdinalIgnoreCase))
    {
        var campos = ex.Message
            .Replace("Campos obligatorios faltantes:", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return BadRequest(new
        {
            status = false,
            detail = "Campos obligatorios faltantes",
            campos_faltantes = campos,
            message = "Campos obligatorios faltantes"
        });
    }

    return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
}
```

> Esto permite al frontend mostrar los campos faltantes individualmente.

---

## 4. Backend — Lógica de Avanzar

### 4.1 Constantes (Constants.cs)

```csharp
// En ActividadesBBVA
public const string EscrituracionFirmarRepLegal = "BBVA_ESCRITURACION_FIRMAR_REP_LEGAL";

// En TransicionesBBVA (nombres deben coincidir con el XPDL del workflow)
public const string FirmarRepLegalEntregaEP = "TBD_TRANSICION_ENTREGA_EP";       // TODO: Obtener del XPDL
public const string FirmarRepLegalDevolucion = "TBD_TRANSICION_DEVOLUCION_EP";   // TODO: Obtener del XPDL

// En una clase estática de códigos
public static class ConceptoFirmaRepLegal
{
    public const string FirmadaConforme = "CRL-1";  // code en tabla catalogo
    public const string NoFirmada = "CRL-2";        // code en tabla catalogo
}
```

> **IMPORTANTE:** Los valores `TBD_TRANSICION_*` deben reemplazarse con los nombres
> reales de las transiciones del archivo XPDL. Consultar con el equipo de workflow.

### 4.2 Dependencias del Application (inyección)

```csharp
public FirmarRepLegalApplication(
    MultibancaDBContext multibancaDBContext,
    IFirmarRepLegalRepository firmarRepLegalRepository,
    IMapper mapper,
    ICommonApplication commonApplication,           // → GetCatalogoByType
    IWorkflowApplication workflowApplication,       // → GetTransitions, CapturarDatosFolio, AvanzarActividad
    IBitacoraApplication bitacoraApplication)       // → Create
    : base(multibancaDBContext, firmarRepLegalRepository, mapper)
```

> **Nota:** A diferencia de BBV-86, esta actividad NO requiere `IActividadesApplication`
> porque no tiene enrutamiento paralelo ni lógica anti-retorno. El flujo es lineal
> (solo 2 destinos posibles, sin verificar conceptos previos).

### 4.3 Flujo

```
1. Leer registro con SP usp_select_firmar_rep_legal_bbva
2. Validar concepto_firma no vacío
3. SI concepto_firma == "CRL-2" (Escritura NO firmada):
     - Validar tipologia obligatoria (code: TRL-1 o TRL-2)
     - Validar casuistica obligatoria (code: CasRL-1..CasRL-5)
     - Validar observaciones obligatorias
     - Buscar transición → Constants.TransicionesBBVA.FirmarRepLegalDevolucion
     - Destino: "Realizar Devolución EP" (Analista de Vivienda)
4. SI concepto_firma == "CRL-1" (Escritura firmada Conforme):
     - Buscar transición → Constants.TransicionesBBVA.FirmarRepLegalEntregaEP
     - Destino: "Realizar Entrega EP Firmada" (Analista de Vivienda)
5. Ejecutar WorkflowApplication.AvanzarActividad(transition_id, folio, user_id)
6. Registrar bitácora (ver sección 4.4)
```

### 4.4 Registro en Bitácora

```csharp
var observacionesBitacora = $"Avance de Firmar Rep. Legal. " +
    $"Concepto: {formulario.concepto_firma}. " +
    $"Destino: [{destinoActividad}].";

if (formulario.concepto_firma == ConceptoFirmaRepLegal.NoFirmada)
{
    observacionesBitacora += $" Tipología: {formulario.tipologia}. Casuística: {formulario.casuistica}.";
}

if (!string.IsNullOrWhiteSpace(formulario.observaciones))
    observacionesBitacora += $" Observaciones: {formulario.observaciones}";

_bitacoraApplication.Create(new bitacora
{
    id_expediente = idExpediente,
    id_actividad = Constants.ActividadesBBVA.EscrituracionFirmarRepLegal,
    id_usuario = userId,
    fecha_alta = DateTime.Now,
    observaciones = observacionesBitacora,
    is_active = true,
    row_status = true
}, userId);
```

### 4.5 Lógica anti-retorno (CA11 equivalente)

Esta actividad NO requiere verificar conceptos previos porque:
- No dispara actividades paralelas.
- El flujo desde "Realizar Devolución EP" retorna a actividades anteriores
  (Revisar EP Abogado), no de vuelta a Firmar Rep. Legal.
- Si en el futuro el flujo puede retornar a esta actividad, se debe agregar
  `IActividadesApplication` y verificar `IsCompleteActivity` antes de avanzar.

### Nota sobre almacenamiento

| Campo en tabla | Se almacena | Ejemplo |
|---|---|---|
| concepto_firma | El **code** de L41 | `"CRL-1"` |
| tipologia | El **code** de L42 | `"TRL-1"` |
| casuistica | El **code** de L43 | `"CasRL-2"` |

> El frontend muestra la `description` en los dropdowns pero envía/almacena el `code`.
> La comparación en backend siempre es code vs constante.
> Si la descripción cambia en la tabla catálogo, nada se rompe.

---

## 5. Frontend — Archivos a crear

| # | Archivo | Descripción |
|---|---|---|
| 1 | `features/actividades/firmar_rep_legal/models/firmar_rep_legal.ts` | Interface + factory EMPTY |
| 2 | `features/actividades/firmar_rep_legal/models/controles.ts` | Interface controles (L41, L42, L43) |
| 3 | `features/actividades/firmar_rep_legal/api/firmarRepLegalService.ts` | 4 llamadas HTTP (ver sección 5.1) |
| 4 | `features/actividades/firmar_rep_legal/hooks/useFirmarRepLegal.ts` | useQuery consulta |
| 5 | `features/actividades/firmar_rep_legal/hooks/useControlesFirmarRepLegal.ts` | useQuery catálogos |
| 6 | `features/actividades/firmar_rep_legal/hooks/useUpsertFirmarRepLegal.ts` | useMutation guardar |
| 7 | `features/actividades/firmar_rep_legal/hooks/useAvanzarFirmarRepLegal.ts` | useMutation avanzar |
| 8 | `features/actividades/firmar_rep_legal/components/DatosHeredadosSection.tsx` | Solo lectura |
| 9 | `features/actividades/firmar_rep_legal/components/ConceptoFirmaSection.tsx` | Campos editables |
| 10 | `features/actividades/firmar_rep_legal/pages/firmar_rep_legal_page.tsx` | Página principal |
| 11 | `routes/Routes.tsx` | Ruta: `firmar_rep_legal/:id_expediente` |

### 5.1 Service — Llamadas HTTP

> **Patrón:** Seguir exactamente la estructura de `firmarEscrituraClienteService.ts`

```typescript
import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmarRepLegal } from '../models/firmar_rep_legal';
import type { ControlesFirmarRepLegal } from '../models/controles';

const PATH_URL = '/api/firmar-rep-legal';

export const firmarRepLegalService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<FirmarRepLegal | null>> {
    const response = await axiosClient.get<ApiResponse<FirmarRepLegal | null>>(
      `${PATH_URL}/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(payload: FirmarRepLegal): Promise<ApiResponse<FirmarRepLegal>> {
    const response = await axiosClient.post<ApiResponse<FirmarRepLegal>>(
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

  async getControles(): Promise<ApiResponse<ControlesFirmarRepLegal>> {
    const response = await axiosClient.get<ApiResponse<ControlesFirmarRepLegal>>(
      `${PATH_URL}/controles`,
    );
    return response.data;
  },
};
```

### 5.2 Hooks — Patrón de implementación

> **Referencia:** Los hooks siguen el patrón de `@tanstack/react-query` establecido en BBV-86.

```typescript
// useFirmarRepLegal.ts — useQuery para consulta
import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmarRepLegal } from '../models/firmar_rep_legal';
import { firmarRepLegalService } from '../api/firmarRepLegalService';

export function useFirmarRepLegal(id_expediente: number) {
  return useQuery<ApiResponse<FirmarRepLegal | null>>({
    queryKey: ['firmar_rep_legal', id_expediente],
    queryFn: () => firmarRepLegalService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
```

```typescript
// useControlesFirmarRepLegal.ts — useQuery para catálogos con staleTime largo
import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesFirmarRepLegal } from '../models/controles';
import { firmarRepLegalService } from '../api/firmarRepLegalService';

export function useControlesFirmarRepLegal() {
  return useQuery<ApiResponse<ControlesFirmarRepLegal>>({
    queryKey: ['firmar_rep_legal_controles'],
    queryFn: () => firmarRepLegalService.getControles(),
    staleTime: 1000 * 60 * 30, // 30 min — los catálogos no cambian durante la sesión
  });
}
```

```typescript
// useUpsertFirmarRepLegal.ts — useMutation para guardar + invalidar cache
import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmarRepLegal } from '../models/firmar_rep_legal';
import { firmarRepLegalService } from '../api/firmarRepLegalService';

export function useUpsertFirmarRepLegal() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<FirmarRepLegal>, unknown, FirmarRepLegal>({
    mutationFn: (payload) => firmarRepLegalService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['firmar_rep_legal', variables.id_expediente],
      });
    },
  });
}
```

```typescript
// useAvanzarFirmarRepLegal.ts — useMutation para avanzar
import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { firmarRepLegalService } from '../api/firmarRepLegalService';

export function useAvanzarFirmarRepLegal() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => firmarRepLegalService.avanzar(id_expediente),
  });
}
```

### 5.3 Componentes — Patrones de referencia (BBV-86)

Los componentes de BBV-86 establecen el patrón a seguir:

| Componente BBV-86 | Props | Patrón |
|---|---|---|
| `InformacionNotariaSection` | `form`, `isDisabled`, `updateField` | Componentes `InputTextForm`, `InputNumberForm`, `InputCalendarForm` de `@/shared/components/` |
| `FormalizacionEscrituraSection` | `form`, `isDisabled`, `updateField`, `representantesLegales` | Dropdown de PrimeReact con `optionLabel="description"` + `optionValue="code"` |
| `DecisionesEnrutamientoSection` | `form`, `isDisabled`, `updateField`, `tipologias`, `tiposLeasing` | `SelectButton` para SI/NO, `Dropdown` para catálogos |
| `ConfirmacionCausarModal` | `visible`, `onConfirm`, `onCancel` | Componente `ConfirmModal` de `@/shared/components/` |
| `RegistroContactoSection` | `id_expediente`, `id_actividad` | Modal + DataTable, hooks transversales |

#### Patrón de Props para componentes de sección

```typescript
// Estructura base para las props de cualquier sección editable
interface SectionProps {
  form: FirmarRepLegal;
  isDisabled: boolean;
  updateField: <K extends keyof FirmarRepLegal>(
    field: K,
    value: FirmarRepLegal[K],
  ) => void;
}
```

#### ConceptoFirmaSection — Estructura esperada

```typescript
import { Dropdown } from 'primereact/dropdown';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import type { FirmarRepLegal } from '../models/firmar_rep_legal';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

interface Props {
  form: FirmarRepLegal;
  isDisabled: boolean;
  updateField: <K extends keyof FirmarRepLegal>(
    field: K,
    value: FirmarRepLegal[K],
  ) => void;
  conceptoOptions: ControlBaseDTO[];      // L41
  tipologiaOptions: ControlBaseDTO[];     // L42
  casuisticaOptions: ControlBaseDTO[];    // L43 (ya filtrada por parent_code)
  onConceptoChange: (value: string) => void;
  onTipologiaChange: (value: string) => void;
}
```

#### DatosHeredadosSection — Estructura esperada

```typescript
// Muestra datos de solo lectura heredados de actividades previas
interface Props {
  datosHeredados: {
    notaria?: string | null;
    numero_notaria?: number | null;
    ciudad_notaria?: string | null;
    fecha_notaria?: string | null;
    numero_escritura?: string | null;
    fecha_escritura?: string | null;
  } | null;
}
```

#### Componentes compartidos usados (de `@/shared/components/`)

- `InputTextForm` — campo de texto con label, required, maxLength
- `InputNumberForm` — campo numérico con label, min, max
- `InputCalendarForm` — selector de fecha con label, required
- `InputTextAreaForm` — textarea con label, maxLength, rows
- `ConfirmModal` — modal de confirmación genérico (si se necesita en el futuro)

#### Librería de UI: PrimeReact

- `Dropdown` — `optionLabel="description"` + `optionValue="code"`
- `SelectButton` — para opciones SI/NO
- `Accordion` / `AccordionTab` — contenedores de secciones
- `Card` — wrapper del formulario
- `Button` — botones de acción
- `Toast` — notificaciones
- `DataTable` / `Column` — si se necesita tabla (no aplica aquí)

---

## 6. Frontend — Estructura de la página

### 6.1 Funciones Transversales — Qué se incluye

| Función transversal | ¿Aplica? | Notas |
|---|---|---|
| Expediente Digital | ✅ Sí | Carga/visualización de documentos |
| Trazabilidad / Bitácora | ✅ Sí | Historial de acciones |
| Carta de Aprobación Automática | ❌ No | No aplica para esta actividad (el Rep. Legal solo firma) |
| Registro de Contacto | ❌ No | No aplica (el Rep. Legal no contacta clientes/notarías) |

> Se usa `<FuncionesTransversales show_registro_contacto={false} />` sin sección
> adicional de Registro de Contacto.

### 6.2 Wireframe

```
┌──────────────────────────────────────────────────┐
│ Título: "Firmar Rep. Legal"                      │
├──────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente           │
│   └─ EncabezadoActividad (solo lectura)          │
├──────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales              │
│   └─ Expediente Digital + Trazabilidad           │
├──────────────────────────────────────────────────┤
│ Acordeón 3: Firmar Rep. Legal                    │
│                                                  │
│   ┌─ DatosHeredadosSection (solo lectura) ─────┐│
│   │  Datos Cliente | Datos Notaría              ││
│   │  VoBo Prorrata | Liquidación Leasing        ││
│   │  Concepto Revisión EP Abogado               ││
│   └────────────────────────────────────────────┘│
│                                                  │
│   ┌─ ConceptoFirmaSection (editable) ──────────┐│
│   │                                            ││
│   │  Concepto (Firma): Dropdown L41            ││
│   │    Opciones: "Escritura firmada Conforme"  ││
│   │              "Escritura NO firmada"         ││
│   │                                            ││
│   │  ─── Visible si "Escritura NO firmada" ─── ││
│   │  Tipología: Dropdown L42 *                 ││
│   │  Casuística: Dropdown L43 * (dep. L42)     ││
│   │  Observaciones: textarea *                 ││
│   │  ──────────────────────────────────────── ││
│   │                                            ││
│   └────────────────────────────────────────────┘│
│                                                  │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]  │
└──────────────────────────────────────────────────┘
```

### 6.3 Flujo de estados UI (patrón Editar → Guardar → Avanzar)

```
Estado inicial (carga desde backend):
  ┌─ Si tiene registro (id > 0): campos bloqueados (isDisabled=true), canAdvance=false
  └─ Si no tiene registro: campos editables (isDisabled=false), canAdvance=false

[Editar] → isDisabled=false, canAdvance=false
[Guardar] → POST Save → si éxito: isDisabled=true, canAdvance=true
[Avanzar] → solo habilitado si canAdvance=true → valida → POST avanzar → navegar a bandeja
[Salir] → navigate('/home/bandeja')
```

> **Importante:** El botón "Avanzar" solo se habilita DESPUÉS de guardar exitosamente.
> Esto asegura que el backend tiene la información persistida antes de transicionar.

---

## 7. Frontend — Lógica de dropdowns dependientes (L42 → L43)

```typescript
// Cargar controles (una sola llamada al backend)
const { data: controlesData } = useControlesFirmarRepLegal();
const controles = controlesData?.detail;

// Opciones del dropdown Concepto (L41) — tipo: CONCEPTO_FIRMA_REP_LEGAL
const conceptoOptions = controles?.concepto_firma ?? [];

// Opciones del dropdown Tipología (L42) — tipo: TIPOLOGIA_REP_LEGAL
const tipologiaOptions = controles?.tipologia ?? [];

// Opciones del dropdown Casuística (L43) — tipo: CASUISTICA_REP_LEGAL
// DEPENDIENTE de tipología: filtra por parent_code
const casuisticaOptions = (controles?.casuistica ?? []).filter(
  (item) => item.parent_code === form.tipologia  // ej: form.tipologia = "TRL-1"
);

// Al cambiar Concepto → limpiar hijos
const handleConceptoChange = (value: string) => {
  setForm(f => ({
    ...f,
    concepto_firma: value,       // code: "CRL-1" o "CRL-2"
    tipologia: null,
    casuistica: null,
    observaciones: null,
  }));
};

// Al cambiar Tipología → limpiar casuística
const handleTipologiaChange = (value: string) => {
  setForm(f => ({ ...f, tipologia: value, casuistica: null }));
};
```

### Campos condicionales

```typescript
// Mostrar tipología, casuística y observaciones SOLO si concepto = "CRL-2"
{form.concepto_firma === 'CRL-2' && (
  <>
    <Dropdown tipologia options={tipologiaOptions} optionValue="code" ... />
    <Dropdown casuistica options={casuisticaOptions} optionValue="code" ... />
    <InputTextarea observaciones ... />
  </>
)}
```

---

## 8. Frontend — Validación (antes de Avanzar)

> Patrón idéntico a BBV-86: retorna un array de campos faltantes, no un solo string.

```typescript
const validateAvanzar = (): string[] => {
  const missing: string[] = [];

  if (!form.concepto_firma)
    missing.push('Concepto de Firma');

  if (form.concepto_firma === 'CRL-2') {  // "Escritura NO firmada"
    if (!form.tipologia)
      missing.push('Tipología');
    if (!form.casuistica)
      missing.push('Casuística');
    if (!form.observaciones?.trim())
      missing.push('Observaciones');
  }

  return missing;
};

// Uso antes de avanzar:
const handleAvanzar = async () => {
  const camposFaltantes = validateAvanzar();
  if (camposFaltantes.length > 0) {
    const msg = `Campos obligatorios faltantes: ${camposFaltantes.join(', ')}`;
    toast.current?.show({ severity: 'warn', summary: 'Validación', detail: msg, life: 5000 });
    return;
  }
  await ejecutarAvanzar();
};
```

---

## 9. Modelo TypeScript

```typescript
import type { Auditoria } from '@/models/Auditoria';

export interface FirmarRepLegal extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  concepto_firma: string | null;
  tipologia: string | null;
  casuistica: string | null;
  observaciones: string | null;
}

export const EMPTY_FIRMAR_REP_LEGAL = (
  id_expediente: number,
): FirmarRepLegal => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL',
  concepto_firma: null,
  tipologia: null,
  casuistica: null,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
```

> **Nota:** Extiende `Auditoria` para incluir los campos de auditoría (`is_active`,
> `row_status`, `created_by`, `created_date`, `modified_by`, `modified_date`)
> consistente con el patrón de BBV-86.

---

## 10. Modelo de Controles

```typescript
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

export interface ControlesFirmarRepLegal {
  concepto_firma: ControlBaseDTO[];   // L41
  tipologia: ControlBaseDTO[];        // L42
  casuistica: ControlBaseDTO[];       // L43 (usar parent_code para filtrar)
}
```

---

## 11. Lo que NO se crea

- ❌ No se crea tabla de catálogo nueva (se reusan L41, L42, L43 existentes)
- ❌ No se toca el motor de workflow (solo se buscan transiciones existentes)
- ❌ No se crean componentes transversales nuevos (se reusan EncabezadoActividad y FuncionesTransversales)
- ❌ No se usa EF migrations (se usa script SQL directo)
- ❌ No se necesita Registro de Contacto (el Representante Legal no contacta terceros)
- ❌ No se necesita Carta de Aprobación Automática (no aplica en esta actividad)
- ❌ No se necesita guarda por tipo de crédito (la actividad ya está downstream del filtro de escrituración)
- ❌ No se necesita `IActividadesApplication` (no hay enrutamiento paralelo ni lógica anti-retorno)

---

## 12. Orden de ejecución

1. Ejecutar script SQL contra `BBVA_LEGALIZACION`
2. Crear Entity + EntityConfig + agregar al DbContext
3. Crear Repository (interface + impl usando SPs)
4. Crear Domain Model + registrar AutoMapper
5. Crear Application (interface + impl con lógica de avance)
6. Crear Controller
7. Registrar en IoC + agregar constante en Constants.cs
8. Frontend: model → service → hooks → components → page
9. Agregar ruta en Routes.tsx
10. Probar flujo completo: nuevo → guardar → editar → avanzar

---

## 13. Datos heredados (solo lectura) — CA02

La HU solicita mostrar: Datos del Cliente, Datos de la Notaría, VoBo Prorrata,
Liquidación Leasing y Concepto de Revisión EP del Abogado.

**Estado actual:** BBV-86 (Firmar Escritura Cliente) ya está implementado, por lo que
los datos de Notaría están disponibles en la tabla `firmar_escritura_cliente`.

| Bloque | Implementación ahora |
|---|---|
| Datos del Cliente | ✅ Se usa `EncabezadoActividad` existente (info del folio) |
| Datos de la Notaría | ✅ Se puede leer de `firmar_escritura_cliente` (campos: notaria, numero_notaria, ciudad_notaria, fecha_notaria) |
| Nro. Escritura / Fecha Escritura | ✅ Se puede leer de `firmar_escritura_cliente` |
| VoBo Prorrata | ❌ Pendiente hasta que se implemente esa HU |
| Liquidación Leasing | ❌ Pendiente hasta que se implemente esa HU |
| Concepto Revisión EP | ❌ Pendiente hasta que se implemente BBV-130 |

> **Decisión:** Se renderiza `EncabezadoActividad` como datos del cliente.
> Para los datos de notaría/escritura, se puede crear un query adicional en el
> `GetByExpediente` que lea de `firmar_escritura_cliente` y los devuelva como
> parte del response (campo `datos_heredados`). Los demás bloques se agregarán
> progresivamente cuando sus HU se implementen.

### 13.1 Opción de implementación para datos heredados

```csharp
// En FirmarRepLegalApplication.GetByExpediente:
// Opción A: devolver datos heredados junto al formulario
public async Task<object> GetByExpediente(long idExpediente)
{
    var entity = await RepositoryProvider.GetByExpediente(idExpediente);
    var formulario = entity != null
        ? _mapper.Map<firmar_rep_legal>(entity)
        : new firmar_rep_legal { id_expediente = idExpediente };

    // Leer datos de notaría de la actividad anterior (BBV-86)
    var datosNotaria = await _firmarEscrituraRepository.GetByExpediente(idExpediente);

    return new
    {
        formulario,
        datos_heredados = new
        {
            notaria = datosNotaria?.notaria,
            numero_notaria = datosNotaria?.numero_notaria,
            ciudad_notaria = datosNotaria?.ciudad_notaria,
            fecha_notaria = datosNotaria?.fecha_notaria,
            numero_escritura = datosNotaria?.numero_escritura,
            fecha_escritura = datosNotaria?.fecha_escritura,
        }
    };
}
```

> Si se prefiere mantener el endpoint simple, se puede crear un endpoint separado
> `GET /api/firmar-rep-legal/datos-heredados/{id_expediente}` o dejarlo para
> una fase posterior. La decisión depende de la complejidad de inyectar
> `IFirmarEscrituraClienteRepository` en el Application de Rep. Legal.
