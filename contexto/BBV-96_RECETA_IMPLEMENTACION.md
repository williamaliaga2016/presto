# BBV-96 — Receta de Implementación: Realizar VB Final Abogado

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Abogado.
> Pantalla de origen dual (EP Registradas o Control de Garantías) con enrutamiento condicional complejo.

---

## Flujo

```
Realizar EP Registradas (BBV-95) ─────────┐
                                           ├──→ Realizar VB Final Abogado ← ESTA HU
Gestionar Control de Garantías (BBV-102) ──┘
    │
    ├─ SI ¿Requiere Devolución? → "Realizar Devolución EP" (Analista de Vivienda)
    │
    └─ NO ¿Requiere Devolución?
         ├─ Escenario A (Aplica Excepción) → "Gestionar Control de Garantías"
         ├─ Escenario B (Sin Excepción, origen regular) → "Validar Condiciones Desembolso"
         └─ Escenario C (Bypass, viene de Control Garantías) → "Gestionar Control de Garantías"
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv96_realizar_vb_final_abogado.sql`

### 1.1 Tabla

```sql
-- BBV-96 — Realizar VB Final Abogado (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_vb_final_abogado CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_vb_final_abogado (
    id                          BIGSERIAL PRIMARY KEY,
    id_expediente               BIGINT NOT NULL,
    id_actividad                VARCHAR(100),
    requiere_devolucion         VARCHAR(2),
    tipologia                   VARCHAR(200),
    casuistica                  VARCHAR(200),
    origen_tramite              VARCHAR(50),
    bandera_excepcion           BOOLEAN NOT NULL DEFAULT FALSE,
    observaciones               VARCHAR(500),
    is_active                   BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                  BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                  INTEGER NOT NULL,
    created_date                TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                 INTEGER,
    modified_date               TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_vb_final_abogado_expediente
    ON public.realizar_vb_final_abogado (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_realizar_vb_final_abogado_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.realizar_vb_final_abogado
LANGUAGE sql STABLE
AS $$
    SELECT actividad.*
    FROM public.realizar_vb_final_abogado actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_vb_final_abogado TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_vb_final_abogado_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_vb_final_abogado_bbva(BIGINT) TO multibanca;
```

### 1.2 Insert / Update

> EF Core `Create()` / `Update()` — mismo patrón de siempre.

### 1.3 Registro en cat_actividades_ws

```sql
-- VB Final Abogado (ya debería existir de BBV-95)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar VB Final Abogado', 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_vb_final_abogado', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO');

-- Destinos
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Devolución EP', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_devolucion_ep', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP');

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Validar Condiciones Desembolso', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'validar_condiciones_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO');

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Gestionar Control de Garantías', 'BBVA_ESCRITURACION_GESTIONAR_CONTROL_GARANTIAS', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'gestionar_control_garantias', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_GESTIONAR_CONTROL_GARANTIAS');
```

### 1.4 Catálogos

> **Tipología y Casuística:** La HU dice "Pendiente de confirmación". Reutilizar los catálogos existentes
> `L39_TIPOLOGIA_DEVOLUCION_EP_ABOGADO` y `L40_CASUISTICA_DEVOLUCION_EP_ABOGADO` que ya existen
> en la BD (usados por BBV-130 y BBV-90). Si negocio confirma catálogos diferentes, crear nuevos.

---

## 2. Backend — Archivos a crear

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `realizar_vb_final_abogado_entity.cs` | Clase con columnas |
| 2 | Entity Config | `realizar_vb_final_abogado_entity_config.cs` | Fluent API |
| 3 | DbContext | `MultibancaDBContext.cs` | DbSet + config |
| 4 | Repo Interface | `IRealizarVBFinalAbogadoRepository.cs` | `GetByExpediente` |
| 5 | Repo Impl | `RealizarVBFinalAbogadoRepository.cs` | Query EF |
| 6 | Domain Model | `realizar_vb_final_abogado.cs` | Modelo de dominio |
| 7 | App Interface | `IRealizarVBFinalAbogadoApplication.cs` | Métodos |
| 8 | App Impl | `RealizarVBFinalAbogadoApplication.cs` | Lógica de enrutamiento |
| 9 | Controller | `RealizarVBFinalAbogadoController.cs` | API |
| 10 | Constants | `Constants.cs` | Actividades + Transiciones |
| 11 | IoC | `IoCRegisterMultibanca.cs` | Registro DI |
| 12 | AutoMapper | `AutoMapperProfileMultibanca.cs` | Mapping |

