# BBV-97 Realizar Gestión Comercial (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


617126, 16:25

[#BBV-97] HU - Actividad Realizar Gestión Comercial (Escrituración y Garantías)

Epica - Presto Escrituración y Garantías BBVA Legalización pov

4 [BBV-97] HU - Actividad Realizar Gestión Comercial (Escrituración y Garantías) creaca: 02jun/26 Actualizada: 19/jun/26

Estado:

‘Tareas por hacer

BBVA- Colombia

Proyecto:

‘Componentes

Ninguno

Versiones afectadas:

Ninguno

Versiones corregidas:

Ninguno

Principal:

Épica - Presto Escrituración y Garantías BBVA Legalización

Historia

Prioridad:

Media

Informador:

Jorge Andres Garzon Paez

Porsona asignada:

Jorge Andres Garzon Paez

Resolución:

Sin resolver

Votos:

o

Etiquetas:

BBVA_LEGALIZACION

‘Trabajo restante estimado:

Desconocido

smpo Trabajado:

Desconocido

Estimación or

inal:

Desconocido


## Descripción


**Yo como:** Comercial.

**Deseo:** Acceder al acordeón "Realizar Gestión Comercial” para visualizar en modo lectura la información del caso escalado (desde Firmar Escritura,

Devolución EP o Validar Condiciones de Desembolso) y registrar si el ciente desiste o no de la operación.

**Para:** Dar por terminado el trámite (Fin Terminal) si el cliente ya no desea continuar, o retomar el caso a la actividad de origen exacta para que retome

‘su curso operativo.


## Alcance


Esta funcionalidad corresponde a una pantalla transaccional resolutiva a cargo del rol Comercial. Funciona como un punto de salvamento o cierre del

crédito. La interfaz es minimalista: hereda dinámicamente todo el contexto de la actividad que originó el escalamiento (para que el comercial entienda

el problema) y habilita un único campo de decisión transversal, Dependiendo de la respuesta, el sistema cancela la operación o la devuelve a la

bandeja del usuario que solicitó el apoyo comercial.


## Criterios de aceptación y reglas de negocio


+ CA01 (Criterio Global Transversal):

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior

2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura con los datos más recientes).

3. El sistema debe incorporar el contenedor para "Funciones Transversales”, dividido en Expediente Di

I y Trazal

ladi

itécora.

4, Funcion

lad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar

5, Trazabi

lad: Al ejecutar "Avanzar

se registrará en la bitácora: Fecha, Actividad (Realizar Gestión Comercial), Usuario Ejecutor (Comercial),

Decisión de Desistimiento y Observaciones.

+ CA02 (Acordeón "Realizar Gestión Comercial" y Herencia Dinámica): El sistema debe desplegar un acordeón central nombrado "Realizar

Gestión Comercial". Este bloque mostrará en estricto modo de solo lectura toda la información del caso. Su contenido se adaptará dependiendo

del momento en el que fue escalado:

+ Siproviene de

mar Escritura Client

Heredará los datos de notaría y cliente.

© Si proviene de "Realizar Devolución EP": Mostrará adicionalmente los conceptos de rechazo de las áreas (Abogado, Prorrata, Leasing).

© Si proviene de "Validar Condiciones Desembolso": Mostrará el contexto financiero o de liquidación de la operación,

+ CA03 (Compuerta de Desistimiento y Cierre): El acordeón habilitará obligatoriamente el campo de selección: "¿Cliente Desiste del Caso?" con

las opciones

ya

112


<!-- --- PAGE 2 --- -->


617126, 18:25

[#BBV-97] HU - Actividad Realizar Gestión Comercial (Escrituracién y Garantías)

+ CA04 (Enrutamiento Multivia Condicionado): Al accionar el botón "Avanzar’, el sistema evaluará la respuesta y el origen del escalamiento para

enrutar la tarea:

+ Siselecciona

* (Desiste): El flujo avanza automáticamente hacia el evento de

in Terminal”, cerrando el caso de manera definitiva.

+ Siselecciona

10" (No desiste, el caso continúa): El sistema leerá la bandera de origen y devolverá el trámite a la actividad exacta que lo

escaló:

+ Retomaa

irmar Escritura Cliento" (Notaria/Analista de Vivienda).

+ Retornaa

tealizar Dovolución EP" (Analista de Vivienda)

+ Retorna a "Validar Condiciones Desembolso" (Analista de Vivienda).

CAOS - Bandera de Origen del Escalamiento: El motor de workflow debe inyectar una variable invisible (Bandera/Flag) en el formulario que guarde

el ID o nombre de la etapa que gatiló a "Realizar Gestión Comercial". Esta bandera es obligatoria para garantizar que, si el cliente no desiste, el

sistema sepa exactamente a qué bandeja regresar la tarea


### CA06 - Confirmación de Cierre y Cancelación de SLAs


«+ Estando en el escenario de Desistimiento (SÍ, al accionar el botón “Avanzar” el sistema debe bloquear el avance inmediato y mostrar un

modal de alerta o advertencia con el siguiente mensaje exacto:

“¿Estás seguro de dar cierre al Folio [Número de Folio]?. Ten en cuenta que el avance del Folio no podrá ser recuperado."

+ Si se selecciona NO en el modal: El modal se cierra, la pantalla permanece en su estado actual conservando todos los datos diligenciados.

© Si se selecciona SÍ en el modal: El orquestador finaliza el subproceso "Cumplimiento", actualiza el estado de todo el Folio a

'Cancelado/Desistido", y detiene/cancela de forma inmediata cualquier SLA o actividad que estuviese pendiente en el sistema para dicho

caso,


## Modelo de datos


Editable

Obligatorio

Campo

Tipo de Dato

Reglas de Negocio / Origen

(Si/No)

(SÍ/No)

Bandera;

Texto/Booleano

No

Si

Oculto (Backend). Registra de dónde viene la tarea para habilitar

Origen_Escalamiento

el retorno correcto.

Datos Heredados del

Varios

No

Si

Bloque de datos precargados en modo solo lectura,

Origen

dependientes del origen,

Lista

Valores: "SÍ"

"NO". Compuerta de decisión central de la

¿Cliente Desiste del Caso?

Si

Si

Desplegable

actividad,

Observaciones

‘Campo libre recomendado para justificar acuerdos con el cliente

Texto (Area)

sí

No

Comerciales

o razones del desistimiento,


rev:c5te927afdded 15031546c632fc49 11484800285

2
