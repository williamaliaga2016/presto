# BBV-97 — Receta de Implementación: Realizar Gestión Comercial

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Comercial.
> Pantalla resolutiva con herencia dinámica y retorno condicional a la actividad de origen.

---

## Flujo

```
Firmar Escritura Cliente (BBV-86) ──────────────┐
Realizar Devolución EP (BBV-90) ────────────────┼──→ Realizar Gestión Comercial ← ESTA HU
Validar Condiciones Desembolso (BBV-99) ────────┘
    │
    ├─ SI ¿Cliente Desiste? → Fin Terminal (cancelar caso + SLAs)
    │
    └─ NO → Retornar a actividad de origen exacta
             ├─ → Firmar Escritura Cliente
             ├─ → Realizar Devolución EP
             └─ → Validar Condiciones Desembolso
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv97_realizar_gestion_comercial.sql`

### 1.1 Tabla

```sql
-- BBV-97 — Realizar Gestión Comercial (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_gestion_comercial CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_gestion_comercial (
    id                      BIGSERIAL PRIMARY KEY,
    id_expediente           BIGINT NOT NULL,
    id_actividad            VARCHAR(100),
    origen_escalamiento     VARCHAR(100),
    cliente_desiste         VARCHAR(2),
    observaciones           VARCHAR(500),
    is_active               BOOLEAN NOT NULL DEFAULT TRUE,
    row_status              BOOLEAN NOT NULL DEFAULT TRUE,
    created_by              INTEGER NOT NULL,
    created_date            TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by             INTEGER,
    modified_date           TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_gestion_comercial_expediente
    ON public.realizar_gestion_comercial (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_realizar_gestion_comercial_bbva(p_id_expediente BIGINT)
RETURNS SETOF public.realizar_gestion_comercial LANGUAGE sql STABLE AS $$
    SELECT * FROM public.realizar_gestion_comercial
    WHERE id_expediente = p_id_expediente AND is_active = TRUE AND row_status = TRUE
    ORDER BY id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_gestion_comercial TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_gestion_comercial_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_gestion_comercial_bbva(BIGINT) TO multibanca;
```

### 1.2 cat_actividades_ws

```sql
-- Realizar Gestión Comercial (ya debería existir de BBV-99)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Gestión Comercial', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_gestion_comercial', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL');
```

### 1.3 No se necesitan catálogos nuevos

> "¿Cliente Desiste?" es SI/NO. No requiere catálogo.

### 1.4 Manejo de múltiples visitas (soft-delete)

> El caso puede pasar varias veces por Gestión Comercial (retorna al origen, se re-escala).
> Al crear un nuevo registro, si ya existe uno activo para ese expediente, se marca el anterior
> como `is_active = false` antes de insertar el nuevo. Esto preserva el historial y respeta
> el índice UNIQUE parcial. El `GetByExpediente` siempre trae el registro activo más reciente.

---

## 2. Backend — Archivos a crear

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `realizar_gestion_comercial_entity.cs` | Columnas |
| 2 | Entity Config | `realizar_gestion_comercial_entity_config.cs` | Fluent API |
| 3 | DbContext | `MultibancaDBContext.cs` | DbSet + config |
| 4 | Repo Interface | `IRealizarGestionComercialRepository.cs` | `GetByExpediente` |
| 5 | Repo Impl | `RealizarGestionComercialRepository.cs` | Query EF |
| 6 | Domain Model | `realizar_gestion_comercial.cs` | Modelo dominio |
| 7 | App Interface | `IRealizarGestionComercialApplication.cs` | Métodos |
| 8 | App Impl | `RealizarGestionComercialApplication.cs` | Lógica retorno/cierre |
| 9 | Controller | `RealizarGestionComercialController.cs` | API |
| 10 | Constants | `Constants.cs` | Transiciones |
| 11 | IoC | Registrar DI |
| 12 | AutoMapper | Mapping |

---

## 3. Backend — Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/realizar-gestion-comercial/GetByIdExpediente/{id_expediente}` | Consulta + datos heredados + origen |
| POST | `/api/realizar-gestion-comercial/Save` | Crea o actualiza |
| POST | `/api/realizar-gestion-comercial/avanzar/{id_expediente}` | Valida + enruta (retorno o fin terminal) |

---

## 4. Backend — Lógica de Avanzar