---

## 3. Backend — Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/realizar-vb-final-abogado/GetByIdExpediente/{id_expediente}` | Consulta + datos heredados + banderas |
| GET | `/api/realizar-vb-final-abogado/controles` | Catálogos tipología/casuística |
| POST | `/api/realizar-vb-final-abogado/Save` | Crea o actualiza |
| POST | `/api/realizar-vb-final-abogado/avanzar/{id_expediente}` | Valida + enruta inteligente + bitácora |

---

## 4. Backend — Lógica de Avanzar

### 4.1 Constantes (Constants.cs)

```csharp
// En ActividadesBBVA (ya existe)
public const string EscrituracionRealizarVBFinalAbogado = "BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO";
// Nuevas
public const string EscrituracionValidarCondicionesDesembolso = "BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO";
public const string EscrituracionGestionarControlGarantias = "BBVA_ESCRITURACION_GESTIONAR_CONTROL_GARANTIAS";
public const string EscrituracionRealizarDevolucionEP = "BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP";

// En TransicionesBBVA
public const string VBFinalAbogadoDevolucionEP = "BBVA_ESCRITURACION_TR_VB_FINAL_DEVOLUCION_EP";
public const string VBFinalAbogadoValidarDesembolso = "BBVA_ESCRITURACION_TR_VB_FINAL_VALIDAR_DESEMBOLSO";
public const string VBFinalAbogadoControlGarantias = "BBVA_ESCRITURACION_TR_VB_FINAL_CONTROL_GARANTIAS";
```

### 4.2 Flujo de Avanzar (CA03/CA04/CA05)

```
1. Leer registro del expediente
2. Validar campos obligatorios:
     - requiere_devolucion obligatorio siempre
     - SI requiere_devolucion == "SI": tipologia + casuistica + observaciones obligatorios (CA04/CA07)
3. Obtener transiciones y folio
4. Determinar destino:
     a) SI requiere_devolucion == "SI" → Realizar Devolución EP (Analista Vivienda)
     b) SI requiere_devolucion == "NO":
          - Si bandera_excepcion == true → Gestionar Control de Garantías (CA05-A)
          - Si origen_tramite == "CONTROL_GARANTIAS" → Gestionar Control de Garantías (CA05-C)
          - Else → Validar Condiciones Desembolso (CA05-B)
5. Registrar bitácora
```

### 4.3 Cálculo de Banderas (CA06)

```csharp
// Al cargar GetByExpediente, todo se resuelve desde la BD de Legalización (tabla actividades):

// 1. origen_tramite: verificar si "Gestionar Control de Garantías" fue completada para este expediente
bool vieneDeControlGarantias = await _actividadesApplication.IsCompleteActivity(
    idExpediente,
    Constants.ActividadesBBVA.EscrituracionGestionarControlGarantias);

string origenTramite = vieneDeControlGarantias
    ? "CONTROL_GARANTIAS"
    : "EP_REGISTRADAS";

// 2. bandera_excepcion: verificar si aplica_excepcion == "SI" en realizar_entrega_ep_firmada
var entregaEP = await _realizarEntregaEpFirmadaRepository.GetByExpediente(idExpediente);
bool banderaExcepcion = entregaEP?.aplica_excepcion == "SI";
```

> **Nota:** Ambas consultas se hacen contra la BD de Legalización (tabla `actividades`
> y tabla `realizar_entrega_ep_firmada`). No se necesita consultar la BD de workflow.
> `IsCompleteActivity` ya existe en `IActividadesApplication` (usado por BBV-86).

### 4.4 Validación de campos obligatorios

