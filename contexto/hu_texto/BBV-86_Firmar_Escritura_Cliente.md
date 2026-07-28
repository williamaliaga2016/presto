# BBV-86 - HU - Actividad Firmar Escritura Cliente (Escrituración y Garantías)

## Información General

| Campo | Valor |
|---|---|
| Épica | Presto Escrituración y Garantías BBVA Legalización |
| Historia | BBV-86 |
| Estado | Tareas por hacer |
| Proyecto | BBVA - Colombia |
| Tipo | Historia |
| Prioridad | Media |
| Informador | Jorge Andres Garzon Paez |
| Persona asignada | Jorge Andres Garzon Paez |
| Resolución | Sin resolver |
| Etiquetas | BBVA_LEGALIZACION |

## Descripción

**Yo como:** Analista de Vivienda / Gestor Notaría.

**Deseo:** Acceder a la pantalla en el acordeón **“Escrituración y Garantías”** para los créditos que requieren este trámite, registrar la confirmación de la firma de la escritura y definir el enrutamiento del trámite mediante los validadores de escalamiento comercial y condiciones del producto (CXI).

**Para:** Continuar el flujo operativo hacia la revisión legal del abogado, escalar comercialmente si hay retenciones o habilitar los flujos paralelos de revisión para proyectos constructores (Prorrata) y Leasing (Causación).

## Alcance

La funcionalidad corresponde a la actividad transaccional inicial del subproceso de Escrituración, aplicable única y exclusivamente a los productos de crédito que sí exigen escrituración. Actúa como una compuerta de enrutamiento múltiple que, basándose en campos de decisión manual y variables heredadas del producto, dispara el flujo hacia el Abogado, el Comercial, el Gestor Constructor o el Analista de Leasing.

## Criterios de Aceptación - Reglas de Negocio

### CA01 - Criterio Global Transversal

1. Mantener la estructura visual de los grupos de datos igual a la actividad anterior.
2. Mostrar una sección fija de **Información General**, estrictamente de solo lectura y con datos actualizados.
3. Incorporar **Funciones Transversales**:
   - Expediente Digital.
   - Trazabilidad/Bitácora.
   - Carta de aprobación automática.
4. Botones de acción: **Guardado** y **Transición / Avanzar**.
5. Al avanzar, registrar en bitácora fecha, actividad `Firmar Escritura Cliente`, usuario ejecutor, decisiones de enrutamiento y observaciones.

### CA02 - Filtro por Tipo de Crédito y Acordeón

El sistema debe desplegar el grupo de datos **“Firmar Escrituración Cliente”** únicamente para solicitudes que previamente determinaron que sí requieren escrituración.

Tipos de crédito que sí requieren escrituración:

- Constructor individual.
- Hipotecario Nuevo.
- Hipotecario Usado.
- Hipotecario CXI.
- Leaseback Habitacional.
- Leasing Nuevo.
- Leasing Usado.
- Leasing CXI.
- Remodelación Para Ampliar / Hipotecar.

### CA03 - Campo de Decisión y Enrutamiento Principal

Debe existir el campo **¿Requiere Escalamiento Comercial?**

- **Sí** → al avanzar, dirigir a **Realizar Gestión Comercial**, asignando la tarea al rol Comercial.
- **No** → exigir los demás campos operativos obligatorios y, al avanzar, dirigir a **Revisar EP Abogado**, asignando la tarea al rol Abogado.

### CA04 - Regla de Negocio para Productos CXI y Leasing

Al avanzar, el sistema evalúa variables heredadas del tipo de crédito.

#### Si el crédito es CXI

El sistema habilita y dispara en paralelo **Realizar VB Prorrata** a cargo del Gestor Constructor en el subproceso de Gestión CXI - Causar.

Aplica para:

- Hipotecario CXI.
- Leasing CXI.

#### Si el crédito es Leasing

Mostrar el campo **¿Requiere Causar?**

Si se selecciona Sí:

- Habilitar y disparar el subproceso **Gestión CXI - Causar** dentro de **Gestión Leasing**.
- Ingresar a **Realizar Causación** a cargo del Analista de Leasing.
- Mostrar el modal: **“¿Estás seguro que requieres realizar causación?”**
- Si confirma Sí, avanza a Gestión Leasing.
- Si responde No, cerrar el modal.

Aplica para Leasing Nuevo, Leasing Usado y Leasing CXI.

