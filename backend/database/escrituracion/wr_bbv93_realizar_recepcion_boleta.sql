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


-- ============================================================
-- Catálogos
-- ============================================================

-- L44 — Tipo de Boleta
WITH l44(codigo, descripcion, orden) AS (
    VALUES
        ('TBOL-1', 'Digital', 1),
        ('TBOL-2', 'Física Original', 2),
        ('TBOL-3', 'Física Copia', 3)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'L44_TIPO_BOLETA', l44.descripcion, l44.codigo, NULL, true, l44.orden
FROM l44
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'L44_TIPO_BOLETA' AND c.valor = l44.codigo
);

-- L45 — Oficina de Registro (listado oficial de Oficinas Registrales de Colombia)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'L45_OFICINA_REGISTRO', v.descripcion, v.codigo, NULL, true, v.orden
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
    WHERE c.tipo = 'L45_OFICINA_REGISTRO' AND c.valor = v.codigo
);


-- ============================================================
-- Registro en cat_actividades_ws (bandeja)
-- ============================================================

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar Recepción Boleta', 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_recepcion_boleta', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA');

INSERT INTO public.cat_actividades_ws (actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa, tiempo_promedio, is_active, row_status, created_by, created_date)
SELECT 'Realizar EP Registradas', 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS', 'WP_BBVA_CONTACTO_CLIENTE', 'Escrituración', 1, 'actividad', 'realizar_ep_registradas', '1', 1, true, true, 'admin', NOW()
WHERE NOT EXISTS (SELECT 1 FROM cat_actividades_ws WHERE id_actividad = 'BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS');
