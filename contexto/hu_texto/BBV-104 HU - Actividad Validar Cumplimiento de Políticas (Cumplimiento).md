# BBV-104 HU - Actividad Validar Cumplimiento de Políticas (Cumplimiento)

> Documento convertido desde PDF para edición y análisis en Kiro.

Épica - Presto Cumplimiento BBVA Legalización (BBV-121)

[BBV-104] HU - Actividad Validar Cumplimiento de Políticas (Cumplimiento) Creada:
02/jun/26 Actualizada: 07/jul/26

Estado: Tareas por hacer
Proyecto: BBVA - Colombia
Componentes: Ninguno
Versiones Ninguno
afectadas:
Versiones Ninguno
corregidas:
Principal: Épica - Presto Cumplimiento BBVA Legalización

Tipo: Historia Prioridad: Media
Informador: Jorge Andres Garzon Paez Persona asignada: Jorge Andres Garzon Paez
Resolución: Sin resolver Votos: 0
Etiquetas: BBVA_LEGALIZACION
Trabajo restante Desconocido
estimado:
Tiempo Trabajado: Desconocido
Estimación original: Desconocido


## Descripción


## Descripción

**Yo como:** Analista de Vivienda.

**Deseo:** Visualizar la información heredada de la actividad anterior en el acordeón "Cumplimiento de
Políticas", validar mediante un checklist las áreas escaladas, definir el enrutamiento según el estado del
inmueble e ingresar el correo para notificar a la notaría.

**Para:** Garantizar el cumplimiento normativo de la solicitud, notificar a los entes externos involucrados y
avanzar el flujo correctamente (hacia Definir Inmueble, Devolución a Comercial o continuar el flujo natural).


## Alcance

La funcionalidad abarca una pantalla transaccional clave en el subproceso de Cumplimiento. Su objetivo
principal es actuar como un punto de control (checklist) para verificar que todas las áreas exigidas por el tipo
de crédito fueron gestionadas. Además, incorpora lógica de enrutamiento dinámico condicionado por la
definición del inmueble y un disparador automático (gatillero) de correos electrónicos hacia la Notaría al
momento de avanzar de etapa.


## Criterios de aceptación y reglas de negocio


### CA01 - Criterio Global Transversal

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos (pestañas o
acordeones) exactamente igual a como venían de la actividad anterior ("Gestionar
Escalamientos").
2. El sistema debe renderizar en la parte superior una sección fija denominada "Información
General", estrictamente de solo lectura a nivel de interfaz directa, mostrando los datos más
recientes.
3. El sistema debe incorporar el contenedor para las "Funciones Transversales", dividido en
Expediente Digital (carga y verificación de documentos), Trazabilidad/Bitácora (historial de
acciones), Registro de Contacto, Carta de Aprobación y Carta de Disminución.
4. Funcionalidad de Botones de Acción: Contar con opciones claras de "Guardado" y de
"Transición / Avanzar".
5. Trazabilidad: Al completar la actividad y ejecutar "Avanzar", el sistema registrará en la
bitácora: Fecha, Actividad (Validar Cumplimiento de Políticas), Usuario Ejecutor y
Observaciones.

### CA02 - Acordeón de Cumplimiento y Checklist de Áreas El sistema debe presentar un grupo de

datos nombrado "Cumplimiento de Políticas". Dentro de este, se debe renderizar:
o La información heredada en modo lectura de la actividad "Gestionar Escalamientos".
o Un Checklist dinámico que liste cada una de las áreas escaladas. Los ítems de este checklist
variarán automáticamente dependiendo del tipo de crédito seleccionado previamente y si se
aplicó algún escalamiento adicional.

### CA03 - Lógica de Continuidad y Enrutamiento por Inmueble El sistema debe bloquear las

preguntas de avance hasta que el checklist del Criterio 2 esté completamente marcado. Una vez
chequeado en su totalidad, se mostrará la pregunta "¿Tiene inmueble definido?":
o Si "NO" (Inmueble NO definido): Al avanzar, el flujo se enruta directamente a la actividad
Definir Inmueble (a cargo del mismo Analista de Vivienda dentro de Cumplimiento).
o Si "SÍ" (Inmueble SÍ definido): El sistema despliega una nueva pregunta: "¿Requiere Escalar
a Comercial?".
▪ Si Comercial = "SÍ": Al avanzar, el flujo se enruta a la actividad Realizar Devolución
Pendiente VB Comercial.
▪ Si Comercial = "NO": El sistema despliega obligatoriamente un Checkbox adicional
con el texto: "Confirmar que todas las áreas han sido gestionadas".

### CA04 - Notificación a Notaría El sistema debe mostrar una sección de "Emisión de Notificación a

Notaría". Esta sección contendrá un campo de texto con validación de formato tipo Email. Al accionar
el botón principal de "Transición / Avanzar" de la pantalla, el sistema utilizará este correo como
gatillero (trigger) para enviar automáticamente la notificación correspondiente a la notaría adjuntando
documentos. Este campo únicamente aplica cuando el Tipo de Crédito:
o Hipotecario Usado
o Leasing Usado
o Remodelación Para Ampliar / Hipotecar
o Constructor individual
- Adicionalmente, debe de mostrar el campo del “Nombre de la notaria” donde al seleccionar (permitir
filtro y búsqueda en el campo) la notaria debe poblar el campo previo de tipo Email según la
coincidencia de la Parametría encontrada y permitir ingresar otro correo separada por “,“.

### CA04 - -2 (Notificación a la Constructora El sistema debe mostrar un campo (editable) de tipo Email

relacionada directamente a la Constructora y el Proyecto (ya poblada en el Folio) y este cumple
obligatoriamente a los siguientes tipos de crédito.
o Hipotecario Nuevo

o Hipotecario, CXI
o Leasing Nuevo
o Leasing, CXI
- Adicionalmente se debe de validar si la constructora cumple las garantías cumplidas
realizarse el envío de la Notificación. Si NO cumple y se realiza la selección del botón de Avanzar
(gatillero) de la pantalla el sistema utilizara este correo desplegado en relación a la coincidencia de la
**Para:** metria encontrada y enviara la notificación correspondiente a la constructora.

### CA05 - - Dependencia de Validación: El bloque de decisión (Inmueble y Comercial CA03

permanecerá oculto o inhabilitado hasta que el usuario marque afirmativamente todas las opciones
presentadas en el checklist de áreas.

### CA06 - - Validación de Gatillero de Correo: Si el flujo avanza por el camino regular (Inmueble SÍ y

Comercial NO), el campo de email de la notaría pasará a ser de diligenciamiento opcional y requerirá
estructura válida (usuario@dominio.com) para permitir la transición.

