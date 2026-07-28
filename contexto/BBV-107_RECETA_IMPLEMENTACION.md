# BBV-107 — Receta de Implementación: Revisar Marcación de Cobertura

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Analista de Cobertura.
> Actividad lineal sin ramificaciones — destino único: Validar Condiciones Desembolso (BBV-99).
> Se construye como **actividad independiente** (ver sección 0 — Vacíos detectados y decisiones).

---

## 0. Vacíos detectados en el proyecto y decisiones tomadas

Al revisar los "hermanos" de esta HU (BBV-94, BBV-99, BBV-141) y el resto del backend, se encontraron dos vacíos que impiden implementar el ticket al pie de la letra. Se decidió lo siguiente con el negocio/PO antes de codificar:

### 0.1 Compuerta de acceso (CA03)

El ticket exige que la pantalla solo se habilite si `Ciudad del Inmueble = Barranquilla` y `Aplica Subsidio = Mi Techo Propio`. Ninguno de los dos campos existe hoy en ningún modelo de dominio (`definir_inmueble_bbva`, `carga_operacion_banco*`, etc.), y la actividad predecesora **"Validar Cumplimiento de Políticas" (BBV-104)** — que sería quien evalúa esta compuerta y enruta hacia acá — todavía no está construida.

**Decisión:** BBV-107 se construye como actividad **independiente**, exactamente con el mismo patrón de BBV-94/HU-141 (su propia tabla, controller y pantalla). La compuerta real (Ciudad+Subsidio) queda pendiente hasta que exista BBV-104. Para pruebas/QA se agrega un script de posicionamiento manual (igual a `wr_test_expediente_208_posicionar_firmar_escritura.sql`) que ubica un expediente de prueba directamente en esta actividad, sin pasar por el motor de workflow.

Este mismo patrón de "registrar el stub de la actividad destino antes de que exista la pantalla" ya se usó en el script de BBV-94 (líneas 44-52 de `wr_bbv94_excepcion_desembolso.sql`), donde se pre-registraron en `cat_actividades_ws` tanto "Realizar Vobo Gerencia COH" como "Validar Condiciones Desembolso" antes de que existieran sus controllers.

### 0.2 Envío de correo (CA05/CA08)

No existe en todo el backend ninguna capacidad de envío de correo (sin SMTP, sin `MailKit`, sin `IEmailService`, nada). El único hallazgo relacionado es un claim JWT llamado `"email"` en `LoginController.cs`.

**Decisión:** Se implementa completo el flujo de validación y bloqueo (CA05/CA08: campo obligatorio, formato válido, bloqueo de "Avanzar" si falta o es inválido), pero detrás de una interfaz nueva `IEmailNotificationApplication` cuya implementación **solo registra en log** (no envía correo real). Cuando se disponga de credenciales SMTP, se reemplaza la implementación sin tocar el `Avanzar()` que la invoca.

---

## Flujo

```
Validar Cumplimiento de Políticas (BBV-104, aún no existe)
    → [Compuerta: Ciudad=Barranquilla AND Subsidio=Mi Techo Propio] → pendiente (ver 0.1)
        → Revisar Marcación de Cobertura ← ESTA HU
            → Siempre (único destino): "Validar Condiciones Desembolso" (Analista de Vivienda)
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv107_revisar_marcacion_cobertura.sql`

### 1.1 Tabla

