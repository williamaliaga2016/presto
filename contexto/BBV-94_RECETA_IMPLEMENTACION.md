# BBV-94 — Receta de Implementación: Realizar Excepción Desembolso

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Comercial.
> Nodo de autorización con escalamiento condicional a Gerencia COH.
> Implementa lógica de sincronización (AND-join) con la ruta larga.

---

## Flujo

```
Firmar Rep. Legal (firmada conforme)
    │
    ├── Ruta corta: Realizar Excepción Desembolso ← ESTA HU
    │                        │
    │                  ¿Requiere VoBo Gerencia?
    │                  /           \
    │               SI              NO
    │               │               │
    │               v               │
    │    Realizar VoBo Gerencia COH │
    │               │               │
    │               v               v
    │         ┌─────────────────────┐
    │         │  AND-JOIN: Espera   │
    │         │  ruta larga también │
    │         └─────────┬───────────┘
    │                   │
    ├── Ruta larga: Entrega EP → Recepción Boleta → EP Registradas → VB Final Abogado
    │                                                                        │
    └────────────────────────────────────────────────────────────────────────┘
                                    │
                                    v
                    Validar Condiciones Desembolso
                    (solo cuando AMBAS rutas completaron)
```

---

## 1. Base de datos — Tabla

```sql
CREATE TABLE IF NOT EXISTS public.realizar_excepcion_desembolso (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100),
    excepcion_autorizada            VARCHAR(2),
    confirmar_excepcion             BOOLEAN DEFAULT FALSE,
    requiere_vobo_gerencia          VARCHAR(2),
    observaciones                   VARCHAR(500),
    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_excepcion_desembolso_expediente
    ON public.realizar_excepcion_desembolso (id_expediente)
    WHERE is_active = true AND row_status = true;

CREATE OR REPLACE FUNCTION public.usp_select_realizar_excepcion_desembolso_bbva(p_id_expediente BIGINT)
RETURNS SETOF public.realizar_excepcion_desembolso LANGUAGE sql STABLE AS $$
    SELECT * FROM public.realizar_excepcion_desembolso
    WHERE id_expediente = p_id_expediente AND is_active = TRUE AND row_status = TRUE
    ORDER BY id DESC LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_excepcion_desembolso TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_excepcion_desembolso_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_excepcion_desembolso_bbva(BIGINT) TO multibanca;
```

---

## 2. Lógica de Avanzar

### 2.1 Validaciones obligatorias

```
- excepcion_autorizada: obligatorio (SI/NO, solo informativo)
- confirmar_excepcion: obligatorio (debe ser true)
- requiere_vobo_gerencia: obligatorio (SI/NO, determina ruta)
```

### 2.2 Enrutamiento (CA06)

```
SI requiere_vobo_gerencia == "SI"
    → Realizar VoBo Gerencia COH (Gerente COH)
    
SI requiere_vobo_gerencia == "NO"
    → Evaluar AND-JOIN:
        SI VB Final Abogado ya completó (ruta larga terminó)
            → Crear Validar Condiciones Desembolso (Analista Vivienda)
        SI NO
            → No crear nada más (esperar a que la ruta larga termine)
            → Registrar en bitácora: "Excepción completada. Esperando ruta larga."
```

### 2.3 Modal de confirmación (CA10)

Antes de avanzar, mostrar modal con texto:
> "Certifico que el trámite cuenta con la aprobación del Área de Riesgos..."

Botones: "Cancelar" / "Aceptar"

---

## 3. Lógica de Sincronización (AND-JOIN)

### Concepto

"Validar Condiciones Desembolso" es un punto de convergencia de 2 rutas paralelas:
- **Ruta corta:** Excepción Desembolso (o VoBo Gerencia COH)
- **Ruta larga:** Entrega EP → Recepción Boleta → EP Registradas → VB Final Abogado

Solo debe crearse cuando **ambas rutas** hayan completado.

### Implementación

**En BBV-94 (Excepción Desembolso) — al avanzar con NO gerencia:**

```csharp
// Después de completar Excepción Desembolso
bool vbFinalCompletado = await _actividadesApplication.IsCompleteActivity(
    idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarVBFinalAbogado);

if (vbFinalCompletado)
{
    // Ruta larga ya terminó → crear Validar Condiciones Desembolso
    await AvanzarAValidarCondiciones(transitions, folio, userId);
}
else
{
    // Ruta larga aún en curso → no crear nada, solo registrar bitácora
    _bitacoraApplication.Create(..., 
        "Excepción completada. Esperando finalización de ruta larga (VB Final Abogado).");
}
```

**En BBV-96 (VB Final Abogado) — al avanzar con NO devolución:**

```csharp
// Después de completar VB Final Abogado
// Verificar si existe una excepción de desembolso para este expediente
bool existeExcepcion = await _actividadesApplication.ExisteActividad(
    idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarExcepcionDesembolso);

if (existeExcepcion)
{
    // Hay excepción → verificar si ya completó
    bool excepcionCompletada = await _actividadesApplication.IsCompleteActivity(
        idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarExcepcionDesembolso);
    
    if (excepcionCompletada)
    {
        // Ambas rutas completaron → crear Validar Condiciones Desembolso
        await AvanzarAValidarCondiciones(transitions, folio, userId);
    }
    else
    {
        // Excepción aún en curso → no crear nada
        _bitacoraApplication.Create(..., 
            "VB Final completado. Esperando Excepción Desembolso.");
    }
}
else
{
    // No hay excepción → la ruta corta no aplica → avanzar directo
    await AvanzarAValidarCondiciones(transitions, folio, userId);
}
```