```csharp
private static void ValidarCamposObligatorios(realizar_vb_final_abogado formulario)
{
    var camposFaltantes = new List<string>();

    if (string.IsNullOrWhiteSpace(formulario.requiere_devolucion))
        camposFaltantes.Add("¿Requiere Devolución?");

    if (formulario.requiere_devolucion == "SI")
    {
        if (string.IsNullOrWhiteSpace(formulario.tipologia))
            camposFaltantes.Add("Tipología");
        if (string.IsNullOrWhiteSpace(formulario.casuistica))
            camposFaltantes.Add("Casuística");
        if (string.IsNullOrWhiteSpace(formulario.observaciones))
            camposFaltantes.Add("Observaciones");
    }

    if (camposFaltantes.Count > 0)
        throw new InvalidOperationException($"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
}
```

---

## 5. Frontend — Archivos a crear

| # | Archivo | Descripción |
|---|---|---|
| 1 | `models/realizar_vb_final_abogado.ts` | Interface + factory |
| 2 | `models/controles.ts` | Tipología + Casuística |
| 3 | `api/realizarVBFinalAbogadoService.ts` | 4 llamadas HTTP |
| 4 | `hooks/useRealizarVBFinalAbogado.ts` | useQuery |
| 5 | `hooks/useControlesVBFinalAbogado.ts` | useQuery catálogos |
| 6 | `hooks/useUpsertRealizarVBFinalAbogado.ts` | useMutation |
| 7 | `hooks/useAvanzarRealizarVBFinalAbogado.ts` | useMutation |
| 8 | `components/DatosHeredadosSection.tsx` | Solo lectura dinámico (CA02) |
| 9 | `components/VBFinalSection.tsx` | Toggle + campos condicionales |
| 10 | `pages/realizar_vb_final_abogado_page.tsx` | Página |
| 11 | `routes/Routes.tsx` | Ruta: `realizar_vb_final_abogado/:id_expediente` |

---

## 6. Frontend — Wireframe (basado en HTML de referencia)

```
┌──────────────────────────────────────────────────────┐
│ Título: "Realizar VB Final Abogado"                  │
├──────────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente               │
├──────────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales                  │
├──────────────────────────────────────────────────────┤
│ Acordeón 3: Realizar VB Final Abogado                │
│                                                      │
│   ┌─ DatosHeredadosSection (dinámico según origen) ┐│
│   │  Si EP Registradas: datos boleta + confirmación││
│   │  Si Control Garantías: datos de esa etapa      ││
│   └────────────────────────────────────────────────┘│
│                                                      │
│   ┌─ VBFinalSection ──────────────────────────────┐│
│   │  ¿Requiere Devolución?: Toggle (SI/NO) *      ││
│   │                                                ││
│   │  [Si SI] ─────────────────────────────────────││
│   │  │ Tipología: Dropdown (L39) *                ││
│   │  │ Casuística: Dropdown (L40) *               ││
│   │  └────────────────────────────────────────────││
│   │                                                ││
│   │  Observaciones: textarea (condicionado) *      ││
│   └────────────────────────────────────────────────┘│
│                                                      │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]     │
└──────────────────────────────────────────────────────┘
```

---

## 7. Frontend — Validación

```typescript
const validateAvanzar = (): string[] => {
  const m: string[] = [];
  if (!form.requiere_devolucion) m.push('¿Requiere Devolución?');
  if (form.requiere_devolucion === 'SI') {
    if (!form.tipologia) m.push('Tipología');
    if (!form.casuistica) m.push('Casuística');
    if (!form.observaciones?.trim()) m.push('Observaciones');
  }
  return m;
};
```

---

## 8. Modelo TypeScript

```typescript
export interface RealizarVBFinalAbogado extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  requiere_devolucion: string | null;    // "SI" | "NO"
  tipologia: string | null;
  casuistica: string | null;
  origen_tramite: string | null;         // "EP_REGISTRADAS" | "CONTROL_GARANTIAS" — readonly
  bandera_excepcion: boolean;            // readonly, calculada en backend
  observaciones: string | null;
}
```

---

## 9. Script de Workflow (DBWFBBVA)