Adicionalmente, siempre se ejecuta **Revisar EP Abogado** asignada al rol Abogado.

### CA05 - Exclusión de Flujo

Todo trámite que no requiera escrituración omitirá automáticamente esta pantalla y su subproceso.

### CA06 - Obligatoriedad Condicionada

Los campos técnicos requeridos para que el Abogado revise la Escritura Pública solo serán obligatorios si el Analista de Vivienda/Notaría determina que **NO** hay escalamiento comercial.

### CA07 - Bloque “Información de Notaría” - Herencia Editable

El sistema debe heredar y precargar la información de la actividad **Validar Cumplimiento de Políticas**. Los campos editables son:

- Notaría.
- Fecha Notaría.
- Número Notaría.

### CA08 - Bloque “Formalización de Escritura” - Captura

El sistema debe desplegar campos obligatorios en blanco para diligenciar los datos del documento firmado:

- Número de la escritura.
- Fecha de la escritura.

Estos datos se utilizarán posteriormente en etapas como VoBo Prorratas y Recepción Boleta.

### CA09 - Multi Enrutamiento Transparente

La habilitación de **Realizar VB Prorrata** y **Realizar Causación** es una acción de backend que valida las condiciones de CXI del CA04. Los flujos se disparan automáticamente al avanzar.

### CA10 - Condicionalidad de Captura

Si el analista marca **Sí** en `¿Requiere Escalamiento Comercial?`, el sistema exime la obligatoriedad de:

- Número firma de la escritura.
- Fecha de la escritura.

Si marca **No**, estos campos son obligatorios.

### CA11 - Restricción de Escalamiento Concepto

El sistema debe identificar si existió un retorno desde alguna parte del flujo y ya había avanzado buscando el resultado de concepto, para no volver a escalar a:

- Leasing Causar.
- Revisar EP Abogado.
- VoBo Prorratas.

### CA12 - Funcionalidad Registro Contacto

Habilitar un botón **Agregar** que abra un modal con:

- Nro. Contacto (autogenerado secuencial).
- Fecha Contacto (por defecto actual, DD/MM/AAAA).
- Resultado de Contacto (lista L8).
- Detalle (lista dependiente L9).
- ¿Inmueble Definido? (Sí/No).
- Área Contactada: Notaría, Constructora o Cliente.
- Observaciones.

Al guardar, registrar la información en una tabla histórica visible en la misma sección, ordenada cronológicamente de forma descendente.

## Modelado de Datos

| Campo | Tipo de Dato | Editable | Obligatorio | Reglas de Negocio / Origen |
|---|---|---:|---:|---|
| Notaría | Alfanumérico | Sí | Sí | Heredado de `Validar Cumplimiento de Políticas`. |
| Número Notaría | Numérico | Sí | Sí | Heredado de `Validar Cumplimiento de Políticas`. |
| Ciudad Notaría | Alfanumérico | Sí | Sí | Heredado de `Validar Cumplimiento de Políticas`. |
| Número de la escritura | Alfanumérico | Sí | Condicionado | En blanco. Obligatorio según escalamiento. |
| Fecha de la escritura | Fecha | Sí | Condicionado | En blanco. Obligatorio según escalamiento. |
| Representante Legal | Alfanumérico | Sí | No | En blanco. Parametría L38. |
| ¿Requiere Escalamiento Comercial? | Lista (Sí/No) | Sí | Sí | Si es Sí, avanza a `Realizar Gestión Comercial`. |
| Tipologías | Lista | Sí | Sí | Dependiente si `¿Requiere Escalamiento Comercial? = Sí`. |
| ¿Requiere Causar? | Booleano | Sí | Sí | Sí habilita Gestión Leasing / `Realizar Causación`. |
| Observaciones | Texto (Área) | Sí | No | Campo libre para notas de cita o firma. |

## Resumen del Flujo

```text
Firmar Escritura Cliente
        |
        v
¿Requiere Escalamiento Comercial?
      /                     \
    Sí                       No
    |                        |
    v                        v
Realizar Gestión       Completar datos de
   Comercial           firma / escritura
                             |
                             v
                     Revisar EP Abogado
                             |
           +-----------------+------------------+
           |                                    |
      ¿Producto CXI?                       ¿Es Leasing?
           |                                    |
          Sí                                   Sí
           |                                    |
           v                                    v
 Realizar VB Prorrata                  ¿Requiere Causar?
                                               |
                                              Sí
                                               |
                                               v
                                      Realizar Causación
```
