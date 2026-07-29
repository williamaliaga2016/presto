# BBV-91 — Receta de Implementación v2: Firmar Rep. Legal

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Representante Legal.
> Versión actualizada con flujo paralelo completo según diagrama y HU.

---

## Flujo Completo

```
Firmar Rep. Legal → Avanzar
    │
    ├─ CRL-2 (Escritura NO firmada)
    │     → Realizar Devolución EP (Analista Vivienda)
    │     [1 destino lineal]
    │
    └─ CRL-1 (Escritura firmada Conforme)
          → EN PARALELO:
          │
          ├─ SIEMPRE → Realizar Entrega EP Firmada (Analista Vivienda)
          ├─ SIEMPRE → Preformalizar (Analista Desembolso)
          │
          └─ CONDICIONAL → Realizar Excepción Desembolso (Comercial)
                           Solo si aplica (ver lógica abajo)
```

---

## 1. Lógica de "¿Aplica Excepción Desembolso?"

Se evalúa en backend al avanzar. No requiere intervención del usuario.

```csharp
// Pseudocódigo
bool aplicaExcepcion = false;

// Paso 1: Obtener código_proyecto del expediente (de validar_informacion_bbva)
string? codigoProyecto = validarInfo?.codigo_proyecto;

// Paso 2: Buscar en tabla TRADICIONES_CONOCIDAS por código_proyecto
var tradicion = await _tradicionesRepository.GetByCodigoProyecto(codigoProyecto);

// Paso 3: Verificar que tipo_desembolso = "ESCRITURA"
if (tradicion != null && tradicion.tipo_desembolso == "ESCRITURA")
{
    // Paso 4: Verificar que tipo_credito del expediente esté en la lista
    string? tipoCredito = validarInfo?.tipo_credito;
    aplicaExcepcion = TiposExcepcionDesembolso.Contains(tipoCredito);
}
```

### Constante de tipos de crédito que aplican:

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
```

---

## 2. Tabla nueva: tradiciones_conocidas

```sql
CREATE TABLE IF NOT EXISTS public.tradiciones_conocidas (
    id                          BIGSERIAL PRIMARY KEY,
    ciudad_del_inmueble         VARCHAR(200),
    codigo_proyecto             VARCHAR(50) NOT NULL,
    proyecto                    VARCHAR(200),
    constructora                VARCHAR(200),
    firma                       VARCHAR(200),
    numero_identificacion_firma VARCHAR(50),
    nit_abogado                 VARCHAR(50),
    nombre_abogado              VARCHAR(200),
    vip                         VARCHAR(10),
    fecha_creacion_modificacion TIMESTAMP WITHOUT TIME ZONE,
    proyecto_av_tipo            VARCHAR(100),
    tipo_desembolso             VARCHAR(50),
    correo_constructora         VARCHAR(200),
    is_active                   BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                  BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                  INTEGER NOT NULL DEFAULT 1,
    created_date                TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_tradiciones_conocidas_codigo_proyecto
    ON public.tradiciones_conocidas (codigo_proyecto)
    WHERE is_active = true AND row_status = true;

-- SP: Consulta por código de proyecto (trae todos los datos)
CREATE OR REPLACE FUNCTION public.usp_select_tradiciones_conocidas_by_codigo_proyecto(
    p_codigo_proyecto VARCHAR)
RETURNS SETOF public.tradiciones_conocidas
LANGUAGE sql
STABLE
AS $$
    SELECT *
    FROM public.tradiciones_conocidas
    WHERE codigo_proyecto = p_codigo_proyecto
      AND is_active = TRUE
      AND row_status = TRUE
    ORDER BY id DESC
    LIMIT 1;
$$;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.tradiciones_conocidas TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.tradiciones_conocidas_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_tradiciones_conocidas_by_codigo_proyecto(VARCHAR) TO multibanca;
```

---

## 3. Scripts de cat_actividades_ws (destinos)

```sql
-- Firmar Rep. Legal (esta actividad)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Firmar Rep. Legal', 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'firmar_rep_legal', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL');

-- Destino 1: Realizar Devolución EP
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Devolución EP', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_devolucion_ep', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP');

-- Destino 2: Realizar Entrega EP Firmada
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Entrega EP Firmada', 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_entrega_ep_firmada', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA');

-- Destino 3: Preformalizar
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Preformalizar', 'BBVA_ESCRITURACION_PREFORMALIZAR', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'preformalizar', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_PREFORMALIZAR');

-- Destino 4: Realizar Excepción Desembolso
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Excepción Desembolso', 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_excepcion_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO');
```

---

## 4. Scripts de xpdl_transitions (workflow)

```sql
-- Transición 1: NO firmada → Devolución EP
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_DEVOLUCION', 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_DEVOLUCION',
       'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_DEVOLUCION');

-- Transición 2: Firmada → Entrega EP Firmada (paralelo)
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP', 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP',
       'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP');

-- Transición 3: Firmada → Preformalizar (paralelo)
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_PREFORMALIZAR', 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_PREFORMALIZAR',
       'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_PREFORMALIZAR', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_PREFORMALIZAR');

