# BBV-99 — Receta de Implementación: Validar Condiciones Desembolso

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Analista de Vivienda.
> Pantalla de herencia multivía con lógica de suspensión y enrutamiento condicional.

---

## Flujo

```
Realizar VB Final Abogado (BBV-96, Escenario B) ──┐
Realizar Excepción Desembolso (BBV-94) ────────────┤
Preformalizar (BBV-98) ────────────────────────────┼──→ Validar Condiciones Desembolso ← ESTA HU
Realizar Gestión Comercial (BBV-97) ───────────────┘
    │
    ├─ SI ¿Requiere Escalamiento Comercial? → "Realizar Gestión Comercial" (Comercial)
    │
    └─ NO → "Gestionar Escalamientos" (Analista de Vivienda)
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv99_validar_condiciones_desembolso.sql`

### 1.1 Tabla

```sql
-- BBV-99 — Validar Condiciones Desembolso (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.validar_condiciones_desembolso CASCADE;

CREATE TABLE IF NOT EXISTS public.validar_condiciones_desembolso (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100),
    origen_tramite                  VARCHAR(50),
    confirmar_plan_pagos            BOOLEAN NOT NULL DEFAULT FALSE,
    requiere_escalamiento_comercial VARCHAR(2),
    suspendida                      BOOLEAN NOT NULL DEFAULT FALSE,
    conteo_caidas                   INTEGER NOT NULL DEFAULT 0,
    observaciones                   VARCHAR(500),
    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_validar_condiciones_desembolso_expediente
    ON public.validar_condiciones_desembolso (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_validar_condiciones_desembolso_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.validar_condiciones_desembolso
LANGUAGE sql STABLE
AS $$
    SELECT actividad.*
    FROM public.validar_condiciones_desembolso actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.validar_condiciones_desembolso TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.validar_condiciones_desembolso_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_validar_condiciones_desembolso_bbva(BIGINT) TO multibanca;
```

### 1.2 cat_actividades_ws

```sql
-- Validar Condiciones Desembolso (ya debería existir de BBV-96)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Validar Condiciones Desembolso', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'validar_condiciones_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO');

-- Destino: Realizar Gestión Comercial
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Gestión Comercial', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_gestion_comercial', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL');

-- Destino: Gestionar Escalamientos
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Gestionar Escalamientos', 'BBVA_ESCRITURACION_GESTIONAR_ESCALAMIENTOS', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'gestionar_escalamientos', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_GESTIONAR_ESCALAMIENTOS');
```

### 1.3 No se necesitan catálogos nuevos

> El campo "¿Requiere Escalamiento Comercial?" es un SI/NO. No requiere catálogo.

---

## 2. Backend — Archivos a crear

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `realizar_condiciones_desembolso_entity.cs` | → `ToTable("validar_condiciones_desembolso")` |
| 2 | Entity Config | `validar_condiciones_desembolso_entity_config.cs` | Fluent API |
| 3 | DbContext | `MultibancaDBContext.cs` | DbSet + config |
| 4 | Repo Interface | `IValidarCondicionesDesembolsoRepository.cs` | `GetByExpediente` |
| 5 | Repo Impl | `ValidarCondicionesDesembolsoRepository.cs` | Query EF |
| 6 | Domain Model | `validar_condiciones_desembolso.cs` | Modelo de dominio |
| 7 | App Interface | `IValidarCondicionesDesembolsoApplication.cs` | Métodos |
| 8 | App Impl | `ValidarCondicionesDesembolsoApplication.cs` | Lógica suspensión + enrutamiento |
| 9 | Controller | `ValidarCondicionesDesembolsoController.cs` | API |
| 10 | Constants | `Constants.cs` | Actividades + Transiciones |
| 11 | IoC | `IoCRegisterMultibanca.cs` | DI |
| 12 | AutoMapper | `AutoMapperProfileMultibanca.cs` | Mapping |

---

