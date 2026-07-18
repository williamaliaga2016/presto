# BBV-130 HU - Actividad Revisar EP Abogado (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

Épica - Presto Escrituración y Garantías BBVA Legalización (BBV-122)

[BBV-130] HU - Actividad Revisar EP Abogado (Escrituración y Garantías) Creada:
16/jun/26 Actualizada: 22/jun/26

Estado: Tareas por hacer
Proyecto: BBVA - Colombia
Componentes: Ninguno
Versiones Ninguno
afectadas:
Versiones Ninguno
corregidas:
Principal: Épica - Presto Escrituración y Garantías BBVA Legalización

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

**Yo como:** Abogado.

**Deseo:** Acceder a la pantalla "Revisar EP Abogado" para visualizar la información heredada de la firma de
escritura, validar o editar el Representante Legal sugerido, y emitir mi dictamen indicando si la Escritura
Pública está conforme.

**Para:** Avanzar el folio según corresponda al analista de vivienda o hacia el Representante legal dependiendo
directamente del dictamen si fue Favorable o No.


## Alcance

Esta funcionalidad corresponde a la revisión jurídica transaccional de la Escritura Pública dentro del
subproceso de Escrituración y Garantías. Su núcleo radica en capturar el concepto de legalidad de la EP a
través de una compuerta de conformidad, la cual genera un Token de enrutamiento (Avance o Devolución)
hacía el Representante Legal o Analista de Vivienda.


## Criterios de aceptación y reglas de negocio


### CA01 - Criterio Global Transversal

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente
igual a como venían de la actividad anterior (heredando la vista de Escrituración y Garantías).
2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo
lectura).
3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en
Expediente Digital y Trazabilidad/Bitácora.
4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar".
5. Trazabilidad: Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Realizar
VB Prorrata), Usuario Ejecutor (Gestor Constructor), Decisión de Vobo y Observaciones.


### CA02 - Herencia Base - Solo Lectura El sistema debe desplegar un acordeón específico denominado

"Revisar EP Abogado". Este bloque debe mostrar en modo estricto de solo lectura toda la
información precargada y los campos capturados previamente por el Analista de Vivienda/Notaría en la
etapa "Firmar Escritura Cliente".

### CA03 - Campo de Representante Legal - Herencia Editable El sistema debe mostrar el campo

"Representante Legal". Este campo vendrá precargado/heredado con el valor ingresado
previamente en la actividad de "Firmar Escritura", pero habilitará su edición para que el Abogado
pueda modificarlo si jurídicamente se requiere otra designación (consumiendo la Parametría L38).

### CA04 - Compuerta de Conformidad y Asignación de Token El sistema habilitará un campo de

control de flujo obligatorio denominado "¿Escritura Pública Conforme?" (Valores: SÍ / NO). Al
accionar el botón "Avanzar", el sistema evaluará la respuesta:
o Si se selecciona "SÍ": El sistema asume éxito y genera internamente el Token de Rep. Legal,
ingresa a la actividad de "Firmar Rep. Legal" a cargo del Rep. Legal.
o Si se selecciona "NO": El sistema asume un rechazo y genera internamente el Token de
Realizar Devolución EP, e ingresa a la actividad de Realizar Devolución EP a cargo del
analista de vivienda.

### CA05 - Despliegue de Novedades - Tipologías y Casuísticas Si en el campo "¿Escritura Pública

Conforme?" el Abogado selecciona "NO", el sistema debe renderizar inmediatamente dos campos
condicionales y obligatorios para justificar el hallazgo legal:
o Tipología: Lista desplegable parametrizada (L39).
o Casuística: Lista desplegable parametrizada (L40).

### CA07 - - Trazabilidad en Bitácora: Al completar la actividad se guardará el registro en la bitácora:

Fecha, Actividad, Usuario (Abogado), Conformidad de la EP y Observaciones.

### CA08 - - Obligatoriedad de Observaciones: Si la Escritura NO es conforme, es estrictamente

obligatorio diligenciar el campo libre de observaciones para explicarle al Analista de Vivienda qué
cláusula o dato de la minuta notarial requiere ser ajustado.

### CA09 - - Funcionalidad carta de aprobación: Si el Abogado indica que SI es Conforme la EP pero la

carta de aprobación se encuentra en estado Por Vencer o Vencido. El Dictamen se mantiene como
favorable pero deberá de ir directamente a la actividad de “Realizar Devolución EP” asignado al
analista de vivienda para que solvente este error de documento.


## Modelo de datos

Tipo de Reglas de Negocio /
Bloque / Campo Editable Obligatorio
Dato Origen
- 
▪
▪

▪ DATOS
HEREDADOS ---
*

Heredados de "Firmar
Datos de Escritura y Cliente Varios NO Sí Escritura Cliente".
Solo lectura.
- 
▪
▪
▪ CAPTURA DE
REVISIÓN
LEGAL ---*

Precargado de
"Firmar Escritura",
Lista
Representante Legal SÍ Sí pero Editable por el
Desplegable
Abogado (Parametría
L38).
Compuerta.
Determina la
Lista
¿Escritura Pública Conforme? SÍ Sí generación del Token
(SÍ/NO)
(Rep Legal o
Devolución).
Obligatorio si ¿EP
Lista
Tipología (Corrección EP) SÍ Condicionado Conforme? = NO.
Desplegable
(Parametría L39).
Obligatorio si ¿EP
Lista
Casuística (Corrección EP) SÍ Condicionado Conforme? = NO.
Desplegable
(Parametría L40).
Obligatorio si ¿EP
Conforme? = NO.
Observaciones Legales Texto (Área) SÍ Condicionado
Detalla el error en la
minuta.
SNAPSHOT#100292-rev:59691b10846014e3077a45b36b7ad260711deea3.
