# Resumen de Cambio — CA11 Firmar Escritura Cliente (Flujo Cíclico)

> Corrección del enrutamiento cuando el caso retorna desde Devolución EP.

---

## Problema detectado

Al avanzar desde **Firmar Escritura Cliente** por segunda vez (después de pasar por Devolución EP), el sistema retorna:

```json
{"status": false, "detail": null, "message": "No se pudo avanzar la actividad."}
```

### Causa raíz

La guarda CA11 usa `IsCompleteActivity` que consulta el SP:

```sql
-- usp_is_complete_activity
SELECT EXISTS (
  SELECT 1 FROM actividades
  WHERE id_expediente = p_id_expediente
  AND id_actividad = p_id_actividad
  AND status IN ('Completada')
);
```

Este SP retorna `true` **permanentemente** una vez que la actividad se completó históricamente. En el segundo ciclo (post-devolución), como Revisar EP ya se completó en el primer ciclo, la guarda impide re-crearla. Si el producto no es CXI ni Leasing, no se dispara ninguna transición → lista vacía → "No se pudo avanzar".

### Flujo que falla

```
Ciclo 1: Firmar Escritura → Revisar EP (se crea) → ... → Devolución EP
Ciclo 2: Devolución EP → Firmar Escritura → Avanzar → IsComplete(RevisarEP)=true → NO CREA NADA → ERROR
```

---

## Solución propuesta

Reemplazar `IsCompleteActivity` por `ExisteActividadActiva` en el bloque else del método `Avanzar`.

### Justificación

| Método | Pregunta que responde | Efecto |
|--------|----------------------|--------|
| `IsCompleteActivity` | ¿Alguna vez se completó esta actividad? | Bloquea para siempre después del primer ciclo |
| `ExisteActividadActiva` | ¿Hay una actividad en curso AHORA MISMO? | Solo bloquea si hay un duplicado vivo |

`ExisteActividadActiva` **ya existe** en el proyecto y se usa en:
- `ValidarCondicionesDesembolsoApplication.cs`
- `WorkflowApplication.cs` (motor de workflow)
- `AsignarFirmasApplication.cs`

### Interpretación de CA11

La HU BBV-86 dice:
> *"El sistema debe identificar si existe un retorno desde alguna parte del flujo y si previamente se avanzó buscando el resultado de concepto. La finalidad es evitar volver a escalar al área correspondiente."*

La intención es **evitar duplicar una actividad que ya está en curso**, NO bloquear permanentemente el re-envío después de una devolución. Cuando Devolución EP retorna el caso, es un nuevo ciclo de revisión.

---

## Archivo a modificar

```
backend/Multibanca.Application.Implementations/Multibanca/BBVA/Escrituracion/FirmarEscrituraClienteApplication.cs
```

### Cambio (3 líneas en el bloque else del método Avanzar)

**ANTES:**
```csharp
// Línea ~156
bool tieneConceptoEP = await _actividadesApplication.IsCompleteActivity(idExpediente, ActividadRevisarEP);
if (!tieneConceptoEP)

// Línea ~171
bool tieneConceptoProrrata = await _actividadesApplication.IsCompleteActivity(idExpediente, ActividadVBProrrata);
if (!tieneConceptoProrrata)

// Línea ~195
bool tieneConceptoCausacion = await _actividadesApplication.IsCompleteActivity(idExpediente, ActividadCausacion);
if (!tieneConceptoCausacion)
```

**DESPUÉS:**
```csharp
// Línea ~156
bool hayRevisarEPActiva = await _actividadesApplication.ExisteActividadActiva(idExpediente, ActividadRevisarEP);
if (!hayRevisarEPActiva)

// Línea ~171
bool hayProrrataActiva = await _actividadesApplication.ExisteActividadActiva(idExpediente, ActividadVBProrrata);
if (!hayProrrataActiva)

// Línea ~195
bool hayCausacionActiva = await _actividadesApplication.ExisteActividadActiva(idExpediente, ActividadCausacion);
if (!hayCausacionActiva)
```

---

## Comportamiento esperado después del cambio

| Escenario | ExisteActividadActiva | Resultado |
|-----------|----------------------|-----------|
| Primera vez (Revisar EP no existe) | `false` | Crea Revisar EP ✅ |
| Revisar EP en curso (evitar duplicado real) | `true` | No la duplica ✅ |
| Post-devolución (Revisar EP completada, no activa) | `false` | Crea nueva Revisar EP ✅ |

---

## Impacto

- **Archivos modificados:** 1 (FirmarEscrituraClienteApplication.cs)
- **Métodos nuevos:** 0 (ExisteActividadActiva ya existe)
- **Cambios en BD:** 0
- **Cambios en frontend:** 0
- **Riesgo:** Bajo. Solo afecta la guarda de duplicados en Firmar Escritura Cliente. El método ExisteActividadActiva ya está probado en producción en otros flujos.

---

## Casos de prueba

1. **Primer ciclo normal:** Firmar Escritura → Avanzar (sin escalamiento) → debe crear Revisar EP ✅
2. **Duplicado en curso:** Si por algún motivo Revisar EP está activa y se intenta avanzar → NO debe duplicar ✅
3. **Flujo cíclico (el bug):** Firmar Escritura → Revisar EP → Devolución EP → Firmar Escritura → Avanzar → debe crear nueva Revisar EP ✅
4. **Con CXI:** Avanzar debe crear Revisar EP + VB Prorrata (si no están activas) ✅
5. **Con Leasing + causar:** Avanzar debe crear Revisar EP + Causación (si no están activas) ✅