## 3. Backend — Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/validar-condiciones-desembolso/GetByIdExpediente/{id_expediente}` | Consulta + banderas + datos heredados + puede_suspender |
| POST | `/api/validar-condiciones-desembolso/Save` | Crea o actualiza |
| POST | `/api/validar-condiciones-desembolso/avanzar/{id_expediente}` | Valida + transiciona workflow + bitácora |
| POST | `/api/validar-condiciones-desembolso/suspender/{id_expediente}` | Suspende la actividad (CA03) |

---

## 4. Backend — Lógica

### 4.1 Constantes (Constants.cs)

```csharp
// En ActividadesBBVA (ya existe de BBV-96)
public const string EscrituracionValidarCondicionesDesembolso = "BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO";
// Nuevas
public const string EscrituracionRealizarGestionComercial = "BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL";
public const string EscrituracionGestionarEscalamientos = "BBVA_ESCRITURACION_GESTIONAR_ESCALAMIENTOS";

// En TransicionesBBVA
public const string ValidarDesembolsoGestionComercial = "BBVA_ESCRITURACION_TR_VALIDAR_DESEMBOLSO_GESTION_COMERCIAL";
public const string ValidarDesembolsoGestionarEscalamientos = "BBVA_ESCRITURACION_TR_VALIDAR_DESEMBOLSO_GESTIONAR_ESCALAMIENTOS";
```

### 4.2 Cálculo de Banderas y Suspensión (CA03/CA06/CA08)

```csharp
// Origen del trámite (CA02/CA08): misma lógica que BBV-96
// Verificar qué actividad previa fue completada para este expediente
bool vieneDeExcepcion = await _actividadesApplication.IsCompleteActivity(
    idExpediente, "BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO");
bool vieneDeAbogado = await _actividadesApplication.IsCompleteActivity(
    idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarVBFinalAbogado);
bool vieneDeGestionComercial = await _actividadesApplication.IsCompleteActivity(
    idExpediente, "BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL");

string origenTramite = vieneDeExcepcion ? "EXCEPCION"
    : vieneDeGestionComercial ? "COMERCIAL"
    : vieneDeAbogado ? "ABOGADO"
    : "PREFORMALIZAR";

// Puede suspender (CA03): verificar si hay actividades registrales activas en paralelo
bool hayRecepcionBoletaActiva = await _actividadesApplication.ExisteActividadActiva(
    idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarRecepcionBoleta);
bool hayEPRegistradasActiva = await _actividadesApplication.ExisteActividadActiva(
    idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarEPRegistradas);
bool puedeSuspender = hayRecepcionBoletaActiva || hayEPRegistradasActiva;

// Conteo de caídas (CA06): contar registros históricos completados de esta actividad
// para este expediente en la tabla actividades
```

### 4.3 Flujo de Avanzar (CA05)

```
1. Leer registro del expediente
2. Validar campos obligatorios (CA04/CA07):
     - confirmar_plan_pagos == true
     - requiere_escalamiento_comercial (obligatorio)
3. Obtener transiciones y folio
4. Determinar destino:
     a) SI requiere_escalamiento_comercial == "SI" → "Realizar Gestión Comercial" (Comercial)
     b) SI requiere_escalamiento_comercial == "NO" → "Gestionar Escalamientos" (Analista)
5. Registrar bitácora
```

### 4.4 Validación de campos obligatorios

```csharp
private static void ValidarCamposObligatorios(validar_condiciones_desembolso formulario)
{
    var camposFaltantes = new List<string>();

    if (!formulario.confirmar_plan_pagos)
        camposFaltantes.Add("Confirmar verificación del Plan de Pagos");

    if (string.IsNullOrWhiteSpace(formulario.requiere_escalamiento_comercial))
        camposFaltantes.Add("¿Requiere Escalamiento Comercial?");

    if (camposFaltantes.Count > 0)
        throw new InvalidOperationException($"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
}
```

### 4.5 Lógica de Suspender (CA03)

