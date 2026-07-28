# BBV-97 - HU - Actividad Realizar Gestión Comercial (Escrituración y Garantías)

## Información General

| Campo | Valor |
|---|---|
| Épica | Presto Escrituración y Garantías BBVA Legalización |
| Historia | BBV-97 |
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

**Yo como:** Comercial.

**Deseo:** Acceder al acordeón **“Realizar Gestión Comercial”** para visualizar en modo lectura la información del caso escalado (desde Firmar Escritura, Devolución EP o Validar Condiciones de Desembolso) y registrar si el cliente desiste o no de la operación.

**Para:** Dar por terminado el trámite (**Fin Terminal**) si el cliente ya no desea continuar, o retornar el caso a la actividad de origen exacta para que retome su curso operativo.

---

## Alcance

Esta funcionalidad corresponde a una pantalla transaccional resolutiva a cargo del rol **Comercial**. Funciona como un punto de salvamento o cierre del crédito.

La interfaz es minimalista: hereda dinámicamente todo el contexto de la actividad que originó el escalamiento (para que el comercial entienda el problema) y habilita un único campo de decisión transversal.

Dependiendo de la respuesta, el sistema cancela la operación o la devuelve a la bandeja del usuario que solicitó el apoyo comercial.

---

## Criterios de Aceptación - Reglas de Negocio

### CA01 - Criterio Global Transversal

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior.
2. El sistema debe renderizar en la cabecera la **“Información General”**, estrictamente de solo lectura con los datos más recientes.
3. El sistema debe incorporar el contenedor para **“Funciones Transversales”**, dividido en:
   - Expediente Digital.
   - Trazabilidad / Bitácora.
4. **Funcionalidad de Botones de Acción:** opciones de **“Guardado”** y **“Transición / Avanzar”**.
5. **Trazabilidad:** al ejecutar **“Avanzar”**, se registrará en la bitácora:
   - Fecha.
   - Actividad: `Realizar Gestión Comercial`.
   - Usuario ejecutor: `Comercial`.
   - Decisión de desistimiento.
   - Observaciones.

---

### CA02 - Acordeón “Realizar Gestión Comercial” y Herencia Dinámica

El sistema debe desplegar un acordeón central nombrado **“Realizar Gestión Comercial”**.

Este bloque mostrará en estricto modo **solo lectura** toda la información del caso. Su contenido se adaptará dependiendo del momento en el que fue escalado:

- **Si proviene de “Firmar Escritura Cliente”**: heredará los datos de notaría y cliente.
- **Si proviene de “Realizar Devolución EP”**: mostrará adicionalmente los conceptos de rechazo de las áreas:
  - Abogado.
  - Prorrata.
  - Leasing.
- **Si proviene de “Validar Condiciones Desembolso”**: mostrará el contexto financiero o de liquidación de la operación.

---

### CA03 - Compuerta de Desistimiento y Cierre

El acordeón habilitará obligatoriamente el campo de selección:

**¿Cliente Desiste del Caso?**

Valores permitidos:

- **Sí**
- **No**

---

### CA04 - Enrutamiento Multivía Condicionado

Al accionar el botón **“Avanzar”**, el sistema evaluará la respuesta y el origen del escalamiento para enrutar la tarea.

#### Si se selecciona “Sí” - Desiste

El flujo avanza automáticamente hacia el evento de **“Fin Terminal”**, cerrando el caso de manera definitiva.

#### Si se selecciona “No” - No desiste, el caso continúa

El sistema leerá la bandera de origen y devolverá el trámite a la actividad exacta que lo escaló:

- Retorna a **“Firmar Escritura Cliente”** - Notaría / Analista de Vivienda.
- Retorna a **“Realizar Devolución EP”** - Analista de Vivienda.
- Retorna a **“Validar Condiciones Desembolso”** - Analista de Vivienda.

---

### CA05 - Bandera de Origen del Escalamiento

El motor de workflow debe inyectar una variable invisible **Bandera/Flag** en el formulario que guarde el ID o nombre de la etapa que gatilló a **“Realizar Gestión Comercial”**.

Esta bandera es obligatoria para garantizar que, si el cliente no desiste, el sistema sepa exactamente a qué bandeja regresar la tarea.

---

### CA06 - Confirmación de Cierre y Cancelación de SLAs

Estando en el escenario de **Desistimiento = Sí**, al accionar el botón **“Avanzar”**, el sistema debe bloquear el avance inmediato y mostrar un modal de alerta o advertencia con el siguiente mensaje:

> **¿Estás seguro de dar cierre al Folio [Número de Folio]?. Ten en cuenta que el avance del Folio no podrá ser recuperado.**

Comportamiento del modal:

- **Si se selecciona NO:** el modal se cierra y la pantalla permanece en su estado actual conservando todos los datos diligenciados.
- **Si se selecciona SÍ:** el orquestador finaliza el subproceso **“Cumplimiento”**, actualiza el estado de todo el Folio a **“Cancelado/Desistido”**, y detiene/cancela de forma inmediata cualquier SLA o actividad que estuviese pendiente en el sistema para dicho caso.

---

## Modelado de Datos

| Campo | Tipo de Dato | Editable (Sí/No) | Obligatorio (Sí/No) | Reglas de Negocio / Origen |
|---|---|---|---|---|
| `Bandera: Origen_Escalamiento` | Texto/Booleano | No | Sí | Oculto (Backend). Registra de dónde viene la tarea para habilitar el retorno correcto. |
| `Datos Heredados del Origen` | Varios | No | Sí | Bloque de datos precargados en modo solo lectura, dependientes del origen. |
| `¿Cliente Desiste del Caso?` | Lista Desplegable | Sí | Sí | Valores: “Sí”, “No”. Compuerta de decisión central de la actividad. |
| `Observaciones Comerciales` | Texto (Área) | Sí | No | Campo libre recomendado para justificar acuerdos con el cliente o razones del desistimiento. |

---

## Resumen del Flujo

```text
Actividad de origen
    |
    +--> Firmar Escritura Cliente
    |
    +--> Realizar Devolución EP
    |
    +--> Validar Condiciones Desembolso
            |
            v
   Realizar Gestión Comercial
            |
            v
   ¿Cliente desiste del caso?
        /             \
      Sí               No
      |                |
      v                v
 Confirmación      Leer bandera
      |             de origen
      v                |
 Fin Terminal           v
 Cancelado /       Retornar exactamente
 Desistido         a actividad de origen
```

---

## Consideraciones para Implementación en Kiro

- La pantalla debe soportar **herencia dinámica** de datos según la actividad de origen.
- Los datos heredados deben mostrarse en **solo lectura**.
- La variable `Origen_Escalamiento` debe mantenerse en backend y no ser editable por el usuario.
- La decisión **¿Cliente Desiste del Caso?** constituye la compuerta principal de salida.
- El retorno cuando la respuesta es **No** debe ser exactamente hacia la actividad que originó el escalamiento.
- El cierre por desistimiento debe:
  - Requerir confirmación.
  - Finalizar el subproceso `Cumplimiento`.
  - Cambiar el Folio a `Cancelado/Desistido`.
  - Cancelar SLAs y actividades pendientes.
- Toda transición debe quedar registrada en la bitácora.