```sql
-- BBV-107 — Revisar Marcación de Cobertura (Escrituración y Garantías)
-- Script idempotente

CREATE TABLE IF NOT EXISTS public.revision_marcacion_cobertura_bbva (
    id                              BIGSERIAL PRIMARY KEY,
    id_expediente                   BIGINT NOT NULL,
    id_actividad                    VARCHAR(100) NOT NULL,

    -- CA05 — Notificación a Colocaciones (bloqueante, CA08)
    email_area_colocaciones         VARCHAR(150),

    -- Identificación (Precargado Editable salvo consecutivo)
    consecutivo                     VARCHAR(50),
    tipo_documento                  VARCHAR(10),
    numero_documento                VARCHAR(30),
    tipo_tramite                    VARCHAR(30),
    nombre                          VARCHAR(200),

    -- Proyecto / Constructora (Diligencia SITCAR)
    constructora                    VARCHAR(200),
    proyecto                        VARCHAR(200),
    fecha_aceptacion_plataforma     DATE,
    tipo_vivienda                   VARCHAR(50),

    -- Valores y obligación
    valor_subsidio                  NUMERIC(18,2),
    numero_obligacion               VARCHAR(50),
    fecha_desembolso                DATE,
    fecha_proximo_canon             DATE,
    valor_desembolso                NUMERIC(18,2),
    intereses_corrientes            NUMERIC(18,2),
    capital                         NUMERIC(18,2),
    seguros                         NUMERIC(18,2),
    cuota_mensual                   NUMERIC(18,2),
    plazo                           INTEGER,
    observacion                     VARCHAR(1000),

    -- Solicitud / Respuesta de marcación (SITCAR)
    fecha_solicitud_marcacion       DATE,
    hora_solicitud_marcacion        VARCHAR(5),   -- formato HH:MM
    fecha_respuesta_marcacion       DATE,          -- condicionado
    hora_respuesta_marcacion        VARCHAR(5),    -- condicionado, formato HH:MM
    responsable_m5                  VARCHAR(150),

    -- Resolución
    no_resolucion                   VARCHAR(50),
    fecha_resolucion                DATE,
    fecha_envio_resolucion          DATE,
    estado_proceso                  VARCHAR(50),

    -- CA07 — Observaciones generales
    observaciones                   VARCHAR(1000),

    is_active                       BOOLEAN NOT NULL DEFAULT TRUE,
    row_status                      BOOLEAN NOT NULL DEFAULT TRUE,
    created_by                      INTEGER NOT NULL,
    created_date                    TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by                     INTEGER,
    modified_date                   TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_revision_marcacion_cobertura_bbva_expediente
    ON public.revision_marcacion_cobertura_bbva (id_expediente)
    WHERE is_active = true AND row_status = true;

-- Permisos
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.revision_marcacion_cobertura_bbva TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.revision_marcacion_cobertura_bbva_id_seq TO multibanca;
```

> **NO se necesita SP para insert/update.** Se usa EF Core con `Create()`/`Update()` del
> `MultibancaGenericApplication` (mismo patrón que HU-141/BBV-94; no todas las HUs recientes usan SP).

### 1.2 Registro en cat_actividades_ws (bandeja)

Esta es una actividad **nueva independiente** (ver 0.1) — no fue pre-registrada por ningún script anterior, así que este script debe registrarla:

```sql
-- Revisar Marcación de Cobertura (esta HU)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Revisar Marcación de Cobertura', 'BBVA_ESCRITURACION_REVISAR_MARCACION_COBERTURA', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'revisar_marcacion_cobertura', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REVISAR_MARCACION_COBERTURA');

-- Destino: Validar Condiciones Desembolso (ya pre-registrada por el script de BBV-94, este INSERT es idempotente por si acaso)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Validar Condiciones Desembolso', 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'validar_condiciones_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO');
```

### 1.3 Script de posicionamiento de prueba (QA/dev, ver 0.1)

**Archivo:** `backend/database/escrituracion/wr_test_expediente_bbv107_posicionar_marcacion_cobertura.sql`

Mismo patrón que `wr_test_expediente_208_posicionar_firmar_escritura.sql`: inserta directamente una fila en `actividades` para llevar un expediente de prueba a esta actividad sin pasar por el motor XPDL (necesario porque BBV-104 no existe todavía y no hay forma real de llegar aquí).

### 1.4 No se necesitan catálogos nuevos

> `tipo_documento`, `tipo_vivienda` y `estado_proceso` son listas desplegables simples; se revisará
> al implementar si ya existen catálogos reutilizables (ej. tipo de documento suele venir de un catálogo común de identificación) o si se hardcodean como enum de frontend (igual a cómo `concepto_vobo` se maneja en HU-141 con valores fijos "Favorable"/"No Favorable").

---

