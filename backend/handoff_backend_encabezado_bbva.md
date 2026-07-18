## CONTEXTO DEL PROYECTO

Eres un desarrollador senior backend (.NET) trabajando en **cglatam.core.col.bbvalegalizacion.backend**,
el API que consume el frontend **BBVA Colombia — Presto Legalización**
(`cglatam.core.col.bbvalegalizacion.frontend`).

El frontend tiene una sección compartida llamada **"Información del Expediente"**
(componente `EncabezadoActividad`, usado en ~82 pantallas de actividad) que
consulta:

```
GET /api/Encabezado/infoEncabezado/{idExpediente}
```

(el `activityID` ahora se envía como **query param opcional**: `?activityID=...`,
ver "Cambio de contrato" más abajo).

## PROBLEMA A RESOLVER

Hoy ese endpoint devuelve datos con forma/terminología de la versión Chile del
producto (rut, banco_alzante, moneda, UF, pesos, etc.) y **no** trae poblados
los campos BBVA Colombia (scoring, titulares, oficina, subproducto, inmueble...),
aunque esos datos sí existen y se leen/escriben correctamente a través de la
API de **Carga Operación Banco**, que ya funciona bien:

```
GET  /api/CargaOperacionBanco/GetByExpediente/{id_expediente}
GET  /api/CargaOperacionBanco/GetDatosOperacionByExpediente/{id_expediente}
GET  /api/CargaOperacionBanco/GetAntecedentesCompradorByExpediente/{id_expediente}
GET  /api/CargaOperacionBanco/GetAntecedenteCreditoByExpediente/{id_expediente}
GET  /api/CargaOperacionBanco/GetDatosComercialByExpediente/{id_expediente}
POST /api/CargaOperacionBanco/Save
```

**Objetivo:** que `infoEncabezado` obtenga sus campos BBVA de las mismas tablas
que usa `CargaOperacionBanco` (join/lookup por `id_expediente`), en vez de
depender de un `activityID` de workflow — ese `activityID` es poco confiable:
varias pantallas del frontend lo mandan vacío (`""`) o con un GUID placeholder
reutilizado entre actividades no relacionadas (aún no configurado en el motor
de workflow).

## CAMBIO DE CONTRATO DEL ENDPOINT

**Antes:**
```
GET /api/Encabezado/infoEncabezado/{idExpediente}/{activityID}
```
`activityID` era obligatorio en la ruta.

**Ahora (ya aplicado en el frontend):**
```
GET /api/Encabezado/infoEncabezado/{idExpediente}
GET /api/Encabezado/infoEncabezado/{idExpediente}?activityID={activityID}
```
`activityID` pasa a ser **query param opcional**. Cuando venga, se puede seguir
usando para resolver `actividad` / `estado` / `usuario_asignado` como hoy.
Cuando **no** venga (o venga vacío), esos tres campos deberán resolverse de
la mejor forma posible sin él (p. ej. última tarea/actividad activa del
expediente) — los campos BBVA de abajo **nunca deben depender de este parámetro**.

## QUÉ DEBE DEVOLVER (`EncabezadoDTO`, ya actualizado en frontend)

```typescript
export interface EncabezadoDTO {
  id_expediente: number;
  actividad: string;
  estado: string;
  usuario_asignado: string;
  fecha_alta?: string | null;
  fecha_asignacion?: string | null;

  // ============================================================
  // CAMPOS BBVA COLOMBIA
  // ============================================================
  id_scoring?: string | null;

  tipo_documento_id_t1?: string | null;
  numero_identificacion_t1?: string | null;
  nombre_completo_t1?: string | null;
  celular_t1?: string | null;

  numero_identificacion_t2?: string | null;
  nombre_completo_t2?: string | null;

  cliente_nomina?: boolean | null;
  situacion_laboral?: string | null;
  correo_declarativo?: string | null;
  telefono_declarativo?: string | null;

  codigo_oficina_bbva?: string | null;
  descripcion_oficina_bbva?: string | null;
  codigo_asesor_bbva?: string | null;

  id_tipo_sub_producto?: string | null;
  monto_otorgado?: number | null;
  canal_originacion?: string | null;
  fecha_aprobacion?: string | null;

  codigo_proyecto?: string | null;
  descripcion_proyecto?: string | null;
  estado_inmueble?: string | null;
  tipo_inmueble?: string | null;
  condiciones_organismo_decisor?: string | null;
}
```

