# BBV-90 Realizar Devolución EP (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


07726, 15:28 {#8BV.90] HU - Actividad Realizar Devolución EP (Escrturación y Garantías)
Epica - Presto Escrituración y Garantías BBVA Legalización (ssv.12
4 [BBV-90] HU - Actividad Realizar Devolución EP (Escrituración y Garantías) creada: o2jun26 Actualizada: 19/Jun/26
Proyecto: BBVA - Colombia
Principal: Épica - Presto Escrituración y Garantías BBVA Legalización
Tipo Historia Prioridad. Media
Etiquetas: BBVA_LEGALIZACION
Tiempo Trabajado: Desconocido

## Descripción

**Yo como:** Analista de Vivienda.
**Para:** Solventar las inconsistencias documentales o de proceso reportadas y enrutar correctamente el flujo hacia Comercial, o reactivar la operación
hacia a frma de escritura, rma del representante legal o revisión final del abogado

## Alcance

Esta funcionalidad corresponde a la bandeja centaizadora de rechazos del subproceso de Escrturacion y Garantía. Acta como el nodo de
corrección donde el Analista de Vivenda revisa qué área devolvió la Escritura Pública (EP) y por qué. La pantalla mostrará en modo lectura los
el enrutamiento dinámico que le otorga al analista el control total para decidir hacia qué punto exacto del ciclo (Acción a Segui) debe regresar el
trámite una vez se subsanen los hallazgos.

## Criterios de aceptación y reglas de negocio

+ CAO1 (Criterio Global Transversal):
(heredando la vista de Escrituración y Garantías).
2. El sistema debe renderizar en la cabecera la "información General” estctamente de solo lectura)
3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en Expediente Digital, Trazabilidad/Bitácora y Carta de
Aprobación
6. Trazablidad: Al ejecular "Avanzar, se registrará en a bicora: Fecha, Actividad (Realizar Devolución EP), Usuario Ejecutor (Analista de
+ CA02 (Acordeón "Realizar Devolución EP” y Aseguramiento de Conceptos): El sistema debe desplegar un acordeón central nombrado
obigatoriamente "Realizar Devolución EP”. Este bloque funcionará como un visor de aultria en modo de solo lectura que consalkará y
mostrar
‘Abogado, VoBo Prorraa, Concepto Causacin)
hitpsliergeston tam atassian.neisiraisuevievsissue-ht/BBV.9/BV-90 im ve


<!-- --- PAGE 2 --- -->


877726, 15:23 [#88V-90] HU - Actividad Realizar Devolucién EP (Escrituración y Garantias)
+ CA03 (Compuerta de Escalamiento Comercial y Novedades): Dentro del acordeón transaccional, el sistema habilitará el campo obligatorio
"¿Requiere escalamiento comercial?" (Valores: SÍ / NO).
© Sise selecciona "SÍ": El sistema debe desplegar dinámicamente dos listas desplegables obligatorias denominadas Tipologías y
tarea al rol Comercial
+ CA04 (Ruta de Reanudación y Acción a Seguir
2. "Firmar Rep. Legal": Al avanzar, el uj so diigo hacia la actividad de "Firmar Rep. Legal"
3. "Realizar EP Rogistradas": Al avanzar, ol flujo transita hacia la actividad de "Realizar EP Rogistradas"
+ CAOS - Persistencia de Histórico de Devolución: Si el caso proviene de múltiples rechazos paralelos (ej. Abogado y Prorrata rechazaron al mismo
tiempo), la pantalla debe mostrar ambos dictámenes consolidados para que el Analista de Vivienda solucione todas las inconsistencias de una sola
+ CADS - Condicionalidad de Despliegue: El campo “Acción a Segui” y los campos "Tipologías/Casuísticas" son mutuamente excluyentes a nivel de

## Modelo de datos

Editable Obligatorio
Campo Tipo de Dato Reglas de Negocio / Origen
P m (Si/No) (Si/No) 3 9 9
Conceptos /Dictémenes neo Ss Heredado. Muestra el estado o rechazo emitido por áreas
Previos paralelas
Tipología y Casuística de Alfanumérico No sí Heredado. Muestra la razón exacta de la devolución.
Rechazo (Histórico)
Observaciones de Rech . ,
servaciones de Rechazo Texto No sí Heredado. Detalle dejado por el área revisora.
(Histórico)
¿Requiero ascalamiento List ,
¿Requiero escalamiento ‘sta El Si Valores: "SÍ", "NO", Compuerta principal de decisión,
comercial? Desplegable
Lista
Tipologías sí Condicionado Obligatorio si ¿Requiere escalamiento comercial? = SÍ,
Desplegable
Casuisticas Be econ 5 Condicionado — Obligatorio si ¿Requiere escalamiento comercial? = SÍ
Lista Obligatorio si ¿Requiere escalamiento comercial? = NO. Valores
Campo Kove para que sl Analista de Vivienda justnque las
Observaciones Texto (Área) Si No Pp para a Justa:
acciones tomadas para solventar la devolución.
Generado alas Mon Jul 06 20:22:56 UTC 2026 por Michael Pulido usando JIRA 1001.0.0-SNAPSHOT100292-
rev.c6te927afdded 1503 16460632049 14848e0265
hitp:ebergestion atar atlassian nesijiaissueviews:ssue-himl/BBV.90/BBV-20 html ze