### 4.1 Constantes (Constants.cs)

```csharp
// En ActividadesBBVA (ya existe de BBV-99)
public const string EscrituracionRealizarGestionComercial = "BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL";

// En TransicionesBBVA — retornos condicionales
public const string GestionComercialRetornoFirmarEscritura = "BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_FIRMAR_ESCRITURA";
public const string GestionComercialRetornoDevolucionEP = "BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_DEVOLUCION_EP";
public const string GestionComercialRetornoValidarDesembolso = "BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_VALIDAR_DESEMBOLSO";
```

### 4.2 Cálculo de Bandera de Origen (CA05)

```csharp
// Determinar de dónde llegó el escalamiento usando IsCompleteActivity en tabla actividades
// Se evalúa cuál actividad completó más recientemente el pase a gestión comercial:
// - Si Firmar Escritura Cliente tiene escalamiento registrado → "FIRMAR_ESCRITURA"
// - Si Realizar Devolución EP completó → "DEVOLUCION_EP"
// - Si Validar Condiciones Desembolso completó con escalamiento → "VALIDAR_DESEMBOLSO"

// Approach simple: verificar cuál de las 3 actividades tiene registro completado
// con requiere_escalamiento_comercial == "SI" o tipologia de escalamiento
var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);
bool vieneDeFirmarEscritura = firmarEscritura?.requiere_escalamiento_comercial == "SI";

var validarDesembolso = await _validarCondicionesDesembolsoRepository.GetByExpediente(idExpediente);
bool vieneDeValidarDesembolso = validarDesembolso?.requiere_escalamiento_comercial == "SI";

// Si ninguno → probablemente viene de Devolución EP
string origenEscalamiento = vieneDeFirmarEscritura ? "FIRMAR_ESCRITURA"
    : vieneDeValidarDesembolso ? "VALIDAR_DESEMBOLSO"
    : "DEVOLUCION_EP";
```

### 4.3 Flujo de Avanzar (CA03/CA04)

```
1. Leer registro del expediente
2. Validar campos obligatorios:
     - cliente_desiste obligatorio (CA03)
3. SI cliente_desiste == "SI":
     a) Mostrar modal de confirmación en frontend (CA06)
     b) Si confirma: CancelCase en workflow → Fin Terminal
     c) Actualizar status del expediente a "Cancelado/Desistido"
4. SI cliente_desiste == "NO":
     a) Leer origen_escalamiento
     b) Seleccionar transición de retorno según origen
     c) AvanzarActividad hacia la actividad de origen
5. Registrar bitácora
```

### 4.4 Validación

```csharp
private static void ValidarCamposObligatorios(realizar_gestion_comercial formulario)
{
    var camposFaltantes = new List<string>();

    if (string.IsNullOrWhiteSpace(formulario.cliente_desiste))
        camposFaltantes.Add("¿Cliente Desiste del Caso?");

    if (camposFaltantes.Count > 0)
        throw new InvalidOperationException($"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
}
```

### 4.5 Lógica de Fin Terminal (CA06)

```csharp
if (formulario.cliente_desiste == "SI")
{
    // Cancelar el caso en el workflow
    await _workflowApplication.CancelCase(idExpediente);

    // Marcar todas las actividades activas del expediente como desistidas
    // usando UtilidadesApplication (ya existe en el proyecto)

    // Registrar bitácora de cierre
    return new List<AssignActivityDTO>(); // Sin actividades nuevas
}
```

> **Nota:** `CancelCase` ya existe en `IWorkflowApplication` (visto en el código del proyecto).
> La lógica de cancelar SLAs y actividades pendientes se reutiliza de `UtilidadesApplication`
> que ya tiene un método para desistir expedientes.

---

## 5. Frontend — Archivos a crear

| # | Archivo | Descripción |
|---|---|---|
| 1 | `models/realizar_gestion_comercial.ts` | Interface + factory |
| 2 | `api/realizarGestionComercialService.ts` | 3 llamadas HTTP |
| 3 | `hooks/useRealizarGestionComercial.ts` | useQuery |
| 4 | `hooks/useUpsertRealizarGestionComercial.ts` | useMutation |
| 5 | `hooks/useAvanzarRealizarGestionComercial.ts` | useMutation |
| 6 | `components/DatosHeredadosSection.tsx` | Dinámico según origen |
| 7 | `components/GestionComercialSection.tsx` | Toggle desiste + observaciones |
| 8 | `pages/realizar_gestion_comercial_page.tsx` | Página + modal confirmación |
| 9 | `routes/Routes.tsx` | Ruta: `realizar_gestion_comercial/:id_expediente` |

