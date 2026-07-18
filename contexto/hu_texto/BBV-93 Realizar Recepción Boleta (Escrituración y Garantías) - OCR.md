# BBV-93 Realizar Recepción Boleta (Escrituración y Garantías) · OCR

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


17728, 15:23

[W88V-93] HU - Actividad Realizar Recepción Boleta (Escrturación y G

rantias)

Epica - Presto Escrituración y Garantias BBVA Legalización wow

Í, [BBV-93] HU - Actividad Realizar Recepción Boleta (Escrituración y Garantías) css

Juas Atala: 18funze

Estado:

Tareas por hacer

Proyecto:

BBVA Colombia

Componentes:

Ninguno

Versiones afectadas:

Ninguno

Versiones corregidas:

Ninguno

Principal:

Epica - Presto Escrituración y Garantías BBVA Legalización

Tipo:

Historia

Prioridad:

Media

Informador:

¿Jorge Andres Garzon Paez

Persona asignada:

Jorge Andros Garzon Paez

Resolución:

‘Sin resolver

Votos:

o

Etiquetas:

BBVA_LEGALIZACION

Trabajo restanto estimado:

Desconocido

Tiempo Trabajade:

Desconocido

Estimación original:

Desconocido


## Descripción


**Yo como:** Analista de Vivienda.

De

10: Acceder al acordeón "Realizar Recepción Boleta" para visualizar a nformación precargada, ejecuar o validar la extracción automática de

alos regisrales meciante el bot del VUR (Número y Fecha de boleta, Matricula), confirmar la recepción fsicaldigital del documento y defn si aplica

alguna excepción comercial.

**Para:** Documentar formalmente el ingreso de la escritura en instrumentos públicos apoyándome en herramientas de automatización (RPA), y

determinar si elujo avanza hacia el registro de la EP o se desvía temporalmente al área comercial para gestionar una excepción de desembolso


## Alcance


sta funcionalidad corresponde a a pantalla transaccional donde se captura la información registra inicial (boleta). Opera dentro de subproceso de

crituración y Garantias. Su mayor innovación e la integración de un molor RPA (Bo! VUR) que intenta precargar los datos registrales ejecutándose.

hasta 3 vecos de forma automática; si fala, habita la captura manual y un botón de relntonto, Finaliza con un nodo de enrutamiento que evalúa la

necesidad de una excepción de desembolso,


## Criterios de aceptación


+ CAD (Criterio Global Transversal)

1. Elsistema debe renderizar y mantenerla estructura visual de los grupos de datos exactamente iguala como venían de la actividad anterior

(eredando la vista de Escrturacién y Garantias)

2 El sistema debe renderizar en la cabecera la Información General" (estrictamente de solo lectura).

3. El sistema debe incorporar el contenedor para "Funciones Transversales”, dividido en Expediente Digital y TrazabllidadíBitácora.

4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición Avanzar"

5. Trazabilidad: Al ejecutar "Avanzar, se registrará enla bitácora: Fecha, Actividad (Realizar Recepción Boleta), Usuario Ejecutor (Analista de

Vvienda), Decisión de Envulamiento y Observaciones.

+ CA02 (Acordeón "Realizar Recopcién Boleta" y Datos Horedados): El sistema debe desplegar un acordeón central nombrado "Realizar

Recepeién Bolota”. En osto bloque se mostrará an ostcto modo do solo lectura la información consolidada:

+ Datos Cliente: Tipo y Nro. Documento, Nombres y Apelidos, Tipo de Crédito.

+ Datos Notaria: Ciudad Nelaria, Nro Notaria y Número de Escritura,

+ CAD3 (Automatización RPA. Bot VUR y Extracción de Datos): Al ingresar a la pantalla (al dia siguiente hábil de avanzar la actividad de Realizar

Entrega EP Firmada), el sistema (Backend) debe ejecutar automáticamente la consulta al robot del VUR para extraer y autocompleta os siguientes

‘campos transaccionales

ps Jlebergestior tam alassian.nesifiaissueviewsissue-htm/BBV-83/BBV-83.Ntml

18


<!-- --- PAGE 2 --- -->


ems,

15:23

[188v-93] HU - Actvidad Realzar Recepción Boleta (Escrturación y Garantias)

.

Número de

boleta (Turno): (Radicado Registro)

.

Fecha de la boleta (Fecha Radicación): (Foca do ingroso con ostrcción d fecha futura),

.

Números de matricula(s) (Del documento descargado)

.

Regla de reintento: El motor del VUR debe ejecutarse intemamente un mínimo de 3 veces antes de decinar la búsqueda y arojar un fal.

+ CADA (Manejo de Errores VUR y Botón de Ejecución Manual): Si ras los 3 intentos el bot no logra extrae la información, e sistema debe:

1

Desplegar una aleta o mensaje estandarizado en pantalla que indique exactamente: "El VUR ha fallado por favor revisa los datos necesarios