### CA07 - - Emisión de Garantías Cumplidas: El sistema debe de permitir la opción de lograr enviar una

notificación hacía la Constructora VIP o Financiado para emitir las garantías cumplidas. Teniendo en
cuenta que es visible si la constructora es VIP. Teniendo en cuenta la paramétrica de L5 -

### CA08 - - Porcentaje de Financiación: El sistema debe de habilitar la opción de botón de cumplimiento

Indicando mediante un mensaje si SI Cumple o NO Cumple la validación. Sino cumple este criterio no
se puede avanzar la etapa hacia Firmar Escritura del Cliente. Ver HU: https://cibergestion-
latam.atlassian.net/browse/BBV-86

### CA09 - - Solicitud Causación 490: Si el crédito es Leasing muestra el campo de ¿Requiere Causar?:

Y Si se seleccionar que SI Requiere Causar el sistema habilita y dispara el subproceso “Gestión
Leasing“ ingresando específicamente a la actividad "Realizar Causación" a cargo del Analista de
Leasing. Si Requiere Causar debe de mostrar un mensaje de alerta en Modal que mencione: “¿Estás
seguro que requieres realizar causación?” Si se indica que SI avanza a Gestión Leasing, sino cierra el
mensaje de alerta.
o Leasing Nuevo
o Leasing Usado
o Leasing CXI

### CA10 - - Calculo de Financiación Fallido: Si en el calculo de financiación no se ha logrado realizar

avanzar la actividad.

### CA11 - - Restricción de avance por documentos: El sistema no debe de permitir avanzar a la siguiente

actividad (Firmar Escritura del Cliente) si algún documento en el Expediente Digital se encuentra en
estado vencido.

### CA12 - - Visualización mensaje de cobertura: El sistema debe de identificar si el Folio es un Tipo de

Crédito Nuevo y cumple con alguno de los siguiente tipos de crédito
o Hipotecario Nuevo
o Hipotecario, CXI
o Leasing Nuevo
o Leasing, CXI
- Adicionalmente debe de cumple que también es una constructora VIP o Financiada para mostrar la
alerta informativa en la pantalla indicando “Cliente puede aplicar a Reduce Tu Cuota, por favor haz
revisión de la cobertura.“


## Modelo de datos


Editable Obligatorio
Campo Tipo de Dato Reglas de Negocio
(Sí-No) (Sí-No)
Datos Heredados Información de contexto heredada de la etapa
Varios No Sí
de Escalamientos previa.
Su longitud depende del tipo de crédito y
Checklist Áreas Múltiple
Sí Sí escalamientos adicionales. Deben estar todos
Escaladas Selección
en "SÍ" para habilitar las siguientes preguntas.
¿Tiene inmueble Lista Visible solo tras completar el checklist.
Sí Condicionado
definido? Desplegable Valores: "SÍ", "NO".
¿Requiere Escalar Lista Visible y obligatorio solo si ¿Tiene inmueble
Sí Condicionado
a Comercial? Desplegable definido? = "SÍ".
Confirmación de Visible y obligatorio solo si ¿Requiere Escalar
Checkbox Sí Condicionado
áreas gestionadas a Comercial? = "NO".
Gatillador de notificación. Obligatorio al
Texto avanzar si la solicitud sigue su curso normal.
Email Notaría Sí Condicionado
(Email) Este campo únicamente aplica cuando el Tipo
de Crédito CA04.
Gatillador de notificación. Obligatorio al
Email Texto avanzar si la solicitud sigue su curso normal.
Si Condicionado
Constructora (Email) Este campo únicamente aplica cuando el Tipo
de Crédito CA04-2.
Botón Porcentaje
Botón SI SI Calculo del porcentaje de Financiación
de Financiación
SNAPSHOT#100292-rev:59691b10846014e3077a45b36b7ad260711deea3.
