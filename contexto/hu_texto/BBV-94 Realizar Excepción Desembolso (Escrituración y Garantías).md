# BBV-94 Realizar Excepción Desembolso (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


6/7/26, 15:24

[#BBV-94] HU - Actividad Realizar Excepción Desembolso (Escrituración y Garantías)

Epica - Presto Escrituración y Garantías BBVA Legalización (ssv-122)

4 [BBV-94] HU - Actividad Realizar Excepción Desembolso (Escrituración y Garantías) creada: oajuni26 Actualizada:

19/uniz6

Estad

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

Épica - Presto Escrituración y Garantías BBVA Legalización

Historia

Pri

idas

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

Desconocido

Trabajo restante estimado:

Desconocido

Tiempo Trabajado:

Desconocido

Estimación original:


## Descripción


**Yo como:** Comercial

**Deseo:** Acceder al acordeón "Realizar Excepción Desembolso" para visualizar la información del crédito, confirmar mediante un check de validación

que se continúa con la excepción de desembolso, y definir si la operación requiere una aprobación superior por parte de la Gerencia

**Para:** Autorizar formalmente el salto del proceso registral y avanzar el flujo directamente hacia la validación de desembolsos, o, en caso de requerirse,

escalar el trámite hacia la actividad de "Realizar Vobo Gerencia COH" para obtener el aval del gerente.


## Alcance


Esta funcionalidad corresponde a una pantalla transaccional resolutiva operada por el área Comercial. Se activa cuando el Analista de Vivienda marcó

que el crédito aplicaba para una excepción previa al registro. Su objetivo principal es asegurar la trazabilidad de la autorización comercial mediante un

check explícito de confirmación. Con la nueva actualización, esta pantalla se convierte en un nodo de enrutamiento condicionado: evalúa si la

excepción puede ser aprobada directamente por el Comercial (pasando a Desembolsos) o si por políticas de atribución requiere ser escalada al

Gerente COH para un visto bueno adicional.


## Criterios de aceptación y reglas de negocio


+ CADA (Criterio Global Transversal):

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior

(heredando la vista de Escrituración y Garantías),

2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura).

3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en Expediente Digital y Trazabilidad/Bitácora.

4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar"

5. Trazabilidad: Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Realizar Excepción Desembolso), Usuario Ejecutor

(Comercial), Decisión de Enrutamiento (Gerencia o Desembolso) y Observaciones,

+ CA02 (Acordeón "Realizar Excepción Desembolso" y Herencia Dinámica): El sistema debe desplegar un acordeón central nombrado "Realizar

Excepción Desembolso”. Este bloque mostrará en estricto modo de solo lectura la información del caso, adaptándose dinámicamente según su

origen:

© Si proviene de

irmar Rep. Legal

Mostrará los datos básicos del cliente, la notaría y el concepto de firma exitosa.

+ Si proviene de "Realizar Recepción Boleta": Mostrará, adicional a lo anterior, todos los datos de radicación en la oficina de registro (Fecha

de ingreso, Radicado, Tipo de boleta, Oficina, etc.)

12


<!-- --- PAGE 2 --- -->


6/7/26, 15:24

[#BBV-94] HU - Actividad Realizar Excepción Desembolso (Escrituración y Garantías)

.


### CA03 - Registro Informa?


‘0 de Autorización): Dentro de la sección operativa, el sistema habilitará un nuevo campo obligatorio denominado

“Excepción Autorizada" con los valores "Si" / "NO".

+ Regla de Comportamiento: Este campo será de carácter Únicamente informativo; no ejecutará ninguna acción de sistema, no habilitará otras

pestañas y no afectará el enrutamiento del flujo. Sirve exclusivamente como constancia de la gestión comercial.


### CA04 - Confirmación de Excepción Comercial Adicional al campo anterior, el sistema debe habilitar obligatoriamente un control tipo Checkbox (o