## 2. Backend — Archivos a crear

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `Data.Repository.Interfaces/Entities/Multibanca/BBVA/Escrituracion/revision_marcacion_cobertura_entity.cs` | Clase con todas las columnas de la sección 1.1 |
| 2 | Entity Config | `Data.Repository.Implementations/EntityConfig/Multibanca/BBVA/Escrituracion/revision_marcacion_cobertura_entity_config.cs` | Fluent API → `ToTable("revision_marcacion_cobertura_bbva")` |
| 3 | DbContext | `MultibancaDBContext.cs` | Agregar `DbSet<revision_marcacion_cobertura_entity>` + registrar config en `OnModelCreating` |
| 4 | Repo Interface | `Data.Repository.Interfaces/Repositories/Multibanca/BBVA/Escrituracion/IRevisionMarcacionCoberturaRepository.cs` | `GetByExpediente` |
| 5 | Repo Impl | `Data.Repository.Implementations/Repositories/Multibanca/BBVA/Escrituracion/RevisionMarcacionCoberturaRepository.cs` | Query con EF, `AsNoTracking` |
| 6 | Domain Model | `Multibanca.Domain.Models/Multibanca/BBVA/Escrituracion/revision_marcacion_cobertura_bbva.cs` | Modelo de dominio (mismas propiedades que la entity) |
| 7 | Response | `Multibanca.Domain.Models/Multibanca/BBVA/Escrituracion/RevisionMarcacionCoberturaResponse.cs` | `{ formulario, herencia }` + clase `RevisionMarcacionCoberturaHerencia` |
| 8 | App Interface | `Multibanca.Application.Interfaces/Multibanca/BBVA/Escrituracion/IRevisionMarcacionCoberturaApplication.cs` | Métodos: `GetByExpediente`, `Guardar`, `Avanzar` |
| 9 | App Impl | `Multibanca.Application.Implementations/Multibanca/BBVA/Escrituracion/RevisionMarcacionCoberturaApplication.cs` | Lógica lineal + validación de email + bitácora |
| 10 | Email Interface | `Multibanca.Application.Interfaces/FuncTransversal/IEmailNotificationApplication.cs` | `EnviarNotificacionCobertura(...)` (ver sección 0.2) |
| 11 | Email Impl (stub) | `Multibanca.Application.Implementations/FuncTransversal/EmailNotificationApplication.cs` | Solo hace log, no envía |
| 12 | Controller | `Multibanca.Backend.Api/Controllers/Multibanca/RevisionMarcacionCoberturaController.cs` | Ruta: `api/RevisionMarcacionCobertura` |
| 13 | Constants | `Multibanca.Common/Constants.cs` | Actividad + transición |
| 14 | IoC | `IoCRegisterMultibanca.cs` | Registrar repository + application + email service |
| 15 | AutoMapper | `AutoMapperProfileMultibanca.cs` | `CreateMap<revision_marcacion_cobertura_bbva, revision_marcacion_cobertura_entity>().ReverseMap()` |

---

## 3. Backend — Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/RevisionMarcacionCobertura/GetByExpediente/{idExpediente}` | Consulta registro + datos heredados de encabezado |
| POST | `/api/RevisionMarcacionCobertura/Save` | Crea o actualiza |
| GET | `/api/RevisionMarcacionCobertura/Avanzar/{idExpediente}` | Valida + envía notificación + transiciona workflow + bitácora |

> Mismo formato de respuesta que `VoboGerenciaCohController` en las 3 rutas:
> `{ status, detail, message }`, con `InvalidOperationException` capturada como `Ok({status:false,message})`
> (falla de regla de negocio, no error de servidor) y cualquier otra excepción como 500.

---

## 4. Backend — Lógica de Avanzar

### 4.1 Constantes (Constants.cs)

```csharp
// En ActividadesBBVA
public const string EscrituracionRevisarMarcacionCobertura = "BBVA_ESCRITURACION_REVISAR_MARCACION_COBERTURA";

// Si no existe todavía (verificar antes de duplicar — el script de BBV-94 solo la registró en cat_actividades_ws, no en Constants.cs)
public const string EscrituracionValidarCondicionesDesembolso = "BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO";

// En TransicionesBBVA
public const string MarcacionCoberturaValidarCondiciones = "BBVA_ESCRITURACION_TR_MARCACION_COBERTURA_VALIDAR_CONDICIONES";
```

### 4.2 Dependencias del Application (inyección)

```csharp
public RevisionMarcacionCoberturaApplication(
    MultibancaDBContext multibancaDBContext,
    IRevisionMarcacionCoberturaRepository repository,
    IMapper mapper,
    IWorkflowApplication workflowApplication,
    IBitacoraApplication bitacoraApplication,
    IEncabezadoApplication encabezadoApplication,
    IEmailNotificationApplication emailNotificationApplication)
    : base(multibancaDBContext, repository, mapper)
```

### 4.3 GetByExpediente — CA02 (herencia desde encabezado)

