# BBV-93 — Receta de Implementación: Realizar Recepción Boleta

> Actividad del subproceso de Escrituración y Garantías.
> Rol: Analista de Vivienda.
> Incluye integración con Bot VUR (RPA) para extracción automática de datos registrales.

---

## Flujo

```
Realizar Entrega EP Firmada (BBV-92)
    → Realizar Recepción Boleta ← ESTA HU
        → Siempre: "Realizar EP Registradas" (Analista de Vivienda)
        → Si aplica excepción: también "Realizar Excepción Desembolso" (Comercial) en paralelo
```

---

## 1. Base de datos — Script SQL

**Archivo:** `backend/database/escrituracion/wr_bbv93_realizar_recepcion_boleta.sql`

### 1.1 Tabla

```sql
-- BBV-93 — Realizar Recepción Boleta (Escrituración y Garantías)
-- Script idempotente

DROP TABLE IF EXISTS public.realizar_recepcion_boleta CASCADE;

CREATE TABLE IF NOT EXISTS public.realizar_recepcion_boleta (
    id                      BIGSERIAL PRIMARY KEY,
    id_expediente           BIGINT NOT NULL,
    id_actividad            VARCHAR(100),
    numero_boleta           VARCHAR(100),
    fecha_boleta            DATE,
    numero_matricula        VARCHAR(200),
    tipo_boleta             VARCHAR(100),
    boleta_en_poder_de      VARCHAR(200),
    codigo_zona             VARCHAR(50),
    oficina_registro        VARCHAR(200),
    boleta_recibida         BOOLEAN NOT NULL DEFAULT FALSE,
    aplica_excepcion        VARCHAR(2),
    vur_ejecutado           BOOLEAN NOT NULL DEFAULT FALSE,
    vur_exitoso             BOOLEAN NOT NULL DEFAULT FALSE,
    vur_intentos            INTEGER NOT NULL DEFAULT 0,
    observaciones           VARCHAR(500),
    is_active               BOOLEAN NOT NULL DEFAULT TRUE,
    row_status              BOOLEAN NOT NULL DEFAULT TRUE,
    created_by              INTEGER NOT NULL,
    created_date            TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    modified_by             INTEGER,
    modified_date           TIMESTAMP WITHOUT TIME ZONE
);

-- Índice UNIQUE parcial: garantiza un solo registro activo por expediente
CREATE UNIQUE INDEX IF NOT EXISTS idx_realizar_recepcion_boleta_expediente
    ON public.realizar_recepcion_boleta (id_expediente)
    WHERE is_active = true AND row_status = true;

-- Stored Procedure — Consulta por expediente
CREATE OR REPLACE FUNCTION public.usp_select_realizar_recepcion_boleta_bbva(
    p_id_expediente BIGINT)
RETURNS SETOF public.realizar_recepcion_boleta
LANGUAGE sql
STABLE
AS $$
    SELECT actividad.*
    FROM public.realizar_recepcion_boleta actividad
    WHERE actividad.id_expediente = p_id_expediente
      AND actividad.is_active = TRUE
      AND actividad.row_status = TRUE
    ORDER BY actividad.id DESC
    LIMIT 1;
$$;

-- Permisos
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.realizar_recepcion_boleta TO multibanca;
GRANT USAGE, SELECT ON SEQUENCE public.realizar_recepcion_boleta_id_seq TO multibanca;
GRANT EXECUTE ON FUNCTION public.usp_select_realizar_recepcion_boleta_bbva(BIGINT) TO multibanca;
```

### 1.2 Insert / Update

> **NO se necesita SP para insert/update.** Se usa EF Core con `Create()` y `Update()` del
> `MultibancaGenericApplication` (mismo patrón que BBV-86, BBV-91, BBV-92).

### 1.3 Registro en cat_actividades_ws (bandeja)

```sql
-- Realizar Recepción Boleta (esta HU) — ya debería existir por BBV-92
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Recepción Boleta', 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_recepcion_boleta', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA');

-- Destino: Realizar EP Registradas (siempre)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar EP Registradas', 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_ep_registradas', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS');

-- Destino: Realizar Excepción Desembolso (condicional, ya debería existir de BBV-92)
INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Excepción Desembolso', 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_excepcion_desembolso', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO');
```

### 1.4 Catálogos nuevos

