# BBV-101 Registro y Control de Cheques (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

\n\n--- PAGE page-1.png ---\n
6/7/26, 15:26 [#BBV-101] HU - Actividad Registro y Control de Cheques (Escrituración y Garantías)
Epica - Presto Escrituración y Garantías BBVA Legalización (sev-122)
+ [BBV-101] HU - Actividad Registro y Control de Cheques (Escrituración y Garantías) creada: 025jun/26 Actualizada:
23/jun/26
Estado: Tareas por hacer
Proyecto: BBVA - Colombia
Componentes: Ninguno
Versiones afectadas: Ninguno
Versiones corregidas: Ninguno
Principal: Épica - Presto Escrituración y Garantías BBVA Legalización
Tipo: Historia Prioridad: Media
Informador: Jorge Andres Garzon Paez Persona asignada: Jorge Andres Garzon Paez
Resolución: Sin resolver Votos: 0
Etiquetas: BBVA_LEGALIZACION
Trabajo restante estimado: Desconocido
Tiempo Trabajado: Desconocido
Estimación original: Desconocido

## Descripción

**Yo como:** Analista de Desembolso / Comercial (Radicación Manual).
**Deseo:** Acceder a la pantalla "Registro y Control de Cheques" para visualizar la información heredada del cliente y el vendedor, y diligenciar los datos
transaccionales de los cheques emitidos para la liberación de los recursos.
**Para:** Documentar operativamente la emisión de los pagos y generar el token que enrutará el flujo hacia la actividad de "Enviar a Custodio” a cargo del
área de Garantías.

## Alcance

Esta funcionalidad corresponde a una pantalla transaccional resolutiva operada por el Analista de Desembolso, Su núcleo es la captura detallada de
los datos del cheque (COH, beneficiarios, montos y fechas), rellenando el vacío de información operativa previo. A nivel de flujo, esta actividad actúa
como un disparador secuencial: al completarse exitosamente, emite un token de avance que habilita de manera directa la siguiente etapa de custodia
documental, trasladando la responsabilidad de la tarea a un nuevo rol (Analista de Garantías).
. . .z .

## Criterios de aceptación y reglas de negocio

+ CA01 (Criterio Global Transversal):
1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior
(heredando la vista de Escrituración y Garantías).
2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura).
3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en Expediente Digital y Trazabilidad/Bitácora.
4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar".
5. Trazabilidad: Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Registro y Control de Cheques), Usuario Ejecutor
(Analista de Desembolso), Confirmación de gestión y Observaciones. (Nota: Ajustado para mantener coherencia de rol en la bitácora).
+ CA02 (Acordeón Principal y Datos Heredados): El sistema debe desplegar un acordeón central nombrado "Registro y Control de Cheques".
Este bloque debe heredar y mostrar obligatoriamente en modo de estricto solo lectura la siguiente información de contexto para el analista:
o Datos del Crédito y Cliente: Fecha Desembolso, Cédula, Cliente, Scoring, Tipo de Crédito.
o Datos del Vendedor: Teléfonos de Contacto Vendedor y Correo del Vendedor.
+ CA03 (Captura de Datos Operativos del Cheque): El sistema habilitará los siguientes campos transaccionales para que el Analista de
Desembolso los diligencie obligatoriamente para documentar el pago:
\n\n--- PAGE page-2.png ---\n
