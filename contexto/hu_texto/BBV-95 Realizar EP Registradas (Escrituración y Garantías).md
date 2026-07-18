# BBV-95 Realizar EP Registradas (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


6/7/26, 15:24

[#BBV-95] HU - Actividad Realizar EP Registradas (Escrituración y Garantías)

Epica - Presto Escrituración y Garantías BBVA Legalización (ssv-122)

+ [BBV-95] HU - Actividad Realizar EP Registradas (Escrituración y Garantías) creaca: o2juni26 Actualizada: 19Jjun/26

Estado:

Tareas por hacer

Proyecto:

BBVA- Colombia

Componentes:

Ninguno.

Versiones afectadas:

Ninguno

Versiones corregidas:

Ninguno

Princip:

Epica - Presto Escrituración y Garantías BBVA Legalización

Historia

Pri

idad

Media

Informador:

Jorge Andres Garzon Paez

Persona asignada:

Jorge Andres Garzon Paez

Resolución:

Sin resolver

Votos:

o

Etiquetas:

BBVA_LEGALIZACION

Trabajo restante estimado:

Desconocido

Tiempo Trabajado:

Desconocido

Estimación original:

Desconocido


## Descripción


**Yo como:** Analista de Vivienda.

**Deseo:** Acceder al acordeón "Realizar EP Registradas" para visualizar en modo lectura la información de radicación de la boleta, registrar los datos de

recepción física de la escritura y confirmar que ha sido registrada exitosamente.

**Para:** Formalizar el registro ante la oficina de instrumentos públicos y avanzar el flujo obligatoriamente hacia la revisión final por parte del Abogado.


## Alcance


Esta funcionalidad corresponde a una pantalla transaccional de confirmación operada por el Analista de Vivienda dentro del subproceso de

Escrituración y Garantías. Actúa como un punto de control final (check-point) que certifica que el trámite registral ha concluido y que el documento

físico ha sido recibido. Consolida la información heredada de la radicación previa (boleta) y utiliza un único control de validación para transitar el caso

de manera directa y lineal hacia el Abogado, sin ramificaciones ni compuertas complejas.


## Criterios de aceptación y reglas de negocio


+ CA01 (Criterio Global Transversal):

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior

(heredando la vista de Escrituracién y Garantías)

2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura).

3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en Expediente Digital y Trazabilidad/Bitácora,

4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar".

5. Trazabilidad: Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Realizar EP Registradas), Usuario Ejecutor (Analista de

Vivienda), Confirmación de registro y Observaciones.

+ CA02 (Acordeón "Realizar EP Registradas"

Datos Heredados): El sistema debe desplegar un acordeón central nombrado "Re:

ar EP

Registradas". Este bloque mostrará en estricto modo de solo lectura la información consolidada del trámite para dar contexto a la confirmación.

© Datos del Cliente y Notari:

(Pre-cargados desde la radicación)

= Datos de Recepción Boleta: Fecha de ingreso, Radicado Registro, Tipo de boleta, Oficina de registro y Número de matrícula.

+ CAO3 (Captura de Datos de Recepción Fisica): Dentro de la sección operativa, el sistema habilitará obligatoriamente los siguientes campos

nuevos para documentar la tenencia del soporte: Teniendo en cuenta la funcionalidad del robot que se ejecuta para la extracción final de la

información. Ver HU: [BBV- HU - Ejecución Robot de Extracción Datos VUR]

© Finalización: Consistencia de Inscripción.

© Causal: Resultado del proceso.

12


<!-- --- PAGE 2 --- -->


6/7/26, 15:24

[#BBV-95] HU - Actividad Realizar EP Registradas (Escrituración y Garantías)

o Fecha Finalización: Fecha aplicada en el proceso de culminación del registro.


### CA04 - Confirmación de Registro Junto a los datos de recepció


, el sistema debe habilitar obligatoriamente un contro! tipo Checkbox con el

enunciado: "Confirmación de EP Registrada".

CAOS (Enrutamiento Lineal hacia el Abogado): Una vez marcado el check, diligenciados los campos de recepción y accionado el botón

“Avanzar”, el flujo transitará de manera incondicional hacia la actividad "Realizar VB Final Abogado", asignando automáticamente la tarea al rol

Abogado.


### CA06 - - Nuevo Campo Tipologías Garantías: Adicionar un nuevo campo donde permite realizar múltiples registros en la información de las


tipologías. Permite adicionar varias opciones y dejar el registro de las seleccionadas. Parametría L53.


### CA07 - - Bloqueo de Avance Sin Confirmación: El sistema impedirá que el Analista de Vivienda transite la actividad si no ha marcado


explícitamente el check de "

:onfirmación de EP Registrada" y diligenciado los campos de recepción física. Esta acción es la garantía de que el

proceso registral finalizó correctamente.

CAQ7 - Ausencia de Devoluciones o Desvios: De acuerdo con el flujo de proceso, esta actividad es netamente lineal. No existirán opciones de

"Concepto Favorable/No Favorable" ni campos de escalamiento comercial en esta pantalla.


## Modelo de datos


Tipo de

Editable (Si-

Obligatorio (Si-

Campo

Dato

No)

No)

Reglas de Negocio / Origen

Datos Cliente y Notaría

Varios

No

si

Heredados de etapas previas (Tipo Crédito, CC, Ciudad Notaria,

etc.)

Heredados de la captura previa (Radicado, Fecha ingreso,

Datos Recepción Boleta

Varios

No

si

Matricula, Oficina). Solo lectura.

Finalización

Fecha

Si

sí

Captura obligatoria de la fecha de entrega del documento VUR.

Causal

Alfanumérico

Si

sí

Captura obligatoria del VUR.

Fecha Finalización

Fecha

Si

si

Captura obligatoria de la fecha de entrega del documento VUR.

List. Despl.

si

sl

Tipologías Garantías

**Para:** metría L53 - Tipologías Garantías,

Confirmación de EP

Checkbox

si

si

Control obligatorio de validación operado por el Analista de

Registrada

Vivienda. Gatillador de avance.

Campo libre para que el Analista documente alguna nota

Observaciones

Texto (Área)

sí

No

relevante del cierre registral.


rev:c5fe927afdded 15031546c632fc49 114848e0265,