### Regla: "El último en llegar crea la actividad siguiente"

| Escenario | Quién crea Validar Condiciones |
|-----------|-------------------------------|
| Excepción termina primero, VB Final después | VB Final Abogado |
| VB Final termina primero, Excepción después | Excepción Desembolso |
| No aplica excepción | VB Final Abogado (directo) |
| Excepción → VoBo Gerencia → y luego VB Final | VoBo Gerencia COH (si VB Final ya completó) o VB Final (si Gerencia ya completó) |

---

## 4. Datos heredados (CA02)

| Origen | Datos visibles |
|--------|---------------|
| Firmar Rep. Legal | Datos cliente, notaría, concepto firma |
| Realizar Recepción Boleta | + Fecha ingreso, radicado, tipo boleta, oficina |

El sistema detecta automáticamente el origen (CA07) verificando si existe registro en `realizar_recepcion_boleta`.

---

## 5. Constantes

```csharp
// Actividades
public const string EscrituracionRealizarExcepcionDesembolso = "BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO";
public const string EscrituracionVoBoGerenciaCOH = "BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH";

// Transiciones
public const string ExcepcionDesembolsoVoBoGerencia = "BBVA_ESCRITURACION_TR_EXCEPCION_VOBO_GERENCIA";
public const string ExcepcionDesembolsoValidarCondiciones = "BBVA_ESCRITURACION_TR_EXCEPCION_VALIDAR_CONDICIONES";
```

---

## 6. Scripts workflow

```sql
-- cat_actividades_ws
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Excepción Desembolso', 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_excepcion_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO');

-- Destino: VoBo Gerencia COH
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar VoBo Gerencia COH', 'BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_vobo_gerencia_coh', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH');

-- xpdl_transitions
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_EXCEPCION_VOBO_GERENCIA', 'BBVA_ESCRITURACION_TR_EXCEPCION_VOBO_GERENCIA',
       'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_EXCEPCION_VOBO_GERENCIA');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_EXCEPCION_VALIDAR_CONDICIONES', 'BBVA_ESCRITURACION_TR_EXCEPCION_VALIDAR_CONDICIONES',
       'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_EXCEPCION_VALIDAR_CONDICIONES');
```

---

## 7. Frontend — Wireframe

```
┌──────────────────────────────────────────────────────────┐
│ Título: "Realizar Excepción Desembolso"                  │
├──────────────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente                   │
├──────────────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales                      │
├──────────────────────────────────────────────────────────┤
│ Acordeón 3: Realizar Excepción Desembolso                │
│                                                          │
│   ┌─ Datos Heredados (solo lectura) ───────────────────┐│
│   │  Datos Cliente | Notaría | Boleta (si aplica)      ││
│   └────────────────────────────────────────────────────┘│
│                                                          │
│   ┌─ Autorización ────────────────────────────────────┐ │
│   │  Excepción Autorizada: Dropdown SI/NO *           │ │
│   │  ☐ Confirmar continuación con excepción *         │ │
│   │  ¿Requiere VoBo Gerencia?: Dropdown SI/NO *       │ │
│   │  Observaciones: textarea (opcional)               │ │
│   └────────────────────────────────────────────────────┘│
│                                                          │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]         │
└──────────────────────────────────────────────────────────┘
```

---

## 8. Archivos a crear

| # | Capa | Archivo |
|---|---|---|
| 1 | Entity | `realizar_excepcion_desembolso_entity.cs` |
| 2 | Entity Config | `realizar_excepcion_desembolso_entity_config.cs` |
| 3 | Repo Interface | `IRealizarExcepcionDesembolsoRepository.cs` |
| 4 | Repo Impl | `RealizarExcepcionDesembolsoRepository.cs` |
| 5 | Domain Model | `realizar_excepcion_desembolso.cs` |
| 6 | App Interface | `IRealizarExcepcionDesembolsoApplication.cs` |
| 7 | App Impl | `RealizarExcepcionDesembolsoApplication.cs` |
| 8 | Controller | `RealizarExcepcionDesembolsoController.cs` |
| 9 | Frontend | model, service, hooks, page, route |

---

## 9. Nota sobre VoBo Gerencia COH (BBV-141)

Si `requiere_vobo_gerencia == "SI"`, el flujo va a VoBo Gerencia COH. Esa actividad al completar debe aplicar la misma lógica de AND-JOIN:

```csharp
// Al completar VoBo Gerencia COH:
bool vbFinalCompletado = await _actividadesApplication.IsCompleteActivity(
    idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarVBFinalAbogado);

if (vbFinalCompletado)
    → Crear Validar Condiciones Desembolso
else
    → Esperar (no crear nada)
```
