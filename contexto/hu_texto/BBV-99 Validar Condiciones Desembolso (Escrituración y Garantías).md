# BBV-99 Validar Condiciones Desembolso (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


617126, 15:25

[#BBV-99] HU - Actividad Validar Condiciones Desembolso (Escrituración y Garantías)

Epica -Presto Escrituración y Garantías BBVA Legalización (vw

i, [BBV-99] HU - Actividad Validar Condiciones Desembolso (Escrituración y Garantías) cresés o2junzs Actuals

19) n26

Estado:

Tareas por hacer

BBVA- Colombia

Proyecto:

Componente:

Ninguno

Versiones afectadas

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

Desconocido

Tiempo Trabajado:

Estimación original:

Desconocido


## Descripción


**Yo como:** Analista de Vivienda.

**Deseo:** Acceder al acordeón "Validar Condiciones Desembolso" para visualizarla información heredada dinámicamente según su procedencia,

verificar que el plan de pagos sea correcto, suspender la actividad (si existen validaciones paralelas de registro activas) y definir si el trámite requiere

‘escalamionto comercial

**Para:** Garantizar que todas las condiciones financieras y documentales del crédito estén subsanadas antes de la liberación de los recursos, o enrutar

correctamente el caso al área comercial o hacia una validación final de escalamientos (ej. para cobertura MCY).


## Alcance


Esta funcionalidad corresponde a una pantalla transaccional de control estricto operada por el Analista de Vivienda. Su característica principal es la

herencia multivia, ya que concentra los trámites que provienen de excepciones comerciales, preformalizaciones directas, vistos buenos de abogados

retornos de la gestión comercial

En esta etapa se introduce una revisión obligatoria del plan de pagos (y su documento adjunto) junto con una nueva lógica de suspensión temporal

de ANS condicionada por la existencia de actividades registrales en curso. Finalmente, la pantalla incluye una compuerta de decisión transversal que

deriva el flujo hacia el área comercial (si hay excepcionesidesistimientos) o redirige el caso a "Gestionar Escalamientos” para asegurar que áreas

‘como Seguros o Cobertura den un aval final previo al Desembolso.


## Criterios de aceptación y reglas de negocio


+ CAD (Criterio Global Transversal):

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior

(heredando la vista de Escrituración y Garantías).

2. El sistema debe renderizar en la cabecera l

Información General" (estrictamente de solo lectura)

3, El sistema debe incorporar el contenedor para "Funciones Transversales”, dividido en Expediente Digital y Trazablidad/Bitacora,

4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y “Transición / Avanzar”

5. Trazabilidad: Al ejecutar "Avanzar”, se registrará en la bitácora: Fecha, Actividad (Validar Condiciones Desembolso), Usuario Ejecutor

(Analista de

ivienda), Decisión y Observaciones (Nota: Ajustado del texto original para mantener coherencia de rol en esta pantalla).

+ CA02 (Acordeón "Validar Condiciones Desembolso" y Herencia Dinámica Multivia) El sistema debe desplegar un acordeón central nombrado

"Validar Condiciones Desembolso". Este bloque mostrará en estricto modo de solo lectura la información contextual, la cual variará y se

adaptará automáticamente dependiendo de la actividad que originó el pase a esta bandeja:

112

hitps://cibergestion latam atlassian nelisifja issueviews:issue-hlm/BBV-99/BBV-99.htm!


<!-- --- PAGE 2 --- -->


617/26, 15:25

[#BBV-99] HU - Actividad Validar Condiciones Desembolso (Escrituración y Garantías)

+ Desde Realizar Excepción Desembolso: Mostrará el check de confirmación comercial y observaciones de excepción.

> Desde Preformaliz

7: Mostrará la ruta de avance seleccionada por Desembolsos.

> Desde Realizar VB Final Abogado: Mostrará ol concepto jurídico positivo.

+ Desde Realizar Gestión Comercial: Mostrará la respuesta indicando que el cliente NO desiste y desea continuar.

CAO3 (Funcionalidad de Suspensión de Actividad y ANS): El sistema (backend) debe evaluar en tiempo real si el expediente tiene activas de

manera paralela las actividades registrales.

+ Condición de Activación: Si el caso tiene activas simultáneamente las actividades de "Realizar EP Registradas" o "Realizar Recepción

Boleta", el sistema habilitará una opción botón explícito de "Suspender Actividad”.

> Acción de Suspensión: Al confirmar la suspensión, la actividad quedará inhabilitada temporalmente, no correrán los tiempos de ANS

(SLA), pero conservará intactos todos los datos heredados y diligenciados hasta el momento.

+ Condición de Ocultamiento: Si "Validar Condiciones Desembolso” es la única actividad en curso, no se presentará en a interfaz ninguna

opción ni botón de suspender la actividad con confirmación

CADA (Verificación y Documento Obligatorio del Plan de Pagos): El acordeón debe incluir una sección obligatoria para la confirmación de las

condiciones financieras,

+ Se habilitará un control denominado "Confirmar verificación del Plan de Pagos".

+ Para poder accionar el botón de avance hacia la siguiento etapa, el sistema validará que el documento final del "Plan de Pagos” se

encuentre obligatoriamente cargado en el Expediente Digital

CAOS (Compuerta de Escalamie

ito Comercial y Enrutamiento): Dentro de la sección operativa, el sistema habiltara el campo obligatorio

Requiere Escalamiento Comercial?” (Valores: SÍ / NO).

+ Sise selecciona "SÍ" y se acciona “Avanzar El flujo se enrutará a la actividad "Realizar Gestión Comercial”, asignando la tarea al rol

Comercial.

> Sie selecciona "NO" y se acciona "Avanzar": El sistema redirigirá el ujo a la actividad central de "Gestionar Escalamientos”. Esto

¡garantizará que se realice una validación final antes del desembolso y permitirá hacer un nuevo escalamiento en caso de ser necesario (como

validarla marcación de Cobertura si el caso aplica para subsidio MCY).

CAOS - Sistema de Contado de Caídas: El sistema debe de identificar la cantidad de veces que se ha habilitado la actividad de Validar

Condiciones de Desembolso teniendo el registro en la Trazabilidad y uno visual del Front.

CAOT - Obligatoriedad Financiera y Documental: El sistema bloqueará el avance de la actividad hacia "Gestionar Escalamientos” si el control de

"Confirmar verificación del Plan de Pagos" no se encuentra debidamente chequeado como afirmativo y el soporte no consta en el repositorio digital

CAOS - Trazabilidad de Origen (Backend): El motor del workflow debe almacenar la bandera de procedencia (Excepción, Preformalizar, Abogado,

Comercial) no solo para pintar la interfaz, sino para mantenerla historia Impia si el caso requiero devolverse posteriormente.


## Modelo de datos


Campo

Tipo de Dato Editable Obligatorio

Reglas de Negocio / Origen

Bandera de Origen

Texto

No

si

Oculto. Registra si viene de Excepción, Preformalizar, Abogado o

(Backend)

Comercial

Datos Contextualos

Heredados

Varios

No

si

Bloque dinámico en modo solo lectura para contexto del analista,

Suspender Actividad

Botón / Acción

Si

Condicionado

Visible solo si hay actividades registrales ("Recepción Boleta” o "EP

Registrada") corriendo en paralelo. Pausa el ANS.

Confirmar verificación del

‘Checkbox / Lista

si

Control obligatorio. Asegura que las cuotas/tasas fueron revisadas.

Plan de Pagos

Despl

Documento.

Documento Plan de Pagos

si

si

Obligatorio cargarlo en el Expediente Digital para permitir el

(Carga)

enrutamiento a Gestionar Escalamientos.

¿Requiere Escalamiento

Comercial?

Lista Desplegable

si

sí

Valores: "SÍ","NO". Compuerta principal de salida

Observaciones de

‘Campo libre para asentar consideraciones sobre el plan de pagos o

Texto (Área)

si

No

Condiciones

las razones de redirección

¡Generado a las Mon Jul 06 20:25:22 UTC 2026 por Michael Pulido usando JIRA 1001.0.0-SNAPSHOT#100292-

rev:c5te927aldded15031546¢832fe491f484800205,

2

hitps://cibergestion latam atlassian neVsifira.issueviews:issue-htmiVBBV-9Y/BBV-99.htm
