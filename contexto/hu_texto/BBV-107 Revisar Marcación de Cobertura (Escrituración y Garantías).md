# Descripción

**Yo como:** Analista de Cobertura.

**Deseo:** Acceder a la pantalla transaccional "Revisar Marcación de Cobertura" para visualizar la información heredada, registrar los datos financieros y de resolución asociadas al subsidio "Mi Techo Propio", y especificar un correo para notificar al área de colocaciones.

**Para:** Garantizar la correcta aplicación de la cobertura en créditos de la ciudad de Barranquilla, enviar la notificación correspondiente y avanzar el flujo operativo hacia la validación de condiciones de desembolso.

**Alcance:** Esta funcionalidad corresponde a una pantalla especializada operada por el Analista de Cobertura. Actúa como un subproceso condicionado de entrada estricta: el motor del flujo solo habilitará esta tarea si el sistema detecta que el inmueble pertenece a Barranquilla y tiene la marca del subsidio "Mi Techo Propio". Su núcleo es la captura de datos operativos (SITCAR) para la liquidación del subsidio y funciona como gatillador para notificar al área de colocaciones antes de devolver el control al Analista de Vivienda.

# Criterios de Aceptación - Reglas de Negocio

- **CA01 (Criterio Global Transversal):**
  1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos.
  2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura con los datos más recientes actualizados).
  3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en **Expediente Digital** (para adjuntar la resolución del subsidio, si aplica) y **Trazabilidad/Bitácora**.
  4. **Funcionalidad de Botones de Acción:** Opciones de "Guardado" y "Transición / Avanzar".
  5. **Trazabilidad:** Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Revisar Marcación de Cobertura), Usuario Ejecutor y Observaciones.

- **CA02 (Acordeón "Revisar Marcación de Cobertura" y Herencia):** El sistema debe desplegar un acordeón central nombrado **"Revisar Marcación de Cobertura"**. Este bloque debe heredar el contexto de la actividad previa de *"Validar Cumplimiento de Políticas"*, mostrando los datos generales de la solicitud para que el Analista de Cobertura tenga la visibilidad completa del caso antes de proceder con el diligenciamiento.

- **CA03 (Lógica de Acceso y Condicionamiento):** La arquitectura del sistema (Backend) debe contar con una compuerta de validación previa al inicio de esta actividad. La pantalla y asignación al **Analista de Cobertura** se habilitará **exclusivamente** si las variables de radicación cumplen simultáneamente dos condiciones:
  1. Ciudad del Inmueble = *Barranquilla*.
  2. Aplica Subsidio = *Mi Techo Propio*.

- **CA04 (Captura de Datos de Cobertura Barranquilla):** Dentro del acordeón, el sistema debe desplegar los campos detallados en el diccionario de datos (ver sección de *Modelado de Datos*), respetando cuáles son "Precargados Editables" y cuáles deben ser "Diligenciados" (SITCAR) por el analista de cobertura.

- **CA05 (Notificación al Área de Colocaciones):** Se debe disponer de una sección independiente denominada "Emisión de Notificación a Colocaciones".
  - Esta sección habilitará un campo de texto con validación de formato Email (Ej. "Correo Área de Colocaciones").
  - **Gatillador (Trigger):** Al momento de accionar el botón "Transición / Avanzar", el sistema tomará obligatoriamente la dirección ingresada en este campo y ejecutará el envío automático de un correo electrónico notificando la aplicación de la cobertura.

- **CA06 (Enrutamiento Directo):** Una vez procesado el envío de la notificación y accionado el avance de la tarea, el flujo transitará de manera directa y lineal hacia la actividad "Validar Condiciones Desembolso", asignando la tarea en la bandeja del rol **Analista de Vivienda** para continuar con el proceso general de escrituración.

- **CA07 (Observaciones):** Se debe incluir un campo de área de texto explícito denominado **"Observaciones"**, para documentar cualquier novedad sobre la resolución o las fechas de marcación.