```sql
-- L44 — Tipo de Boleta
WITH l44(codigo, descripcion, orden) AS (
    VALUES
        ('TBOL-1', 'Física Original', 1),
        ('TBOL-2', 'Física Copia', 2),
        ('TBOL-3', 'Electrónica', 3)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'TIPO_BOLETA', l44.descripcion, l44.codigo, NULL, true, l44.orden
FROM l44
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'TIPO_BOLETA' AND c.valor = l44.codigo
);

-- L45 — Oficina de Registro (fuente: listado oficial de Oficinas Registrales)
-- NOTA: Son ~230 registros. El frontend debe usar Dropdown con buscador (filterable).
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'OFICINA_REGISTRO', v.descripcion, v.codigo, NULL, true, v.orden
FROM (VALUES
    ('400','LETICIA',1),('520','MITÚ',2),('001 N','MEDELLÍN ZONA NORTE',3),('001 S','MEDELLÍN ZONA SUR',4),
    ('2','ABEJORRAL',5),('3','AMALFI',6),('4','ANDES',7),('5','BOLÍVAR',8),('7','DABEIBA',9),
    ('8','APARTADÓ',10),('10','FREDONIA',11),('11','FRONTINO',12),('12','GIRARDOTA',13),
    ('13','ITUANGO',14),('14','JERICÓ',15),('15','CAUCASIA',16),('17','LA CEJA',17),
    ('18','MARINILLA',18),('19','PUERTO BERRÍO',19),('20','RIONEGRO',20),('23','SANTA BÁRBARA',21),
    ('24','SANTAFÉ DE ANTIOQUIA',22),('25','SANTA ROSA DE OSOS',23),('26','SANTO DOMINGO',24),
    ('27','SEGOVIA',25),('28','SONSON',26),('29','SOPETRÁN',27),('32','TÁMESIS',28),
    ('33','TITIRIBÍ',29),('34','TURBO',30),('35','URRAO',31),('37','YARUMAL',32),
    ('38','YOLOMBÓ',33),('40','BARRANQUILLA',34),('41','SOLEDAD',35),('45','SABANALARGA',36),
    ('050 C','BOGOTÁ ZONA CENTRO',37),('050 N','BOGOTÁ ZONA NORTE',38),('050 S','BOGOTA ZONA SUR',39),
    ('51','SOACHA',40),('60','CARTAGENA',41),('62','EL CARMEN DE BOLÍVAR',42),('64','MAGANGUÉ',43),
    ('65','MOMPÓS',44),('68','SIMITÍ',45),('70','TUNJA',46),('72','CHIQUINQUIRÁ',47),
    ('74','DUITAMA',48),('76','EL COCUY',49),('78','GARAGOA',50),('79','GUATEQUE',51),
    ('82','MIRAFLORES',52),('83','MONIQUIRÁ',53),('86','OROCUÉ',54),('88','PUERTO BOYACÁ',55),
    ('90','RAMIRIQUÍ',56),('92','SANTA ROSA DE VITERBO',57),('93','SOATÁ',58),('94','SOCHA',59),
    ('95','SOGAMOSO',60),('100','MANIZALES',61),('102','AGUADAS',62),('104','ANSERMA',63),
    ('106','LA DORADA',64),('108','MANZANARES',65),('110','NEIRA',66),('112','PÁCORA',67),
    ('114','PENSILVANIA',68),('115','RIOSUCIO',69),('118','SALAMINA',70),('120','POPAYÁN',71),
    ('122','BOLÍVAR',72),('124','CALOTO',73),('126','GUAPI',74),('128','PATÍA',75),
    ('130','PUERTO TEJADA',76),('132','SANTANDER DE QUILICHAO',77),('134','SILVIA',78),
    ('140','MONTERÍA',79),('142','MONTELÍBANO',80),('143','CERETÉ',81),('144','CHINÚ',82),
    ('146','LORICA',83),('148','SAHAGÚN',84),('150','AGUA DE DIOS',85),('152','CAQUEZA',86),
    ('154','CHOCONTÁ',87),('156','FACATATIVÁ',88),('157','FUSAGASUGÁ',89),('160','GACHETA',90),
    ('161','CONTRATACIÓN',91),('162','GUADUAS',92),('166','LA MESA',93),('167','LA PALMA',94),
    ('170','PACHO',95),('172','VILLA DE SAN DIEGO DE UBATE',96),('176','ZIPAQUIRÁ',97),
    ('180','QUIBDÓ',98),('184','ISTMINA',99),('186','NUQUÍ',100),('190','VALLEDUPAR',101),
    ('192','CHIMICHAGUA',102),('196','AGUACHICA',103),('200','NEIVA',104),('202','GARZÓN',105),
    ('204','LA PLATA',106),('206','PITALITO',107),('210','RIOHACHA',108),('212','MAICAO',109),
    ('214','SAN JUAN DEL CESAR',110),('220','SANTA MARTA',111),('222','CIÉNAGA',112),
    ('224','EL BANCO',113),('225','FUNDACIÓN',114),('226','PLATO',115),('228','SITIONUEVO',116),
    ('230','VILLAVICENCIO',117),('232','ACACÍAS',118),('234','PUERTO LÓPEZ',119),
    ('236','SAN MARTÍN',120),('240','PASTO',121),('242','BARBACOAS',122),('244','IPIALES',123),
    ('246','LA CRUZ',124),('248','LA UNIÓN',125),('250','SAMANIEGO',126),
    ('252','SAN ANDRES DE TUMACO',127),('254','TÚQUERRES',128),('260','CÚCUTA',129),
    ('264','CHINÁCOTA',130),('266','CONVENCIÓN',131),('270','OCAÑA',132),('271','CACHIRÁ',133),
    ('272','PAMPLONA',134),('276','SALAZAR',135),('280','ARMENIA',136),('282','CALARCA',137),
    ('284','FILANDIA',138),('290','PEREIRA',139),('292','APÍA',140),('293','BELÉN DE UMBRÍA',141),
    ('294','DOSQUEBRADAS',142),('296','SANTA ROSA DE CABAL',143),('297','SANTUARIO',144),
    ('300','BUCARAMANGA',145),('302','BARICHARA',146),('303','BARRANCABERMEJA',147),
    ('306','CHARALÁ',148),('307','GIRARDOT',149),('308','CONCEPCIÓN',150),('312','MÁLAGA',151),
    ('314','PIEDECUESTA',152),('315','PUENTE NACIONAL',153),('318','SAN ANDRÉS',154),
    ('319','SAN GIL',155),('320','SAN VICENTE DE CHUCURÍ',156),('321','SOCORRO',157),
    ('324','VÉLEZ',158),('326','ZAPATOCA',159),('340','SINCELEJO',160),('342','COROZAL',161),
    ('346','SAN MARCOS',162),('347','SAN LUIS DE SINCÉ',163),('350','IBAGUÉ',164),
    ('351','AMBALEMA',165),('352','ARMERO - GUAYABAL',166),('354','CAJAMARCA',167),
    ('355','CHAPARRAL',168),('357','ESPINAL',169),('359','FRESNO',170),('360','GUAMO',171),
    ('362','HONDA',172),('364','LÍBANO',173),('366','MELGAR',174),('368','PURIFICACIÓN',175),
    ('370','CALI',176),('372','BUENAVENTURA',177),('373','GUADALAJARA DE BUGA',178),
    ('376','CARTAGO',179),('378','PALMIRA',180),('380','ROLDANILLO',181),('382','SEVILLA',182),
    ('384','TULUÁ',183),('410','ARAUCA',184),('420','FLORENCIA',185),
    ('425','SAN VICENTE DEL CAGUÁN',186),('440','MOCOA',187),('441','SIBUNDOY',188),
    ('442','PUERTO ASÍS',189),('450','SAN ANDRÉS',190),('470','YOPAL',191),
    ('475','PAZ DE ARIPORO',192),('480','SAN JOSÉ DEL GUAVIARE',193),('500','INÍRIDA',194),
    ('540','PUERTO CARREÑO',195)
) AS v(codigo, descripcion, orden)
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'OFICINA_REGISTRO' AND c.valor = v.codigo
);
```