```sql
-- Actividades destino
INSERT INTO public.xpdl_activities (activity_id, workflow_process_id, display_name, name, task_type, task_form_type, task_form_uri, performer, sub_flow_id)
SELECT 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Validar Condiciones Desembolso', 'Validar Condiciones Desembolso', 'TaskUser', 'UserDefined', 'validar_condiciones_desembolso', 'ANALISTA_VIVIENDA', NULL
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_activities WHERE activity_id = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO');

INSERT INTO public.xpdl_activities (activity_id, workflow_process_id, display_name, name, task_type, task_form_type, task_form_uri, performer, sub_flow_id)
SELECT 'BBVA_ESCRITURACION_GESTIONAR_CONTROL_GARANTIAS', 'WP_BBVA_CONTACTO_CLIENTE', 'Gestionar Control de Garantías', 'Gestionar Control de Garantías', 'TaskUser', 'UserDefined', 'gestionar_control_garantias', 'ANALISTA_VIVIENDA', NULL
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_activities WHERE activity_id = 'BBVA_ESCRITURACION_GESTIONAR_CONTROL_GARANTIAS');

-- Transiciones desde VB Final Abogado
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_VB_FINAL_DEVOLUCION_EP', 'BBVA_ESCRITURACION_TR_VB_FINAL_DEVOLUCION_EP', 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_VB_FINAL_DEVOLUCION_EP');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_VB_FINAL_VALIDAR_DESEMBOLSO', 'BBVA_ESCRITURACION_TR_VB_FINAL_VALIDAR_DESEMBOLSO', 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_VB_FINAL_VALIDAR_DESEMBOLSO');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_VB_FINAL_CONTROL_GARANTIAS', 'BBVA_ESCRITURACION_TR_VB_FINAL_CONTROL_GARANTIAS', 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO', 'BBVA_ESCRITURACION_GESTIONAR_CONTROL_GARANTIAS', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_VB_FINAL_CONTROL_GARANTIAS');
```

---

## 10. Diferencias clave con BBV-95

| Aspecto | BBV-95 (EP Registradas) | BBV-96 (VB Final Abogado) |
|---|---|---|
| Rol | Analista de Vivienda | Abogado |
| Enrutamiento | Lineal (1 destino) | Condicional complejo (3 destinos) |
| Campos | Finalización + Causal + Fecha + Toggle | Toggle + Tipología/Casuística condicionados |
| Banderas ocultas | No | Sí: `origen_tramite` + `bandera_excepcion` (CA06) |
| Origen dual | No | Sí: EP Registradas o Control Garantías (CA02) |
| Catálogos | Ninguno | L39 (Tipología) + L40 (Casuística) — ya existentes |
| Validación documento | No | Sí: "Vobo Final Abogado" (si avanza) y "Formato Corrección" (si tipología = Corrección a Registro) |

---

## 11. Notas de implementación

1. **Enrutamiento triple (CA05):** El Application evalúa `requiere_devolucion`, `bandera_excepcion` y `origen_tramite` para elegir cuál de las 3 transiciones usar.

2. **Banderas de sistema (CA06):** Se calculan en `GetByExpediente` al cargar. `bandera_excepcion` se obtiene de `realizar_entrega_ep_firmada.aplica_excepcion`. `origen_tramite` por ahora se fija en `"EP_REGISTRADAS"` (se ajustará cuando se implemente BBV-102).

3. **Catálogos reutilizados:** Tipología y Casuística usan `L39_TIPOLOGIA_DEVOLUCION_EP_ABOGADO` y `L40_CASUISTICA_DEVOLUCION_EP_ABOGADO` (mismos que BBV-130/BBV-90). NO se crean catálogos nuevos.

4. **Patrón del toggle + campos condicionales:** Idéntico al patrón de BBV-91 (Firmar Rep. Legal) donde `concepto_firma = "NO firmada"` muestra tipología/casuística. Aquí: `requiere_devolucion = "SI"` muestra tipología/casuística.

5. **Documento obligatorio (CA04/CA05):** La validación de adjuntos (Vobo Final Abogado, Formato de Corrección a Registro) se implementa como warning o se posterga si no existe servicio de verificación contra expediente digital.
