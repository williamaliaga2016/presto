# BBV-98 - HU - Actividad Preformalizar (Escrituración y Garantías)

## Información General

| Campo | Valor |
|---|---|
| Épica | Presto Escrituración y Garantías BBVA Legalización |
| Historia | BBV-98 |
| Estado | Tareas por hacer |
| Proyecto | BBVA - Colombia |
| Componentes | Ninguno |
| Versiones afectadas | Ninguno |
| Versiones corregidas | Ninguno |
| Principal | Épica - Presto Escrituración y Garantías BBVA Legalización |
| Tipo | Historia |
| Prioridad | Media |
| Informador | Jorge Andres Garzon Paez |
| Persona asignada | Jorge Andres Garzon Paez |
| Resolución | Sin resolver |
| Votos | 0 |
| Etiquetas | BBVA_LEGALIZACION |
| Trabajo restante estimado | Desconocido |
| Tiempo trabajado | Desconocido |
| Estimación original | Desconocido |

---

## Descripción

**Yo como:** Analista de Desembolso / Comercial (Radicación Manual).

**Deseo:** Acceder al acordeón **“Preformalizar”** para visualizar la información heredada de la actividad **“Validar Cumplimiento de Políticas”** y seleccionar la ruta operativa correspondiente para continuar el trámite.

**Para:** Definir y enrutar el flujo hacia **“Validación de Condiciones de Desembolso”** o directamente hacia **“Realizar Desembolso”**, asegurando la continuidad del proceso de liberación de fondos.

---

## Alcance

Esta funcionalidad corresponde a una pantalla transaccional operada por el área de Desembolsos.

Su objetivo principal es heredar el contexto del cumplimiento de políticas y servir como una compuerta de decisión o enrutador.

El analista definirá, mediante un campo de selección, si el crédito requiere una etapa intermedia de validación de condiciones —retornando temporalmente al Analista de Vivienda— o si salta directamente a la ejecución del desembolso en su propia bandeja.

---

## Criterios de Aceptación - Reglas de Negocio

### CA01 - Criterio Global Transversal

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior **“Validar Cumplimiento de Políticas”**.
2. El sistema debe renderizar en la cabecera la **“Información General”**, estrictamente de solo lectura y con los datos más recientes.
3. El sistema debe incorporar el contenedor para **“Funciones Transversales”**, dividido en:
   - Expediente Digital.
   - Trazabilidad/Bitácora.
4. **Funcionalidad de Botones de Acción:** opciones de **“Guardado”** y **“Transición / Avanzar”**.
5. **Trazabilidad:** al ejecutar **“Avanzar”**, se registrará en la bitácora:
   - Fecha.
   - Actividad: `Preformalizar`.
   - Usuario ejecutor: `Analista de Desembolso`.
   - Decisión de enrutamiento.
   - Observaciones.

---

### CA02 - Acordeón “Preformalizar” y Herencia Completa

El sistema debe desplegar un acordeón central denominado **“Preformalizar”**.

Este bloque debe heredar y mostrar, en estricto modo de **solo lectura**, toda la información, variables y checklist previamente gestionados en la actividad **“Validar Cumplimiento de Políticas”**.

Si el caso proviene de la actividad **“Realizar Desembolso”**, posterior a la actividad **“Rep. Legal”**, el sistema debe guardar o mostrar el contexto completo al Analista de Desembolso.

---

### CA03 - Campo de Decisión y Enrutamiento

Dentro del acordeón, el sistema debe habilitar un campo transaccional obligatorio, por ejemplo:

- **¿Siguiente Acción?**
- **Ruta de Avance**

El campo debe contener las siguientes opciones de enrutamiento:

#### Opción A - Validar Condiciones Desembolso

Al seleccionar esta opción y accionar **“Avanzar”**:

- El flujo transita hacia la actividad **“Validar Condiciones Desembolso”**.
- La tarea se asigna y escala al **Analista de Vivienda**.

#### Opción B - Realizar Desembolso

Al seleccionar esta opción y accionar **“Avanzar”**:

- El flujo transita hacia la actividad **“Realizar Desembolso”**.
- La tarea permanece a cargo del **Analista de Desembolso**.

---

### CA04 - Obligatoriedad de Enrutamiento

El sistema no permitirá que el Analista de Desembolso accione el botón **“Avanzar”** sin haber seleccionado explícitamente una de las dos rutas de destino.

---

### CA05 - Transparencia de Herencia

La interfaz debe garantizar que ningún dato proveniente de **Cumplimiento de Políticas** pueda ser alterado en esta pantalla, protegiendo la integridad del dictamen previo.

---

### CA06 - Herencia de Datos de Realizar Desembolso

El sistema debe identificar si existió un reproceso desde la actividad **“Realizar Desembolso”** y heredar los datos que se hayan ingresado en dicha actividad.

---

### CA07 - Asignación de Rol

El sistema debe identificar el origen del caso:

- Si fue una **radicación manual**.
- Si corresponde al rol **Comercial** que venía trabajando el caso.

La asignación debe conservar el rol correspondiente según el origen.

---

## Modelado de Datos

| Campo | Tipo de Dato | Editable (Sí/No) | Obligatorio (Sí/No) | Reglas de Negocio / Origen |
|---|---|---:|---:|---|
| Datos Heredados de Cumplimiento de Políticas | Varios | No | Sí | Incluye checklist de áreas y datos operacionales previos. Estricto modo solo lectura. |
| Siguiente Acción (Enrutamiento) | Lista Desplegable | Sí | Sí | Valores: `Validar Condiciones Desembolso`, `Realizar Desembolso`. Compuerta de decisión obligatoria. |
| Observaciones | Texto (Área) | Sí | No | Campo libre para documentar alguna eventualidad antes de despachar el caso a la siguiente etapa. |

---

## Resumen del Flujo

```text
Validar Cumplimiento de Políticas
              |
              v
         Preformalizar
              |
              v
       Seleccionar ruta
          /          \
         /            \
        v              v
Validar Condiciones   Realizar Desembolso
de Desembolso               |
        |                    |
        v                    v
Analista de Vivienda   Analista de Desembolso
```

---

## Consideraciones para Implementación en Kiro

- Todos los datos heredados deben mostrarse en modo solo lectura.
- La ruta de avance debe ser obligatoria.
- No debe permitirse avanzar sin seleccionar una opción.
- El sistema debe soportar herencia desde:
  - `Validar Cumplimiento de Políticas`.
  - `Realizar Desembolso`, en caso de reproceso.
- La asignación del rol debe respetar el origen del caso, especialmente para radicación manual.
- Toda transición debe quedar registrada en la bitácora.
