# BBV-103 Enviar a Custodio (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


6/7/26, 15:26 [#BBV-103] HU - Actividad Enviar a Custodio (Escrituración y Garantías)
Epica - Presto Escrituración y Garantías BBVA Legalización (av-122)
+ [BBV-103] HU - Actividad Enviar a Custodio (Escrituración y Garantías) creada: o2jjun/26 Actualizada: 23/Jun/26
Estado: Tareas por hacer
Proyecto: BBVA - Colombia
Componentes: Ninguno
Versiones afectadas: Ninguno
Versiones corregidas: Ninguno
Principal: Épica - Presto Escrituración y Garantías BBVA Legalización
Tipo: Historia Prioridad: Media
Informador: Jorge Andres Garzon Paez Persona asignada: Jorge Andres Garzon Paez
Resolución: Sin resolver Votos: o
Etiquetas: BBVA_LEGALIZACION
Trabajo restante estimado: Desconocido
Tiempo Trabajado: Desconocido
Estimación original: Desconocido

## Descripción

**Yo como:** Analista de Garantías.
**Deseo:** Acceder al acordeón "Enviar a Custodio" para visualizar la información heredada de las actividades de control de garantías y emisión de
cheques, diligenciar los datos operativos del envío del resguardo documental (Fecha, Guía y Sello) y registrar mis observaciones.
**Para:** Completar de manera definitiva la gestión de archivo físico/digital, emitiendo el token de confirmación que permite al sistema cerrar el trámite y
transmitir la información final.

## Alcance

Esta funcionalidad corresponde a la pantalla transaccional de cierre operativo del subproceso, operada exclusivamente por el Analista de Garantías.
Su núcleo es resolver el vacío de captura de datos de envío documental, solicitando específicamente la guía, fecha y sello. A nivel de arquitectura de
flujo, su avance funge como el gatillo final: al completarse, la actividad ingresa como token de finalización ("Enviar a Custodio") confirmando el éxito
del empaquetado y habilitando la sincronización definitiva del Folio Presto.

## Criterios de aceptación y reglas de negocio

+ CA01 (Criterio Global Transversal):

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior
(heredando la vista de Escrituración y Garantías).

2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura).

3. El sistema debe incorporar el contenedor para "Funciones Transversales”, dividido en Expediente Digital y Trazabilidad/Bitácora.

4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar".

5. Trazabilidad: Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Enviar a Custodio), Usuario Ejecutor (Analista de
Garantías), Confirmación de envío y Observaciones (Nota técnica: Ajustado del original para mantener coherencia de rol y actividad en el log
del sistema)

+ CA02 (Acordeón "Enviar a Custodio" y Herencia Multivia): El sistema debe desplegar un acordeón central nombrado “Enviar a Custodio". Este
bloque heredará y mostrará, en estricto modo de solo lectura, toda la información consolidada proveniente de las dos actividades previas:

o Desde Gestionar Control de Garantías: Estado de la garantía, titular y variables operativas del inmueble.

o Desde Registro y Control de Cheques: Confirmación de emisión de cheques y valores.

+ CA03 (Captura de Datos de Custodia): El sistema habilitará los siguientes campos transaccionales para que el Analista de Garantías formalice el
envío documental:

o Fecha Envío: Selector de calendario.


<!-- --- PAGE 2 --- -->


6/7/26, 15:26 [HBBV-103] HU - Actividad Enviar a Custodio (Escrituración y Garantías)
+ Guía: Campo alfanumérico para registrar el número de rastreo de la empresa de mensajería o control interno.
+ Sello: Campo alfanumérico (o lista, según prefiera el negocio) para identificar el precinto o código del paquete
+ CA04 (Generación de Token y Cierre de Flujo): Al diligenciar los datos y accionar el botón "Avanzar", la actividad ingresa como Token para
Enviar a Custodio a cargo del Analista de Garantías. Este token impacta el nodo de sincronización del sistema, informando que la etapa de archivo
físico/digital ha concluido de manera exitosa, lo que permite al motor de procesos (Backend) cerrar el Folio Presto
+ CA05 (Observaciones): Dentro de la sección operativa, el sistema habilitará un campo de área de texto denominado "Observaciones" para que el
analista registre cualquier anotación sobre el paquete, números adicionales, o salvedades de los documentos físicos.
+ CAO6 - Obligatoriedad de Envio: El sistema no permitirá accionar el avance ni generar el token de finalización si los campos de Fecha Envío, Guía
y Sello se encuentran vacíos. Son el respaldo auditable de la entrega de valores
+ CAO7 - Transparencia de Edición: El Analista de Garantías no podrá modificar en esta pantalla ningún dato heredado de los cheques ni de la
validación de control de garantías previo
+ CAO8 - Funcionalidades de Gestión de Bandejas:

o Gestión y Asignación desde Bandeja General: El sistema consolida todos los casos nuevos en una bandeja global denominada "General",
accesible para todos los analistas de garantías. Al utilizar un botón de asignación sobre un folio seleccionado, el sistema retira
automáticamente el registro de la bandeja "General" y lo traslada de forma exclusiva a la bandeja "Asignadas" del usuario logueado.

+ Guardado Parcial de Trámites: El sistema proporciona un botón "Guardar Parcialmente" dentro de la vista de gestión de la bandeja
"Asignadas". Al accionarlo, el sistema almacena todos los avances registrados por el usuario sin dar por finalizado el trámite y mueve
automáticamente el folio a una bandeja global denominada "Parcial".

+ Consulta y Reasignación de Avances: El sistema expone la bandeja "Parcial" a todos los analistas de garantías para que puedan consultar
los casos que ya cuentan con avance, Mediante un botón de asignación, el sistema permite que cualquier analista seleccione un folio de esta
bandeja y lo transfiera a su propia bandeja "Asignadas" para continuar la gestión.

o Visibilidad Estructural de Seguimiento: El sistema mantiene habilitadas y diferenciadas tres vistas de trabajo para organizar el flujo:
"General" (acceso global para iniciar casos), "Parcial" (acceso global para retomar casos en pausa) y "Asignadas" (acceso personal para la
gestión activa del usuario).

o Restricción de uso compartido asignado: El sistema debe de identificar cuando un Folio fue asignado por la bandeja de General o Parcial y
no debe de permitir que algún usuario pueda sobre escribir o tomar ese caso a menos de que exista una reasignación por el administrador.

. Editable (Sé los or
Campo Tipo de Dato No) (SÉ ontigatorio Reglas de Negocio / Origen
Datos Hi i
atos Heredados (Garantías yi. No Si Información cruzada de validación y pagos. Modo solo lectura.
Cheques)
Fecha Envio Fecha si sí Captura operativa. Cuándo se despacha el paquete.
Guía Alfanumérico Si si Captura operativa. Número de rastreo de la valija/documentos.
Sello Alfanumérico SÍ Sí Captura operativa. Código de seguridad o precinto.
Acción Gatillador de avance que certifica el final del empaquetado a
Token: Enviar a Custodio NO sí me er poa
(Backend) cargo del Analista de Garantías.
. , : Campo libre para asentar anotaciones operativas del envío
Observaciones Texto (Área) Si No
documental.
rev:c5fe927afdde415031546c632fc49114848e0285.
