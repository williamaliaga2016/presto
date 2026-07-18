# BBV-98 Preformalizar (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


617126, 15:25

[YBBV-98] HU - Actividad Preformalizar (Escrturación y Garantias)

Epica -Presto Escrituración y Garantías BBVA Legalización (vw

4 [BBV-98] HU - Actividad Preformalizar (Escrituración y Garantías) creada: 02unzs Actuatza:10Juni20

Estado:

Tareas por hacer

BBVA- Colombia

Proyecto:

Componente:

Ninguno

Versiones afectadas:

Ninguno

Versiones corregidas:

Ninguno

Principal:

Épica - Presto Escrituración y Garantias BBVA Legalización

Tipo:

Historia

Prioridad:

Media

Informador:

¿Jorge Andres Garzon Paez

Persona asignada:

\Jorge Andres Garzon Paez

Resolución:

Sin resolver

Votos:

o

iquetas:

BBVA LEGALIZACIÓN

Desconocido

‘Trabajo restante estimad:

Desconocido

Tiempo Trabajado:

Estimación original:

Desconocido


## Descripción


**Yo como:** Analista de Desembolso / Comercial (Radicación Manual)

**Deseo:** Acceder al acordeón "Preformalizar” para visualizarla información heredada de la actividad "Validar Cumplimiento de Políticas”, y seleccionar

la ruta operativa correspondiente para continuar el trámite.

**Para:** Definir y enrutar el lujo hacia la "Velidación de Condiciones de Desembols

o directamente hacia "Realizar Desembols

asegurando la

¡continuidad del proceso de liberación de fondos.


## Alcance


Esta funcionalidad corresponde a una pantalla transaccional operada por el área de Desembolsos. Su objetivo principal es heredar el contexto del

«cumplimiento de políticas y servir como una compuerta de decisión (enrutador). El analista definirá, mediante un campo de selección, si el crédito

requiere una etapa intermedia de validación de condiciones (retomando temporalmente al Analista de Vivienda) o si salta directamente a la ejecución

<del desembolso en su propia bandeja,


## Criterios de aceptación y reglas de negocio


+ CAD (Criterio Global Transversal):

1. El sistema debe renderizar y mantenerla estructura visual de los grupos de datos exactamente igual a como venian de la actividad anterior

("Validar Cumplimiento de Políticas”)

2, El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura con los datos más recientes).

3. El sistema debe Incorporar el contenedor para "Funciones Transversales”, dividido en Expediente Digital y Trazabilidad/Bitácora

4, Funcionalidad de Botones de Acción: Opciones de "Guardado" y Transición / Avanzar”

5. Trazabilidad: Al ejecutar "Avanzar', so registrará en la bitácora: Fecha, Actividad (Preformalizar), Usuario Ejecutor (Analista de Desembolso),

Decisión de enrutamiento y Observaciones,

+ GA02 (Acordeón "Preformalizar” y Herencia Completa): El sistema debe desplegar un acordeón central nombrad

"Preformalizar”. Este bloque

debe heredar y mostrar, en estricto modo de solo lectura, toda la información, variables y el checklist previamente gestionados en la actividad de

“Validar Cumplimiento de Políticas”, Posterior a la actividad de

fep. Legal” y si proviene de la actividad d

“Realizar Desembolso” otorgando

el contexto completo al Analista de Desembolso,

+ CA03 (Campo de Decisión y Enrutamiento): Dentro del acordeón, el sistema debe habiitar un campo transaccional obligatori (ej. "¿Siguiente

Accién?" o "Ruta do Avance") con las siguientes dos opciones de enrutamiento:

> Opción A ("Validar Condiciones Desembolso"): Al seleccionar esta opción y accionar “Avanzar” el jo transita hacia la actividad "Validar

Condiciones Desembolso", asignando y escalando la tarea al Analista de

ienda

112

hitps://cibergestion-latam atlassian nelisifja issueviews:issue-hlm/BBV-98/BBV-98 htm!


<!-- --- PAGE 2 --- -->


617/26, 15:25

[YBBV-98] HU - Actividad Preformalizar (Escrturación y Garantias)

+ Opción B(

ealizar Desembolso”): Al seleccionar esta opción y accionar “Avanzar”, el flujo transita hacia la actividad "Realizar

'Desembolso", manteniendo la tarea a cargo del Analista de Desembolso.


### CA04 - - Obligatoriedad de Enrutamiento: El sistema no permitrá que el Analista de Desombolso accione el botén "Avanzar" sin haber


seleccionado explicitamente una de las dos rutas de destino,

CAOS - Transparencia de Herencia: La interfaz debe garantizar que ningún dato proveniente de Cumplimiento de Políticas pueda ser alterado en

esta pantalla, protegiendo la integridad del dictamen previo.

CAO6 - Herencia de datos Realizar Desembolso: El sistema identifica si existió un reproceso desde la actividad de Realizar Desembolso y hereda

los datos que se hayan ingresado en la actividad

CAOT - Asignación de Rol. El sistema debe de identificar el origen del caso si fue una radicación manual y en caso de que si debe de caer al

Comercial que venía trabajando el caso.


## Modelo de datos


Editable

Obligatorio

Campo

Tipo de Dato

(SiINo)

(SiINo)

Reglas de Negocio / Origen

Datos Heredados de

Incluye checklist de áreas y datos operacionales previos.

Varios

No

si

Cumplimiento de Pe

ticas

Estrcto solo lectura.

ista

si

Valores:

jlidar Condiciones Desembols:

Realizar

Desplegable

Desembols:

Compuerta de decisión obligatoria

Campo libre para documentar alguna eventualidad antes de

Observaciones

Texto (Área)

sí

No

despachar el caso a la siguiento etapa.

¡Generado a las Mon Jul 06 20:25:14 UTC 2026 por Michael Pulido usando JIRA 1001.0.0-SNAPSHOT#100292-

rev:c5le927afdde415031545c6321c49114848002e5,

2

hitps:/cibergestionatam atlassian neUsifja.Issueviewssissue-hmi/BBV-98/BBV-98 htm!
