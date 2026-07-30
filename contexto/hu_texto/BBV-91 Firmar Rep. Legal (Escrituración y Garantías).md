# BBV-91 - HU - Actividad Firmar Rep. Legal (Escrituración y Garantías)

## Información General

| Campo | Valor |
|---|---|
| Épica | Presto Escrituración y Garantías BBVA Legalización |
| Historia | BBV-91 |
| Estado | Tareas por hacer |
| Proyecto | BBVA - Colombia |
| Tipo | Historia |
| Prioridad | Media |
| Informador | Jorge Andres Garzon Paez |
| Persona asignada | Jorge Andres Garzon Paez |
| Resolución | Sin resolver |
| Etiquetas | BBVA_LEGALIZACION |

## Descripción

**Yo como:** Representante Legal.

**Deseo:** Acceder al acordeón **“Firmar Rep. Legal”** para visualizar la información heredada del caso, emitir mi visto bueno confirmando si la Escritura Pública fue firmada o no, y justificar mi decisión en caso de rechazo.

**Para:** Avanzar el flujo hacia la entrega de la escritura firmada, o enrutar adecuadamente las devoluciones hacia la preformalización y Realizar Entrega EP Firmada o la revisión del abogado, según corresponda.

## Alcance

Esta funcionalidad corresponde a la pantalla transaccional de formalización donde el Representante Legal interactúa.

La interfaz consolida la información trabajada por las instancias previas —Abogado, Prorrata y/o Leasing— en modo de **solo lectura**.

Su núcleo funcional es capturar el dictamen de la firma mediante un campo de parametrización específica y aplicar una compuerta lógica de enrutamiento:

- Si se firma, el flujo avanza hacia Vivienda y Preformalización.
- Si no se firma, el sistema retorna al Analista de Vivienda.

## Criterios de Aceptación - Reglas de Negocio

### CA01 - Criterio Global Transversal

1. Mantener la estructura visual heredada de las actividades anteriores.
2. Mostrar la sección **Información General** en modo solo lectura.
3. Incorporar:
   - Expediente Digital.
   - Trazabilidad/Bitácora.
4. Incluir botones de **Guardado** y **Transición / Avanzar**.
5. Al avanzar, registrar en bitácora:
   - Fecha.
   - Actividad `Firmar Rep. Legal`.
   - Usuario ejecutor `Representante Legal`.
   - Concepto de firma.
   - Observaciones.

### CA02 - Acordeón “Firmar Rep. Legal” y Datos Heredados

El sistema debe desplegar el acordeón **“Firmar Rep. Legal”** y mostrar en modo solo lectura:

- Datos del cliente.
- Datos de la notaría.
- VoBo Prorrata.
- Liquidación Leasing.
- Concepto de Revisión EP del Abogado.

### CA03 - Campos Transaccionales de Firma

#### Concepto

Lista desplegable basada en la parametría **L41**:

- `Escritura firmada Conforme`
- `Escritura NO firmada`

#### Despliegue de Novedades

Si el concepto es **Escritura NO firmada**, mostrar obligatoriamente:

- **Tipología** — parametría L42.
- **Casuística** — parametría L43.
- **Observaciones** — texto libre.

### CA04 - Enrutamiento por Escritura NO firmada

Al avanzar con el concepto **Escritura NO firmada**:

- El flujo avanza a **Realizar Devolución EP**.
- La tarea se asigna al **Analista de Vivienda**.

### CA05 - Enrutamiento por Escritura firmada

Al avanzar con el concepto **Escritura firmada Conforme**, el sistema debe disparar en paralelo:

- **Preformalizar**, a cargo del Analista de Desembolso.
- **Realizar Entrega EP Firmada**, a cargo del Analista de Vivienda.

### CA06 - Obligatoriedad de Justificación

No se podrá avanzar con el concepto **Escritura NO firmada** si no se han seleccionado:

- Tipología.
- Casuística.

## Modelado de Datos

| Campo | Tipo de Dato | Editable | Obligatorio | Reglas de Negocio / Origen |
|---|---|---:|---:|---|
| Datos Heredados Consolidados | Varios | No | Sí | Precargados de Abogado, Leasing y Prorrata. Solo lectura. |
| Concepto (Firma) | Lista desplegable | Sí | Sí | Parametría L41. |
| Tipología | Lista desplegable | Sí | Condicionado | Obligatorio si el concepto es `Escritura NO firmada`. Parametría L42. |
| Casuística | Lista desplegable | Sí | Condicionado | Obligatorio si el concepto es `Escritura NO firmada`. Parametría L43. |
| Observaciones | Texto (Área) | Sí | Condicionado | Obligatorio si el concepto es `Escritura NO firmada`. |

## Resumen del Flujo

```text
Firmar Rep. Legal
        |
        v
Seleccionar concepto
      /         \
Conforme       NO firmada
   |               |
   v               v
Preformalizar   Tipología
+               Casuística
Entrega EP      Observaciones
Firmada             |
                    v
          Realizar Devolución EP
```
