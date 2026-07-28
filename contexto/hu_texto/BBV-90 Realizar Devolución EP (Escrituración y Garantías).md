# BBV-90 - HU - Actividad Realizar Devolución EP (Escrituración y Garantías)

## Información General

| Campo | Valor |
|---|---|
| Épica | Presto Escrituración y Garantías BBVA Legalización |
| Historia | BBV-90 |
| Estado | Tareas por hacer |
| Proyecto | BBVA - Colombia |
| Tipo | Historia |
| Prioridad | Media |
| Informador | Jorge Andres Garzon Paez |
| Persona asignada | Jorge Andres Garzon Paez |
| Resolución | Sin resolver |
| Etiquetas | BBVA_LEGALIZACION |

## Descripción

**Yo como:** Analista de Vivienda.

**Deseo:** Acceder al acordeón **“Realizar Devolución EP”** para visualizar el histórico consolidado de rechazos emitidos por áreas previas, definir si la novedad requiere un escalamiento comercial (registrando sus causales) o reanudar el flujo seleccionando a qué etapa exacta debe retornar el trámite.

**Para:** Solventar las inconsistencias documentales o de proceso reportadas y enrutar correctamente el flujo hacia Comercial, o reactivar la operación hacia la firma de escritura, firma del representante legal o revisión final del abogado.

## Alcance

Esta funcionalidad corresponde a la bandeja centralizadora de rechazos del subproceso de **Escrituración y Garantías**. Actúa como el nodo de corrección donde el **Analista de Vivienda** revisa qué área devolvió la Escritura Pública (EP) y por qué.

La pantalla mostrará en modo lectura los dictámenes previos y exigirá una decisión operativa principal mediante el campo **“¿Requiere Escalamiento Comercial?”**. Su principal característica es el enrutamiento dinámico que le otorga al analista el control total para decidir hacia qué punto exacto del ciclo (**Acción a Seguir**) debe regresar el trámite una vez se subsanen los hallazgos.

## Criterios de Aceptación - Reglas de Negocio

### CA01 - Criterio Global Transversal

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior, heredando la vista de Escrituración y Garantías.
2. Debe renderizar en la cabecera la **“Información General”**, estrictamente de solo lectura.
3. Debe incorporar el contenedor de **“Funciones Transversales”**, dividido en:
   - Expediente Digital.
   - Trazabilidad/Bitácora.
   - Carta de Aprobación.
4. Botones de acción: **“Guardado”** y **“Transición / Avanzar”**.
5. Al ejecutar **“Avanzar”**, se debe registrar en bitácora:
   - Fecha.
   - Actividad: `Realizar Devolución EP`.
   - Usuario Ejecutor: `Analista de Vivienda`.
   - Decisión de Enrutamiento.
   - Observaciones.

### CA02 - Acordeón “Realizar Devolución EP” y Aseguramiento de Conceptos

El sistema debe desplegar un acordeón central denominado obligatoriamente **“Realizar Devolución EP”**. Funcionará como un visor de auditoría en modo de solo lectura y consolidará:

- El o los conceptos/dictámenes negativos emitidos en las instancias previas que gatillaron la devolución.
- Las **Tipologías**, **Casuísticas** y **Observaciones** exactas registradas por el área que detectó la novedad.

### CA03 - Compuerta de Escalamiento Comercial y Novedades

Dentro del acordeón transaccional, el sistema habilitará el campo obligatorio:

**¿Requiere escalamiento comercial?**  
Valores: **Sí / No**.

- Si se selecciona **Sí**, el sistema debe desplegar dos listas obligatorias:
  - `Tipologías`
  - `Casuísticas`
- Al accionar **“Avanzar”**, el flujo se dirige a **“Realizar Gestión Comercial”**, asignando la tarea al rol Comercial.

### CA04 - Ruta de Reanudación y Acción a Seguir

Si se selecciona **No**, el sistema debe mostrar un campo obligatorio **“Acción a Seguir”** con tres opciones:

1. **Firmar Escritura** → retorna a **“Firmar Escritura Cliente”**.
2. **Firmar Rep. Legal** → se dirige a **“Firmar Rep. Legal”**.
3. **Realizar EP Registradas** → transita a **“Realizar EP Registradas”**.

### CA05 - Persistencia de Histórico de Devolución

Si el caso proviene de múltiples rechazos paralelos, la pantalla debe mostrar los dictámenes consolidados para que el Analista de Vivienda solucione todas las inconsistencias de una sola vez.

### CA06 - Condicionalidad de Despliegue

El campo **“Acción a Seguir”** y los campos **“Tipologías/Casuísticas”** son mutuamente excluyentes.

- Si `¿Requiere escalamiento comercial? = Sí`:
  - Mostrar `Tipologías`.
  - Mostrar `Casuísticas`.
  - Ocultar `Acción a Seguir`.
- Si `¿Requiere escalamiento comercial? = No`:
  - Mostrar `Acción a Seguir`.
  - Ocultar `Tipologías`.
  - Ocultar `Casuísticas`.

## Modelado de Datos

| Campo | Tipo de Dato | Editable | Obligatorio | Reglas de Negocio / Origen |
|---|---|---:|---:|---|
| Conceptos / Dictámenes Previos | Alfanumérico | No | Sí | Heredado. Muestra el estado o rechazo emitido por áreas paralelas. |
| Tipología y Casuística de Rechazo (Histórico) | Alfanumérico | No | Sí | Heredado. Muestra la razón exacta de la devolución. |
| Observaciones de Rechazo (Histórico) | Texto | No | Sí | Heredado. Detalle dejado por el área revisora. |
| ¿Requiere escalamiento comercial? | Lista desplegable | Sí | Sí | Valores: “Sí”, “No”. Compuerta principal de decisión. |
| Tipologías | Lista desplegable | Sí | Condicionado | Obligatorio si `¿Requiere escalamiento comercial? = Sí`. |
| Casuísticas | Lista desplegable | Sí | Condicionado | Obligatorio si `¿Requiere escalamiento comercial? = Sí`. |
| Acción a Seguir | Lista desplegable | Sí | Condicionado | Obligatorio si `¿Requiere escalamiento comercial? = No`. |
| Observaciones | Texto (Área) | Sí | No | Campo libre para justificar las acciones tomadas para solventar la devolución. |

## Resumen del Flujo

```text
Realizar Devolución EP
        |
        v
¿Requiere escalamiento comercial?
       / \
     Sí   No
     |     |
     v     v
Tipologías Acción a Seguir
Casuísticas   |
     |        +--> Firmar Escritura Cliente
     |        +--> Firmar Rep. Legal
     |        +--> Realizar EP Registradas
     v
Realizar Gestión Comercial
```