> **Nota:** Los valores de L45 son los oficiales del listado de Oficinas Registrales de Colombia (~195 registros).
> El frontend debe usar un **Dropdown con buscador** (`filter={true}` en PrimeReact) dado el volumen de opciones.
> El código de la oficina (ej: "050 N") se almacena en `valor` y la descripción (ej: "BOGOTÁ ZONA NORTE") en `descripcion`.
> CA10 indica que `oficina_registro` depende del `codigo_zona`. Por ahora se implementa como lista
> independiente con buscador. Si se requiere dependencia estricta, agregar un catálogo `CODIGO_ZONA`
> y vincular L45 con `id_padre`.

---

## 2. Backend — Archivos a crear

| # | Capa | Archivo | Descripción |
|---|---|---|---|
| 1 | Entity | `Data.Repository.Interfaces/Entities/Multibanca/BBVA/Escrituracion/realizar_recepcion_boleta_entity.cs` | Clase con todas las columnas |
| 2 | Entity Config | `Data.Repository.Implementations/EntityConfig/Multibanca/BBVA/Escrituracion/realizar_recepcion_boleta_entity_config.cs` | Fluent API → `ToTable("realizar_recepcion_boleta")` |
| 3 | DbContext | `MultibancaDBContext.cs` | Agregar `DbSet<realizar_recepcion_boleta_entity>` + config |
| 4 | Repo Interface | `Data.Repository.Interfaces/Repositories/Multibanca/BBVA/Escrituracion/IRealizarRecepcionBoletaRepository.cs` | `GetByExpediente` |
| 5 | Repo Impl | `Data.Repository.Implementations/Repositories/Multibanca/BBVA/Escrituracion/RealizarRecepcionBoletaRepository.cs` | Query con EF |
| 6 | Domain Model | `Multibanca.Domain.Models/Multibanca/BBVA/Escrituracion/realizar_recepcion_boleta.cs` | Modelo de dominio |
| 7 | App Interface | `Multibanca.Application.Interfaces/Multibanca/BBVA/Escrituracion/IRealizarRecepcionBoletaApplication.cs` | Métodos: GetByExpediente, GetControles, Avanzar, EjecutarVUR |
| 8 | App Impl | `Multibanca.Application.Implementations/Multibanca/BBVA/Escrituracion/RealizarRecepcionBoletaApplication.cs` | Lógica VUR + excepción + enrutamiento |
| 9 | Controller | `Multibanca.Backend.Api/Controllers/Multibanca/BBVA/Escrituracion/RealizarRecepcionBoletaController.cs` | Ruta: `api/realizar-recepcion-boleta` |
| 10 | Constants | `Multibanca.Common/Constants.cs` | Actividad + transiciones + catálogos |
| 11 | IoC | `IoCRegisterMultibanca.cs` | Registrar repository + application |
| 12 | AutoMapper | `AutoMapperProfileMultibanca.cs` | `CreateMap<entity, domain>().ReverseMap()` |

---