- **CA08 - Bloqueo por Notificación:** El sistema impedirá que el Analista de Cobertura avance la etapa si el campo "Correo Área de Colocaciones" está vacío o no cumple con el formato válido, ya que el correo es esencial para la conciliación de la cartera.

- **CA09 - Precargados Editables:** Los campos marcados como "Precargados Editables" (ej. N° Obligación, C.C, Valor Desembolso) heredarán la información de las bases del banco, pero permitirán su modificación manual por parte del analista en caso de presentarse un desfase operativo de última hora.

# Modelado de Datos

*De acuerdo con el mapeo del anexo "Cobertura Barranquilla", se estructuran los siguientes campos:*

| Campo | Tipo de Dato / Interfaz | Editable | Obligatorio | Reglas de Negocio / Origen |
|---|---|---|---|---|
| Email Área de Colocaciones | Texto (Email) | Sí | Sí | Campo independiente para la notificación. Gatillador al avanzar. |
| 1. Consecutivo* | Alfanumérico | No | Sí | Asociado a la notificación o llegada del correo inicial. |
| Tipo de Doc. | Lista Desplegable | Sí | Sí | Precargado Editable. (Ej. CC, CE, NIT). |
| C.C (Número) | Alfanumérico | Sí | Sí | Precargado Editable. Documento del cliente. |
| TT (Tipo Trámite) | Alfanumérico | Sí | Sí | Precargado Editable. |
| Nombre | Texto | Sí | Sí | Precargado Editable. Nombre completo del titular. |
| Constructora | Texto/Desplegable | Sí | Sí | Diligencia SITCAR. |
| Proyecto | Texto/Desplegable | Sí | Sí | Diligencia SITCAR. |
| Fecha de Aceptación Plataforma | Fecha | Sí | Sí | Diligencia SITCAR. Formato DD/MM/AAAA. |
| Tipo de Vivienda | Lista Desplegable | Sí | Sí | Precargado Editable. |
| Valor Subsidio | Moneda | Sí | Sí | Diligencia SITCAR. |
| N° Obligación | Alfanumérico | Sí | Sí | Precargado Editable. |
| Fecha de Desembolso | Fecha | Sí | Sí | Precargado Editable. |
| Fecha Próximo Canon | Fecha | Sí | Sí | Diligencia SITCAR. |
| Valor Desembolso | Moneda | Sí | Sí | Precargado Editable. |
| Intereses Corrientes | Moneda | Sí | No | Diligencia SITCAR. |
| Capital | Moneda | Sí | No | Diligencia SITCAR. |
| Seguros | Moneda | Sí | No | Diligencia SITCAR. |
| Cuota Mensual | Moneda | Sí | No | Diligencia SITCAR. |
| Plazo | Numérico | Sí | No | Diligencia SITCAR. |
| Observación | Texto (Área) | Sí | No | Diligencia SITCAR / Criterio 5. |
| Fecha Solicitud Marcación | Fecha | Sí | Sí | Diligencia SITCAR. |
| Hora Solicitud Marcación | Hora (HH:MM) | Sí | Sí | Diligencia SITCAR. |
| Fecha Respuesta Marcación | Fecha | Sí | Condicionado | Diligencia SITCAR. |
| Hora Respuesta Marcación | Hora (HH:MM) | Sí | Condicionado | Diligencia SITCAR. |
| Responsable M5 | Texto | Sí | Sí | Diligencia SITCAR. |
| No Resolución | Alfanumérico | Sí | Sí | Diligencia SITCAR. |
| Fecha de la Resolución | Fecha | Sí | Sí | Diligencia SITCAR. |
| Fecha Envío Resolución | Fecha | Sí | Sí | Diligencia SITCAR. |
| Estado Proceso | Lista Desplegable | Sí | Sí | Diligencia SITCAR. |
| Observaciones | Texto | Sí | Sí | |