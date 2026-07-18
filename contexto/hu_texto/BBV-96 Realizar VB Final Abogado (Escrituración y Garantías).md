# BBV-96 Realizar VB Final Abogado (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


617126, 18:24

[#BBV-96] HU - Actividad Realizar VB Final Abogado (Escrituración y Garantías)

Epica - Presto Escrituración y Garantías BBVA Legalización pov

4 [BBV-96] HU - Actividad Realizar VB Final Abogado (Escrituración y Garantías) creada: 02jun/26 Actualizada: 19/juni26

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


**Yo como:** Abogado.

**Deseo:** Acceder al acordeón "Realizar VB Final Abogado" para visualizar la información heredada (ya sea del registro de la escritura o del control de

garantías), emitir mi visto bueno indicando si requiere o no devolución, y registrar las causales en caso de rechazo.

**Para:** Certificar legalmente la idoneidad del registro y permitir que el flujo avance de forma inteligente hacia la validación de condiciones de

desembolso o control de garantías, o retorne el caso por inconsistencias a la bandeja de devoluciones.


## Alcance


Esta funcionalidad corresponde a la revisión jurídica definitiva operada por el rol Abogado. Se caracteriza por ser una pantalla de origen dual: puede

ser invocada desde el flujo estándar (tras confirmar las EP Registradas) o desde un ciclo posterior (escalamiento desde Gestionar Control de

Garantías). Su salida (enrutamiento) dependerá directamente del campo "¿Requiere Devolución?" y aplicará una lógica de bypass de sistema

evaluando si el crédito tuvo una excepción comercial previa o si el desembolso ya se realizó.


## Criterios de aceptación y reglas de negocio


+ CA01 (Criterio Global Transversal):

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior

(heredando la vista de Escrituración y Garantías),

2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura).

3. El sistema debe incorporar el contenedor para

'unciones Transversales", dividido en Expediente Di

I y Trazal

lad

itácora.

4. Funcion

lad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar".

5, Trazabi

lad: Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Realizar VB Final Abogado), Usuario Ejecutor (Abogado),

Decisión de VoBo y Observaciones.

+ CA02 (Acordeón "Realizar VB Final Abogado" y Herencia Dinámica): El sistema debe desplegar un acordeón central nombrado "Realizar VB

Final Abogado". Este bloque mostrará en estricto modo de solo lectura la información del caso, adaptándose automáticamente según el origen de

la tarea

© Si proviene de

"Realizar EP Registradas": Mostrará los datos de radicación de la boleta y la confirmación de EP registrada.

© Si proviene de

"Gestionar Control de Garantías": Mostrará la información específica diligenciada previamente en esa etapa por el Analista

de Desembolso.

+ CAO3 (Compuerta de Decisión - ¿Requiere Devolución?

Jentro de la sección operativa, el sistema habilitará el campo obligatorio "¿Requiere

Devolución?” con los valores "SÍ" / "NO".

112


<!-- --- PAGE 2 --- -->


617126, 18:24

[#BBV-6] HU - Actividad Realizar VB Final Abogado (Escrituración y Garantías)

+ CA04 (Ruta de Devolución y Novedades): Si en “¿Requiere Devolución?” el abogado selecciona “SI”

+ El sistema desplegará inmediatamente dos campos obligatorios adicionales: Tipología y Casuística (Pendiente de confirmación).

+ Adicionalmente, Si cumple con la Tipología: “Corrección a Registro" debe ser obligatorio realizar el cargue del documento: “Formato de

Corrección a Registr

con el fin de que el Analista de Vivienda pueda revisar el documento en el expediente digital

+ Alaccionar “Avanzar”, el flujo se enrutará obligatoriamente hacia la actividad "Realizar Devolución El

, asignando la tarea al Analista de

Vivienda para que solvente las inconsistencias detectadas.

+ CAOS (Ruta de Avance y Bypass Condicionado por Excepción/Origen): Si en "¿Requiere Devolución?” el abogado selecciona "NO", el

sistema evaluará internamente las banderas de Origen y Excepción para determinar el avance al accionar "Transición / Avanzar".

+ Escenario A (Aplica Excepción): Si el crédito tuvo una excepción comercial previa, el flujo transita hacia la actividad "Gestionar Control de

Garantías"

+ Escenario B (Sin Excepción y Origen Regular): Si NO se realizó excepción y el trámite viene de Realizar EP Registradas, el fujo transita

hacia la actividad "Validar Condiciones Desembolso".

+ Escenario C (Bypass por Escalamiento de Garantías): Si NO hubo excepción, pero el origen del trámite viene escalado desde Gestionar

Control de Garantias (es decir, el desembolso ya se realizó previamente), el sistema deshabilita la ruta a "Validar Condiciones

Desembolso" y retorna directamente a la actividad "Gestionar Control de Garantías”,

+ En cualquiera de los escenarios al cumplir que NO requiere devolución, es decir, es exitosa para avanzar, debe de adjuntarse

obligatoriamente el documento: “Vobo Final Abogado” en el Expediente Digital

+ CA06 - Banderas de Sistema: El motor de workflow debe inyectar dos variables invisibles en el formulario: Origen_Tramite y

Bandera_Excepcion. Estas banderas controlarán la interfaz de herencia y el enrutamiento complejo del CA-05 sin requerir intervención manual del

abogado.

+ CAQ7 - Obligatoriedad de Observaciones: La pantalla debe contar con un campo "Observaciones". Su diligenciamiento será obligatorio si se

requiere devolución, para detallar los motivos legales del rechazo.


## Modelo de datos


Tipo de

Campo

Editable Obligatorio

Reglas de Negoci

J Origen

Dato

Oculto (Backend). Identifica si viene de EP Registradas o Control

Bandera: Origen_Trami

Booleano/Texto

No

si

Garantías.

Bandera:

Booleano

No

sí

Excepcion_Desembolso

Oculto (Backend). Indica si el caso tiene excepción comercial activa

Datos Heredados Dinámicos

Varios

No

Si

Heredados según el origen de la tarea, Visibles en solo lectura.

sí

sí

'Compuerta principal que decide si hay avance o devolución a

¿Requiere Devolución?

Lista (SINO)

Vivienda.

Tipología

Lista Desp.

Si

Condicionado

Obligatorio si ¿Requiero Devolución:

SÍ (Pendiente de

confirmación),

Obligatorio si ¿Requiere Devolución:

SÍ (Pendiente de

Casuistica

Lista Desp

Si

Condicionado

confirmación),

Obligatorio si ¿Requiere Devolución:

= SÍ. Justifica el rechazo

Observaciones

Texto (Area)

si

Condicionado

jurídico.


rev:c5te927afdded 15031546c632fc49 11484800205,

2

https://cibergestion-latam.atlassian,netisijira issueviews:issue-htm/BBV-96/BBV-96 html