## 3. Backend — Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/realizar-recepcion-boleta/GetByIdExpediente/{id_expediente}` | Consulta registro + calcula aplica_excepcion + datos heredados |
| GET | `/api/realizar-recepcion-boleta/controles` | Retorna catálogos L44 (tipo_boleta) y L45 (oficina_registro) |
| POST | `/api/realizar-recepcion-boleta/Save` | Crea o actualiza |
| POST | `/api/realizar-recepcion-boleta/avanzar/{id_expediente}` | Valida + transiciona workflow + bitácora |
| POST | `/api/realizar-recepcion-boleta/ejecutar-vur/{id_expediente}` | Dispara el bot VUR manualmente (CA04) |

### 3.1 Patrón de manejo de errores (HandleException)

Mismo patrón que BBV-91/BBV-92 con `campos_faltantes`.

---

## 4. Backend — Lógica de Avanzar

### 4.1 Constantes (Constants.cs)

```csharp
// En ActividadesBBVA
public const string EscrituracionRealizarRecepcionBoleta = "BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA";
public const string EscrituracionRealizarEPRegistradas = "BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS";

// En TransicionesBBVA
public const string RecepcionBoletaEPRegistradas = "BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EP_REGISTRADAS";
public const string RecepcionBoletaExcepcionDesembolso = "BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EXCEPCION_DESEMBOLSO";

// En Catalogo
public const string TipoBoleta = "TIPO_BOLETA";
public const string OficinaRegistro = "OFICINA_REGISTRO";
```

### 4.2 Dependencias del Application (inyección)

```csharp
public RealizarRecepcionBoletaApplication(
    MultibancaDBContext multibancaDBContext,
    IRealizarRecepcionBoletaRepository repository,
    IMapper mapper,
    ICommonApplication commonApplication,               // → GetCatalogoByType
    IWorkflowApplication workflowApplication,           // → GetTransitions, CapturarDatosFolio, AvanzarActividad
    IBitacoraApplication bitacoraApplication,           // → Create
    IValidarInformacionApplication validarInfoApp,      // → tipo_credito
    IFirmarEscrituraClienteApplication firmarEscApp)    // → datos heredados notaría
    : base(multibancaDBContext, repository, mapper)
```

### 4.3 Lógica de cálculo de `aplica_excepcion` (CA06)

```csharp
// Tipos de crédito que aplican excepción cuando tipo_desembolso = BOLETA
private static readonly string[] TiposExcepcionBoleta = new[]
{
    "LEASING_USADO",
    "HIPOTECARIO_USADO",
    "REMODELACION_AMPLIAR_HIPOTECAR"
};

private static string CalcularAplicaExcepcion(string? tipoCredito, string? tipoDesembolso)
{
    if (string.IsNullOrWhiteSpace(tipoCredito))
        return "NO";

    if (!string.Equals(tipoDesembolso, "BOLETA", StringComparison.OrdinalIgnoreCase))
        return "NO";

    return TiposExcepcionBoleta.Contains(tipoCredito, StringComparer.OrdinalIgnoreCase)
        ? "SI"
        : "NO";
}
```

> **Fuente:** `validar_informacion_bbva.tipo_credito` + paramétrica constructoras `tipo_desembolso`.
> Si no existe la paramétrica de constructoras aún, evaluar solo con `tipo_credito` (mismo patrón BBV-92).

### 4.4 Flujo de Avanzar

```
1. Leer registro del expediente
2. Validar campos obligatorios (CA05, CA07, CA08):
     - numero_boleta
     - fecha_boleta (no futura)
     - numero_matricula
     - tipo_boleta
     - codigo_zona
     - oficina_registro
     - boleta_recibida == true (CA08)
     - Si tipo_boleta es "Física Original" o "Física Copia": boleta_en_poder_de obligatorio (CA11)
     - Documento "Folio(s) Previo(s)" adjunto (CA07) — validar contra expediente digital
3. Obtener transiciones y folio
4. SIEMPRE avanzar hacia "Realizar EP Registradas" (Analista de Vivienda)
5. SI aplica_excepcion == "SI":
     - Avanzar EN PARALELO hacia "Realizar Excepción Desembolso" (Comercial)
6. Registrar bitácora
```

### 4.5 Registro en Bitácora

```csharp
var observacionesBitacora = $"Avance de Realizar Recepción Boleta. " +
    $"Número Boleta: {formulario.numero_boleta}. " +
    $"Fecha Boleta: {formulario.fecha_boleta:yyyy-MM-dd}. " +
    $"Matrícula: {formulario.numero_matricula}. " +
    $"VUR Exitoso: {(formulario.vur_exitoso ? "Sí" : "No")}. " +
    $"¿Aplica Excepción?: {formulario.aplica_excepcion}. " +
    $"Destino principal: [Realizar EP Registradas].";

if (formulario.aplica_excepcion == "SI")
    observacionesBitacora += " Destino paralelo: [Realizar Excepción Desembolso].";

if (!string.IsNullOrWhiteSpace(formulario.observaciones))
    observacionesBitacora += $" Observaciones: {formulario.observaciones}";
```

### 4.6 Validación de campos obligatorios