```csharp
public async Task<bool> Suspender(long idExpediente, int userId)
{
    var entity = await RepositoryProvider.GetByExpediente(idExpediente)
        ?? throw new InvalidOperationException("No existe registro para suspender.");

    var formulario = _mapper.Map<validar_condiciones_desembolso>(entity);
    formulario.suspendida = true;
    Update(formulario, userId);

    // Registrar en bitácora
    _bitacoraApplication.Create(new bitacora
    {
        id_expediente = idExpediente,
        id_actividad = ActividadValidarDesembolso,
        id_usuario = userId,
        fecha_alta = DateTime.Now,
        observaciones = "Actividad suspendida. Hay actividades registrales en curso paralelo.",
        is_active = true,
        row_status = true
    }, userId);

    return true;
}
```

---

## 5. Frontend — Archivos a crear

| # | Archivo | Descripción |
|---|---|---|
| 1 | `models/validar_condiciones_desembolso.ts` | Interface + factory |
| 2 | `api/validarCondicionesDesembolsoService.ts` | 4 llamadas HTTP (get, save, avanzar, suspender) |
| 3 | `hooks/useValidarCondicionesDesembolso.ts` | useQuery |
| 4 | `hooks/useUpsertValidarCondicionesDesembolso.ts` | useMutation |
| 5 | `hooks/useAvanzarValidarCondicionesDesembolso.ts` | useMutation |
| 6 | `hooks/useSuspenderValidarCondicionesDesembolso.ts` | useMutation |
| 7 | `components/CondicionesDesembolsoSection.tsx` | Checkbox plan pagos + toggle escalamiento + observaciones |
| 8 | `pages/validar_condiciones_desembolso_page.tsx` | Página principal |
| 9 | `routes/Routes.tsx` | Ruta: `validar_condiciones_desembolso/:id_expediente` |

---

## 6. Frontend — Wireframe

```
┌──────────────────────────────────────────────────────────┐
│ Título: "Validar Condiciones Desembolso"                 │
├──────────────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente                   │
├──────────────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales                      │
├──────────────────────────────────────────────────────────┤
│ Acordeón 3: Validar Condiciones Desembolso               │
│                                                          │
│   [Suspender Actividad] ← solo visible si puedeSuspender│
│                                                          │
│   Conteo de Caídas: N (informativo)  — CA06             │
│                                                          │
│   ┌─ CondicionesDesembolsoSection ────────────────────┐ │
│   │  ☑ Confirmar verificación Plan de Pagos *         │ │
│   │  ¿Requiere Escalamiento Comercial?: Toggle *      │ │
│   │  Observaciones: textarea (opcional)               │ │
│   └──────────────────────────────────────────────────┘ │
│                                                          │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]         │
└──────────────────────────────────────────────────────────┘
```

---

## 7. Frontend — Validación

```typescript
const validateAvanzar = (): string[] => {
  const m: string[] = [];
  if (!form.confirmar_plan_pagos) m.push('Confirmar verificación del Plan de Pagos');
  if (!form.requiere_escalamiento_comercial) m.push('¿Requiere Escalamiento Comercial?');
  return m;
};
```

---

## 8. Modelo TypeScript

```typescript
export interface ValidarCondicionesDesembolso extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  origen_tramite: string | null;
  confirmar_plan_pagos: boolean;
  requiere_escalamiento_comercial: string | null;  // "SI" | "NO"
  suspendida: boolean;
  conteo_caidas: number;
  observaciones: string | null;
}

export interface GetByExpedienteResponse {
  formulario: ValidarCondicionesDesembolso;
  puede_suspender: boolean;
  origen_tramite: string;
  conteo_caidas: number;
}
```

---

## 9. Script de Workflow (DBWFBBVA)

