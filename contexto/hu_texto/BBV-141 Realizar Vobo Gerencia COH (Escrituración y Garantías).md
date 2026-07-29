# Descripción

**Yo como:** Gerente COH.

**Deseo:** Acceder a la actividad "Realizar Vobo Gerencia COH" para visualizar la información heredada del caso, emitir mi visto bueno confirmando el Vobo Gerencial y justificar mi decisión en caso de rechazo.

**Para:** Avanzar el flujo hacia la realización de la excepción del desembolso a cargo del Comercial y visualice mi respuesta.

# Alcance

Esta funcionalidad corresponde a la pantalla transaccional de formalización donde el Gerente COH interactúa. La interfaz consolida la información trabajada por el comercial en la actividad previa. Su núcleo funcional es capturar el dictamen del vobo gerencial mediante un campo de parametrización específica y aplicar el enrutamiento por cualquier decisión hacia el comercial.

# Criterios de Aceptación - Reglas de Negocio

## CA01 (Criterio Global Transversal)

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de las actividades anteriores (heredando la vista de Escrituración y Garantías).
2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura con los datos más recientes).
3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en **Expediente Digital** (para visualizar la minuta/escritura) y **Trazabilidad/Bitácora**.
4. **Funcionalidad de Botones de Acción:** Opciones de "Guardado" y "Transición / Avanzar".
5. **Trazabilidad:** Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Firmar Rep. Legal), Usuario Ejecutor (Representante Legal), Concepto de Firma y Observaciones.

## CA02 (Acordeón "Vobo Gerencia COH" y Datos Heredados)

El sistema debe desplegar un acordeón central nombrado "Vobo Gerencia COH".

En este bloque se mostrará, en estricto modo de **solo lectura**.

## CA03 (Campos Transaccionales de Firma)

- **Concepto:** Lista desplegable dictamen: *"Favorable"* y *"No Favorable"*.
- **Despliegue de Novedades:** Si el concepto seleccionado es *"No Favorable"*, el sistema debe convertir el campo observaciones como obligatorio:
  - **Observaciones:** Campo de texto libre.

## CA04 (Enrutamiento por Favorable/No Favorable)

Al accionar "Avanzar" con el concepto *"Favorable"* o *"No Favorable"*, el sistema realiza:

- Avanza a la actividad de “Realizar Excepción Desembolso” a cargo del Comercial.

# Modelado de Datos

| Campo | Tipo de Dato | Editable (Sí-No) | Obligatorio (Sí-No) | Reglas de Negocio / Origen |
|---|---|---|---|---|
| Datos Heredados Consolidados | Varios | No | Sí | Precargados de la actividad de Realizar Excepción Desembolso. |
| Concepto | Lista Desplegable | Sí | Sí | Valores: "Favorable", "NO Favorable". |
| Observaciones | Texto (Área) | Sí | Condicionado | **Obligatorio** si Concepto = "Escritura NO firmada" para detallar la novedad. |