```csharp
public async Task<RevisionMarcacionCoberturaResponse> GetByExpediente(long idExpediente)
{
    var entity = await RepositoryProvider.GetByExpediente(idExpediente);

    var formulario = entity != null
        ? _mapper.Map<revision_marcacion_cobertura_bbva>(entity)
        : new revision_marcacion_cobertura_bbva { id_expediente = idExpediente };

    // No hay actividad predecesora construida (BBV-104, ver sección 0.1):
    // la "herencia" se limita a los datos generales del encabezado.
    RevisionMarcacionCoberturaHerencia? herencia = null;
    try
    {
        var encabezado = await _encabezadoApplication.InformacionEncabezado(
            idExpediente, Constants.ActividadesBBVA.EscrituracionRevisarMarcacionCobertura);

        herencia = new RevisionMarcacionCoberturaHerencia
        {
            nombre_cliente        = encabezado?.nombre_completo_t1,
            numero_identificacion = encabezado?.numero_identificacion_t1,
            tipo_identificacion   = encabezado?.tipo_documento_id_t1,
        };
    }
    catch
    {
        // Falla al obtener encabezado: el frontend mostrará "-" y bloqueará solo Avanzar (patrón HU-141).
    }

    return new RevisionMarcacionCoberturaResponse { formulario = formulario, herencia = herencia };
}
```

### 4.4 Avanzar (CA05/CA06/CA08 — Lineal + notificación bloqueante)

```
1. Leer registro del expediente (debe existir; si no, error "Debe guardar antes de avanzar")
2. ValidarCamposObligatorios: todos los campos "Obligatorio = Sí" del modelado de datos
   (consecutivo, tipo_documento, numero_documento, tipo_tramite, nombre, constructora, proyecto,
    fecha_aceptacion_plataforma, tipo_vivienda, valor_subsidio, numero_obligacion, fecha_desembolso,
    fecha_proximo_canon, valor_desembolso, fecha_solicitud_marcacion, hora_solicitud_marcacion,
    responsable_m5, no_resolucion, fecha_resolucion, fecha_envio_resolucion, estado_proceso,
    observaciones)
   + condicionados: si hay fecha_respuesta_marcacion, entonces hora_respuesta_marcacion también
   es obligatoria (y viceversa)
3. ValidarEmail (CA08): email_area_colocaciones no vacío y con formato válido → si falla,
   InvalidOperationException("El correo del Área de Colocaciones es obligatorio y debe tener un formato válido.")
4. Enviar notificación (CA05, gatillador): await _emailNotificationApplication
     .EnviarNotificacionCobertura(idExpediente, formulario.email_area_colocaciones, userId)
   — el stub solo registra en log; no bloquea el avance si "falla" (no hay envío real todavía)
5. Obtener folio + transiciones del workflow
6. Avanzar SIEMPRE hacia "Validar Condiciones Desembolso" (Analista de Vivienda) — único destino
7. Registrar bitácora: Fecha, Actividad, Usuario Ejecutor, Observaciones (CA01.5)
```

### 4.5 Validación de campos obligatorios (esqueleto)

```csharp
private static void ValidarCamposObligatorios(revision_marcacion_cobertura_bbva f)
{
    var faltantes = new List<string>();

    if (string.IsNullOrWhiteSpace(f.tipo_documento)) faltantes.Add("Tipo de Documento");
    if (string.IsNullOrWhiteSpace(f.numero_documento)) faltantes.Add("C.C (Número)");
    if (string.IsNullOrWhiteSpace(f.tipo_tramite)) faltantes.Add("TT (Tipo Trámite)");
    if (string.IsNullOrWhiteSpace(f.nombre)) faltantes.Add("Nombre");
    if (string.IsNullOrWhiteSpace(f.constructora)) faltantes.Add("Constructora");
    if (string.IsNullOrWhiteSpace(f.proyecto)) faltantes.Add("Proyecto");
    if (!f.fecha_aceptacion_plataforma.HasValue) faltantes.Add("Fecha de Aceptación Plataforma");
    if (string.IsNullOrWhiteSpace(f.tipo_vivienda)) faltantes.Add("Tipo de Vivienda");
    if (!f.valor_subsidio.HasValue) faltantes.Add("Valor Subsidio");
    if (string.IsNullOrWhiteSpace(f.numero_obligacion)) faltantes.Add("N° Obligación");
    if (!f.fecha_desembolso.HasValue) faltantes.Add("Fecha de Desembolso");
    if (!f.fecha_proximo_canon.HasValue) faltantes.Add("Fecha Próximo Canon");
    if (!f.valor_desembolso.HasValue) faltantes.Add("Valor Desembolso");
    if (!f.fecha_solicitud_marcacion.HasValue) faltantes.Add("Fecha Solicitud Marcación");
    if (string.IsNullOrWhiteSpace(f.hora_solicitud_marcacion)) faltantes.Add("Hora Solicitud Marcación");
    if (string.IsNullOrWhiteSpace(f.responsable_m5)) faltantes.Add("Responsable M5");
    if (string.IsNullOrWhiteSpace(f.no_resolucion)) faltantes.Add("No Resolución");
    if (!f.fecha_resolucion.HasValue) faltantes.Add("Fecha de la Resolución");
    if (!f.fecha_envio_resolucion.HasValue) faltantes.Add("Fecha Envío Resolución");
    if (string.IsNullOrWhiteSpace(f.estado_proceso)) faltantes.Add("Estado Proceso");
    if (string.IsNullOrWhiteSpace(f.observaciones)) faltantes.Add("Observaciones");

    // Condicionado: fecha/hora de respuesta van juntas
    if (f.fecha_respuesta_marcacion.HasValue ^ !string.IsNullOrWhiteSpace(f.hora_respuesta_marcacion))
        faltantes.Add("Fecha y Hora de Respuesta Marcación (ambas o ninguna)");

    if (faltantes.Count > 0)
        throw new InvalidOperationException($"Campos obligatorios faltantes: {string.Join(", ", faltantes)}");
}

private static void ValidarEmail(revision_marcacion_cobertura_bbva f)
{
    var email = f.email_area_colocaciones?.Trim();
    if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
        throw new InvalidOperationException(
            "El correo del Área de Colocaciones es obligatorio y debe tener un formato de email válido.");
}
```

