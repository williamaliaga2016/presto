# [BBV-93] HU - Actividad Realizar Recepción Boleta (Escrituración y Garantías)

* **Épica:** Presto Escrituración y Garantías BBVA Legalización (-122)
* **Proyecto:** BBVA-Colombia
* **Creada:** 02/jun/26 | **Actualizada:** 19/jun/26
* **Estado:** Tareas por hacer
* **Tipo:** Historia
* **Prioridad:** Media
* **Informador:** Jorge Andres Garzon Paez
* **Persona asignada:** Jorge Andres Garzon Paez
* **Resolución:** Sin resolver
* **Votos:** 0
* **Etiquetas:** `BBVA_LEGALIZACION`
* **Componentes:** Ninguno
* **Versiones afectadas / corregidas:** Ninguno
* **Estimación:** Desconocida

---

## Descripción

**Yo como:** Analista de Vivienda.  
**Deseo:** Acceder al acordeón *"Realizar Recepción Boleta"* para visualizar la información precargada, ejecutar o validar la extracción automática de datos registrales mediante el bot del VUR (Número y Fecha de boleta, Matrícula), confirmar la recepción física/digital del documento y definir si aplica alguna excepción comercial.  
**Para:** Documentar formalmente el ingreso de la escritura en instrumentos públicos apoyándome en herramientas de automatización (RPA), y determinar si el flujo avanza hacia el registro de la EP o se desvía temporalmente al área comercial para gestionar una excepción de desembolso.

---

## Alcance

Esta funcionalidad corresponde a la pantalla transaccional donde se captura la información registral inicial (boleta). Opera dentro del subproceso de **Escrituración y Garantías**. 

Su mayor innovación es la integración de un motor RPA (**Bot VUR**) que intenta precargar los datos registrales ejecutándose hasta 3 veces de forma automática; si falla, habilita la captura manual y un botón de reintento. Finaliza con un nodo de enrutamiento que evalúa la necesidad de una excepción de desembolso.

---

## Criterios de Aceptación

### CA01 — Criterio Global Transversal
1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior (heredando la vista de Escrituración y Garantías).
2. El sistema debe renderizar en la cabecera la *"Información General"* (estrictamente de solo lectura).
3. El sistema debe incorporar el contenedor para *"Funciones Transversales"*, dividido en Expediente Digital y Trazabilidad/Bitácora.
4. **Funcionalidad de Botones de Acción:** Opciones de *"Guardado"* y *"Transición / Avanzar"*.
5. **Trazabilidad:** Al ejecutar *"Avanzar"*, se registrará en la bitácora: Fecha, Actividad (*Realizar Recepción Boleta*), Usuario Ejecutor (*Analista de Vivienda*), Decisión de Enrutamiento y Observaciones.

### CA02 — Acordeón "Realizar Recepción Boleta" y Datos Heredados
El sistema debe desplegar un acordeón central nombrado **"Realizar Recepción Boleta"**. En este bloque se mostrará en estricto modo de solo lectura la información consolidada:
* **Datos Cliente:** Tipo y Nro. Documento, Nombres y Apellidos, Tipo de Crédito.
* **Datos Notaría:** Ciudad Notaría, Nro Notaría y Número de Escritura.

### CA03 — Automatización RPA - Bot VUR y Extracción de Datos
Al ingresar a la pantalla (al día siguiente hábil de avanzar la actividad de *Realizar Entrega EP Firmada*), el sistema (Backend) debe ejecutar automáticamente la consulta al robot del VUR para extraer y autocompletar los siguientes campos transaccionales:
* **Número de la boleta (Turno):** (Radicado Registro).
* **Fecha de la boleta (Fecha Radicación):** (Fecha de ingreso con restricción de fecha futura).
* **Números de matrícula(s):** (Del documento descargado).
* **Regla de reintento:** El motor del VUR debe ejecutarse internamente un mínimo de 3 veces antes de declinar la búsqueda y arrojar un fallo.

### CA04 — Manejo de Errores VUR y Botón de Ejecución Manual
Si tras los 3 intentos el bot no logra extraer la información, el sistema debe:
1. Desplegar una alerta o mensaje estandarizado en pantalla que indique exactamente:  
   > *"El VUR ha fallado por favor revisa los datos necesarios a diligenciar en la página [HU - Ejecución Robot VUR]"*.
2. Dejar los campos de captura vacíos y habilitados para que el Analista de Vivienda los diligencie de forma manual.
3. Habilitar un botón de acción denominado **"Ejecutar VUR"**, el cual permitirá al usuario forzar un nuevo intento de extracción a demanda si considera que la plataforma ya se encuentra estable.

### CA05 — Campos Transaccionales de Recepción Adicionales
Además de los campos extraídos por el VUR, el sistema habilitará obligatoriamente:
* **Tipo de boleta:** Lista desplegable parametrizada (L44).
* **Código zona:** Campo de texto/desplegable.
* **Oficina de registro:** Lista desplegable parametrizada (L45), debe ser diligenciado por el robot cuando se ejecuta la consulta VUR.
* **Boleta Recibida:** Un control tipo Checkbox obligatorio para certificar la tenencia del soporte registral.

