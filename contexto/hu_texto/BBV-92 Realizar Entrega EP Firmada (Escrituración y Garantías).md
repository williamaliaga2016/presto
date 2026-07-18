# BBV-92 Realizar Entrega EP Firmada (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


17728, 15:23

[W8V-92] HU- Actividad Realizar Entrega EP Firmada (Escrturación y Garantias)

Epica -Presto Escrituración y Garantias BBVA Legalización poz

Í, [BBV-92] HU - Actividad Realizar Entrega EP Firmada (Escrituración y Garantías) crnsao2junzs Actatase 18juvzs

Estado:

Tareas por hacer

Proyecto:

BBVA- Colombia

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

Maia

Informador:

¿Jorge Andres Garzon Paez

Persona asignada:

Jorge Andros Garzon Paez

Resolución:

¡Sin resolver

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

o: Acceder al acordeón "Realizar Entrega EP Firmada” para visualizar la información heredada de la firma del Representante Legal, registrar

formalmanto a quién y en qué fecha se oniregó el documento y definir il operacién aplica o no para una excepción de desembalso.

**Para:** Continuar el flujo estándar hacia la recopción dela boleta registral o, on su defect, habitar la actividad de excepción de desembolso a cargo

del area comercial


## Alcance


a funcionalidad corresponde a una pantalla transaccional operativa y de contro, ubicada inmediatamente después de que el Representante Legal

mite su visto bueno de firma, Actúa como un paso de formalización donde el Analia de Vivienda documenta la saida de la escritura (capturando.

Fecha entrega y Entregado a) y evalia, mediante un campo de decisión galilador sil crédito sigue la ruta stándar hacia Instrumentos Públicos o si

habilita la ruta excepcional comercial


## Criterios de aceptación y reglas de negocio


+ CAD (Criterio Global Transversal)

1, Elsistema debe renderizar y mantenerla estructura visual de los grupos de datos exactamente iguala como venian de la actividad anterior

(heredando la vista de Escrturación y Garantias)

2 El sistema debe renderizar en la cabecera la Información General" (estrictamente de solo lectura).

3. El sistema debe Incorporar el contenedor para "Funciones Transversales”, dividido en Expediente Digital y Trazabllidad/Bitácora.

4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición Avanzar"

5. Trazabllidad: Al ejecutar "Avanzar, se registrará en la bitácora: Fecha, Actividad (Realizar Entrega EP Firmada), Usuario Ejecutor (Analista

de Vivienda), Decisión tomada (Aplica o no excepción) y Observaciones.

+ CA02 Regla de enrutamiento Excepción de Desembolso: El sistema debe de identifica las siguientes condicionales:

+ Ena paramética de LS Constructoras a parir dela constructoralproyecto en el registro del Tipo de Ds

.mbolso debe ser iguala

ESCRITURA para aplicarla excepción de desembolso teniendo en cuenta los siguientes tipos de crédito

+ Constructor individual

:

(Leasing Usado ))

.

Hipotecario, XI

.

Hipotecario Usado

.

Leasing Nuevo

Its Jlbergestior tam .atassian,nevsifiaissueviews:ssue-htmVBBV-921B8V.92.html

12


<!-- --- PAGE 2 --- -->


17728, 15:23

[188.92] HU- Actividad Realizar Entrega EP Firmada (Escrturación y Garantias)

+ Leasing, CX

» Remodelación Para Ampliar / Hipotecar

+ Aplica la Excepción del Di

mbolso

Y automáticamente genera el camino para la actividad de “Realizar Excepción Dosembolso", asignando la tarea a cargo del ol Comercial

En caso de que no sea el io de crédito siempre avanza naturalmente a la actividad Realizar Entrega EP Firmada (actual)

+ Tener en cuenta la Siguiente Lógica:

(CA03 (Acordeón “Realizar Entr

ja EP Firmada” y Herencia): El sistema debe desplegar un acordeón central nombrado "Realizar Entrega EP

Firmada". En este bloque se mostrará, en estricto modo de solo lectura, toda la información consolidada y el concepto de fima exitosa proveniente

de la pantalla previa (Firmar Rep. Lega)

CADA (Captura de Datos de Entrega): Dentro de la sección de gestion, el sistema habillaré obigaloriamente os siguientes campos nueves en

blanco para que el analista los diigencie:

+ Entregado a: Campo de texto para registrar el nombre de la persona, Iramilador o entdad que recibo la EP física

CAOS (Campo de Decisión y Enrutamiento): Inmediatamente después de los campos de captura, el sistema debe habitar el campo obligatorio

*¿Aplico Excepción?” con as Indicaciones de SI - NO teniendo en cuenta las siguientes reglas, este será únicamente de lectura.

+ Sies “St

on el Tipo de Crécito (CAO2) debe de mostrar SI

+ Sies "NO" en el tipo de crádito (CA02) deve de mostrar NO.

CAOS (Enrutamiento): El sistema al realizar el avance de la actividad debo de Ingresar directamente por su ruta natural hacia la actividad "Realizar

Recepción Boleta”, asignando la tarea a cargo del mismo Analista de Vivienda.


## Modelo de datos


Campo

Tipo de

Editable (Sh

Obligatorio

Reglas de Negocio / Origen

Dato

No)

Datos Heredados de Firmar

Información previa precargada en modo lectura (Incluye concepto

Varios

No

si

Rap. Legal

“Esctura firmada Conforme”)

Alfanumérico Si

si

Entregado a:

Nombre de la persona tramitador que recibe la escritura,

Valores: "SÍ “NO”. Determina si aplica la excepción del desembolso.

ú¿Aplico Excepción?

‘Texto

No

No

epondiando el tipo de eri

Observaciones

No

Campo loro opcional para deja notas sobro la entrega ola oxcopción

Texto (Area) SÍ

(Gi aplica),

¡Generado alas Mon Jul 06 20:23:12 UTC 2028 por Michael Pulido usando JIRA 1001.0.0-SNAPSHOTH100292-

revieSfe027aldde415031545-032(0¢9 1484800205,

itpsebergestion tam alassian.nesifiaissueviewsissue-htm/BBV-S2/BBV-82.Mtml

22