---

## 6. Frontend — Wireframe

```
┌──────────────────────────────────────────────────────────┐
│ Título: "Realizar Gestión Comercial"                     │
├──────────────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente                   │
├──────────────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales                      │
├──────────────────────────────────────────────────────────┤
│ Acordeón 3: Realizar Gestión Comercial                   │
│                                                          │
│   ┌─ DatosHeredadosSection (dinámico por origen) ──────┐│
│   │  Si Firmar Escritura: datos notaría + cliente      ││
│   │  Si Devolución EP: conceptos rechazo               ││
│   │  Si Validar Desembolso: contexto financiero        ││
│   └────────────────────────────────────────────────────┘│
│                                                          │
│   ┌─ GestionComercialSection ──────────────────────────┐│
│   │  ¿Cliente Desiste del Caso?: Toggle (SI/NO) *      ││
│   │  Observaciones: textarea (opcional)                ││
│   └────────────────────────────────────────────────────┘│
│                                                          │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]         │
│                                                          │
│   [Modal CA06 — si desiste y avanza]:                    │
│   "¿Estás seguro de dar cierre al Folio [X]?            │
│    El avance no podrá ser recuperado."                   │
│    [NO — Cerrar]  [SÍ — Confirmar cierre]               │
└──────────────────────────────────────────────────────────┘
```

---

## 7. Frontend — Validación y Modal

```typescript
const validateAvanzar = (): string[] => {
  const m: string[] = [];
  if (!form.cliente_desiste) m.push('¿Cliente Desiste del Caso?');
  return m;
};

// Si cliente_desiste == "SI", mostrar ConfirmDialog antes de llamar al endpoint
const handleAvanzar = async () => {
  const faltantes = validateAvanzar();
  if (faltantes.length > 0) { /* toast warning */ return; }

  if (form.cliente_desiste === 'SI') {
    // Mostrar modal de confirmación (CA06)
    confirmDialog({
      message: `¿Estás seguro de dar cierre al Folio ${id_expediente}? El avance del Folio no podrá ser recuperado.`,
      header: 'Confirmación de Cierre',
      icon: 'pi pi-exclamation-triangle',
      accept: () => ejecutarAvanzar(),
      reject: () => { /* no hacer nada */ }
    });
  } else {
    ejecutarAvanzar();
  }
};
```

---

## 8. Modelo TypeScript

```typescript
export interface RealizarGestionComercial extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  origen_escalamiento: string | null;  // "FIRMAR_ESCRITURA" | "DEVOLUCION_EP" | "VALIDAR_DESEMBOLSO"
  cliente_desiste: string | null;      // "SI" | "NO"
  observaciones: string | null;
}

export interface GetByExpedienteResponse {
  formulario: RealizarGestionComercial;
  origen_escalamiento: string;
  datos_heredados: any;
}
```

---

## 9. Script de Workflow (DBWFBBVA)

```sql
-- Transiciones de retorno desde Gestión Comercial
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_FIRMAR_ESCRITURA', 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_FIRMAR_ESCRITURA', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_FIRMAR_ESCRITURA');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_DEVOLUCION_EP', 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_DEVOLUCION_EP', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_DEVOLUCION_EP');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_VALIDAR_DESEMBOLSO', 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_VALIDAR_DESEMBOLSO', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_GESTION_COMERCIAL_VALIDAR_DESEMBOLSO');
```

---

## 10. Diferencias clave con BBV-96 y BBV-99

| Aspecto | BBV-96 | BBV-99 | BBV-97 (esta) |
|---|---|---|---|
| Rol | Abogado | Analista Vivienda | Comercial |
| Decisión principal | ¿Requiere Devolución? | ¿Escalamiento Comercial? | ¿Cliente Desiste? |
| Fin Terminal | No | No | Sí (CA04/CA06) |
| Modal confirmación | No | No | Sí — "¿Seguro cierre folio?" |
| Retorno a origen | No | No | Sí — retorna a actividad exacta |
| Catálogos | L39/L40 | Ninguno | Ninguno |
| Suspensión | No | Sí (CA03) | No |
| Origen multivía | 2 | 4 | 3 |

