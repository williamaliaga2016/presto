# [BBV-95] HU - Actividad Realizar EP Registradas (Escrituración y Garantías)

* **Épica:** Presto Escrituración y Garantías BBVA Legalización (-122)[cite: 2]
* **Proyecto:** BBVA-Colombia[cite: 2]
* **Creada:** 02/jun/26 | **Actualizada:** 19/jun/26[cite: 2]
* **Estado:** Tareas por hacer[cite: 2]
* **Tipo:** Historia[cite: 2]
* **Prioridad:** Media[cite: 2]
* **Informador:** Jorge Andres Garzon Paez[cite: 2]
* **Persona asignada:** Jorge Andres Garzon Paez[cite: 2]
* **Resolución:** Sin resolver[cite: 2]
* **Votos:** 0[cite: 2]
* **Etiquetas:** `BBVA_LEGALIZACION`[cite: 2]
* **Componentes:** Ninguno[cite: 2]
* **Versiones afectadas / corregidas:** Ninguno[cite: 2]
* **Estimación:** Desconocida[cite: 2]

---

## Descripción

**Yo como:** Analista de Vivienda[cite: 2].  
**Deseo:** Acceder al acordeón *"Realizar EP Registradas"* para visualizar en modo lectura la información de radicación de la boleta, registrar los datos de recepción física de la escritura y confirmar que ha sido registrada exitosamente[cite: 2].  
**Para:** Formalizar el registro ante la oficina de instrumentos públicos y avanzar el flujo obligatoriamente hacia la revisión final por parte del Abogado[cite: 2].

---

## Alcance

Esta funcionalidad corresponde a una pantalla transaccional de confirmación operada por el Analista de Vivienda dentro del subproceso de **Escrituración y Garantías**[cite: 2]. 

Actúa como un punto de control final (*check-point*) que certifica que el trámite registral ha concluido y que el documento físico ha sido recibido[cite: 2]. Consolida la información heredada de la radicación previa (boleta) y utiliza un único control de validación para transitar el caso de manera directa y lineal hacia el Abogado, sin ramificaciones ni compuertas complejas[cite: 2].

---

## Criterios de Aceptación — Reglas de Negocio

### CA01 — Criterio Global Transversal
1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior (heredando la vista de Escrituración y Garantías)[cite: 2].
2. El sistema debe renderizar en la cabecera la *"Información General"* (estrictamente de solo lectura)[cite: 2].
3. El sistema debe incorporar el contenedor para *"Funciones Transversales"*, dividido en Expediente Digital y Trazabilidad/Bitácora[cite: 2].
4. **Funcionalidad de Botones de Acción:** Opciones de *"Guardado"* y *"Transición / Avanzar"*[cite: 2].
5. **Trazabilidad:** Al ejecutar *"Avanzar"*, se registrará en la bitácora: Fecha, Actividad (*Realizar EP Registradas*), Usuario Ejecutor (*Analista de Vivienda*), Confirmación de registro y Observaciones[cite: 2].

### CA02 — Acordeón "Realizar EP Registradas" y Datos Heredados
El sistema debe desplegar un acordeón central nombrado **"Realizar EP Registradas"**[cite: 2]. Este bloque mostrará en estricto modo de solo lectura la información consolidada del trámite para dar contexto a la confirmación:
* **Datos del Cliente y Notaría:** (Pre-cargados desde la radicación)[cite: 2].
* **Datos de Recepción Boleta:** Fecha de ingreso, Radicado Registro, Tipo de boleta, Oficina de registro y Número de matrícula[cite: 2].

### CA03 — Captura de Datos de Recepción Física
Dentro de la sección operativa, el sistema habilitará obligatoriamente los siguientes campos nuevos para documentar la tenencia del soporte (Teniendo en cuenta la funcionalidad del robot que se ejecuta para la extracción final de la información; Ver HU: *[BBV- HU - Ejecución Robot de Extracción Datos VUR]*):
* **Finalización:** Consistencia de Inscripción[cite: 2].
* **Causal:** Resultado del proceso[cite: 2].
* **Fecha Finalización:** Fecha aplicada en el proceso de culminación del registro[cite: 2].

### CA04 — Confirmación de Registro
Junto a los datos de recepción, el sistema debe habilitar obligatoriamente un control tipo Checkbox con el enunciado: **"Confirmación de EP Registrada"**[cite: 2].

### CA05 — Enrutamiento Lineal hacia el Abogado
Una vez marcado el check, diligenciados los campos de recepción y accionado el botón **"Avanzar"**, el flujo transitará de manera incondicional hacia la actividad **"Realizar VB Final Abogado"**, asignando automáticamente la tarea al rol **Abogado**[cite: 2].

### CA06 — Nuevo Campo Tipologías Garantías
Adicionar un nuevo campo donde permite realizar múltiples registros en la información de las tipologías[cite: 2]. Permite adicionar varias opciones y dejar el registro de las seleccionadas (Parametría L53)[cite: 2].

### CA07 — Bloqueo de Avance Sin Confirmación
El sistema impedirá que el Analista de Vivienda transite la actividad si no ha marcado explícitamente el check de **"Confirmación de EP Registrada"** y diligenciado los campos de recepción física[cite: 2]. Esta acción es la garantía de que el proceso registral finalizó correctamente[cite: 2].

### CA08 — Ausencia de Devoluciones o Desvíos
De acuerdo con el flujo de proceso, esta actividad es netamente lineal[cite: 2]. No existirán opciones de *"Concepto Favorable/No Favorable"* ni campos de escalamiento comercial en esta pantalla[cite: 2].

---

## Modelado de Datos

| Campo | Tipo de Dato | Editable (Sí/No) | Obligatorio (Sí/No) | Reglas de Negocio / Origen |
| :--- | :---: | :---: | :---: | :--- |
| **Datos Cliente y Notaria** | Varios | No | Sí | Heredados de etapas previas (Tipo Crédito, CC, Ciudad Notaria, etc.)[cite: 2]. |
| **Datos Recepción Boleta** | Varios | No | Sí | Heredados de la captura previa (Radicado, Fecha ingreso, Matrícula, Oficina). Solo lectura[cite: 2]. |
| **Finalización** | Fecha | Sí | Sí | Captura obligatoria de la fecha de entrega del documento VUR[cite: 2]. |
| **Causal** | Alfanumérico | Sí | Sí | Captura obligatoria del VUR[cite: 2]. |
| **Fecha Finalización** | Fecha | Sí | Sí | Captura obligatoria de la fecha de entrega del documento VUR[cite: 2]. |
| **Tipologías Garantías** | List. Despl. | Sí | Sí | Parametría L53 - Tipologías Garantías[cite: 2]. |
| **Confirmación de EP Registrada** | Checkbox | Sí | Sí | Control obligatorio de validación operado por el Analista de Vivienda. Gatillador de avance[cite: 2]. |
| **Observaciones** | Texto (Área) | Sí | No | Campo libre para que el Analista documente alguna nota relevante del cierre registral[cite: 2]. |

---
*Generado a partir de la exportación de JIRA BBV-95.*[cite: 2]