```csharp
private static void ValidarCamposObligatorios(realizar_recepcion_boleta formulario)
{
    var camposFaltantes = new List<string>();

    if (string.IsNullOrWhiteSpace(formulario.numero_boleta))
        camposFaltantes.Add("Número de Boleta");

    if (!formulario.fecha_boleta.HasValue)
        camposFaltantes.Add("Fecha de Boleta");
    else if (formulario.fecha_boleta.Value > DateTime.Today)
        camposFaltantes.Add("Fecha de Boleta (no puede ser futura)");

    if (string.IsNullOrWhiteSpace(formulario.numero_matricula))
        camposFaltantes.Add("Número de Matrícula");

    if (string.IsNullOrWhiteSpace(formulario.tipo_boleta))
        camposFaltantes.Add("Tipo de Boleta");

    if (string.IsNullOrWhiteSpace(formulario.codigo_zona))
        camposFaltantes.Add("Código Zona");

    if (string.IsNullOrWhiteSpace(formulario.oficina_registro))
        camposFaltantes.Add("Oficina de Registro");

    if (!formulario.boleta_recibida)
        camposFaltantes.Add("Boleta Recibida (debe estar marcada)");

    // CA11 — Si tipo boleta es Física, exige "Boleta En Poder De"
    if (formulario.tipo_boleta == "TBOL-2" || formulario.tipo_boleta == "TBOL-3")
    {
        if (string.IsNullOrWhiteSpace(formulario.boleta_en_poder_de))
            camposFaltantes.Add("Boleta En Poder De");
    }

    if (camposFaltantes.Count > 0)
    {
        throw new InvalidOperationException(
            $"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
    }
}
```

### 4.7 Lógica del Bot VUR (CA03, CA04)

```csharp
/// <summary>
/// Ejecuta la consulta al Bot VUR para extraer datos registrales.
/// Reintenta hasta 3 veces antes de declarar fallo.
/// </summary>
public async Task<realizar_recepcion_boleta> EjecutarVUR(long idExpediente, int userId)
{
    var entity = await RepositoryProvider.GetByExpediente(idExpediente);
    var formulario = entity != null
        ? _mapper.Map<realizar_recepcion_boleta>(entity)
        : new realizar_recepcion_boleta { id_expediente = idExpediente };

    const int MAX_INTENTOS = 3;
    bool exitoso = false;

    for (int intento = 1; intento <= MAX_INTENTOS && !exitoso; intento++)
    {
        formulario.vur_intentos = intento;

        try
        {
            // TODO: Integrar con el servicio real del Bot VUR
            // var resultado = await _vurService.ConsultarRegistro(idExpediente);
            // formulario.numero_boleta = resultado.NumeroRadicado;
            // formulario.fecha_boleta = resultado.FechaRadicacion;
            // formulario.numero_matricula = resultado.NumeroMatricula;
            // formulario.oficina_registro = resultado.OficinaRegistro;
            // exitoso = true;

            // PLACEHOLDER: Simular fallo hasta integración real
            exitoso = false;
        }
        catch
        {
            exitoso = false;
        }
    }

    formulario.vur_ejecutado = true;
    formulario.vur_exitoso = exitoso;

    // Guardar resultado
    if (entity != null)
    {
        _mapper.Map(formulario, entity);
        RepositoryProvider.Update(entity, userId);
    }
    else
    {
        formulario.id_actividad = Constants.ActividadesBBVA.EscrituracionRealizarRecepcionBoleta;
        var newEntity = _mapper.Map<realizar_recepcion_boleta_entity>(formulario);
        RepositoryProvider.Create(newEntity, userId);
    }

    return formulario;
}
```

---

## 5. Frontend — Archivos a crear

| # | Archivo | Descripción |
|---|---|---|
| 1 | `features/actividades/realizar_recepcion_boleta/models/realizar_recepcion_boleta.ts` | Interface + factory EMPTY |
| 2 | `features/actividades/realizar_recepcion_boleta/models/controles.ts` | Interface controles (tipo_boleta, oficina_registro) |
| 3 | `features/actividades/realizar_recepcion_boleta/api/realizarRecepcionBoletaService.ts` | 5 llamadas HTTP |
| 4 | `features/actividades/realizar_recepcion_boleta/hooks/useRealizarRecepcionBoleta.ts` | useQuery consulta |
| 5 | `features/actividades/realizar_recepcion_boleta/hooks/useControlesRecepcionBoleta.ts` | useQuery catálogos |
| 6 | `features/actividades/realizar_recepcion_boleta/hooks/useUpsertRealizarRecepcionBoleta.ts` | useMutation guardar |
| 7 | `features/actividades/realizar_recepcion_boleta/hooks/useAvanzarRealizarRecepcionBoleta.ts` | useMutation avanzar |
| 8 | `features/actividades/realizar_recepcion_boleta/hooks/useEjecutarVUR.ts` | useMutation bot VUR |
| 9 | `features/actividades/realizar_recepcion_boleta/hooks/useDatosHeredadosRecepcionBoleta.ts` | useQuery datos heredados |
| 10 | `features/actividades/realizar_recepcion_boleta/components/DatosHeredadosSection.tsx` | Solo lectura (CA02) |
| 11 | `features/actividades/realizar_recepcion_boleta/components/VurSection.tsx` | Campos VUR + botón Ejecutar VUR |
| 12 | `features/actividades/realizar_recepcion_boleta/components/RecepcionBoletaSection.tsx` | Campos editables restantes |
| 13 | `features/actividades/realizar_recepcion_boleta/pages/realizar_recepcion_boleta_page.tsx` | Página principal |
| 14 | `routes/Routes.tsx` | Ruta: `realizar_recepcion_boleta/:id_expediente` |