Lista Desplegable) con el enunciado: "Confirmar continuación con excepción de desembolso”. El usuario Comercial deberá marcar

obligatoriamente este check para poder accionar el avance de la etapa.


### CA05 - Compuerta de Escalamiento a Gerencia El sistema habilitará un nuevo campo de decisión obligatorio denominado "¿Requiere Vobo


Gerencia?" con los valores "SI" / "NO". Este campo determinará la ruta de salida de la actividad.


### CA06 - Lógica de Enrutamiento Condicionado Una vez diligenciados los campos, confirmado el check y accionado el botón “Avanzar”, el flujo


evaluará la respuesta del CA-05:

.

© Sise selecciona

'SÍ" en ¿Requiere Vobo Gerencia?: El flujo se desvía a una nueva actividad denominada "Realizar Vobo Gerencia

COH", y el sistema asignará la tarea al rol de Gerente COH.

> Si se selecciona "NO" en ¿Requiere Vobo Gerencia?: El flujo saldrá del actividad actual e ingresará directamente a la actividad "Validar

Condiciones Desembolso”, asignando la tarea al rol Analista de Vivienda.


### CA07 - Bloqueo de Avance y Origen Transparente El sistema impedirá que el Comercial transite la actividad si no ha activado explícitamente el


check de continuación de excepción (CA-04). Adicionalmente, la interfaz no requerirá que el Comercial le indique de dónde viene el caso; el sistema

identificará automáticamente la procedencia y precargará los bloques de datos correspondientes.


### CA08 - - Lógica de Escalamient


a compuerta "¿Requiere Vobo Gerencia?" es la que rige el destino final del trámite en esta pantalla. Es de

marcado estricto y obligatorio.


### CA09 - - Cont


idad del Trámite

| check de "Confirmar continuación con excepción de desembolso" sigue siendo la garantía legal y comercial de

que la liberación de recursos sin registro procede, por lo que es ineludible, independientemente de si el caso escala al gerente o va directo al

analista de vivienda


### CA10 - - Mensaje de avance: Al momento de realizar el avance en el sistema muestra la alerta de Modal que indique: “Certifico que el trámite cuenta


col

probación del Área de Riesgos de la Operación en los términos indicados (Nacar) Estudio de títulos sin observaciones Pagaré firmado por el

deudor y seguros Avalúo dictamen favorable sin observaciones Boleta de ingreso a registro con folio previo Formato de autorización de desembolso

con boleta firmado por el cliente. ¿Estás seguro de avanzar?". Muestra dos botones “Cancelar” que al seleccionar cierra la alerta y mantiene la

actividad y si se indica “Aceptar” avanza hacia el enrutamiento necesario.


## Modelo de datos


Tipo de

Editable

Obligatorio

Campo

Reglas de Negocio / Origen

Dato

(Si-No)

(Si-No)

Datos Cliente y Notaría

Varios

No

Si

Heredados de etapas previas. Siempre visibles en solo lectura

Datos Boleta de Ret

tro

Varios

No

Condicionado

Heredados (ej. Fecha, Radicado, Oficina). Visibles en solo

lectura solo si el caso proviene de "Realizar Recepción Boleta".

Valores: "SÍ!

Si

Si

'NO". Únicamente informativo, no altera el flujo

Excepción Autorizada

Lista Despl.

de avance.

Confirmar continuación con

Checkbox

Si

Si

Control obligatorio de gatillo comercial para respaldar la

excepción de desembolso

excepción

¿Requiere Vobo Gerencia?

Lista Despl.

Si

Si

Valores: "SÍ", "NO".

ompuerta de enrutamiento principal.

Sugerido para que el Comercial justifique las razones del visto

Observaciones de Excepción

Texto (Área)

Si

No

bueno excepcional o los motivos del escalamiento a Gerencia


rev:c5fe927afdded 15031546c632fc49 114848e02e5.

https://cibergestion-latam.atlassian. net/sifjira.issueviews:issue-html/BBV-94/BBV-94,htm|