### 4.6 Servicio de correo (stub, sección 0.2)

```csharp
// Multibanca.Application.Interfaces/FuncTransversal/IEmailNotificationApplication.cs
public interface IEmailNotificationApplication
{
    Task EnviarNotificacionCobertura(long idExpediente, string correoDestino, int userId);
}

// Multibanca.Application.Implementations/FuncTransversal/EmailNotificationApplication.cs
public class EmailNotificationApplication : IEmailNotificationApplication
{
    private readonly ILogger<EmailNotificationApplication> _logger;

    public EmailNotificationApplication(ILogger<EmailNotificationApplication> logger)
        => _logger = logger;

    public Task EnviarNotificacionCobertura(long idExpediente, string correoDestino, int userId)
    {
        // TODO: reemplazar por envío SMTP real cuando existan credenciales/config.
        _logger.LogInformation(
            "[EmailNotificationApplication] Notificación de cobertura simulada. Expediente={IdExpediente}, Destino={Correo}, Usuario={UserId}",
            idExpediente, correoDestino, userId);
        return Task.CompletedTask;
    }
}
```

### 4.7 Registro en Bitácora

```csharp
var obs = $"Revisión Marcación de Cobertura. Estado Proceso: {formulario.estado_proceso}. " +
          $"Notificación enviada a: {formulario.email_area_colocaciones}. Destino: [Validar Condiciones Desembolso].";
if (!string.IsNullOrWhiteSpace(formulario.observaciones))
    obs += $" Observaciones: {formulario.observaciones}";

_bitacoraApplication.Create(new bitacora
{
    id_expediente = idExpediente,
    id_actividad  = Constants.ActividadesBBVA.EscrituracionRevisarMarcacionCobertura,
    id_usuario    = userId,
    fecha_alta    = DateTime.Now,
    observaciones = obs,
    is_active     = true,
    row_status    = true
}, userId);
```

---

## 5. Frontend — Archivos a crear