-- Transición 4: Firmada + condición → Excepción Desembolso (paralelo condicional)
INSERT INTO public.xpdl_transitions (transition_id, name, from_activity, to_activity, condition, workflow_process_id)
SELECT 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_EXCEPCION_DESEMBOLSO', 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_EXCEPCION_DESEMBOLSO',
       'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL', 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'Otherwise', 'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (SELECT 1 FROM public.xpdl_transitions WHERE transition_id = 'BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_EXCEPCION_DESEMBOLSO');
```

---

## 5. Constantes nuevas (Constants.cs)

```csharp
// En ActividadesBBVA
public const string EscrituracionPreformalizar = "BBVA_ESCRITURACION_PREFORMALIZAR";
// EscrituracionRealizarExcepcionDesembolso ya existe

// En TransicionesBBVA — actualizar las existentes:
public const string FirmarRepLegalDevolucion = "BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_DEVOLUCION";
public const string FirmarRepLegalEntregaEP = "BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP";
public const string FirmarRepLegalPreformalizar = "BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_PREFORMALIZAR";
public const string FirmarRepLegalExcepcionDesembolso = "BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_EXCEPCION_DESEMBOLSO";

// Constante para tipo desembolso
public const string TipoDesembolsoEscritura = "ESCRITURA";
```

---

## 6. Backend — Lógica de Avanzar actualizada

```csharp
public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
{
    // ... validaciones ...

    if (formulario.concepto_firma == ConceptoNoFirmada)
    {
        // CRL-2 → Devolución EP (lineal)
        var transId = transitions.First(x => x.name == TransicionDevolucion).transition_id;
        actividadesCreadas.AddRange(await _workflowApplication.AvanzarActividad(transId, folio, userId));
    }
    else // CRL-1 — Firmada Conforme
    {
        // 1. SIEMPRE → Realizar Entrega EP Firmada
        var transEntrega = transitions.First(x => x.name == TransicionEntregaEP).transition_id;
        actividadesCreadas.AddRange(await _workflowApplication.AvanzarActividad(transEntrega, folio, userId));

        // 2. SIEMPRE → Preformalizar
        var transPreformalizar = transitions.First(x => x.name == TransicionPreformalizar).transition_id;
        actividadesCreadas.AddRange(await _workflowApplication.AvanzarActividad(transPreformalizar, folio, userId));

        // 3. CONDICIONAL → Realizar Excepción Desembolso
        if (await AplicaExcepcionDesembolso(idExpediente))
        {
            var transExcepcion = transitions.First(x => x.name == TransicionExcepcionDesembolso).transition_id;
            actividadesCreadas.AddRange(await _workflowApplication.AvanzarActividad(transExcepcion, folio, userId));
        }
    }

    // Bitácora ...
    return actividadesCreadas;
}

private async Task<bool> AplicaExcepcionDesembolso(long idExpediente)
{
    // Obtener código_proyecto del expediente
    var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
    string? codigoProyecto = validarInfo?.codigo_proyecto;
    if (string.IsNullOrEmpty(codigoProyecto)) return false;

    // Buscar en tradiciones_conocidas
    var tradicion = await _tradicionesRepository.GetByCodigoProyecto(codigoProyecto);
    if (tradicion == null) return false;

    // Verificar tipo_desembolso = "ESCRITURA"
    if (!string.Equals(tradicion.tipo_desembolso, TipoDesembolsoEscritura, StringComparison.OrdinalIgnoreCase))
        return false;

    // Verificar tipo_credito en la lista
    string? tipoCredito = validarInfo?.tipo_credito;
    return !string.IsNullOrEmpty(tipoCredito) 
        && TiposExcepcionDesembolso.Contains(tipoCredito, StringComparer.OrdinalIgnoreCase);
}
```

---

## 7. Archivos nuevos necesarios

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `tradiciones_conocidas_entity.cs` | Todas las columnas de la tabla |
| 2 | Entity Config | `tradiciones_conocidas_entity_config.cs` | `ToTable("tradiciones_conocidas")` |
| 3 | Repo Interface | `ITradicionesConocidasRepository.cs` | `GetByCodigoProyecto(string codigo)` — llama SP |
| 4 | Repo Impl | `TradicionesConocidasRepository.cs` | Usa `usp_select_tradiciones_conocidas_by_codigo_proyecto` |
| 5 | DbContext | Agregar `DbSet<tradiciones_conocidas_entity>` | |
| 6 | IoC | Registrar repository | |

---

## 8. Dependencias adicionales en FirmarRepLegalApplication

```csharp
// Agregar al constructor:
private readonly IValidarInformacionRepository _validarInformacionRepository;
private readonly ITradicionesConocidasRepository _tradicionesRepository;
```

---

## 9. Resumen de resultados al avanzar

| Concepto | Actividades creadas | Roles |
|----------|-------------------|-------|
| NO firmada | 1: Devolución EP | Analista Vivienda |
| Firmada (sin excepción) | 2: Entrega EP + Preformalizar | Analista Vivienda + Analista Desembolso |
| Firmada (con excepción) | 3: Entrega EP + Preformalizar + Excepción Desembolso | Analista Vivienda + Analista Desembolso + Comercial |