### 5.1 Service — Llamadas HTTP

```typescript
import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarRecepcionBoleta } from '../models/realizar_recepcion_boleta';
import type { ControlesRecepcionBoleta } from '../models/controles';

const PATH_URL = '/api/realizar-recepcion-boleta';

export const realizarRecepcionBoletaService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<RealizarRecepcionBoleta | null>> {
    const response = await axiosClient.get<ApiResponse<RealizarRecepcionBoleta | null>>(
      `${PATH_URL}/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesRecepcionBoleta>> {
    const response = await axiosClient.get<ApiResponse<ControlesRecepcionBoleta>>(
      `${PATH_URL}/controles`,
    );
    return response.data;
  },

  async guardar(payload: RealizarRecepcionBoleta): Promise<ApiResponse<RealizarRecepcionBoleta>> {
    const response = await axiosClient.post<ApiResponse<RealizarRecepcionBoleta>>(
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

  async ejecutarVUR(id_expediente: number): Promise<ApiResponse<RealizarRecepcionBoleta>> {
    const response = await axiosClient.post<ApiResponse<RealizarRecepcionBoleta>>(
      `${PATH_URL}/ejecutar-vur/${id_expediente}`,
    );
    return response.data;
  },
};
```

### 5.2 Hooks

Mismo patrón que BBV-91/BBV-92:
- `useRealizarRecepcionBoleta` → `useQuery` con `queryKey: ['realizar_recepcion_boleta', id_expediente]`
- `useControlesRecepcionBoleta` → `useQuery` con `queryKey: ['controles_recepcion_boleta']`
- `useUpsertRealizarRecepcionBoleta` → `useMutation` + `invalidateQueries`
- `useAvanzarRealizarRecepcionBoleta` → `useMutation`
- `useEjecutarVUR` → `useMutation` + `invalidateQueries(['realizar_recepcion_boleta', ...])`
- `useDatosHeredadosRecepcionBoleta` → `useQuery` que consume firmarEscrituraClienteService.getByExpediente + validarInformacion (reutiliza patrón BBV-91)

### 5.3 Componentes

| Componente | Props | Descripción |
|---|---|---|
| `DatosHeredadosSection` | `datosHeredados` | Datos Cliente + Datos Notaría (readonly) — CA02 |
| `VurSection` | `form`, `isDisabled`, `updateField`, `onEjecutarVUR`, `isVurLoading` | Campos VUR (numero_boleta, fecha_boleta, matricula) + botón + alerta error |
| `RecepcionBoletaSection` | `form`, `isDisabled`, `updateField`, `tipoBoletaOptions`, `oficinaOptions` | Tipo boleta, código zona, oficina (Dropdown filterable), checkbox boleta recibida, boleta_en_poder_de, observaciones |

---

## 6. Frontend — Estructura de la página

### 6.1 Funciones Transversales

| Función transversal | ¿Aplica? | Notas |
|---|---|---|
| Expediente Digital | ✅ Sí | Carga/visualización — valida Folio(s) Previo(s) (CA07) |
| Trazabilidad / Bitácora | ✅ Sí | Historial de acciones |
| Carta de Aprobación | ❌ No | No aplica |
| Registro de Contacto | ❌ No | No aplica |

### 6.2 Wireframe

```
┌──────────────────────────────────────────────────────┐
│ Título: "Realizar Recepción Boleta"                  │
├──────────────────────────────────────────────────────┤
│ Acordeón 1: Información del Expediente               │
│   └─ EncabezadoActividad (solo lectura)              │
├──────────────────────────────────────────────────────┤
│ Acordeón 2: Funciones Transversales                  │
│   └─ Expediente Digital + Trazabilidad               │
├──────────────────────────────────────────────────────┤
│ Acordeón 3: Realizar Recepción Boleta                │
│                                                      │
│   ┌─ DatosHeredadosSection (solo lectura) ─────────┐│
│   │  Tipo Doc | Nro Doc | Nombres | Apellidos      ││
│   │  Tipo Crédito                                   ││
│   │  Ciudad Notaría | Nro Notaría | Nro Escritura  ││
│   └────────────────────────────────────────────────┘│
│                                                      │
│   ┌─ VurSection ───────────────────────────────────┐│
│   │  [Ejecutar VUR]  ← botón (CA04)               ││
│   │  ⚠️ Alerta si VUR falló                       ││
│   │  Nro Boleta (Radicado): InputText *            ││
│   │  Fecha Boleta: Calendar * (no futura)          ││
│   │  Nro Matrícula: InputText *                    ││
│   └────────────────────────────────────────────────┘│
│                                                      │
│   ┌─ RecepcionBoletaSection ───────────────────────┐│
│   │  Tipo Boleta: Dropdown (L44) *                 ││
│   │  Boleta En Poder De: InputText (condicional)   ││
│   │  Código Zona: InputText *                      ││
│   │  Oficina de Registro: Dropdown (L45) *         ││
│   │  ☑ Boleta Recibida: Checkbox *                 ││
│   │  ¿Aplica Excepción?: [SI/NO] (readonly)       ││
│   │  Observaciones: textarea (opcional)            ││
│   └────────────────────────────────────────────────┘│
│                                                      │
│   Botones: [Editar] [Guardar] [Avanzar] [Salir]     │
└──────────────────────────────────────────────────────┘
```

### 6.3 Flujo de estados UI

```
Estado inicial (carga desde backend):
  ┌─ Si tiene registro (id > 0): campos bloqueados, canAdvance=false
  │   └─ Si vur_ejecutado && !vur_exitoso: mostrar alerta VUR
  └─ Si no tiene registro: campos editables, canAdvance=false
       └─ Auto-ejecutar VUR al cargar por primera vez (CA03)

[Ejecutar VUR] → POST ejecutar-vur → actualiza campos si exitoso
[Editar] → isDisabled=false, canAdvance=false
[Guardar] → POST Save → si éxito: isDisabled=true, canAdvance=true
[Avanzar] → solo habilitado si canAdvance=true → valida → POST avanzar → navegar a bandeja
[Salir] → navigate('/home/bandeja')
```

---

## 7. Frontend — Validación (antes de Avanzar)

```typescript
const validateAvanzar = (): string[] => {
  const missing: string[] = [];

  if (!form.numero_boleta?.trim()) missing.push('Número de Boleta');
  if (!form.fecha_boleta) missing.push('Fecha de Boleta');
  if (form.fecha_boleta && new Date(form.fecha_boleta) > new Date()) missing.push('Fecha de Boleta (no puede ser futura)');
  if (!form.numero_matricula?.trim()) missing.push('Número de Matrícula');
  if (!form.tipo_boleta) missing.push('Tipo de Boleta');
  if (!form.codigo_zona?.trim()) missing.push('Código Zona');
  if (!form.oficina_registro) missing.push('Oficina de Registro');
  if (!form.boleta_recibida) missing.push('Boleta Recibida');

  // CA11 — Condicional por tipo boleta física
  if ((form.tipo_boleta === 'TBOL-2' || form.tipo_boleta === 'TBOL-3') && !form.boleta_en_poder_de?.trim()) {
    missing.push('Boleta En Poder De');
  }

  return missing;
};
```

---

## 8. Modelo TypeScript

```typescript
import type { Auditoria } from '@/models/Auditoria';

export interface RealizarRecepcionBoleta extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  numero_boleta: string | null;
  fecha_boleta: string | null;
  numero_matricula: string | null;
  tipo_boleta: string | null;
  boleta_en_poder_de: string | null;
  codigo_zona: string | null;
  oficina_registro: string | null;
  boleta_recibida: boolean;
  aplica_excepcion: string | null;     // "SI" | "NO" — readonly, calculado en backend
  vur_ejecutado: boolean;
  vur_exitoso: boolean;
  vur_intentos: number;
  observaciones: string | null;
}