| # | Archivo | Descripción |
|---|---|---|
| 1 | `features/actividades/revisar_marcacion_cobertura/models/revision_marcacion_cobertura.ts` | Interfaces `RevisionMarcacionCobertura` + `RevisionMarcacionCoberturaHerencia` + factory `EMPTY_...` |
| 2 | `features/actividades/revisar_marcacion_cobertura/api/revisarMarcacionCoberturaService.ts` | 3 llamadas HTTP |
| 3 | `features/actividades/revisar_marcacion_cobertura/hooks/useRevisionMarcacionCobertura.ts` | `useQuery` consulta |
| 4 | `features/actividades/revisar_marcacion_cobertura/hooks/useUpsertRevisionMarcacionCobertura.ts` | `useMutation` guardar |
| 5 | `features/actividades/revisar_marcacion_cobertura/hooks/useAvanzarRevisionMarcacionCobertura.ts` | `useMutation` avanzar |
| 6 | `features/actividades/revisar_marcacion_cobertura/components/DatosHeredados.tsx` | Solo lectura (CA02), igual patrón `ReadonlyField` de HU-141 |
| 7 | `features/actividades/revisar_marcacion_cobertura/components/SeccionMarcacionCobertura.tsx` | ~26 campos editables agrupados |
| 8 | `features/actividades/revisar_marcacion_cobertura/components/SeccionNotificacionColocaciones.tsx` | Campo de correo (CA05/CA08) |
| 9 | `features/actividades/revisar_marcacion_cobertura/pages/revisar_marcacion_cobertura_page.tsx` | Página principal (acordeón + botones) |
| 10 | `shared/components/InputTimeForm.tsx` | Nuevo — input de solo-hora (HH:MM), no existe todavía |
| 11 | `routes/Routes.tsx` | Ruta: `revisar_marcacion_cobertura/:id_expediente` |

### 5.1 Service — Llamadas HTTP