```sql
-- Actividades destino
INSERT INTO public.xpdl_activities (activity_id, workflow_process_id, display_name, name, task_type, task_form_type, task_form_uri, performer, sub_flow_id)
SELECT 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'WP_BBVA_CONTACTO_CLIENTE', 'Realizar Gestión Comercial', 'Realizar Gestión Comercial', 'TaskUser', 'UserDefined', 'realizar_gestion_comercial', 'COMERCIAL', NULL
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_activities WHERE activity_id = 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL');

INSERT INTO public.xpdl_activities (activity_id, workflow_process_id, display_name, name, task_type, task_form_type, task_form_uri, performer, sub_flow_id)
SELECT 'BBVA_ESCRITURACION_GESTIONAR_ESCALAMIENTOS', 'WP_BBVA_CONTACTO_CLIENTE', 'Gestionar Escalamientos', 'Gestionar Escalamientos', 'TaskUser', 'UserDefined', 'gestionar_escalamientos', 'ANALISTA_VIVIENDA', NULL
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_activities WHERE activity_id = 'BBVA_ESCRITURACION_GESTIONAR_ESCALAMIENTOS');

-- Transiciones
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_VALIDAR_DESEMBOLSO_GESTION_COMERCIAL', 'BBVA_ESCRITURACION_TR_VALIDAR_DESEMBOLSO_GESTION_COMERCIAL', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_VALIDAR_DESEMBOLSO_GESTION_COMERCIAL');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_VALIDAR_DESEMBOLSO_GESTIONAR_ESCALAMIENTOS', 'BBVA_ESCRITURACION_TR_VALIDAR_DESEMBOLSO_GESTIONAR_ESCALAMIENTOS', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'BBVA_ESCRITURACION_GESTIONAR_ESCALAMIENTOS', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_VALIDAR_DESEMBOLSO_GESTIONAR_ESCALAMIENTOS');
```

---

## 10. Diferencias clave con BBV-96

| Aspecto | BBV-96 (VB Final Abogado) | BBV-99 (Validar Condiciones Desembolso) |
|---|---|---|
| Rol | Abogado | Analista de Vivienda |
| Enrutamiento | 3 destinos (devolución/desembolso/garantías) | 2 destinos (gestión comercial/escalamientos) |
| Suspensión | No | Sí (CA03) — botón condicionado |
| Conteo caídas | No | Sí (CA06) — informativo |
| Plan de Pagos | No | Sí (CA04) — checkbox + documento obligatorio |
| Origen multivía | 2 orígenes (EP Registradas / Control Garantías) | 4 orígenes (Excepción/Preformalizar/Abogado/Comercial) |
| Catálogos | L39/L40 (tipología/casuística) | Ninguno |
| Campos condicionales | Toggle + tipología/casuística | Solo toggle escalamiento |

---

## 11. Notas de implementación

1. **Suspensión (CA03):** Se implementa como un endpoint `/suspender` que marca `suspendida = true` en la tabla. El botón "Suspender Actividad" solo es visible si `puede_suspender = true` (calculado en backend verificando actividades registrales activas con `ExisteActividadActiva`).

2. **Conteo de caídas (CA06):** Se calcula en `GetByExpediente` contando cuántas veces la actividad `VALIDAR_CONDICIONES_DESEMBOLSO` fue completada para ese expediente en la tabla `actividades`. Se muestra como badge informativo en el frontend.

3. **Origen multivía (CA02/CA08):** Se determina usando `IsCompleteActivity` contra la tabla `actividades` (BD Legalización), verificando cuál de las 4 posibles actividades previas fue completada más recientemente.

4. **Validación documento Plan de Pagos (CA04/CA07):** La verificación contra expediente digital se implementa como warning/UI o se posterga si no existe servicio de consulta de documentos adjuntos.

5. **Reutilización:**
   - Patrón Application/Repository/Controller: idéntico a BBV-95/BBV-96
   - Toggle escalamiento: mismo patrón UI que BBV-96 (toggle + enrutamiento)
   - Banderas de origen: misma lógica de `IsCompleteActivity` que BBV-96
   - `ExisteActividadActiva`: ya existe en `IActividadesApplication`
