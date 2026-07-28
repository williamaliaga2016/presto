# BBV-90 — Receta de Implementación: Realizar Devolución EP

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Analista de Vivienda.
> Bandeja centralizadora de rechazos con enrutamiento dinámico (4 destinos).

---

## Flujo

```
Firmar Rep. Legal (BBV-91, concepto NO firmada) ───┐
Realizar VB Final Abogado (BBV-96, requiere dev.) ─┼──→ Realizar Devolución EP ← ESTA HU
                                                    │
    ├─ SI ¿Escalamiento Comercial? → "Realizar Gestión Comercial" (Comercial)
    │
    └─ NO → Acción a Seguir:
             ├─ Firmar Escritura → "Firmar Escritura Cliente"
             ├─ Firmar Rep. Legal → "Firmar Rep. Legal"
             └─ EP Registradas → "Realizar EP Registradas"
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv90_realizar_devolucion_ep.sql`

### 1.1 Tabla

```sql
CREATE TABLE IF NOT EXISTS public.realizar_devolucion_ep (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100),
    requiere_escalamiento_comercial VARCHAR(2),
    tipologia                       VARCHAR(200),
    casuistica                      VARCHAR(200),
    accion_a_seguir                 VARCHAR(100),
    observaciones                   VARCHAR(500),
    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);
```

### 1.2 Catálogo reutilizado

> Tipología de escalamiento usa `TIPOLOGIA_ESCALAMIENTO` (misma paramétrica L4 que BBV-86).
> No se crea catálogo nuevo. El endpoint `/controles` retorna ese catálogo.
> La HU BBV-90 menciona "Casuísticas" pero no define paramétrica — se omite por ahora.

---

## 2. Backend — Archivos creados

| # | Capa | Archivo |
|---|---|---|
| 1 | Entity | `realizar_devolucion_ep_entity.cs` |
| 2 | Entity Config | `realizar_devolucion_ep_entity_config.cs` |
| 3 | Repo Interface | `IRealizarDevolucionEPRepository.cs` |
| 4 | Repo Impl | `RealizarDevolucionEPRepository.cs` |
| 5 | Domain Model | `realizar_devolucion_ep.cs` |
| 6 | App Interface | `IRealizarDevolucionEPApplication.cs` |
| 7 | App Impl | `RealizarDevolucionEPApplication.cs` |
| 8 | Controller | `RealizarDevolucionEPController.cs` |

---

## 3. Backend — Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/realizar-devolucion-ep/GetByIdExpediente/{id_expediente}` | Consulta + datos heredados (conceptos rechazo) |
| GET | `/api/realizar-devolucion-ep/controles` | Catálogo tipología escalamiento |
| POST | `/api/realizar-devolucion-ep/Save` | Crea o actualiza |
| POST | `/api/realizar-devolucion-ep/avanzar/{id_expediente}` | Valida + enruta (4 destinos) |

---

## 4. Backend — Lógica de Avanzar

### 4.1 Constantes

```csharp
// En TransicionesBBVA
public const string DevolucionEPGestionComercial = "BBVA_ESCRITURACION_TR_DEVOLUCION_EP_GESTION_COMERCIAL";
public const string DevolucionEPFirmarEscritura = "BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_ESCRITURA";
public const string DevolucionEPFirmarRepLegal = "BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_REP_LEGAL";
public const string DevolucionEPEPRegistradas = "BBVA_ESCRITURACION_TR_DEVOLUCION_EP_EP_REGISTRADAS";
```

### 4.2 Enrutamiento (CA03/CA04)

```
SI escalamiento == "SI" → Gestión Comercial
SI escalamiento == "NO" → según accion_a_seguir:
    "FIRMAR_ESCRITURA" → Firmar Escritura Cliente
    "FIRMAR_REP_LEGAL" → Firmar Rep. Legal
    "EP_REGISTRADAS" → Realizar EP Registradas
```

### 4.3 Campos mutuamente excluyentes (CA06)

- SI escalamiento: muestra Tipología + Casuística, oculta Acción a Seguir
- NO escalamiento: muestra Acción a Seguir, oculta Tipología + Casuística

---

## 5. Frontend — Archivos creados

| # | Archivo |
|---|---|
| 1 | `models/realizar_devolucion_ep.ts` (incluye `ACCIONES_A_SEGUIR`) |
| 2 | `api/realizarDevolucionEPService.ts` |
| 3 | `hooks/useRealizarDevolucionEP.ts` |
| 4 | `hooks/useUpsertRealizarDevolucionEP.ts` |
| 5 | `hooks/useAvanzarRealizarDevolucionEP.ts` |
| 6 | `pages/realizar_devolucion_ep_page.tsx` |
| 7 | `routes/Routes.tsx` (ruta: `realizar_devolucion_ep/:id_expediente`) |

---

## 6. Frontend — Wireframe