---

## 11. Datos heredados por origen (CA02)

| Origen | Datos a mostrar | Fuente (tabla) |
|---|---|---|
| **Firmar Escritura Cliente** | Notaría, Nro Notaría, Ciudad, Nro Escritura, Fecha Escritura, Representante Legal, Tipo Crédito | `firmar_escritura_cliente` + `validar_informacion_bbva` |
| **Realizar Devolución EP** | Tipología rechazo, Casuística, Observaciones del abogado (concepto devolución) + datos notaría heredados | `realizar_vb_final_abogado` (tipología/casuística) + `firmar_escritura_cliente` |
| **Validar Condiciones Desembolso** | Confirmación plan de pagos, ¿Requiere escalamiento? (contexto financiero), Origen del trámite | `validar_condiciones_desembolso` |

### Implementación en GetByExpediente:

```csharp
// Según origen_escalamiento, cargar datos heredados distintos
object datosHeredados;

if (origenEscalamiento == "FIRMAR_ESCRITURA")
{
    var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);
    datosHeredados = new
    {
        origen_label = "Firmar Escritura Cliente",
        notaria = firmarEscritura?.notaria,
        numero_notaria = firmarEscritura?.numero_notaria,
        ciudad_notaria = firmarEscritura?.ciudad_notaria,
        numero_escritura = firmarEscritura?.numero_escritura,
        fecha_escritura = firmarEscritura?.fecha_escritura,
        representante_legal = firmarEscritura?.representante_legal,
    };
}
else if (origenEscalamiento == "DEVOLUCION_EP")
{
    var vbFinal = await _vbFinalAbogadoRepository.GetByExpediente(idExpediente);
    var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);
    datosHeredados = new
    {
        origen_label = "Realizar Devolución EP",
        tipologia_rechazo = vbFinal?.tipologia,
        casuistica_rechazo = vbFinal?.casuistica,
        observaciones_abogado = vbFinal?.observaciones,
        notaria = firmarEscritura?.notaria,
        numero_escritura = firmarEscritura?.numero_escritura,
    };
}
else // VALIDAR_DESEMBOLSO
{
    var validarDesembolso = await _validarCondicionesDesembolsoRepository.GetByExpediente(idExpediente);
    datosHeredados = new
    {
        origen_label = "Validar Condiciones Desembolso",
        plan_pagos_confirmado = validarDesembolso?.confirmar_plan_pagos,
        requiere_escalamiento = validarDesembolso?.requiere_escalamiento_comercial,
        observaciones_condiciones = validarDesembolso?.observaciones,
    };
}
```

> Los repositorios necesarios (`IFirmarEscrituraClienteRepository`, `IRealizarVBFinalAbogadoRepository`,
> `IValidarCondicionesDesembolsoRepository`) se inyectan en el constructor del Application.

1. **Retorno a origen (CA04-NO):** El `origen_escalamiento` determina cuál transición usar. Si viene de Firmar Escritura → retorna a Firmar Escritura. Misma lógica que BBV-96 para el triple enrutamiento, pero aquí retorna a actividades pasadas en vez de avanzar a nuevas.

2. **Fin Terminal (CA04-SI / CA06):** Usar `CancelCase` del workflow + la lógica de `UtilidadesApplication` que ya existe para desistir expedientes (cambia status de todas las actividades activas a "Desistido").

3. **Modal de confirmación (CA06):** Usar `ConfirmDialog` de PrimeReact. Solo aparece cuando `cliente_desiste == "SI"` y el usuario hace clic en Avanzar.

4. **Cálculo del origen (CA05):** Misma lógica que BBV-96/BBV-99 — verificar en las tablas de actividades previas cuál tiene `requiere_escalamiento_comercial == "SI"`.

5. **Reutilización:**
   - Patrón Application/Repository/Controller: idéntico a todos los anteriores
   - Datos heredados dinámicos: mismo patrón que BBV-96 y BBV-99
   - Toggle + enrutamiento: mismo patrón UI que BBV-96 (toggle decisión → destino condicional)
   - `CancelCase`: ya existe en `IWorkflowApplication`
   - Desistimiento de actividades: ya existe en `UtilidadesApplication`