export const EMPTY_REALIZAR_RECEPCION_BOLETA = (
  id_expediente: number,
): RealizarRecepcionBoleta => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA',
  numero_boleta: null,
  fecha_boleta: null,
  numero_matricula: null,
  tipo_boleta: null,
  boleta_en_poder_de: null,
  codigo_zona: null,
  oficina_registro: null,
  boleta_recibida: false,
  aplica_excepcion: null,
  vur_ejecutado: false,
  vur_exitoso: false,
  vur_intentos: 0,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
```

### 8.1 Interface de Controles

```typescript
import type { ControlBaseDTO } from '@/core/api/models/ControlBaseDTO';

export interface ControlesRecepcionBoleta {
  tipo_boleta: ControlBaseDTO[];
  oficina_registro: ControlBaseDTO[];
}

export const EMPTY_CONTROLES_RECEPCION_BOLETA: ControlesRecepcionBoleta = {
  tipo_boleta: [],
  oficina_registro: [],
};
```

---

## 9. Lo que NO se crea

- ❌ No se usa EF Migrations (script SQL directo)
- ❌ No se necesita Registro de Contacto
- ❌ No se necesita Carta de Aprobación
- ❌ No se necesita integración real del Bot VUR (se deja como TODO/placeholder con el endpoint listo)

---

## 10. Orden de ejecución

1. Ejecutar script SQL contra `BBVA_LEGALIZACION` (tabla + catálogos + cat_actividades_ws)
2. Crear Entity + EntityConfig + agregar al DbContext
3. Crear Repository (interface + impl)
4. Crear Domain Model + registrar AutoMapper
5. Crear Application (interface + impl con lógica VUR + excepción)
6. Crear Controller
7. Registrar en IoC + agregar constantes en Constants.cs
8. Frontend: model → controles → service → hooks → components → page
9. Agregar ruta en Routes.tsx
10. Probar flujo completo: carga + VUR (fallo) → manual → guardar → avanzar

---

## 11. Datos heredados (solo lectura) — CA02

| Bloque | Campo | Fuente | Tabla |
|---|---|---|---|
| Datos Cliente | Tipo Documento | `validar_informacion_bbva` | tipo_documento |
| Datos Cliente | Nro Documento | `validar_informacion_bbva` | numero_documento |
| Datos Cliente | Nombres | `validar_informacion_bbva` | nombres |
| Datos Cliente | Apellidos | `validar_informacion_bbva` | apellidos |
| Datos Cliente | Tipo Crédito | `validar_informacion_bbva` | tipo_credito |
| Datos Notaría | Ciudad Notaría | `firmar_escritura_cliente` | ciudad_notaria |
| Datos Notaría | Nro Notaría | `firmar_escritura_cliente` | numero_notaria |
| Datos Notaría | Nro Escritura | `firmar_escritura_cliente` | numero_escritura |

> Reutilizar el patrón implementado en BBV-91: consultar `firmarEscrituraClienteService.getByExpediente`
> desde el frontend. Para los datos de cliente, usar un endpoint existente de `validar_informacion`
> o agregar los campos al response de `GetByExpediente` del backend.

---

## 12. Script de Workflow (DBWFBBVA)

```sql
-- Actividad: Realizar Recepción Boleta (ya debería existir de BBV-92)
-- Verificar con: SELECT * FROM xpdl_activities WHERE activity_id = 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA';