### CA06 — Campo de Decisión y Enrutamiento
Inmediatamente después de los campos de captura, el sistema debe habilitar el campo de lectura **"¿Aplica Excepción?"** con las indicaciones de **SI** / **NO**, teniendo en cuenta las siguientes reglas:

* **Si aplica Excepción:**
  * Cuando la *Constructora/Proyecto* y el *Tipo de Desembolso (L5)* es **BOLETA** y aplica para los siguientes tipos de crédito:
    * Leasing Usado
    * Hipotecario Usado
    * Remodelación Para Ampliar / Hipotecar
  * **Y Adicionalmente**, debe cumplir con el documento de *Folio(s) Previo(s)* al momento de realizar avance en la actividad.
  * **Comportamiento:** El sistema habilita y enruta el flujo paralelamente a la actividad *"Realizar Excepción Desembolso"*, asignando la tarea a cargo del rol **Comercial**. Y siempre debe ingresar a la actividad de *"Realizar EP Registradas"* a cargo del **Analista de Vivienda**.

* **Si es "NO":**
  * Cuando en el tipo de crédito sea "NO" o diferente al Tipo de Desembolso "Boleta" y se avanza la actividad.
  * **Comportamiento:** El flujo ingresará siempre por su ruta natural hacia la actividad *"Realizar EP Registradas"*, asignando la tarea a cargo del mismo **Analista de Vivienda**.

### CA07 — Restricción de Avance por Documento
El sistema debe identificar que en la actividad obligatoriamente exista anexado en el expediente digital el documento de **Folio(s) Previo(s)**.

### CA08 — Bloqueo de Avance por Checkbox
El sistema no permitirá accionar el botón **"Avanzar"** si el checkbox *"Boleta Recibida"* no ha sido marcado afirmativamente.

### CA09 — Prioridad de Datos VUR
Si el bot **"Ejecutar VUR"** extrae la información exitosamente, los campos de *Número de Boleta*, *Fecha* y *Matrícula* se autocompletarán. El analista podrá editarlos si detecta inconsistencias contra el soporte físico (OCR vs. Físico).

### CA10 — Dependencia Paramétrica
Los campos *"Tipo de boleta"* (L44) y *"Oficina de registro"* (L45) están restringidos a los valores homologados. *"Oficina de registro"* debe auto-filtrarse dependiendo del *"Código zona"* ingresado.

### CA11 — Dependencia Tipo de Boleta
Si el *Tipo de Boleta* es **Física Original** o **Física Copia**, habilita un nuevo campo nombrado **"Boleta En Poder De:"** vacío para diligenciar la información. Resaltar un Tooltip en el campo que mencione:
> *"Indicar la Oficina BBVA quien tiene el resguardo de la boleta."*

---

## Modelado de Datos

| Campo | Tipo de Dato | Editable | Obligatorio | Reglas de Negocio / Origen |
| :--- | :---: | :---: | :---: | :--- |
| **Datos Cliente** | Varios | NO | SÍ | Precargados de formularios previos (Solo Lectura). |
| **Datos Notaría** | Varios | SÍ | SÍ | Precargados de formularios previos y número de escritura. |
| **Botón "Ejecutar VUR"** | Acción | N/A | N/A | Gatilla el bot RPA manualmente si este falló en la carga inicial. |
| **Número de la boleta (Radicado)** | Alfanumérico | SÍ | SÍ | Extraído por el VUR o diligenciado manualmente si falla. |
| **Fecha de la boleta (Ingreso)** | Fecha | SÍ | SÍ | Extraído por el VUR o diligenciado manualmente si falla. Restricción de fecha futura. |
| **Número de matrícula** | Numérico | SÍ | SÍ | Extraído por el VUR o diligenciado manualmente si falla. |
| **Tipo de boleta** | Lista Despl. | SÍ | SÍ | Valores de la Parametría (L44). |
| **Boleta En Poder De:** | Texto | SÍ | SÍ | Se habilita cuando Tipo de Boleta es *Física Original* o *Física Copia*. |
| **Código zona** | Alfanumérico | SÍ | SÍ | Admite caracteres especiales. Condiciona la oficina. |
| **Oficina de registro** | Lista Despl. | SÍ | SÍ | Valores de la Parametría (L45). |
| **Boleta Recibida** | Checkbox | SÍ | SÍ | Control obligatorio de confirmación física/digital. |
| **¿Aplica Excepción Desembolso?** | Texto | NO | NO | Valores: "SI", "NO". Determina si aplica la excepción del desembolso dependiendo el tipo de crédito. |
| **Observaciones** | Texto (Área) | SÍ | NO | Campo libre para documentar el fallo del VUR u otras notas. |

---
*Generado a partir de la exportación de JIRA BBV-93.*