```typescript
import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  RevisionMarcacionCobertura,
  RevisionMarcacionCoberturaResponse,
} from '../models/revision_marcacion_cobertura';

const PATH_URL = '/api/RevisionMarcacionCobertura';

export const revisarMarcacionCoberturaService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<RevisionMarcacionCoberturaResponse | null>> {
    const response = await axiosClient.get<ApiResponse<RevisionMarcacionCoberturaResponse | null>>(
      `${PATH_URL}/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(payload: RevisionMarcacionCobertura): Promise<ApiResponse<RevisionMarcacionCobertura>> {
    const response = await axiosClient.post<ApiResponse<RevisionMarcacionCobertura>>(
      `${PATH_URL}/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<{ actividad_destino: string }>> {
    const response = await axiosClient.get<ApiResponse<{ actividad_destino: string }>>(
      `${PATH_URL}/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
```

---

## 6. Frontend — Estructura de la página

### 6.1 Funciones Transversales

| Función transversal | ¿Aplica? | Notas |
|---|---|---|
| Expediente Digital | ✅ Sí | Adjuntar la resolución del subsidio (CA01.3) |
| Trazabilidad / Bitácora | ✅ Sí | Historial de acciones |
| Registro de Contacto | ❌ No | `show_registro_contacto={false}` (igual a HU-141/BBV-94) |
| Carta de Aprobación / Carta de Compromiso | ❌ No | No aplican a esta actividad |

### 6.2 Wireframe

```
┌──────────────────────────────────────────────────────┐
│ Título: "Revisar Marcación de Cobertura"             │
├──────────────────────────────────────────────────────┤
│ Acordeón 1: Información General (expandido)          │
│   └─ EncabezadoActividad (solo lectura)              │
├──────────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales (colapsado)      │
│   └─ Expediente Digital + Bitácora                   │
├──────────────────────────────────────────────────────┤
│ Acordeón 3: Revisar Marcación de Cobertura (expand.) │
│                                                       │
│   ┌─ DatosHeredados (solo lectura, CA02) ───────────┐│
│   │  Nombre Cliente, N° Identificación, Tipo Doc.   ││
│   └──────────────────────────────────────────────────┘│
│                                                       │
│   ┌─ SeccionMarcacionCobertura (editable, CA04) ────┐│
│   │  Identificación: Consecutivo(ro) Tipo Doc* CC*  ││
│   │                  TT* Nombre*                    ││
│   │  Proyecto: Constructora* Proyecto*              ││
│   │            Fecha Aceptación Plataforma*         ││
│   │            Tipo Vivienda*                        ││
│   │  Valores:  Valor Subsidio* N° Obligación*       ││
│   │            Fecha Desembolso* Fecha Próx. Canon* ││
│   │            Valor Desembolso* Intereses Corr.    ││
│   │            Capital Seguros Cuota Mensual Plazo  ││
│   │            Observación                          ││
│   │  Marcación: Fecha/Hora Solicitud*                ││
│   │             Fecha/Hora Respuesta (condicionado) ││
│   │             Responsable M5*                      ││
│   │  Resolución: No Resolución* Fecha Resolución*   ││
│   │              Fecha Envío Resolución*             ││
│   │              Estado Proceso*                     ││
│   │  Observaciones* (CA07)                           ││
│   └──────────────────────────────────────────────────┘│
│                                                       │
│   ┌─ SeccionNotificacionColocaciones (CA05/CA08) ───┐│
│   │  Correo Área de Colocaciones* (bloqueante)      ││
│   └──────────────────────────────────────────────────┘│
│                                                       │
│   Botones: [Guardar] [Avanzar] [Salir]               │
└──────────────────────────────────────────────────────┘
```

### 6.3 Flujo de estados UI

Igual patrón de doble error que HU-141/BBV-94:
- `loadError` (fallo de red/API grave) → oculta sección, deshabilita Guardar y Avanzar.
- `herenciaError` (encabezado no disponible) → banner + botón "Reintentar", deshabilita solo Avanzar.
- `handleAvanzar` valida en frontend: campos obligatorios de la sección 4.5 **y** formato de correo (CA08) antes de llamar al backend; si algo falla, toast de advertencia sin llamar a la API.

---

## 7. Frontend — Validación (antes de Avanzar)

```typescript
export const camposObligatoriosFaltantes = (form: RevisionMarcacionCobertura): string[] => {
  const missing: string[] = [];

  if (!form.tipo_documento) missing.push('Tipo de Documento');
  if (!form.numero_documento?.trim()) missing.push('C.C (Número)');
  if (!form.tipo_tramite?.trim()) missing.push('TT (Tipo Trámite)');
  if (!form.nombre?.trim()) missing.push('Nombre');
  if (!form.constructora?.trim()) missing.push('Constructora');
  if (!form.proyecto?.trim()) missing.push('Proyecto');
  if (!form.fecha_aceptacion_plataforma) missing.push('Fecha de Aceptación Plataforma');
  if (!form.tipo_vivienda) missing.push('Tipo de Vivienda');
  if (form.valor_subsidio == null) missing.push('Valor Subsidio');
  if (!form.numero_obligacion?.trim()) missing.push('N° Obligación');
  if (!form.fecha_desembolso) missing.push('Fecha de Desembolso');
  if (!form.fecha_proximo_canon) missing.push('Fecha Próximo Canon');
  if (form.valor_desembolso == null) missing.push('Valor Desembolso');
  if (!form.fecha_solicitud_marcacion) missing.push('Fecha Solicitud Marcación');
  if (!form.hora_solicitud_marcacion?.trim()) missing.push('Hora Solicitud Marcación');
  if (!form.responsable_m5?.trim()) missing.push('Responsable M5');
  if (!form.no_resolucion?.trim()) missing.push('No Resolución');
  if (!form.fecha_resolucion) missing.push('Fecha de la Resolución');
  if (!form.fecha_envio_resolucion) missing.push('Fecha Envío Resolución');
  if (!form.estado_proceso) missing.push('Estado Proceso');
  if (!form.observaciones?.trim()) missing.push('Observaciones');

  const tieneRespuesta = !!form.fecha_respuesta_marcacion || !!form.hora_respuesta_marcacion?.trim();
  if (tieneRespuesta && (!form.fecha_respuesta_marcacion || !form.hora_respuesta_marcacion?.trim()))
    missing.push('Fecha y Hora de Respuesta Marcación (ambas o ninguna)');

  return missing;
};

// Reutilizar isValidEmail de carga_operacion_banco/utils/inputFilters.ts
export const emailColocacionesInvalido = (form: RevisionMarcacionCobertura): boolean =>
  !form.email_area_colocaciones?.trim() || !isValidEmail(form.email_area_colocaciones);
```

---

## 8. Modelo TypeScript

```typescript
export interface RevisionMarcacionCobertura {
  id: number;
  id_expediente: number;
  id_actividad: string;

  email_area_colocaciones: string | null;

  consecutivo: string | null;
  tipo_documento: string | null;
  numero_documento: string | null;
  tipo_tramite: string | null;
  nombre: string | null;

  constructora: string | null;
  proyecto: string | null;
  fecha_aceptacion_plataforma: string | null;
  tipo_vivienda: string | null;

  valor_subsidio: number | null;
  numero_obligacion: string | null;
  fecha_desembolso: string | null;
  fecha_proximo_canon: string | null;
  valor_desembolso: number | null;
  intereses_corrientes: number | null;
  capital: number | null;
  seguros: number | null;
  cuota_mensual: number | null;
  plazo: number | null;
  observacion: string | null;

  fecha_solicitud_marcacion: string | null;
  hora_solicitud_marcacion: string | null;
  fecha_respuesta_marcacion: string | null;
  hora_respuesta_marcacion: string | null;
  responsable_m5: string | null;

  no_resolucion: string | null;
  fecha_resolucion: string | null;
  fecha_envio_resolucion: string | null;
  estado_proceso: string | null;

  observaciones: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RevisionMarcacionCoberturaHerencia {
  nombre_cliente?: string | null;
  numero_identificacion?: string | null;
  tipo_identificacion?: string | null;
}

export interface RevisionMarcacionCoberturaResponse {
  formulario: RevisionMarcacionCobertura;
  herencia: RevisionMarcacionCoberturaHerencia | null;
}

export const EMPTY_REVISION_MARCACION_COBERTURA = (
  id_expediente: number,
): RevisionMarcacionCobertura => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_REVISAR_MARCACION_COBERTURA',
  email_area_colocaciones: null,
  consecutivo: null,
  tipo_documento: null,
  numero_documento: null,
  tipo_tramite: null,
  nombre: null,
  constructora: null,
  proyecto: null,
  fecha_aceptacion_plataforma: null,
  tipo_vivienda: null,
  valor_subsidio: null,
  numero_obligacion: null,
  fecha_desembolso: null,
  fecha_proximo_canon: null,
  valor_desembolso: null,
  intereses_corrientes: null,
  capital: null,
  seguros: null,
  cuota_mensual: null,
  plazo: null,
  observacion: null,
  fecha_solicitud_marcacion: null,
  hora_solicitud_marcacion: null,
  fecha_respuesta_marcacion: null,
  hora_respuesta_marcacion: null,
  responsable_m5: null,
  no_resolucion: null,
  fecha_resolucion: null,
  fecha_envio_resolucion: null,
  estado_proceso: null,
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

## 9. Lo que NO se crea (en esta HU)

- ❌ No se implementa la compuerta real de Ciudad=Barranquilla / Subsidio=Mi Techo Propio (depende de BBV-104, ver 0.1).
- ❌ No se implementa envío SMTP real (depende de credenciales, ver 0.2) — solo el stub con log.
- ❌ No hay integración real con SITCAR — todos los campos "Diligencia SITCAR" son de captura manual, igual tratamiento técnico que los "Precargado Editable" (no existe distinción de tipo en todo el código, confirmado contra `vobo_gerencia_coh_bbva`/`excepcion_desembolso`).
- ❌ No se construye la pantalla de "Validar Condiciones Desembolso" (BBV-99) — el destino queda como bandeja sin pantalla, igual al vacío que ya existe hoy en el sistema.
- ❌ No hay devoluciones ni desvíos — flujo estrictamente lineal (CA06).

---

## 10. Orden de ejecución

1. Ejecutar script SQL (tabla + `cat_actividades_ws` + script de posicionamiento de prueba) contra `BBVA_LEGALIZACION`.
2. Crear Entity + EntityConfig + agregar al DbContext.
3. Crear Repository (interface + impl).
4. Crear Domain Model + Response + registrar AutoMapper.
5. Crear `IEmailNotificationApplication` + stub, registrar en IoC.
6. Crear Application (interface + impl con validaciones + notificación + bitácora).
7. Crear Controller.
8. Registrar Application/Repository en IoC + agregar constantes en `Constants.cs`.
9. Frontend: modelo → service → hooks → `InputTimeForm` (nuevo shared) → componentes → página.
10. Agregar ruta en `Routes.tsx`.
11. Probar flujo completo: posicionar expediente de prueba → carga → guardar → bloqueo por correo/campos faltantes → avanzar exitoso → verificar bitácora y nueva fila en bandeja para "Validar Condiciones Desembolso".

---

## 11. Diferencias clave con HU-141 (Vobo Gerencia COH)

| Aspecto | HU-141 (Vobo Gerencia COH) | BBV-107 (Marcación de Cobertura) |
|---|---|---|
| Rol | Gerente COH | Analista de Cobertura |
| Campos editables | 2 (concepto + observaciones) | ~26 campos |
| Herencia | Tabla predecesora real (`excepcion_desembolso`) | Solo encabezado (predecesora BBV-104 no existe) |
| Notificación por correo | No aplica | Sí, bloqueante (CA05/CA08) — capacidad nueva en el backend |
| Acceso a la pantalla | Vía enrutamiento real desde BBV-94 | Standalone + script de posicionamiento de prueba (ver 0.1) |
| Complejidad de formulario | Baja | Alta (fechas, horas, moneda, condicionados) |
| Componente nuevo compartido | Ninguno | `InputTimeForm.tsx` (no existía input de solo-hora) |