Respuesta envuelta en el `ApiResponse<T>` estándar del proyecto:
```typescript
export interface ApiResponse<T> {
  status: boolean;
  detail: T;
  message?: string;
}
```

## MAPEO DE CAMPOS (fuente de verdad: tablas de `carga_operacion_banco`)

Todas las búsquedas son por `id_expediente` (mismo criterio que ya usa
`CargaOperacionBancoController`).

**Desde `carga_operacion_banco_datos_operacion`:**

| Origen (`CargaOperacionBancoDatosOperacion`) | Destino (`EncabezadoDTO`) |
|---|---|
| `id_scoring` | `id_scoring` |
| `codigo_asesor` | `codigo_asesor_bbva` |
| `codigo_oficina` | `codigo_oficina_bbva` |
| `descripcion_oficina` | `descripcion_oficina_bbva` |
| `canal_originacion` | `canal_originacion` |
| `tipo_inmueble` | `tipo_inmueble` |
| `estado_inmueble` | `estado_inmueble` |
| `codigo_proyecto` | `codigo_proyecto` |
| `descripcion_proyecto` | `descripcion_proyecto` |

**Desde `carga_operacion_banco_antecedente_comprador` (lista, filtrar `row_status = true` / `is_active = true`):**

| Origen | Destino |
|---|---|
| Registro 1 → `tipo_documento_id` | `tipo_documento_id_t1` |
| Registro 1 → `numero_identificacion` | `numero_identificacion_t1` |
| Registro 1 → `nombre_completo` | `nombre_completo_t1` |
| Registro 1 → `celular` | `celular_t1` |
| Registro 1 → `situacion_laboral` | `situacion_laboral` |
| Registro 1 → `cliente_nomina` | `cliente_nomina` |
| Registro 2 (si existe) → `numero_identificacion` | `numero_identificacion_t2` |
| Registro 2 (si existe) → `nombre_completo` | `nombre_completo_t2` |

> ⚠️ **Decisión pendiente de confirmar por el equipo backend:** ¿cómo se determina
> cuál registro es "Titular 1" y cuál "Titular 2"? Propuesta: ordenar por
> `id_carga_operacion_banco_antecedente_comprador` ascendente (orden de alta).
> Si existe algún campo de rol/orden explícito en la tabla, usar ese en su lugar.

**Desde `carga_operacion_banco_antecedente_credito`:**

| Origen | Destino |
|---|---|
| `id_tipo_sub_producto` | `id_tipo_sub_producto` |
| `monto_otorgado` | `monto_otorgado` |
| `fecha_aprobacion` | `fecha_aprobacion` |
| `condiciones_organismo_decisor` | `condiciones_organismo_decisor` |

**Desde `carga_operacion_banco_datos_comercial`:**

| Origen | Destino |
|---|---|
| `correo_declarativo_cliente` | `correo_declarativo` |
| `numero_telefono_declarativo` | `telefono_declarativo` |

**Campos genéricos (sin cambios, resolución actual del endpoint):**

`id_expediente`, `actividad`, `estado`, `usuario_asignado`, `fecha_alta`, `fecha_asignacion`.

## CRITERIOS DE ACEPTACIÓN

1. `GET /api/Encabezado/infoEncabezado/26` (sin `activityID`) devuelve los
   campos BBVA poblados para el expediente 26, con los mismos valores que
   hoy devuelve `GET /api/CargaOperacionBanco/GetByExpediente/26` y sus
   endpoints relacionados.
2. `GET /api/Encabezado/infoEncabezado/26?activityID=<guid-real-o-invalido>`
   devuelve los mismos campos BBVA que el punto 1 (no deben verse afectados
   por un `activityID` inválido/placeholder).
3. Si el expediente no tiene registro en `carga_operacion_banco` (aún no pasó
   por esa actividad), los campos BBVA deben venir en `null`, no debe fallar
   la consulta.
4. Los campos Chile legados (rut, banco_alzante, moneda, tasa, plazo, etc.) ya
   fueron **eliminados del contrato** — no es necesario seguir calculándolos
   ni devolviéndolos.

## ARCHIVOS FRONTEND YA MODIFICADOS (referencia, no tocar desde backend)

```
src/features/encabezado/models/encabezado.ts        → EncabezadoDTO limpio (sin campos Chile)
src/features/encabezado/api/encabezadoService.ts     → activityID como query param opcional
src/features/encabezado/hooks/useEncabezado.ts       → enabled solo depende de idExpediente
src/features/encabezado/pages/EncabezadoActividad.tsx → render solo de campos BBVA + metadata genérica
```