-- Actividad destino: Realizar EP Registradas
INSERT INTO public.xpdl_activities (
    activity_id, workflow_process_id, display_name, name,
    task_type, task_form_type, task_form_uri, performer, sub_flow_id
)
SELECT
    'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS',
    'WP_BBVA_CONTACTO_CLIENTE',
    'Realizar EP Registradas',
    'Realizar EP Registradas',
    'TaskUser', 'UserDefined', 'realizar_ep_registradas', 'ANALISTA_VIVIENDA', NULL
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_activities
    WHERE activity_id = 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS'
);

-- Transición 1: Siempre → EP Registradas
INSERT INTO public.xpdl_transitions (
    transition_id, name, from_activity, to_activity, condition, workflow_process_id
)
SELECT
    'BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EP_REGISTRADAS',
    'BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EP_REGISTRADAS',
    'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA',
    'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS',
    'Otherwise',
    'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_transitions
    WHERE transition_id = 'BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EP_REGISTRADAS'
);

-- Transición 2: Si aplica excepción → Excepción Desembolso (paralela)
INSERT INTO public.xpdl_transitions (
    transition_id, name, from_activity, to_activity, condition, workflow_process_id
)
SELECT
    'BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EXCEPCION_DESEMBOLSO',
    'BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EXCEPCION_DESEMBOLSO',
    'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA',
    'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO',
    'Otherwise',
    'WP_BBVA_CONTACTO_CLIENTE'
WHERE NOT EXISTS (
    SELECT 1 FROM public.xpdl_transitions
    WHERE transition_id = 'BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EXCEPCION_DESEMBOLSO'
);
```

---

## 13. Diferencias clave con BBV-92

| Aspecto | BBV-92 (Entrega EP Firmada) | BBV-93 (Recepción Boleta) |
|---|---|---|
| Rol | Analista de Vivienda | Analista de Vivienda |
| Campos editables | Entregado a + Observaciones | 8 campos + checkbox |
| Campos calculados | `aplica_excepcion` (tipo crédito) | `aplica_excepcion` (tipo crédito + desembolso BOLETA) |
| Integración externa | Ninguna | Bot VUR (RPA) con reintento 3x |
| Endpoint extra | No | `/ejecutar-vur` + `/controles` |
| Dropdowns | No | Sí: L44 (tipo boleta), L45 (oficina registro) |
| Catálogos nuevos | Ninguno | 2 (L44, L45) |
| Validación documento | No | Sí: "Folio(s) Previo(s)" (CA07) |
| Checkbox bloqueante | No | Sí: "Boleta Recibida" (CA08) |
| Campo condicional | No | Sí: "Boleta En Poder De" si boleta física (CA11) |
| Destino principal | Recepción Boleta | EP Registradas |
| Destino paralelo | Excepción Desembolso | Excepción Desembolso (misma lógica) |

---

## 14. Notas de implementación

1. **Bot VUR (CA03/CA04):** Se crea el endpoint y la estructura de reintento pero la integración real con el servicio RPA queda como TODO. El frontend ya tiene el botón "Ejecutar VUR" funcional.

2. **Validación de documento Folio Previo (CA07):** Idealmente se valida contra el Expediente Digital. Si no existe un servicio para verificar documentos adjuntos, se puede postergar esta validación o implementarla como warning en lugar de bloqueo.

3. **Dependencia Oficina ↔ Código Zona (CA10):** Por simplicidad se implementa como lista independiente. Si se requiere filtrado, crear un catálogo `CODIGO_ZONA` y vincular L45 con `id_padre`.

4. **Reutilización de código:**
   - Patrón Application/Repository/Controller: idéntico a BBV-92
   - Lógica de excepción: reutiliza el patrón de BBV-92 con ajuste de condición (tipo_desembolso)
   - Datos heredados frontend: reutiliza `firmarEscrituraClienteService` (igual que BBV-91)
   - Hook de controles: mismo patrón que BBV-91 (`useControlesFirmarRepLegal`)