```
┌──────────────────────────────────────────────────────────┐
│ Título: "Realizar Devolución EP"                         │
├──────────────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente                   │
├──────────────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales                      │
├──────────────────────────────────────────────────────────┤
│ Acordeón 3: Realizar Devolución EP                       │
│                                                          │
│   ┌─ Conceptos de Rechazo (fondo rojo, readonly) ──────┐│
│   │  Tipología rechazo: [del VB Final Abogado]         ││
│   │  Casuística rechazo: [del VB Final Abogado]        ││
│   │  Observaciones rechazo: [del VB Final Abogado]     ││
│   └────────────────────────────────────────────────────┘│
│                                                          │
│   ┌─ Decisión de Enrutamiento ─────────────────────────┐│
│   │  ¿Requiere Escalamiento?: SwitchForm (SI/NO) *     ││
│   │                                                    ││
│   │  [Si SI]:                                          ││
│   │    Tipología: Dropdown *                           ││
│   │    Casuística: Dropdown                            ││
│   │                                                    ││
│   │  [Si NO]:                                          ││
│   │    Acción a Seguir: Dropdown *                     ││
│   │      • Firmar Escritura Cliente                    ││
│   │      • Firmar Rep. Legal                           ││
│   │      • Realizar EP Registradas                     ││
│   │                                                    ││
│   │  Observaciones: textarea (opcional)                ││
│   └────────────────────────────────────────────────────┘│
│                                                          │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]         │
└──────────────────────────────────────────────────────────┘
```

---

## 7. Datos heredados (CA02)

| Campo | Fuente | Tabla |
|---|---|---|
| Tipología rechazo | VB Final Abogado | `realizar_vb_final_abogado.tipologia` |
| Casuística rechazo | VB Final Abogado | `realizar_vb_final_abogado.casuistica` |
| Observaciones rechazo | VB Final Abogado | `realizar_vb_final_abogado.observaciones` |

---

## 8. Script de Workflow (DBWFBBVA)

```sql
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_GESTION_COMERCIAL', 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_GESTION_COMERCIAL', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_GESTION_COMERCIAL');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_ESCRITURA', 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_ESCRITURA', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_ESCRITURA');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_REP_LEGAL');

INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_EP_REGISTRADAS', 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_EP_REGISTRADAS', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_DEVOLUCION_EP_EP_REGISTRADAS');
```

---

## 9. Notas de implementación

1. **Campos mutuamente excluyentes (CA06):** El frontend usa un flag `esEscalamiento` que muestra/oculta los bloques. Al cambiar el toggle, se limpian los campos del bloque oculto.

2. **Acción a Seguir:** Hardcoded en frontend como array de opciones (no viene de catálogo). Los codes coinciden con la lógica del switch en el backend. **Opciones según HTML de referencia:** Firmar Escritura Cliente, Firmar Rep. Legal, Realizar VB Final Abogado.

3. **Datos heredados:** Se consultan de `realizar_vb_final_abogado` — los conceptos de rechazo que el abogado registró al devolver la EP.

4. **Tipología escalamiento:** Reutiliza el catálogo `TIPOLOGIA_ESCALAMIENTO` (L4) que ya existe (mismo de BBV-86).

5. **Reutilización:**
   - Patrón Application/Repository/Controller: idéntico a BBV-97
   - `SwitchForm`: componente compartido
   - Lógica de enrutamiento múltiple: mismo patrón que BBV-96/BBV-97

---

## 10. TODO — Otros orígenes posibles

Según el diagrama `BBVA_Legalizacion.drawio` (página Escrituración y Garantías), **Realizar Devolución EP** recibe casos de **4 orígenes confirmados dentro de Escrituración**:

| # | Origen | Tabla consultada | Campos para grilla |
|---|--------|-----------------|-------------------|
| 1 | Firmar Rep. Legal (BBV-91) | `firmar_rep_legal` | tipologia, casuistica, observaciones |
| 2 | Realizar VB Final Abogado (BBV-96) | `realizar_vb_final_abogado` | tipologia, casuistica, observaciones |
| 3 | Revisar EP Abogado (BBV-130) | `revisar_ep_abogado` | tipologia, casuistica, observaciones_legales |
| 4 | Realizar Gestión Comercial | `realizar_gestion_comercial` | observaciones (sin tipología/casuística) |

La grilla "Conceptos / Dictámenes Previos" muestra una fila por cada origen con las columnas:
**ÁREA | TIPOLOGÍA | CASUÍSTICA DE RECHAZO | OBSERVACIONES**

### Acciones a Seguir (destinos de salida)

| Code | Descripción | Transición |
|------|-------------|------------|
| `FIRMAR_ESCRITURA` | Firmar Escritura Cliente | `BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_ESCRITURA` |
| `FIRMAR_REP_LEGAL` | Firmar Rep. Legal | `BBVA_ESCRITURACION_TR_DEVOLUCION_EP_FIRMAR_REP_LEGAL` |
| `VB_FINAL_ABOGADO` | Realizar VB Final Abogado | `BBVA_ESCRITURACION_TR_DEVOLUCION_EP_VB_FINAL_ABOGADO` |