a dilgenciar on la página [HU - Ejecución Robot VUR?".

Dejar los campos de captura vacios y habitados para que el Analsta de Vivienda los diigencie de forma manual

Habiitar un botón de acción denominado "Ejecutar VUR”, el cual permitirá al usuario forzar un nuevo intento de extracción a demanda si

considera que la plataforma ya se encuentra estable

+ CAOS (Campos Transaccionalos de Recepción Adicionales): Adomás de los campos extraídos por el VUR, ol sistema hablitará oblgatoriamonto:

+ Tipo de boleta: Lista desplegable parametizada (L44)

+ Código zona: Campo de texto/desplegadl,

© Oficina de registro: List

‘desplegable parametrizada (45) debe de serdigenciado por el robot cuando se elect

la consulta VUR.

+ Boleta Recibida: Un control tipo Checkbox obligatorio para cerifcar la tenencia del soporte registra.

+ CAOS (Campo de Decisión y Enrutamionto): Inmediatamente después de los campos de captura, el sistema debe habiltar el campo de lectura

"¿Aplica Excepción?” con las Indicaciones de SI - NO teniendo en cuenta las siguientes reglas

+ Siaplica Excepción:

+ Cuando la ConstructorafProyecto y el Tipo de Desembolso (LS) as BOLETA y apica los siguientes tipos de créditos aplica la excepción

» Leasing Usado

+ Hipotecario Usado

» Remodelacion Para Ampliar / Mipotecar

|.

Y Adicionalmente, debe de cumpli con el documento de la Falio(s) Previo(s) al momento de realzar avance en la actividad: El sistema habiltay

‘enruta ol uj paralelamente a a actividad "Realizar Excepción Desembolso”, asignando la area a cargo del rol Comercial Y Siempre debe de

ingresar ala actividad de "Realizar EP Registradas” A cargo del Analista de Vivenda.

l

Si es "NO" en el tipo de crédito o diferente al Tipo de Desembolso es “Bolt

y se avanza la actividad: El Mujo ingresará siempre por su ruta

natural hacia la actividad “

'salzar EP Registradas", asignando la tarea a cargo del mismo Analista de Vivienda.

l

CAO7 - Restriccién avance por documento: E sistema debe de identificar que en la actividad oblgatoriamento exista anexado en el expedionte

digital los documentos de: Fotos) Previos)

CAD8 - Bloqueo de Avance por Checkbox: El sistema no permit

‘accionar el botón "Avanzar si el checkbox "Boleta Reciida no ha sido

marcado afirmativamente

CAS = Prioridad de Datos VUR: Sil ot "Ejecutar VUR” extras la información exttosamonto los campos de Número de Boleta, Fecha y Matrícula

se autocompletarán. El analista podrá editarlos si detecta inconsistencias contra el soporte físico (OCR vs Fico)


### CA10 - - Dependencia Paramétrica: Los campos “Tipo de boleta" (44 y "Ofeina de registro" (Lé5) están restíngidos a ls valores homologados,


"Oficina de registro" debe auto-trarse dependiendo del "Código zona" ingresado.


### CA11 - - Dependencia Tipo de Boleta: Si el Tipo de Boleta es Fisica Original Física Copia habita un nuevo campo nombrado “Boleta En Poder


De vacio para diligenciar la información. Resaltar un oolip en el campo que mencione "Indicar la Oficina BBVA quien tiene el resguardo de a

boleta”


## Modelo de datos


Campo

Tipo do

Editablo Obligatorio

Reglas de Negocio / Origen

Dato

Datos Cliente

Varios.

No

si

Precargados de formularios previos (Solo Lectura)

Datos Notaría

Varios

st

si

Precargados de formularios previos y número de escritura.

Botón "Ejecutar VUR™

Acción

NA

NA

atl ol bot RPA manualmonto si esto faló on la carga inicia

Número de

bol

Alfanumérico

sí

si

(Radicado)

Extraido porel VUR o diigenciado manualmente si fala

Extraído por ol VUR o dilgenciado manvalmento s: fala restricción de fecha

Fecha de la boleta (ngroso)

Fecha

sí

si

futura,

Número de matrícula

Numérico

sí

si

Extraido porel VUR o diligenciado manualmente si fal

Tipo do boleta

Lista Despl

sí

si

Valores de la Parametría(Lá4).

Boleta En Poder De:

Texto

si

si

Se habilta cuando Tipo de Boleta es Fisica Original o Física Copia.

úAlfanumérica

sí

si

Admite ca

Código zona

lores especiales. Consicionala ocina

si

si

Oficina de registro

Lista Despi

Valores de la Parametría (LaS).

ps Jlebergestior tam alassian.nesifiaissueviewsissue-htm/BBV-83/BBV-83.Ntml

28


<!-- --- PAGE 3 --- -->


17728, 15:23

[188v-93] HU - Actividad Realzar Recepción Boleta (Escrturación y Garantias)

Boleta Recibida

Checkbox

sí

si

Control obligatoria de confirmación fiscal gia!

¿Aplica Excepción

Valores: "SÍ

"NO", Determina sí aplica la excepción del desembolso

Texto

No

No

Desombolso?

epenciando al tipo de crit,

Observaciones

Texto (Area)

si

No

Campo libre para documentar el fal del VUR u otras notas.

¡Generado alas Mon Jul 06 20:23:21 UTC 2026 por Michael Pulido usando JIRA 1001.0.0-SNAPSHOT#100292-

revie5fe927 aldde415031545c03210¢9 1484800205,

ps ebergestion tam alassian.nesifiaissueviewsissue-htm/BBV-83/BBV-83.Ntml

3
