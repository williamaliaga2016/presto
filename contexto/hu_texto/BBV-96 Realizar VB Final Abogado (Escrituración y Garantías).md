# BBV-96 - HU - Actividad Realizar VB Final Abogado (Escrituración y Garantías)

## Información General

| Campo | Valor |
|---|---|
| Épica | Presto Escrituración y Garantías BBVA Legalización |
| Historia | BBV-96 |
| Estado | Tareas por hacer |
| Proyecto | BBVA - Colombia |
| Tipo | Historia |
| Prioridad | Media |
| Informador | Jorge Andres Garzon Paez |
| Persona asignada | Jorge Andres Garzon Paez |
| Resolución | Sin resolver |
| Etiquetas | BBVA_LEGALIZACION |

## Descripción

**Yo como:** Abogado.

**Deseo:** Acceder al acordeón **“Realizar VB Final Abogado”** para visualizar la información heredada —ya sea del registro de la escritura o del control de garantías—, emitir mi visto bueno indicando si requiere o no devolución, y registrar las causales en caso de rechazo.

**Para:** Certificar legalmente la idoneidad del registro y permitir que el flujo avance de forma inteligente hacia la validación de condiciones de desembolso o control de garantías, o retorne el caso por inconsistencias a la bandeja de devoluciones.

## Alcance

Esta funcionalidad corresponde a la revisión jurídica definitiva operada por el rol **Abogado**.

Se caracteriza por ser una pantalla de origen dual:

- Puede ser invocada desde el flujo estándar, luego de confirmar las EP registradas.
- Puede ser invocada desde un ciclo posterior, por escalamiento desde **Gestionar Control de Garantías**.

Su salida dependerá directamente del campo **¿Requiere Devolución?** y aplicará una lógica de bypass de sistema evaluando:

- Si el crédito tuvo una excepción comercial previa.
- Si el desembolso ya se realizó.
- El origen exacto del trámite.

## Criterios de Aceptación - Reglas de Negocio

### CA01 - Criterio Global Transversal

1. Mantener la estructura visual heredada de la actividad anterior.
2. Mostrar **Información General** en modo solo lectura.
3. Incorporar:
   - Expediente Digital.
   - Trazabilidad/Bitácora.
4. Incluir botones de **Guardado** y **Transición / Avanzar**.
5. Al avanzar, registrar:
   - Fecha.
   - Actividad `Realizar VB Final Abogado`.
   - Usuario ejecutor `Abogado`.
   - Decisión de VoBo.
   - Observaciones.

### CA02 - Acordeón “Realizar VB Final Abogado” y Herencia Dinámica

El sistema debe desplegar el acordeón **Realizar VB Final Abogado** y mostrar la información en modo solo lectura según el origen:

- Si proviene de **Realizar EP Registradas**, mostrar los datos de radicación de la boleta y la confirmación de EP registrada.
- Si proviene de **Gestionar Control de Garantías**, mostrar la información diligenciada por el Analista de Desembolso.

### CA03 - Compuerta de Decisión

Campo obligatorio:

**¿Requiere Devolución?**

Valores:

- Sí
- No

### CA04 - Ruta de Devolución y Novedades

Si se selecciona **Sí**:

- Mostrar obligatoriamente:
  - Tipología.
  - Casuística.
- Si la tipología es **Corrección a Registro**, exigir el documento **Formato de Corrección a Registro**.
- Al avanzar, enrutar a **Realizar Devolución EP**.
- Asignar la tarea al **Analista de Vivienda**.

### CA05 - Ruta de Avance y Bypass Condicionado

Si se selecciona **No**, evaluar las banderas de origen y excepción:

#### Escenario A - Aplica Excepción

Si el crédito tuvo una excepción comercial previa:

- Ir a **Gestionar Control de Garantías**.

#### Escenario B - Sin Excepción y Origen Regular

Si no hubo excepción y el trámite viene de **Realizar EP Registradas**:

- Ir a **Validar Condiciones Desembolso**.

#### Escenario C - Bypass por Escalamiento de Garantías

Si no hubo excepción, pero el trámite viene de **Gestionar Control de Garantías**:

- Deshabilitar la ruta hacia **Validar Condiciones Desembolso**.
- Retornar directamente a **Gestionar Control de Garantías**.

Para avanzar sin devolución, debe adjuntarse obligatoriamente el documento **VoBo Final Abogado** en el Expediente Digital.

### CA06 - Banderas del Sistema

Variables invisibles del workflow:

- `Origen_Tramite`
- `Bandera_Excepcion`

Estas banderas controlan la herencia y el enrutamiento del CA05.

### CA07 - Obligatoriedad de Observaciones

El campo **Observaciones** será obligatorio cuando se requiera devolución.

## Modelado de Datos

| Campo | Tipo de Dato | Editable | Obligatorio | Reglas de Negocio / Origen |
|---|---|---:|---:|---|
| `Bandera: Origen_Tramite` | Booleano/Texto | No | Sí | Oculto. Identifica si viene de EP Registradas o Control Garantías. |
| `Bandera: Excepcion_Desembolso` | Booleano | No | Sí | Oculto. Indica si existe excepción comercial activa. |
| Datos Heredados Dinámicos | Varios | No | Sí | Heredados según el origen. Solo lectura. |
| ¿Requiere Devolución? | Lista Sí/No | Sí | Sí | Compuerta principal de decisión. |
| Tipología | Lista desplegable | Sí | Condicionado | Obligatorio si `¿Requiere Devolución? = Sí`. |
| Casuística | Lista desplegable | Sí | Condicionado | Obligatorio si `¿Requiere Devolución? = Sí`. |
| Observaciones | Texto (Área) | Sí | Condicionado | Obligatorio si `¿Requiere Devolución? = Sí`. |

## Resumen del Flujo

```text
Realizar VB Final Abogado
          |
          v
¿Requiere Devolución?
      /          \
    Sí            No
    |             |
    v             v
Tipología      Evaluar banderas
Casuística     de origen/excepción
Observaciones      |
    |         +----+------------------+
    v         |                       |
Realizar      v                       v
Devolución EP Gestionar Control   Validar Condiciones
              de Garantías        Desembolso
```
